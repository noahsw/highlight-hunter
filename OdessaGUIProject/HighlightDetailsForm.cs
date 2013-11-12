using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Security.Permissions;
using System.Windows.Forms;
using NLog;
using OdessaGUIProject.DRM_Helpers;
using OdessaGUIProject.Properties;
using OdessaGUIProject.UI_Helpers;
using OdessaGUIProject.Players;
using OdessaGUIProject.Workers;
using GaDotNet.Common.Helpers;

namespace OdessaGUIProject
{
    public partial class HighlightDetailsForm : Form
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// How many seconds we'll zoom back per timer tick
        /// </summary>
        private const int ZoomSecondsIncrement = 5;

        private int _currentTickX;
        private int _endTickX;
        private int _startTickX;
        private readonly BorderlessWindow _borderlessWindow;

        private HighlightObject _currentHighlight;
        private int _currentHighlightIndex;
        private HourGlass _hourGlass;

        /// <summary>
        /// Whether use is holding mouse down, seeking to right position
        /// </summary>
        private bool _isMouseDownToSeek;

        private bool _isPlayHeadBeforeEndTickBox;

        /// <summary>
        /// The coordinates of the left bounds
        /// </summary>
        private int _leftCoordinateBounds;

        /// <summary>
        /// Bounds of when we start zooming
        /// </summary>
        private int _leftZoomOutCoordinates;

        /// <summary>
        /// The coordinates of the right bounds
        /// </summary>
        private int _rightCoordinateBounds;

        /// <summary>
        /// Bounds of when we start zooming
        /// </summary>
        private int _rightZoomOutCoordinates;

        /// <summary>
        /// The timeline distance that must be maintained between the start and end boxes
        /// </summary>
        private int _timelineWidthBufferBetweenStartAndEnd;

        private readonly VideoPlayerSeekHelper _videoPlayerSeekHelper = new VideoPlayerSeekHelper();
        /* Locations of the tickBoxes during mouse move */

        /// <summary>
        /// Whether we paused video to seek for the user
        /// Used to resume video after dragging trim bars
        /// </summary>
        private bool _wasPlayingBeforeSeek;

        /// <summary>
        /// Used when user releases marker, we'll resume where the playhead previously was.
        /// </summary>
        private double _positionBeforeSeeking;

        /// <summary>
        /// The position of the end of the timeline
        /// </summary>
        private double _zoomEndPosition;

        /// <summary>
        /// The position of the start of the timeline
        /// </summary>
        private double _zoomStartPosition;

        public HighlightDetailsForm()
        {
            InitializeComponent();

            //3. Ignore a windows erase message to reduce flicker. (http://social.microsoft.com/Forums/en-US/Offtopic/thread/fa0d5277-eb83-4e3e-a2d4-bb00270594a9/)
            // doesn't seem to help
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);

            DesignLanguage.ApplyCustomFont(Controls);

            _borderlessWindow = new BorderlessWindow(this, true, true);
            _borderlessWindow.SendNCWinMessage += SendNCWinMessage;
            MaximizedBounds = Screen.GetWorkingArea(this);

            titleTextBox.Text = "";
        }

        internal delegate void HighlightRemovedEventHandler(object sender, HighlightEventArgs e);

        internal event HighlightRemovedEventHandler HighlightRemoved;

        internal int InitialHighlightIndex { get; set; } // we use this so we can defer loading the player until the form is visible

        internal event EventHandler InitializeStartOverTutorialBubble;

        protected virtual void OnInitializeStartOverTutorialBubble()
        {
            EventHandler handler = InitializeStartOverTutorialBubble;
            if (handler != null) handler(this, EventArgs.Empty);
        }

        internal event EventHandler TutorialProgressUpdated;

        protected virtual void OnTutorialProgressUpdated()
        {
            EventHandler handler = TutorialProgressUpdated;
            if (handler != null) handler(this, EventArgs.Empty);
        }

        /// <summary>
        /// tracks whether current highlight has loaded yet. use to not flicker the playstate button
        /// </summary>
        //private bool isLoaded;
        internal int CurrentHighlightIndex
        {
            set
            {
                _hourGlass = new HourGlass();

                SuspendLayout();

                tickBoxLocationsTimer.Enabled = false; // this will be enabled once PlayState = Playing

                var previousHighlight = _currentHighlight;

                if (previousHighlight != null) // unsubscribe old events
                {
                    saveToDiskButton.UnsubscribeToEvents();
                    shareToFacebookButton.UnsubscribeToEvents();
                    previousHighlight.DurationChanged -= currentHighlight_DurationChanged;
                    previousHighlight.TitleChanged -= currentHighlight_TitleChanged;
                }

                _currentHighlightIndex = value;

                //isLoaded = false;

                _currentHighlight = MainModel.HighlightObjects[value];

                Logger.Info("Switching highlight to " + _currentHighlight.Title);

                if (videoPlayer.URL != _currentHighlight.InputFileObject.SourceFileInfo.FullName)
                    playHeadBox.Visible = false; // only hide if we're changing files so it doesn't jump around between highlights

                _currentHighlight.HasBeenReviewed = true;

                shareToFacebookButton.PublishWorkerType = PublishWorker.PublishWorkerTypes.Facebook;
                shareToFacebookButton.CurrentHighlight = _currentHighlight;

                saveToDiskButton.PublishWorkerType = PublishWorker.PublishWorkerTypes.Save;
                saveToDiskButton.CurrentHighlight = _currentHighlight;

                _currentHighlight.DurationChanged += currentHighlight_DurationChanged;
                _currentHighlight.TitleChanged += currentHighlight_TitleChanged;

                _zoomStartPosition = _currentHighlight.StartTime.TotalSeconds - 45; // before start time
                if (_zoomStartPosition < 0)
                    _zoomStartPosition = 0;

                _zoomEndPosition = _currentHighlight.EndTime.TotalSeconds + 15; // after end time
                if (_zoomEndPosition > _currentHighlight.InputFileObject.VideoDurationInSeconds)
                    _zoomEndPosition = _currentHighlight.InputFileObject.VideoDurationInSeconds;

                _timelineWidthBufferBetweenStartAndEnd = GetTimelineWidthFromSeconds(1);

#if DEBUG
                Logger.Info("StartTime: " + _currentHighlight.StartTime.TotalSeconds);
                Logger.Info("EndTime: " + _currentHighlight.EndTime.TotalSeconds);
                Logger.Info("BookmarkTime: " + _currentHighlight.BookmarkTime.TotalSeconds);
                Logger.Info("zoomStartPosition: " + _zoomStartPosition);
                Logger.Info("zoomEndPosition: " + _zoomEndPosition);
#endif

                UpdateTickBoxLocations(false);

                startTickBox.Visible = true;
                endTickBox.Visible = true;

                titleTextBox.Text = _currentHighlight.Title;

                // load the video last because it will block for a half second
                if (videoPlayer.URL == _currentHighlight.InputFileObject.SourceFileInfo.FullName)
                { // no need to reload the ioItem

                    var isAlreadyPlaying = videoPlayer.PlayState == GenericPlayerControl.PlayStates.Playing;

                    // don't use SeekToPosition because we know this is the initial seek request.
                    videoPlayer.CurrentPosition = _currentHighlight.StartTime.TotalSeconds;
                    // we're just seeking so kill the hour glass now
                    _hourGlass.Dispose();

                    if (isAlreadyPlaying)
                    {
                        tickBoxLocationsTimer.Enabled = true;
                        playHeadBox.Visible = true;
                    }
                }
                else
                {
                    videoPlayer.URL = _currentHighlight.InputFileObject.SourceFileInfo.FullName;

                    if (videoPlayer.PlayerType == GenericPlayerControl.PlayerTypes.WindowsMediaPlayer && 
                        _videoPlayerSeekHelper.mediaPlayer == null)
                    {
                        _videoPlayerSeekHelper.mediaPlayer = videoPlayer.WindowsMediaPlayer;
                        _videoPlayerSeekHelper.DoneWorking += videoPlayerSeekHelper_DoneWorking;
                    }

                    // don't use SeekToPosition because we know this is the initial seek request.
                    videoPlayer.CurrentPosition = _currentHighlight.StartTime.TotalSeconds; // this must come after .URL change

                    // leave cursor be a wait cursor until the video player starts playing
                }

                videoPlayer.Play();
                
                playStatePicture.Image = Resources.editpanel_playbackcontrols_pause;

                if (MainModel.isQuickTimeInstalled == false)
                    installQuickTimeLinkLabel.Visible = true;

                ResumeLayout();

                Invalidate(); //repaint the title bar so the form title updates
            }
            get
            {
                return _currentHighlightIndex;
            }
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            _borderlessWindow.Dispose();
            videoPlayer.Dispose();

            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void currentHighlight_DurationChanged(object sender, EventArgs e)
        {
            UpdateHighlightDurationLabel();
            UpdateHighlightTimeline();
        }

        private void currentHighlight_TitleChanged(object sender, EventArgs e)
        {
            titleTextBox.Text = _currentHighlight.Title;
        }

        #region Borderless Window

        protected override CreateParams CreateParams
        {
            [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)] // http://msdn.microsoft.com/library/ms182305(VS.100).aspx
            get
            {
// ReSharper disable InconsistentNaming
                const int WS_MINIMIZEBOX = 0x20000;
                const int CS_DROPSHADOW = 0x20000;
                const int CS_DBLCLKS = 0x8;
                // ReSharper restore InconsistentNaming

                CreateParams cParams = base.CreateParams;

                int classFlags = CS_DBLCLKS;
// ReSharper disable InconsistentNaming
                int OSVER = Environment.OSVersion.Version.Major * 10;
// ReSharper restore InconsistentNaming
                OSVER += Environment.OSVersion.Version.Minor;
                if (OSVER >= 51) classFlags = CS_DROPSHADOW | CS_DBLCLKS;

                cParams.ClassStyle = classFlags;
                cParams.Style |= WS_MINIMIZEBOX;
                return cParams;
            }
        }

        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        protected override void WndProc(ref Message m)
        {
            if (WindowState == FormWindowState.Maximized)
            {
                if (m.Msg == USER32.WM_SYSCOMMAND &&
                    m.WParam.ToInt32() == (int)USER32.SysCommand.SC_MOVE ||
                    m.Msg == (int)USER32.NCMouseMessage.WM_NCLBUTTONDOWN &&
                    m.WParam.ToInt32() == (int)USER32.NCHitTestResult.HTCAPTION)
                {
                    m.Msg = USER32.WM_NULL;
                }
            }

            base.WndProc(ref m);

            //Logger.Trace("Message from MainForm: " + m.Msg);

            switch (m.Msg)
            {
                case USER32.WM_GETSYSMENU:
                    _borderlessWindow.SystemMenu.Show(this, PointToClient(new Point(m.LParam.ToInt32())));
                    break;

                case USER32.WM_NCACTIVATE:
                    _borderlessWindow.IsFormActive = m.WParam.ToInt32() != 0;
                    Invalidate();
                    break;

                case USER32.WM_NCHITTEST:
                    m.Result = _borderlessWindow.OnNonClientHitTest(m.LParam);
                    break;

                case (int)USER32.NCMouseMessage.WM_NCLBUTTONUP:
                    _borderlessWindow.OnNonClientLButtonUp(m.LParam);
                    break;

                case (int)USER32.NCMouseMessage.WM_NCRBUTTONUP:
                    _borderlessWindow.OnNonClientRButtonUp(m.LParam);
                    break;

                case (int)USER32.NCMouseMessage.WM_NCMOUSEMOVE:
                    _borderlessWindow.OnNonClientMouseMove(m.LParam);
                    break;
            }
        }

        private void SendNCWinMessage(int msg, IntPtr wParam, IntPtr lParam)
        {
            Message message = Message.Create(Handle, msg, wParam, lParam);
            WndProc(ref message);
        }

        #endregion Borderless Window

        private void DisplayNextHighlight()
        {
            CurrentHighlightIndex = (_currentHighlightIndex + 1) % MainModel.HighlightObjects.Count;
        }

        private void DisplayPreviousHighlight()
        {
            var newIndex = _currentHighlightIndex - 1;
            if (newIndex < 0)
                newIndex = MainModel.HighlightObjects.Count - 1; // go to last one
            CurrentHighlightIndex = newIndex;
        }

        private void DonateClip(bool isMarkIntentional)
        {
            var activationState = Protection.GetLicenseStatus();
            if (activationState == Protection.ActivationState.Unlicensed || activationState == Protection.ActivationState.TrialExpired)
            {
                MessageBox.Show("Activate first so you don't get watermarks!");
                return;
            }

            const int surroundingSeconds = 5;

            _currentHighlight.StartTime = _currentHighlight.BookmarkTime.Subtract(TimeSpan.FromSeconds(surroundingSeconds));
            _currentHighlight.EndTime = _currentHighlight.BookmarkTime.Add(TimeSpan.FromSeconds(surroundingSeconds));
            _currentHighlight.SaveWorker = new SaveWorker(_currentHighlight);
            _currentHighlight.SaveWorker.OutputFormat = SaveWorker.OutputFormats.Original;

            //var execFolder = Path.GetDirectoryName(Application.ExecutablePath);
            var filename = _currentHighlight.Title + " - " + _currentHighlight.FriendlyStartTime.Replace(":", ".") + " to " + _currentHighlight.FriendlyEndTime.Replace(":", ".") + _currentHighlight.InputFileObject.SourceFileInfo.Extension;

            var outputFileName = Path.Combine(@"D:\Projects\Odessa\Test Videos\trunk", filename);

            _currentHighlight.SaveWorker.RunWorkerAsync(outputFileName);

            saveToDiskButton.CurrentHighlight = _currentHighlight; // so it shows status

            if (isMarkIntentional)
            {
                File.WriteAllText(outputFileName + ".txt", "0:00:0" + surroundingSeconds);
            }

            UpdateTickBoxLocations();
        }

        private void donateFalsePositiveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DonateClip(false);
        }

        private void donateIntentionalMarkToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DonateClip(true);
        }

        private void donateMissedMarkToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // move End bar to missed bookmark
            _currentHighlight.BookmarkTime = _currentHighlight.EndTime;
            DonateClip(true);
        }

        private void trimTickBox_MouseDown(object sender, MouseEventArgs e)
        {

            if (e.Button == MouseButtons.Left)
            {

                if (sender == startTickBox)
                    _startTickX = e.X;
                else if (sender == endTickBox)
                    _endTickX = e.X;
                
                _isMouseDownToSeek = true;
                
                _wasPlayingBeforeSeek = (videoPlayer.PlayState == GenericPlayerControl.PlayStates.Playing);
                
                _positionBeforeSeeking = GetPositionOfPlayHeadTickBox();

                playHeadBox.Visible = false;

                tickBoxLocationsTimer.Enabled = false;

                videoPlayer.Pause(); // QuickTime fires Play and Pause events repeatedly when seeking
            }
        }

        private void endTickBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;

            int newCoordinate = endTickBox.Left + (e.X - _endTickX);
            if (newCoordinate > _rightZoomOutCoordinates && // on edge. zoom out
                (e.X - _endTickX) > 0 && // moving mouse in zoom out direction
                _currentHighlight.InputFileObject.VideoDurationInSeconds - _zoomEndPosition >= ZoomSecondsIncrement) // enough room to zoom out
            {
                Logger.Info("Zooming out started");
                zoomOutEndTimer.Enabled = true;
                return;
            }

            zoomOutEndTimer.Enabled = false;

            if (newCoordinate > _rightCoordinateBounds)
                return; // out of bounds
            if (newCoordinate < startTickBox.Left + startTickBox.Width + _timelineWidthBufferBetweenStartAndEnd)
                return; // out of bounds

            //SuspendLayout();

            endTickBox.Left = newCoordinate;

            double position = GetPositionOfEndTickBox();
            _currentHighlight.EndTime = TimeSpan.FromSeconds(position);

            SeekToPosition(position);

            //ResumeLayout();

            PaintRefresh();
        }

        private void trimTickBox_MouseUp(object sender, MouseEventArgs e)
        {

            _isMouseDownToSeek = false;

            if (sender == startTickBox)
            {
                zoomOutStartTimer.Enabled = false;
                SeekToPosition(GetPositionOfStartTickBox());
            }
            else if (sender == endTickBox)
            {
                zoomOutEndTimer.Enabled = false;
                SeekToPosition(_positionBeforeSeeking);
            }

            if (handlesTutorialBubble.Visible)
                handlesTutorialBubble_Advance(null, EventArgs.Empty);

            ResumePlayStateBeforeSeeking();

        }

        private double GetPositionFromTickBoxLocation(int controlMeasurementPoint)
        {
            double percentage = (double)(controlMeasurementPoint - timelineBox.Left) / timelineBox.Width;

            double position = percentage * (_zoomEndPosition - _zoomStartPosition) + _zoomStartPosition;

            return position;

            //return (tickBox.Left - timelineBox.Left + (tickBox.Width / 2)) * wmPlayer.Ctlcontrols.currentItem.duration / timelineBox.Width;
        }

        private double GetPositionOfEndTickBox()
        {
            return GetPositionFromTickBoxLocation(endTickBox.Left);
        }

        private double GetPositionOfPlayHeadTickBox()
        {
            return GetPositionFromTickBoxLocation(playHeadBox.Left + (playHeadBox.Width / 2));
        }

        private double GetPositionOfStartTickBox()
        {
            return GetPositionFromTickBoxLocation(startTickBox.Right);
        }

        /// <summary>
        /// Returns the pixel width of the timeline that represents @seconds in the current zoom
        /// </summary>
        /// <param name="seconds"></param>
        /// <returns></returns>
        private int GetTimelineWidthFromSeconds(double seconds)
        {
            var width = (int)(timelineBox.Width * (seconds / (_zoomEndPosition - _zoomStartPosition))) + 1; // +1 because int conversion will round down
            return width;
        }

        private void HighlightDetailsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            
            using (_hourGlass = new HourGlass())
            {
                tickBoxLocationsTimer.Enabled = false;

                _videoPlayerSeekHelper.Cancel();

                Settings.Default.Save(); // save sizing settings

                if (closeDetailsTutorialBubble.Visible)
                    closeDetailsTutorialBubble_Advance(null, EventArgs.Empty);
            }
        }

        private void HighlightDetailsForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (titleTextBox.ContainsFocus)
                return;

            if (e.KeyChar == (char)Keys.Left)
                DisplayPreviousHighlight();

            if (e.KeyChar == (char)Keys.Right)
                DisplayNextHighlight();
        }

        private void HighlightDetailsForm_Load(object sender, EventArgs e)
        {
            UpdateTimelineBounds();

            #region Load sizing settings

            // default WindowState is Normal
            if (Settings.Default.HighlightDetailsWindowState == FormWindowState.Maximized)
                WindowState = FormWindowState.Maximized;
            if (WindowState == FormWindowState.Normal && !Settings.Default.HighlightDetailsSize.IsEmpty)
            {
                Logger.Debug("Resizing to " + Settings.Default.HighlightDetailsSize);
                Size = Settings.Default.HighlightDetailsSize;
                CenterToScreen();
            }

            #endregion Load sizing settings
        }

        private void HighlightDetailsForm_Paint(object sender, PaintEventArgs e)
        {
            _borderlessWindow.PaintForm(e, "Highlight Details |", (_currentHighlightIndex + 1) + " of " + MainModel.HighlightObjects.Count);
        }

        private void HighlightDetailsForm_Resize(object sender, EventArgs e)
        {
            //Logger.Trace(this.Height - startTickBox.Bottom);
            //Logger.Trace(this.Height - endTickBox.Bottom);

            var equal = startTickBox.Top == endTickBox.Top;
            if (!equal)
            {
                Logger.Error("Not equal!");
            }

            if (Created)
            {
                UpdateTimelineBounds();
                UpdateTickBoxLocations();

                _borderlessWindow.BuildPaths();
                Invalidate();

                Settings.Default.HighlightDetailsWindowState = WindowState;
                if (WindowState == FormWindowState.Normal)
                    Settings.Default.HighlightDetailsSize = Size;
            }
        }

        private void HighlightDetailsForm_Shown(object sender, EventArgs e)
        {
            // it doesn't dispose sometimes which is super annoying
            //hourGlass = new HourGlass();

            if (Settings.Default.HasOpenedDetails == false)
            {
                AnalyticsHelper.FireEvent("First details");
                Settings.Default.HasOpenedDetails = true;
                Settings.Default.Save();
            }

            if (MainModel.HighlightObjects.Count == 1)
            {
                nextHighlightPictureButtonControl.Enabled = false;
                previousHighlightPictureButtonControl.Enabled = false;
            }

            CurrentHighlightIndex = InitialHighlightIndex;

            InitializeBookmarkFlagTutorialBubble();
            InitializeHandlesTutorialBubble();
            InitializeShareButtonTutorialBubble();
            InitializeCloseDetailsTutorialBubble();
        }

        private void MoveBookmarkToPosition(double position)
        {
            double percentage = ((position - _zoomStartPosition) / (_zoomEndPosition - _zoomStartPosition));
            bookmarkLocationPictureBox.Left = timelineBox.Left + (int)(percentage * timelineBox.Width) - (bookmarkLocationPictureBox.Width / 2);
        }

        private void MoveEndTickBoxToPosition(double position)
        {
            double percentage = ((position - _zoomStartPosition) / (_zoomEndPosition - _zoomStartPosition));
            endTickBox.Left = timelineBox.Left + (int)(percentage * timelineBox.Width);
        }

        private void MovePlayHeadToPosition(double position)
        {
// ReSharper disable CompareOfFloatsByEqualityOperator
            Debug.Assert(_currentHighlight != null && (position > 0 || _currentHighlight.StartTime.TotalSeconds == 0), "Position shouldn't be 0!");
// ReSharper restore CompareOfFloatsByEqualityOperator

            double percentage = ((position - _zoomStartPosition) / (_zoomEndPosition - _zoomStartPosition));
            playHeadBox.Left = timelineBox.Left + (int)(percentage * timelineBox.Width) - (playHeadBox.Width / 2);
        }

        private void MoveStartTickBoxToPosition(double position)
        {
            double percentage = ((position - _zoomStartPosition) / (_zoomEndPosition - _zoomStartPosition));
            startTickBox.Left = timelineBox.Left + (int)(percentage * timelineBox.Width) - startTickBox.Width;
        }

        private void nextHighlightPictureBox_Click(object sender, EventArgs e)
        {
            DisplayNextHighlight();
        }

        private void playHeadBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                _currentTickX = e.X;
                _isMouseDownToSeek = true;
                _wasPlayingBeforeSeek = (videoPlayer.PlayState == GenericPlayerControl.PlayStates.Playing);
            }
        }

        private void playHeadBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                int newPosition = playHeadBox.Left + (e.X - _currentTickX);
                if (newPosition < _leftCoordinateBounds)
                    return; // out of bounds
                if (newPosition > _rightCoordinateBounds)
                    return; // out of bounds

                playHeadBox.Left = newPosition;

                double newSeconds = GetPositionOfPlayHeadTickBox();

                //Logger.Trace("Calling SeekPosition");
                SeekToPosition(newSeconds);
            }
        }

        private void playHeadBox_MouseUp(object sender, MouseEventArgs e)
        {
            _isMouseDownToSeek = false;
            ResumePlayStateBeforeSeeking();
        }

        private void playStatePicture_Click(object sender, EventArgs e)
        {
            Logger.Debug("playStatePicture clicked");

            if (videoPlayer.PlayState == GenericPlayerControl.PlayStates.Transitioning) // if we're transition, it takes a while to stop
                _hourGlass = new HourGlass();

            if (videoPlayer.PlayState == GenericPlayerControl.PlayStates.Playing || videoPlayer.PlayState == GenericPlayerControl.PlayStates.Transitioning)
            {
                videoPlayer.Pause();
            }
            else if (videoPlayer.PlayState == GenericPlayerControl.PlayStates.Stopped)
            { // we hit the end of the clip so let's start at the beginning before playing
                // we do this seek request manually so the playhead immediately jumps to 0
                _hourGlass.Dispose();

                SeekToPosition(_currentHighlight.StartTime.TotalSeconds);
                tickBoxLocationsTimer.Enabled = true;
                videoPlayer.Play();
            }
            else
            {
                videoPlayer.Play();
            }
        }

        private void previousHighlightPictureBox_Click(object sender, EventArgs e)
        {
            DisplayPreviousHighlight();
        }

        private void removeHighlightPictureButtonControl_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to remove this highlight?" + Environment.NewLine + Environment.NewLine +
                "You can get it back by rescanning your videos.", "Remove highlight?", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                == DialogResult.No)
                return;


            if ((_currentHighlight.SaveWorker != null && _currentHighlight.SaveWorker.PublishWorkerResult == PublishWorker.PublishWorkerResults.NotFinished) ||
                 (_currentHighlight.FacebookShareWorker != null && _currentHighlight.FacebookShareWorker.PublishWorkerResult == PublishWorker.PublishWorkerResults.NotFinished))
            {
                if (MessageBox.Show("This highlight is currently being published." + Environment.NewLine + Environment.NewLine +
                    "Would you like to cancel publishing and remove the highlight?", "Cancel publishing?", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) 
                    == DialogResult.Yes)
                {
                    using (_hourGlass = new HourGlass())
                    {
                        if (_currentHighlight.SaveWorker != null)
                        {
                            _currentHighlight.SaveWorker.CancelAsync();
                            while (_currentHighlight.SaveWorker.IsBusy) { Application.DoEvents(); }
                        }

                        if (_currentHighlight.FacebookShareWorker != null)
                        {
                            _currentHighlight.FacebookShareWorker.CancelAsync();
                            while (_currentHighlight.FacebookShareWorker.IsBusy) { Application.DoEvents(); }
                        }
                    }
                }
                else
                    return;
            }

            HighlightRemoved(sender, new HighlightEventArgs(_currentHighlight));

            if (MainModel.HighlightObjects.Count == 1)
            {
                nextHighlightPictureButtonControl.Enabled = false;
                previousHighlightPictureButtonControl.Enabled = false;
            }

            // move on to next one
            var newIndex = _currentHighlightIndex;
            if (newIndex == MainModel.HighlightObjects.Count) // we deleted the last one
                newIndex = 0; // move to front
            if (MainModel.HighlightObjects.Count == 0) // no highlights left
            {
                Close();
            }
            else
            {
                CurrentHighlightIndex = newIndex;
            }
        }

        private void ResumePlayStateBeforeSeeking()
        {
            videoPlayer.Mute = false;
            if (_wasPlayingBeforeSeek)
            {
                Logger.Debug("Resuming playback");
                videoPlayer.Play();
            }
            else
            {
                Logger.Debug("Keeping playback paused");
                videoPlayer.Pause();
            }
        }

        private void rightPanel_MouseClick(object sender, MouseEventArgs e)
        {
#if DEBUG
            if (e.Button == MouseButtons.Right)
                donateContextMenuStrip.Show(MousePosition, ToolStripDropDownDirection.BelowRight);
#endif
        }

        private void SeekToPosition(double position)
        {
            Logger.Debug("Seeking to " + position);

            if (_wasPlayingBeforeSeek) //  videoPlayer.PlayState == WMPPlayState.wmppsPaused  && !isPausedByUser
            {
                playStatePicture.Image = Resources.editpanel_playbackcontrols_pause; // it's going to start playing
                //Logger.Debug("Anticipating playback so setting to pause image");
            }

            if (videoPlayer.PlayerType == GenericPlayerControl.PlayerTypes.WindowsMediaPlayer)
            {
                tickBoxLocationsTimer.Enabled = false;

                _videoPlayerSeekHelper.SeekRequest(position);
            }
            else if (videoPlayer.PlayerType == GenericPlayerControl.PlayerTypes.QuickTime)
            {
                videoPlayer.CurrentPosition = position;

                videoPlayerSeekHelper_DoneWorking(null, EventArgs.Empty);
            }
        }

        private void startTickBox_DoubleClick(object sender, EventArgs e)
        {
            playHeadBox.Left = startTickBox.Right - (playHeadBox.Width / 2);
            SeekToPosition(GetPositionOfStartTickBox());
        }


        private void startTickBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;

            int newCoordinate = startTickBox.Left + (e.X - _startTickX);
            if (newCoordinate < _leftZoomOutCoordinates && // on edge. zoom out
                (e.X - _startTickX) < 0 && // moving mouse in zoom out direction
                _zoomStartPosition >= ZoomSecondsIncrement) // enough room to zoom out
            {
                Logger.Info("Zooming out started");
                zoomOutStartTimer.Enabled = true;
                return;
            }
            zoomOutStartTimer.Enabled = false;


            if (newCoordinate < _leftCoordinateBounds)
                return; // out of bounds
            if (newCoordinate > endTickBox.Left - startTickBox.Width - _timelineWidthBufferBetweenStartAndEnd)
                return; // out of bounds.  can't go past endTickBox

            SuspendLayout();

            startTickBox.Left = newCoordinate;

            double position = GetPositionOfStartTickBox();
            _currentHighlight.StartTime = TimeSpan.FromSeconds(position);

            SeekToPosition(position);

            ResumeLayout();

            PaintRefresh();
        }

        private void tickBoxLocationsTimer_Tick(object sender, EventArgs e)
        {
            MovePlayHeadToPosition(videoPlayer.CurrentPosition);

            // this code pauses the player when we hit the end tickbox
            if (videoPlayer.CurrentPosition < _currentHighlight.EndTime.TotalSeconds)
                _isPlayHeadBeforeEndTickBox = true;
            else if (_isPlayHeadBeforeEndTickBox)
            { // play head just hit the tickbox
                Logger.Debug("Playhead just hit endBox");
                videoPlayer.Pause();
                _isPlayHeadBeforeEndTickBox = false;
            }

            if (playHeadBox.Right > _rightCoordinateBounds)
            { // this might not necessarily be the end of the clip. it could be the end of the zoomed region.
                videoPlayer.Pause();
                SeekToPosition(_currentHighlight.StartTime.TotalSeconds);
            }
        }

        private void timeline_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;

            tickBoxLocationsTimer.Enabled = false; // so we don't update playhead tickbox while we seek

            _isPlayHeadBeforeEndTickBox = false; // so we don't pause by manually jumping over endbox

            var control = sender as Control;

            Debug.Assert(control != null, "control != null");
            int newCoordinate = control.Left + e.X;

            playHeadBox.Left = newCoordinate - (playHeadBox.Width / 2);
            playHeadBox.Refresh();

            double newSeconds = GetPositionOfPlayHeadTickBox();

            _isMouseDownToSeek = true;
            _wasPlayingBeforeSeek = (videoPlayer.PlayState == GenericPlayerControl.PlayStates.Playing);

            videoPlayer.Pause();

            SeekToPosition(newSeconds);
        }


        private void timeline_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                var control = sender as Control;

                Debug.Assert(control != null, "control != null");
                var newCoordinate = control.Left + (e.X - _currentTickX);
                if (newCoordinate < _leftCoordinateBounds)
                    return; // out of bounds
                if (newCoordinate > _rightCoordinateBounds)
                    return; // out of bounds

                playHeadBox.Left = newCoordinate - (playHeadBox.Width / 2);
                playHeadBox.Refresh();

                var newPosition = GetPositionOfPlayHeadTickBox();

                SeekToPosition(newPosition);
            }
        }

        private void timeline_MouseUp(object sender, MouseEventArgs e)
        {

            _isMouseDownToSeek = false;

            if (videoPlayer.PlayerType == GenericPlayerControl.PlayerTypes.QuickTime)
            {
                ResumePlayStateBeforeSeeking();

                tickBoxLocationsTimer.Enabled = true;
            }
            else if (videoPlayer.PlayerType == GenericPlayerControl.PlayerTypes.WindowsMediaPlayer)
            {
                // since WMP needs time to seek, we should do one last final seek at _MouseUp so videoPlayerSeeker_DoneWorking
                // can show the playhead in the right position
                // without this, the playhead jumps back and forth between original position and new position

                var control = sender as Control;

                Debug.Assert(control != null, "control != null");
                var newCoordinate = control.Left + (e.X - _currentTickX);

                playHeadBox.Left = newCoordinate - (playHeadBox.Width / 2);
                playHeadBox.Refresh();

                var newPosition = GetPositionOfPlayHeadTickBox();

                SeekToPosition(newPosition);
            }

        }

        private void titleTextBox_Leave(object sender, EventArgs e)
        {
            _currentHighlight.Title = titleTextBox.Text;
        }

        private void titleTextBox_TextChanged(object sender, EventArgs e)
        {
            if (Created)
                _currentHighlight.Title = titleTextBox.Text;
        }

        private void UpdateHighlightDurationLabel()
        {
            highlightDurationLabel.Text = _currentHighlight.FriendlyDuration; // Math.Round((currentHighlight.EndTime - currentHighlight.StartTime).TotalSeconds, 0).ToString() + " secs";

            Debug.Assert(highlightDurationLabel.Text != "0:00", "Invalid duration");
            //highlightDurationLabel.Left = startTickBox.Right;
            //highlightDurationLabel.Width = endTickBox.Left - highlightDurationLabel.Left;
        }

        private void UpdateHighlightTimeline()
        {
            highlightTimelinePictureBox.Left = startTickBox.Right;
            highlightTimelinePictureBox.Width = endTickBox.Left - highlightTimelinePictureBox.Left;
        }

        private void UpdatePlayStatePicture()
        {
            Logger.Debug("called with isSeekerWorking=" + _videoPlayerSeekHelper.IsWorking + " and playState=" + videoPlayer.PlayState);

            if (_videoPlayerSeekHelper.IsWorking)
                return; // we're probably seeking to update the frame

            if (videoPlayer.PlayState == GenericPlayerControl.PlayStates.Playing || videoPlayer.PlayState == GenericPlayerControl.PlayStates.Transitioning)
            {
                playStatePicture.Image = Resources.editpanel_playbackcontrols_pause;
            }
            else
            {
                playStatePicture.Image = Resources.editpanel_playbackcontrols_play;
            }
        }

        private void UpdateTickBoxLocations(bool includePlayHead = true)
        {
            if (_currentHighlight != null)
            {
                MoveStartTickBoxToPosition(_currentHighlight.StartTime.TotalSeconds);
                MoveEndTickBoxToPosition(_currentHighlight.EndTime.TotalSeconds);
                MoveBookmarkToPosition(_currentHighlight.BookmarkTime.TotalSeconds);
                UpdateHighlightTimeline();
                UpdateTutorialBubbleCoordinates();
                UpdateHighlightDurationLabel();
            }

            if (includePlayHead && 
                videoPlayer != null && 
                videoPlayer.PlayState != GenericPlayerControl.PlayStates.NoVideo && 
                videoPlayer.CurrentPosition > 0)
                MovePlayHeadToPosition(videoPlayer.CurrentPosition);
        }

        private void UpdateTutorialBubbleCoordinates()
        {
            if (bookmarkFlagTutorialBubble.Visible)
            {
                var x = bookmarkLocationPictureBox.Left + GetOriginXForCenterPoint(bookmarkLocationPictureBox, bookmarkFlagTutorialBubble);
                if (x < 0)
                    x = 0;
                bookmarkFlagTutorialBubble.Left = x;
            }

            if (handlesTutorialBubble.Visible)
            {
                var x = startTickBox.Left + GetOriginXForCenterPoint(startTickBox, handlesTutorialBubble);
                x += 70; // account for BottomLeft arrow
                if (x < 0)
                    x = 0;
                handlesTutorialBubble.Left = x;
            }
        }

        private static int GetOriginXForCenterPoint(Control anchorControl, Control slaveControl)
        {
            if (anchorControl == null)
                return 0;

            if (slaveControl == null)
                return 0;

            return (anchorControl.Width - slaveControl.Width) / 2;
        }

        
        private void UpdateTimelineBounds()
        {
            _leftCoordinateBounds = timelineBox.Left; // -(playHeadBox.Width / 2);
            _rightCoordinateBounds = timelineBox.Right; // - (endTickBox.Width / 2);

            // let's not make this proportionate to how big the window is anymore. this could be confusing to users because the zoom out region will be indeterminate
            _leftZoomOutCoordinates = _leftCoordinateBounds + 20; //(int)(leftCoordinateBounds + 0.10 * (rightCoordinateBounds - leftCoordinateBounds)); // 10% from the left, we start zoom out
            _rightZoomOutCoordinates = _rightCoordinateBounds - 20; // (int)(rightCoordinateBounds - 0.05 * (rightCoordinateBounds - leftCoordinateBounds)); // 5% from right, we start zoom out
        }

        private void videoPlayer_InitializationError(object sender, EventArgs e)
        {
            if (MessageBox.Show("We had trouble loading the video player. Please make sure you have Windows Media Player or QuickTime installed." + Environment.NewLine + Environment.NewLine +
                "Without it, you won't be able to preview your highlights before sharing." + Environment.NewLine + Environment.NewLine +
                "Would you like help on how to install a video player?", "Video player required", MessageBoxButtons.YesNo, MessageBoxIcon.Error)
                == DialogResult.Yes)
            {
                BrowserHelper.LaunchBrowser("http://support.highlighthunter.com/customer/portal/articles/658912-how-to-install-windows-media-player", "installplayer");
            }
        }



        private void videoPlayer_PlayerError(object sender, PlayerErrorEventArgs e)
        {
            _hourGlass.Dispose();

            if (MainModel.isQuickTimeInstalled == false)
                installQuickTimeLinkLabel.Visible = true;

            if (e.ErrorMessage.Length == 0)
                return;

            if (MessageBox.Show(e.ErrorMessage, "Error playing video", MessageBoxButtons.YesNo, MessageBoxIcon.Error)
                == DialogResult.Yes)
            {
                SupportHelper.OpenKBArticle(e.SupportID);
            }
        }

        private void videoPlayer_OpenStateChanged(object sender, OpenStateChangedEventArgs e)
        {
            if (e.OpenState == GenericPlayerControl.OpenStates.Open)
            {
                playHeadBox.Visible = true;
                
                // do this here because we know media has loaded
                UpdateTickBoxLocations();
            }
        }

        private void videoPlayer_PlayStateChanged(object sender, PlayStateChangedEventArgs e)
        {
            UpdatePlayStatePicture();

            if (e.PlayState == GenericPlayerControl.PlayStates.Playing)
            {
                //isLoaded = true;
                tickBoxLocationsTimer.Enabled = true;
                _hourGlass.Dispose();

                if (!_isMouseDownToSeek)
                {
                    tickBoxLocationsTimer_Tick(sender, EventArgs.Empty);
                    playHeadBox.Visible = true;
                }
            }
            else
            {
                tickBoxLocationsTimer.Enabled = false;

                if (e.PlayState == GenericPlayerControl.PlayStates.Paused)
                    _hourGlass.Dispose();

                if (e.PlayState == GenericPlayerControl.PlayStates.Ended)
                { // end of clip. let's move playhead to start of highlight
                    Logger.Info("Hit the end of clip!");
                }
            }
        }

        private void videoPlayerSeekHelper_DoneWorking(object sender, EventArgs e)
        {
            Logger.Debug("DoneWorking");

            // at this point the player is set to play, but it's muted (that's how it seeks)
            if (_isMouseDownToSeek)
            { // don't let play because user hasn't released mouse yet
                videoPlayer.Pause();

                if (videoPlayer.PlayerType == GenericPlayerControl.PlayerTypes.WindowsMediaPlayer)
                    videoPlayer.Mute = true;
            }
            else
            {
                ResumePlayStateBeforeSeeking();

                // only update tick mark locations when done working
                tickBoxLocationsTimer.Enabled = true;

                tickBoxLocationsTimer_Tick(sender, EventArgs.Empty); // update location of playHeadBox

                playHeadBox.Visible = true;

            }

            /* REMED this out because it was making the playhead jump. I think it's because the playhead would update it's position and cause a mousemove
            if (videoPlayer.PlayState == WMPPlayState.wmppsPlaying)
            { // only do this if we're playing so the playhead doesn't jump after a paused seek
            }
            */
        }

        private void zoomOutEndTimer_Tick(object sender, EventArgs e)
        {
            var newZoomEndPosition = _zoomEndPosition + ZoomSecondsIncrement;
            if (newZoomEndPosition > _currentHighlight.InputFileObject.VideoDurationInSeconds)
                newZoomEndPosition = _currentHighlight.InputFileObject.VideoDurationInSeconds;
            _zoomEndPosition = newZoomEndPosition;

            var newEndTime = _currentHighlight.EndTime.Add(TimeSpan.FromSeconds(ZoomSecondsIncrement));
            if (newEndTime.TotalSeconds > _currentHighlight.InputFileObject.VideoDurationInSeconds)
                newEndTime = TimeSpan.FromSeconds(_currentHighlight.InputFileObject.VideoDurationInSeconds);
            _currentHighlight.EndTime = newEndTime;

            Logger.Debug("Updated zoomEndPosition to " + _zoomEndPosition);

            _timelineWidthBufferBetweenStartAndEnd = GetTimelineWidthFromSeconds(1);

            MoveStartTickBoxToPosition(_currentHighlight.StartTime.TotalSeconds);
            MoveEndTickBoxToPosition(_currentHighlight.EndTime.TotalSeconds);
            MoveBookmarkToPosition(_currentHighlight.BookmarkTime.TotalSeconds);
            MovePlayHeadToPosition(videoPlayer.CurrentPosition);
            UpdateHighlightTimeline();

            /* old code. not sure why it's different from zoomOutStartTimer
            endTickBox.Left = rightZoomOutCoordinates;

            var position = GetPositionOfEndTickBox();
            currentHighlight.EndTime = TimeSpan.FromSeconds(position);
             */ 

            if (videoPlayer.CurrentPosition > _currentHighlight.EndTime.TotalSeconds)
            { // user is trimming down the highlight from the end
                videoPlayer.Pause();
                SeekToPosition(_currentHighlight.EndTime.TotalSeconds);
            }
        }

        private void zoomOutStartTimer_Tick(object sender, EventArgs e)
        {
            var newZoomStartPosition = _zoomStartPosition - ZoomSecondsIncrement;
            if (newZoomStartPosition < 0)
                newZoomStartPosition = 0;
            _zoomStartPosition = newZoomStartPosition;

            var newStartTime = _currentHighlight.StartTime.Subtract(TimeSpan.FromSeconds(ZoomSecondsIncrement));
            if (newStartTime.TotalSeconds < 0)
                newStartTime = TimeSpan.FromSeconds(0);
            _currentHighlight.StartTime = newStartTime;

            Logger.Debug("Updated zoomStartPosition to " + _zoomStartPosition);

            _timelineWidthBufferBetweenStartAndEnd = GetTimelineWidthFromSeconds(1);

            MoveStartTickBoxToPosition(_currentHighlight.StartTime.TotalSeconds);
            MoveEndTickBoxToPosition(_currentHighlight.EndTime.TotalSeconds);
            MoveBookmarkToPosition(_currentHighlight.BookmarkTime.TotalSeconds);
            MovePlayHeadToPosition(videoPlayer.CurrentPosition);
            UpdateHighlightTimeline();

            /*
            startTickBox.Left = leftZoomOutCoordinates;

            double position = GetPositionOfStartTickBox();
            currentHighlight.StartTime = TimeSpan.FromSeconds(position);
             */

            videoPlayer.Pause();
            Logger.Trace("Calling SeekPosition");
            SeekToPosition(_currentHighlight.StartTime.TotalSeconds);
        }

        private void installQuickTimeLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            BrowserHelper.LaunchBrowser("http://support.highlighthunter.com/customer/portal/articles/658912-how-to-install-a-video-player", "recommendQuickTime");
        }

        private void PaintRefresh()
        {
            timelineBox.Update();
            highlightTimelinePictureBox.Update();
            startTickBox.Update();
            endTickBox.Update();
            Application.DoEvents();
        }

        private void InitializeBookmarkFlagTutorialBubble()
        {
            var progress = TutorialHelper.GetTutorialProgress();
            if (progress != TutorialProgress.TutorialBookmarkFlag)
            {
                Logger.Info("Skipping TutorialBookmarkFlag tutorial. Progress = " + progress);
                return;
            }

            bookmarkFlagTutorialBubble.Visible = true;
            UpdateTutorialBubbleCoordinates();
        }

        private void InitializeHandlesTutorialBubble()
        {
            var progress = TutorialHelper.GetTutorialProgress();
            if (progress != TutorialProgress.TutorialHandles)
            {
                Logger.Info("Skipping TutorialHandles tutorial. Progress = " + progress);
                return;
            }

            handlesTutorialBubble.Visible = true;
            UpdateTutorialBubbleCoordinates();
        }

        private void InitializeShareButtonTutorialBubble()
        {
            var progress = TutorialHelper.GetTutorialProgress();
            if (progress != TutorialProgress.TutorialShareButton)
            {
                Logger.Info("Skipping TutorialShareButton tutorial. Progress = " + progress);
                return;
            }

            shareButtonTutorialBubble.Visible = true;
            UpdateTutorialBubbleCoordinates();
        }

        private void InitializeCloseDetailsTutorialBubble()
        {
            var progress = TutorialHelper.GetTutorialProgress();
            if (progress != TutorialProgress.TutorialCloseDetails)
            {
                Logger.Info("Skipping TutorialCloseDetails tutorial. Progress = " + progress);
                return;
            }

            closeDetailsTutorialBubble.Visible = true;
            UpdateTutorialBubbleCoordinates();
        }

        private void bookmarkFlagTutorialBubble_Advance(object sender, EventArgs e)
        {
            bookmarkFlagTutorialBubble.Visible = false;

            TutorialHelper.AdvanceProgress();

            OnTutorialProgressUpdated();

            InitializeHandlesTutorialBubble();
        }

        private void handlesTutorialBubble_Advance(object sender, EventArgs e)
        {
            handlesTutorialBubble.Visible = false;

            TutorialHelper.AdvanceProgress();

            OnTutorialProgressUpdated();

            InitializeShareButtonTutorialBubble();
        }

        private void shareToFacebookButton_AdvanceTutorial(object sender, EventArgs e)
        {
            shareButtonTutorialBubble.Visible = false;

            TutorialHelper.AdvanceProgress();

            OnTutorialProgressUpdated();

            InitializeCloseDetailsTutorialBubble();
        }

        private void closeDetailsTutorialBubble_Advance(object sender, EventArgs e)
        {
            closeDetailsTutorialBubble.Visible = false;

            TutorialHelper.AdvanceProgress();

            OnTutorialProgressUpdated();

            OnInitializeStartOverTutorialBubble();
        }

        internal void HideTutorialBubbles()
        {
            bookmarkFlagTutorialBubble.Visible = false;
            handlesTutorialBubble.Visible = false;
            shareButtonTutorialBubble.Visible = false;
            closeDetailsTutorialBubble.Visible = false;
        }

    }
}
using System;
using System.Drawing;
using System.Windows.Forms;
using NLog;
using WMPLib;
using OdessaGUIProject.Properties;
using OdessaGUIProject.DRM_Helpers;
using OdessaGUIProject.Other_Helpers;
using System.IO;
using OdessaGUIProject.Workers;

namespace OdessaGUIProject.UI_Controls
{
    internal partial class TrimVideoControl : UserControl
    {

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private AxWMPLib.AxWindowsMediaPlayer wmPlayer;

        private VideoPlayerSeekHelper videoPlayerSeekHelper = new VideoPlayerSeekHelper();

        /* Locations of the tickBoxes during mouse move */
        private int _startTickX;
        private int _endTickX;
        private int _currentTickX;

        private double zoomStartPosition;
        private double zoomEndPosition;

        /* Bounds of tickBoxes */
        private int leftBounds;
        private int rightBounds;

        private bool isPlayHeadBeforeEndTickBox;

        private bool isPausedByUser;

        private bool isSeekerWorking;

        private bool isLoaded; // tracks whether current highlight has loaded yet. use to not flicker the playstate button

        private HighlightObject currentHighlight;

        internal RawVideoThumbnailControl currentVideoThumbnailControl; // reference to the video thumbnail control in the list of highlights

        internal HighlightObject CurrentHighlight
        {
            set
            {
                isLoaded = false;

                currentHighlight = value;

                zoomStartPosition = currentHighlight.EndTime.TotalSeconds - 120; // 2 minutes before bookmark
                if (zoomStartPosition < 0)
                    zoomStartPosition = 0;

                zoomEndPosition = currentHighlight.EndTime.TotalSeconds + 15; // 15 seconds after bookmark
                if (zoomEndPosition > currentHighlight.InputFileObject.VideoDurationInSeconds)
                    zoomEndPosition = currentHighlight.InputFileObject.VideoDurationInSeconds;

                if (wmPlayer.URL == currentHighlight.InputFileObject.SourceFileInfo.FullName)
                { // no need to reload the ioItem
                    wmPlayer.Ctlcontrols.currentPosition = currentHighlight.StartTime.TotalSeconds;
                }
                else
                {
                    wmPlayer.URL = currentHighlight.InputFileObject.SourceFileInfo.FullName;
                    wmPlayer.Ctlcontrols.currentPosition = currentHighlight.StartTime.TotalSeconds; // this must come after .URL change
                }
                wmPlayer.Ctlcontrols.play();
                playStatePicture.Image = Resources.pause;
                UpdateTickBoxLocations();

                playHeadBox.Visible = true;
                startTickBox.Visible = true;
                endTickBox.Visible = true;

                saveToDiskCheckBox.Checked = currentHighlight.SaveToDisk;
                shareToFacebookCheckBox.Checked = currentHighlight.ShareToFacebook;
                titleTextBox.Text = currentHighlight.Title;
            }
            get
            {
                return currentHighlight;
            }

        }


        public TrimVideoControl()
        {
            InitializeComponent();

            BackColor = Color.Transparent;

            // Try creating WMP control
            // We do this here so we can gracefully catch errors if the control doesn't load
            try
            {
                
                wmPlayer = new AxWMPLib.AxWindowsMediaPlayer();
                ((System.ComponentModel.ISupportInitialize)(wmPlayer)).BeginInit();
                //SuspendLayout();
                wmPlayer.CreateControl();
                wmPlayer.Parent = this;
                wmPlayer.Name = "wmPlayer";
                wmPlayer.Ctlenabled = true;
                System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TrimVideoControl));
                wmPlayer.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("wmPlayer.OcxState")));
                wmPlayer.Location = new Point(12, 13);
                wmPlayer.Size = new Size(636, 358);
                wmPlayer.enableContextMenu = true;
                wmPlayer.stretchToFit = true;
                wmPlayer.uiMode = "none";
                wmPlayer.settings.autoStart = false;
                wmPlayer.ErrorEvent += wmPlayer_ErrorEvent;
                wmPlayer.MediaChange += wmPlayer_MediaChange;
                wmPlayer.MediaError += wmPlayer_MediaError;
                wmPlayer.OpenStateChange += wmPlayer_OpenStateChange;
                wmPlayer.PlayStateChange += wmPlayer_PlayStateChange;
                wmPlayer.Warning += wmPlayer_Warning;
                this.Controls.Add(wmPlayer);
                ((System.ComponentModel.ISupportInitialize)(wmPlayer)).EndInit();
                
                //this.ResumeLayout(false);
                //this.PerformLayout(); 
                //wmPlayer.Show();
                //wmPlayer.BringToFront();
            }
            catch (Exception ex)
            {
                Logger.Error("Error creating WMP control: " + ex);
            }

            

            videoPlayerSeekHelper.mediaPlayer = wmPlayer;

            videoPlayerSeekHelper.DoneWorking += new VideoPlayerSeekHelper.DoneWorkingEventHandler(videoPlayerSeekHelper_DoneWorking);
        }

        void videoPlayerSeekHelper_DoneWorking(object sender, EventArgs e)
        {
            Logger.Debug("called");

            if (wmPlayer.playState == WMPPlayState.wmppsPlaying)
            { // only do this if we're playing so the playhead doesn't jump after a paused seek

                // only update tick mark locations when done working
                tickBoxLocationsTimer.Enabled = true;

                // only update play status when done working
                playerStatusTimer.Enabled = true;
            }


            isSeekerWorking = false;
        }

        
        private void TrimVideoControl_Load(object sender, EventArgs e)
        {

            leftBounds = timelineBox.Left; // -(playHeadBox.Width / 2);
            rightBounds = timelineBox.Left + timelineBox.Width; // - (endTickBox.Width / 2);

            var activationState = Protection.GetLicenseStatus();
            if (activationState == Protection.ActivationState.TrialExpired || activationState == Protection.ActivationState.Unlicensed)
            {
                currentPositionLabel.Visible = false;
                timelineBox.Width = wmPlayer.Left + wmPlayer.Width - timelineBox.Left;
            }

        }

        private void TrimVideoControl_Unload()
        {
            playerStatusTimer.Enabled = false;
            tickBoxLocationsTimer.Enabled = false;
            wmPlayer.close();
            wmPlayer = null;
        }


        private void wmPlayer_MediaError(object sender, AxWMPLib._WMPOCXEvents_MediaErrorEvent e)
        {
            try
            // If the Player encounters a corrupt or missing ioItem, 
            // show the hexadecimal error code and URL.
            {
                var errSource = e.pMediaObject as IWMPMedia2;
                if (errSource != null)
                {
                    IWMPErrorItem errorItem = errSource.Error;
                    MessageBox.Show("Error " + errorItem.errorCode.ToString("X")
                                    + " in " + errSource.sourceURL);
                    if (errorItem.errorCode.ToString("X") == "C00D1199")
                    { // TODO: user needs to get codecs. K-Lite Basic worked just fine on a fresh XP
                        if (MessageBox.Show("Ah bummer! We had trouble loading your video file. This is most likely due to a missing codec." + Environment.NewLine + Environment.NewLine +
                            "The easiest fix is to download the free K-Lite Codec pack. Would you like to do that now?", "Unable to load video", MessageBoxButtons.YesNo, MessageBoxIcon.Error)
                            == DialogResult.Yes)
                        {
                            BrowserHelper.LaunchBrowser("http://www.codecguide.com/download_k-lite_codec_pack_basic.htm");
                        }
                    }
                }
            }
            catch (InvalidCastException ex)
            // In case pMediaObject is not an IWMPMedia item.
            {
                Logger.Error("Exception thrown: {0}", ex);
                MessageBox.Show("Lame error. We don't know what caused this one :-(");
            }

        }

        private void seekButton_Click(object sender, EventArgs e)
        {
            wmPlayer.Ctlcontrols.currentPosition = 20;
        }


        private void fullScreenButton_Click(object sender, EventArgs e)
        {
            wmPlayer.fullScreen = true;
        }


        private void startTickBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                _startTickX = e.X;
            }
        }

        private void startTickBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && Math.Abs(e.X - _startTickX) > 5) // 5 is a threshold we use to make sure there was enough movement
            {
                int newPosition = startTickBox.Left + (e.X - _startTickX);
                if (newPosition < leftBounds)
                    return; // out of bounds
                if (newPosition > endTickBox.Left - startTickBox.Width)
                    return; // out of bounds.  can't go past endTickBox

                startTickBox.Left = newPosition;

                double position = GetPositionFromTickBoxLocation(startTickBox);
                CurrentHighlight.StartTime = TimeSpan.FromSeconds(position);

                currentVideoThumbnailControl.UpdateTimeLabel();

                // I don't think we should seek when we move the start box. It's disruptive.
                // double newSeconds = GetPositionFromTickBoxLocation(startTickBox);
                // SeekToPosition(newSeconds);
            }
        }

        private double GetPositionFromTickBoxLocation(Control tickBox)
        {
            
            double percentage = (double)(tickBox.Left - timelineBox.Left) / timelineBox.Width;

            double position = percentage * (zoomEndPosition - zoomStartPosition) + zoomStartPosition;

            return position;

            //return (tickBox.Left - timelineBox.Left + (tickBox.Width / 2)) * wmPlayer.Ctlcontrols.currentItem.duration / timelineBox.Width;
        }

        private void MoveTickBoxToPosition(Control tickBox, double position)
        {
            // location of position zoomStartPosition is timelineBox.Left
            // location of position zoomEndPosition is timelineBox.Left + timelineBox.Width

            double percentage = ((position - zoomStartPosition) / (zoomEndPosition - zoomStartPosition));
            tickBox.Left = timelineBox.Left + (int)(percentage * timelineBox.Width);

        }

        private void wmPlayer_MediaChange(object sender, AxWMPLib._WMPOCXEvents_MediaChangeEvent e)
        {
            Logger.Debug("MediaChange event: " + e.ToString());

        }

        private void endTickBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                _endTickX = e.X;
            }
        }

        private void endTickBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                int newPosition = endTickBox.Left + (e.X - _endTickX);
                if (newPosition > rightBounds)
                    return; // out of bounds
                if (newPosition < startTickBox.Left + startTickBox.Width)
                    return; // out of bounds

                endTickBox.Left = newPosition;

                double position = GetPositionFromTickBoxLocation(endTickBox);
                CurrentHighlight.EndTime = TimeSpan.FromSeconds(position);
                currentVideoThumbnailControl.UpdateTimeLabel();
                
                // I don't think we should do this for the end box
                //double newSeconds = GetPositionFromTickBoxLocation(endTickBox);
                //SeekToPosition(newSeconds);
            }
        }

        private void wmPlayer_PlayStateChange(object sender, AxWMPLib._WMPOCXEvents_PlayStateChangeEvent e)
        {
            Logger.Debug("PlayStateChanged to " + GetFriendlyPlayState((WMPPlayState)e.newState));
            
            if (isLoaded) // this must be above the bottom code
                UpdatePlayStatePicture();

            if ((WMPPlayState)e.newState == WMPPlayState.wmppsPlaying)
            {
                isLoaded = true;
                playerStatusTimer.Enabled = true;
                tickBoxLocationsTimer.Enabled = true;
            }
            else
            {
                playerStatusTimer.Enabled = false;
                tickBoxLocationsTimer.Enabled = false;

                if ((WMPPlayState)e.newState == WMPPlayState.wmppsMediaEnded)
                { // end of clip. let's move playhead to start of highlight
                    Logger.Debug("Hit the end of clip!");
                }
            }

        }

        private string GetFriendlyPlayState(WMPPlayState playState)
        {
            switch (playState)
            {
                case WMPPlayState.wmppsBuffering: return "Buffering";
                case WMPPlayState.wmppsLast: return "Last";
                case WMPPlayState.wmppsMediaEnded: return "Media Ended";
                case WMPPlayState.wmppsPaused: return "Paused";
                case WMPPlayState.wmppsPlaying: return "Playing";
                case WMPPlayState.wmppsReady: return "Ready";
                case WMPPlayState.wmppsReconnecting: return "Reconnecting";
                case WMPPlayState.wmppsScanForward: return "Scan Forward";
                case WMPPlayState.wmppsScanReverse: return "Scan Reverse";
                case WMPPlayState.wmppsStopped: return "Stopped";
                case WMPPlayState.wmppsTransitioning: return "Transitioning";
                case WMPPlayState.wmppsUndefined: return "Undefined";
                case WMPPlayState.wmppsWaiting: return "Waiting";
                default: return "Unknown";
            }
        }

        private void playerStatusTimer_Tick(object sender, EventArgs e)
        {
                currentPositionLabel.Text = wmPlayer.Ctlcontrols.currentPositionString + " / " + wmPlayer.Ctlcontrols.currentItem.durationString;

        }

        private void playHeadBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                _currentTickX = e.X;
            }
        }

        private void playHeadBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                int newPosition = playHeadBox.Left + (e.X - _currentTickX);
                if (newPosition < leftBounds)
                    return; // out of bounds
                if (newPosition > rightBounds)
                    return; // out of bounds

                playHeadBox.Left = newPosition;

                double newSeconds = GetPositionFromTickBoxLocation(playHeadBox);

                SeekToPosition(newSeconds);

            }
        }


        private void playHeadBox_MouseUp(object sender, MouseEventArgs e)
        { // not sure why this event is here
            //videoPlayerSeekHelper.SeekRequest((playHeadBox.Left - timelineBox.Left) / (double)timelineBox.Width *
              //                                         wmPlayer.Ctlcontrols.currentItem.duration);

        }

        private void wmPlayer_OpenStateChange(object sender, AxWMPLib._WMPOCXEvents_OpenStateChangeEvent e)
        {
            Logger.Debug("OpenStateChange: " + e.newState.ToString());

            if (e.newState == (int)WMPOpenState.wmposMediaOpen)
            {
                // do this here because we know media has loaded
                UpdateTickBoxLocations();
            }
        }

        private void UpdateTickBoxLocations()
        {
            MoveTickBoxToPosition(startTickBox, CurrentHighlight.StartTime.TotalSeconds);
            MoveTickBoxToPosition(endTickBox, CurrentHighlight.EndTime.TotalSeconds);
            MoveTickBoxToPosition(playHeadBox, CurrentHighlight.StartTime.TotalSeconds);

            Cursor = Cursors.Default;
        }



        private void wmPlayer_Warning(object sender, AxWMPLib._WMPOCXEvents_WarningEvent e)
        {
            Logger.Error("WMP Warning: {0}", e.description);
        }

        private void wmPlayer_ErrorEvent(object sender, EventArgs e)
        {
            Logger.Error("WMP Error: {0}", e.ToString());
        }


        public void PausePlayer()
        {
            wmPlayer.Ctlcontrols.pause();
        }

        private void startTickBox_MouseUp(object sender, MouseEventArgs e)
        {
            
        }

        private void timelineBox_MouseDown(object sender, MouseEventArgs e)
        {

            if (e.Button == MouseButtons.Left)
            {
                tickBoxLocationsTimer.Enabled = false; // so we don't update playhead tickbox while we seek

                isPlayHeadBeforeEndTickBox = false; // so we don't pause by manually jumping over endbox

                int newPosition = timelineBox.Left + e.X;
                
                playHeadBox.Left = newPosition;

                double newSeconds = GetPositionFromTickBoxLocation(playHeadBox);

                SeekToPosition(newSeconds);

            }

        }

        private void tickBoxLocationsTimer_Tick(object sender, EventArgs e)
        {
            MoveTickBoxToPosition(playHeadBox, wmPlayer.Ctlcontrols.currentPosition);

            // this code pauses the player when we hit the end tickbox
            if (playHeadBox.Left < endTickBox.Left)
                isPlayHeadBeforeEndTickBox = true;
            else if (isPlayHeadBeforeEndTickBox)
            { // play head just hit the tickbox
                isPausedByUser = false;
                PausePlayer();
                isPlayHeadBeforeEndTickBox = false;
            }

            if (playHeadBox.Left > rightBounds)
            { // this might not necessarily be the end of the clip. it could be the end of the zoomed region.
                isPausedByUser = true; // so that we don't start auto-looping
                PausePlayer();
                SeekToPosition(currentHighlight.StartTime.TotalSeconds);
            }

        }

        private void startTickBox_DoubleClick(object sender, EventArgs e)
        {
            playHeadBox.Left = startTickBox.Left;
            double newSeconds = GetPositionFromTickBoxLocation(startTickBox);

            isPausedByUser = false; // set this so we start playing automatically
            SeekToPosition(newSeconds);
        }

        private void endTickBox_MouseUp(object sender, MouseEventArgs e)
        {

        }

        private void playStatePicture_Click(object sender, EventArgs e)
        {
            if (wmPlayer.playState == WMPPlayState.wmppsPlaying)
            {
                isPausedByUser = true;
                wmPlayer.Ctlcontrols.pause();
            }
            else
            {
                if (wmPlayer.playState == WMPPlayState.wmppsStopped)
                { // we hit the end of the clip so let's start at the beginning before playing
                    // we do this seek request manually so the playhead immediately jumps to 0
                    wmPlayer.Ctlcontrols.currentPosition = 0;
                    playerStatusTimer.Enabled = true;
                    tickBoxLocationsTimer.Enabled = true;
                }
                wmPlayer.Ctlcontrols.play();
            }
        }

        private void UpdatePlayStatePicture()
        {
            Logger.Debug("called");

            if (isSeekerWorking)
                return; // we're probably seeking to update the frame

            if (wmPlayer.playState == WMPPlayState.wmppsPlaying)
                playStatePicture.Image = Resources.pause;
            else
                playStatePicture.Image = Resources.play;
        }

        private void SeekToPosition(double position)
        {
            isSeekerWorking = true;
            tickBoxLocationsTimer.Enabled = false;

            if (wmPlayer.playState == WMPPlayState.wmppsPaused && !isPausedByUser)
                playStatePicture.Image = Resources.pause; // it's going to start playing

            videoPlayerSeekHelper.SeekRequest(position, !isPausedByUser);

        }

        private void shareToFacebookCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            currentHighlight.ShareToFacebook = shareToFacebookCheckBox.Checked;
            currentVideoThumbnailControl.UpdateFacebookVisibility(shareToFacebookCheckBox.Checked);

            if (shareToFacebookCheckBox.Checked)
            {
                // make sure we have access token
                if (Properties.Settings.Default.FacebookAccessToken.Length == 0)
                {
                    PausePlayer();
                    shareToFacebookCheckBox.Enabled = false;
                    var facebookHelper = new FacebookHelper();
                    if (facebookHelper.LoginToFacebook() == false)
                    {
                        shareToFacebookCheckBox.Checked = false;
                    }
                    shareToFacebookCheckBox.Enabled = true;
                }

            }
        }

        private void saveToDiskCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            currentHighlight.SaveToDisk = saveToDiskCheckBox.Checked;
            currentVideoThumbnailControl.UpdateSaveVisibility(saveToDiskCheckBox.Checked);
        }

        private void titleTextBox_Leave(object sender, EventArgs e)
        {
            currentHighlight.Title = titleTextBox.Text;
        }

        private void titleTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            Char pressedKey = e.KeyChar;
            if (Char.IsLetterOrDigit(pressedKey) || Char.IsSeparator(pressedKey) || pressedKey == (char)Keys.Back)
            {
                e.Handled = false; // allow it
            }
            else if (Char.IsPunctuation(pressedKey) || Char.IsSymbol(pressedKey))
            {
                var invalidChars = Path.GetInvalidFileNameChars();
                for (int i = 0; i < invalidChars.Length; i++)
                {
                    if (pressedKey == invalidChars[i])
                    {
                        e.Handled = true;
                        return;
                    }
                }
                // Allow input.
                e.Handled = false;
            }
            else
            {
                // Stop the character from being entered into the control since not a letter, nor punctuation, nor a space.
                e.Handled = true;
            }
        }

        private void titleTextBox_TextChanged(object sender, EventArgs e)
        {
            currentVideoThumbnailControl.UpdateTitle(titleTextBox.Text);
            currentHighlight.Title = titleTextBox.Text;
        }

        private void shareToFacebookButton_Click(object sender, EventArgs e)
        {
            // TEMP: THIS IS A HACK FOR THE DEMO VIDEO
            shareToFacebookCheckBox.Checked = true;
            
            if (SaveAndShare != null)
                SaveAndShare(sender, e);

        }

        private void saveToDiskButton_Click(object sender, EventArgs e)
        {
            // TEMP: THIS IS A HACK FOR THE DEMO VIDEO
            saveToDiskCheckBox.Checked = true;

            if (SaveAndShare != null)
                SaveAndShare(sender, e);
        }

    }
}

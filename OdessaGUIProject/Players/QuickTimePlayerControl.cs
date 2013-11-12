using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using QTOControlLib;
using QTOLibrary;
using NLog;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using OdessaGUIProject.Workers;
using GaDotNet.Common.Helpers;

namespace OdessaGUIProject.Players
{
    public partial class QuickTimePlayerControl : UserControl
    {

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private bool isDisposed = false;

        private AxQTOControlLib.AxQTControl qtPlayer;

        internal GenericPlayerControl.PlayStates PlayState;

        public QuickTimePlayerControl()
        {
            InitializeComponent();

            initializePlayer();

            this.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Name = "QuickTimePlayer";
            this.Enabled = true;
            this.TabIndex = 0;

        }

        private void initializePlayer()
        {
            if (MainModel.QuickTimePlayer == null)
            {
                // check QT version just to make sure
                // without this, Vista reported QT was installed just by having QTO*.dlls there
                if (!isQuickTimeInstalled())
                    throw new Exception("QuickTime is not installed!");

                MainModel.QuickTimePlayer = new AxQTOControlLib.AxQTControl();
            }

            qtPlayer = MainModel.QuickTimePlayer;

            ((System.ComponentModel.ISupportInitialize)(this.qtPlayer)).BeginInit();

            this.qtPlayer.Dock = DockStyle.Fill;
            this.qtPlayer.Enabled = true;
            this.qtPlayer.Name = "axQTControl1";
            this.qtPlayer.TabIndex = 0;

            // wmPlayer didn't have this line. everything seems to be working fine
            //this.QuickTimePlayer.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("QuickTimePlayer.OcxState")));

            this.qtPlayer.QTEvent += new AxQTOControlLib._IQTControlEvents_QTEventEventHandler(qtPlayer_QTEvent);
            this.qtPlayer.StatusUpdate += new AxQTOControlLib._IQTControlEvents_StatusUpdateEventHandler(qtPlayer_StatusUpdate);
            this.qtPlayer.Error += new AxQTOControlLib._IQTControlEvents_ErrorEventHandler(qtPlayer_Error);

            this.Controls.Add(qtPlayer);
            ((System.ComponentModel.ISupportInitialize)(qtPlayer)).EndInit();

            if (qtPlayer.QuickTimeVersion == 0)
                throw new Exception("QuickTimeVersion == 0");

            this.qtPlayer.ErrorHandling = (int)QTErrorHandlingOptionsEnum.qtErrorHandlingRaiseException;
            this.qtPlayer.Sizing = QTSizingModeEnum.qtMovieFitsControlMaintainAspectRatio;
            this.qtPlayer.MovieControllerVisible = false;
        }

        private bool isQuickTimeInstalled()
        {
            try
            {
                var t = Type.GetTypeFromProgID("QuickTimeCheckObject.QuickTimeCheck.1");
                Activator.CreateInstance(t);
                return true;
            }
            catch (Exception ex)
            {
                Logger.Info(ex);
                return false;
            }

        }

        private void qtPlayer_Error(object sender, AxQTOControlLib._IQTControlEvents_ErrorEvent e)
        {
            Logger.Error("Error code: " + e.errorCode + " from " + e.origin);
        }

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (isDisposed)
                return;

            Logger.Debug("Disposed with disposing = " + disposing);
            // don't call closePlayer();. It throws RCW exception
            // don't call QuickTimeTerminate(). It throws RCW exception

            
            if (disposing)
            {

                if (qtPlayer != null)
                {
                    try
                    {
                        if (qtPlayer.Movie != null)
                        {
                            qtPlayer.Movie.Stop();

                            removeMovieEventListeners(qtPlayer.Movie);
                        }

                        qtPlayer.URL = QuickTimePlayerControl.GetPathToBlackPixel(); // supposedly this closes file handles

                    }
                    catch (Exception ex)
                    {
                        Logger.Error(ex);
                    }

                    qtPlayer.QTEvent -= qtPlayer_QTEvent;
                    qtPlayer.StatusUpdate -= qtPlayer_StatusUpdate;
                    qtPlayer.Error -= qtPlayer_Error;

                    if (Controls.Contains(qtPlayer))
                        Controls.Remove(qtPlayer);

                    /* We don't dispose object until app closes. In case user comes back to Details
                 * This is so we don't keep destroying and creating it, which tended to lead to crashes

                    QuickTimePlayer.QuickTimeTerminate();
                    QuickTimePlayer.Dispose(); // try to avoid 'COM object that has been separated from its underlying RCW cannot be used.'
                    QuickTimePlayer = null;
                     */ 
                }

                if (components != null)
                    components.Dispose();
            }

            isDisposed = true;
            GC.SuppressFinalize(this);

            base.Dispose(disposing);
        }

        /*
        public void Dispose()
        {
            Logger.Debug("Public dispose");

            Dispose(true);

            // Use SupressFinalize in case a subclass
            // of this type implements a finalizer.
            GC.SuppressFinalize(this);
        }
         */ 

        internal event EventHandler<PlayerErrorEventArgs> PlayerError;

        internal event EventHandler<OpenStateChangedEventArgs> OpenStateChanged;

        internal event EventHandler<PlayStateChangedEventArgs> PlayStateChanged;

        internal static List<string> SupportedFileExtensions
        {
            get
            {
                return new List<string>() {".MOV", ".MP4", ".M4V", ".AVI"};
            }
        }

        internal string URL
        {
            get
            {
                if (qtPlayer == null || qtPlayer.Movie == null)
                    return null;
                return qtPlayer.Movie.URL;
            }
            set
            {

                try
                {
                    if (qtPlayer != null && qtPlayer.Movie != null)
                        removeMovieEventListeners(qtPlayer.Movie);

                    PlayState = GenericPlayerControl.PlayStates.NoVideo; // reset playstate in case events aren't called

                    qtPlayer.URL = value;

                    addMovieEventListeners(qtPlayer.Movie);
                }
                catch (AccessViolationException ex)
                {
                    Logger.Error(ex);

                    AnalyticsHelper.FireEvent("QuickTime Error - AccessViolationException");

                    // try reinitializing
                    qtPlayer.Dispose();
                    qtPlayer = null;
                    
                    throw new ArgumentException();
                }
                catch (COMException ex)
                {
                    QTUtils qtu = new QTUtils();

                    Logger.Error("Error Code: " + ex.ErrorCode.ToString("X", CultureInfo.InvariantCulture) + Environment.NewLine +
                        "QT Error code : " + qtu.QTErrorFromErrorCode(ex.ErrorCode).ToString(CultureInfo.InvariantCulture));

                    AnalyticsHelper.FireEvent("QuickTime Error - " + ex.ErrorCode.ToString("X", CultureInfo.InvariantCulture) + " - " + qtu.QTErrorFromErrorCode(ex.ErrorCode).ToString(CultureInfo.InvariantCulture));
                    AnalyticsHelper.FireEvent("QuickTime Error - " + ex.ErrorCode.ToString("X", CultureInfo.InvariantCulture) + " - " + qtu.QTErrorFromErrorCode(ex.ErrorCode).ToString(CultureInfo.InvariantCulture)
                        + " - " + Path.GetExtension(value));

                    try
                    {
                        FileInfo fileInfo = new FileInfo(value);
                        UploadMediaInfo(fileInfo, ex.ErrorCode.ToString("X", CultureInfo.InvariantCulture) + " - " + qtu.QTErrorFromErrorCode(ex.ErrorCode).ToString(CultureInfo.InvariantCulture));
                    }
                    catch (Exception ex2)
                    {
                        Logger.Error("Error creating FileInfo for " + value + ": " + ex2);
                    }

                    throw new ArgumentException();

                }
                catch (Exception ex)
                {
                    Logger.Error("Exception: " + ex.ToString());

                    AnalyticsHelper.FireEvent("QuickTime Error - unknown");

                    try
                    {
                        FileInfo fileInfo = new FileInfo(value);
                        UploadMediaInfo(fileInfo, "unknown: " + ex);
                    }
                    catch (Exception ex2)
                    {
                        Logger.Error("Error creating FileInfo for " + value + ": " + ex2);
                    }


                    throw new ArgumentException();

                }

            }
        }


        internal double CurrentPosition
        {
            get
            {
                if (qtPlayer != null && qtPlayer.Movie != null)
                    return (double)qtPlayer.Movie.Time / (double)qtPlayer.Movie.TimeScale;
                else
                {
                    Debug.Assert(false, "Player is null!");
                    return 0;
                }
            }

            set
            {
                if (qtPlayer != null && qtPlayer.Movie != null)
                {
                    if (CurrentPosition == value) // we're already at the right position
                        return;

                    qtPlayer.Movie.Time = (int)Math.Ceiling(value * qtPlayer.Movie.TimeScale);
                }
                else
                {
                    Debug.Assert(false, "Player is null!");
                }
            }
        }


        internal bool Mute
        {
            get
            {
                if (qtPlayer != null && qtPlayer.Movie != null)
                    return qtPlayer.Movie.AudioMute;
                else
                {
                    Debug.Assert(false, "Player is null!");
                    return false;
                }
            }

            set
            {
                if (qtPlayer != null && qtPlayer.Movie != null)
                    qtPlayer.Movie.AudioMute = value;
                else
                {
                    Debug.Assert(false, "Player is null!");
                }
            }
        }


        internal void Play()
        {
            if (qtPlayer == null || qtPlayer.Movie == null)
                return;

            if (PlayState == GenericPlayerControl.PlayStates.Playing)
                return;

            Logger.Debug("Actually called play");

            qtPlayer.Movie.Play();
        }


        internal void Pause()
        {
            if (qtPlayer == null || qtPlayer.Movie == null)
                return;

            if (PlayState == GenericPlayerControl.PlayStates.Paused)
                return;

            qtPlayer.Movie.Pause();
        }


        internal void Stop()
        {
            if (qtPlayer == null || qtPlayer.Movie == null)
                return;

            if (PlayState == GenericPlayerControl.PlayStates.Stopped)
                return;

            qtPlayer.Movie.Stop();
        }


        private void qtPlayer_StatusUpdate(object sender, AxQTOControlLib._IQTControlEvents_StatusUpdateEvent e)
        {
            Logger.Info("StatusUpdate: " + e.statusMessage);
            // Status update event handler 
            // Handle movie fullscreen events 

            /*
                qtMovieLoadStateError = -1,
                qtStatusNone = 0,
                qtStatusConnecting = 2,
                qtStatusNegotiating = 5,
                qtStatusRequestedData = 11,
                qtStatusBuffering = 12,
                qtMovieLoadStateLoading = 1000,
                qtMovieLoadStateLoaded = 2000,
                qtStatusURLChanged = 4096,
                qtStatusFullScreenBegin = 4097,
                qtStatusFullScreenEnd = 4098,
                qtStatusMovieLoadFinalize = 4099,
                qtMovieLoadStatePlayable = 10000,
                qtMovieLoadStatePlaythroughOK = 20000,
                qtMovieLoadStateComplete = 100000,
            */ 

            switch ((QTStatusCodesEnum)e.statusCode)
            {
                // fullscreen begin 
                case QTStatusCodesEnum.qtStatusFullScreenBegin:
                    this.Hide();	// hide movie window 
                    break;

                // fullscreen end 
                case QTStatusCodesEnum.qtStatusFullScreenEnd:
                    qtPlayer.SetScale(1);	// set back to a reasonable size 
                    this.Show();	// show movie window again 
                    break;

                case QTStatusCodesEnum.qtMovieLoadStateError:
                    var playerErrorEventArgs = new PlayerErrorEventArgs();
                    playerErrorEventArgs.ErrorMessage = "We couldn't load the movie!";
                    playerErrorEventArgs.SupportID = "ErrorLoadingVideo";
                    if (PlayerError != null)
                        PlayerError(sender, playerErrorEventArgs);
                    break;

                case QTStatusCodesEnum.qtMovieLoadStateComplete:
                    var openStateEventArgs = new OpenStateChangedEventArgs();
                    openStateEventArgs.OpenState = GenericPlayerControl.OpenStates.Open;
                    if (OpenStateChanged != null)
                        OpenStateChanged(sender, openStateEventArgs);
                    break;
            }
        }

        void qtPlayer_QTEvent(object sender, AxQTOControlLib._IQTControlEvents_QTEventEvent e)
        {

            // Code to handle various QuickTime Events 

            // When running your code in debug mode you will 
            // see the following messages displayed in the 
            // Immediate Window 

            switch ((QTEventIDsEnum)e.eventID)
            {
                // status strings 
                case QTEventIDsEnum.qtEventShowStatusStringRequest:
                    {
                        string msg = e.eventObject.GetParam(QTEventObjectParametersEnum.qtEventParamStatusString).ToString();
                        Logger.Info("qtEventShowStatusStringRequest: " + msg);
                    }
                    break;

                // rate changes 
                case QTEventIDsEnum.qtEventRateWillChange:
                    {
                        var rate = (e.eventObject.GetParam(QTEventObjectParametersEnum.qtEventParamMovieRate));
                        int therate = Convert.ToInt16(rate);
                        if (therate > 0)
                        {
                            PlayState = GenericPlayerControl.PlayStates.Playing;
                        }
                        else
                        {
                            PlayState = GenericPlayerControl.PlayStates.Paused;
                        }

                        if (PlayStateChanged != null)
                            PlayStateChanged(sender, new PlayStateChangedEventArgs() { PlayState = PlayState });

                        Logger.Debug("RateWillChange to: " + rate);
                    }
                    break;

                // time changes 
                case QTEventIDsEnum.qtEventTimeWillChange:
                    {
                        var time = (int)e.eventObject.GetParam(QTEventObjectParametersEnum.qtEventParamMovieTime);
                        Logger.Debug("TimeWillChange to: " + (double)(time) / qtPlayer.Movie.TimeScale );
                    }
                    break;

                // audio volume changes 
                case QTEventIDsEnum.qtEventAudioVolumeDidChange:
                    {
                        string vol = e.eventObject.GetParam(QTEventObjectParametersEnum.qtEventParamAudioVolume).ToString();
                        Logger.Debug("AudioVolumeDidChange to: " + vol);
                    }
                    break;

                case QTEventIDsEnum.qtEventMovieDidEnd:
                    {
                        Logger.Debug("MovieDidEnd");
                        if (PlayStateChanged != null)
                            PlayStateChanged(sender, new PlayStateChangedEventArgs() { PlayState = GenericPlayerControl.PlayStates.Stopped });
                    }
                    break;

                default:
                    {
                        Logger.Debug("Other QT_Event: " + e.eventID);
                        break;
                    }
            }
        }

        private void addMovieEventListeners(QTMovie myMovie)
        {
            // Contains code to demonstrate how to add listeners 
            // for various QuickTime Events 

            // Make sure a movie is loaded first 
            if (myMovie == null) return;

            // status string listener 
            //myMovie.EventListeners.Add(QTEventClassesEnum.qtEventClassApplicationRequest, QTEventIDsEnum.qtEventShowStatusStringRequest, 0, null);

            // rate change listener 
            myMovie.EventListeners.Add(QTEventClassesEnum.qtEventClassStateChange, QTEventIDsEnum.qtEventRateWillChange, 0, null);

            // time change listener 
            //myMovie.EventListeners.Add(QTEventClassesEnum.qtEventClassTemporal, QTEventIDsEnum.qtEventTimeWillChange, 0, null);

            // audio volume change listener 
            //myMovie.EventListeners.Add(QTEventClassesEnum.qtEventClassAudio, QTEventIDsEnum.qtEventAudioVolumeDidChange, 0, null);

            // move did end
            myMovie.EventListeners.Add(QTEventClassesEnum.qtEventClassStateChange, QTEventIDsEnum.qtEventMovieDidEnd);


        }

        // remove all event listeners for the movie 
        private void removeMovieEventListeners(QTMovie myMovie)
        {
            // Make sure a movie is loaded first 
            if (myMovie != null)
            {
                // Remove all event listeners 
                myMovie.EventListeners.RemoveAll();
            }
        }

        private void QuickTimePlayerControl_Resize(object sender, EventArgs e)
        {
            qtPlayer.SetScale(1); // this should keep video quality looking good. otherwise it's low-res when you expand.
            this.Update(); // this should hopefully help with redrawing 
        }

        private static string GetPathToBlackPixel()
        {
            string path = "";
            try
            {
                var thisexe = new FileInfo(Assembly.GetExecutingAssembly().Location);
                if (thisexe.DirectoryName != null) 
                    path = Path.Combine(thisexe.DirectoryName, "blackpixel.png");
            }
            catch (Exception ex)
            {
                Logger.Error("Exception: " + ex);
            }

            return path;

        }

        private void UploadMediaInfo(FileInfo fileInfo, string errorCode)
        {
            var uploadMediaInfoWorker = new UploadMediaInfoWorker(fileInfo, "QuickTime Error: " + errorCode);
            uploadMediaInfoWorker.RunWorkerAsync();
            // we don't care when this thing ends
        }

    }
}

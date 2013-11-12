using System;
using System.Windows.Forms;
using AxWMPLib;
using NLog;
using GaDotNet.Common.Helpers;
using System.IO;
using OdessaGUIProject.Workers;

namespace OdessaGUIProject.Players
{
    public partial class WindowsMediaPlayerControl : UserControl
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly AxWMPLib.AxWindowsMediaPlayer wmPlayer;

        private bool isDisposed = false;

        internal GenericPlayerControl.PlayStates PlayState;

        public WindowsMediaPlayerControl()
        {
            InitializeComponent();

            // let GenericPlayerControl catch the exception

            wmPlayer = new AxWMPLib.AxWindowsMediaPlayer();

            ((System.ComponentModel.ISupportInitialize)(wmPlayer)).BeginInit();
            this.wmPlayer.Name = "wmPlayer";
            this.wmPlayer.Enabled = true;
            this.wmPlayer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wmPlayer.TabIndex = 0;
            this.wmPlayer.OpenStateChange += new AxWMPLib._WMPOCXEvents_OpenStateChangeEventHandler(this.wmPlayer_OpenStateChange);
            this.wmPlayer.PlayStateChange += new AxWMPLib._WMPOCXEvents_PlayStateChangeEventHandler(this.wmPlayer_PlayStateChange);
            this.wmPlayer.ErrorEvent += new System.EventHandler(this.wmPlayer_ErrorEvent);
            this.wmPlayer.MediaChange += new AxWMPLib._WMPOCXEvents_MediaChangeEventHandler(this.wmPlayer_MediaChange);
            this.wmPlayer.MediaError += new AxWMPLib._WMPOCXEvents_MediaErrorEventHandler(this.wmPlayer_MediaError);

            this.Controls.Add(wmPlayer);
            ((System.ComponentModel.ISupportInitialize)(wmPlayer)).EndInit();

            wmPlayer.uiMode = "none"; // this must come after EndInit()
            wmPlayer.stretchToFit = true; // this must come after EndInit()

            this.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Name = "wmPlayer";
            this.Enabled = true;
            this.TabIndex = 0;

            /*
            ((System.ComponentModel.ISupportInitialize)(wmPlayer)).BeginInit();
            this.wmPlayer.Name = "wmPlayer";
            this.wmPlayer.Enabled = true;
            this.wmPlayer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wmPlayer.Location = new System.Drawing.Point(0, 0);
            this.wmPlayer.Size = new System.Drawing.Size(489, 271);
            this.wmPlayer.TabIndex = 0;
            this.wmPlayer.OpenStateChanged += new AxWMPLib._WMPOCXEvents_OpenStateChangeEventHandler(this.wmPlayer_OpenStateChange);
            this.wmPlayer.PlayStateChanged += new AxWMPLib._WMPOCXEvents_PlayStateChangeEventHandler(this.wmPlayer_PlayStateChange);
            this.wmPlayer.ErrorEvent += new System.EventHandler(this.wmPlayer_ErrorEvent);
            this.wmPlayer.MediaChange += new AxWMPLib._WMPOCXEvents_MediaChangeEventHandler(this.wmPlayer_MediaChange);
            this.wmPlayer.PlayerError += new AxWMPLib._WMPOCXEvents_MediaErrorEventHandler(this.wmPlayer_PlayerError);

            wmPlayer.uiMode = "none";

            this.Controls.Add(wmPlayer);
            ((System.ComponentModel.ISupportInitialize)(wmPlayer)).EndInit();
            */
        }


        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {

            if (isDisposed)
                return;

            Logger.Debug("Disposing with disposing = " + disposing);

            
            if (disposing)
            {
                if (wmPlayer != null)
                {
                    wmPlayer.OpenStateChange -= this.wmPlayer_OpenStateChange;
                    wmPlayer.PlayStateChange -= this.wmPlayer_PlayStateChange;
                    wmPlayer.ErrorEvent -= this.wmPlayer_ErrorEvent;
                    wmPlayer.MediaChange -= this.wmPlayer_MediaChange;
                    wmPlayer.MediaError -= this.wmPlayer_MediaError;

                    wmPlayer.Dispose(); // try to avoid 'COM object that has been separated from its underlying RCW cannot be used.'
                }

                if (components != null)
                    components.Dispose();
            }

            isDisposed = true;
            GC.SuppressFinalize(this);

            base.Dispose(disposing);
        }

        internal event EventHandler<PlayerErrorEventArgs> PlayerError;

        internal event EventHandler<OpenStateChangedEventArgs> OpenStateChanged;

        internal event EventHandler<PlayStateChangedEventArgs> PlayStateChanged;

        internal double CurrentPosition
        {
            get { return wmPlayer.Ctlcontrols.currentPosition; }
            set { wmPlayer.Ctlcontrols.currentPosition = value; }
        }

        internal bool Mute
        {
            get { return wmPlayer.settings.mute; }
            set { wmPlayer.settings.mute = value; }
        }

        internal string URL
        {
            get { return wmPlayer.URL; }
            set 
            {
                PlayState = GenericPlayerControl.PlayStates.NoVideo; // reset playstate in case events aren't called
                wmPlayer.URL = value; 
            }
        }

        internal AxWindowsMediaPlayer WindowsMediaPlayer
        {
            get { return wmPlayer; }
        }

        internal void Pause()
        {
            if (wmPlayer.playState == WMPLib.WMPPlayState.wmppsPlaying)
                wmPlayer.Ctlcontrols.pause();
            else if (wmPlayer.playState == WMPLib.WMPPlayState.wmppsTransitioning) // if we're transition, we have to stop
                wmPlayer.Ctlcontrols.stop();
        }

        internal void Play()
        {
            if (PlayState == GenericPlayerControl.PlayStates.Playing)
                return;

            Logger.Debug("Actually called play");

            wmPlayer.Ctlcontrols.play();
        }

        internal void Stop()
        {
            if (PlayState == GenericPlayerControl.PlayStates.Stopped)
                return;

            wmPlayer.Ctlcontrols.stop();
        }

        private string GetFriendlyPlayState(WMPLib.WMPPlayState playState)
        {
            switch (playState)
            {
                case WMPLib.WMPPlayState.wmppsBuffering: return "Buffering";
                case WMPLib.WMPPlayState.wmppsLast: return "Last";
                case WMPLib.WMPPlayState.wmppsMediaEnded: return "Media Ended";
                case WMPLib.WMPPlayState.wmppsPaused: return "Paused";
                case WMPLib.WMPPlayState.wmppsPlaying: return "Playing";
                case WMPLib.WMPPlayState.wmppsReady: return "Ready";
                case WMPLib.WMPPlayState.wmppsReconnecting: return "Reconnecting";
                case WMPLib.WMPPlayState.wmppsScanForward: return "Scan Forward";
                case WMPLib.WMPPlayState.wmppsScanReverse: return "Scan Reverse";
                case WMPLib.WMPPlayState.wmppsStopped: return "Stopped";
                case WMPLib.WMPPlayState.wmppsTransitioning: return "Transitioning";
                case WMPLib.WMPPlayState.wmppsUndefined: return "Undefined";
                case WMPLib.WMPPlayState.wmppsWaiting: return "Waiting";
                default: return "Unknown";
            }
        }

        private void wmPlayer_ErrorEvent(object sender, EventArgs e)
        {
            Logger.Error("ErrorEvent: " + e.ToString());
        }

        private void wmPlayer_MediaChange(object sender, _WMPOCXEvents_MediaChangeEvent e)
        {
            try
            {
                var changedItem = (WMPLib.IWMPMedia3)e.item;

                Logger.Info("MediaChange event: " + changedItem.name);
            }
            catch (Exception) { }

        }

        private void wmPlayer_MediaError(object sender, _WMPOCXEvents_MediaErrorEvent e)
        {

            try
            // If the Player encounters a corrupt or missing ioItem,
            // show the hexadecimal error code and URL.
            {

                var errSource = e.pMediaObject as WMPLib.IWMPMedia2;
                if (errSource != null)
                {
                    WMPLib.IWMPErrorItem errorItem = errSource.Error;
                    Logger.Error("Error " + errorItem.errorCode.ToString("X") + " in " + errSource.sourceURL);

                    AnalyticsHelper.FireEvent("WMP Error - " + errorItem.errorCode.ToString("X"));
                    AnalyticsHelper.FireEvent("WMP Error - " + errorItem.errorCode.ToString("X") + " - " + Path.GetExtension(errSource.sourceURL));

                    try
                    {
                        FileInfo fileInfo = new FileInfo(errSource.sourceURL);
                        UploadMediaInfo(fileInfo, errorItem.errorCode.ToString("X"));
                    }
                    catch (Exception ex)
                    {
                        Logger.Error("Error creating FileInfo for " + errSource.sourceURL + ": " + ex);
                    }

                    if (PlayerError != null)
                    {
                        var playerErrorEventArgs = new PlayerErrorEventArgs();

                        if (errorItem.errorCode.ToString("X") == "C00D1199")
                        {
                            playerErrorEventArgs.ErrorMessage = "Ah bummer! We had trouble playing your video. This is most likely due to a missing codec." + Environment.NewLine + Environment.NewLine +
                                "The easiest fix is to download the free K-Lite Codec pack. Would you like to do that now?";
                            playerErrorEventArgs.SupportID = "WMPErrorC00D1199";
                        }
                        else
                        {
                            playerErrorEventArgs.ErrorMessage = "Error loading video! Please make sure you can play this video in Windows Media Player." + Environment.NewLine + Environment.NewLine +
                                "Error code: " + errorItem.errorCode.ToString("X") + Environment.NewLine + Environment.NewLine +
                                "Would you like to open a support ticket?";
                            playerErrorEventArgs.SupportID = "ErrorLoadingVideo";
                        }

                        PlayerError(sender, playerErrorEventArgs);
                    }

                }
                else
                    Logger.Error("Unknown error!");


            }
            catch (InvalidCastException ex)
            // In case pMediaObject is not an IWMPMedia item.
            {
                Logger.Error("Exception thrown: " + ex);
            }


        }

        private void wmPlayer_OpenStateChange(object sender, _WMPOCXEvents_OpenStateChangeEvent e)
        {
            var openStateEventArgs = new OpenStateChangedEventArgs();

            string friendlyState;
            switch ((WMPLib.WMPOpenState)e.newState)
            {
                case WMPLib.WMPOpenState.wmposUndefined: friendlyState = "Undefined"; break;
                case WMPLib.WMPOpenState.wmposPlaylistChanging: friendlyState = "PlaylistChanging"; break;
                case WMPLib.WMPOpenState.wmposPlaylistLocating: friendlyState = "PlaylistLocating"; break;
                case WMPLib.WMPOpenState.wmposPlaylistConnecting: friendlyState = "PlaylistConnecting"; break;
                case WMPLib.WMPOpenState.wmposPlaylistLoading: friendlyState = "PlaylistLoading"; break;
                case WMPLib.WMPOpenState.wmposPlaylistOpening: friendlyState = "PlaylistOpening"; break;
                case WMPLib.WMPOpenState.wmposPlaylistOpenNoMedia: friendlyState = "PlaylistOpenNoMedia"; break;
                case WMPLib.WMPOpenState.wmposPlaylistChanged: friendlyState = "PlaylistChanged"; break;
                case WMPLib.WMPOpenState.wmposMediaChanging: friendlyState = "MediaChanging"; break;
                case WMPLib.WMPOpenState.wmposMediaLocating: friendlyState = "MediaLocating"; break;
                case WMPLib.WMPOpenState.wmposMediaConnecting: friendlyState = "MediaConnecting"; break;
                case WMPLib.WMPOpenState.wmposMediaLoading: friendlyState = "MediaLoading"; break;
                case WMPLib.WMPOpenState.wmposMediaOpening: friendlyState = "MediaOpening"; break;
                case WMPLib.WMPOpenState.wmposMediaOpen: 
                    openStateEventArgs.OpenState = GenericPlayerControl.OpenStates.Open;
                    friendlyState = "MediaOpen"; 
                    break;
                case WMPLib.WMPOpenState.wmposBeginCodecAcquisition: friendlyState = "BeginCodecAcquisition"; break;
                case WMPLib.WMPOpenState.wmposEndCodecAcquisition: friendlyState = "EndCodecAcquisition"; break;
                case WMPLib.WMPOpenState.wmposBeginLicenseAcquisition: friendlyState = "BeginLicenseAcquisition"; break;
                case WMPLib.WMPOpenState.wmposEndLicenseAcquisition: friendlyState = "EndLicenseAcquisition"; break;
                case WMPLib.WMPOpenState.wmposBeginIndividualization: friendlyState = "BeginIndividualization"; break;
                case WMPLib.WMPOpenState.wmposEndIndividualization: friendlyState = "EndIndividualization"; break;
                case WMPLib.WMPOpenState.wmposMediaWaiting: friendlyState = "MediaWaiting"; break;
                case WMPLib.WMPOpenState.wmposOpeningUnknownURL: friendlyState = "OpeningUnknownURL"; break;
                default: friendlyState = "Unknown"; break;
            }

            Logger.Debug("OpenStateChanged: " + friendlyState);

            if (OpenStateChanged != null)
            {
                OpenStateChanged(sender, openStateEventArgs);
            }
        }

        private void wmPlayer_PlayStateChange(object sender, AxWMPLib._WMPOCXEvents_PlayStateChangeEvent e)
        {
            Logger.Debug("PlayStateChanged to " + GetFriendlyPlayState((WMPLib.WMPPlayState)e.newState));

            switch ((WMPLib.WMPPlayState)e.newState)
            {
                case WMPLib.WMPPlayState.wmppsPlaying:
                    PlayState = GenericPlayerControl.PlayStates.Playing;
                    break;
                case WMPLib.WMPPlayState.wmppsPaused:
                    PlayState = GenericPlayerControl.PlayStates.Paused;
                    break;
                case WMPLib.WMPPlayState.wmppsStopped:
                    PlayState = GenericPlayerControl.PlayStates.Stopped;
                    break;
                case WMPLib.WMPPlayState.wmppsReady:
                    PlayState = GenericPlayerControl.PlayStates.Ready;
                    break;
                case WMPLib.WMPPlayState.wmppsTransitioning:
                    PlayState = GenericPlayerControl.PlayStates.Transitioning;
                    break;
                case WMPLib.WMPPlayState.wmppsMediaEnded:
                    PlayState = GenericPlayerControl.PlayStates.Ended;
                    break;
            }

            if (PlayStateChanged != null)
                PlayStateChanged(sender, new PlayStateChangedEventArgs() { PlayState = PlayState } );
        }

        private void UploadMediaInfo(FileInfo fileInfo, string errorCode)
        {
            var uploadMediaInfoWorker = new UploadMediaInfoWorker(fileInfo, "WMP Error: " + errorCode);
            uploadMediaInfoWorker.RunWorkerAsync();
            // we don't care when this thing ends
        }

    }
}
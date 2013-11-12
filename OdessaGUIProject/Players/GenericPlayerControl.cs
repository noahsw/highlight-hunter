using System;
using System.Windows.Forms;
using AxWMPLib;
using NLog;
using System.Diagnostics;
using System.IO;
using GaDotNet.Common.Helpers;

namespace OdessaGUIProject.Players
{
    public partial class GenericPlayerControl : UserControl
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        internal PlayerTypes PlayerType;
        internal PlayStates PlayState;
        private QuickTimePlayerControl qtPlayer;
        private WindowsMediaPlayerControl wmPlayer;
        /// <summary>
        /// Needed for controlling loadingLabel
        /// </summary>
        private bool isClosing = false;

        private bool isDisposed = false;

        public GenericPlayerControl()
        {
            Logger.Debug("Initializing");

            PlayState = PlayStates.NoVideo;

            InitializeComponent();

        }

        public event EventHandler InitializationError;

        public event EventHandler<OpenStateChangedEventArgs> OpenStateChanged;

        public event EventHandler<PlayerErrorEventArgs> PlayerError;

        public event EventHandler<PlayStateChangedEventArgs> PlayStateChanged;

        public enum OpenStates
        {
            NoVideo,
            Open
        };

        public enum PlayerTypes
        {
            WindowsMediaPlayer,
            QuickTime
        }

        public enum PlayStates
        {
            NoVideo,
            Ready,
            Transitioning,
            Playing,
            Paused,
            Stopped,
            Ended
        };
        internal double CurrentPosition
        {
            get
            {
                if (wmPlayer != null)
                    return wmPlayer.CurrentPosition;
                else if (qtPlayer != null)
                    return qtPlayer.CurrentPosition;
                else
                {
                    Debug.Assert(false, "Both players are null!");
                    return 0;
                }
            }

            set
            {
                Logger.Debug("Setting CurrentPosition to " + value);
                if (wmPlayer != null)
                    wmPlayer.CurrentPosition = value;
                else if (qtPlayer != null)
                    qtPlayer.CurrentPosition = value;
                else
                {
                    Debug.Assert(false, "Both players are null!");
                }
            }
        }

        internal bool Mute
        {
            get
            {
                if (wmPlayer != null)
                    return wmPlayer.Mute;
                else if (qtPlayer != null)
                    return qtPlayer.Mute;
                else
                {
                    Debug.Assert(false, "Both players are null!");
                    return false;
                }
            }

            set
            {
                if (wmPlayer != null)
                    wmPlayer.Mute = value;
                else if (qtPlayer != null)
                    qtPlayer.Mute = value;
                else
                {
                    Debug.Assert(false, "Both players are null!");
                }
            }
        }

        internal string PlayerName
        {
            get
            {
                return "Windows Media Player";
            }
        }

        internal string URL
        {
            get
            {
                if (wmPlayer != null)
                    return wmPlayer.URL;
                else if (qtPlayer != null)
                    return qtPlayer.URL;
                else
                    return "";
            }
            set
            {
                Application.DoEvents(); // try to make things a little smoother

                string oldURL = URL;
                if (value != oldURL)
                {
                    Logger.Info("Switching input files. Deciding which player to use.");

                    string extension = Path.GetExtension(value).ToUpperInvariant();
                    if (QuickTimePlayerControl.SupportedFileExtensions.Contains(extension))
                    {
                        Logger.Info("Extension supported by QuickTime");

                        if (qtPlayer != null)
                            Logger.Info("QuickTime already loaded");

                        if (qtPlayer == null && InitializeQuickTimePlayerControl() == false)
                        {
                            Application.DoEvents(); // try to make things a little smoother

                            if (wmPlayer != null)
                                Logger.Info("WMP already loaded");

                            if (wmPlayer == null && InitializeWindowsMediaPlayerControl() == false)
                            {
                                Logger.Error("Both players failed");

                                // couldn't initialize either player.
                                if (InitializationError != null)
                                    InitializationError(null, EventArgs.Empty);
                            }
                        }

                    }
                    else
                    { // not supported by QuickTime. Use WMP.
                        Logger.Info("Not supported by QuickTime. Let's use WMP.");

                        if (wmPlayer != null)
                            Logger.Info("WMP already loaded");

                        if (wmPlayer == null && InitializeWindowsMediaPlayerControl() == false)
                        {
                            Logger.Error("Both players failed");

                            // couldn't initialize either player.
                            if (InitializationError != null)
                                InitializationError(null, EventArgs.Empty);
                            return;
                        }
                    }

                }
                else
                { // same URL
                    // don't do anything because player should already be initialized
                }

                Logger.Info("Setting URL to " + value);

                Debug.Assert(wmPlayer != null || qtPlayer != null, "Both players are null!");

                if (wmPlayer != null)
                    wmPlayer.URL = value;
                else if (qtPlayer != null)
                {
                    try
                    {
                        qtPlayer.URL = value;
                    }
                    catch (ArgumentException)
                    { // exception thrown which means player can't play this video. try other player
                        MainModel.isQuickTimeSupported = false;

                        DisposeQuickTimeControl();

                        Application.DoEvents(); // smooth things out
                        if (InitializeWindowsMediaPlayerControl())
                            URL = value; // use setter in case WMP can't play either
                    }
                }
            }
        }

        internal AxWindowsMediaPlayer WindowsMediaPlayer
        {
            get { return wmPlayer.WindowsMediaPlayer; }
        }

        internal void Pause()
        {
            Debug.Assert(wmPlayer != null || qtPlayer != null, "Both players are null!");

            if (wmPlayer != null)
                wmPlayer.Pause();
            else if (qtPlayer != null)
                qtPlayer.Pause();
        }

        internal void Play()
        {
            Logger.Debug("Called Play()");

            Debug.Assert(wmPlayer != null || qtPlayer != null, "Both players are null!");
            
            if (wmPlayer != null)
                wmPlayer.Play();
            else if (qtPlayer != null)
                qtPlayer.Play();
            
        }

        internal void Stop()
        {
            Debug.Assert(wmPlayer != null || qtPlayer != null, "Both players are null!");
            
            if (wmPlayer != null)
                wmPlayer.Stop();
            else if (qtPlayer != null)
                qtPlayer.Stop();
            
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

            isClosing = true;

            

            if (disposing)
            {
                DisposeWindowsMediaPlayerControl();

                DisposeQuickTimeControl();

                if (components != null)
                    components.Dispose();

            }

            isDisposed = true;

            GC.SuppressFinalize(this);
            
            base.Dispose(disposing);
        }

        private void GenericPlayerControl_Load(object sender, EventArgs e)
        {
            loadingLabel.Left = (this.Width / 2) - (loadingLabel.Width / 2);
            loadingLabel.Top = (this.Height / 2) - (loadingLabel.Height / 2);
        }

        private void handleOpenStateChanged(object sender, OpenStateChangedEventArgs e)
        {
            Logger.Info("OpenState: " + e.OpenState);

            if (OpenStateChanged != null)
                OpenStateChanged(sender, e);
        }

        private void handlePlayerError(object sender, PlayerErrorEventArgs e)
        {
            Logger.Info("PlayerError: " + e.ErrorMessage + " (" + e.SupportID + ")");

            if (PlayerError != null)
                PlayerError(sender, e);
        }

        private void handlePlayStateChanged(object sender, PlayStateChangedEventArgs e)
        {
            Logger.Info("PlayState: " + e.PlayState);

            this.PlayState = e.PlayState;

            if (e.PlayState == PlayStates.Playing || e.PlayState == PlayStates.Ready || e.PlayState == PlayStates.Stopped)
            {
                if (wmPlayer != null)
                    wmPlayer.Show();
                if (qtPlayer != null)
                    qtPlayer.Show();

                loadingLabel.Visible = false;
            }

            if (e.PlayState == PlayStates.Transitioning && !isClosing)
            {
                loadingLabel.Visible = true;

                if (wmPlayer != null)
                    wmPlayer.Hide();
                if (qtPlayer != null)
                    qtPlayer.Hide();
            }

            if (PlayStateChanged != null)
                PlayStateChanged(sender, e);
        }

        private bool InitializeQuickTimePlayerControl()
        {
            if (!MainModel.isQuickTimeSupported) // it failed last time so don't try initializing again this session
            {
                Logger.Debug("isQuickTimeSupported = false");
                return false;
            }

            try
            {
                DisposeWindowsMediaPlayerControl();

                qtPlayer = new QuickTimePlayerControl();

                qtPlayer.OpenStateChanged += new EventHandler<OpenStateChangedEventArgs>(handleOpenStateChanged);
                qtPlayer.PlayStateChanged += new EventHandler<PlayStateChangedEventArgs>(handlePlayStateChanged);
                qtPlayer.PlayerError += new EventHandler<PlayerErrorEventArgs>(handlePlayerError);

                qtPlayer.Visible = false;

                this.Controls.Add(qtPlayer);

                this.PlayerType = PlayerTypes.QuickTime;

                AnalyticsHelper.FireEvent("Each details - QuickTime installed");

                Logger.Info("success");

                return true;
            }
            catch (Exception ex)
            {
                Logger.Error("Error initializing: " + ex);

                MainModel.isQuickTimeInstalled = false;

                MainModel.isQuickTimeSupported = false;

                AnalyticsHelper.FireEvent("Each details - QuickTime not installed");
                
                return false;
            }
        }

        private bool InitializeWindowsMediaPlayerControl()
        {
            try
            {
                DisposeQuickTimeControl();

                wmPlayer = new WindowsMediaPlayerControl();

                wmPlayer.OpenStateChanged += new EventHandler<OpenStateChangedEventArgs>(this.handleOpenStateChanged);
                wmPlayer.PlayStateChanged += new EventHandler<PlayStateChangedEventArgs>(this.handlePlayStateChanged);
                wmPlayer.PlayerError += new System.EventHandler<PlayerErrorEventArgs>(this.handlePlayerError);

                wmPlayer.Visible = false;

                this.Controls.Add(wmPlayer);

                this.PlayerType = PlayerTypes.WindowsMediaPlayer;

                Logger.Info("success");

                return true;
            }
            catch (Exception ex)
            {
                Logger.Error("Error initializing: " + ex);

                return false;
            }
        }


        private void DisposeWindowsMediaPlayerControl()
        {
            if (wmPlayer != null)
            {
                wmPlayer.OpenStateChanged -= this.handleOpenStateChanged;
                wmPlayer.PlayStateChanged -= this.handlePlayStateChanged;
                wmPlayer.PlayerError -= this.handlePlayerError;
                wmPlayer.Dispose();
                wmPlayer = null;
            }
        }

        private void DisposeQuickTimeControl()
        {
            if (qtPlayer != null)
            {
                qtPlayer.OpenStateChanged -= this.handleOpenStateChanged;
                qtPlayer.PlayStateChanged -= this.handlePlayStateChanged;
                qtPlayer.PlayerError -= this.handlePlayerError;
                qtPlayer.Dispose();
                qtPlayer = null;
            }
        }

    }


    public class OpenStateChangedEventArgs : EventArgs
    {
        public GenericPlayerControl.OpenStates OpenState;
    }

    public class PlayerErrorEventArgs : EventArgs
    {
        public string ErrorMessage;
        public string SupportID;
    }
    public class PlayStateChangedEventArgs : EventArgs
    {
        public GenericPlayerControl.PlayStates PlayState;
    }
}
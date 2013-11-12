using System;
using System.ComponentModel;
using System.Threading;
using NLog;

namespace OdessaGUIProject
{
    internal class VideoPlayerSeekHelper
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public AxWMPLib.AxWindowsMediaPlayer mediaPlayer;

        //private bool wasPaused;
        private bool isWorking;

        private double targetPosition;
        //private bool forcePlay;

        private bool isCancelled;

        public event EventHandler DoneWorking;

        internal bool IsWorking
        {
            get { return isWorking; }
        }

        public void SeekRequest(double position)
        {
            //Debug.WriteLine("Seek request to " + position + ". isWorking: " + isWorking);

            if (isCancelled)
                return;

            targetPosition = position;
            //this.forcePlay = forcePlay;

            var diff = Math.Abs(mediaPlayer.Ctlcontrols.currentPosition - targetPosition);
            //Logger.Trace("Diff: " + diff);

            if (diff > 0.1) // at least tenth of a second difference
                WakeUpWorker();
        }

        /// <summary>
        /// Used so we don't try seeking when video player is not available
        /// </summary>
        internal void Cancel()
        {
            isCancelled = true;
        }

        private void initialPauseWorker_DoWork(object sender, DoWorkEventArgs e)
        { // if this is the first seek request, let's pause first and wait for the user to find their spot
            Thread.Sleep(500);
        }

        private void initialPauseWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            PerformSeek();
        }

        private void PerformSeek()
        {
            //wasPaused = !(mediaPlayer.playState == WMPLib.WMPPlayState.wmppsPlaying);

            if (isCancelled)
                return;
            
            var diff = Math.Abs(mediaPlayer.Ctlcontrols.currentPosition - targetPosition);
            if (diff > 0.1) // at least tenth of a second difference
            {
                Logger.Debug("Diff: " + diff);

                var seekWorker = new BackgroundWorker();
                seekWorker.DoWork += new DoWorkEventHandler(seekWorker_DoWork);
                seekWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(seekWorker_RunWorkerCompleted);
                seekWorker.RunWorkerAsync(targetPosition);
            }
            else
            {
                isWorking = false;
            }
        }

        private void seekWorker_DoWork(object sender, DoWorkEventArgs e)
        {

            if (isCancelled)
                return;

            var newPosition = (double)e.Argument;

            Logger.Debug("Performing seek to " + newPosition);

            //bool wasMuted = mediaPlayer.settings.mute;

            mediaPlayer.settings.mute = true;
            mediaPlayer.Ctlcontrols.currentPosition = newPosition;
            //if (wasPaused)
            //{
            mediaPlayer.Ctlcontrols.play();
            //}


            Thread.Sleep(500); // sleep some time so we seek
            // if we don't sleep long enough, a bunch of play requests queue up. when the user stops draggin, those play requests get fulfilled and the video jumps all over the place.




            //if (wasPaused)
            //mediaPlayer.Ctlcontrols.pause();

            /*
            // wait for seek to actually happen
             * 6/29/12: Why is this code REMED out?
            while (Math.Abs(mediaPlayer.Ctlcontrols.CurrentPosition - newCoordinate) > 1)
            {
                Application.DoEvents();
            }
             */

            /* 6/29/12: NOW we determine whether to resume on the MouseUp event of the tickboxes
            // go back to being paused if it was paused originally
            if (wasPaused && forcePlay == false)
            {
                mediaPlayer.Ctlcontrols.pause();
            }
             */

            //if (wasMuted == false)
            //mediaPlayer.settings.mute = false; // if we weren't muted before, let's unmute

            e.Result = newPosition;
        }

        private void seekWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            var setPosition = (double)e.Result;

            Logger.Debug("setPosition = " + setPosition);

            if (setPosition != targetPosition) // targetPosition changed while we were seeking
            {
                Logger.Debug("targetPosition changed while we were seeking. Seek again.");
                PerformSeek();
            }
            else
            {
                Logger.Debug("We reached targetPosition");
                isWorking = false;
                if (DoneWorking != null && !isCancelled)
                    DoneWorking(this, null);
            }
        }

        private void WakeUpWorker()
        {
            if (isWorking == false)
            { // only perform seek if we aren't working yet. if we're already working, we'll get to the target request soon
                isWorking = true;

                var initialPauseWorker = new BackgroundWorker();
                initialPauseWorker.DoWork += new DoWorkEventHandler(initialPauseWorker_DoWork);
                initialPauseWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(initialPauseWorker_RunWorkerCompleted);
                initialPauseWorker.RunWorkerAsync();
            }
        }
    }
}
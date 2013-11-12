using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Media;
using System.Reflection;
using System.Windows.Forms;
using GaDotNet.Common.Helpers;
using NLog;
using OdessaGUIProject.Properties;
using OdessaGUIProject.UI_Helpers;
using OdessaGUIProject.Workers;
using OdessaGUIProject.DRM_Helpers;

namespace OdessaGUIProject.UI_Controls
{
    public partial class ScanControl : UserControl
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        internal ScanWorkerHost scanWorkerHost;

        /// <summary>
        /// Reference to progress bar on MainForm. It doesn't work if on a UserControl
        /// </summary>
        internal Windows7ProgressBar windows7ProgressBar;

        private object progressLocker = new object();

        /// <summary>
        /// Used to keep track of how often we update the time remaining counter
        /// </summary>
        private int updateTimeRemainingCounter;

        public ScanControl()
        {
            InitializeComponent();

            this.BackColor = Color.Transparent; // so we can see what we're doing in the designer

            timeRemainingLabel.Text = "Calculating time remaining...";
        }

        private delegate void SetControlPropertyThreadSafeDelegate(Control control, string propertyName, object propertyValue);

        internal event EventHandler ScanCancelled;

        // initialized in RunScan() method
        internal event EventHandler ScanCompletedWithHighlights;

        public void RunScan()
        {
            scanWorkerHost = null; // put this here so we don't calculate progress from previous run

            // reset ScanWorkerResult so we don't incorrectly calculate progress
            foreach (var inputFileObject in MainModel.InputFileObjects)
                inputFileObject.ScanWorkerResult = ScanWorker.ScanWorkerResults.NotFinished;

            MainModel.IsScanning = true;

            MainModel.CaptureDurationInSeconds = Settings.Default.CaptureDurationInSeconds;
            MainModel.IgnoreEarlyHighlights = Settings.Default.IgnoreEarlyHighlights;
            MainModel.DetectionThreshold =
                (MainModel.DetectionThresholds)
                Settings.Default.HandThresholdOption;

            Logger.Info("Input file count: " + MainModel.InputFileObjects.Count);

            formProgressBar.Value = 0;
            cancelButton.Enabled = true;
            windows7ProgressBar.ShowInTaskbar = true;
            windows7ProgressBar.Value = 0;
            statusLabel.Text = "Starting...";
            timeRemainingLabel.Text = "Calculating time remaining...";

            #region track settings used

            AnalyticsHelper.FireEvent("Each scan - setting - sensitivity - " + Settings.Default.HandThresholdOption);

            AnalyticsHelper.FireEvent("Each scan - setting - capture duration",
                                      Settings.Default.CaptureDurationInSeconds);

            AnalyticsHelper.FireEvent("Each scan - setting - ignore early highlights",
                                      Convert.ToInt32(Settings.Default.IgnoreEarlyHighlights));

            var activationState = Protection.GetLicenseStatus();
            if (activationState == Protection.ActivationState.Activated)
                AnalyticsHelper.FireEvent("Each scan - activation state - activated");
            else if (activationState == Protection.ActivationState.Trial)
                AnalyticsHelper.FireEvent("Each scan - activation state - trial");
            else if (activationState == Protection.ActivationState.TrialExpired)
                AnalyticsHelper.FireEvent("Each scan - activation state - trial expired");
            else if (activationState == Protection.ActivationState.Unlicensed)
                AnalyticsHelper.FireEvent("Each scan - activation state - unlicensed");


            #endregion track settings used

            // track the fact that we're starting a scan
            AnalyticsHelper.FireEvent("Each scan - input file count", MainModel.InputFileObjects.Count);

            AnalyticsHelper.FireEvent("Scan", MainModel.InputFileObjects.Count);


            scanWorkerHost = new ScanWorkerHost();
            scanWorkerHost.StatusChanged += new ScanWorkerHost.StatusChangedEventHandler(scanWorkerHost_StatusChanged);
            scanWorkerHost.RunWorkerCompleted += scanWorkerHost_RunWorkerCompleted;
            scanWorkerHost.RunWorkerAsync();

            updateProgressTimer.Enabled = true; // this must be after scanWorkerHost is initialized
        }

        public void UpdateProgress(int progress)
        {
            SetControlPropertyThreadSafe(formProgressBar, "Value", progress);
            SetControlPropertyThreadSafe(windows7ProgressBar, "Value", progress);
        }

        public void UpdateTimeRemaining(string timeRemaining)
        {
            SetControlPropertyThreadSafe(timeRemainingLabel, "Text", timeRemaining);
        }

        /// <summary>
        /// This method is used to update control properties from background threads
        /// </summary>
        /// <param name="control"></param>
        /// <param name="propertyName"></param>
        /// <param name="propertyValue"></param>
        private static void SetControlPropertyThreadSafe(Control control, string propertyName, object propertyValue)
        {
            if (control.InvokeRequired)
            {
                control.Invoke(new SetControlPropertyThreadSafeDelegate(SetControlPropertyThreadSafe), new object[] { control, propertyName, propertyValue });
            }
            else
            {
                control.GetType().InvokeMember(propertyName, BindingFlags.SetProperty, null, control, new object[] { propertyValue }, CultureInfo.CurrentCulture);
            }
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            if (scanWorkerHost != null)
                scanWorkerHost.CancelAsync();

            if (ScanCancelled != null)
                ScanCancelled(sender, e);
        }

        private string GetTimeSavingsMessage(double scanVideoDurationInSeconds)
        {
            Logger.Info("called with " + scanVideoDurationInSeconds);

            double timeSavedInSeconds = scanVideoDurationInSeconds / 2;
            // we assume we save half the duration of the video

            if (timeSavedInSeconds < 60 * 10)
            {
                // if time saved is less than 10 minutes, don't message them
                return "";
            }

            //int seconds = secondsLeft % 60;
            var div = (int)(timeSavedInSeconds / 60);
            int minutes = div % 60;
            if (minutes % 5 != 0)
                minutes = ((minutes + 5) / 10) * 10; // round to nearest 5
            var hours = (int)(timeSavedInSeconds / 3600);

            string ret = "";
            if (hours > 0)
            {
                if (minutes > 0)
                {
                    if (hours == 1)
                        ret += "1 hour and ";
                    else
                        ret += hours + " hours and ";
                    ret += minutes;
                    ret += " minutes";
                }
                else
                {
                    if (hours == 1)
                        ret += "1 hour";
                    else
                        ret += hours + " hours";
                }
            }
            else if (minutes > 0)
            {
                ret += minutes;
                ret += " minutes";
            }
            else
            {
                // we should never get here but just in case
                return "";
            }

            return ret;
        }

        private void ScanControl_Load(object sender, EventArgs e)
        {
            DesignLanguage.ApplyCustomFont(this.Controls);
        }

        private void scanWorkerHost_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            updateProgressTimer.Enabled = false;

            windows7ProgressBar.ShowInTaskbar = false;

            cancelButton.Enabled = false;
            MainModel.IsScanning = false;

            if (scanWorkerHost.IsCancelled)
            {
                ScanCancelled(sender, e);
            }
            else
            {
                SystemSounds.Exclamation.Play();
                FlashWindow.Flash(this.ParentForm);

                ScanCompletedWithHighlights(sender, e);
            }
        }

        private void scanWorkerHost_StatusChanged(object sender, DoWorkEventArgs e)
        {
            Logger.Debug(CultureInfo.InvariantCulture, "Status changed to " + e.Result);
            UpdateStatus((string)e.Result);
        }

        private void updateProgressTimer_Tick(object sender, EventArgs e)
        {
            lock (progressLocker)
            {
                // we only update the time remaining every 5 seconds, which is every 10 times this routine is called
                if (updateTimeRemainingCounter == 0)
                    UpdateTimeRemaining(scanWorkerHost.GetTimeRemaining());

                updateTimeRemainingCounter = (updateTimeRemainingCounter + 1) % 10;

                int progressValue = 0;
                if (scanWorkerHost.GetProgressValue(ref progressValue))
                {
                    UpdateProgress(progressValue);
                }
            }
        }

        private void UpdateStatus(string status)
        {
            SetControlPropertyThreadSafe(statusLabel, "Text", status);
        }

        internal bool ShouldContinueScanning()
        {
            if (!MainModel.IsScanning)
                return false;

            if (MessageBox.Show(
                "Highlight Hunter is busy working for you." + Environment.NewLine + Environment.NewLine +
                "Would you like to stop the scan?",
                "Stop scanning?",
                MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
            {
                if (scanWorkerHost != null && scanWorkerHost.IsBusy)
                    scanWorkerHost.CancelAsync();

                return false;
            }

            return true;
        }
    }
}
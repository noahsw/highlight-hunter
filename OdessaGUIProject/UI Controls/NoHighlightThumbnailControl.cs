using System;
using System.Windows.Forms;
using OdessaGUIProject.UI_Helpers;
using OdessaGUIProject.Workers;

namespace OdessaGUIProject.UI_Controls
{
    partial class NoHighlightThumbnailControl : UserControl
    {
        private InputFileObject inputFileObject;

        internal NoHighlightThumbnailControl()
        {
            InitializeComponent();

            DesignLanguage.ApplyCustomFont(this.Controls);
        }

        internal event EventHandler RescanRequested;

        internal InputFileObject InputFileObject
        {
            get { return inputFileObject; }
            set
            {
                inputFileObject = value;

                switch (inputFileObject.ScanWorkerResult)
                {
                    case ScanWorker.ScanWorkerResults.UnableToDetermineBitrate:
                    case ScanWorker.ScanWorkerResults.UnableToDetermineDimensions:
                    case ScanWorker.ScanWorkerResults.UnableToDetermineFramesPerSecond:
                    case ScanWorker.ScanWorkerResults.UnableToDetermineVideoDuration:
                    case ScanWorker.ScanWorkerResults.UnableToScan:
                        statusLabel.Text = "Error while scanning: " + ScanWorker.GetFriendlyScanResult(inputFileObject.ScanWorkerResult) + "." + Environment.NewLine +
                            "Make sure you scan the actual video files that came *straight off your camera*.";
                        openSupportLinkLabel.Visible = true;
                        break;

                    case ScanWorker.ScanWorkerResults.Cancelled:
                        statusLabel.Text = "Cancelled by user";
                        break;

                    case ScanWorker.ScanWorkerResults.Success: // no highlights found
                        if ((int)MainModel.DetectionThreshold < (int)MainModel.DetectionThresholds.Loosest ||
                            MainModel.IgnoreEarlyHighlights)
                        { // user is not on loosest setting so let's suggest they do it
                            rescanLinkLabel.Visible = true;
                        }
                        break;
                }
            }
        }

        private void NoHighlightThumbnailControl_Load(object sender, EventArgs e)
        {
        }

        private void openSupportLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            BrowserHelper.LaunchBrowser(
                                "http://support.highlighthunter.com/customer/portal/articles/307210-understanding-scan-errors",
                                "scanerror");
        }

        private void rescanLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (MainModel.HighlightObjects.Count > 0)
            {
                if (MessageBox.Show("Proceeding will clear all existing highlights." + Environment.NewLine + Environment.NewLine +
                    "Are you sure you want to continue?", "Clear existing highlights?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                {
                    // we should blow them away because a user might want to rescan with different sensitivity
                    MainModel.HighlightObjects.Clear();
                }
                else
                    return;
            }

            // save new setting and retry scan
            Properties.Settings.Default.IgnoreEarlyHighlights = false;
            Properties.Settings.Default.HandThresholdOption = (int)MainModel.DetectionThresholds.Loosest;
            Properties.Settings.Default.Save();
            MainModel.DetectionThreshold = MainModel.DetectionThresholds.Loosest;

            if (RescanRequested != null)
                RescanRequested(sender, e);
        }
    }
}
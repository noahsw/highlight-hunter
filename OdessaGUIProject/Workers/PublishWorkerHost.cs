using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using NLog;
using OdessaGUIProject.DRM_Helpers;

namespace OdessaGUIProject.Workers
{
    internal class PublishWorkerHost : BackgroundWorker
    {

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Keeps track of all the ScanWorkers that are running scans
        /// </summary>
        internal List<PublishWorker> ActivePublishWorkers = new List<PublishWorker>();

        /// <summary>
        /// Keeps track of all the ScanWorkers that are completed
        /// </summary>
        internal List<PublishWorker> CompletedPublishWorkers = new List<PublishWorker>();

        private string outputDirectoryPath;

        private double totalProgressUnits = 0;
        private double bankedProgressUnits = 0; // the progress units from completed workers

        internal delegate void StatusChangedEventHandler(object sender, DoWorkEventArgs e);
        internal event StatusChangedEventHandler StatusChanged;

        internal bool IsCancelled { get; set; } // We use this instead of e.Cancel so that we can read e.Result


        internal PublishWorkerHost(string outputDirectoryPath)
        {
            DoWork += new DoWorkEventHandler(SaveWorkerHost_DoWork);
            WorkerReportsProgress = true;
            WorkerSupportsCancellation = true;

            this.outputDirectoryPath = outputDirectoryPath;
        }

        private void SaveWorkerHost_DoWork(object sender, DoWorkEventArgs e)
        {
            Logger.Info("Started");

            var statusChangedEventArgs = new DoWorkEventArgs(null);
            e.Result = "Starting...";
            StatusChanged(sender, statusChangedEventArgs);

            ActivePublishWorkers.Clear();

            foreach (var publishWorker in CompletedPublishWorkers)
            {
                publishWorker.Dispose();
            }
            CompletedPublishWorkers.Clear();

            //totalSecondsOfOutput = CalculateTotalSecondsOfOutput();
            totalProgressUnits = CalculateTotalProgressUnits();

            foreach (var highlightObject in MainModel.HighlightObjects)
            {
                if (CancellationPending)
                    break;

                if (highlightObject.SaveToDisk)
                {
                    var saveWorker = new SaveWorker(highlightObject);
                    saveWorker.StatusChanged += publishWorker_StatusChanged;
                    saveWorker.ProgressChanged += publishWorker_ProgressChanged;
                    RunWorker(saveWorker); // runs syncronously
                }

                if (highlightObject.ShareToFacebook)
                {
                    var facebookShareWorker = new FacebookShareWorker(highlightObject);
                    facebookShareWorker.StatusChanged += publishWorker_StatusChanged;
                    facebookShareWorker.ProgressChanged += publishWorker_ProgressChanged;
                    RunWorker(facebookShareWorker); // runs syncronously
                }

            }

            // wait for ActiveScanWorkers to clean up

            IsCancelled = CancellationPending; // we use this instead of e.Cancel because if we set e.Cancel, we can't read e.Result (http://bytes.com/topic/c-sharp/answers/519073-asynch-crash-when-e-cancel-set)
            e.Result = CompletedPublishWorkers;


        }

        void publishWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            double nonbankedProgressUnits = e.ProgressPercentage * ((PublishWorker)sender).TotalProgressUnits / 100.0;
            int newProgress = (int)(100 * (bankedProgressUnits + nonbankedProgressUnits) / totalProgressUnits);

            ReportProgress(newProgress);
        }

        void publishWorker_StatusChanged(object sender, DoWorkEventArgs e)
        {
            StatusChanged(this, e);
        }


        private void RunWorker(PublishWorker publishWorker)
        {

            ActivePublishWorkers.Add(publishWorker);

            try
            {
                publishWorker.RunWorkerAsync(outputDirectoryPath);
                while (publishWorker.IsBusy)
                { // wait for the scanworker to finish
                    if (CancellationPending)
                        publishWorker.CancelAsync();

                    System.Threading.Thread.Sleep(500);

                }
            }
            catch (Exception ex)
            {
                Logger.Error("Error starting PublishWorker! {0}", ex.ToString());
            }

            Logger.Info("PublishWorker[" + publishWorker.Id + "] ended with result " + publishWorker.PublishWorkerResult.ToString());
            ActivePublishWorkers.RemoveAt(0);
            CompletedPublishWorkers.Add(publishWorker);
            bankedProgressUnits += publishWorker.TotalProgressUnits;

        }


        internal void CancelPublish() // TODO: not referenced yet
        {
            Logger.Info("Asked to cancel");
            foreach (var sw in ActivePublishWorkers)
            {
                if (sw != null)
                {
                    sw.CancelAsync(); // this enough to cancel
                }
            }

            CancelAsync();
        }


        /// <summary>
        /// This is used to show progress bar
        /// </summary>
        /// <returns></returns>
        private double CalculateTotalSecondsOfOutput()
        {
            double ret = 0;
            foreach (var highlightObject in MainModel.HighlightObjects)
            {
                ret += (highlightObject.EndTime - highlightObject.StartTime).TotalSeconds;
            }
            return ret;
        }


        private double CalculateTotalProgressUnits()
        {
            double shareToSaveRatio = GetShareToSaveRatio();

            double ret = 0;
            foreach (var highlightObject in MainModel.HighlightObjects)
            {
                double fileFraction = (highlightObject.EndTime - highlightObject.StartTime).TotalSeconds / highlightObject.InputFileObject.VideoDurationInSeconds;
                double publishedKBytes = highlightObject.InputFileObject.SourceFileInfo.Length / 1024 * fileFraction;

                if (highlightObject.SaveToDisk)
                    ret += publishedKBytes;

                if (highlightObject.ShareToFacebook)
                    ret += publishedKBytes * shareToSaveRatio;
            }
            return ret;
        }


        internal static double GetShareToSaveRatio()
        {
            // this is how much progress should be given to every byte saved vs. every byte uploaded
            // this is dependent on license status because watermarking is much slower
            var activationState = Protection.GetLicenseStatus();
            if (activationState == Protection.ActivationState.Activated || activationState == Protection.ActivationState.Trial)
                return 90;
            else
                return 80;
        }

    }
}

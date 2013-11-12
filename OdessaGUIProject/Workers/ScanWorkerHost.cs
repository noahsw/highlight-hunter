using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using GaDotNet.Common.Helpers;
using NLog;
using OdessaGUIProject.Workers;
using System.IO;

namespace OdessaGUIProject
{
    internal class ScanWorkerHost : BackgroundWorker
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Keeps track of the InputFileObject that's currently being scanned
        /// </summary>
        internal InputFileObject ActiveInputFileObject;

        /// <summary>
        /// How many ScanWorkers have finished. So we can show this in status.
        /// </summary>
        internal int CompletedScanWorkerCount = 0;

        /// <summary>
        /// How many KBytes we've processed
        /// </summary>
        internal long InputFilesCompletedInKBytes;

        /// <summary>
        /// The total file size of all input files
        /// </summary>
        internal long InputFilesTotalSizeInKBytes;

        /// <summary>
        /// The starting time of the scan
        /// </summary>
        internal DateTime ScanStartTime;

        internal ScanWorkerHost()
        {
            DoWork += ScanWorkerHost_DoWork;
            WorkerReportsProgress = true;
            WorkerSupportsCancellation = true;
            IsCancelled = false;
        }

        internal delegate void StatusChangedEventHandler(object sender, DoWorkEventArgs e);

        internal event StatusChangedEventHandler StatusChanged;

        internal bool IsCancelled { get; set; }

        /// <summary>
        /// During the scan, return the progress
        /// </summary>
        /// <returns>True if progress should be updated</returns>
        internal bool GetProgressValue(ref int value)
        {
            if (ActiveInputFileObject == null || InputFilesTotalSizeInKBytes == 0)
                return false;

            InputFilesCompletedInKBytes = 0;
            foreach (var inputFileObject in MainModel.InputFileObjects)
            {
                if (inputFileObject.ScanWorkerResult == ScanWorker.ScanWorkerResults.NotFinished ||
                    inputFileObject == ActiveInputFileObject)
                    continue;

                InputFilesCompletedInKBytes += inputFileObject.SourceFileInfo.Length / 1024;
            }

            long scanWorkersCompletedInKBytes = 0;
            if (ActiveInputFileObject != null)
            {
                if (ActiveInputFileObject.TotalFrames > 0)
                {
                    scanWorkersCompletedInKBytes += (long)
                                                    (ActiveInputFileObject.ScanWorker.ProcessedFrames / (double)ActiveInputFileObject.TotalFrames *
                                                     (ActiveInputFileObject.SourceFileInfo.Length / 1024d));
                    //Console.WriteLine("progress of " + scanWorker.inputFile.Name);
                    //Console.WriteLine(NativeOdessaMethods.GetFramesProcessed() + " of " + NativeOdessaMethods.GetTotalFrames());
                    //Console.WriteLine("duration: " + scanWorker.inputFile.Length / 1024);
                }
                else
                {
                    Console.WriteLine("TotalFrames = 0!");
                }
            }

            //Console.WriteLine("already completed: " + MainModel.InputFilesCompletedInKBytes);
            //Console.WriteLine("this completed: " + ScanWorkersCompletedInKBytes);

            long totalKBytesProcessed = InputFilesCompletedInKBytes + scanWorkersCompletedInKBytes;

            if (totalKBytesProcessed > 0)
            {
                value = (int)(totalKBytesProcessed * 100 / InputFilesTotalSizeInKBytes);
                if (value > 100) // to prevent errors
                {
                    Debug.Assert(true, "Somehow progress value is greater than 100");
                    value = 100;
                }
                //Console.WriteLine("progress: " + value + " (" + TotalKBytesProcessed + " / " + MainModel.InputFilesTotalSizeInKBytes + ")");
                return true;
            }
            value = 0;
            return true;
        }

        /// <summary>
        /// Calculates the time remaining in the scan
        /// </summary>
        /// <returns></returns>
        internal string GetTimeRemaining()
        {
            long elapsedTimeInSeconds = Convert.ToInt64((DateTime.Now - ScanStartTime).TotalSeconds);

            if (elapsedTimeInSeconds < 10)
                // our estimate won't be accurate for several seconds so let's not even display it
                return "Calculating time remaining...";

            int currentProgressValue = 0;

            if (GetProgressValue(ref currentProgressValue) == false)
                return "";

            if (currentProgressValue > 0)
            {
                long totalEstimatedTimeInSeconds = elapsedTimeInSeconds * 100 / currentProgressValue;

                long timeRemainingInSeconds = totalEstimatedTimeInSeconds - elapsedTimeInSeconds;

                var ts = TimeSpan.FromSeconds(timeRemainingInSeconds);

                //logger.Info("Elapsed seconds: " + ElapsedTimeInSeconds + ", TotalKBytesProcessed: " + TotalKBytesProcessed.ToString("0,000,000") +
                //    ", InputFilesTotalSizeInKBytes: " + MainModel.InputFilesTotalSizeInKBytes.ToString("0,000,000"));

                if (ts.Hours > 0)
                    return ts.Hours + "h " + ts.Minutes + "m left";
                if (ts.Minutes > 0)
                    return ts.Minutes + "m left";
                return "<1m left";
            }
            return "Calculating time remaining...";
        }

        /// <summary>
        /// Calculates the total file size of all the input files.
        /// Used to calculate progress.
        /// </summary>
        /// <returns></returns>
        private static long CalculateInputFilesTotalSize()
        {
            long ret = 0;
            foreach (var inputFileObject in MainModel.InputFileObjects)
            {
                ret += (inputFileObject.SourceFileInfo.Length / 1024);
            }
            return ret;
        }

        private void ReportStatusChanged()
        {
            string status;
            if (ActiveInputFileObject != null)
            {
                status = "Scanning " + Path.GetFileNameWithoutExtension(ActiveInputFileObject.SourceFileInfo.Name) + " (" + (CompletedScanWorkerCount + 1) + " of " +
                           MainModel.InputFileObjects.Count + ")";
            }
            else
                status = "Starting...";

            var e = new DoWorkEventArgs(null);
            e.Result = status;
            StatusChanged(this, e);
        }

        private void scanWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            // trigger ScanWorkerHost to fire its progressUpdate event
            ReportProgress(0);
        }

        private void ScanWorkerHost_DoWork(object sender, DoWorkEventArgs e)
        {
            // Authorization check happens in ScanWorker

            Logger.Info("ScanWorkerHost started");

            InputFilesTotalSizeInKBytes = CalculateInputFilesTotalSize();
            InputFilesCompletedInKBytes = 0;
            ScanStartTime = DateTime.Now;

            MainModel.HighlightObjects.Clear();

            ActiveInputFileObject = null;


            Logger.Info("DetectionThreshold: " + MainModel.DetectionThreshold.ToString());

            foreach (var inputFileObject in MainModel.InputFileObjects)
            {
                if (CancellationPending)
                    break;

                using (inputFileObject.ScanWorker = new ScanWorker(inputFileObject))
                {
                    inputFileObject.ScanWorker.UseCaptureOffset = MainModel.UseCaptureOffset;
                    ActiveInputFileObject = inputFileObject;

                    inputFileObject.ScanWorker.ProgressChanged += scanWorker_ProgressChanged;

                    ReportStatusChanged();

                    try
                    {
                        inputFileObject.ScanWorker.RunWorkerAsync();
                        while (inputFileObject.ScanWorker.IsBusy)
                        { // wait for the scanworker to finish
                            if (CancellationPending)
                                inputFileObject.ScanWorker.CancelScan();

                            System.Threading.Thread.Sleep(500);
                        }
                        inputFileObject.ScanWorkerResult = inputFileObject.ScanWorker.ScanWorkerResult;
                    }
                    catch (Exception ex)
                    {
                        Logger.Error("Error starting ScanWorker! " + ex.ToString());
                    }
                }

                CompletedScanWorkerCount++;
            }

            ActiveInputFileObject = null;

            Logger.Info("Highlights found: " + MainModel.HighlightObjects.Count);

            TrackAnalytics();

            IsCancelled = CancellationPending; // we use this instead of e.Cancel because if we set e.Cancel, we can't read e.Result (http://bytes.com/topic/c-sharp/answers/519073-asynch-crash-when-e-cancel-set)

            if (!IsCancelled)
            { // in case we scan really fast, wait a few seconds so the user sees what happens
                var minScanTime = TimeSpan.FromSeconds(4);
                while (DateTime.Now.Subtract(ScanStartTime) < minScanTime)
                    Thread.Sleep(200);
            }
        }

        private void TrackAnalytics()
        {

            TimeSpan totalScanDuration = DateTime.Now - ScanStartTime;
            double totalVideoDurationInSeconds = 0;
            if (totalScanDuration.TotalSeconds > 0)
            {
                var sampleVideoPath = MainModel.GetPathToSampleVideo();
                var areRealVideosIncluded = false; 
                var isSampleVideoIncluded = false;

                foreach (var inputFileObject in MainModel.InputFileObjects)
                {
                    var scanWorker = inputFileObject.ScanWorker;
                    if (scanWorker == null)
                        continue;

                    if (scanWorker.ScanWorkerResult != ScanWorker.ScanWorkerResults.Success)
                        continue; // we only care about good scans

                    if (scanWorker.InputFileObject.SourceFileInfo.FullName == sampleVideoPath)
                        isSampleVideoIncluded = true;
                    else
                        areRealVideosIncluded = true;

                    totalVideoDurationInSeconds += scanWorker.InputFileObject.VideoDurationInSeconds;
                }

                if (totalVideoDurationInSeconds > 0 && areRealVideosIncluded)
                {
                    AnalyticsHelper.FireEvent("Each scan - highlights found", MainModel.HighlightObjects.Count);

                    var scanMultiplier = (int)Math.Round(totalVideoDurationInSeconds / totalScanDuration.TotalSeconds, 0);
                    Logger.Info("Average scan multiplier = " + scanMultiplier + "x");

                    AnalyticsHelper.FireEvent("Each scan - scan multiplier", scanMultiplier);

                    AnalyticsHelper.FireEvent("Each scan - video duration in minutes",
                                              (int)Math.Round(totalVideoDurationInSeconds / 60, 0));
                }

                if (isSampleVideoIncluded && Properties.Settings.Default.HasScannedSampleVideo == false)
                {
                    if (AnalyticsHelper.FireEvent("First scan sample video"))
                    {
                        Properties.Settings.Default.HasScannedSampleVideo = true;
                        Properties.Settings.Default.Save();
                    }
                }
            }
        }
    }
}
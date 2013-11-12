using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using NLog;
using OdessaGUIProject.DRM_Helpers;

namespace OdessaGUIProject.Workers
{
    internal abstract class PublishWorker : BackgroundWorker
    {
        #region PublishWorkerResults enum

        internal enum PublishWorkerResults
        {
            NotFinished,
            UnableToSplice,
            UnableToShare,
            UnableToAuthenticate,
            Success,
            Cancelled,
        }

        #endregion PublishWorkerResults enum

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Stored error message so user can get more information
        /// </summary>
        internal string ErrorMessage;

        internal HighlightObject HighlightObject;

        /// <summary>
        /// A reference to the output file from this PublishWorker
        /// </summary>
        internal FileInfo OutputFileInfo;

        protected internal Protection.ActivationState activationState { get; set; }

        internal PublishWorker(HighlightObject highlightObject)
        {
            HighlightObject = highlightObject;

            Id = DateTime.Now.Ticks; //  (new Random()).Next();

            Logger.Info("Starting new " + this.GetType().Name + "[" + Id + "] on " + highlightObject.InputFileObject.SourceFileInfo.FullName);

#if DEBUG
            Logger.Info("At bookmarkTime " + highlightObject.BookmarkTime.TotalSeconds);
#endif

            // do this in main thread
            activationState = Protection.GetLicenseStatus(true);

            WorkerReportsProgress = true;
            WorkerSupportsCancellation = true;

            DoWork += new DoWorkEventHandler(PublishWorker_DoWork);

            PublishWorkerResult = PublishWorkerResults.NotFinished;
        }

        internal enum PublishWorkerTypes
        {
            Facebook,
            Save
        }

        /// <summary>
        /// This holds the ID # of the PublishWorker
        /// </summary>
        internal long Id { get; private set; }

        /// <summary>
        /// So if we connect to a PublishWorker that's in progress, we can immediately get the progress
        /// BackgroundWorker doesn't support this so we're adding it ourselves
        /// </summary>
        internal int Progress { get; set; }

        internal PublishWorkerResults PublishWorkerResult { get; set; }

        internal double TotalProgressUnits { get; set; }

        internal bool CheckForCancelledWorker()
        {
            if (CancellationPending)
            {
                PublishWorkerResult = PublishWorkerResults.Cancelled; // don't use e.Cancel because of http://bytes.com/topic/c-sharp/answers/519073-asynch-crash-when-e-cancel-set
                Logger.Info(this.GetType().Name + "[" + Id + "]: Cancelled!");
                return true;
            }
            return false;
        }

        internal abstract void PublishWorker_DoWork(object sender, DoWorkEventArgs e);

        internal new void ReportProgress(int percentProgress)
        {
            Debug.Assert(percentProgress <= 100, "Progress is greater than 100!");
            if (percentProgress > 100)
                percentProgress = 100;

            Progress = percentProgress;

            if (IsBusy == false) // try to avoid exceptions
                return;

            try
            {
                base.ReportProgress(Progress);
            }
            catch (InvalidOperationException ex) 
            {
                // this will throw exception if work has already completed. let's do this to be safe.
                Logger.Debug("We can safely ignore InvalidOperationException: " + ex);
            } 
        }

        internal abstract bool ViewResult(ref string errorMessage);

        internal string GetFriendlyPublishWorkerResult()
        {
            switch (PublishWorkerResult)
            {
                case PublishWorkerResults.Cancelled:
                    return "Cancelled";
                case PublishWorkerResults.NotFinished:
                    return "Not finished";
                case PublishWorkerResults.Success:
                    return "Success";
                case PublishWorkerResults.UnableToAuthenticate:
                    return "Unable to Authenticate";
                case PublishWorkerResults.UnableToShare:
                    return "Unable to Share";
                case PublishWorkerResults.UnableToSplice:
                    return "Unable to Splice";
                default:
                    Debug.Assert(true, "Unaccounted for case!");
                    return "";
            }
        }
    }
}
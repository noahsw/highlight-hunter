using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Timers;
using GaDotNet.Common.Helpers;
using NLog;
using OdessaGUIProject.Properties;
using System.IO;

namespace OdessaGUIProject.Workers
{
    internal class ScanWorker : BackgroundWorker
    {
        #region ScanWorkerResults enum

        public enum ScanWorkerResults
        {
            NotFinished,
            UnableToDetermineVideoDuration,
            UnableToDetermineFramesPerSecond,
            UnableToDetermineBitrate,
            UnableToDetermineDimensions,
            UnableToScan,
            NotAuthorized,
            Success,
            Cancelled,
        }

        #endregion ScanWorkerResults enum

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        internal bool UseCaptureOffset = true;

        internal ScanWorker(InputFileObject inputFileObject)
        {
            Id = DateTime.Now.Ticks;
            Logger.Info("Starting new ScanWorker[" + Id + "] on " + inputFileObject.SourceFileInfo.FullName);

            DoWork += ScanWorkerDoWork;
            RunWorkerCompleted += new RunWorkerCompletedEventHandler(ScanWorker_RunWorkerCompleted);
            WorkerReportsProgress = true;
            WorkerSupportsCancellation = true;

            ScanWorkerResult = ScanWorkerResults.NotFinished;

            InputFileObject = inputFileObject;
        }

        /// <summary>
        /// This holds the ID # of the ScanWorker
        /// </summary>
        internal long Id { get; private set; }

        internal InputFileObject InputFileObject { get; set; }

        internal long ProcessedFrames { get; private set; }

        internal DateTime ScanStart { get; set; }

        internal DateTime ScanStop { get; set; }

        internal ScanWorkerResults ScanWorkerResult { get; private set; }

        internal int TotalHighlightsFound { get; private set; }

        private NativeOdessaMethods.OdessaReturnCodes OdessaReturnCode { get; set; }

        public static string GetFriendlyScanResult(ScanWorkerResults scanWorkerResult)
        {
            switch (scanWorkerResult)
            {
                case ScanWorkerResults.Success:
                    return "Success";
                case ScanWorkerResults.UnableToDetermineBitrate:
                case ScanWorkerResults.UnableToDetermineDimensions:
                case ScanWorkerResults.UnableToDetermineFramesPerSecond:
                case ScanWorkerResults.UnableToDetermineVideoDuration:
                case ScanWorkerResults.UnableToScan:
                    return "Unable to scan video for highlights";
                default:
                    return scanWorkerResult.ToString();
            }
        }

        internal static void TrackFirstHighlightFound()
        {
            if (Settings.Default.FirstHighlightFound == false)
            {
                if (AnalyticsHelper.FireEvent("First highlight found"))
                {
                    Settings.Default.FirstHighlightFound = true;
                    Settings.Default.Save();
                }
            }
        }

        internal static void TrackTotalVideosScanned()
        {
            Settings.Default.TotalVideosScanned += 1;
            Settings.Default.Save();

            AnalyticsHelper.FireEvent("Total videos scanned - " + Settings.Default.TotalVideosScanned);

            if (Settings.Default.TotalVideosScanned == 1)
                AnalyticsHelper.FireEvent("First scan");
        }

        /// <summary>
        /// Use this instead so we can break out of NativeOdessaMethods.Scan() call
        /// </summary>
        internal void CancelScan()
        {
            CancelAsync();
            NativeOdessaMethods.CancelScan(); // tells scan method to break
        }

        /// <summary>
        /// Collapse nearby frame locations into single frames
        /// </summary>
        /// <param name="darkFrameNumbers"></param>
        /// <param name="captureDurationInSeconds"> </param>
        /// <param name="ignoreEarlyHighlights"> </param>
        /// <param name="useCaptureOffset"> </param>
        /// <returns>Number of highlights created</returns>
        internal int CreateHighlightObjects(List<long> darkFrameNumbers,
                                                         int captureDurationInSeconds, bool ignoreEarlyHighlights,
                                                         bool useCaptureOffset)
        {
            if (MainModel.IsAuthorized() == false)
                throw new Exception("Odessa Engine Not Authorized!");

            #region print input (DEBUG only)

#if DEBUG
            Logger.Info("Results of collapsing:");
            for (int darkFrameIndex = 0; darkFrameIndex < darkFrameNumbers.Count; darkFrameIndex++)
            {
                //Application.DoEvents();  // REMED out on 7/27/11 because this is no longer on app thread
                var darkFrameTimeSpan = TimeSpan.FromSeconds(darkFrameNumbers[darkFrameIndex] / InputFileObject.FramesPerSecond);

                Logger.Info("Dark frames at " + darkFrameNumbers[darkFrameIndex] +
                            " (" + darkFrameTimeSpan.ToString() + ")");
            }
#endif

            #endregion print input (DEBUG only)

            #region see if we should ignore early highlights

            int earlyHighlightTimeframeInSeconds = 0;
            if (ignoreEarlyHighlights)
            {
                earlyHighlightTimeframeInSeconds = 10; // we ignore all highlights within 10 seconds of start
                Logger.Info("Removing early highlights");
            }
            /* I think we should just turn it off. It's too deceiving because it can hide what's really going on
#if RELEASE // when testing, *NEVER* ignore highlights. otherwise this gives us a false perception of false positives
            else
            {
                earlyHighlightTimeframeInSeconds = 2; // always ignore highlights within first 2 seconds
                Logger.Info("Keeping early highlights but ignoring before 2 seconds");
            }
#endif
             */

            var earlyHighlightFrameNumber = (int)(earlyHighlightTimeframeInSeconds * InputFileObject.FramesPerSecond);

            Logger.Info("Removing highlights before frame " + earlyHighlightFrameNumber);

            for (int blackFrameIndex = 0; blackFrameIndex < darkFrameNumbers.Count; blackFrameIndex++)
            {
                if (darkFrameNumbers[blackFrameIndex] < earlyHighlightFrameNumber)
                {
                    darkFrameNumbers.RemoveAt(blackFrameIndex);
                    blackFrameIndex--;
                }
            }

            #endregion see if we should ignore early highlights

            #region print output (DEBUG only)

#if DEBUG
            Logger.Info("Results of threshold validation and early highlight removal:");
            for (int darkFrameIndex = 0; darkFrameIndex < darkFrameNumbers.Count; darkFrameIndex++)
            {
                var darkFrameTimeSpan = TimeSpan.FromSeconds((darkFrameNumbers[darkFrameIndex] / InputFileObject.FramesPerSecond));

                Logger.Info("Dark frames at " + darkFrameNumbers[darkFrameIndex] +
                            " (" + darkFrameTimeSpan.ToString() + ")");
            }
#endif

            #endregion print output (DEBUG only)

            #region now that we've removed ones that are too close, let's create the HighlightObject

            foreach (long darkFrame in darkFrameNumbers)
            {
                const double captureOffsetInSeconds = 0; // 0.5; let's get right up to the bookmark
                // let's capture another few seconds so we see the user's hand cover the lens

                // how long before the bookmark should we end the highlight?
                double secondsBeforeBookmark = 2.0;

                /*
                int captureOffsetInFrames = 0;
                if (useCaptureOffset)
                    captureOffsetInFrames = (int)(captureOffsetInSeconds * InputFileObject.FramesPerSecond);

                long framesAfterDarkFrame = (long)(InputFileObject.VideoDurationInSeconds * InputFileObject.FramesPerSecond) - darkFrame;

                long adjustedDarkFrame = darkFrame;
                if (framesAfterDarkFrame > captureOffsetInFrames)
                {
                    // we can add the full offset
                    adjustedDarkFrame += captureOffsetInFrames;
                }
                else
                {
                    // we can only add the number of frames after the ending frame
                    adjustedDarkFrame += framesAfterDarkFrame;
                }
                */

                var bookmarkTimeSpan = TimeSpan.FromSeconds(darkFrame / InputFileObject.FramesPerSecond);
                if (useCaptureOffset)
                    bookmarkTimeSpan = bookmarkTimeSpan.Add(TimeSpan.FromSeconds(captureOffsetInSeconds));
                if (bookmarkTimeSpan.TotalSeconds > InputFileObject.VideoDurationInSeconds)
                    bookmarkTimeSpan = TimeSpan.FromSeconds(InputFileObject.VideoDurationInSeconds);

                var endTimeSpan = bookmarkTimeSpan.Subtract(TimeSpan.FromSeconds(secondsBeforeBookmark));
                if (endTimeSpan.TotalSeconds < 0)
                    endTimeSpan = bookmarkTimeSpan;
                if (endTimeSpan.TotalSeconds < secondsBeforeBookmark)
                    endTimeSpan = TimeSpan.FromSeconds(secondsBeforeBookmark);

                var startTimeSpan = endTimeSpan.Subtract(TimeSpan.FromSeconds(captureDurationInSeconds));
                if (startTimeSpan.TotalSeconds < 0)
                    startTimeSpan = TimeSpan.Zero;

                var highlightObject = new HighlightObject();
                highlightObject.InputFileObject = InputFileObject;
                highlightObject.BookmarkTime = bookmarkTimeSpan;
                highlightObject.StartTime = startTimeSpan;
                highlightObject.EndTime = endTimeSpan;
                highlightObject.GenerateHighlightTitle(); // do this before adding
                MainModel.HighlightObjects.Add(highlightObject);

#if DEBUG
                Logger.Info("Adding new highlight from " + highlightObject.StartTime.ToString() + " " +
                            "to " + highlightObject.EndTime.ToString() +
                            " with bookmark = " + highlightObject.BookmarkTime.ToString());
#endif
            }

            #endregion now that we've removed ones that are too close, let's create the HighlightObject

            return darkFrameNumbers.Count;
        }

        /// <summary>
        /// Find the black frames in the video file
        /// </summary>
        /// <returns>Return code of Scan method</returns>
        internal int FindDarkFrames(ref List<long> bookmarkLocations)
        {
            if (MainModel.IsAuthorized() == false)
            {
                throw new Exception("Highlight Hunter Engine Not Authorized!");
            }

            int returnCode = 0;

            IntPtr arrayValue = IntPtr.Zero;
            try
            {
                int size;
                arrayValue = NativeOdessaMethods.Scan(InputFileObject.SourceFileInfo.FullName, out size, out returnCode);
                if (arrayValue != IntPtr.Zero)
                {
                    var result = new long[size];
                    Marshal.Copy(arrayValue, result, 0, size);
                    bookmarkLocations = new List<long>(result);
                }
                else
                {
                    Logger.Error("ScanWorker[" + Id + "]: error scanning! returnCode = " + returnCode);
                    throw new Exception("ScanWorker[" + Id + "]: error scanning! returnCode = " + returnCode);
                }
            }
            catch (Exception ex)
            {
                Logger.Error("ScanWorker[" + Id + "]: Exception while scanning: " + ex);
            }
            finally
            {
                // don't forget to free the list
                NativeOdessaMethods.FreePointer(arrayValue);
            }

            return returnCode;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns>True on success</returns>
        internal bool SetFramesPerSecond()
        {
            if (InputFileObject == null)
                return false;

            Logger.Info("ScanWorker[" + Id + "]: called");

            try
            {
                double fps;
                int returnCode = NativeOdessaMethods.GetFramesPerSecond(InputFileObject.SourceFileInfo.FullName, out fps);
                if (returnCode == (int)NativeOdessaMethods.OdessaReturnCodes.Success)
                {
                    Logger.Info("ScanWorker[" + Id + "]: FPS: " + fps);
                    InputFileObject.FramesPerSecond = fps;
                    return true;
                }
                
                OdessaReturnCode = (NativeOdessaMethods.OdessaReturnCodes)returnCode;
                Logger.Error("ScanWorker[" + Id + "]: Error while getting FPS in " + InputFileObject.SourceFileInfo.FullName +
                             ". returnCode = " + returnCode);
                AnalyticsHelper.FireEvent("Each video - scan result - UnableToDetermineFramesPerSecond - " + returnCode);
                AnalyticsHelper.FireEvent("Each video - scan result - UnableToDetermineFramesPerSecond - " + returnCode + " - " + Path.GetExtension(InputFileObject.SourceFileInfo.Name));
                return false;
            }
            catch (Exception ex)
            {
                Logger.Error("ScanWorker[" + Id + "]: Exception thrown while getting FPS in " + InputFileObject.SourceFileInfo.FullName +
                             ". " + ex);
                return false;
            }
        }

        internal void TrackScanDuration()
        {
            TimeSpan eachVideoScanDuration = ScanStop - ScanStart;
            try
            {
                int eachVideoScanDurationAsInt = Convert.ToInt32(eachVideoScanDuration.TotalSeconds / 60);
                if (eachVideoScanDurationAsInt >= 0)
                    AnalyticsHelper.FireEvent("Each video - scan duration in mins", eachVideoScanDurationAsInt);
                else
                    Debug.Assert(false, "Each video - scan duration in mins is negative!");
            }
            catch (Exception ex)
            {
                Logger.Error("Exception reporting scan duration: " + ex.ToString());
            }
        }

        internal void TrackScanMultiplier()
        {
            TimeSpan eachVideoScanDuration = ScanStop - ScanStart;

            if (eachVideoScanDuration.TotalSeconds > 0)
            {
                var eachVideoScanMultiplier =
                    (int)Math.Round(InputFileObject.VideoDurationInSeconds / eachVideoScanDuration.TotalSeconds, 0);
                if (eachVideoScanMultiplier > 0)
                    AnalyticsHelper.FireEvent("Each video - scan multiplier", eachVideoScanMultiplier);
            }
        }

        internal void TrackScanResult()
        {
            AnalyticsHelper.FireEvent("Each video - scan result - " + ScanWorkerResult.ToString());
        }

        internal void TrackTotalHoursScanned()
        {
            var oldHoursScanned = Convert.ToInt32(Settings.Default.TotalMinutesScanned) / 60;

            Settings.Default.TotalMinutesScanned += InputFileObject.VideoDurationInSeconds / 60;
            Settings.Default.Save();

            try
            {
                int totalHoursScanned = Convert.ToInt32(Settings.Default.TotalMinutesScanned) / 60;
                if (oldHoursScanned != totalHoursScanned) // we don't want to double-count individuals. wait until they reach next hour before reporting
                    AnalyticsHelper.FireEvent("Total hours scanned - " + totalHoursScanned);
            }
            catch (Exception ex)
            {
                Logger.Error("Exception reporting total hours scanned: " + ex.ToString());
            }
        }

        internal void TrackVideoDuration()
        {
            try
            {
                int videoDurationAsInt = Convert.ToInt32(InputFileObject.VideoDurationInSeconds / 60);
                if (videoDurationAsInt >= 0)
                    AnalyticsHelper.FireEvent("Each video - video duration in mins", videoDurationAsInt);
                else
                    Debug.Assert(false, "Each video - video duration in mins is negative!");
            }
            catch (Exception ex)
            {
                Logger.Error("Exception reporting video duration: " + ex.ToString());
            }
        }

        private bool CheckForCancelledWorker()
        {
            if (CancellationPending)
            {
                ScanWorkerResult = ScanWorkerResults.Cancelled; // don't use e.Cancel because of http://bytes.com/topic/c-sharp/answers/519073-asynch-crash-when-e-cancel-set
                Logger.Info("ScanWorker[" + Id + "].CancelWorker(): Cancelled!");
                return true;
            }
            return false;
        }

        private void ScanWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Logger.Info("ScanWorker[" + Id + "] ended with result " + ScanWorkerResult.ToString());

            if (ScanWorkerResult == ScanWorker.ScanWorkerResults.Success)
            { // we do this here so the analytics track as we go instead of one big push at the end of the scan
#if !TEST // don't track these from test because we'll have file locking issues when they try to save settings at the same time
                TrackVideoDuration();
                TrackScanDuration();
                TrackScanMultiplier();
                TrackTotalVideosScanned();
                TrackTotalHoursScanned();
                TrackFirstHighlightFound();
#endif
            }
            else if (ScanWorkerResult != ScanWorkerResults.NotAuthorized && ScanWorkerResult != ScanWorkerResults.Cancelled)
            {
                UploadMediaInfo();
            }

            TrackScanResult();
        }

        private void ScanWorkerDoWork(object sender, DoWorkEventArgs e)
        {
            Logger.Info("ScanWorker[" + Id + "]: called");

            ScanStart = DateTime.Now;

            if (MainModel.IsAuthorized() == false)
            {
                Logger.Error("Engine not authorized!");
                ScanWorkerResult = ScanWorkerResults.NotAuthorized;
                e.Result = 0;
                ScanStop = DateTime.Now;
                return;
            }

            // don't check for engine cancel here because we want status to say "cancelling..."

            #region Raise status update event

            ReportProgress(0);
            //MainModel.OnStatusUpdateEvent();

            #endregion Raise status update event

            int returnCode;

            #region Get total frames

            if (SetTotalFrames() == false)
            {
                Logger.Error("ScanWorker[" + Id + "]: Total frames = 0 in " + InputFileObject.SourceFileInfo.FullName);
                // keep going. it just means we can't calculate progress.
            }

            #endregion Get total frames

            if (CheckForCancelledWorker()) return;

            #region Get FramesPerSecond

            if (SetFramesPerSecond() == false || Math.Abs(InputFileObject.FramesPerSecond - 0) < Double.Epsilon)
            {
                Logger.Error("ScanWorker[" + Id + "]: Frames per second = 0 in " + InputFileObject.SourceFileInfo.FullName);
                ScanWorkerResult = ScanWorkerResults.UnableToDetermineFramesPerSecond;
                e.Result = 0; // number of highlights found
                ScanStop = DateTime.Now;
                return;
            }

            #endregion Get FramesPerSecond

            if (CheckForCancelledWorker()) return;

            #region Get video duration

            if (SetVideoDuration() == false || Math.Abs(InputFileObject.VideoDurationInSeconds - 0) < Double.Epsilon)
            {
                Logger.Error("ScanWorker[" + Id + "]: Unable to get video duration of " + InputFileObject.SourceFileInfo.FullName);
                ScanWorkerResult = ScanWorkerResults.UnableToDetermineVideoDuration;
                e.Result = 0; // number of highlights found
                ScanStop = DateTime.Now;
                return;
            }

            #endregion Get video duration

            if (CheckForCancelledWorker()) return;

            #region Get bitrate

            if (SetBitrate() == false || InputFileObject.Bitrate == 0)
            {
                Logger.Error("ScanWorker[" + Id + "]: Unable to determine bitrate of " + InputFileObject.SourceFileInfo.FullName);
                ScanWorkerResult = ScanWorkerResults.UnableToDetermineBitrate;
                e.Result = 0;
                ScanStop = DateTime.Now;
                return;
            }

            #endregion Get bitrate

            if (CheckForCancelledWorker()) return;

            #region Get video dimensions

            if (SetVideoDimensions() == false || InputFileObject.VideoWidth == 0 || InputFileObject.VideoHeight == 0)
            {
                Logger.Error("ScanWorker[" + Id + "]: Unable to determine dimensions of " + InputFileObject.SourceFileInfo.FullName);
                ScanWorkerResult = ScanWorkerResults.UnableToDetermineDimensions;
                e.Result = 0;
                ScanStop = DateTime.Now;
                return;
            }

            #endregion Get video dimensions

            if (CheckForCancelledWorker()) return;

            #region Scan for bookmarks

            Logger.Info("ScanWorker[" + Id + "]: Scanning for bookmarks");

            var updateScanProgress = new Timer(200);
            updateScanProgress.Elapsed += UpdateScanProgressElapsed;
            updateScanProgress.Enabled = true;

            var darkFrameNumbers = new List<long>();
            try
            {
                returnCode = FindDarkFrames(ref darkFrameNumbers);
                OdessaReturnCode = (NativeOdessaMethods.OdessaReturnCodes)returnCode;
                UpdateScanProgressElapsed(sender, null); // call this so progress is updated before we disable it
                updateScanProgress.Enabled = false;
            }
            catch (Exception ex)
            {
                Logger.Info("ScanWorker[" + Id + "]: Exception while scanning for bookmarks in " + InputFileObject.SourceFileInfo.FullName +
                            ": " + ex);
                ScanWorkerResult = ScanWorkerResults.UnableToScan;
                AnalyticsHelper.FireEvent("Each video - scan result - UnableToScan - " + ex);
                var extension = Path.GetExtension(InputFileObject.SourceFileInfo.Name);
                if (extension != null)
                    AnalyticsHelper.FireEvent("Each video - scan result - UnableToScan - " + extension.ToLowerInvariant());
                ScanStop = DateTime.Now;
                return;
            }
            if (returnCode != (int)NativeOdessaMethods.OdessaReturnCodes.Success)
            {
                Logger.Info("ScanWorker[" + Id + "]: Unable to scan for bookmarks: returnCode=" + returnCode);
                ScanWorkerResult = ScanWorkerResults.UnableToScan;
                AnalyticsHelper.FireEvent("Each video - scan result - UnableToScan - " + returnCode);
                ScanStop = DateTime.Now;
                return;
            }


            #endregion Scan for bookmarks

            if (CheckForCancelledWorker()) return;

            // Find the video chunks among the black frames
            TotalHighlightsFound = CreateHighlightObjects(darkFrameNumbers,
                                                    MainModel.CaptureDurationInSeconds,
                                                    MainModel.IgnoreEarlyHighlights, UseCaptureOffset);

            Logger.Info("ScanWorker[" + Id + "]: Total highlights: " + TotalHighlightsFound);

            ScanStop = DateTime.Now; // this is where we mark the end of the scan. we don't include splicing.

            ScanWorkerResult = ScanWorkerResults.Success;
        }

        internal bool SetBitrate()
        {
            Logger.Info("ScanWorker[" + Id + "]: called");

            if (InputFileObject == null)
                return false;

            try
            {
                int bitrate;
                int returnCode = NativeOdessaMethods.GetBitrate(InputFileObject.SourceFileInfo.FullName, out bitrate);
                // MainModel.GetBitrate(inputFile);
                if (returnCode == (int)NativeOdessaMethods.OdessaReturnCodes.Success)
                {
                    Logger.Info("ScanWorker[" + Id + "]: Bitrate: " + bitrate);
                    InputFileObject.Bitrate = bitrate;
                    return true;
                }

                OdessaReturnCode = (NativeOdessaMethods.OdessaReturnCodes)returnCode;
                Logger.Error("ScanWorker[" + Id + "]: Unable to determine bitrate: returnCode=" + returnCode);
                AnalyticsHelper.FireEvent("Each video - scan result - UnableToDetermineBitrate - " + returnCode);
                var extension = Path.GetExtension(InputFileObject.SourceFileInfo.Name);
                if (extension != null)
                    AnalyticsHelper.FireEvent("Each video - scan result - UnableToDetermineBitrate - " + returnCode + " - " + extension.ToLowerInvariant());
                return false;
            }
            catch (Exception ex)
            {
                Logger.Error("ScanWorker[" + Id + "]: Exception while determining bitrate: " + ex);
                return false;
            }
        }

        private bool SetCodec()
        {
            if (InputFileObject == null)
                return false;

            Logger.Info("ScanWorker[" + Id + "]: called");

            InputFileObject.Codec = "h264"; // doing this until we figure out if this caused trevor's crash
            return true;
            /*
            try
            {
                string codec;
                int returnCode = NativeOdessaMethods.GetCodec(InputFileObject.SourceFileInfo.FullName, out codec);
                if (returnCode == (int) NativeOdessaMethods.OdessaReturnCodes.Success)
                {
                    Logger.Info("ScanWorker[" + Id + "]: Codec: " + codec);
                    Codec = codec;
                    return true;
                }
                Logger.Error("ScanWorker[" + Id + "]: Error while getting codec in " + InputFileObject.SourceFileInfo.FullName +
                             ". returnCode = " + returnCode);
                return false;
            }
            catch (Exception ex)
            {
                Logger.Error("ScanWorker[" + Id + "]: Exception thrown while getting codec in " + InputFileObject.SourceFileInfo.FullName +
                             ". " + ex);
                return false;
            }
             */
        }

        internal bool SetTotalFrames()
        {
            if (InputFileObject == null)
                return false;

            Logger.Info("ScanWorker[" + Id + "]: called");

            try
            {
                long frames;
                int returnCode = NativeOdessaMethods.GetTotalFrames(InputFileObject.SourceFileInfo.FullName, out frames);
                if (returnCode == (int)NativeOdessaMethods.OdessaReturnCodes.Success)
                {
                    Logger.Info("ScanWorker[" + Id + "]: Total frames: " + frames);
                    InputFileObject.TotalFrames = frames;
                    return true;
                }

                OdessaReturnCode = (NativeOdessaMethods.OdessaReturnCodes)returnCode;
                Logger.Error("ScanWorker[" + Id + "]: Error while getting total frames in " + InputFileObject.SourceFileInfo.FullName +
                             ". returnCode = " + returnCode);
                AnalyticsHelper.FireEvent("Each video - scan result - UnableToDetermineTotalFrames - " + returnCode);
                var extension = Path.GetExtension(InputFileObject.SourceFileInfo.Name);
                if (extension != null)
                    AnalyticsHelper.FireEvent("Each video - scan result - UnableToDetermineTotalFrames - " + returnCode + " - " + extension.ToLowerInvariant());
                return false;
            }
            catch (Exception ex)
            {
                Logger.Error("ScanWorker[" + Id + "]: Exception thrown while getting total frames in " + InputFileObject.SourceFileInfo.FullName +
                             ". " + ex);
                return false;
            }
        }

        internal bool SetVideoDimensions()
        {
            if (InputFileObject == null)
                return false;

            Logger.Info("ScanWorker[" + Id + "]: called");

            try
            {
                int height;
                int width;
                int returnCode = NativeOdessaMethods.GetDimensions(InputFileObject.SourceFileInfo.FullName, out width, out height);
                if (returnCode == (int)NativeOdessaMethods.OdessaReturnCodes.Success)
                {
                    Logger.Info("ScanWorker[" + Id + "]: Dimensions: " + width + "x" + height);
                    InputFileObject.VideoWidth = width;
                    InputFileObject.VideoHeight = height;
                    return true;
                }

                OdessaReturnCode = (NativeOdessaMethods.OdessaReturnCodes)returnCode;
                Logger.Error("ScanWorker[" + Id + "]: Unable to determine dimensions: returnCode=" + returnCode);
                AnalyticsHelper.FireEvent("Each video - scan result - UnableToDetermineDimensions - " + returnCode);
                var extension = Path.GetExtension(InputFileObject.SourceFileInfo.Name);
                if (extension != null)
                    AnalyticsHelper.FireEvent("Each video - scan result - UnableToDetermineDimensions - " + returnCode + " - " + extension.ToLowerInvariant());
                return false;
            }
            catch (Exception ex)
            {
                Logger.Error("ScanWorker[" + Id + "]: Exception while determining dimensions: " + ex);
                return false;
            }
        }

        internal bool SetVideoDuration()
        {
            if (InputFileObject == null)
                return false;

            Logger.Info("ScanWorker[" + Id + "]: called");

            try
            {
                double duration;
                int returnCode = NativeOdessaMethods.GetDuration(InputFileObject.SourceFileInfo.FullName, out duration);
                if (returnCode == (int)NativeOdessaMethods.OdessaReturnCodes.Success)
                {
                    Logger.Info("ScanWorker[" + Id + "]: Video duration: " + duration);
                    InputFileObject.VideoDurationInSeconds = duration;
                    return true;
                }

                OdessaReturnCode = (NativeOdessaMethods.OdessaReturnCodes)returnCode;
                Logger.Error("ScanWorker[" + Id + "]: Error while getting video duration of " + InputFileObject.SourceFileInfo.FullName +
                             ". returnCode=" + returnCode);
                AnalyticsHelper.FireEvent("Each video - scan result - UnableToDetermineVideoDuration - " + returnCode);
                var extension = Path.GetExtension(InputFileObject.SourceFileInfo.Name);
                if (extension != null)
                    AnalyticsHelper.FireEvent("Each video - scan result - UnableToDetermineVideoDuration - " + returnCode + " - " + extension.ToLowerInvariant());
                return false;
            }
            catch (Exception ex)
            {
                Logger.Error("ScanWorker[" + Id + "]: Exception thrown while getting video duration of " +
                             InputFileObject.SourceFileInfo.FullName + ". " + ex);
                return false;
            }
        }

        private void UpdateScanProgressElapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                ProcessedFrames = NativeOdessaMethods.GetFramesProcessed();
            }
            catch (Exception ex)
            {
                Logger.Error("Exception calling GetFramesProcessed: " + ex);
            }

#if DEBUG
            //Debug.WriteLine("Processed Frames: " + ProcessedFrames);
#endif
        }

        private void UploadMediaInfo()
        {
            var uploadMediaInfoWorker = new UploadMediaInfoWorker(InputFileObject.SourceFileInfo, OdessaReturnCode);
            uploadMediaInfoWorker.RunWorkerAsync();
            // we don't care when this thing ends
        }
    }
}
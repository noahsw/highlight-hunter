using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using OdessaPCTestHelpers;
using NLog;


namespace TuningHostProject
{
    internal static class HostWorker
    {

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        
        /// <summary>
        /// A reference back to the calling form
        /// </summary>
        private static TuningForm _tf;

        public static volatile bool CancelTest = false;

        public static int TotalPassesCount = 0;

        public static int CompletedPassesCount = 0;

        public static int CompletedPassesCountThisSession = 0;

        public static int CompletedPassesCountAtSessionStart = 0;

        public static int CurrentPassNumber = 0;

        public static int NextInputFileNumber = 1;

        public static int InputFileCount = 0;

        public static int CompletedScansInThisPass = 0;


        /// <summary>
        /// The current threshold settings. These are public so the calling form can access them.
        /// </summary>
        public static int CurrentIndividualPixelBrightnessThreshold;
        public static int CurrentDarknessPercentageThreshold;
        public static int CurrentPixelScanPercentageThreshold;
        public static float CurrentSecondsSkipThreshold;
        public static float CurrentConsecutiveDarkFramesInSecondsThreshold;

        /// <summary>
        /// The current number of allowed concurrent scans. Default is 3.
        /// </summary>
        public static int AllowedConcurrentScanCount = 3;

        /// <summary>
        /// Maintains a list of currently running scans and their associated process
        /// </summary>
        public static List<ScanItem> ScanProcessList = new List<ScanItem>();

        /// <summary>
        /// Maintains a list of files to scan
        /// </summary>
        private static Queue<FileInfo> _scanFilesQueue;

        /// <summary>
        /// Variables to keep track of the pass info
        /// </summary>
        public static int PassActualScore = 0;
        public static int PassMaxScore = 0;
        public static int PassFoundCount = 0;
        public static int PassFalsePositivesCount = 0;
        public static int PassMissingCount = 0;
        public static int PassAccurateCount = 0;
        public static TimeSpan PassScanTime = TimeSpan.Zero;

        //private static Logging _logger = null; // this is initialized when we start scanning in DoWork()

        private static Logging _tuningGraphLogger = new Logging();

        /// <summary>
        /// Represents an item currently being scanned
        /// </summary>
        public struct ScanItem
        {
            public int InputFileNumber;
            public Process ThisProcess;
            public FileInfo ThisFileInfo;
        }


        public static void Initialize(TuningForm mTf)
        {
            _tf = mTf;
        }

        public static void Cleanup()
        {

        }

        public static void DoWork()
        {

            CompletedPassesCountThisSession = 0;

            var findDarkFramesHelper = new FindDarkFramesHelper();

            InputFileCount = findDarkFramesHelper.AvailableFiles.Count();

            /*
            float ConsecutiveDarkFramesThresholdInSecondsStart = (float)0.15; //0.2;
            float ConsecutiveDarkFramesThresholdInSecondsEnd = (float)0.15; // 0.3;
            float ConsecutiveDarkFramesThresholdInSecondsIncrement = (float)0.05;

            int IndividualPixelBrightnessThresholdStart = 80; // 66;
            int IndividualPixelBrightnessThresholdEnd = 80; // 76;
            int IndividualPixelBrightnessThresholdIncrement = 2;

            int DarknessThresholdPercentageStart = 80; // 86;
            int DarknessThresholdPercentageEnd = 80; // 94;
            int DarknessThresholdPercentageIncrement = 2;
            */
            
            // resume where we left off
            CompletedPassesCount = Properties.Settings.Default.CompletedPassesCount;
            CompletedPassesCountAtSessionStart = CompletedPassesCount;
            
            int i = 0;

            for (CurrentIndividualPixelBrightnessThreshold = Properties.Settings.Default.ThresholdIndividualPixelBrightnessStart;
                CurrentIndividualPixelBrightnessThreshold <= Properties.Settings.Default.ThresholdIndividualPixelBrightnessEnd;
                CurrentIndividualPixelBrightnessThreshold += Properties.Settings.Default.ThresholdIndividualPixelBrightnessIncrement)
            {

                for (CurrentDarknessPercentageThreshold = Properties.Settings.Default.ThresholdDarkPixelsPerFrameAsPercentageStart;
                    CurrentDarknessPercentageThreshold <= Properties.Settings.Default.ThresholdDarkPixelsPerFrameAsPercentageEnd;
                    CurrentDarknessPercentageThreshold += Properties.Settings.Default.ThresholdDarkPixelsPerFrameAsPercentageIncrement)
                {

                    for (CurrentPixelScanPercentageThreshold = Properties.Settings.Default.ThresholdPixelScanPercentageStart;
                        CurrentPixelScanPercentageThreshold <= Properties.Settings.Default.ThresholdPixelScanPercentageEnd;
                        CurrentPixelScanPercentageThreshold += Properties.Settings.Default.ThresholdPixelScanPercentageIncrement)
                    {

                        int secondsSkipCurrentPass = 1; // needed because we're dealing with floats

                        for (CurrentSecondsSkipThreshold = Properties.Settings.Default.ThresholdSecondsSkipStart;
                            CurrentSecondsSkipThreshold <= Properties.Settings.Default.ThresholdSecondsSkipEnd;
                            CurrentSecondsSkipThreshold = (secondsSkipCurrentPass++) * Properties.Settings.Default.ThresholdSecondsSkipIncrement + Properties.Settings.Default.ThresholdSecondsSkipStart)
                        {

                            int consecutiveDarkFramesInSecondsCurrentPass = 1; // needed because we're dealing with floats

                            for (CurrentConsecutiveDarkFramesInSecondsThreshold = Properties.Settings.Default.ThresholdConsecutiveDarkFramesInSecondsStart;
                                CurrentConsecutiveDarkFramesInSecondsThreshold <= Properties.Settings.Default.ThresholdConsecutiveDarkFramesInSecondsEnd;
                                CurrentConsecutiveDarkFramesInSecondsThreshold = (consecutiveDarkFramesInSecondsCurrentPass++) * Properties.Settings.Default.ThresholdConsecutiveDarkFramesInSecondsIncrement + Properties.Settings.Default.ThresholdConsecutiveDarkFramesInSecondsStart)
                            {

                                CurrentPassNumber = CompletedPassesCount + 1;

                                if (CheckForCancel())
                                    break;

                                i++;

                                if (i < CompletedPassesCount + 1)
                                {
                                    continue; // resume where we left off
                                }

                                CompletedScansInThisPass = 0;

                                PassActualScore = 0;
                                PassMaxScore = 0;
                                PassFoundCount = 0;
                                PassFalsePositivesCount = 0;
                                PassMissingCount = 0;
                                PassAccurateCount = 0;
                                PassScanTime = TimeSpan.Zero;

                                //_logger = new Logging();
                                //_logger.InitLogger("Odessa_TuningLog" + CurrentPassNumber + ".txt", false, true, _tf.LogDirectory);

                                LogInfo("-------------");
                                LogInfo("Starting pass number " + CurrentPassNumber + " of " + TotalPassesCount);
                                LogInfo("CurrentIndividualPixelBrightnessThreshold: " + CurrentIndividualPixelBrightnessThreshold);
                                LogInfo("CurrentDarknessPercentageThreshold: " + CurrentDarknessPercentageThreshold);
                                LogInfo("CurrentPixelScanPercentageThreshold: " + CurrentPixelScanPercentageThreshold);
                                LogInfo("CurrentSecondsSkipThreshold: " + CurrentSecondsSkipThreshold);
                                LogInfo("CurrentConsecutiveDarkFramesInSecondsThreshold: " + CurrentConsecutiveDarkFramesInSecondsThreshold);
                                LogInfo("Total input files: " + InputFileCount);

                                // START SCANNING
                                NextInputFileNumber = 1;

                                _scanFilesQueue = new Queue<FileInfo>(findDarkFramesHelper.AvailableFiles.Values);

                                while (ScanProcessList.Count > 0 || _scanFilesQueue.Count > 0)
                                { // while there are files in this pass being scanned

                                    #region Check whether to start a new process
                                    if (ScanProcessList.Count < AllowedConcurrentScanCount && _scanFilesQueue.Count > 0)
                                    { // we've got room to start a new process

                                        FileInfo fi = _scanFilesQueue.Dequeue();

                                        var si = new ScanItem { InputFileNumber = NextInputFileNumber, ThisFileInfo = fi };

                                        LogInfo("InputFileNumber #" + si.InputFileNumber + ": Scanning " + fi.Name);

                                        // call TuningScanner
                                        var p = new Process();
                                        p.StartInfo.FileName = Path.Combine(Environment.CurrentDirectory, "TuningScannerProject.exe");
                                        p.StartInfo.Arguments = "\"" + fi.FullName + "\" " +
                                            si.InputFileNumber + " " +
                                            CurrentPassNumber + " " +
                                            CurrentIndividualPixelBrightnessThreshold + " " +
                                            CurrentDarknessPercentageThreshold + " " +
                                            CurrentPixelScanPercentageThreshold + " " +
                                            CurrentSecondsSkipThreshold + " " +
                                            CurrentConsecutiveDarkFramesInSecondsThreshold
                                            ;
                                        p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;

                                        LogInfo("InputFileNumber #" + si.InputFileNumber + ": Starting with arguments: " + p.StartInfo.FileName + " " + p.StartInfo.Arguments);

                                        p.Start();
                                        p.PriorityClass = ProcessPriorityClass.Idle;

                                        // add it to our ScanProcessList so we can track it
                                        si.ThisProcess = p;
                                        ScanProcessList.Add(si);
                                        NextInputFileNumber += 1;

                                    }
                                    #endregion

                                    #region Check to see if scans have finished
                                    foreach (ScanItem si in ScanProcessList)
                                    {

                                        if (si.ThisProcess.HasExited)
                                        { // process just ended, let's analyze results.

                                            ScanProcessList.Remove(si);

                                            if (si.ThisProcess.ExitCode != 0) // 0 means success.
                                            {
                                                LogInfo("InputFileNumber #" + si.InputFileNumber + ": process did not exit successfully!");
                                                CancelTest = true;
                                                break;
                                            }


                                            // load results from output file
                                            string testResultOutputFilePath = Path.Combine(Path.GetTempPath(), "Odessa_TestResult_" + si.InputFileNumber + ".txt");
                                            LogInfo("InputFileNumber #" + si.InputFileNumber + ": Reading test results from " + testResultOutputFilePath);
                                            FileStream stream = File.Open(testResultOutputFilePath, FileMode.Open);
                                            var bformatter = new BinaryFormatter();
                                            var tr = (FindDarkFramesHelper.TestResult)bformatter.Deserialize(stream);
                                            stream.Close();

                                            // clean up test output file
                                            try
                                            {
                                                File.Delete(testResultOutputFilePath);
                                            }
                                            catch { }

                                            // add test results to pass results
                                            PassFoundCount += tr.Matched.Count;
                                            PassFalsePositivesCount += tr.FalsePositives.Count;
                                            PassMissingCount += tr.Missing.Count;
                                            PassActualScore += tr.ActualScore;
                                            PassMaxScore += tr.MaxScore;
                                            PassScanTime += tr.ScanTime;
                                            if (tr.ActualScore == tr.MaxScore)
                                                PassAccurateCount++;

                                            LogInfo("InputFileNumber #" + si.InputFileNumber + ": inputFile = " + tr.InputFile.Name);

                                            LogInfo("InputFileNumber #" + si.InputFileNumber + ": DarkTimesExpectedCount = " + tr.DarkTimesExpected.Count);
                                            foreach (string expectedDarkTime in tr.DarkTimesExpected)
                                                LogInfo("InputFileNumber #" + si.InputFileNumber + ": - " + expectedDarkTime);

                                            LogInfo("InputFileNumber #" + si.InputFileNumber + ": DarkTimesFoundCount = " + tr.DarkTimesFound.Count);
                                            foreach (string darkTimeFound in tr.DarkTimesFound)
                                                LogInfo("InputFileNumber #" + si.InputFileNumber + ": - " + darkTimeFound);

                                            LogInfo("InputFileNumber #" + si.InputFileNumber + ": MatchedCount = " + tr.Matched.Count);
                                            foreach (string time in tr.Matched)
                                                LogInfo("InputFileNumber #" + si.InputFileNumber + ": - " + time);

                                            LogInfo("InputFileNumber #" + si.InputFileNumber + ": FalsePositivesCount = " + tr.FalsePositives.Count);
                                            foreach (string time in tr.FalsePositives)
                                                LogInfo("InputFileNumber #" + si.InputFileNumber + ": - " + time);

                                            LogInfo("InputFileNumber #" + si.InputFileNumber + ": MissingCount = " + tr.Missing.Count);
                                            foreach (string time in tr.Missing)
                                                LogInfo("InputFileNumber #" + si.InputFileNumber + ": - " + time);

                                            LogInfo("InputFileNumber #" + si.InputFileNumber + ": ActualScore = " + tr.ActualScore);
                                            LogInfo("InputFileNumber #" + si.InputFileNumber + ": MaxScore = " + tr.MaxScore);
                                            LogInfo("InputFileNumber #" + si.InputFileNumber + ": ScanTime = " + tr.ScanTime.TotalSeconds + "secs");

                                            CompletedScansInThisPass += 1;

                                            break; // since we just removed an item from this collection, we can't continue this foreach

                                        }


                                    } // for each process
                                    #endregion

                                    if (CheckForCancel())
                                        break;

                                    Thread.Sleep(1000); // sleep for a second

                                }


                                if (CheckForCancel())
                                    break;

                                LogInfo("PassFoundCount = " + PassFoundCount);
                                LogInfo("PassFalsePositivesCount = " + PassFalsePositivesCount);
                                LogInfo("PassMissingCount = " + PassMissingCount);
                                LogInfo("PassMaxScore = " + PassMaxScore);
                                LogInfo("PassActualScore = " + PassActualScore);
                                LogInfo("PassAccurateCount = " + PassAccurateCount);
                                LogInfo("PassScanTime = " + PassScanTime.TotalSeconds + "secs");

                                _tuningGraphLogger = new Logging();
                                if (_tuningGraphLogger.InitLogger(Program.TUNING_RESULTS_FILENAME, true, false, _tf.LogDirectory))
                                { // log didn't exist before so let's write header
                                    WriteTuningGraphLogHeader();
                                }

                                // write out to tuning graph log
                                if (_tuningGraphLogger.WriteToLog(CurrentPassNumber + "," +
                                    CurrentIndividualPixelBrightnessThreshold + "," +
                                    CurrentDarknessPercentageThreshold + "," +
                                    CurrentPixelScanPercentageThreshold + "," +
                                    CurrentSecondsSkipThreshold + "," +
                                    CurrentConsecutiveDarkFramesInSecondsThreshold + "," +
                                    PassFalsePositivesCount + "," +
                                    PassMissingCount + "," +
                                    "=" + PassAccurateCount + "/" + InputFileCount + "," +
                                    PassScanTime.TotalSeconds + ","
                                    , false) == false)
                                { // write was't successful to our main log (probably because it was left open). let's try writing to a different log
                                    _tuningGraphLogger.CloseLogger(false);
                                    _tuningGraphLogger = null;
                                    _tuningGraphLogger = new Logging();
                                    _tuningGraphLogger.InitLogger("Odessa_TuningGraph_" + (new Random()).Next(1000) + ".csv", true, false, _tf.LogDirectory);
                                    _tuningGraphLogger.WriteToLog(CurrentPassNumber + "," +
                                        CurrentIndividualPixelBrightnessThreshold + "," +
                                        CurrentDarknessPercentageThreshold + "," +
                                        CurrentPixelScanPercentageThreshold + "," +
                                        CurrentSecondsSkipThreshold + "," +
                                        CurrentConsecutiveDarkFramesInSecondsThreshold + "," +
                                        PassFalsePositivesCount + "," +
                                        PassMissingCount + "," +
                                        "=" + PassAccurateCount + "/" + InputFileCount + "," +
                                        PassScanTime.TotalSeconds + ","
                                        , false);
                                }


                                _tuningGraphLogger.CloseLogger(false);
                                _tuningGraphLogger = null;

                                if (CheckForCancel())
                                    break;

                                CompletedPassesCount += 1;
                                CompletedPassesCountThisSession += 1;

                                // save to propertybag so we can resume later
                                Properties.Settings.Default.CurrentThresholdIndividualPixelBrightness = CurrentIndividualPixelBrightnessThreshold;
                                Properties.Settings.Default.CurrentThresholdDarkPixelsPerFrameAsPercentage = CurrentDarknessPercentageThreshold;
                                Properties.Settings.Default.CurrentThresholdPixelScanPercentage = CurrentPixelScanPercentageThreshold;
                                Properties.Settings.Default.CurrentThresholdSecondsSkip = CurrentSecondsSkipThreshold;
                                Properties.Settings.Default.CurrentThresholdConsecutiveDarkFramesInSeconds = CurrentConsecutiveDarkFramesInSecondsThreshold;
                                Properties.Settings.Default.CompletedPassesCount = CompletedPassesCount;
                                Properties.Settings.Default.Save();


                                LogInfo("CompletedPassesCount = " + CompletedPassesCount);

                                // close log and archive it
                                /* REMED out on 9/27/11 because we now save the logs to this new directory from the start
                                string OldTuningLogFilePath = Logger.LogFilePath;
                                Logger.CloseLogger();
                                Logger = null;
                                string NewTuningLogFilePath = Path.Combine(Properties.Settings.Default.TuningResultsDirectory, "Odessa_TuningLog" + CurrentPassNumber + ".txt");
                                try
                                {
                                    File.Copy(OldTuningLogFilePath, NewTuningLogFilePath, true);
                                    File.Delete(OldTuningLogFilePath);
                                }
                                catch (Exception ex)
                                {
                                    Debug.WriteLine("Error copying log file: " + ex.ToString());
                                }
                                 */

                                if (CheckForCancel())
                                    break;

                            } // for loop 

                            if (CheckForCancel())
                                break;

                        } // for loop


                        if (CheckForCancel())
                            break;

                    } // for loop

                    if (CheckForCancel())
                        break;

                } // for loop 

                if (CheckForCancel())
                    break;

            } // for loop

            Logger.Info("Done with massive loop!");

            findDarkFramesHelper.Dispose();

            if (CheckForCancel() == false)
                _tf.Invoke(_tf.MDelegateThreadFinished);

        }


        private static bool CheckForCancel()
        {
            if (CancelTest == true)
            { // cancel!

                foreach (ScanItem si in ScanProcessList)
                {
                    if (si.ThisProcess.HasExited == false)
                    {
                        try
                        {
                            si.ThisProcess.Kill();
                        }
                        catch (Exception e)
                        {
                            Debug.WriteLine("Couldn't kill process: " + e.ToString());
                        }
                    }
                }

                //LogInfo("Cancel detected!");
                _tf.Invoke(_tf.MDelegateThreadCancelled);

                return true;
            }
            else
                return false; // keep running

        }


        public static void WriteTuningGraphLogHeader()
        {
            _tuningGraphLogger.WriteToLog("PassNumber, " +
                "IndividualPixelBrightness, " +
                "DarkPixelsPerFrameAsPercentage, " +
                "PixelScanPercentage," +
                "SecondsSkip, " +
                "ConsecutiveDarkFramesInSeconds, " +
                "PassFalsePositivesCount," +
                "PassMissingCount," +
                "PassAccurateCount," +
                "PassScanTime,"
                , false);
        }


        private static void LogInfo(string message)
        {
            var theEvent = new LogEventInfo(LogLevel.Info, "", message);
            theEvent.Properties["PassNumber"] = CurrentPassNumber;
            Logger.Log(theEvent);
        }

        /*        
                public void RequestStop()
                {
                    _shouldStop = true;
                }
                // Volatile is used as hint to the compiler that this data
                // member will be accessed by multiple threads.
                private volatile bool _shouldStop;
            }
                */

    }

}

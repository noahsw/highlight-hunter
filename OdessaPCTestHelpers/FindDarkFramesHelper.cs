using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Diagnostics;
using NLog;
using OdessaGUIProject;
using OdessaGUIProject.Workers;

namespace OdessaPCTestHelpers
{

    public class FindDarkFramesHelper : IDisposable
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private enum ScanResultScore
        {
            FalsePositive = -1,
            Found = 1,
            Missing = -2
        }

        public Dictionary<string, FileInfo> AvailableFiles = new Dictionary<string, FileInfo>();

        [Serializable]
        public struct TestResult
        {
            public FileInfo InputFile;
            public int ActualScore;
            public int MaxScore;
            // MinScore is negative infinity because we could find tons of false positives
            public List<string> DarkTimesFound; // the times found during the scan
            public List<string> DarkTimesExpected; // the times we expected
            public List<string> FalsePositives; // the times found that were not expected
            public List<string> Matched; // the times found that were expected
            public List<string> Missing; // the times expected that were not found
            public TimeSpan ScanTime;
            public bool Failed;
        }


        public FindDarkFramesHelper()
        {
            MainModel.Initialize();

            /* REMED because we now use NLog
            if (logger = null)
            {
                Logger = new OdessaEngineV2HelperProject.LoggingHelper();
                Logger.InitLogger("Odessa_EngineFindDarkFramesUnitTest.log");
            }
            else
            {
                Logger = logger;
            }
            */


            const string root = @"D:\Projects\Odessa\Test Videos\trunk\";

            var validExtensions = MainModel.GetVideoExtensions();

            foreach (var filename in Directory.EnumerateFiles(root, "*.*", SearchOption.TopDirectoryOnly))
            {
                if (validExtensions.Contains(Path.GetExtension(filename).ToLowerInvariant()))
                {
                    AddInputFile(new FileInfo(filename));
                }
            }

            /*

            AddInputFile(new FileInfo(Path.Combine(root, @"Contour_sample.MOV.highlight.00.00.00.to.00.00.11.MOV")));
            AddInputFile(new FileInfo(Path.Combine(root, @"Contour_sample.MOV.highlight.00.00.15.to.00.00.39.MOV")));
            // AddInputFile(new FileInfo(Path.Combine(root, @"Contour_allblack.MOV"))); // REMED because this is not a real world sample

            #region Boat tests
            AddInputFile(new FileInfo(Path.Combine(root, @"GOPR0_boat_001.MP4")));
            AddInputFile(new FileInfo(Path.Combine(root, @"GOPR0_boat_002.MP4")));
            #endregion

            #region Paddleboarding tests
            AddInputFile(new FileInfo(Path.Combine(root, @"GOPR0_paddle_014.MP4")));
            AddInputFile(new FileInfo(Path.Combine(root, @"GOPR0_paddle_015.MP4.highlight.00.00.00.to.00.00.18.MP4")));
            // REMED because I fall in the water and it's too dark
            //AddInputFile(new FileInfo(Path.Combine(root, @"GOPR0016.MP4")));
            AddInputFile(new FileInfo(Path.Combine(root, @"GOPR0_paddle_015.MP4.highlight.00.00.53.to.00.01.08.MP4")));
            AddInputFile(new FileInfo(Path.Combine(root, @"GOPR0_paddle_015.MP4.highlight.00.01.36.to.00.01.48.MP4")));
            AddInputFile(new FileInfo(Path.Combine(root, @"GOPR0_paddle_015.MP4.highlight.00.03.23.to.00.03.38.MP4")));
            AddInputFile(new FileInfo(Path.Combine(root, @"GOPR0_paddle_015.MP4.highlight.00.04.24.to.00.04.38.MP4")));
            AddInputFile(new FileInfo(Path.Combine(root, @"GOPR0_paddle_016.MP4.highlight.00.00.23.to.00.00.33.MP4")));
            AddInputFile(new FileInfo(Path.Combine(root, @"GOPR0_paddle_016.MP4.highlight.00.01.58.to.00.02.18.MP4")));
            AddInputFile(new FileInfo(Path.Combine(root, @"GOPR0_paddle_016.MP4.highlight.00.03.43.to.00.03.55.MP4")));
            AddInputFile(new FileInfo(Path.Combine(root, @"GOPR0_paddle_016.MP4.highlight.00.05.03.to.00.05.13.MP4")));
            AddInputFile(new FileInfo(Path.Combine(root, @"GOPR0_paddle_020.MP4.highlight.00.05.13.to.00.05.28.MP4")));
            AddInputFile(new FileInfo(Path.Combine(root, @"GOPR0_paddle_022.MP4.highlight.00.00.03.to.00.00.13.MP4")));
            AddInputFile(new FileInfo(Path.Combine(root, @"GOPR0_paddle_022.MP4.highlight.00.00.54.to.00.01.02.MP4")));
            AddInputFile(new FileInfo(Path.Combine(root, @"GOPR0_paddle_022.MP4.highlight.00.02.33.to.00.02.43.MP4")));
            AddInputFile(new FileInfo(Path.Combine(root, @"GOPR0_paddle_023.MP4.highlight.00.01.23.to.00.01.33.MP4")));
            AddInputFile(new FileInfo(Path.Combine(root, @"GOPR0_paddle_023.MP4.highlight.00.01.53.to.00.02.08.MP4")));
            AddInputFile(new FileInfo(Path.Combine(root, @"GOPR0_paddle_026.MP4.highlight.00.02.23.to.00.02.43.MP4")));
            AddInputFile(new FileInfo(Path.Combine(root, @"GOPR0_paddle_027.MP4.highlight.00.06.08.to.00.06.20.MP4")));
            AddInputFile(new FileInfo(Path.Combine(root, @"GOPR0_paddle_027.MP4.highlight.00.06.41.to.00.06.52.MP4")));
            AddInputFile(new FileInfo(Path.Combine(root, @"GOPR0_paddle_027.MP4.highlight.00.07.13.to.00.07.23.MP4")));
            AddInputFile(new FileInfo(Path.Combine(root, @"GOPR0_paddle_027.MP4.highlight.00.08.53.to.00.09.10.MP4")));
            AddInputFile(new FileInfo(Path.Combine(root, @"GOPR0_paddle_027.MP4.highlight.00.12.56.to.00.13.06.MP4")));
            AddInputFile(new FileInfo(Path.Combine(root, @"GOPR0_paddle_027.MP4.highlight.00.14.23.to.00.14.35.MP4")));
            AddInputFile(new FileInfo(Path.Combine(root, @"GOPR0_paddle_027.MP4.highlight.00.14.45.to.00.14.55.MP4")));
            AddInputFile(new FileInfo(Path.Combine(root, @"GOPR0_paddle_027.MP4.highlight.00.15.58.to.00.16.12.MP4")));
            AddInputFile(new FileInfo(Path.Combine(root, @"GOPR0_paddle_028.MP4.highlight.00.00.23.to.00.01.03.MP4")));
            #endregion

            #region Biking tests
            AddInputFile(new FileInfo(Path.Combine(root, @"GOPR0_bike_030_1.MP4.highlight.00.00.00.to.00.00.41.MP4")));
            AddInputFile(new FileInfo(Path.Combine(root, @"GOPR0_bike_030_2.MP4.highlight.00.00.00.to.00.00.11.MP4")));
            AddInputFile(new FileInfo(Path.Combine(root, @"GOPR0_bike_030_5.MP4")));
            AddInputFile(new FileInfo(Path.Combine(root, @"GOPR0_bike_030_6.MP4.highlight.00.00.00.to.00.00.18.MP4")));
            AddInputFile(new FileInfo(Path.Combine(root, @"GOPR0_bike_030_8.MP4")));
            #endregion

            #region Snow shade tests
            AddInputFile(new FileInfo(Path.Combine(root, @"Contour_snow_false positive shade.MOV")));
            AddInputFile(new FileInfo(Path.Combine(root, @"Contour_snow_false positive shade 2.MOV")));
            AddInputFile(new FileInfo(Path.Combine(root, @"Contour_snow_false positive shade 3.MOV")));
            AddInputFile(new FileInfo(Path.Combine(root, @"Contour_snow_false positive shade 5.MOV")));
            AddInputFile(new FileInfo(Path.Combine(root, @"Contour_snow_false positive shade 6.MOV")));
            AddInputFile(new FileInfo(Path.Combine(root, @"Contour_snow_false positive shade 7.MOV")));
            AddInputFile(new FileInfo(Path.Combine(root, @"Contour_snow_false positive shade 8.MOV")));
            AddInputFile(new FileInfo(Path.Combine(root, @"Contour_snow_false positive shade 9.MOV")));
            AddInputFile(new FileInfo(Path.Combine(root, @"Contour_snow_false positive shade 10.MOV")));
            AddInputFile(new FileInfo(Path.Combine(root, @"Contour_snow_false positive shade 11.MOV")));
            #endregion

            AddInputFile(new FileInfo(Path.Combine(root, @"GOPR0_snow_false positive gloves.MP4")));
            AddInputFile(new FileInfo(Path.Combine(root, @"GOPR0_snow_false positive gloves 2.MP4")));


            AddInputFile(new FileInfo(Path.Combine(root, @"Contour_snow_speed.MOV")));
            AddInputFile(new FileInfo(Path.Combine(root, @"Contour_snow_snowball fight.MOV")));
            AddInputFile(new FileInfo(Path.Combine(root, @"Contour_snow_trev backside 180.MOV")));
            AddInputFile(new FileInfo(Path.Combine(root, @"Contour_snow_trevor 180.MOV")));
            AddInputFile(new FileInfo(Path.Combine(root, @"GOPR0_snow_noah jump.MP4")));
            AddInputFile(new FileInfo(Path.Combine(root, @"GOPR0_snow_stevens view from top.MP4")));
            AddInputFile(new FileInfo(Path.Combine(root, @"GOPR0_snow_trev jump.MP4")));


            AddInputFile(new FileInfo(Path.Combine(root, @"GOPR0_inside_1.MP4")));

            #region Kiting tests
            //AddInputFile(new FileInfo(Path.Combine(root, @"GOPR0_kite_001.MP4.highlight.00.00.24.to.00.00.44.MP4")));
            //AddInputFile(new FileInfo(Path.Combine(root, @"GOPR0_kite_001.MP4.highlight.00.01.24.to.00.01.44.MP4")));
            AddInputFile(new FileInfo(Path.Combine(root, @"GOPR0_kite_005.MP4.highlight.00.00.00.to.00.00.13.MP4")));
            AddInputFile(new FileInfo(Path.Combine(root, @"GOPR0_kite_006.MP4.highlight.00.00.00.to.00.00.18.MP4")));
            #endregion

            #region Snorkel tests
            AddInputFile(new FileInfo(Path.Combine(root, @"GOPR0_snorkel_008.MP4.highlight.00.00.00.to.00.00.18.MP4")));
            #endregion

            #region Snowboarding tests
            //AddInputFile(new FileInfo(Path.Combine(root, @"GOPR0_snow_005.MP4.highlight.00.04.04.to.00.04.24.MP4")));
            //AddInputFile(new FileInfo(Path.Combine(root, @"GOPR0_snow_008.MP4.highlight.00.00.41.to.00.00.54.MP4")));
            //AddInputFile(new FileInfo(Path.Combine(root, @"GOPR0_snow_009.MP4.highlight.00.01.59.to.00.02.09.MP4")));
            //AddInputFile(new FileInfo(Path.Combine(root, @"GOPR0_snow_013.MP4.highlight.00.00.00.to.00.00.14.MP4")));
            //AddInputFile(new FileInfo(Path.Combine(root, @"GOPR0_snow_014.MP4.highlight.00.00.10.to.00.00.20.MP4")));
            //AddInputFile(new FileInfo(Path.Combine(root, @"GOPR0_snow_021.MP4.highlight.00.00.00.to.00.00.16.MP4")));
            AddInputFile(new FileInfo(Path.Combine(root, @"GOPR0_snow_022.MP4.highlight.00.06.58.to.00.07.16.MP4")));
            //AddInputFile(new FileInfo(Path.Combine(root, @"GOPR0_snow_045.MP4.highlight.00.00.39.to.00.01.19.MP4")));
            //AddInputFile(new FileInfo(Path.Combine(root, @"GOPR0_snow_045.MP4.highlight.00.02.49.to.00.03.19.MP4")));
            //AddInputFile(new FileInfo(Path.Combine(root, @"GOPR0_snow_045.MP4.highlight.00.06.04.to.00.06.19.MP4")));
            //AddInputFile(new FileInfo(Path.Combine(root, @"GOPR0_snow_045.MP4.highlight.00.06.29.to.00.06.49.MP4")));
            //AddInputFile(new FileInfo(Path.Combine(root, @"GOPR0_snow_045.MP4.highlight.00.11.39.to.00.11.54.MP4")));
            AddInputFile(new FileInfo(Path.Combine(root, @"GOPR0_snow_057.MP4.highlight.00.04.48.to.00.05.08.MP4")));
            #endregion


            #region Gideon's webcam
            // for some reason we have a hard time detecting bookmarks in these videos. it may be specific to webcam so i'm excluding for now
            //AddInputFile(new FileInfo(Path.Combine(root, @"Gideon_webcam.m4v.highlight.00.03.09.to.00.03.25.m4v")));
            //AddInputFile(new FileInfo(Path.Combine(root, @"Gideon_webcam.m4v.highlight.00.06.29.to.00.06.44.m4v")));
            //AddInputFile(new FileInfo(Path.Combine(root, @"Gideon_webcam.m4v.highlight.00.24.15.to.00.24.30.m4v")));
            #endregion

            #region Motorcycle
            AddInputFile(new FileInfo(Path.Combine(root, @"GOPR8417_marius.MP4")));
            #endregion

            */

        }


        public void Dispose()
        {
            MainModel.Dispose();
        }

        private void AddInputFile(FileInfo fi)
        {
            // if (fi.Exists) // TODO: uncomment this so we figure out which test videos we lost and need to recreate
            AvailableFiles.Add(fi.Name, fi);
        }



        /// <summary>
        /// This goes through a number of files that have an expected hand
        /// </summary>
        public TestResult FindDarkFramesTest(string filename)
        {

            FileInfo inputFile = AvailableFiles[filename];

            var inputFileObject = new InputFileObject(AvailableFiles[filename]);

            Logger.Info("-------------");
            Logger.Info("Running dark frame test on " + inputFile.FullName);

            Logger.Info("Processing: " + inputFile.FullName);

            DateTime startScanTime = DateTime.Now;

            MainModel.IgnoreEarlyHighlights = false;
            MainModel.UseCaptureOffset = false;

            MainModel.HighlightObjects.Clear();

            var scanWorker = new ScanWorker(inputFileObject);
            scanWorker.RunWorkerAsync();

            while (scanWorker.IsBusy)
            {
                System.Threading.Thread.Sleep(500);
            }

            Logger.Info("Results for " + inputFile.FullName);

            DateTime endScanTime = DateTime.Now;

            TimeSpan scanTimeSpan = endScanTime - startScanTime;

            Logger.Info("Video duration: " + Math.Round(inputFileObject.VideoDurationInSeconds, 2) + "s");
            Logger.Info("Scan time: " + Math.Round(scanTimeSpan.TotalSeconds, 2) + "s");

            Debug.Assert(inputFileObject.FramesPerSecond > 0);
            Debug.Assert(inputFileObject.VideoDurationInSeconds > 0);

            var tr = new TestResult
                         {
                             InputFile = inputFile,
                             ActualScore = 0,
                             DarkTimesFound = new List<string>(),
                             DarkTimesExpected = new List<string>(),
                             FalsePositives = new List<string>(),
                             Matched = new List<string>(),
                             Missing = new List<string>(),
                             ScanTime = scanTimeSpan
                         };

            #region Look up expected dark times

            string expectedDarkTimesFilePath = inputFile.FullName + ".txt";
            var expectedDarkTimes = new string[] { };
            if (File.Exists(expectedDarkTimesFilePath))
            {
                var sr = new StreamReader(expectedDarkTimesFilePath);
                expectedDarkTimes = sr.ReadToEnd().Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            }

            tr.MaxScore = expectedDarkTimes.Length * (int)ScanResultScore.Found;
            tr.DarkTimesExpected = expectedDarkTimes.ToList<string>();

            #endregion

            #region Convert dark times to dark spots

            var expectedMarkTimeSpans = new List<TimeSpan>();
            foreach (string s in expectedDarkTimes)
            {
                TimeSpan ts = TimeSpan.Parse(s); // ex. 0:06

                //expectedMarkTimeSpans.Add((long)(ts.TotalSeconds * inputFileObject.FramesPerSecond));
                expectedMarkTimeSpans.Add(ts);

            }

            Logger.Info("Expected dark times and frames:");
            foreach (var ts in expectedMarkTimeSpans)
            {
                var frame = Math.Round(ts.TotalSeconds * inputFileObject.FramesPerSecond, 0);
                Logger.Info("- " + ts.ToString("c") + " => " + frame);
            }

            #endregion



            Logger.Info("Found dark times and frames:");
            foreach (var highlightObject in MainModel.HighlightObjects)
            {
                tr.DarkTimesFound.Add(highlightObject.BookmarkTime.ToString("c"));
                var frame = Math.Round(highlightObject.BookmarkTime.TotalSeconds * inputFileObject.FramesPerSecond, 0);
                Logger.Info("- " + highlightObject.BookmarkTime.ToString("c") + " => " + frame);
            }



            // If we have any failures at all, the entire test fails. But when debugging, we'll want to know where and how it failed.
            bool testFail = false;



            #region Make sure expected dark spots are near actual dark frames

            Logger.Info("Comparing expected spots with actual spots");

            var scanSummary = new SortedDictionary<TimeSpan, ScanResultScore>();

            const int toleranceInSeconds = 4; // our expected dark frame must be within 3 seconds of actual dark frames
            //var toleranceInFrames = (int)(toleranceInSeconds * inputFileObject.FramesPerSecond);

            foreach (var expectedMarkTimeSpan in expectedMarkTimeSpans)
            {
                var isFound = false;

                foreach (var highlightObject in MainModel.HighlightObjects)
                {

                    if (Math.Abs(highlightObject.BookmarkTime.TotalSeconds - expectedMarkTimeSpan.TotalSeconds) <= toleranceInSeconds)
                    {
                        if (scanSummary.ContainsKey(expectedMarkTimeSpan) == false)
                        {
                            var frame = Math.Round(expectedMarkTimeSpan.TotalSeconds * inputFileObject.FramesPerSecond, 0);
                            Logger.Info("- Success: Found dark spot near " + expectedMarkTimeSpan.ToString("c") + " => frame " + frame);
                            scanSummary.Add(expectedMarkTimeSpan, ScanResultScore.Found);
                            tr.Matched.Add(expectedMarkTimeSpan.ToString("c"));
                            isFound = true;
                        }
                        else
                        { // there's a false positive here. two found bookmarks were near an expected bookmark
                            testFail = true;
                            scanSummary.Add(highlightObject.BookmarkTime, ScanResultScore.FalsePositive);
                            tr.FalsePositives.Add(highlightObject.BookmarkTime.ToString("c"));
                            var frame = Math.Round(highlightObject.BookmarkTime.TotalSeconds * inputFileObject.FramesPerSecond, 0);
                            Logger.Info("- Found false positive at " + highlightObject.BookmarkTime.ToString("c") + " => frame " + frame);
                        }
                        break;

                    }
                }

                if (isFound == false)
                {
                    scanSummary.Add(expectedMarkTimeSpan, ScanResultScore.Missing);
                    testFail = true;
                    tr.Missing.Add(expectedMarkTimeSpan.ToString("c"));
                    var frame = Math.Round(expectedMarkTimeSpan.TotalSeconds * inputFileObject.FramesPerSecond, 0);
                    Logger.Info("- Error! Could not find dark spot near " + expectedMarkTimeSpan.ToString("c") + " => frame " + frame);
                }

            }

            // find the false positives
            foreach (var highlightObject in MainModel.HighlightObjects)
            {
                bool isFound = false;

                foreach (var expectedMarkTimeSpan in expectedMarkTimeSpans)
                {
                    if (Math.Abs(highlightObject.BookmarkTime.TotalSeconds - expectedMarkTimeSpan.TotalSeconds) <= toleranceInSeconds)
                    {
                        isFound = true;
                        break;
                    }
                }

                if (isFound == false)
                {
                    testFail = true;
                    scanSummary.Add(highlightObject.BookmarkTime, ScanResultScore.FalsePositive);
                    tr.FalsePositives.Add(highlightObject.BookmarkTime.ToString("c"));
                    var frame = Math.Round(highlightObject.BookmarkTime.TotalSeconds * inputFileObject.FramesPerSecond, 0);
                    Logger.Info("- Found false positive at " + highlightObject.BookmarkTime.ToString("c") + " => frame " + frame); 
                }
            }

            #endregion


            #region Print summary

            Logger.Info("-------------");
            Logger.Info("Test results summary:");
            foreach (var kvp in scanSummary)
            {
                var frame = Math.Round(kvp.Key.TotalSeconds * inputFileObject.FramesPerSecond, 0);
                Logger.Info("- Time " + kvp.Key.ToString("c") + " (frame " + frame + "): " + kvp.Value.ToString());

                tr.ActualScore += (int)kvp.Value;
            }

            Logger.Info("Max score: " + tr.MaxScore);
            Logger.Info("Actual score: " + tr.ActualScore);
            Logger.Info("Found count: " + tr.Matched.Count);
            Logger.Info("False positive count: " + tr.FalsePositives.Count);
            Logger.Info("Missing count: " + tr.Missing.Count);

            #endregion

            if (testFail)
            {
                tr.Failed = true;
                Logger.Info("Test failed. See debug log for details.");
            }



            return tr;

        }


#if TEST
        public void SetCustomDetectionThreshold(
            int thresholdIndividualPixelBrightness,
            int thresholdDarkPixelsPerFrameAsPercentage,
            int thresholdPixelScanPercentage,
            float thresholdSecondsSkip,
            float thresholdConsecutiveDarkFramesInSeconds
            )
        {

            NativeOdessaMethods.SetCustomDetectionThreshold(
                thresholdIndividualPixelBrightness,
                thresholdDarkPixelsPerFrameAsPercentage,
                thresholdPixelScanPercentage,
                thresholdSecondsSkip,
                thresholdConsecutiveDarkFramesInSeconds);
        }
#endif



    }
}

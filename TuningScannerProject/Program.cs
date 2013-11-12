using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using OdessaPCTestHelpers;
using NLog;

namespace TuningScannerProject
{
    class Program
    {

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args">Engine parameters</param>
        static void Main(string[] args)
        {

            Environment.ExitCode = -1; // assume fail until we reach end

            var findDarkFramesHelper = new FindDarkFramesHelper();

            string currentFileName;
            int currentPassNumber;
            int processIndex;
            int thresholdIndividualPixelBrightness;
            int thresholdDarkPixelsPerFrameAsPercentage;
            int thresholdPixelScanPercentage;
            float thresholdSecondsSkip;
            float thresholdConsecutiveDarkFramesInSeconds;
            
            try
            {
                currentFileName = args[0];
                processIndex = int.Parse(args[1]);
                currentPassNumber = int.Parse(args[2]);

                thresholdIndividualPixelBrightness = int.Parse(args[3]);
                thresholdDarkPixelsPerFrameAsPercentage = int.Parse(args[4]);
                thresholdPixelScanPercentage = int.Parse(args[5]);
                thresholdSecondsSkip = float.Parse(args[6]);
                thresholdConsecutiveDarkFramesInSeconds = float.Parse(args[7]);
                //Logger.InitLogger("Odessa_TuningScanner_" + processIndex + ".log");

#if TEST
                
                findDarkFramesHelper.SetCustomDetectionThreshold(
                    thresholdIndividualPixelBrightness,
                    thresholdDarkPixelsPerFrameAsPercentage,
                    thresholdPixelScanPercentage,
                    thresholdSecondsSkip,
                    thresholdConsecutiveDarkFramesInSeconds
                    );
#else
                Console.WriteLine("THIS ONLY RUNS UNDER TEST CONFIG!!");
                Console.ReadLine();
#endif

            }
            catch (Exception ex)
            {
                Logger.Error("Exception while loading arguments! Closing... " + ex);
                return;
            }

            Logger.Info("{0}, Pass Number: {1}", processIndex, currentPassNumber);
            Logger.Info("{0}, ThresholdIndividualPixelBrightness: {1}", processIndex, thresholdIndividualPixelBrightness);
            Logger.Info("{0}, ThresholdDarkPixelsPerFrameAsPercentage: {1}", processIndex, thresholdDarkPixelsPerFrameAsPercentage);
            Logger.Info("{0}, ThresholdPixelScanPercentage: {1}", processIndex, thresholdPixelScanPercentage);
            Logger.Info("{0}, ThresholdSecondsSkip: {1}", processIndex, thresholdSecondsSkip);
            Logger.Info("{0}, ThresholdConsecutiveDarkFramesInSeconds: {1}", processIndex, thresholdConsecutiveDarkFramesInSeconds);

            Logger.Info("{0}, Scanning {1}", processIndex, currentFileName);

            string filename = (new FileInfo(currentFileName)).Name;

            FindDarkFramesHelper.TestResult tr;
            try
            {
                tr = findDarkFramesHelper.FindDarkFramesTest(filename);
            }
            catch (Exception ex)
            {
                Logger.Error("{0}, Exception writing test results to output file: {1}", processIndex, ex);
                Environment.ExitCode = -1; // fail
                findDarkFramesHelper.Dispose();
                return;
            }

            Logger.Info("{0}, {1} score: {2} out of possible {3}", processIndex, filename, tr.ActualScore, tr.MaxScore);

            try
            {
                // save results where the host process can get it
                Logger.Info("{0}, Writing test results to output file", processIndex);
                Stream stream = File.Open(Path.Combine(Path.GetTempPath(), "Odessa_TestResult_" + processIndex + ".txt"), FileMode.Create);
                var bformatter = new BinaryFormatter();
                bformatter.Serialize(stream, tr);
                stream.Close();
            }
            catch (Exception ex)
            {
                Logger.Error("{0}, Exception writing test results to output file: {1}", processIndex, ex);
                Environment.ExitCode = -1; // fail
            }

            findDarkFramesHelper.Dispose();

            Logger.Info("{0}, Closing down", processIndex);

            Environment.ExitCode = 0; // success

        }

        
        
    }
}

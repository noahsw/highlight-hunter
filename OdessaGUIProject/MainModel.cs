using System;
using System.Collections.Generic;
using System.Deployment.Application;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;
using NLog;
using OdessaGUIProject.Players;

namespace OdessaGUIProject
{
    internal static class MainModel
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        internal static List<HighlightObject> HighlightObjects = new List<HighlightObject>();
        internal static List<InputFileObject> InputFileObjects = new List<InputFileObject>();

        private static bool isAuthorized = false;

        internal static AxQTOControlLib.AxQTControl QuickTimePlayer;

        internal static bool isQuickTimeSupported = true; // this could be false even if isQuickTimeInstalled = true when QuickTime can't play our file
        internal static bool isQuickTimeInstalled = true;

        internal static string ApplicationVersion
        {
            get
            {
                if (ApplicationDeployment.IsNetworkDeployed)
                    return ApplicationDeployment.CurrentDeployment.CurrentVersion.ToString();
                else
                    return Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
        }

        internal static bool IsScanning { get; set; }

        /// <summary>
        /// Unload of the Engine and clean up resources
        /// </summary>
        internal static void Dispose()
        {
            try
            {
                if (QuickTimePlayer != null)
                {
                    QuickTimePlayer.QuickTimeTerminate();
                    QuickTimePlayer.Dispose();
                    QuickTimePlayer = null;
                }

                NativeOdessaMethods.Dispose();
            }
            catch (Exception ex)
            {
                Logger.Error("Exception: " + ex.Message);
            }
        }

        internal static string GetMyVideosDirectory()
        {
            //Environment.GetFolderPath(Environment.SpecialFolder.MyVideos);

            // ReSharper disable InconsistentNaming
            const int CSIDL_MYVIDEO = 0x000e; // "My Videos" folder
            // ReSharper restore InconsistentNaming
            var path = new StringBuilder(260);
            SHGetSpecialFolderPath(IntPtr.Zero, path, CSIDL_MYVIDEO, false);
            return path.ToString();
        }

        /// <summary>
        /// Returns the outputPath to the FFMpeg executable
        /// </summary>
        /// <returns></returns>
        internal static string GetPathToFFmpeg()
        {

            string ffmpegPath = "";
            try
            {
                var thisexe = new FileInfo(Assembly.GetExecutingAssembly().Location);
                if (thisexe.DirectoryName != null) ffmpegPath = Path.Combine(thisexe.DirectoryName, "ffmpeg.exe");
            }
            catch (Exception ex)
            {
                Logger.Error("Exception: " + ex);
            }

            if (File.Exists(ffmpegPath) == false)
            {
                ffmpegPath = Path.Combine(Environment.CurrentDirectory, "ffmpeg.exe"); // fallback
            }

            Logger.Info("Returning " + ffmpegPath);

            return ffmpegPath;
        }

        internal static string GetPathToSampleVideo()
        {
            var thisexe = new FileInfo(Application.ExecutablePath);
            if (thisexe.DirectoryName != null)
            {
                string clipPath = Path.Combine(thisexe.DirectoryName, "Pro snowboarder Justin Morgan.mov");
                return clipPath;
            }

            return "";
        }

        /// <summary>
        /// Get a list of video file extensions from the MIME database (in lower case)
        /// </summary>
        /// <returns></returns>
        internal static List<string> GetVideoExtensions()
        {

            var ret = new List<string>()
                { 
                    ".mp4",
                    ".mov",
                    ".mts",
                    ".m2ts",
                    ".m2t",
                    ".m4v",
                    ".avi"
                };

            return ret;
        }

        internal static void Initialize()
        {
            LoggingHelper.WriteSystemInformation();
            Logger.Info("Path to FFmpeg: " + GetPathToFFmpeg());

            try
            {
                int i = NativeOdessaMethods.Initialize(Path.Combine(Path.GetTempPath(), "HighlightHunter-EngineV2.log"));

                if (i == (int)NativeOdessaMethods.OdessaReturnCodes.Authorized)
                    isAuthorized = true;
                else
                    isAuthorized = false;
            }
            catch (Exception ex)
            {
                Logger.Error("Exception calling NativeOdessaMethods.Initialize(): " + ex.Message);
                Debug.Assert(false, "Exception calling NativeOdessaMethods.Initialize() !!");
            }
        }

        internal static bool IsAuthorized()
        {
            return isAuthorized;
        }

        internal static void LaunchExplorerWithFilesSelected(List<FileInfo> files)
        {
            if (files.Count == 0) // no hands found
                return;

            Logger.Info("Launching explorer with files selected...");

            var filesToSelect = new List<string>();
            foreach (FileInfo file in files)
            {
                filesToSelect.Add(file.FullName);
                Logger.Info("- " + file.FullName);
            }

            Logger.Info("Launching explorer");

            NativeWin32Methods.OpenFolderAndSelectFiles(files[0].DirectoryName, filesToSelect.ToArray());
        }

        /// <summary>
        /// Used for fetching user's video directory. Built in .NET method doesn't work in XP.
        /// </summary>
        /// <param name="hwndOwner"></param>
        /// <param name="lpszPath"></param>
        /// <param name="nFolder"></param>
        /// <param name="fCreate"></param>
        /// <returns></returns>
        [DllImport("shell32.dll")]
        private static extern bool SHGetSpecialFolderPath(IntPtr hwndOwner, [Out] StringBuilder lpszPath, int nFolder,
                                                          bool fCreate);

        #region Threshold Variables

        public enum DetectionThresholds
        {
            Strictest = 0,
            Stricter = 1,
            Medium = 2,
            Looser = 3,
            Loosest = 4,
        }

        #endregion Threshold Variables

        #region Settings

        /// <summary>
        /// How many seconds before the bookmark that the highlight should start
        /// </summary>
        public static int CaptureDurationInSeconds = 30;

        /// <summary>
        /// Whether to ignore highlights in the first 10 seconds of a video
        /// </summary>
        public static bool IgnoreEarlyHighlights = true;

        public static bool UseCaptureOffset = true;

        /// <summary>
        /// The current detection threshold being used
        /// </summary>
        private static DetectionThresholds _detectionThreshold;

        /// <summary>
        /// The available detection thresholds of the scanning engine
        /// </summary>
        public static DetectionThresholds DetectionThreshold
        {
            get { return _detectionThreshold; }
            set
            {
                _detectionThreshold = value;
                NativeOdessaMethods.SetDetectionThreshold((int)value);
            }
        }

        #endregion Settings
    }
}
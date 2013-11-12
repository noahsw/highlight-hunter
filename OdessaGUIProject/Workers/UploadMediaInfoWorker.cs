using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Windows.Forms;
using NLog;
using OdessaGUIProject.Other_Helpers;
using System.Collections.Generic;

namespace OdessaGUIProject.Workers
{
    internal class UploadMediaInfoWorker : BackgroundWorker
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private string currentOutput = "";
        private FileInfo inputFile;
        private NativeOdessaMethods.OdessaReturnCodes odessaReturnCode;
        private string errorCode = "";

        internal UploadMediaInfoWorker(FileInfo inputFile)
        {
            Logger.Info("Starting new UploadMediaInfoWorker on " + inputFile.FullName);
            this.inputFile = inputFile;

            DoWork += new DoWorkEventHandler(UploadMediaInfoWorker_DoWork);
        }

        internal UploadMediaInfoWorker(FileInfo inputFile, NativeOdessaMethods.OdessaReturnCodes odessaReturnCode)
            : this(inputFile)
        {
            this.odessaReturnCode = odessaReturnCode;
        }

        internal UploadMediaInfoWorker(FileInfo inputFile, string errorCode)
            : this(inputFile)
        {
            this.errorCode = errorCode;
        }

        private string GetMediaInfoOutput()
        {
            string mediaInfoPath = GetPathToMediaInfo();
            if (mediaInfoPath == "" || File.Exists(mediaInfoPath) == false)
                return "";

            var mediaInfoProcess = new Process()
            {
                StartInfo =
                {
                    FileName = mediaInfoPath,
                    Arguments = "-f \"" + inputFile.FullName + "\"",
                    UseShellExecute = false,
                    ErrorDialog = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true,
                    WindowStyle = ProcessWindowStyle.Hidden
                },
            };

            mediaInfoProcess.OutputDataReceived += new DataReceivedEventHandler(mediaInfoProcess_OutputDataReceived);

            try
            {
                mediaInfoProcess.Start();

                mediaInfoProcess.BeginOutputReadLine();
                while (true)
                {
                    if (mediaInfoProcess.WaitForExit(10))
                    {
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Exception starting MediaInfo process! " + ex);
                return "";
            }

            return currentOutput;
        }

        private static string GetPathToMediaInfo()
        {
            var thisexe = new FileInfo(Application.ExecutablePath);
            if (thisexe.DirectoryName != null)
            {
                return Path.Combine(thisexe.DirectoryName, "MediaInfo.exe");
            }
            else
                return "";
        }

        private void mediaInfoProcess_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            currentOutput += e.Data + Environment.NewLine;
        }

        private void UploadMediaInfoOutput(string mediaInfoOutput)
        {
            var url = BrowserHelper.Host + "/scanerror-report.php";

            var data = new Dictionary<string, string>();
            if (this.odessaReturnCode != NativeOdessaMethods.OdessaReturnCodes.Success)
                data.Add("odessaReturnCode", this.odessaReturnCode.ToString());
            if (this.errorCode != "")
                data.Add("errorCode", this.errorCode);
            data.Add("message", mediaInfoOutput);

            PostDataHelper.PostData(url, data, true);

            Logger.Info("MediaInfo uploaded");
        }

        private void UploadMediaInfoWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            string mediaInfoOutput = GetMediaInfoOutput();

            if (!String.IsNullOrEmpty(mediaInfoOutput))
                UploadMediaInfoOutput(mediaInfoOutput);
        }
    }
}
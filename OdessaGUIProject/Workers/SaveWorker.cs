using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using GaDotNet.Common.Helpers;
using NLog;
using OdessaGUIProject.DRM_Helpers;
using OdessaGUIProject.Properties;
using System.Threading;

namespace OdessaGUIProject.Workers
{
    internal class SaveWorker : PublishWorker
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        internal SaveWorker(HighlightObject highlightObject)
            : base(highlightObject)
        {
            
            double framesToSave = (highlightObject.EndTime - highlightObject.StartTime).TotalSeconds * highlightObject.InputFileObject.FramesPerSecond;
            TotalProgressUnits = framesToSave;

            OutputFormat = OutputFormats.Original; // default
        }

        internal enum OutputFormats
        {
            Original,
            ProRes,
            Facebook, // used for Facebook
        }

        internal bool HideOutputFile { get; set; }

        internal OutputFormats OutputFormat { get; set; }

        internal bool ForceWatermark { get; set; }

        private string FfmpegOutput { get; set; }

        private string FfmpegArguments { get; set; }

        internal override void PublishWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            ReportProgress(0);
            
            #region Track analytics
#if !TEST

            if (OutputFormat != OutputFormats.Facebook)
            { // we don't want to track when facebook sharer uses this
                
                if (activationState == Protection.ActivationState.Activated)
                    AnalyticsHelper.FireEvent("Each save - activation state - activated");
                else if (activationState == Protection.ActivationState.Trial)
                    AnalyticsHelper.FireEvent("Each save - activation state - trial");
                else if (activationState == Protection.ActivationState.TrialExpired)
                    AnalyticsHelper.FireEvent("Each save - activation state - trial expired");
                else if (activationState == Protection.ActivationState.Unlicensed)
                    AnalyticsHelper.FireEvent("Each save - activation state - unlicensed");

                if (Properties.Settings.Default.HasSavedVideo == false)
                {
                    if (AnalyticsHelper.FireEvent("First save"))
                    {
                        Properties.Settings.Default.HasSavedVideo = true;
                        Properties.Settings.Default.Save();
                    }
                }
            }


#endif
            #endregion Track analytics

            var outputFilePath = (string)e.Argument;

            var outputFileInfo = SpliceVideo(outputFilePath);

            if (PublishWorkerResult == PublishWorkerResults.Success)
                OutputFileInfo = outputFileInfo;
            else if (PublishWorkerResult != PublishWorkerResults.Cancelled)
                UploadMediaInfo();

            if (OutputFormat != OutputFormats.Facebook)
            { // we don't want to track when facebook sharer uses this
                AnalyticsHelper.FireEvent("Each save - result - " + GetFriendlyPublishWorkerResult());
            }

        }

        

        internal override bool ViewResult(ref string errorMessage)
        {
            var process = new Process();
            process.StartInfo.FileName = OutputFileInfo.FullName;

            try
            {
                process.Start();
                return true;
            }
            catch (Win32Exception ex)
            {
                errorMessage = "Please make sure you have a video player that can open " + OutputFileInfo.Name + ".";
                Logger.Error("Could not open saved video! " + ex);
                return false;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                Logger.Error("Could not open saved video! " + ex);
                return false;
            }

        }

        private static void SaveWatermarkToDisk(int videoWidth, int videoHeight, string watermarkPath)
        {
            Bitmap bm;

            /* We now generate bitmap on the fly
            if (videoWidth == 848 && videoHeight == 480)
                bm = Resources.watermark_848x480;
            else if (videoWidth == 1280 && videoHeight == 720)
                bm = Resources.watermark_1280x720;
            else if (videoWidth == 1280 && videoHeight == 960)
                bm = Resources.watermark_1280x960;
            else if (videoWidth == 1920 && videoHeight == 1080)
                bm = Resources.watermark_1920x1080;
            else if (videoWidth == 1920 && videoHeight == 1088)
                bm = Resources.watermark_1920x1088;
            else
                bm = Resources.watermark_848x480;
            */

            // take off 16 pixels because sometimes we think the dimensions are longer than it really is. this is an ffmpeg bug.
            videoWidth -= 16;
            videoHeight -= 16;

            int shortestSide;
            if (videoWidth > videoHeight)
                shortestSide = videoHeight;
            else
                shortestSide = videoWidth;

            var logoDimension = (int)(shortestSide * 0.05);

            Image logo;
            if (logoDimension <= 16)
                logo = Resources.watermark_16x16;
            else if (logoDimension <= 24)
                logo = Resources.watermark_24x24;
            else if (logoDimension <= 36)
                logo = Resources.watermark_36x36;
            else if (logoDimension <= 48)
                logo = Resources.watermark_48x48;
            else if (logoDimension <= 54)
                logo = Resources.watermark_54x54;
            else
                logo = Resources.watermark_64x64;

            var logoPoint = new Point(videoWidth - logo.Width * 2, videoHeight - logo.Height * 2);

            var image = new Bitmap(videoWidth, videoHeight, PixelFormat.Format32bppArgb);
            using (var g = Graphics.FromImage(image))
            {
                g.DrawImageUnscaled(logo, logoPoint);
            }

            bm = image;

            #region The MD5 hash is being computed differently on Win7 and WinXP. Until I figure this out, I'm disabling it

            /*

            // verify valid CRCs from Bitmap object
            var converter = new ImageConverter();

            string contents = converter.ConvertToInvariantString(bm);
            Logger.Info("Contents: " + contents);

            var rawImageData = converter.ConvertTo(bm, typeof(byte[])) as byte[];
            if (rawImageData == null)
                throw new Exception("Watermarks have been tampered with!");

            //byte[] rawImageData = File.ReadAllBytes(watermarkPath);

            var md5 = new MD5CryptoServiceProvider();
            byte[] data = md5.ComputeHash(rawImageData);
            var thisCRC = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                thisCRC.Append(data[i].ToString("X2"));
            }

#if DEBUG
            Logger.Info("Hash: " + thisCRC);
#endif

            const string crc848X480 = "FD84A7D38F05C1E189FDC7FDDF34DF8";
            const string crc1280X720 = "132670E229B21145AAB77047CCCCFE";
            const string crc1280X960 = "321777DBE5FAC821D45A2FB92EB63869";
            const string crc1920X1080 = "699AC9FB4751E9A83D6BAB21DB3270";
            const string crc1920X1088 = "699AC9FB4751E9A83D6BAB21DB3270";

            bool isCRCValid;
            if (videoWidth == 848 && videoHeight == 480)
                isCRCValid = (thisCRC.ToString() == crc848X480);
            else if (videoWidth == 1280 && videoHeight == 720)
                isCRCValid = (thisCRC.ToString() == crc1280X720);
            else if (videoWidth == 1280 && videoHeight == 960)
                isCRCValid = (thisCRC.ToString() == crc1280X960);
            else if (videoWidth == 1920 && videoHeight == 1080)
                isCRCValid = (thisCRC.ToString() == crc1920X1080);
            else if (videoWidth == 1920 && videoHeight == 1088)
                isCRCValid = (thisCRC.ToString() == crc1920X1088);
            else
                isCRCValid = (thisCRC.ToString() == crc848X480);

            if (isCRCValid == false)
            {
                throw new Exception("Watermarks have been tampered with!");
            }

             */

            #endregion The MD5 hash is being computed differently on Win7 and WinXP. Until I figure this out, I'm disabling it

            // watermarks passed CRC check
            try
            {
#if DEBUG
                Logger.Info("Saving to " + watermarkPath);
#endif
                bm.Save(watermarkPath, ImageFormat.Png);
            }
            catch (System.Runtime.InteropServices.ExternalException ex)
            {
                Logger.Error("Exception while saving: " + ex);
            }
        }

        private bool IsValidVideo(FileInfo fileInfo)
        {
            if (fileInfo == null)
                return false;

            if (OutputFormat == OutputFormats.ProRes)
            {
                return fileInfo.Length > 1024 * 1024; // 1mb in size
            }
            else
            {

                Thread.Sleep(1000); // let things settle. sometimes we can't read the video dimensions and hopefully this will help that.

                var inputFileObject = new InputFileObject(fileInfo);
                using (var scanWorker = new ScanWorker(inputFileObject))
                {
                    scanWorker.SetVideoDimensions();
                }

                Debug.Assert(inputFileObject.VideoWidth > 0 && inputFileObject.VideoHeight > 0, "Cannot determine dimensions of " + inputFileObject.SourceFileInfo.FullName);

                return (inputFileObject.VideoWidth > 0 && inputFileObject.VideoHeight > 0);
            }

        }

        /// <summary>
        /// Splices out a new highlight video and saves it to the OutputDirectory
        /// </summary>
        /// <param name="outputFilePath"></param>
        /// <param name="isReencoding">Refers to whether we should re-encode. Needed because some formats need to be re-encoded.</param>
        internal FileInfo SpliceVideo(string outputFilePath, bool isReencoding = false)
        {

            if (HideOutputFile)
                Logger.Info(Id + ": Called with isReencoding=" + isReencoding);
            else
                Logger.Info(Id + ": Called with outputFilePath=" + outputFilePath + " and isReencoding=" + isReencoding);

            if (MainModel.IsAuthorized() == false)
            {
                Logger.Error("Highlight Hunter Engine Not Authorized!");
                ErrorMessage = "Corrupt installation";
                PublishWorkerResult = PublishWorkerResults.UnableToSplice;
                return null;
            }

            if (Math.Abs(HighlightObject.InputFileObject.FramesPerSecond - 0) < Double.Epsilon)
            {
                Logger.Error("FramesPerSecond equals zero!");
                ErrorMessage = "Could not retrieve frame rate of video.";
                PublishWorkerResult = PublishWorkerResults.UnableToSplice;
                return null;
            }

            if (HighlightObject.InputFileObject.Bitrate == 0)
            {
                Logger.Error("Bitrate equals zero!");
                ErrorMessage = "Could not retrieve bitrate of video.";
                PublishWorkerResult = PublishWorkerResults.UnableToSplice;
                return null;
            }

            var isWatermarked = false;

#if !TEST

            #region Look up activation status

            if (activationState == Protection.ActivationState.Unlicensed ||
                activationState == Protection.ActivationState.TrialExpired)
                isWatermarked = true;

            
            #endregion Look up activation status
#else
            isWatermarked = ForceWatermark;
#endif


            var start = HighlightObject.StartTime;

            var end = HighlightObject.EndTime;

            var duration = end - start;

#if !TEST
            if (activationState == Protection.ActivationState.Activated || activationState == Protection.ActivationState.Trial)
            {
                string ssAsString = start.Hours.ToString("00", CultureInfo.InvariantCulture) + ":" + start.Minutes.ToString("00", CultureInfo.InvariantCulture) + ":" +
                                start.Seconds.ToString("00", CultureInfo.InvariantCulture);
                string endAsString = end.Hours.ToString("00", CultureInfo.InvariantCulture) + ":" + end.Minutes.ToString("00", CultureInfo.InvariantCulture) + ":" +
                                     end.Seconds.ToString("00", CultureInfo.InvariantCulture);

                Logger.Info("Splicing from " + ssAsString + " to " + endAsString);
            }
#endif

            if (!HideOutputFile)
                Logger.Info("Writing to: " + outputFilePath);

            // set working together to temp outputPath so the OdessaWatermark works
            var splicingProcess = new Process
                                      {
                                          StartInfo =
                                              {
                                                  FileName = MainModel.GetPathToFFmpeg(),
                                                  WorkingDirectory = Path.GetTempPath(),
                                              }
                                      };

            splicingProcess.StartInfo.Arguments = 
                "-ss " + start.TotalSeconds.ToString(CultureInfo.InvariantCulture) + " " + 
                "-t " + duration.TotalSeconds.ToString(CultureInfo.InvariantCulture) + " " +
                "-y " + // -y overwrites output
                "-i \"" + HighlightObject.InputFileObject.SourceFileInfo.FullName + "\" " +
                "-threads 3 " +
                "-acodec copy " +
                "-copyts " + // Copy timestamps from input to output. (helps with audio syncing)
                "-copytb -1 "; // Specify how to set the encoder timebase when stream copying. mode is an integer numeric value, and can assume one of the following values: -1 Try to make the choice automatically, in order to generate a sane output.

            ReportProgress(1);

            string watermarkPath = Path.Combine(Path.GetTempPath(), "OdessaWatermark" + Id + ".png"); // Include Id because we may be saving multiple files at the same time
            if (isWatermarked && OutputFormat != SaveWorker.OutputFormats.Facebook)
            {
                #region Save watermark to disk

                try
                {
                    SaveWatermarkToDisk(HighlightObject.InputFileObject.VideoWidth, HighlightObject.InputFileObject.VideoHeight, watermarkPath);
                }
                catch (Exception ex)
                {
                    // the user might have done something to prevent the watermark from being saved
                    Logger.Error("Exception while saving watermark to disk: " + ex);

                    #region delete temporary watermark file on disk

                    if (File.Exists(watermarkPath))
                    {
                        try
                        {
                            File.Delete(watermarkPath);
                        }
                        catch
                        {
                            Debug.Assert(true, "Couldn't delete watermark outputPath at " + watermarkPath);
                        }
                    }

                    #endregion delete temporary watermark file on disk

                    ErrorMessage = "Corrupt installation.";
                    PublishWorkerResult = PublishWorkerResults.UnableToSplice;
                    return null;
                }
                if (File.Exists(watermarkPath) == false)
                {
                    Logger.Error("Watermark was not saved to disk!");
                    ErrorMessage = "Corrupt installation.";
                    PublishWorkerResult = PublishWorkerResults.UnableToSplice;
                    return null;
                }

                #endregion Save watermark to disk

                splicingProcess.StartInfo.Arguments +=
                    "-vf \"movie=OdessaWatermark" + Id + ".png [watermark]; [in][watermark]overlay=0:0 [out]\" ";
            }

            ReportProgress(2);

            #region Figure out -vcodec, -b, and -r param

            var needsSpecificBitrateAndFPS = true;

            var frameRate = HighlightObject.InputFileObject.FramesPerSecond;
            switch (OutputFormat)
            {
                case OutputFormats.Original:
                    if (!isWatermarked && !isReencoding)
                    {
                        needsSpecificBitrateAndFPS = false;
                        splicingProcess.StartInfo.Arguments += "-vcodec copy ";
                    }
                    else
                    {
                        needsSpecificBitrateAndFPS = true;
                        // don't need to specify vcodec because ffmpeg will use extension of file
                    }
                    break;

                case OutputFormats.Facebook:
                    needsSpecificBitrateAndFPS = true;
                    // according to https://www.facebook.com/help/?faq=124738474272230, we should keep:

                    // encode to H.264
                    // ffmpeg doesn't have an LGPL h.264 encoder so we'll encode to MPEG4 part 2 instead
                    // TODO: use GPL h.264 encoder instead?
                    splicingProcess.StartInfo.Arguments += "-vcodec libx264 ";

                    // FPS to 30 or under
                    if (frameRate > 30)
                        frameRate = 30;

                    // keep max dimension to 1280
                    var videoHeight = HighlightObject.InputFileObject.VideoHeight;
                    var videoWidth = HighlightObject.InputFileObject.VideoWidth;
                    var isResized = false;
                    if (videoWidth > 1280)
                    {
                        videoHeight = (int)(videoHeight * 1280.0 / videoWidth);
                        videoWidth = 1280;
                        isResized = true;
                    }
                    if (videoHeight > 1280)
                    {
                        videoWidth = (int)(videoWidth * 1280.0 / videoHeight);
                        videoHeight = 1280;
                        isResized = true;
                    }

                    // make sure height and width are always divisible by 2
                    if (videoWidth % 2 > 0)
                        videoWidth -= 1;
                    if (videoHeight % 2 > 0)
                        videoHeight -= 1;

                    if (isResized)
                        splicingProcess.StartInfo.Arguments += "-s " + videoWidth.ToString(CultureInfo.InvariantCulture) + "x" + videoHeight.ToString(CultureInfo.InvariantCulture) + " ";
                    break;

                case OutputFormats.ProRes:
                    needsSpecificBitrateAndFPS = false;
                    splicingProcess.StartInfo.Arguments += "-vcodec prores ";
                    break;
            }

            if (needsSpecificBitrateAndFPS)
                splicingProcess.StartInfo.Arguments +=
                    "-r " + frameRate.ToString("0.00", CultureInfo.InvariantCulture) +
                    " -b:v " + HighlightObject.InputFileObject.Bitrate.ToString("0", CultureInfo.InvariantCulture) + " ";

            #endregion Figure out -vcodec, -b, and -r param

            #region Verify validity of outputFilePath

            if (OutputFormat == OutputFormats.ProRes)
            { // make sure it has .MOV extension
                if (Path.GetExtension(outputFilePath).ToUpperInvariant() != ".MOV")
                    outputFilePath += ".MOV";
            }
            
            #endregion

            splicingProcess.StartInfo.Arguments += "\"" + outputFilePath + "\"";
            splicingProcess.StartInfo.UseShellExecute = false;
            splicingProcess.StartInfo.ErrorDialog = false;

            splicingProcess.StartInfo.RedirectStandardError = true;
            splicingProcess.StartInfo.CreateNoWindow = true;
            splicingProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;

            splicingProcess.ErrorDataReceived += SplicingProcessOutputDataReceived;

            FfmpegArguments = splicingProcess.StartInfo.Arguments;

            if (CheckForCancelledWorker()) return null;

            if (!isWatermarked && !HideOutputFile)
                Logger.Info("Calling ffmpeg with arguments: " + splicingProcess.StartInfo.FileName + " " +
                    splicingProcess.StartInfo.Arguments);

#if DEBUG
            Logger.Info("Calling ffmpeg with arguments: " + splicingProcess.StartInfo.FileName + " " +
                splicingProcess.StartInfo.Arguments);
#endif

            try
            {
                splicingProcess.Start();
                //SplicingProcess.PriorityClass = ProcessPriorityClass.BelowNormal;

                ReportProgress(3);

                splicingProcess.BeginErrorReadLine();
                while (true)
                {
                    if (CheckForCancelledWorker())
                    {
                        splicingProcess.Kill();
                        break;
                    }

                    if (splicingProcess.WaitForExit(100))
                    {
                        break;
                    }
                }

                ReportProgress(100);

                Logger.Info("Ffmpeg finished");

            }
            catch (Exception ex)
            {
                Logger.Error("Exception running ffmpeg process! " + ex);
                ErrorMessage = "Could not start splicing process. " + ex;
                PublishWorkerResult = PublishWorkerResults.UnableToSplice;
                return null;
            }

            #region delete temporary watermark file on disk

            if (File.Exists(watermarkPath))
            {
                try
                {
                    File.Delete(watermarkPath);
                }
                catch
                {
                    Debug.Assert(true, "Couldn't delete watermark outputPath at " + watermarkPath);
                }
            }

            #endregion delete temporary watermark file on disk

            if (CheckForCancelledWorker()) return null;

            if (File.Exists(outputFilePath))
            {
                try
                {
                    var outputFileInfo = new FileInfo(outputFilePath);

                    // see if it's a valid output file. if the file is under 1mb, something is wrong.
                    // this may not be the best way to detect a bad output file. a better way might be to read ffmpeg output
                    if (IsValidVideo(outputFileInfo))
                    {
                        PublishWorkerResult = PublishWorkerResults.Success;
                        return outputFileInfo;
                    }
                    else
                    {
                        if (!isReencoding)
                        {
                            Logger.Info("Output file doesn't seem valid. Let's try reencoding.");
                            outputFileInfo = SpliceVideo(outputFilePath, isReencoding: true);
                            if (!IsValidVideo(outputFileInfo))
                            {
                                Logger.Info("Even re-encoding didn't work. Something else is wrong!");
                                PublishWorkerResult = PublishWorkerResults.UnableToSplice;
                                ErrorMessage = "Error while saving video";
                                return null;
                            }
                            else
                            {   // PublishWorkerResult would be set by second SpliceVideo call
                                return outputFileInfo;
                            }
                        }
                        else
                        { // we have no other ways to do this
                            Logger.Error("Reencoding failed as well");
                            PublishWorkerResult = PublishWorkerResults.UnableToSplice;
                            ErrorMessage = "Error while saving video";
                            return null;
                        }
                    }
                }
                catch (Exception ex)
                {
                    if (!HideOutputFile)
                        Logger.Error("Exception returning OutputFilePath of " + outputFilePath + ": " + ex);

                    ErrorMessage = "Could not fetch OutputFilePath";
                    PublishWorkerResult = PublishWorkerResults.UnableToSplice;
                    return null;
                }
            }
            else
            {
                Logger.Error("Output file not found");
                ErrorMessage = "Could not fetch OutputFilePath";
                PublishWorkerResult = PublishWorkerResults.UnableToSplice;
                return null;
            }
        }

        private void SplicingProcessOutputDataReceived(object sendingProcess, DataReceivedEventArgs outLine)
        {
            if (!String.IsNullOrEmpty(outLine.Data))
            {
                // Add the text to the collected output.
                //SplicingProcessOutput += outLine.Data + Environment.NewLine;

                // frame=  212 fps=6.1 q=0.0 size=   77812kB time=00:00:03.53 bitrate=180225.2kbits
                var fpsIndex = outLine.Data.IndexOf(" fps=", StringComparison.Ordinal);
                if (fpsIndex > 0)
                {
                    var beforeFps = outLine.Data.Substring(0, fpsIndex);
                    var frameIndex = beforeFps.LastIndexOf(" ", StringComparison.Ordinal);
                    if (frameIndex <= 0)
                        frameIndex = beforeFps.LastIndexOf("=", StringComparison.Ordinal);
                    if (frameIndex > 0)
                    {
                        var frameAsString = beforeFps.Substring(frameIndex + 1);
                        int processedFrames;
                        if (Int32.TryParse(frameAsString, out processedFrames))
                        {
                            var newProgress = (int)(100.0 * processedFrames / TotalProgressUnits);
                            if (newProgress > 3) // so progress doesn't go down, since SpliceVideo already puts this to 3
                                ReportProgress(newProgress);
                        }
                    }
                }

                if (!HideOutputFile)
                    Logger.Info("ffmpeg output: " + outLine.Data);

                FfmpegOutput += outLine.Data + Environment.NewLine;
            }
        }

        private void UploadMediaInfo()
        {
            var errorCode = "Error Message: " + ErrorMessage + "\n\n" +
                            "Ffmpeg Arguments: " + FfmpegArguments + "\n\n" +
                            "Ffmpeg Output: " + FfmpegOutput;
            var uploadMediaInfoWorker = new UploadMediaInfoWorker(HighlightObject.InputFileObject.SourceFileInfo,
                errorCode);
            uploadMediaInfoWorker.RunWorkerAsync();
            // we don't care when this thing ends
        }
    }
}
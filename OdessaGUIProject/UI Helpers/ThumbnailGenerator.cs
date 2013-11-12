using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using NLog;
using System.Globalization;

namespace OdessaGUIProject
{
    internal static class ThumbnailGenerator
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private static bool _isWorking = false;
        private static readonly object locker = new object();

        private static Queue<ThumbnailQueueItem> ThumbnailQueue = new Queue<ThumbnailQueueItem>();

        internal static void AddToQueue(ThumbnailQueueItem thumbnailQueueItem)
        {
            Debug.WriteLine("AddToQueue");

            lock (locker)
            {
                ThumbnailQueue.Enqueue(thumbnailQueueItem);

                if (_isWorking == false)
                {
                    _isWorking = true;
                    RunWorker();
                }

                //GeneratorTimer_Elapsed(null, null);
            }
        }

        private static Image GenerateThumbnail(ThumbnailQueueItem thumbnailQueueItem)
        {
            Image ret = null;

            Guid guid = Guid.NewGuid();
            string thumbFilePath = Path.Combine(Path.GetTempPath(), "HH-thumbs-" + guid.ToString() + ".png");

            Logger.Info("Generating: " + thumbFilePath);

            var thumbnailProcess = new Process();
            thumbnailProcess.StartInfo.FileName = MainModel.GetPathToFFmpeg();
            thumbnailProcess.StartInfo.Arguments = "-ss " + thumbnailQueueItem.SeekInSeconds.ToString(CultureInfo.InvariantCulture) + " -t 1 " +
                "-i \"" + thumbnailQueueItem.SourceFileInfo.FullName + "\" -y -vframes 1 -filter:v yadif -an " +
                "-s " + thumbnailQueueItem.Size.Width.ToString(CultureInfo.InvariantCulture) + "x" + thumbnailQueueItem.Size.Height.ToString(CultureInfo.InvariantCulture) + " \"" + thumbFilePath + "\"";
            /*
             * -t 1         duration of 1 second
             * -y           overwrite existing file
             * -filter:v yadif  deinterlace
             * -vframes 1   record 1 video frame
             * -an          disable audio recording
             * -s           size of thumbnail
             */
            thumbnailProcess.StartInfo.UseShellExecute = false;
            thumbnailProcess.StartInfo.CreateNoWindow = true;
            thumbnailProcess.StartInfo.RedirectStandardError = true;
            try
            {
                Logger.Debug("Starting with arguments: " + thumbnailProcess.StartInfo.Arguments);
                thumbnailProcess.Start();
                thumbnailProcess.WaitForExit();
                //Logger.Debug("FFmpeg output: " + thumbnailProcess.StandardError.ReadToEnd());

                byte[] buffer = File.ReadAllBytes(thumbFilePath);
                MemoryStream ms = new MemoryStream(buffer);
                ret = Image.FromStream(ms);
                File.Delete(thumbFilePath);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                // ret = backup image
            }

            return ret;
        }

        private static void RunWorker()
        {
            BackgroundWorker thumbnailGeneratorWorker = new BackgroundWorker();
            thumbnailGeneratorWorker.DoWork += new DoWorkEventHandler(thumbnailGeneratorWorker_DoWork);
            thumbnailGeneratorWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(thumbnailGeneratorWorker_RunWorkerCompleted);
            try
            {
                thumbnailGeneratorWorker.RunWorkerAsync(ThumbnailQueue.Dequeue());
            }
            catch (Exception ex)
            {
                Logger.Error("Error running thumbnail generator worker: " + ex);
            }
        }

        private static void thumbnailGeneratorWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            ThumbnailQueueItem thumbnailQueueItem = (ThumbnailQueueItem)e.Argument;
            thumbnailQueueItem.Thumbnail = GenerateThumbnail(thumbnailQueueItem);
            thumbnailQueueItem.FireThumbnailGeneratedEventHandler();
        }

        private static void thumbnailGeneratorWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (ThumbnailQueue.Count > 0)
                RunWorker();
            else
                _isWorking = false;
        }
    }

    internal class ThumbnailQueueItem
    {
        internal double SeekInSeconds = 1;
        internal Size Size;
        internal FileInfo SourceFileInfo;
        internal Image Thumbnail;

        internal delegate void ThumbnailGeneratedEventHandler(object sender, Image thumbnail);

        internal event ThumbnailGeneratedEventHandler ThumbnailGenerated;

        // ffmpeg doesn't like seeking to 0
        internal void FireThumbnailGeneratedEventHandler()
        {
            if (ThumbnailGenerated != null)
                ThumbnailGenerated(this, Thumbnail);
        }
    }
}
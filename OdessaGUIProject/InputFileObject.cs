using System.IO;
using OdessaGUIProject.Workers;

namespace OdessaGUIProject
{
    internal class InputFileObject
    {
        internal InputFileObject()
        {
            ScanWorkerResult = ScanWorker.ScanWorkerResults.NotFinished;
        }

        internal InputFileObject(FileInfo sourceFileInfo)
            : base()
        {
            SourceFileInfo = sourceFileInfo;
        }

        internal int Bitrate { get; set; }

        internal string Codec { get; set; }

        internal double FramesPerSecond { get; set; }

        internal ScanWorker ScanWorker { get; set; }

        internal ScanWorker.ScanWorkerResults ScanWorkerResult { get; set; }

        internal FileInfo SourceFileInfo { get; set; }

        internal long TotalFrames { get; set; }

        internal double VideoDurationInSeconds { get; set; } // double because that's what we get from ffmpeg

        internal int VideoHeight { get; set; }

        internal int VideoWidth { get; set; }
    }
}
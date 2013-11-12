using System;
using System.IO;
using System.Text;
using GaDotNet.Common.Helpers;
using NLog;

namespace OdessaGUIProject.Workers
{
    internal class EdlExportWorker : AbstractExportWorker
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        internal override string AppName
        {
            get { return "Sony Vegas"; }
        }

        internal override bool ExportHighlightsToFile(string outputPath)
        {
            bool success = true;

            StringBuilder sb = new StringBuilder();

            int id = 1;

            sb.AppendLine(
                @"""ID"";""Track"";""StartTime"";""Length"";""PlayRate"";""Locked"";""Normalized"";""StretchMethod"";""Looped"";""OnRuler"";""MediaType"";""FileName"";""Stream"";""StreamStart"";""StreamLength"";""FadeTimeIn"";""FadeTimeOut"";""SustainGain"";""CurveIn"";""GainIn"";""CurveOut"";""GainOut"";""Layer"";""Color"";""CurveInR"";""CurveOutR"";""PlayPitch"";""LockPitch"";""FirstChannel"";""Channels""");

            #region Video

            var startTime = 0.0;

            for (int i = 0; i < MainModel.HighlightObjects.Count; i++)
            {
                var highlightObject = MainModel.HighlightObjects[i];

                var track = 1;
                var length = (highlightObject.EndTime - highlightObject.StartTime).TotalSeconds * 1000;
                var playRate = "1.000000";
                var locked = "FALSE";
                var normalized = "FALSE";
                var stretchMethod = "0";
                var looped = "TRUE";
                var onRuler = "FALSE";
                var mediaType = "VIDEO";
                var fileName = "\"" + highlightObject.InputFileObject.SourceFileInfo.FullName + "\"";
                var stream = "0";
                var streamStart = highlightObject.StartTime.TotalSeconds * 1000;
                var streamLength = length;
                var fadeTimeIn = "0.0000";
                var fadeTimeOut = "0.0000";
                var sustainGain = "1.000000";
                var curveIn = "4";
                var gainIn = "0.000000";
                var layer = "0";
                var color = "-1";
                var curveInR = "4";
                var curveOutR = "4";
                var playPitch = "0.000000";
                var lockPitch = "FALSE";
                var firstChannel = "0";
                var channels = "0";

                sb.AppendLine(
                    id + "; " +
                    track + "; " +
                    startTime + "; " +
                    length + "; " +
                    playRate + "; " +
                    locked + "; " +
                    normalized + "; " +
                    stretchMethod + "; " +
                    looped + "; " +
                    onRuler + "; " +
                    mediaType + "; " +
                    fileName + "; " +
                    stream + "; " +
                    streamStart + "; " +
                    streamLength + "; " +
                    fadeTimeIn + "; " +
                    fadeTimeOut + "; " +
                    sustainGain + "; " +
                    curveIn + "; " +
                    gainIn + "; " +
                    layer + "; " +
                    color + "; " +
                    curveInR + "; " +
                    curveOutR + "; " +
                    playPitch + "; " +
                    lockPitch + "; " +
                    firstChannel + "; " +
                    channels
                    );

                startTime += length;

                id += 1;
            }

            #endregion Video

            #region Audio

            startTime = 0.0;

            for (int i = 0; i < MainModel.HighlightObjects.Count; i++)
            {
                var highlightObject = MainModel.HighlightObjects[i];

                var track = "0";
                var length = (highlightObject.EndTime - highlightObject.StartTime).TotalSeconds * 1000;
                var playRate = "1.000000";
                var locked = "FALSE";
                var normalized = "FALSE";
                var stretchMethod = "0";
                var looped = "TRUE";
                var onRuler = "FALSE";
                var mediaType = "AUDIO";
                var fileName = "\"" + highlightObject.InputFileObject.SourceFileInfo.FullName + "\"";
                var stream = "0";
                var streamStart = highlightObject.StartTime.TotalSeconds * 1000;
                var streamLength = length;
                var fadeTimeIn = "0.0000";
                var fadeTimeOut = "0.0000";
                var sustainGain = "1.000000";
                var curveIn = "4";
                var gainIn = "0.000000";
                var layer = "0";
                var color = "-1";
                var curveInR = "4";
                var curveOutR = "4";
                var playPitch = "0.000000";
                var lockPitch = "FALSE";
                var firstChannel = "0";
                var channels = "2";

                sb.AppendLine(
                    id + "; " +
                    track + "; " +
                    startTime + "; " +
                    length + "; " +
                    playRate + "; " +
                    locked + "; " +
                    normalized + "; " +
                    stretchMethod + "; " +
                    looped + "; " +
                    onRuler + "; " +
                    mediaType + "; " +
                    fileName + "; " +
                    stream + "; " +
                    streamStart + "; " +
                    streamLength + "; " +
                    fadeTimeIn + "; " +
                    fadeTimeOut + "; " +
                    sustainGain + "; " +
                    curveIn + "; " +
                    gainIn + "; " +
                    layer + "; " +
                    color + "; " +
                    curveInR + "; " +
                    curveOutR + "; " +
                    playPitch + "; " +
                    lockPitch + "; " +
                    firstChannel + "; " +
                    channels
                    );

                startTime += length;

                id += 1;
            }

            #endregion Audio

            try
            {
                File.WriteAllText(outputPath, sb.ToString());
                ProjectFileLocation = outputPath;

                AnalyticsHelper.FireEvent("Export - EDL");

                success = true;
            }
            catch (Exception ex)
            {
                Logger.Error("Error writing to file: " + ex);
                success = false;
            }

            return success;
        }

        internal override string GetPathToApp()
        {
            return "";
        }

        internal override string GetProjectFileExtension()
        {
            return ".txt";
        }

        internal override string GetProjectFilename()
        {
            return "My Highlights.txt";
        }

        internal override string GetSaveDialogFilter()
        {
            return "EDL (*.txt)|*.txt|All Files (*.*)|*.*";
        }

        internal override string LaunchAppMessage()
        {
            return "1. Open video editor" + Environment.NewLine +
                "2. Go to File \\ Import..." + Environment.NewLine +
                "3. Select the EDL file at " + ProjectFileLocation;
        }

        internal override bool SupportsLaunchingByExtension()
        {
            return false;
        }
    }
}
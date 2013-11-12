using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using GaDotNet.Common.Helpers;
using NLog;

namespace OdessaGUIProject.Workers
{
    internal class AvidStudioExportWorker : AbstractExportWorker
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        internal override bool ExportHighlightsToFile(string outputPath)
        {
            bool success = true;

            StringBuilder sb = new StringBuilder();

            int masterClipId = 1;

            sb.Append(
                @"<?xml version=""1.0"" encoding=""UTF-8""?>
<!DOCTYPE xmeml>
<xmeml version=""4"">
<project>
	<name>Highlights from Highlight Hunter</name>
<children>
");

            for (int i = 0; i < MainModel.HighlightObjects.Count; i++)
            {
                var highlightObject = MainModel.HighlightObjects[i];

                var uuid = (Guid.NewGuid()).ToString();
                var duration = highlightObject.InputFileObject.TotalFrames;
                var timebase = CalculateTimebase(highlightObject.InputFileObject.FramesPerSecond);
                var isNTSC = CalculateIsNTSC(highlightObject.InputFileObject.FramesPerSecond);
                int inFrame = (int)(highlightObject.StartTime.TotalSeconds * highlightObject.InputFileObject.FramesPerSecond);
                int outFrame = (int)(highlightObject.EndTime.TotalSeconds * highlightObject.InputFileObject.FramesPerSecond);
                var clipName = highlightObject.Title + " " + masterClipId;

                var pathURL = GeneratePathURL(highlightObject.InputFileObject.SourceFileInfo.FullName);

                sb.Append(
                    @"<clip id=""masterclip-" + masterClipId + @""">
	<uuid>" + uuid + @"</uuid>
	<masterclipid>masterclip-" + masterClipId + @"</masterclipid>
	<ismasterclip>TRUE</ismasterclip>
	<duration>" + duration + @"</duration><rate>
	<timebase>" + timebase + @"</timebase>
	<ntsc>" + isNTSC + @"</ntsc>
</rate>
	<in>" + inFrame + @"</in>
	<out>" + outFrame + @"</out>
	<name>" + clipName + @"</name>
        ");

                sb.Append(@"<media>
");

                #region Video

                sb.Append(@"<video>
");

                sb.Append(@"<track>
");
                sb.Append(@"<clipitem id=""clipitem-" + masterClipId + @"-video"">
	<masterclipid>masterclip-" + masterClipId + @"</masterclipid>
	<name>" + clipName + @"</name>
<file id=""file-" + masterClipId + @""">
	<name>" + highlightObject.InputFileObject.SourceFileInfo.Name + @"</name>
	<pathurl>" + pathURL + @"</pathurl>
<media>
<video>
<samplecharacteristics>
	<width>" + highlightObject.InputFileObject.VideoWidth + @"</width>
	<height>" + highlightObject.InputFileObject.VideoHeight + @"</height>
</samplecharacteristics>
</video>
<audio>
<samplecharacteristics>
	<depth>16</depth>
	<samplerate>48000</samplerate>
</samplecharacteristics>
 <channelcount>1</channelcount>
 <layout>mono</layout>
 <audiochannel>
 	<sourcechannel>1</sourcechannel>
 	<channellabel>discrete</channellabel>
 </audiochannel>
</audio>
<audio>
<samplecharacteristics>
	<depth>16</depth>
	<samplerate>48000</samplerate>
</samplecharacteristics>
 <channelcount>1</channelcount>
 <layout>mono</layout>
 <audiochannel>
 	<sourcechannel>2</sourcechannel>
 	<channellabel>discrete</channellabel>
 </audiochannel>
</audio>
</media>
</file>
");

                sb.Append(
@"<link>
	<linkclipref>clipitem-" + masterClipId + @"-video</linkclipref>
	<mediatype>video</mediatype>
	<trackindex>1</trackindex>
	<clipindex>1</clipindex>
</link>
<link>
	<linkclipref>clipitem-" + masterClipId + @"-audio-1</linkclipref>
	<mediatype>audio</mediatype>
	<trackindex>1</trackindex>
	<clipindex>1</clipindex>
</link>
<link>
	<linkclipref>clipitem-" + masterClipId + @"-audio-2</linkclipref>
	<mediatype>audio</mediatype>
	<trackindex>2</trackindex>
	<clipindex>1</clipindex>
</link>
</clipitem>
");

                sb.Append(@"</track>
</video>
");

                #endregion Video

                #region Audio

                sb.Append(@"<audio>
<track>
<clipitem id=""clipitem-" + masterClipId + @"-audio-1"">
	<masterclipid>masterclip-" + masterClipId + @"</masterclipid>
	<name>" + clipName + @"</name>
<file id=""file-" + masterClipId + @"""/>
<sourcetrack>
	<mediatype>audio</mediatype>
	<trackindex>1</trackindex>
</sourcetrack>
<link>
	<linkclipref>clipitem-" + masterClipId + @"-video</linkclipref>
	<mediatype>video</mediatype>
	<trackindex>1</trackindex>
	<clipindex>1</clipindex>
</link>
<link>
	<linkclipref>clipitem-" + masterClipId + @"-audio-1</linkclipref>
	<mediatype>audio</mediatype>
	<trackindex>1</trackindex>
	<clipindex>1</clipindex>
</link>
<link>
	<linkclipref>clipitem-" + masterClipId + @"-audio-2</linkclipref>
	<mediatype>audio</mediatype>
	<trackindex>2</trackindex>
	<clipindex>1</clipindex>
</link>
</clipitem>
</track>
<track>
<clipitem id=""clipitem-" + masterClipId + @"-audio-2"">
	<masterclipid>masterclip-" + masterClipId + @"</masterclipid>
	<name>" + clipName + @"</name>
<file id=""file-" + masterClipId + @"""/>
<sourcetrack>
	<mediatype>audio</mediatype>
	<trackindex>2</trackindex>
</sourcetrack>
<link>
	<linkclipref>clipitem-" + masterClipId + @"-video</linkclipref>
	<mediatype>video</mediatype>
	<trackindex>1</trackindex>
	<clipindex>1</clipindex>
</link>
<link>
	<linkclipref>clipitem-" + masterClipId + @"-audio-1</linkclipref>
	<mediatype>audio</mediatype>
	<trackindex>1</trackindex>
	<clipindex>1</clipindex>
</link>
<link>
	<linkclipref>clipitem-" + masterClipId + @"-audio-2</linkclipref>
	<mediatype>audio</mediatype>
	<trackindex>2</trackindex>
	<clipindex>1</clipindex>
</link>
</clipitem>
</track>
</audio>
");

                #endregion Audio

                sb.Append(@"</media>
</clip>
");

                masterClipId += 1;
            }

            try
            {
                File.WriteAllText(outputPath, sb.ToString());
                ProjectFileLocation = outputPath;

                AnalyticsHelper.FireEvent("Export - Adobe Premiere");

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
            return ".xml";
        }

        internal override string GetProjectFilename()
        {
            return "My Highlights.xml";
        }

        internal override string GetSaveDialogFilter()
        {
            return "Avid Studio XML (*.xml)|*.xml|All Files (*.*)|*.*";
        }

        internal override string LaunchAppMessage()
        {
            return "1. Open Avid Studio" + Environment.NewLine +
                "2. Go to File \\ Import..." + Environment.NewLine +
                "3. Select the Avid Studio XML file at " + ProjectFileLocation;
        }

        internal override bool SupportsLaunchingByExtension()
        {
            return false;
        }

        private static bool CalculateIsNTSC(double framesPerSecond)
        { // based on https://developer.apple.com/library/mac/#documentation/AppleApplications/Reference/FinalCutPro_XML/FrameRate/FrameRate.html#//apple_ref/doc/uid/TP30001158-TPXREF103
            int roundedFPS = (int)Math.Round(framesPerSecond, 0);

            if (framesPerSecond < 24)
                return true;

            if (roundedFPS == 24 || roundedFPS == 25)
                return false;

            if (framesPerSecond < 30)
                return true;

            if (roundedFPS == 30 || roundedFPS == 50)
                return false;

            if (framesPerSecond < 60)
                return true;

            if (roundedFPS == 60)
                return false;

            Logger.Error("Couldn't figure out NTSC!");
            Debug.Assert(false, "Couldn't figure out NTSC!");

            return true;
        }

        private static int CalculateTimebase(double framesPerSecond)
        { // based on https://developer.apple.com/library/mac/#documentation/AppleApplications/Reference/FinalCutPro_XML/FrameRate/FrameRate.html#//apple_ref/doc/uid/TP30001158-TPXREF103
            return (int)Math.Round(framesPerSecond, 0);
        }

        private static string GeneratePathURL(string filepath)
        {
            string ret = Uri.EscapeUriString(filepath);
            ret = ret.Replace(":", "%3a");
            ret = ret.Replace("%5C", "/");
            ret = "file://localhost/" + ret;

            return ret;
        }

        internal override string AppName
        {
            get { return "Avid Studio"; }
        }
    }
}
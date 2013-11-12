using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using GaDotNet.Common.Helpers;
using NLog;

namespace OdessaGUIProject.Workers
{
    internal class MovieMakerExportWorker : AbstractExportWorker
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        // keeps track of the InputFileObjects and their mediaItem.
        private Dictionary<InputFileObject, int> mediaItems = new Dictionary<InputFileObject, int>();

        internal override bool ExportHighlightsToFile(string outputPath)
        {
            bool success = true;

            StringBuilder sb = new StringBuilder();
            sb.Append(
                @"<?xml version=""1.0"" encoding=""utf-8""?>
<Project name=""My Highlights"" themeId=""0"" version=""65538"" templateID=""SimpleProjectTemplate"">
  <MediaItems>
      ");
            for (int i = 0; i < MainModel.InputFileObjects.Count; i++)
            {
                var inputFileObject = MainModel.InputFileObjects[i];
                var width = inputFileObject.VideoWidth;
                var height = inputFileObject.VideoHeight;
                var duration = inputFileObject.VideoDurationInSeconds;
                var mediaItemID = i + 1;
                sb.Append("<MediaItem id=\"" + mediaItemID + "\" filePath=\"" + inputFileObject.SourceFileInfo.FullName + "\" " +
                    "arWidth=\"" + width + "\" arHeight=\"" + height + "\" duration=\"" + duration + "\" songTitle=\"\" songArtist=\"\" songAlbum=\"\" />" + Environment.NewLine);

                mediaItems.Add(inputFileObject, mediaItemID);
            }
            sb.Append(@"</MediaItems>
        <Extents>
        ");

            int extentID = 5;
            List<int> usedExtendIDs = new List<int>();
            for (int i = 0; i < MainModel.HighlightObjects.Count; i++)
            {
                var highlight = MainModel.HighlightObjects[i];
                var mediaItemID = mediaItems[highlight.InputFileObject];
                var inTime = highlight.StartTime.TotalSeconds;
                var outTime = highlight.EndTime.TotalSeconds;

                sb.Append(
                @"<VideoClip extentID=""" + extentID + @""" gapBefore=""0"" mediaItemID=""" + mediaItemID + @""" inTime=""" + inTime + @""" outTime=""" + outTime + @""" speed=""1"">
      <Effects />
      <Transitions />
      <BoundProperties>
        <BoundPropertyBool Name=""Mute"" Value=""false"" />
        <BoundPropertyInt Name=""rotateStepNinety"" Value=""0"" />
        <BoundPropertyFloat Name=""Volume"" Value=""1"" />
      </BoundProperties>
    </VideoClip>
    ");

                usedExtendIDs.Add(extentID);

                extentID += 2; // skip by 2, otherwide MovieMaker thinks it's part of the same clip
            }

            sb.Append(
                @"<ExtentSelector extentID=""1"" gapBefore=""0"" primaryTrack=""true"">
                <Effects />
        <Transitions />
      <BoundProperties />
      <ExtentRefs>
");

            foreach (var usedExtendID in usedExtendIDs)
            {
                sb.Append(@"<ExtentRef id=""" + usedExtendID + @""" />
");
            }

            sb.Append(
                @"</ExtentRefs>
    </ExtentSelector>
    <ExtentSelector extentID=""2"" gapBefore=""0"" primaryTrack=""false"">
      <Effects />
      <Transitions />
      <BoundProperties />
      <ExtentRefs />
    </ExtentSelector>
    <ExtentSelector extentID=""3"" gapBefore=""0"" primaryTrack=""false"">
      <Effects />
      <Transitions />
      <BoundProperties />
      <ExtentRefs />
    </ExtentSelector>
    <ExtentSelector extentID=""4"" gapBefore=""0"" primaryTrack=""false"">
      <Effects />
      <Transitions />
      <BoundProperties />
      <ExtentRefs />
    </ExtentSelector>
  </Extents>
  <BoundPlaceholders>
    <BoundPlaceholder placeholderID=""SingleExtentView"" extentID=""0"" />
    <BoundPlaceholder placeholderID=""Main"" extentID=""1"" />
    <BoundPlaceholder placeholderID=""SoundTrack"" extentID=""2"" />
    <BoundPlaceholder placeholderID=""Text"" extentID=""4"" />
    <BoundPlaceholder placeholderID=""Narration"" extentID=""3"" />
  </BoundPlaceholders>
  <BoundProperties>
    <BoundPropertyFloatSet Name=""AspectRatio"">
      <BoundPropertyFloatElement Value=""1.7777776718139648"" />
    </BoundPropertyFloatSet>
    <BoundPropertyFloat Name=""SoundTrackMix"" Value=""0"" />
  </BoundProperties>
  <ThemeOperationLog themeID=""0"">
    <MonolithicThemeOperations />
  </ThemeOperationLog>
</Project>
");

            try
            {
                File.WriteAllText(outputPath, sb.ToString());

                AnalyticsHelper.FireEvent("Export - Movie Maker");

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
            return ""; // not needed because we can launch by extension
        }

        internal override string GetProjectFileExtension()
        {
            return ".wlmp";
        }

        internal override string GetProjectFilename()
        {
            return "My Highlights.wlmp";
        }

        internal override string GetSaveDialogFilter()
        {
            return "Movie Maker Projects (*.wlmp)|*.wlmp|All files (*.*)|*.*";
        }

        internal override string LaunchAppMessage()
        { // not needed for movie maker
            throw new NotImplementedException();
        }

        internal override bool SupportsLaunchingByExtension()
        {
            return true;
        }

        internal override string AppName
        {
            get { return "Windows Movie Maker"; }
        }
    }
}
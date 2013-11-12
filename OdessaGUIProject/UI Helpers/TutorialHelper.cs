using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using GaDotNet.Common.Helpers;

namespace OdessaGUIProject.UI_Helpers
{
    public enum TooltipDirection
    {
        Left,
        BottomLeft,
        Bottom,
        TopRight
    }

    public enum TutorialProgress
    {
        TutorialAddSampleVideo,
        TutorialScanButton,
        TutorialHighlightsFound,
        TutorialBookmarkFlag,
        TutorialHandles,
        TutorialShareButton,
        TutorialCloseDetails,
        TutorialStartOver,
        TutorialFinished
    }
    internal class TutorialHelper
    {

        internal static void AdvanceProgress()
        {
            Properties.Settings.Default.TutorialProgress += 1;
            Properties.Settings.Default.Save();

            AnalyticsHelper.FireEvent("Tutorial progress - " + Properties.Settings.Default.TutorialProgress + " - " +
                                      GetFriendlyProgress(GetTutorialProgress()));
        }

        internal static void Finish()
        {
            Properties.Settings.Default.TutorialProgress = (int)TutorialProgress.TutorialFinished;
            Properties.Settings.Default.Save();

            AnalyticsHelper.FireEvent("Tutorial progress - Exit");
        }

        internal static TutorialProgress GetTutorialProgress()
        {
            return (TutorialProgress) Properties.Settings.Default.TutorialProgress;
        }

        internal static void ResetProgress()
        {
            Properties.Settings.Default.TutorialProgress = (int)TutorialProgress.TutorialAddSampleVideo;
            Properties.Settings.Default.Save();
        }

        internal static void TutorialStarted()
        {
            AnalyticsHelper.FireEvent("Tutorial progress - " + (int)TutorialProgress.TutorialAddSampleVideo + " - " +
                                      GetFriendlyProgress(TutorialProgress.TutorialAddSampleVideo));
        }

        private static string GetFriendlyProgress(TutorialProgress progress)
        {
            switch (progress)
            {
                case TutorialProgress.TutorialAddSampleVideo: return "AddSampleVideo";
                case TutorialProgress.TutorialScanButton: return "ScanButton";
                case TutorialProgress.TutorialHighlightsFound: return "HighlightsFound";
                case TutorialProgress.TutorialBookmarkFlag: return "BookmarkFlag";
                case TutorialProgress.TutorialHandles: return "Handles";
                case TutorialProgress.TutorialShareButton: return "ShareButton";
                case TutorialProgress.TutorialCloseDetails: return "CloseDetails";
                case TutorialProgress.TutorialStartOver: return "StartOver";
                case TutorialProgress.TutorialFinished: return "Finished";
                default: Debug.Assert(false, "Unaccounted case");
                    return "";
            }
        }
    }
}

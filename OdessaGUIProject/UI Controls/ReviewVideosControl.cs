using System;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using NLog;
using OdessaGUIProject.DRM_Helpers;
using OdessaGUIProject.UI_Helpers;
using OdessaGUIProject.Workers;
using GaDotNet.Common.Helpers;

namespace OdessaGUIProject.UI_Controls
{
    public sealed partial class ReviewVideosControl : UserControl
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private HighlightDetailsForm highlightDetailsForm;

        public ReviewVideosControl()
        {
            InitializeComponent();

            this.BackColor = Color.Transparent;

            DesignLanguage.ApplyCustomFont(this.Controls);
        }

        internal event EventHandler DisplayActivationOptions;

        internal event EventHandler HighlightDetailsClosed;

        internal event EventHandler HighlightDetailsOpened;

        internal event EventHandler RescanRequested;

        internal event EventHandler StartOverRequested;

        internal event EventHandler TutorialProgressUpdated;

        private void OnTutorialProgressUpdated()
        {
            EventHandler handler = TutorialProgressUpdated;
            if (handler != null) handler(this, EventArgs.Empty);
        }

        public void DisplayHighlights()
        {
            using (new HourGlass())
            {
                innerHighlightsPanel.Controls.Clear();

                foreach (var inputFileObject in MainModel.InputFileObjects)
                {
                    AddDivider(inputFileObject);

                    var highlightCount = 0;

                    foreach (var highlight in MainModel.HighlightObjects)
                    {
                        if (highlight.InputFileObject == inputFileObject)
                        {
                            AddHighlightToPanel(highlight);
                            highlightCount++;
                        }
                    }

                    if (highlightCount == 0)
                    {
                        AddNoHighlightToPanel(inputFileObject);
                    }
                }
            }
        }

        private void AddDivider(InputFileObject inputFileObject)
        {
            var highlightDividerBarControl = new HighlightDividerBarControl();
            highlightDividerBarControl.InputFileObject = inputFileObject;
            highlightDividerBarControl.Margin = Padding.Empty;
            innerHighlightsPanel.Controls.Add(highlightDividerBarControl);
            highlightDividerBarControl.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        }

        private void AddHighlightToPanel(HighlightObject highlightObject)
        {
            var highlightThumbnailControl = new HighlightThumbnailControl();
            highlightThumbnailControl.HighlightObject = highlightObject;
            highlightThumbnailControl.HighlightDetailsOpening += highlightThumbnailControl_HighlightDetailsOpening;
            highlightThumbnailControl.HighlightRemoved += new HighlightThumbnailControl.HighlightRemovedEventHandler(highlightThumbnailControl_HighlightRemoved);
            highlightThumbnailControl.Margin = Padding.Empty;
            innerHighlightsPanel.Controls.Add(highlightThumbnailControl);
            highlightThumbnailControl.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right; // this must be set after we add the control, otherwise it doesn't stick

            if (highlightObject.HighlightObjectIndex == 0)
                highlightThumbnailControl.ShowPlayOverlay();

        }

        private void AddNoHighlightToPanel(InputFileObject inputFileObject)
        {
            var noHighlightThumbnailControl = new NoHighlightThumbnailControl();
            noHighlightThumbnailControl.InputFileObject = inputFileObject;
            noHighlightThumbnailControl.Margin = Padding.Empty;
            noHighlightThumbnailControl.RescanRequested += new EventHandler(noHighlightThumbnailControl_RescanRequested);
            innerHighlightsPanel.Controls.Add(noHighlightThumbnailControl);
            noHighlightThumbnailControl.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right; // this must be set after we add the control, otherwise it doesn't stick
        }

        private void Export(AbstractExportWorker exportWorker)
        {
            var activationState = Protection.GetLicenseStatus();
            if (activationState == Protection.ActivationState.TrialExpired || activationState == Protection.ActivationState.Unlicensed)
            {
                DisplayActivationOptions(null, null);
                return;
            }

            string projectFileName = "";
            if (exportWorker.SupportsLaunchingByExtension())
            {
                projectFileName = Path.Combine(Path.GetTempPath(), "My highlights - " + string.Format(CultureInfo.CurrentCulture, "{0:yyyy-MM-dd_hh-mm-ss-tt}",
        DateTime.Now) + exportWorker.GetProjectFileExtension());
            }
            else
            {
                using (var saveFileDialog = new SaveFileDialog())
                {
                    saveFileDialog.Title = "Save project file";
                    saveFileDialog.FileName = exportWorker.GetProjectFilename();
                    saveFileDialog.DefaultExt = exportWorker.GetProjectFileExtension();
                    saveFileDialog.InitialDirectory = MainModel.GetMyVideosDirectory();
                    saveFileDialog.Filter = exportWorker.GetSaveDialogFilter();
                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                        projectFileName = saveFileDialog.FileName;
                    else
                        return;
                }
            }

            if (exportWorker.ExportHighlightsToFile(projectFileName))
            {
                if (exportWorker.SupportsLaunchingByExtension())
                {
                    using (new HourGlass())
                    {
                        var p = new Process();
                        p.StartInfo.FileName = projectFileName;
                        try
                        {
                            p.Start();
                            p.WaitForInputIdle();
                        }
                        catch (Exception ex)
                        {
                            Logger.Error("Error running {0}: {1}", projectFileName, ex);
                            if (MessageBox.Show("Unfortunately we had trouble opening your highlights in " + exportWorker.AppName + "." + Environment.NewLine + Environment.NewLine +
                                "Can you tell us about this on our Support Center?", "Error exporting", MessageBoxButtons.YesNo, MessageBoxIcon.Error)
                                == DialogResult.Yes)
                            {
                                BrowserHelper.LaunchBrowser("http://support.highlighthunter.com", "ErrorExporting");
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Export was successful! Follow these steps to proceed:" + Environment.NewLine + Environment.NewLine +
                        exportWorker.LaunchAppMessage(), "Export successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                if (MessageBox.Show("Unfortunately we had trouble opening your highlights in " + exportWorker.AppName + "." + Environment.NewLine + Environment.NewLine +
        "Can you tell us about this on our Support Center?", "Error exporting", MessageBoxButtons.YesNo, MessageBoxIcon.Error)
                    == DialogResult.Yes)
                {
                    BrowserHelper.LaunchBrowser("http://support.highlighthunter.com", "ErrorExporting");
                }
            }



            if (Properties.Settings.Default.HasExportedHighlights == false)
            {
                if (AnalyticsHelper.FireEvent("First export"))
                {
                    Properties.Settings.Default.HasExportedHighlights = true;
                    Properties.Settings.Default.Save();
                }
            }
        }

        private void exportLinkLabel_Click(object sender, EventArgs e)
        {
            exportContextMenuStrip.Show(exportLinkLabel, 0, Convert.ToInt16(exportLinkLabel.Height * 1.1));
        }

        private void exportToAdobePremiereToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var exportWorker = new AdobePremiereExportWorker();
            Export(exportWorker);
        }

        private void exportToSonyVegasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var exportWorker = new EdlExportWorker();
            Export(exportWorker);
        }

        private void exportToWindowsLiveMovieMakerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var movieMakerExportWorker = new MovieMakerExportWorker();

            Export(movieMakerExportWorker);
        }

        private void highlightDetailsForm_HighlightRemoved(object sender, HighlightEventArgs e)
        {
            RemoveHighlight(e.HighlightObject);
        }

        private void highlightThumbnailControl_HighlightRemoved(object sender, HighlightEventArgs e)
        {
            RemoveHighlight(e.HighlightObject);
        }

        private void highlightThumbnailControl_HighlightDetailsOpening(object sender, HighlightEventArgs e)
        {
            using (var dimmerMask = new DimmerMask(this.ParentForm))
            {
                dimmerMask.Show(this.ParentForm);

                using (highlightDetailsForm = new HighlightDetailsForm())
                {
                    highlightDetailsForm.InitialHighlightIndex = e.HighlightObject.HighlightObjectIndex;
                    highlightDetailsForm.HighlightRemoved += highlightDetailsForm_HighlightRemoved;
                    highlightDetailsForm.InitializeStartOverTutorialBubble += highlightDetailsForm_InitializeStartOverTutorialBubble;
                    highlightDetailsForm.TutorialProgressUpdated += highlightDetailsForm_TutorialProgressUpdated;

                    if (highlightsFoundTutorialBubble.Visible)
                        highlightsFoundTutorialBubble_Advance(null, EventArgs.Empty);

                    if (HighlightDetailsOpened != null)
                        HighlightDetailsOpened(sender, e);

                    highlightDetailsForm.ShowDialog();

                    if (HighlightDetailsClosed != null)
                        HighlightDetailsClosed(sender, e);
                }

                dimmerMask.Close();
            }
        }

        void highlightDetailsForm_TutorialProgressUpdated(object sender, EventArgs e)
        {
            OnTutorialProgressUpdated();
        }

        void highlightDetailsForm_InitializeStartOverTutorialBubble(object sender, EventArgs e)
        {
            var progress = TutorialHelper.GetTutorialProgress();
            if (progress != TutorialProgress.TutorialStartOver)
            {
                Logger.Info("Skipping TutorialStartOver tutorial. Progress = " + progress);
                return;
            }

            startOverTutorialBubble.Visible = true;
            
        }

        private void noHighlightThumbnailControl_RescanRequested(object sender, EventArgs e)
        {
            if (RescanRequested != null)
                RescanRequested(sender, e);
        }

        private void parentPanel_Paint(object sender, PaintEventArgs e)
        {
            Graphics v = e.Graphics;
            //  e.ClipRectangle.Left, e.ClipRectangle.Top, e.ClipRectangle.Width - 1, e.ClipRectangle.Height - 1
            RoundedCorners.DrawRoundRect(v, new SolidBrush(Color.FromArgb(85, 85, 85)), innerHighlightsPanel.Left, innerHighlightsPanel.Top, innerHighlightsPanel.Width - 1, innerHighlightsPanel.Height - 1, 5);

            //Without rounded corners
            //e.Graphics.DrawRectangle(Pens.Blue, e.ClipRectangle.Left, e.ClipRectangle.Top, e.ClipRectangle.Width - 1, e.ClipRectangle.Height - 1);

            base.OnPaint(e);
        }

        private void RemoveHighlight(HighlightObject highlightObject)
        {
            // remove control
            foreach (Control c in innerHighlightsPanel.Controls)
            {
                var h = c as HighlightThumbnailControl;
                if (h != null && h.HighlightObject == highlightObject)
                {
                    innerHighlightsPanel.Controls.Remove(h);
                    h.Dispose();
                    h = null;
                }
            }

            // remove from HighlightObjects
            MainModel.HighlightObjects.Remove(highlightObject);

            // recount highlightObjectIndex
            var newIndex = 0;
            foreach (var objects in MainModel.HighlightObjects)
            {
                objects.HighlightObjectIndex = newIndex++;
            }

            // update divider
            foreach (Control c in innerHighlightsPanel.Controls)
            {
                var d = c as HighlightDividerBarControl;
                if (d != null && d.InputFileObject == highlightObject.InputFileObject)
                    d.RefreshHighlightCount();
            }
        }

        private void ReviewVideosControl_Load(object sender, EventArgs e)
        {
            InitializeHighlightsFoundTutorialBubble();
        }

        private void saveAllLinkLabel_Click(object sender, EventArgs e)
        {
            using (var folderBrowserDialog = new FolderBrowserDialog())
            {
                folderBrowserDialog.Description = "Where would you like to save your highlights?";

                if (Properties.Settings.Default.LastSavedDirectory.Length == 0)
                    folderBrowserDialog.SelectedPath = MainModel.GetMyVideosDirectory();
                else
                    folderBrowserDialog.SelectedPath = Properties.Settings.Default.LastSavedDirectory;

                folderBrowserDialog.ShowNewFolderButton = true;

                if (folderBrowserDialog.ShowDialog() != DialogResult.OK)
                    return;

                using (new HourGlass())
                {
                    Properties.Settings.Default.LastSavedDirectory = folderBrowserDialog.SelectedPath;
                    Properties.Settings.Default.Save();

                    foreach (var highlightObject in MainModel.HighlightObjects)
                    {
                        string extension = highlightObject.InputFileObject.SourceFileInfo.Extension; // includes dot
                        var outputFormat = (SaveWorker.OutputFormats)Enum.Parse(typeof(SaveWorker.OutputFormats), Properties.Settings.Default.SaveOutputFormat);
                        if (outputFormat == SaveWorker.OutputFormats.ProRes)
                            extension = ".mov";

                        string outputPath = Path.Combine(folderBrowserDialog.SelectedPath, highlightObject.Title + extension);

                        highlightObject.SaveWorker = new SaveWorker(highlightObject);
                        highlightObject.SaveWorker.OutputFormat = outputFormat;
                        highlightObject.SaveWorker.RunWorkerAsync(outputPath);
                    }

                    AnalyticsHelper.FireEvent("Each save all");
                }
            }
        }

        private void ReviewVideosControl_Resize(object sender, EventArgs e)
        {
            proGlyphPictureBox.Left = exportLinkLabel.Right;
        }

        internal bool ShouldContinuePublishing()
        {
            var isPublishing = false;
            foreach (var highlightObject in MainModel.HighlightObjects)
            {
                if ((highlightObject.SaveWorker != null && highlightObject.SaveWorker.IsBusy) ||
                     (highlightObject.FacebookShareWorker != null && highlightObject.FacebookShareWorker.IsBusy))
                {
                    isPublishing = true;
                    break;
                }
            }

            if (!isPublishing)
                return false;

            if (MessageBox.Show(
                "Highlight Hunter is busy working for you." + Environment.NewLine + Environment.NewLine +
                    "Would you like to stop publishing your highlights?",
                "Stop publishing?",
                MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
            {
                using (new HourGlass())
                {

                    foreach (var highlightObject in MainModel.HighlightObjects)
                    {
                        if (highlightObject.SaveWorker != null && highlightObject.SaveWorker.IsBusy)
                            highlightObject.SaveWorker.CancelAsync();
                        if (highlightObject.FacebookShareWorker != null &&
                            highlightObject.FacebookShareWorker.IsBusy)
                            highlightObject.FacebookShareWorker.CancelAsync();
                    }

                    // wait for them to cancel
                    foreach (var highlightObject in MainModel.HighlightObjects)
                    {
                        if (highlightObject.SaveWorker != null)
                            while (highlightObject.SaveWorker.IsBusy)
                            {
                                Application.DoEvents();
                            }
                        // DoEvents is required: http://social.msdn.microsoft.com/Forums/en-US/clr/thread/ad9a9a02-8a11-4bb8-b50a-613bcaa46886
                        if (highlightObject.FacebookShareWorker != null)
                            while (highlightObject.FacebookShareWorker.IsBusy)
                            {
                                Application.DoEvents();
                            }
                        // DoEvents is required: http://social.msdn.microsoft.com/Forums/en-US/clr/thread/ad9a9a02-8a11-4bb8-b50a-613bcaa46886
                    }
                }

                return false;
            }

            return true;
        }

        internal bool ShouldContinueReviewing()
        {
            if (MainModel.IsScanning)
                return false;

            var isUnreviewedHighlights = false;
            foreach (var highlightObject in MainModel.HighlightObjects)
            {
                if (highlightObject.HasBeenReviewed == false)
                {
                    isUnreviewedHighlights = true;
                    break;
                }
            }

            if (!isUnreviewedHighlights)
                return false;

            if (MessageBox.Show(
                "Do you want to review the rest of your highlights?",
                "Review highlights?",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                == DialogResult.No)
            {
                return false;
            }

            return true;
        }

        private void startOverLinkLabel_Click(object sender, EventArgs e)
        {
            StartOver();
        }

        private void StartOver()
        {

            var progress = TutorialHelper.GetTutorialProgress();
            if (progress == TutorialProgress.TutorialStartOver)
            {
                startOverTutorialBubble_Advance(null, EventArgs.Empty);
                return; // so we don't call start over twice
            }

            if (ShouldContinuePublishing())
                return;

            if (ShouldContinueReviewing())
                return;

            StartOverRequested(null, EventArgs.Empty);

            foreach (Control control in innerHighlightsPanel.Controls)
            {
                control.Dispose();
            }
            innerHighlightsPanel.Controls.Clear();

        }

        private void proGlyphPictureBox_Click(object sender, EventArgs e)
        {
            DisplayActivationOptions(sender, e);
        }

        internal void InitializeHighlightsFoundTutorialBubble()
        {
            var progress = TutorialHelper.GetTutorialProgress();
            if (progress != TutorialProgress.TutorialHighlightsFound)
            {
                Logger.Info("Skipping TutorialHighlightsFound tutorial. Progress = " + progress);
                return;
            }

            highlightsFoundTutorialBubble.Visible = true;
        }

        private void startOverTutorialBubble_Advance(object sender, EventArgs e)
        {
            startOverTutorialBubble.Visible = false;

            TutorialHelper.AdvanceProgress();

            OnTutorialProgressUpdated();

            StartOver();
        }

        private void highlightsFoundTutorialBubble_Advance(object sender, EventArgs e)
        {
            highlightsFoundTutorialBubble.Visible = false;

            TutorialHelper.AdvanceProgress();

            OnTutorialProgressUpdated();
        }

        internal void HideTutorialBubbles()
        {
            highlightsFoundTutorialBubble.Visible = false;
            startOverTutorialBubble.Visible = false;

            if (highlightDetailsForm != null)
                highlightDetailsForm.HideTutorialBubbles();
        }
    }
}
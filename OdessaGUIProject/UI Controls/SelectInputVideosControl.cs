using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using GaDotNet.Common.Helpers;
using NLog;
using OdessaGUIProject.UI_Helpers;

namespace OdessaGUIProject.UI_Controls
{
    internal sealed partial class SelectInputVideosControl : UserControl
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly object _locker = new object();

        internal SelectInputVideosControl()
        {
            InitializeComponent();

            BackColor = Color.Transparent;
        }

        internal event EventHandler ScanStarted;

        internal event EventHandler TutorialProgressUpdated;

        private void OnTutorialProgressUpdated()
        {
            EventHandler handler = TutorialProgressUpdated;
            if (handler != null) handler(this, EventArgs.Empty);
        }

        internal void ClearVideos()
        {
            foreach (Control control in inputFilesPanel.Controls)
            {
                if (!(control is DropHereSmallControl))
                {
                    control.Dispose();
                }
            }
            inputFilesPanel.Controls.Clear();

            inputFilesPanel.Controls.Add(dropHereSmallControl);

            dropHereBigControl.Visible = true;

            scanButton.Visible = false;
        }


        private void AddFilesFromCamera(string path)
        {
            using (new HourGlass())
            {
                var supportedExtensions = MainModel.GetVideoExtensions();
                var videoAdded = false;

                foreach (string file in Directory.GetFiles(path, "*.*", SearchOption.AllDirectories))
                {
                    Logger.Info("Found " + file);

                    var extension = Path.GetExtension(file);
                    if (extension == null || !supportedExtensions.Contains(extension.ToLowerInvariant())) continue;
                    AddInputFile(file);
                    videoAdded = true;
                }

                if (videoAdded)
                {
                    AnalyticsHelper.FireEvent("Each select - camera");

                    Properties.Settings.Default.HasAddedOtherVideos = true;
                    Properties.Settings.Default.Save();
                }
            }
        }

        internal void AddSampleVideo()
        {
            AddInputFile(MainModel.GetPathToSampleVideo());
        }

        private void AddInputFile(string file)
        {
            Logger.Info("Adding " + file);

            #region Make sure file isn't already added

            foreach (var existingFile in MainModel.InputFileObjects)
            {
                if (existingFile.SourceFileInfo.FullName.Equals(file, StringComparison.OrdinalIgnoreCase))
                    return;
            }

            #endregion Make sure file isn't already added

            dropHereBigControl.Visible = false;

            if (MainModel.InputFileObjects.Count == 0 &&
                TutorialHelper.GetTutorialProgress() != TutorialProgress.TutorialAddSampleVideo)
                dropHereSmallControl.Visible = true;

            var inputFileObject = new InputFileObject();
            inputFileObject.SourceFileInfo = new FileInfo(file);

            var videoThumbnailControl = new RawVideoThumbnailControl();
            videoThumbnailControl.InputFileObject = inputFileObject;
            videoThumbnailControl.Margin = new Padding(10);

            inputFilesPanel.Controls.Add(videoThumbnailControl);

            inputFilesPanel.Controls.SetChildIndex(dropHereSmallControl, inputFilesPanel.Controls.Count - 1);

            videoThumbnailControl.ThumbnailRemoved += videoThumbnailControl_ThumbnailRemoved;

            MainModel.InputFileObjects.Add(inputFileObject);

            scanButton.Visible = true;

            TrackFirstSelect();

            //inputFilesPanel.ScrollControlIntoView(currentHighlightThumbnailControl); // REMED because this was screwing up placement of controls
        }

        private void TrackFirstSelect()
        {
            if (Properties.Settings.Default.HasSelectedVideo == false)
            {
                AnalyticsHelper.FireEvent("First select");
                Properties.Settings.Default.HasSelectedVideo = true;
                Properties.Settings.Default.Save();
            }
        }

        private void BrowseForVideos()
        {
            using (var openFileDialog = new OpenFileDialog())
            {
                openFileDialog.CheckFileExists = true;
                openFileDialog.Multiselect = true;

                if (Properties.Settings.Default.LastBrowsedPath.Length > 0)
                { // use last directory
                    openFileDialog.InitialDirectory = Properties.Settings.Default.LastBrowsedPath;
                }
                else
                {
                    openFileDialog.InitialDirectory = MainModel.GetMyVideosDirectory();
                }

                string supportedExtensionsAsSingleString = "";
                foreach (string supportedExtension in MainModel.GetVideoExtensions())
                {
                    supportedExtensionsAsSingleString += "*" + supportedExtension + ";";
                }
                if (supportedExtensionsAsSingleString.Length > 0)
                    supportedExtensionsAsSingleString = supportedExtensionsAsSingleString.Substring(0,
                                                                                                    supportedExtensionsAsSingleString
                                                                                                        .Length - 1);

                openFileDialog.Title = "Which videos would you like to scan for highlights?";
                openFileDialog.Filter = "Supported Videos (" + supportedExtensionsAsSingleString + ")|" +
                                        supportedExtensionsAsSingleString + "|All Files (*.*)|*.*";
                openFileDialog.FileName = "";

                var addedVideo = false;
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    using (new HourGlass())
                    {
                        var supportedExtensions = MainModel.GetVideoExtensions();

                        foreach (string fileName in openFileDialog.FileNames)
                        {
                            Logger.Info("Found: " + fileName);
                            if (supportedExtensions.Contains(Path.GetExtension(fileName).ToLowerInvariant()))
                            {
                                addedVideo = true;
                                AddInputFile(fileName);
                            }
                        }

                        if (addedVideo)
                        {
                            Properties.Settings.Default.HasAddedOtherVideos = true;
                            AnalyticsHelper.FireEvent("Each select - browse");
                        }

                        Properties.Settings.Default.LastBrowsedPath = Path.GetDirectoryName(openFileDialog.FileNames[0]);

                        Properties.Settings.Default.Save();
                    }
                }
            }
        }

        private void cameraContextMenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            string path = e.ClickedItem.Name;

            AddFilesFromCamera(path);
        }

        private void DisplayCameraMenu(Point screenPosition)
        {
            cameraContextMenu.Items.Clear();

            try
            {
                DriveInfo[] ListDrives = DriveInfo.GetDrives();

                foreach (DriveInfo Drive in ListDrives)
                {
                    if (Drive.DriveType == DriveType.Removable && Drive.IsReady)
                    {
                        string label = "";
                        try
                        {
                            label = Drive.VolumeLabel;
                        }
                        catch (Exception) { }
                        if (label == "")
                            label = "Removable Disk";
                        string text = label + " (" + Drive.Name + ")";
                        ToolStripMenuItem toolStripMenuItem = new ToolStripMenuItem(text, null, null, Drive.Name);
                        cameraContextMenu.Items.Add(toolStripMenuItem);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Couldn't get list of drives: " + ex);
            }

            if (cameraContextMenu.Items.Count == 0)
                MessageBox.Show("Woops! It doesn't look like there's a camera plugged in." + Environment.NewLine + Environment.NewLine +
                    "Make sure you can see your camera in My Computer.", "No camera found", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
                cameraContextMenu.Show(screenPosition, ToolStripDropDownDirection.BelowRight);
        }

        private void dropHereBigControl_BrowseComputerHandler(object sender, EventArgs e)
        {
            BrowseForVideos();
        }

        private void dropHereBigControl_DragDrop(object sender, DragEventArgs e)
        {
            HandleDragDrop(e);
            dropHereBigControl.ResetState();
        }

        private void dropHereBigControl_DragEnter(object sender, DragEventArgs e)
        {
            // Check if the Dataformat of the data can be accepted
            // (we only accept ioItem drops from Explorer, etc.)
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Link; // Okay
                dropHereBigControl.ShowDropState();
            }
            else
                e.Effect = DragDropEffects.None; // Unknown data, ignore it
        }

        private void dropHereBigControl_DragLeave(object sender, EventArgs e)
        {
            dropHereBigControl.ResetState();
        }

        private void dropHereBigControl_SelectFromCameraHandler(object sender, EventArgs e)
        {
            DisplayCameraMenu(MousePosition);
        }

        private void dropHereSmallControl_BrowseComputerHandler(object sender, EventArgs e)
        {
            BrowseForVideos();
        }

        private void dropHereSmallControl_DragDrop(object sender, DragEventArgs e)
        {
            HandleDragDrop(e);
            dropHereSmallControl.ResetState();
        }

        private void dropHereSmallControl_DragEnter(object sender, DragEventArgs e)
        {
            // Check if the Dataformat of the data can be accepted
            // (we only accept ioItem drops from Explorer, etc.)
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Link; // Okay
                dropHereSmallControl.ShowDropState();
            }
            else
                e.Effect = DragDropEffects.None; // Unknown data, ignore it
        }

        private void dropHereSmallControl_DragLeave(object sender, EventArgs e)
        {
            dropHereSmallControl.ResetState();
        }

        private void dropHereSmallControl_SelectFromCameraHandler(object sender, EventArgs e)
        {
            DisplayCameraMenu(MousePosition);
        }

        private void HandleDragDrop(DragEventArgs e)
        {
            Logger.Info("Handling drop");

            using (new HourGlass())
            {
                // Extract the data from the DataObject-Container into a string list
                string[] FileList = (string[])e.Data.GetData(DataFormats.FileDrop, false);

                // Do something with the data...
                List<string> extensions = MainModel.GetVideoExtensions();

                bool addedValidFile = false;

                // For example add all files into a simple label control:
                foreach (var ioItem in FileList)
                {
                    if (Directory.Exists(ioItem))
                    {
                        foreach (string file in Directory.GetFiles(ioItem, "*.*", SearchOption.AllDirectories))
                        {
                            Logger.Info("Found: " + file);
                            if (extensions.Contains(Path.GetExtension(file).ToLowerInvariant()))
                            {
                                AddInputFile(file);
                                addedValidFile = true;
                            }
                        }
                    }
                    else
                    {
                        Logger.Info("Found: " + ioItem);
                        if (extensions.Contains(Path.GetExtension(ioItem).ToLowerInvariant()))
                        {
                            AddInputFile(ioItem);
                            addedValidFile = true;
                        }
                    }
                }

                if (addedValidFile)
                {
                    Properties.Settings.Default.HasAddedOtherVideos = true;
                    Properties.Settings.Default.Save();

                    AnalyticsHelper.FireEvent("Each select - drag and drop");
                }
            }
        }

        private void inputFilesPanel_DragDrop(object sender, DragEventArgs e)
        {
            HandleDragDrop(e);
        }

        private void inputFilesPanel_DragEnter(object sender, DragEventArgs e)
        {
            Logger.Debug("entered");

            // Check if the Dataformat of the data can be accepted
            // (we only accept ioItem drops from Explorer, etc.)
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Link; // Okay
                dropHereSmallControl.ShowDropState();
                dropHereBigControl.ShowDropState();
            }
            else
                e.Effect = DragDropEffects.None; // Unknown data, ignore it
        }

        private void inputFilesPanel_DragLeave(object sender, EventArgs e)
        {
            dropHereSmallControl.ResetState();
            dropHereBigControl.ResetState();
        }

        private void ScanAllVideos()
        {
            var progress = TutorialHelper.GetTutorialProgress();
            if (progress == TutorialProgress.TutorialAddSampleVideo)
            { // skip two steps since user knows what they're doing
                sampleVideoTutorialBubble_Advance(null, EventArgs.Empty);
                progress = TutorialHelper.GetTutorialProgress(); // refresh the progress
            }
            if (progress == TutorialProgress.TutorialScanButton)
            {
                scanButtonTutorialBubble_Advance(null, EventArgs.Empty);
                return; // so we don't call it twice
            }

            #region Make sure engine inputs are okay

            if (MainModel.InputFileObjects.Count == 0)
            {
                if (MessageBox.Show("Please choose some videos to scan first." + Environment.NewLine + Environment.NewLine +
                                    "Do you want to select one now?",
                                    "Choose a video", MessageBoxButtons.YesNo, MessageBoxIcon.Information) ==
                    DialogResult.Yes)
                {
                    BrowseForVideos();
                }
                return;
            }

            #endregion Make sure engine inputs are okay

            lock (_locker) // so a user can't click this too fast
            {
                ScanStarted(this, null);
            }
        }

        private void scanButton_Click(object sender, EventArgs e)
        {
            ScanAllVideos();
        }

        private void scanSampleVideoLinkLabel_Click(object sender, EventArgs e)
        {
            AddSampleVideo();
        }

        private void SelectInputVideosControl_Load(object sender, EventArgs e)
        {
            DesignLanguage.ApplyCustomFont(Controls);

            dropHereBigControl.Left = inputFilesPanel.Left;
            dropHereBigControl.Width = inputFilesPanel.Width;
            dropHereBigControl.Top = inputFilesPanel.Top;
            dropHereBigControl.Height = inputFilesPanel.Height;

            InitializeSampleVideoTutorialBubble();
        }

        internal void InitializeSampleVideoTutorialBubble()
        {
            var progress = TutorialHelper.GetTutorialProgress();
            if (progress != TutorialProgress.TutorialAddSampleVideo)
            {
                Logger.Info("Skipping AddSampleVideo tutorial. Progress = " + progress);
                return;
            }

            sampleVideoTutorialBubble.Visible = true;

            AddSampleVideo();
        }

        private void InitializeScanButtonTutorialBubble()
        {
            var progress = TutorialHelper.GetTutorialProgress();
            if (progress != TutorialProgress.TutorialScanButton)
            {
                Logger.Info("Skipping ScanButton tutorial. Progress = " + progress);
                return;
            }

            scanButtonTutorialBubble.Visible = true;

            AddSampleVideo(); // make sure sample video is in there
        }

        private void videoThumbnailControl_ThumbnailClicked(object sender, InputFileObjectEventArgs e)
        {
            var p = new Process();
            p.StartInfo.FileName = e.InputFileObject.SourceFileInfo.FullName;
            try
            {
                p.Start();
            }
            catch (Exception ex)
            {
                Logger.Error("Exception playing " + e.InputFileObject.SourceFileInfo.FullName + ": " + ex);
            }
        }

        private void videoThumbnailControl_ThumbnailRemoved(object sender, InputFileObjectEventArgs e)
        {
            // remove control
            foreach (Control c in inputFilesPanel.Controls)
            {
                var h = c as RawVideoThumbnailControl;
                if (h != null && h.InputFileObject == e.InputFileObject)
                {
                    inputFilesPanel.Controls.Remove(h);
                    h.Dispose();
                }
            }

            MainModel.InputFileObjects.Remove(e.InputFileObject);

            if (MainModel.InputFileObjects.Count == 0)
            {
                ClearVideos(); // go back to big drop image
                scanButton.Visible = false;
            }
        }

        private void selectInputVideosWelcomeControl_DragDrop(object sender, DragEventArgs e)
        {
            HandleDragDrop(e);
            dropHereBigControl.ResetState();
        }

        private void selectInputVideosWelcomeControl_DragEnter(object sender, DragEventArgs e)
        {
            // Check if the Dataformat of the data can be accepted
            // (we only accept ioItem drops from Explorer, etc.)
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Link; // Okay
                dropHereBigControl.ShowDropState();
            }
            else
                e.Effect = DragDropEffects.None; // Unknown data, ignore it
        }

        private void selectInputVideosWelcomeControl_DragLeave(object sender, EventArgs e)
        {
            dropHereBigControl.ResetState();
        }

        private void sampleVideoTutorialBubble_Advance(object sender, EventArgs e)
        {
            sampleVideoTutorialBubble.Visible = false;

            TutorialHelper.AdvanceProgress();

            OnTutorialProgressUpdated();

            InitializeScanButtonTutorialBubble();
        }

        private void scanButtonTutorialBubble_Advance(object sender, EventArgs e)
        {
            scanButtonTutorialBubble.Visible = false;

            TutorialHelper.AdvanceProgress();

            OnTutorialProgressUpdated();

            ScanAllVideos();
        }

        internal void HideTutorialBubbles()
        {
            sampleVideoTutorialBubble.Visible = false;
            scanButtonTutorialBubble.Visible = false;

            if (MainModel.InputFileObjects.Count > 0)
                dropHereSmallControl.Visible = true;
        }
    }
}
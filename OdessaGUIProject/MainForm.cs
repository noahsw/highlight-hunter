//#define DEMOVIDEO

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Security.Permissions;
using System.Windows.Forms;
using GaDotNet.Common.Helpers;
using NLog;
using OdessaGUIProject.DRM_Helpers;
using OdessaGUIProject.Properties;
using OdessaGUIProject.UI_Controls;
using OdessaGUIProject.UI_Helpers;
using OdessaGUIProject.Workers;

namespace OdessaGUIProject
{
    public partial class MainForm : Form
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private BorderlessWindow borderlessWindow;
        private Steps currentStep;
        private ReviewVideosControl reviewVideosControl;
        private ScanControl scanControl;
        private SelectInputVideosControl selectInputVideosControl;

        public MainForm()
        {
            InitializeComponent();

            #region Make sure we aren't running a Test config

#if TEST
            MessageBox.Show("Run under either DEBUG or RELEASE!");
            Application.Exit();
#endif

            #endregion Make sure we aren't running a Test config


            //this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.DoubleBuffer | ControlStyles.UserPaint | ControlStyles.ResizeRedraw, true);

            Logger.Info("App version: " + MainModel.ApplicationVersion);
            MainModel.Initialize(); // put here so we can start logging everything

            #region Borderless window

            borderlessWindow = new BorderlessWindow(this, true, true);
            borderlessWindow.SendNCWinMessage += SendNCWinMessage;
            this.MaximizedBounds = Screen.GetWorkingArea(this);

            #endregion Borderless window

            InitializeTutorialEscapeHatch();

            selectInputVideosControl = new SelectInputVideosControl();
            selectInputVideosControl.ScanStarted += selectInputVideosControl_ScanStarted;
            selectInputVideosControl.TutorialProgressUpdated += TutorialProgressUpdated;
            selectInputVideosControl.Dock = DockStyle.Fill;
            mainPanel.Controls.Add(selectInputVideosControl);

            scanControl = new ScanControl();
            scanControl.ScanCompletedWithHighlights += scanControl_ScanCompletedWithHighlights;
            scanControl.ScanCancelled += new EventHandler(scanControl_ScanCancelled);
            scanControl.windows7ProgressBar = win7ProgressBar;
            scanControl.Dock = DockStyle.Fill;
            mainPanel.Controls.Add(scanControl);

            reviewVideosControl = new ReviewVideosControl();
            reviewVideosControl.DisplayActivationOptions += reviewVideosControl_DisplayActivationOptions;
            reviewVideosControl.HighlightDetailsOpened += reviewVideosControl_HighlightDetailsOpened;
            reviewVideosControl.HighlightDetailsClosed += reviewVideosControl_HighlightDetailsClosed;
            reviewVideosControl.RescanRequested += reviewVideosControl_RescanRequested;
            reviewVideosControl.StartOverRequested += reviewVideosControl_StartOverRequested;
            reviewVideosControl.TutorialProgressUpdated += TutorialProgressUpdated;
            reviewVideosControl.Dock = DockStyle.Fill;
            mainPanel.Controls.Add(reviewVideosControl);

            SwitchToStep(Steps.SelectVideos);

            DesignLanguage.ApplyCustomFont(this.Controls);

            AnalyticsHelper.OptedIn = true; // everyone always opts in now

            #region track app load analytics

            AnalyticsHelper.FireEvent("Load", null);
            AnalyticsHelper.FireEvent("Load - OS - " + AnalyticsHelper.GetFriendlyOsVersion(), null);

            if (Settings.Default.HasAppLoadedYet == false)
            {
                Settings.Default.HasAppLoadedYet = true;
                Settings.Default.Save();
                AnalyticsHelper.FireEvent("First load", null); // this doesn't need to be conditional because analytics are always on by default
            }

            Settings.Default.TotalLoads += 1;
            Settings.Default.Save();

            AnalyticsHelper.FireEvent("Total loads - " + Settings.Default.TotalLoads);

            #endregion track app load analytics

            #region Initialize DRM Protection

            Protection.Initialize();
            UpdateUIwithLicenseStatus();

            #endregion Initialize DRM Protection
        }

        void TutorialProgressUpdated(object sender, EventArgs e)
        {
            var progress = TutorialHelper.GetTutorialProgress();
            if (progress == TutorialProgress.TutorialFinished)
                tutorialEscapeHatch.Visible = false;

            tutorialEscapeHatch.RefreshProgress();
        }

        private enum Steps
        {
            SelectVideos,
            Scan,
            Review
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                borderlessWindow.Dispose();

                components.Dispose();
            }

            DesignLanguage.Dispose();

            base.Dispose(disposing);
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var f = new AboutBox();
            f.ShowDialog();
        }

        private void activateLabelButton_Click(object sender, EventArgs e)
        {
            DisplayActivationOptions();
        }

        private void breadcrumbsControl_ReviewClicked(object sender, EventArgs e)
        {
            SwitchToStep(Steps.Review);
        }

        private void breadcrumbsControl_ScanClicked(object sender, EventArgs e)
        {
            SwitchToStep(Steps.Scan);
        }

        private void breadcrumbsControl_SelectVideosClicked(object sender, EventArgs e)
        {
            SwitchToStep(Steps.SelectVideos);
        }

        private void breadcrumbsControl_SelectVideosClicked_1(object sender, EventArgs e)
        {
            SwitchToStep(Steps.SelectVideos);
        }

        private void DisplayActivationOptions()
        {
            using (var dimmerMask = new DimmerMask(this))
            {
                dimmerMask.Show(this);

                using (var awForm = new ActivationWelcome())
                {
                    awForm.ShowDialog();
                }

                dimmerMask.Close();
            }

            UpdateUIwithLicenseStatus();
        }

        private void DisplayTutorial()
        {
            TutorialHelper.ResetProgress();

            TutorialHelper.TutorialStarted();

            selectInputVideosControl.InitializeSampleVideoTutorialBubble();

            tutorialEscapeHatch.RefreshProgress();
            tutorialEscapeHatch.Visible = true;

            using (var dimmerMask = new DimmerMask(this))
            {
                dimmerMask.Show(this);

                var fr = new TutorialForm
                {
                    CloseHandler = UpdateUIwithDRMStatusHandler
                };
                fr.ShowDialog();

                dimmerMask.Close();
            }
        }

        private void facebookToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BrowserHelper.LaunchBrowser("https://www.facebook.com/sharer/sharer.php?u=http%3A%2F%2Fwww.highlighthunter.com&t=I'm%20using%20Highlight+Hunter%20to%20find%20the%20highlights%20in%20my%20videos");
        }
        
        private void helpPictureButtonControl_Click(object sender, EventArgs e)
        {
            HelpContextMenuStrip.Show(helpPictureButtonControl, 0, Convert.ToInt16(helpPictureButtonControl.Height * 1.1));
        }

        /// <summary>
        /// The UpdateChecker calls this to see if it's safe to prompt the user for an update
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void IsSafeToPrompt(object sender, CancelEventArgs e)
        {
            // if we're scanning or there are other forms open, don't prompt
            if (MainModel.IsScanning || Application.OpenForms.Count > 1)
                e.Cancel = true;
            else
                e.Cancel = false;
        }

        private void licenseStatusLabelButton_Click(object sender, EventArgs e)
        {
            DisplayActivationOptions();
        }

        private void MainForm_DoubleClick(object sender, EventArgs e)
        {
#if DEBUG

            using (new HourGlass())
            {
                /*
                for (var i = 1; i <= 1000; i++)
                {
                    var activationState = Protection.GetLicenseStatus(true);
                }

                MessageBox.Show("done");

                return;
                */

                var inputFileObject = new InputFileObject()
                {
                    SourceFileInfo = new FileInfo(MainModel.GetPathToSampleVideo()),
                };

                using (var scanWorker = new ScanWorker(inputFileObject))
                {
                    scanWorker.SetBitrate();
                    scanWorker.SetFramesPerSecond();
                    scanWorker.SetTotalFrames();
                    scanWorker.SetVideoDimensions();
                    scanWorker.SetVideoDuration();
                }

                MainModel.InputFileObjects.Add(inputFileObject);

                var newHighlight1 = (new HighlightObject()
                {
                    InputFileObject = inputFileObject,
                    StartTime = TimeSpan.FromSeconds(10),
                    BookmarkTime = TimeSpan.FromSeconds(27),
                    EndTime = TimeSpan.FromSeconds(25),
                });
                newHighlight1.GenerateHighlightTitle();
                MainModel.HighlightObjects.Add(newHighlight1);


                var sampleFileDir = Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\..\sample-files");

                var inputFileObject2 = new InputFileObject()
                {
                    SourceFileInfo = new FileInfo(Path.Combine(sampleFileDir, @"Long videos\GO021100.MP4")),
                };

                using (var scanWorker = new ScanWorker(inputFileObject2))
                {
                    scanWorker.SetBitrate();
                    scanWorker.SetFramesPerSecond();
                    scanWorker.SetTotalFrames();
                    scanWorker.SetVideoDimensions();
                    scanWorker.SetVideoDuration();
                }

                MainModel.InputFileObjects.Add(inputFileObject2);

                var newHighlight2 = (new HighlightObject()
                {
                    InputFileObject = inputFileObject2,
                    StartTime = TimeSpan.FromSeconds(60 * 20 + 4),
                    BookmarkTime = TimeSpan.FromSeconds(60 * 20 + 19),
                    EndTime = TimeSpan.FromSeconds(60 * 20 + 17),
                });
                newHighlight2.GenerateHighlightTitle();
                MainModel.HighlightObjects.Add(newHighlight2);

                var newHighlight3 = new HighlightObject()
                {
                    InputFileObject = inputFileObject2,
                    StartTime = TimeSpan.FromSeconds(60 * 10 + 4),
                    BookmarkTime = TimeSpan.FromSeconds(60 * 10 + 19),
                    EndTime = TimeSpan.FromSeconds(60 * 10 + 17)
                };
                newHighlight3.GenerateHighlightTitle();
                MainModel.HighlightObjects.Add(newHighlight3);

                var newHighlight4 = new HighlightObject()
                {
                    InputFileObject = inputFileObject2,
                    StartTime = TimeSpan.FromSeconds(60 * 5 + 4),
                    BookmarkTime = TimeSpan.FromSeconds(60 * 5 + 19),
                    EndTime = TimeSpan.FromSeconds(60 * 5 + 17),
                };
                newHighlight4.GenerateHighlightTitle();
                MainModel.HighlightObjects.Add(newHighlight4);


                var inputFileObject3 = new InputFileObject()
                {
                    SourceFileInfo = new FileInfo(Path.Combine(sampleFileDir, "00002.MTS")),
                };

                using (var scanWorker = new ScanWorker(inputFileObject3))
                {
                    scanWorker.SetBitrate();
                    scanWorker.SetFramesPerSecond();
                    scanWorker.SetTotalFrames();
                    scanWorker.SetVideoDimensions();
                    scanWorker.SetVideoDuration();
                }

                MainModel.InputFileObjects.Add(inputFileObject3);

                var newHighlight5 = (new HighlightObject()
                {
                    InputFileObject = inputFileObject3,
                    StartTime = TimeSpan.FromSeconds(30),
                    BookmarkTime = TimeSpan.FromSeconds(47),
                    EndTime = TimeSpan.FromSeconds(45),
                });
                newHighlight5.GenerateHighlightTitle();
                MainModel.HighlightObjects.Add(newHighlight5);

                

                SwitchToStep(Steps.Scan);
                scanControl_ScanCompletedWithHighlights(sender, e);

            }
#endif
        }

        

        private bool ShouldContinueScanningSampleVideo()
        {
            if (Settings.Default.TotalVideosScanned > 0 || !Settings.Default.AgreedToEULA)
                return false;

            if (MessageBox.Show(
                "Before you leave, do you want to try scanning a sample video?",
                "Scan a sample video",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question) == DialogResult.No)
            {
                return false;
            }

            selectInputVideosControl.AddSampleVideo();
            return true;
        }

        private bool ShouldContinueClosing()
        {
            // this order is important!

            if (scanControl.ShouldContinueScanning())
                return false;

            if (ShouldContinueScanningSampleVideo())
                return false;

            if (reviewVideosControl.ShouldContinuePublishing())
                return false;

            if (reviewVideosControl.ShouldContinueReviewing())
                return false;

            return true;
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {

            if (!ShouldContinueClosing())
            {
                e.Cancel = true;
                return;
            }

            try
            {
                SaveSizingSettings();
                Properties.Settings.Default.Save(); // save sizing settings
            }
            catch (Exception) { }

            MainModel.Dispose();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            #region Load sizing settings

            // default WindowState is Normal
            if (Properties.Settings.Default.MainFormWindowState == FormWindowState.Maximized)
                this.WindowState = FormWindowState.Maximized;
            if (Properties.Settings.Default.MainFormSize.IsEmpty)
            { // this is their first time running. let's see if their screen is too small
                if (Screen.PrimaryScreen.WorkingArea.Width - this.Width < 20 ||
                    Screen.PrimaryScreen.WorkingArea.Height - this.Height < 20)
                { // screen is too small. let's maximize so they see everything
                    this.WindowState = FormWindowState.Maximized;
                }
            }
            if (this.WindowState == FormWindowState.Normal && !Properties.Settings.Default.MainFormSize.IsEmpty)
            {
                Logger.Debug("Resizing to " + Properties.Settings.Default.MainFormSize);
                this.Size = Properties.Settings.Default.MainFormSize;
                this.CenterToScreen();
            }

            #endregion Load sizing settings

        }

        private void ShowEULA()
        {
            if (Settings.Default.AgreedToEULA != false) return;

            Activate();

            using (var dimmerMask = new DimmerMask(this))
            {
                dimmerMask.Show(this);

                var consentForm = new ConsentForm();
                consentForm.ShowDialog();

                dimmerMask.Close();
            }

            if (Settings.Default.AgreedToEULA == false)
            {
                Application.Exit();
            }
        }

        private void MainForm_MouseClick(object sender, MouseEventArgs e)
        {
#if DEBUG
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                SwitchToStep(Steps.Scan);
            }
#endif
        }

        private void MainForm_Paint(object sender, PaintEventArgs e)
        {
            //Logger.Trace("called");
            string subTitle = "";
            switch (currentStep)
            {
                case Steps.SelectVideos: subTitle = "Add Videos"; break;
                case Steps.Scan: subTitle = "Scan for Highlights"; break;
                case Steps.Review: subTitle = "Review Highlights"; break;
            }

            borderlessWindow.PaintForm(e, "Highlight Hunter |", subTitle);
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            if (this.Created)
            {
                borderlessWindow.BuildPaths();
                this.Invalidate();

                SaveSizingSettings();

            }
        }

        private void SaveSizingSettings()
        {
            Properties.Settings.Default.MainFormWindowState = this.WindowState;

            if (this.WindowState == FormWindowState.Normal)
            {
                Properties.Settings.Default.MainFormSize = this.Size;
            }
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {

            ShowEULA();

            #region show the tutorial dialog

            if (Settings.Default.ShowFirstRunScreenV2)
            {
                this.Activate();
                DisplayTutorial();
            }

            #endregion show the tutorial dialog

            #region check for updates
            var updateChecker = new UpdateChecker(IsSafeToPrompt);
            updateChecker.CheckForUpdate();
            #endregion check for updates

            #region Preload some things (disabled)

            /* don't do this until we can verify that it actually helps
            Thread jitter = new Thread(() =>
            {
                var wmp = new GenericPlayerControl();
                /*
                foreach (var type in Assembly.Load("MyHavyAssembly, Version=1.8.2008.8, Culture=neutral, PublicKeyToken=8744b20f8da049e3").GetTypes())
                {
                    foreach (var method in type.GetMethods(BindingFlags.DeclaredOnly |
                                        BindingFlags.NonPublic |
                                        BindingFlags.Public | BindingFlags.Instance |
                                        BindingFlags.Static))
                    {
                        System.Runtime.CompilerServices.RuntimeHelpers.PrepareMethod(method.MethodHandle);
                    }
                }
            });
            jitter.SetApartmentState(ApartmentState.STA);
            jitter.Priority = ThreadPriority.Lowest;
            jitter.Start();
            */

            #endregion Preload some things (disabled)
        }

        private void openTutorialToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DisplayTutorial();
        }

        private void resetUI()
        {
            UpdateUIwithLicenseStatus();

            //activateLabelButton.Enabled = true;
        }

        private void reviewVideosControl_DisplayActivationOptions(object sender, EventArgs e)
        {
            DisplayActivationOptions();
        }

        private void reviewVideosControl_HighlightDetailsClosed(object sender, EventArgs e)
        {
            //dimmerMask.Close();
            //dimmerMask = null;
        }

        private void reviewVideosControl_HighlightDetailsOpened(object sender, EventArgs e)
        {
            //dimmerMask = new DimmerMask(this);
            //dimmerMask.Show(this);
        }

        private void reviewVideosControl_RescanRequested(object sender, EventArgs e)
        {
            selectInputVideosControl_ScanStarted(sender, e);
        }

        private void reviewVideosControl_StartOverRequested(object sender, EventArgs e)
        {
            MainModel.HighlightObjects.Clear();
            MainModel.InputFileObjects.Clear();
            selectInputVideosControl.ClearVideos();

            SwitchToStep(Steps.SelectVideos);
        }

        private void scanControl_ScanCancelled(object sender, EventArgs e)
        {
            resetUI();

            SwitchToStep(Steps.SelectVideos);
        }

        #region Borderless Window

        protected override CreateParams CreateParams
        {
            [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)] // http://msdn.microsoft.com/library/ms182305(VS.100).aspx
            get
            {
                const int WS_MINIMIZEBOX = 0x20000;
                const int CS_DROPSHADOW = 0x20000;
                const int CS_DBLCLKS = 0x8;

                System.Windows.Forms.CreateParams cParams = base.CreateParams;

                int ClassFlags = CS_DBLCLKS;
                int OSVER = Environment.OSVersion.Version.Major * 10;
                OSVER += Environment.OSVersion.Version.Minor;
                if (OSVER >= 51) ClassFlags = CS_DROPSHADOW | CS_DBLCLKS;

                cParams.ClassStyle = ClassFlags;
                cParams.Style |= WS_MINIMIZEBOX;
                return cParams;
            }
        }

        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        protected override void WndProc(ref Message m)
        {
            if (this.WindowState == FormWindowState.Maximized)
            {
                if (m.Msg == USER32.WM_SYSCOMMAND &&
                    m.WParam.ToInt32() == (int)USER32.SysCommand.SC_MOVE ||
                    m.Msg == (int)USER32.NCMouseMessage.WM_NCLBUTTONDOWN &&
                    m.WParam.ToInt32() == (int)USER32.NCHitTestResult.HTCAPTION)
                {
                    m.Msg = USER32.WM_NULL;
                }
            }

            base.WndProc(ref m);

            //Logger.Trace("Message from MainForm: " + m.Msg);

            switch (m.Msg)
            {
                case (int)USER32.WM_GETSYSMENU:
                    borderlessWindow.SystemMenu.Show(this, this.PointToClient(new Point(m.LParam.ToInt32())));
                    break;

                case USER32.WM_NCACTIVATE:
                    borderlessWindow.IsFormActive = m.WParam.ToInt32() != 0;
                    this.Invalidate();
                    break;

                case USER32.WM_NCHITTEST:
                    m.Result = borderlessWindow.OnNonClientHitTest(m.LParam);
                    break;

                case (int)USER32.NCMouseMessage.WM_NCLBUTTONUP:
                    borderlessWindow.OnNonClientLButtonUp(m.LParam);
                    break;

                case (int)USER32.NCMouseMessage.WM_NCRBUTTONUP:
                    borderlessWindow.OnNonClientRButtonUp(m.LParam);
                    break;

                case (int)USER32.NCMouseMessage.WM_NCMOUSEMOVE:
                    borderlessWindow.OnNonClientMouseMove(m.LParam);
                    break;
                default:
                    break;
            }
        }

        private void SendNCWinMessage(int msg, IntPtr wParam, IntPtr lParam)
        {
            Message message = Message.Create(this.Handle, msg, wParam, lParam);
            this.WndProc(ref message);
        }

        #endregion Borderless Window

        private void scanControl_ScanCompletedWithHighlights(object sender, EventArgs e)
        {
            // this is called when user is done reviewing videos
            resetUI();

            SwitchToStep(Steps.Review);

            reviewVideosControl.DisplayHighlights();
        }

        private void selectInputVideosControl_ScanStarted(object sender, EventArgs e)
        {
            
            using (new HourGlass())
            {
                scanControl.RunScan();

                SwitchToStep(Steps.Scan);
            }
        }

        private void settingsPictureButtonControl_Click(object sender, EventArgs e)
        {
            using (var dimmerMask = new DimmerMask(this))
            {
                dimmerMask.Show(this);

                using (var sf = new SettingsForm())
                {
                    sf.ShowDialog();
                }
                AnalyticsHelper.OptedIn = Settings.Default.OptIntoAnalytics;

                UpdateUIwithLicenseStatus();

                dimmerMask.Close();
            }
            //dimmerMask = null;
        }

        private void shareLabelButton_Click(object sender, EventArgs e)
        {
            //shareContextMenuStrip.Show(shareLabelButton, 0, Convert.ToInt16(shareLabelButton.Height * 1.1));
        }

        private void supportCenterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BrowserHelper.LaunchBrowser("http://support.highlighthunter.com", "faq");
        }

        private void SwitchToStep(Steps step)
        {
            switch (step)
            {
                case Steps.SelectVideos:
                    selectInputVideosControl.InitializeSampleVideoTutorialBubble();
                    selectInputVideosControl.Show();
                    breadcrumbsControl.SwitchToSelect();
                    scanControl.Hide();
                    reviewVideosControl.Hide();
                    break;

                case Steps.Scan:
                    scanControl.Show();
                    breadcrumbsControl.SwitchToScan();
                    selectInputVideosControl.Hide();
                    reviewVideosControl.Hide();
                    break;

                case Steps.Review:
                    reviewVideosControl.InitializeHighlightsFoundTutorialBubble();
                    reviewVideosControl.Show();
                    breadcrumbsControl.SwitchToReview();
                    selectInputVideosControl.Hide();
                    scanControl.Hide();
                    break;
            }

            Invalidate(); // redraw main title

            currentStep = step;
        }

        private void troubleshootingLogsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string[] files = Directory.GetFiles(Path.GetTempPath(), "HighlightHunter-*.log");
            var list = new List<FileInfo>();
            foreach (string file in files)
                list.Add(new FileInfo(file));

            MainModel.LaunchExplorerWithFilesSelected(list);
        }

        private void twitterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BrowserHelper.LaunchBrowser("https://twitter.com/intent/tweet?original_referer=http%3A%2F%2Fwww.highlighthunter.com%2F&source=tweetbutton&text=Highlight%20Hunter%20-%20Find%20the%20highlights%20in%20your%20videos%208%20times%20faster%20%40HighlightHunter&url=http%3A%2F%2Fwww.highlighthunter.com");
        }


        private void UpdateUIwithLicenseStatus()
        {
            var activationState = Protection.GetLicenseStatus();
            Logger.Info("License status: " + activationState);

            if (activationState == Protection.ActivationState.Trial || activationState == Protection.ActivationState.Activated)
                titlePictureBox.Image = Resources.header_main_logo_pro;
            else
                titlePictureBox.Image = Resources.header_main_logo;
        }

        private void UpdateUIwithDRMStatusHandler(object sender, EventArgs e)
        {
            //this.Activate();
            UpdateUIwithLicenseStatus();
        }

        private void InitializeTutorialEscapeHatch()
        {
            var progress = TutorialHelper.GetTutorialProgress();
            if (progress == TutorialProgress.TutorialFinished)
                return;

            if (progress != TutorialProgress.TutorialAddSampleVideo)
            {
                TutorialHelper.ResetProgress();
                AnalyticsHelper.FireEvent("Tutorial progress - Reset");
            }

            tutorialEscapeHatch.RefreshProgress();
            tutorialEscapeHatch.Visible = true;
        }

        private void tutorialEscapeHatch_TutorialExited(object sender, EventArgs e)
        {
            tutorialEscapeHatch.Visible = false;

            selectInputVideosControl.HideTutorialBubbles();

            reviewVideosControl.HideTutorialBubbles();
        }

    }
}
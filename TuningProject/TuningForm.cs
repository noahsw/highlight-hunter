using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using OdessaPCTestHelpers;
using OdessaGUIProject;
using OdessaGUIProject.Workers;
using NLog;

namespace TuningHostProject
{
    public partial class TuningForm : Form
    {

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private DateTime _startTime;

        public delegate void DelegateThreadFinished();
        public DelegateThreadFinished MDelegateThreadFinished;

        public delegate void DelegateThreadCancelled();
        public DelegateThreadCancelled MDelegateThreadCancelled;

        public DirectoryInfo LogDirectory;

        public TuningForm()
        {
            InitializeComponent();

            MDelegateThreadCancelled = ScanCancelled;
            MDelegateThreadFinished = ScanFinished;


            HostWorker.CompletedPassesCount = Properties.Settings.Default.CompletedPassesCount;

            TuningResultsDirectoryTextBox.Text = Properties.Settings.Default.TuningResultsDirectory;
            if (TuningResultsDirectoryTextBox.Text == "")
                TuningResultsDirectoryTextBox.Text = Path.Combine(Environment.CurrentDirectory, "Logs");

            PopulateThresholdSetupComboBoxes();

            LoadThresholdSettings();

            SetThresholdSetupComboBoxesEnableValue(Properties.Settings.Default.CompletedPassesCount == 0);

            UpdateLabels();

        }

        public void ScanCancelled()
        {
            CancelTestButton.Text = "Cancel";

            UpdateLabels();

            ProgressTimer.Enabled = false;

            RunButton.Enabled = true;
            RunButton.Text = "Run";
            CancelTestButton.Enabled = false;
            ResetButton.Enabled = true;
            TuningResultsDirectoryTextBox.Enabled = true;
            TuningResultsDirectoryBrowseButton.Enabled = true;
            TrimButton.Enabled = true;
            AdjustConcurrentScanCount.Enabled = false;

            if (HostWorker.CompletedPassesCount == 0)
                SetThresholdSetupComboBoxesEnableValue(true);
        }

        public void ScanFinished()
        {

            // copy spreadsheet to log directory
            // REMED out because we now save the log file in the actual logs directory
            //string OldTuningResultsLog = Path.Combine(Path.GetTempPath(), Program.TUNING_RESULTS_FILENAME);
            //string NewTuningResultsLog = Path.Combine(TuningResultsDirectoryTextBox.Text, Program.TUNING_RESULTS_FILENAME);
            //File.Copy(OldTuningResultsLog, NewTuningResultsLog, true);

            UpdateLabels();

            ProgressTimer.Enabled = false;

            RunButton.Enabled = true;
            RunButton.Text = "Run";
            CancelTestButton.Enabled = false;
            ResetButton.Enabled = true;
            TuningResultsDirectoryTextBox.Enabled = true;
            TuningResultsDirectoryBrowseButton.Enabled = true;
            TrimButton.Enabled = true;
            AdjustConcurrentScanCount.Enabled = false;
            SetThresholdSetupComboBoxesEnableValue(true);
        }

        private void RunButton_Click(object sender, EventArgs e)
        {

            RunButton.Enabled = false;
            RunButton.Text = "Running...";
            CancelTestButton.Enabled = true;
            ProgressTimer.Enabled = true;
            ResetButton.Enabled = false;
            TuningResultsDirectoryTextBox.Enabled = false;
            LogDirectory = Directory.CreateDirectory(TuningResultsDirectoryTextBox.Text);
            TuningResultsDirectoryBrowseButton.Enabled = false;
            TrimButton.Enabled = false;
            AdjustConcurrentScanCount.Enabled = true;
            SetThresholdSetupComboBoxesEnableValue(false);
            Application.DoEvents();

            #region verify that all input files exist
            bool everyInputFileExists = true;

            var findDarkFramesHelper = new FindDarkFramesHelper();
            
            foreach (KeyValuePair<string, FileInfo> kvp in findDarkFramesHelper.AvailableFiles)
            {
                if (kvp.Value.Exists == false)
                {
                    everyInputFileExists = false;
                    MessageBox.Show("inputFile does not exist!" + Environment.NewLine + Environment.NewLine +
                        kvp.Value.FullName, "inputFile does not exist!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            findDarkFramesHelper.Dispose();

            if (findDarkFramesHelper.AvailableFiles.Count == 0)
            {
                MessageBox.Show("No files to scan!");
                ScanCancelled();
                return;
            }

            if (everyInputFileExists == false)
            {
                ScanCancelled();
                return;
            }
            #endregion

            #region Verify valid threshold parameters

            if (Convert.ToInt32(ThresholdIndividualPixelBrightnessSetupStart.SelectedItem) > Convert.ToInt32(ThresholdIndividualPixelBrightnessSetupEnd.SelectedItem) ||
                Convert.ToInt32(ThresholdDarkPixelsPerFrameAsPercentageSetupStart.SelectedItem) > Convert.ToInt32(ThresholdDarkPixelsPerFrameAsPercentageSetupEnd.SelectedItem) ||
                Convert.ToInt32(ThresholdPixelScanPercentageSetupStart.SelectedItem) > Convert.ToInt32(ThresholdPixelScanPercentageSetupEnd.SelectedItem) ||
                Convert.ToDecimal(ThresholdSecondsSkipSetupStart.SelectedItem) > Convert.ToDecimal(ThresholdSecondsSkipSetupEnd.SelectedItem) ||
                Convert.ToDecimal(ThresholdConsecutiveDarkFramesInSecondsStart.SelectedItem) > Convert.ToDecimal(ThresholdConsecutiveDarkFramesInSecondsEnd.SelectedItem))
            {
                MessageBox.Show("Invalid threshold parameters! Start values must be less than end values.");
                ScanCancelled();
                return;
            }

            #endregion

            #region Save threshold parameters

            Properties.Settings.Default.ThresholdIndividualPixelBrightnessStart = Convert.ToInt32(ThresholdIndividualPixelBrightnessSetupStart.SelectedItem);
            Properties.Settings.Default.ThresholdIndividualPixelBrightnessEnd = Convert.ToInt32(ThresholdIndividualPixelBrightnessSetupEnd.SelectedItem);
            Properties.Settings.Default.ThresholdIndividualPixelBrightnessIncrement = Convert.ToInt32(ThresholdIndividualPixelBrightnessSetupIncrement.SelectedItem);

            Properties.Settings.Default.ThresholdDarkPixelsPerFrameAsPercentageStart = Convert.ToInt32(ThresholdDarkPixelsPerFrameAsPercentageSetupStart.SelectedItem);
            Properties.Settings.Default.ThresholdDarkPixelsPerFrameAsPercentageEnd = Convert.ToInt32(ThresholdDarkPixelsPerFrameAsPercentageSetupEnd.SelectedItem);
            Properties.Settings.Default.ThresholdDarkPixelsPerFrameAsPercentageIncrement = Convert.ToInt32(ThresholdDarkPixelsPerFrameAsPercentageSetupIncrement.SelectedItem);

            Properties.Settings.Default.ThresholdPixelScanPercentageStart = Convert.ToInt32(ThresholdPixelScanPercentageSetupStart.SelectedItem);
            Properties.Settings.Default.ThresholdPixelScanPercentageEnd = Convert.ToInt32(ThresholdPixelScanPercentageSetupEnd.SelectedItem);
            Properties.Settings.Default.ThresholdPixelScanPercentageIncrement = Convert.ToInt32(ThresholdPixelScanPercentageSetupIncrement.SelectedItem);

            Properties.Settings.Default.ThresholdSecondsSkipStart = float.Parse(ThresholdSecondsSkipSetupStart.SelectedItem.ToString());
            Properties.Settings.Default.ThresholdSecondsSkipEnd = float.Parse(ThresholdSecondsSkipSetupEnd.SelectedItem.ToString());
            Properties.Settings.Default.ThresholdSecondsSkipIncrement = float.Parse(ThresholdSecondsSkipIncrement.SelectedItem.ToString());

            Properties.Settings.Default.ThresholdConsecutiveDarkFramesInSecondsStart = float.Parse(ThresholdConsecutiveDarkFramesInSecondsStart.SelectedItem.ToString());
            Properties.Settings.Default.ThresholdConsecutiveDarkFramesInSecondsEnd = float.Parse(ThresholdConsecutiveDarkFramesInSecondsEnd.SelectedItem.ToString());
            Properties.Settings.Default.ThresholdConsecutiveDarkFramesInSecondsIncrement = float.Parse(ThresholdConsecutiveDarkFramesInSecondsIncrement.SelectedItem.ToString());

            Properties.Settings.Default.Save();
            #endregion

            // verify we have write access to excel spreadsheet
            try
            {
                if (File.Exists(GetTuningPath().FullName))
                {
                    // the following line will cause exception if file is open
                    new StreamWriter(GetTuningPath().FullName, true);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Test pass output file is open! Please close it before continuing.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


            _startTime = DateTime.Now;

            AdjustConcurrentScanCount_Tick(sender, e); // refresh allowed concurrent scan count

            HostWorker.Initialize(this);
            var workerThread = new Thread(HostWorker.DoWork);

            // Start the worker thread.
            workerThread.Start();
            Logger.Info("main thread: Starting worker thread...");

            

            

        }



        public FileInfo GetTuningPath()
        {
            return new FileInfo(Path.Combine(TuningResultsDirectoryTextBox.Text, Program.TUNING_RESULTS_FILENAME));
        }



        private void TuningForm_Load(object sender, EventArgs e)
        {

#if !TEST
            MessageBox.Show("Please run under TEST configuration!");
            Application.Exit();
#endif


            // not needed. this is done in test suite now
            //Engine.Initialize("ChuckTaylor44");

            UpdateTotalPassesCount();


        }


        private void UpdateLabels()
        {

            CurrentIndividualPixelBrightnessThresholdLabelValue.Text = HostWorker.CurrentIndividualPixelBrightnessThreshold.ToString(CultureInfo.CurrentCulture);
            CurrentDarknessThresholdPercentageLabelValue.Text = HostWorker.CurrentDarknessPercentageThreshold.ToString(CultureInfo.CurrentCulture);
            CurrentPixelScanPercentageLabelValue.Text = HostWorker.CurrentPixelScanPercentageThreshold.ToString(CultureInfo.CurrentCulture);
            CurrentSecondsSkipLabelValue.Text = HostWorker.CurrentSecondsSkipThreshold.ToString(CultureInfo.CurrentCulture);
            CurrentConsecutiveDarkFramesInSecondsLabelValue.Text = HostWorker.CurrentConsecutiveDarkFramesInSecondsThreshold.ToString(CultureInfo.CurrentCulture);


            if (HostWorker.InputFileCount > 0)
                PassProgressLabelValue.Text = HostWorker.CompletedScansInThisPass.ToString(CultureInfo.InvariantCulture) + " of " + HostWorker.InputFileCount;
            else
                PassProgressLabelValue.Text = "~";

            CompletedPassesLabelValue.Text = HostWorker.CompletedPassesCount.ToString(CultureInfo.InvariantCulture) + " of " + HostWorker.TotalPassesCount;

            ConcurrentScansRunningLabelValue.Text = HostWorker.ScanProcessList.Count + " of " + HostWorker.AllowedConcurrentScanCount;
            
        }

        private void CancelTestButton_Click(object sender, EventArgs e)
        {
            CancelTestButton.Text = "Cancelling...";
            CancelTestButton.Enabled = false;
            HostWorker.CancelTest = true;

            
        }

        private void TuningForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            HostWorker.Cleanup();
            //Engine.Dispose();
        }

        private void ResetButton_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure?", "Reset?", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                Properties.Settings.Default.BestDarknessPercentageThreshold = 0;
                Properties.Settings.Default.BestIndividualPixelBrightnessThreshold = 0;
                Properties.Settings.Default.BestSecondsSkipThreshold = 0;
                Properties.Settings.Default.BestPixelScanPercentageThreshold = 0;
                Properties.Settings.Default.BestActualScore = int.MinValue;
                Properties.Settings.Default.CurrentThresholdIndividualPixelBrightness = 0;
                Properties.Settings.Default.CurrentThresholdDarkPixelsPerFrameAsPercentage = 0;
                Properties.Settings.Default.CurrentThresholdPixelScanPercentage = 0;
                Properties.Settings.Default.CurrentThresholdSecondsSkip = 0;
                Properties.Settings.Default.CurrentThresholdConsecutiveDarkFramesInSeconds = 0;
                Properties.Settings.Default.CompletedPassesCount = 0;
                Properties.Settings.Default.Save();
                HostWorker.CompletedPassesCount = 0;
                HostWorker.CompletedPassesCountThisSession = 0;
                SetThresholdSetupComboBoxesEnableValue(true);
                UpdateLabels();

                // deleted tuning graph log
                try
                {
                    // Directory.Delete(TuningResultsDirectoryTextBox.Text); // doesn't work if directory isn't empty
                    File.Delete(GetTuningPath().FullName);
                }
                catch (Exception ex) {
                    MessageBox.Show("Unable to delete " + GetTuningPath().FullName + Environment.NewLine + Environment.NewLine +
                        ex);
                }


            }
        }

        private void ProgressTimer_Tick(object sender, EventArgs e)
        {

            UpdateLabels();

            if (HostWorker.CompletedPassesCountThisSession == 0)
            {
                TimeRemainingValueLabel.Text = "~";
                return;
            }

            TimeSpan duration = DateTime.Now - _startTime;

            TimeSpan totalTime = TimeSpan.FromSeconds(duration.TotalSeconds * (HostWorker.TotalPassesCount - HostWorker.CompletedPassesCountAtSessionStart) / HostWorker.CompletedPassesCountThisSession);

            TimeSpan timeLeft = totalTime - duration;

            if (timeLeft.Days > 0)
                TimeRemainingValueLabel.Text = timeLeft.Days + "d " + timeLeft.Hours + "h left";
            else if (timeLeft.Hours > 0)
                TimeRemainingValueLabel.Text = timeLeft.Hours + "h " + timeLeft.Minutes + "m left";
            else if (timeLeft.Minutes > 0)
                TimeRemainingValueLabel.Text = timeLeft.Minutes + "m left";
            else
                TimeRemainingValueLabel.Text = "<1m left";

        }

        private void TestButton_Click(object sender, EventArgs e)
        {

            

        }

        private void TuningResultsDirectoryBrowseButton_Click(object sender, EventArgs e)
        {

            TuningResultsBrowserDialog.SelectedPath = TuningResultsDirectoryTextBox.Text;
            TuningResultsBrowserDialog.ShowNewFolderButton = false;
            TuningResultsBrowserDialog.Description = "Where should the tuning results be saved?";
            if (TuningResultsBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                if (Directory.Exists(TuningResultsBrowserDialog.SelectedPath))
                {
                    TuningResultsDirectoryTextBox.Text = TuningResultsBrowserDialog.SelectedPath;
                    Properties.Settings.Default.TuningResultsDirectory = TuningResultsBrowserDialog.SelectedPath;
                    Properties.Settings.Default.Save();
                }
            }
        }

        private void TrimButton_Click(object sender, EventArgs e)
        {
            MainModel.Initialize();

            OpenFileDialog.CheckFileExists = true;
            OpenFileDialog.FileName = "";

            string supportedExtensionsAsSingleString = "";
            foreach (string supportedExtension in MainModel.GetVideoExtensions())
            {
                supportedExtensionsAsSingleString += "*" + supportedExtension + ";";
            }
            if (supportedExtensionsAsSingleString.Length > 0)
                supportedExtensionsAsSingleString = supportedExtensionsAsSingleString.Substring(0, supportedExtensionsAsSingleString.Length - 1);
            
            OpenFileDialog.Multiselect = false;
            OpenFileDialog.Title = "Tell me which video you'd like to trim.";
            OpenFileDialog.Filter = "Supported Files (" + supportedExtensionsAsSingleString + ")|" + supportedExtensionsAsSingleString + "|All Files (*.*)|*.*";
            OpenFileDialog.ShowDialog();

            if (File.Exists(OpenFileDialog.FileName))
            {
                var inputFileObject = new InputFileObject(new FileInfo(OpenFileDialog.FileName));

                string ss = "0:00:00", end = "0:00:00";
                if (InputBox("Start time", "What is the starting time that you'd like to trim from? (e.g. 0:00:20)", ref ss) == DialogResult.OK)
                {
                    if (InputBox("End time", "What is the ending time that you'd like to trim to? (e.g. 0:00:35)", ref end) == DialogResult.OK)
                    {

                        var saveFileDialog = new SaveFileDialog();
                        saveFileDialog.InitialDirectory = inputFileObject.SourceFileInfo.DirectoryName;
                        saveFileDialog.FileName = inputFileObject.SourceFileInfo.Name + " - " + ss + " to " + end + inputFileObject.SourceFileInfo.Extension;
                        saveFileDialog.Title = "Where should we save the new file?";

                        if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                        {

                            using (var scanWorker = new ScanWorker(inputFileObject))
                            {
                        
                                scanWorker.SetFramesPerSecond();
                                scanWorker.SetBitrate();

                                var highlightObject = new HighlightObject();
                                highlightObject.InputFileObject = inputFileObject;
                                highlightObject.StartTime = TimeSpan.Parse(ss); // ex. 0:06
                                highlightObject.EndTime = TimeSpan.Parse(end);

                                var saveWorker = new SaveWorker(highlightObject);
                                saveWorker.RunWorkerAsync(saveFileDialog.FileName);

                                while (saveWorker.PublishWorkerResult == PublishWorker.PublishWorkerResults.NotFinished)
                                    Application.DoEvents();

                                if (saveWorker.PublishWorkerResult == PublishWorker.PublishWorkerResults.Success)
                                    while (saveWorker.OutputFileInfo == null)
                                        Application.DoEvents();


                                var files = new List<FileInfo> { saveWorker.OutputFileInfo };
                                MainModel.LaunchExplorerWithFilesSelected(files);

                                

                            }

                        }

                    }

                }
            }


            MainModel.Dispose();
        }



        public static DialogResult InputBox(string title, string promptText, ref string value)
        {
            var form = new Form();
            var label = new Label();
            var textBox = new TextBox();
            var buttonOk = new Button();
            var buttonCancel = new Button();

            form.Text = title;
            label.Text = promptText;
            textBox.Text = value;

            buttonOk.Text = "OK";
            buttonCancel.Text = "Cancel";
            buttonOk.DialogResult = DialogResult.OK;
            buttonCancel.DialogResult = DialogResult.Cancel;

            label.SetBounds(9, 20, 372, 13);
            textBox.SetBounds(12, 36, 372, 20);
            buttonOk.SetBounds(228, 72, 75, 23);
            buttonCancel.SetBounds(309, 72, 75, 23);

            label.AutoSize = true;
            textBox.Anchor = textBox.Anchor | AnchorStyles.Right;
            buttonOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            buttonCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

            form.ClientSize = new Size(396, 107);
            form.Controls.AddRange(new Control[] { label, textBox, buttonOk, buttonCancel });
            form.ClientSize = new Size(Math.Max(300, label.Right + 10), form.ClientSize.Height);
            form.FormBorderStyle = FormBorderStyle.FixedDialog;
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MinimizeBox = false;
            form.MaximizeBox = false;
            form.AcceptButton = buttonOk;
            form.CancelButton = buttonCancel;

            DialogResult dialogResult = form.ShowDialog();
            value = textBox.Text;
            return dialogResult;
        }

        private void AdjustConcurrentScanCount_Tick(object sender, EventArgs e)
        {

            const int cpuRequiredPerProcess = 20; // actual is closer to 20
            const int cpuBuffer = 20; // always save 30% CPU for random spikes

            const int mBytesRequiredPerProcess = 40; // actual is closer to 60
            const int mBytesBuffer = 400; // always save 500mb of RAM for random spikes

            var cpuCounter = new PerformanceCounter
                                 {CategoryName = "Processor", CounterName = "% Processor Time", InstanceName = "_Total"};




            var ramCounter = new PerformanceCounter {CategoryName = "Memory", CounterName = "Available MBytes"};

            cpuCounter.NextValue(); // for some reason this returns 0 on the first call
            ramCounter.NextValue();
            Thread.Sleep(500);

            int freeCycles = 100 - Convert.ToInt32(cpuCounter.NextValue()) - cpuBuffer;

            long freeMBytes = (long)ramCounter.NextValue() - mBytesBuffer;

            int allowedViaCPUCalc = (freeCycles + cpuRequiredPerProcess * HostWorker.ScanProcessList.Count) / cpuRequiredPerProcess;
            int allowedViaRAMCalc = Convert.ToInt32((freeMBytes + mBytesRequiredPerProcess * HostWorker.ScanProcessList.Count) / mBytesRequiredPerProcess);

            HostWorker.AllowedConcurrentScanCount = Math.Min(allowedViaCPUCalc, allowedViaRAMCalc);

            cpuCounter.Close();

            ramCounter.Close();

        }


        private void PopulateThresholdSetupComboBoxes()
        {
            for (int i = 30; i <= 100; i++)
            {
                ThresholdIndividualPixelBrightnessSetupStart.Items.Add(i.ToString(CultureInfo.InvariantCulture));
                ThresholdIndividualPixelBrightnessSetupEnd.Items.Add(i.ToString(CultureInfo.InvariantCulture));
            }
            for (int i = 1; i <= 9; i++)
            {
                ThresholdIndividualPixelBrightnessSetupIncrement.Items.Add(i.ToString(CultureInfo.InvariantCulture));
                ThresholdDarkPixelsPerFrameAsPercentageSetupIncrement.Items.Add(i.ToString(CultureInfo.InvariantCulture));
            }

            for (int i = 30; i <= 100; i += 5)
            {
                ThresholdDarkPixelsPerFrameAsPercentageSetupStart.Items.Add(i.ToString(CultureInfo.InvariantCulture));
                ThresholdDarkPixelsPerFrameAsPercentageSetupEnd.Items.Add(i.ToString(CultureInfo.InvariantCulture));
            }

            for (int i = 1; i <= 100; i += 1)
            {
                ThresholdPixelScanPercentageSetupStart.Items.Add(i.ToString(CultureInfo.InvariantCulture));
                ThresholdPixelScanPercentageSetupEnd.Items.Add(i.ToString(CultureInfo.InvariantCulture));
            }
            for (int i = 1; i <= 20; i++)
            {
                ThresholdPixelScanPercentageSetupIncrement.Items.Add(i.ToString(CultureInfo.InvariantCulture));
            }

            for (float i = 0.02F; i <= 1.4; i += 0.01F)
            {
                ThresholdSecondsSkipSetupStart.Items.Add(Math.Round(i, 2).ToString(CultureInfo.InvariantCulture).Substring(1));
                ThresholdSecondsSkipSetupEnd.Items.Add(Math.Round(i, 2).ToString(CultureInfo.InvariantCulture).Substring(1));
            }

            for (float i = 0.01F; i <= 0.1; i += 0.01F)
            {
                ThresholdSecondsSkipIncrement.Items.Add(Math.Round(i, 2).ToString(CultureInfo.InvariantCulture).Substring(1));
            }

            for (var i = 0.0F; i <= 1.4; i += 0.1F)
            {
                ThresholdConsecutiveDarkFramesInSecondsStart.Items.Add(Math.Round(i, 2).ToString(CultureInfo.InvariantCulture));
                ThresholdConsecutiveDarkFramesInSecondsEnd.Items.Add(Math.Round(i, 2).ToString(CultureInfo.InvariantCulture));
            }
            for (var i = 0.1F; i <= 0.6; i += 0.1F)
            {
                ThresholdConsecutiveDarkFramesInSecondsIncrement.Items.Add(Math.Round(i, 2).ToString(CultureInfo.InvariantCulture));
            }

        }


        private void LoadThresholdSettings()
        {
            ThresholdIndividualPixelBrightnessSetupStart.SelectedItem = Properties.Settings.Default.ThresholdIndividualPixelBrightnessStart.ToString(CultureInfo.InvariantCulture);
            ThresholdIndividualPixelBrightnessSetupEnd.SelectedItem = Properties.Settings.Default.ThresholdIndividualPixelBrightnessEnd.ToString(CultureInfo.InvariantCulture);
            ThresholdIndividualPixelBrightnessSetupIncrement.SelectedItem = Properties.Settings.Default.ThresholdIndividualPixelBrightnessIncrement.ToString(CultureInfo.InvariantCulture);

            ThresholdDarkPixelsPerFrameAsPercentageSetupStart.SelectedItem = Properties.Settings.Default.ThresholdDarkPixelsPerFrameAsPercentageStart.ToString(CultureInfo.InvariantCulture);
            ThresholdDarkPixelsPerFrameAsPercentageSetupEnd.SelectedItem = Properties.Settings.Default.ThresholdDarkPixelsPerFrameAsPercentageEnd.ToString(CultureInfo.InvariantCulture);
            ThresholdDarkPixelsPerFrameAsPercentageSetupIncrement.SelectedItem = Properties.Settings.Default.ThresholdDarkPixelsPerFrameAsPercentageIncrement.ToString(CultureInfo.InvariantCulture);

            ThresholdPixelScanPercentageSetupStart.SelectedItem = Properties.Settings.Default.ThresholdPixelScanPercentageStart.ToString(CultureInfo.InvariantCulture);
            ThresholdPixelScanPercentageSetupEnd.SelectedItem = Properties.Settings.Default.ThresholdPixelScanPercentageEnd.ToString(CultureInfo.InvariantCulture);
            ThresholdPixelScanPercentageSetupIncrement.SelectedItem = Properties.Settings.Default.ThresholdPixelScanPercentageIncrement.ToString(CultureInfo.InvariantCulture);

            ThresholdSecondsSkipSetupStart.SelectedItem = Math.Round(Properties.Settings.Default.ThresholdSecondsSkipStart, 2).ToString(CultureInfo.InvariantCulture).Substring(1);
            ThresholdSecondsSkipSetupEnd.SelectedItem = Math.Round(Properties.Settings.Default.ThresholdSecondsSkipEnd, 2).ToString(CultureInfo.InvariantCulture).Substring(1);
            ThresholdSecondsSkipIncrement.SelectedItem = Math.Round(Properties.Settings.Default.ThresholdSecondsSkipIncrement, 2).ToString(CultureInfo.InvariantCulture).Substring(1);

            ThresholdConsecutiveDarkFramesInSecondsStart.SelectedItem = Math.Round(Properties.Settings.Default.ThresholdConsecutiveDarkFramesInSecondsStart, 2).ToString(CultureInfo.InvariantCulture);
            ThresholdConsecutiveDarkFramesInSecondsEnd.SelectedItem = Math.Round(Properties.Settings.Default.ThresholdConsecutiveDarkFramesInSecondsEnd, 2).ToString(CultureInfo.InvariantCulture);
            ThresholdConsecutiveDarkFramesInSecondsIncrement.SelectedItem = Math.Round(Properties.Settings.Default.ThresholdConsecutiveDarkFramesInSecondsIncrement, 2).ToString(CultureInfo.InvariantCulture);
        }


        private void SetThresholdSetupComboBoxesEnableValue(bool value)
        {
            ThresholdIndividualPixelBrightnessSetupStart.Enabled = value;
            ThresholdIndividualPixelBrightnessSetupEnd.Enabled = value;
            ThresholdIndividualPixelBrightnessSetupIncrement.Enabled = value;

            ThresholdDarkPixelsPerFrameAsPercentageSetupStart.Enabled = value;
            ThresholdDarkPixelsPerFrameAsPercentageSetupEnd.Enabled = value;
            ThresholdDarkPixelsPerFrameAsPercentageSetupIncrement.Enabled = value;

            ThresholdPixelScanPercentageSetupStart.Enabled = value;
            ThresholdPixelScanPercentageSetupEnd.Enabled = value;
            ThresholdPixelScanPercentageSetupIncrement.Enabled = value;

            ThresholdSecondsSkipSetupStart.Enabled = value;
            ThresholdSecondsSkipSetupEnd.Enabled = value;
            ThresholdSecondsSkipIncrement.Enabled = value;

            ThresholdConsecutiveDarkFramesInSecondsStart.Enabled = value;
            ThresholdConsecutiveDarkFramesInSecondsEnd.Enabled = value;
            ThresholdConsecutiveDarkFramesInSecondsIncrement.Enabled = value;
        }

        private void ThresholdSetupValuesChanged(object sender, EventArgs e)
        {
            UpdateTotalPassesCount();
        }

        private void UpdateTotalPassesCount()
        {
            if (!Created)
                return; // we aren't loaded yet


            int m1 = (Convert.ToInt32(ThresholdIndividualPixelBrightnessSetupEnd.SelectedItem) - Convert.ToInt32(ThresholdIndividualPixelBrightnessSetupStart.SelectedItem)) / Convert.ToInt32(ThresholdIndividualPixelBrightnessSetupIncrement.SelectedItem) + 1;
            int m2 = (Convert.ToInt32(ThresholdDarkPixelsPerFrameAsPercentageSetupEnd.SelectedItem) - Convert.ToInt32(ThresholdDarkPixelsPerFrameAsPercentageSetupStart.SelectedItem)) / Convert.ToInt32(ThresholdDarkPixelsPerFrameAsPercentageSetupIncrement.SelectedItem) + 1;
            int m3 = (Convert.ToInt32(ThresholdPixelScanPercentageSetupEnd.SelectedItem) - Convert.ToInt32(ThresholdPixelScanPercentageSetupStart.SelectedItem)) / Convert.ToInt32(ThresholdPixelScanPercentageSetupIncrement.SelectedItem) + 1;
            int m4 = (int)Math.Floor((Convert.ToDecimal(ThresholdSecondsSkipSetupEnd.SelectedItem) - Convert.ToDecimal(ThresholdSecondsSkipSetupStart.SelectedItem)) / Convert.ToDecimal(ThresholdSecondsSkipIncrement.SelectedItem)) + 1;
            int m5 = (int)Math.Floor((Convert.ToDecimal(ThresholdConsecutiveDarkFramesInSecondsEnd.SelectedItem) - Convert.ToDecimal(ThresholdConsecutiveDarkFramesInSecondsStart.SelectedItem)) / Convert.ToDecimal(ThresholdConsecutiveDarkFramesInSecondsIncrement.SelectedItem)) + 1;

            HostWorker.TotalPassesCount = m1 * m2 * m3 * m4 * m5;

            Debug.Assert(HostWorker.TotalPassesCount > 0, "TotalPassesCount is not zero!");

            UpdateLabels();
        }

        private void TuningResultsDirectoryTextBox_Leave(object sender, EventArgs e)
        {
            Properties.Settings.Default.TuningResultsDirectory = TuningResultsDirectoryTextBox.Text;
            Properties.Settings.Default.Save();
        }



        /*
         * private void SmartTest()
        {

            PassApproach LastPassApproach = PassApproach.FirstApproach;
            int LastPassActualScore = 0;

            EngineFindDarkFramesUnitTest.Initialize(null);

            InputFileCount = EngineFindDarkFramesUnitTest.inputFiles.Count();

            while (Engine.CancelScan == false)
            {

                bool IsMissingHighlights = false;

                int PassActualScore = 0;
                int PassMaxScore = 0;

                NextInputFileNumber = 1;

                logger.Info("-------------");
                logger.Info("Starting pass number " + (CompletedPassesCount + 1));

                logger.Info("ThresholdConsecutiveDarkFramesInSeconds: " + Engine.CurrentThresholdConsecutiveDarkFramesInSeconds);
                logger.Info("PixelBrightnessThreshold: " + Engine.CurrentThresholdIndividualPixelBrightness);
                logger.Info("ThresholdDarkPixelsPerFrameAsPercentage: " + Engine.CurrentThresholdDarkPixelsPerFrameAsPercentage);

                logger.Info("LastPassActualScore: " + LastPassActualScore);
                logger.Info("LastPassApproach: " + LastPassApproach.ToString());

                foreach (KeyValuePair<string, FileInfo> kvp in EngineFindDarkFramesUnitTest.inputFiles)
                {

                    UpdateLabels();

                    logger.Info("Scanning " + kvp.Key);

                    EngineFindDarkFramesUnitTest.TestResult tr = EngineFindDarkFramesUnitTest.FindDarkFramesTest(kvp.Value, false);

                    logger.Info("Score: " + tr.ActualScore + " out of possible " + tr.MaxScore);

                    if (tr.IsMissingHighlights)
                    { // if any single pass misses highlights, let's quit and go looser
                        logger.Info("We missed highlights in " + kvp.Key + "! Let's skip rest of pass and loosen parameters.");
                        IsMissingHighlights = true;
                        break;
                    }

                    PassActualScore += tr.ActualScore;
                    PassMaxScore += tr.MaxScore;
                    NextInputFileNumber += 1;

                    if (CheckForCancel())
                        break;

                }

                CompletedPassesCount += 1;

                UpdateLabels();

                logger.Info("CompletedPassesCount = " + CompletedPassesCount);
                logger.Info("IsMissingHighlights = " + IsMissingHighlights.ToString());

                // analyze scores and make adjustments

                if (IsMissingHighlights)
                { // a test run missed highlights so let's loosen the thresholds
                    ThresholdMultiplier *= (float)2; // make a big change to get back into the green
                    logger.Info("Loosening threshold parameters due to missed highlight. Multiplier = " + ThresholdMultiplier);
                    Engine.LoosenThresholdParameters(ThresholdMultiplier);
                    continue;
                }

                // only print this if we haven't missed highlights
                logger.Info("PassMaxScore = " + PassMaxScore);
                logger.Info("PassActualScore = " + PassActualScore);

                if (PassActualScore == PassMaxScore)
                {
                    logger.Info("We hit our max score! Let's try to tighten threshold parameters and still get max score.");
                    // try to go tighter

                    Properties.Settings.Default.BestActualScore = PassActualScore;

                    // only save as best parameters if they're stricter. basically we hit the same max score but with tighter settings
                    if (Properties.Settings.Default.BestConsecutiveDarkFramesThresholdInSeconds < Engine.CurrentThresholdConsecutiveDarkFramesInSeconds)
                        Properties.Settings.Default.BestConsecutiveDarkFramesThresholdInSeconds = Engine.CurrentThresholdConsecutiveDarkFramesInSeconds;
                    if (Properties.Settings.Default.BestDarknessThresholdPercentage < Engine.CurrentThresholdDarkPixelsPerFrameAsPercentage)
                        Properties.Settings.Default.BestDarknessThresholdPercentage = Engine.CurrentThresholdDarkPixelsPerFrameAsPercentage;
                    if (Properties.Settings.Default.BestIndividualPixelBrightnessThreshold > Engine.CurrentThresholdIndividualPixelBrightness)
                        Properties.Settings.Default.BestIndividualPixelBrightnessThreshold = Engine.CurrentThresholdIndividualPixelBrightness;
                    Properties.Settings.Default.Save();

                    LastPassApproach = PassApproach.Stricter;
                    ThresholdMultiplier *= (float)1; // 0.5;
                    logger.Info("Tightening threshold parameters. Multiplier = " + ThresholdMultiplier);
                    Engine.TightenThresholdParameters(ThresholdMultiplier);
                    //MessageBox.Show("We hit the max score! Woohoo!");
                    //break;
                }
                else
                { // let's make some adjustments

                    if (PassActualScore > Properties.Settings.Default.BestActualScore)
                    { // we beat our last score!
                        logger.Info("Our score of " + PassActualScore + " beats our best score of " + Properties.Settings.Default.BestActualScore);

                        Properties.Settings.Default.BestActualScore = PassActualScore;
                        Properties.Settings.Default.BestConsecutiveDarkFramesThresholdInSeconds = Engine.CurrentThresholdConsecutiveDarkFramesInSeconds;
                        Properties.Settings.Default.BestDarknessThresholdPercentage = Engine.CurrentThresholdDarkPixelsPerFrameAsPercentage;
                        Properties.Settings.Default.BestIndividualPixelBrightnessThreshold = Engine.CurrentThresholdIndividualPixelBrightness;
                        Properties.Settings.Default.Save();

                    }

                    // make adjustments
                    if (LastPassActualScore == 0)
                    { // this was our first run so let's just make it tighter and see if our score goes up
                        logger.Info("This was our first run so let's tighten and see if our score goes up");
                        LastPassApproach = PassApproach.Stricter;

                        ThresholdMultiplier *= (float)1;
                        logger.Info("Tightening threshold parameters. Multiplier = " + ThresholdMultiplier);
                        Engine.TightenThresholdParameters(ThresholdMultiplier);
                    }
                    else if (PassActualScore >= LastPassActualScore)
                    { // we did better or the same, let's continue going in this direction
                        logger.Info("We did better so let's continue approach of " + LastPassApproach.ToString());
                        if (LastPassApproach == PassApproach.Looser)
                        { // loosen
                            LastPassApproach = PassApproach.Looser;
                            ThresholdMultiplier *= (float)1; // keep the same
                            logger.Info("Loosening threshold parameters. Multiplier = " + ThresholdMultiplier);
                            Engine.LoosenThresholdParameters(ThresholdMultiplier);
                        }
                        else if (LastPassApproach == PassApproach.Stricter)
                        { // tighten
                            LastPassApproach = PassApproach.Stricter;
                            ThresholdMultiplier *= (float)1; // keep the same
                            logger.Info("Tightening threshold parameters. Multiplier = " + ThresholdMultiplier);
                            Engine.TightenThresholdParameters(ThresholdMultiplier);
                        }
                        else
                        {
                            Debug.Fail("We should never get here");
                        }
                    }
                    else if (PassActualScore < LastPassActualScore)
                    { // we did worse, let's go the opposite direction
                        logger.Info("We did worse so let's NOT continue approach of " + LastPassApproach.ToString());
                        if (LastPassApproach == PassApproach.Looser)
                        { // tighten
                            LastPassApproach = PassApproach.Stricter;
                            ThresholdMultiplier *= (float)0.5;
                            logger.Info("Tightening threshold parameters. Multiplier = " + ThresholdMultiplier);
                            Engine.TightenThresholdParameters(ThresholdMultiplier);
                        }
                        else if (LastPassApproach == PassApproach.Stricter)
                        { // loosen
                            LastPassApproach = PassApproach.Looser;
                            ThresholdMultiplier *= (float)0.5;
                            logger.Info("Loosening threshold parameters. Multiplier = " + ThresholdMultiplier);
                            Engine.LoosenThresholdParameters(ThresholdMultiplier);
                        }
                        else
                        {
                            Debug.Fail("We should never get here");
                        }
                    }
                    else
                    {
                        Debug.Fail("We should never get here");
                    }

                } // make adjustments

                LastPassActualScore = PassActualScore;

                if (CheckForCancel())
                    break;


            } // while we're running test
        }

        */
    }
}

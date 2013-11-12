namespace TuningHostProject
{
    partial class TuningForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.GroupBox groupBox2;
            System.Windows.Forms.Label ConcurrentScansRunningLabel;
            System.Windows.Forms.Label TimeRemainingLabel;
            System.Windows.Forms.Label CompletedPassesLabel;
            System.Windows.Forms.Label label1;
            System.Windows.Forms.Label label2;
            System.Windows.Forms.Label label3;
            System.Windows.Forms.Label label4;
            System.Windows.Forms.Label label5;
            System.Windows.Forms.Label label6;
            System.Windows.Forms.Label label7;
            System.Windows.Forms.Label label8;
            System.Windows.Forms.Label label9;
            System.Windows.Forms.Label label10;
            System.Windows.Forms.Label label11;
            System.Windows.Forms.Label label12;
            System.Windows.Forms.Label label13;
            System.Windows.Forms.Label label15;
            System.Windows.Forms.Label label16;
            this.ConcurrentScansRunningLabelValue = new System.Windows.Forms.Label();
            this.TimeRemainingValueLabel = new System.Windows.Forms.Label();
            this.PassProgressLabelValue = new System.Windows.Forms.Label();
            this.CompletedPassesLabelValue = new System.Windows.Forms.Label();
            this.PassProgressLabel = new System.Windows.Forms.Label();
            this.CurrentPixelScanPercentageLabelValue = new System.Windows.Forms.Label();
            this.CurrentSecondsSkipLabelValue = new System.Windows.Forms.Label();
            this.CurrentIndividualPixelBrightnessThresholdLabelValue = new System.Windows.Forms.Label();
            this.CurrentDarknessThresholdPercentageLabelValue = new System.Windows.Forms.Label();
            this.CancelTestButton = new System.Windows.Forms.Button();
            this.RunButton = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.CurrentConsecutiveDarkFramesInSecondsLabelValue = new System.Windows.Forms.Label();
            this.ThresholdConsecutiveDarkFramesInSecondsIncrement = new System.Windows.Forms.ComboBox();
            this.ThresholdConsecutiveDarkFramesInSecondsEnd = new System.Windows.Forms.ComboBox();
            this.ThresholdConsecutiveDarkFramesInSecondsStart = new System.Windows.Forms.ComboBox();
            this.ThresholdPixelScanPercentageSetupIncrement = new System.Windows.Forms.ComboBox();
            this.ThresholdPixelScanPercentageSetupEnd = new System.Windows.Forms.ComboBox();
            this.ThresholdPixelScanPercentageSetupStart = new System.Windows.Forms.ComboBox();
            this.ThresholdSecondsSkipIncrement = new System.Windows.Forms.ComboBox();
            this.ThresholdSecondsSkipSetupEnd = new System.Windows.Forms.ComboBox();
            this.ThresholdSecondsSkipSetupStart = new System.Windows.Forms.ComboBox();
            this.ThresholdDarkPixelsPerFrameAsPercentageSetupIncrement = new System.Windows.Forms.ComboBox();
            this.ThresholdDarkPixelsPerFrameAsPercentageSetupEnd = new System.Windows.Forms.ComboBox();
            this.ThresholdDarkPixelsPerFrameAsPercentageSetupStart = new System.Windows.Forms.ComboBox();
            this.ThresholdIndividualPixelBrightnessSetupIncrement = new System.Windows.Forms.ComboBox();
            this.ThresholdIndividualPixelBrightnessSetupEnd = new System.Windows.Forms.ComboBox();
            this.ThresholdIndividualPixelBrightnessSetupStart = new System.Windows.Forms.ComboBox();
            this.ResetButton = new System.Windows.Forms.Button();
            this.ProgressTimer = new System.Windows.Forms.Timer(this.components);
            this.TestButton = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.TuningResultsDirectoryBrowseButton = new System.Windows.Forms.Button();
            this.TuningResultsDirectoryTextBox = new System.Windows.Forms.TextBox();
            this.TuningResultsBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.TrimButton = new System.Windows.Forms.Button();
            this.OpenFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.SaveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.AdjustConcurrentScanCount = new System.Windows.Forms.Timer(this.components);
            groupBox2 = new System.Windows.Forms.GroupBox();
            ConcurrentScansRunningLabel = new System.Windows.Forms.Label();
            TimeRemainingLabel = new System.Windows.Forms.Label();
            CompletedPassesLabel = new System.Windows.Forms.Label();
            label1 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            label4 = new System.Windows.Forms.Label();
            label5 = new System.Windows.Forms.Label();
            label6 = new System.Windows.Forms.Label();
            label7 = new System.Windows.Forms.Label();
            label8 = new System.Windows.Forms.Label();
            label9 = new System.Windows.Forms.Label();
            label10 = new System.Windows.Forms.Label();
            label11 = new System.Windows.Forms.Label();
            label12 = new System.Windows.Forms.Label();
            label13 = new System.Windows.Forms.Label();
            label15 = new System.Windows.Forms.Label();
            label16 = new System.Windows.Forms.Label();
            groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(this.ConcurrentScansRunningLabelValue);
            groupBox2.Controls.Add(ConcurrentScansRunningLabel);
            groupBox2.Controls.Add(this.TimeRemainingValueLabel);
            groupBox2.Controls.Add(TimeRemainingLabel);
            groupBox2.Controls.Add(this.PassProgressLabelValue);
            groupBox2.Controls.Add(this.CompletedPassesLabelValue);
            groupBox2.Controls.Add(this.PassProgressLabel);
            groupBox2.Controls.Add(CompletedPassesLabel);
            groupBox2.Font = new System.Drawing.Font("Calibri", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            groupBox2.Location = new System.Drawing.Point(20, 386);
            groupBox2.Margin = new System.Windows.Forms.Padding(2);
            groupBox2.Name = "groupBox2";
            groupBox2.Padding = new System.Windows.Forms.Padding(2);
            groupBox2.Size = new System.Drawing.Size(574, 179);
            groupBox2.TabIndex = 118;
            groupBox2.TabStop = false;
            groupBox2.Text = "Statistics ";
            // 
            // ConcurrentScansRunningLabelValue
            // 
            this.ConcurrentScansRunningLabelValue.BackColor = System.Drawing.SystemColors.Control;
            this.ConcurrentScansRunningLabelValue.Font = new System.Drawing.Font("Calibri", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ConcurrentScansRunningLabelValue.Location = new System.Drawing.Point(466, 142);
            this.ConcurrentScansRunningLabelValue.Name = "ConcurrentScansRunningLabelValue";
            this.ConcurrentScansRunningLabelValue.Size = new System.Drawing.Size(87, 17);
            this.ConcurrentScansRunningLabelValue.TabIndex = 17;
            this.ConcurrentScansRunningLabelValue.Text = "~";
            this.ConcurrentScansRunningLabelValue.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // ConcurrentScansRunningLabel
            // 
            ConcurrentScansRunningLabel.AutoSize = true;
            ConcurrentScansRunningLabel.Font = new System.Drawing.Font("Calibri", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            ConcurrentScansRunningLabel.Location = new System.Drawing.Point(19, 142);
            ConcurrentScansRunningLabel.Name = "ConcurrentScansRunningLabel";
            ConcurrentScansRunningLabel.Size = new System.Drawing.Size(158, 17);
            ConcurrentScansRunningLabel.TabIndex = 16;
            ConcurrentScansRunningLabel.Text = "Concurrent Scans Running:";
            // 
            // TimeRemainingValueLabel
            // 
            this.TimeRemainingValueLabel.BackColor = System.Drawing.SystemColors.Control;
            this.TimeRemainingValueLabel.Font = new System.Drawing.Font("Calibri", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TimeRemainingValueLabel.Location = new System.Drawing.Point(467, 107);
            this.TimeRemainingValueLabel.Name = "TimeRemainingValueLabel";
            this.TimeRemainingValueLabel.Size = new System.Drawing.Size(87, 17);
            this.TimeRemainingValueLabel.TabIndex = 15;
            this.TimeRemainingValueLabel.Text = "~";
            this.TimeRemainingValueLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // TimeRemainingLabel
            // 
            TimeRemainingLabel.AutoSize = true;
            TimeRemainingLabel.Font = new System.Drawing.Font("Calibri", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            TimeRemainingLabel.Location = new System.Drawing.Point(19, 107);
            TimeRemainingLabel.Name = "TimeRemainingLabel";
            TimeRemainingLabel.Size = new System.Drawing.Size(103, 17);
            TimeRemainingLabel.TabIndex = 14;
            TimeRemainingLabel.Text = "Time Remaining:";
            // 
            // PassProgressLabelValue
            // 
            this.PassProgressLabelValue.BackColor = System.Drawing.SystemColors.Control;
            this.PassProgressLabelValue.Font = new System.Drawing.Font("Calibri", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PassProgressLabelValue.Location = new System.Drawing.Point(443, 38);
            this.PassProgressLabelValue.Name = "PassProgressLabelValue";
            this.PassProgressLabelValue.Size = new System.Drawing.Size(111, 17);
            this.PassProgressLabelValue.TabIndex = 12;
            this.PassProgressLabelValue.Text = "1 of 3";
            this.PassProgressLabelValue.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // CompletedPassesLabelValue
            // 
            this.CompletedPassesLabelValue.BackColor = System.Drawing.SystemColors.Control;
            this.CompletedPassesLabelValue.Font = new System.Drawing.Font("Calibri", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CompletedPassesLabelValue.Location = new System.Drawing.Point(429, 72);
            this.CompletedPassesLabelValue.Name = "CompletedPassesLabelValue";
            this.CompletedPassesLabelValue.Size = new System.Drawing.Size(125, 17);
            this.CompletedPassesLabelValue.TabIndex = 11;
            this.CompletedPassesLabelValue.Text = "0 of 100";
            this.CompletedPassesLabelValue.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // PassProgressLabel
            // 
            this.PassProgressLabel.AutoSize = true;
            this.PassProgressLabel.Font = new System.Drawing.Font("Calibri", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PassProgressLabel.Location = new System.Drawing.Point(19, 38);
            this.PassProgressLabel.Name = "PassProgressLabel";
            this.PassProgressLabel.Size = new System.Drawing.Size(170, 17);
            this.PassProgressLabel.TabIndex = 10;
            this.PassProgressLabel.Text = "Scans Completed in this Pass:";
            // 
            // CompletedPassesLabel
            // 
            CompletedPassesLabel.AutoSize = true;
            CompletedPassesLabel.Font = new System.Drawing.Font("Calibri", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            CompletedPassesLabel.Location = new System.Drawing.Point(19, 72);
            CompletedPassesLabel.Name = "CompletedPassesLabel";
            CompletedPassesLabel.Size = new System.Drawing.Size(112, 17);
            CompletedPassesLabel.TabIndex = 8;
            CompletedPassesLabel.Text = "Passes Completed:";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new System.Drawing.Font("Calibri", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            label1.Location = new System.Drawing.Point(19, 78);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(203, 17);
            label1.TabIndex = 23;
            label1.Text = "DarkPixelsPerFrameAsPercentage:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new System.Drawing.Font("Calibri", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            label2.Location = new System.Drawing.Point(19, 155);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(79, 17);
            label2.TabIndex = 22;
            label2.Text = "SecondsSkip:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new System.Drawing.Font("Calibri", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            label3.Location = new System.Drawing.Point(19, 40);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(151, 17);
            label3.TabIndex = 21;
            label3.Text = "IndividualPixelBrightness:";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new System.Drawing.Font("Calibri", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            label4.Location = new System.Drawing.Point(322, 40);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(20, 17);
            label4.TabIndex = 25;
            label4.Text = "to";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new System.Drawing.Font("Calibri", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            label5.Location = new System.Drawing.Point(400, 40);
            label5.Name = "label5";
            label5.Size = new System.Drawing.Size(21, 17);
            label5.TabIndex = 26;
            label5.Text = "by";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new System.Drawing.Font("Calibri", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            label6.Location = new System.Drawing.Point(400, 78);
            label6.Name = "label6";
            label6.Size = new System.Drawing.Size(21, 17);
            label6.TabIndex = 31;
            label6.Text = "by";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Font = new System.Drawing.Font("Calibri", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            label7.Location = new System.Drawing.Point(322, 78);
            label7.Name = "label7";
            label7.Size = new System.Drawing.Size(20, 17);
            label7.TabIndex = 30;
            label7.Text = "to";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Font = new System.Drawing.Font("Calibri", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            label8.Location = new System.Drawing.Point(400, 155);
            label8.Name = "label8";
            label8.Size = new System.Drawing.Size(21, 17);
            label8.TabIndex = 36;
            label8.Text = "by";
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Font = new System.Drawing.Font("Calibri", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            label9.Location = new System.Drawing.Point(322, 155);
            label9.Name = "label9";
            label9.Size = new System.Drawing.Size(20, 17);
            label9.TabIndex = 35;
            label9.Text = "to";
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Font = new System.Drawing.Font("Calibri", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            label10.Location = new System.Drawing.Point(400, 116);
            label10.Name = "label10";
            label10.Size = new System.Drawing.Size(21, 17);
            label10.TabIndex = 42;
            label10.Text = "by";
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Font = new System.Drawing.Font("Calibri", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            label11.Location = new System.Drawing.Point(322, 116);
            label11.Name = "label11";
            label11.Size = new System.Drawing.Size(20, 17);
            label11.TabIndex = 41;
            label11.Text = "to";
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.Font = new System.Drawing.Font("Calibri", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            label12.Location = new System.Drawing.Point(19, 116);
            label12.Name = "label12";
            label12.Size = new System.Drawing.Size(129, 17);
            label12.TabIndex = 38;
            label12.Text = "PixelScanPercentage:";
            // 
            // label13
            // 
            label13.AutoSize = true;
            label13.Font = new System.Drawing.Font("Calibri", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            label13.Location = new System.Drawing.Point(400, 193);
            label13.Name = "label13";
            label13.Size = new System.Drawing.Size(21, 17);
            label13.TabIndex = 48;
            label13.Text = "by";
            // 
            // label15
            // 
            label15.AutoSize = true;
            label15.Font = new System.Drawing.Font("Calibri", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            label15.Location = new System.Drawing.Point(322, 193);
            label15.Name = "label15";
            label15.Size = new System.Drawing.Size(20, 17);
            label15.TabIndex = 47;
            label15.Text = "to";
            // 
            // label16
            // 
            label16.AutoSize = true;
            label16.Font = new System.Drawing.Font("Calibri", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            label16.Location = new System.Drawing.Point(19, 193);
            label16.Name = "label16";
            label16.Size = new System.Drawing.Size(203, 17);
            label16.TabIndex = 44;
            label16.Text = "ConsecutiveDarkFramesInSeconds:";
            // 
            // CurrentPixelScanPercentageLabelValue
            // 
            this.CurrentPixelScanPercentageLabelValue.AutoSize = true;
            this.CurrentPixelScanPercentageLabelValue.Font = new System.Drawing.Font("Calibri", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CurrentPixelScanPercentageLabelValue.Location = new System.Drawing.Point(523, 155);
            this.CurrentPixelScanPercentageLabelValue.Name = "CurrentPixelScanPercentageLabelValue";
            this.CurrentPixelScanPercentageLabelValue.Size = new System.Drawing.Size(33, 17);
            this.CurrentPixelScanPercentageLabelValue.TabIndex = 25;
            this.CurrentPixelScanPercentageLabelValue.Text = "1.15";
            // 
            // CurrentSecondsSkipLabelValue
            // 
            this.CurrentSecondsSkipLabelValue.AutoSize = true;
            this.CurrentSecondsSkipLabelValue.Font = new System.Drawing.Font("Calibri", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CurrentSecondsSkipLabelValue.Location = new System.Drawing.Point(523, 116);
            this.CurrentSecondsSkipLabelValue.Name = "CurrentSecondsSkipLabelValue";
            this.CurrentSecondsSkipLabelValue.Size = new System.Drawing.Size(33, 17);
            this.CurrentSecondsSkipLabelValue.TabIndex = 22;
            this.CurrentSecondsSkipLabelValue.Text = "1.15";
            // 
            // CurrentIndividualPixelBrightnessThresholdLabelValue
            // 
            this.CurrentIndividualPixelBrightnessThresholdLabelValue.AutoSize = true;
            this.CurrentIndividualPixelBrightnessThresholdLabelValue.Font = new System.Drawing.Font("Calibri", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CurrentIndividualPixelBrightnessThresholdLabelValue.Location = new System.Drawing.Point(523, 40);
            this.CurrentIndividualPixelBrightnessThresholdLabelValue.Name = "CurrentIndividualPixelBrightnessThresholdLabelValue";
            this.CurrentIndividualPixelBrightnessThresholdLabelValue.Size = new System.Drawing.Size(22, 17);
            this.CurrentIndividualPixelBrightnessThresholdLabelValue.TabIndex = 21;
            this.CurrentIndividualPixelBrightnessThresholdLabelValue.Text = "90";
            // 
            // CurrentDarknessThresholdPercentageLabelValue
            // 
            this.CurrentDarknessThresholdPercentageLabelValue.AutoSize = true;
            this.CurrentDarknessThresholdPercentageLabelValue.Font = new System.Drawing.Font("Calibri", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CurrentDarknessThresholdPercentageLabelValue.Location = new System.Drawing.Point(523, 78);
            this.CurrentDarknessThresholdPercentageLabelValue.Name = "CurrentDarknessThresholdPercentageLabelValue";
            this.CurrentDarknessThresholdPercentageLabelValue.Size = new System.Drawing.Size(22, 17);
            this.CurrentDarknessThresholdPercentageLabelValue.TabIndex = 23;
            this.CurrentDarknessThresholdPercentageLabelValue.Text = "85";
            // 
            // CancelTestButton
            // 
            this.CancelTestButton.Enabled = false;
            this.CancelTestButton.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CancelTestButton.Location = new System.Drawing.Point(134, 10);
            this.CancelTestButton.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.CancelTestButton.Name = "CancelTestButton";
            this.CancelTestButton.Size = new System.Drawing.Size(115, 32);
            this.CancelTestButton.TabIndex = 115;
            this.CancelTestButton.Text = "&Cancel";
            this.CancelTestButton.UseVisualStyleBackColor = true;
            this.CancelTestButton.Click += new System.EventHandler(this.CancelTestButton_Click);
            // 
            // RunButton
            // 
            this.RunButton.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RunButton.Location = new System.Drawing.Point(9, 10);
            this.RunButton.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.RunButton.Name = "RunButton";
            this.RunButton.Size = new System.Drawing.Size(115, 32);
            this.RunButton.TabIndex = 116;
            this.RunButton.Text = "&Start";
            this.RunButton.UseVisualStyleBackColor = true;
            this.RunButton.Click += new System.EventHandler(this.RunButton_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.CurrentConsecutiveDarkFramesInSecondsLabelValue);
            this.groupBox1.Controls.Add(this.ThresholdConsecutiveDarkFramesInSecondsIncrement);
            this.groupBox1.Controls.Add(label13);
            this.groupBox1.Controls.Add(this.CurrentPixelScanPercentageLabelValue);
            this.groupBox1.Controls.Add(label15);
            this.groupBox1.Controls.Add(this.ThresholdConsecutiveDarkFramesInSecondsEnd);
            this.groupBox1.Controls.Add(this.CurrentSecondsSkipLabelValue);
            this.groupBox1.Controls.Add(this.ThresholdConsecutiveDarkFramesInSecondsStart);
            this.groupBox1.Controls.Add(this.CurrentIndividualPixelBrightnessThresholdLabelValue);
            this.groupBox1.Controls.Add(label16);
            this.groupBox1.Controls.Add(this.ThresholdPixelScanPercentageSetupIncrement);
            this.groupBox1.Controls.Add(this.CurrentDarknessThresholdPercentageLabelValue);
            this.groupBox1.Controls.Add(label10);
            this.groupBox1.Controls.Add(label11);
            this.groupBox1.Controls.Add(this.ThresholdPixelScanPercentageSetupEnd);
            this.groupBox1.Controls.Add(this.ThresholdPixelScanPercentageSetupStart);
            this.groupBox1.Controls.Add(label12);
            this.groupBox1.Controls.Add(this.ThresholdSecondsSkipIncrement);
            this.groupBox1.Controls.Add(label8);
            this.groupBox1.Controls.Add(label9);
            this.groupBox1.Controls.Add(this.ThresholdSecondsSkipSetupEnd);
            this.groupBox1.Controls.Add(this.ThresholdSecondsSkipSetupStart);
            this.groupBox1.Controls.Add(this.ThresholdDarkPixelsPerFrameAsPercentageSetupIncrement);
            this.groupBox1.Controls.Add(label6);
            this.groupBox1.Controls.Add(label7);
            this.groupBox1.Controls.Add(this.ThresholdDarkPixelsPerFrameAsPercentageSetupEnd);
            this.groupBox1.Controls.Add(this.ThresholdDarkPixelsPerFrameAsPercentageSetupStart);
            this.groupBox1.Controls.Add(this.ThresholdIndividualPixelBrightnessSetupIncrement);
            this.groupBox1.Controls.Add(label5);
            this.groupBox1.Controls.Add(label4);
            this.groupBox1.Controls.Add(this.ThresholdIndividualPixelBrightnessSetupEnd);
            this.groupBox1.Controls.Add(label1);
            this.groupBox1.Controls.Add(label2);
            this.groupBox1.Controls.Add(label3);
            this.groupBox1.Controls.Add(this.ThresholdIndividualPixelBrightnessSetupStart);
            this.groupBox1.Font = new System.Drawing.Font("Calibri", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(20, 147);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox1.Size = new System.Drawing.Size(574, 224);
            this.groupBox1.TabIndex = 117;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Threshold Parameters";
            // 
            // CurrentConsecutiveDarkFramesInSecondsLabelValue
            // 
            this.CurrentConsecutiveDarkFramesInSecondsLabelValue.AutoSize = true;
            this.CurrentConsecutiveDarkFramesInSecondsLabelValue.Font = new System.Drawing.Font("Calibri", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CurrentConsecutiveDarkFramesInSecondsLabelValue.Location = new System.Drawing.Point(523, 188);
            this.CurrentConsecutiveDarkFramesInSecondsLabelValue.Name = "CurrentConsecutiveDarkFramesInSecondsLabelValue";
            this.CurrentConsecutiveDarkFramesInSecondsLabelValue.Size = new System.Drawing.Size(26, 17);
            this.CurrentConsecutiveDarkFramesInSecondsLabelValue.TabIndex = 27;
            this.CurrentConsecutiveDarkFramesInSecondsLabelValue.Text = "0.8";
            // 
            // ThresholdConsecutiveDarkFramesInSecondsIncrement
            // 
            this.ThresholdConsecutiveDarkFramesInSecondsIncrement.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ThresholdConsecutiveDarkFramesInSecondsIncrement.Enabled = false;
            this.ThresholdConsecutiveDarkFramesInSecondsIncrement.Font = new System.Drawing.Font("Calibri", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ThresholdConsecutiveDarkFramesInSecondsIncrement.FormattingEnabled = true;
            this.ThresholdConsecutiveDarkFramesInSecondsIncrement.Location = new System.Drawing.Point(428, 185);
            this.ThresholdConsecutiveDarkFramesInSecondsIncrement.Name = "ThresholdConsecutiveDarkFramesInSecondsIncrement";
            this.ThresholdConsecutiveDarkFramesInSecondsIncrement.Size = new System.Drawing.Size(52, 25);
            this.ThresholdConsecutiveDarkFramesInSecondsIncrement.TabIndex = 49;
            this.ThresholdConsecutiveDarkFramesInSecondsIncrement.SelectedIndexChanged += new System.EventHandler(this.ThresholdSetupValuesChanged);
            // 
            // ThresholdConsecutiveDarkFramesInSecondsEnd
            // 
            this.ThresholdConsecutiveDarkFramesInSecondsEnd.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ThresholdConsecutiveDarkFramesInSecondsEnd.Enabled = false;
            this.ThresholdConsecutiveDarkFramesInSecondsEnd.Font = new System.Drawing.Font("Calibri", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ThresholdConsecutiveDarkFramesInSecondsEnd.FormattingEnabled = true;
            this.ThresholdConsecutiveDarkFramesInSecondsEnd.Location = new System.Drawing.Point(347, 185);
            this.ThresholdConsecutiveDarkFramesInSecondsEnd.Name = "ThresholdConsecutiveDarkFramesInSecondsEnd";
            this.ThresholdConsecutiveDarkFramesInSecondsEnd.Size = new System.Drawing.Size(52, 25);
            this.ThresholdConsecutiveDarkFramesInSecondsEnd.TabIndex = 46;
            this.ThresholdConsecutiveDarkFramesInSecondsEnd.SelectedIndexChanged += new System.EventHandler(this.ThresholdSetupValuesChanged);
            // 
            // ThresholdConsecutiveDarkFramesInSecondsStart
            // 
            this.ThresholdConsecutiveDarkFramesInSecondsStart.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ThresholdConsecutiveDarkFramesInSecondsStart.Enabled = false;
            this.ThresholdConsecutiveDarkFramesInSecondsStart.Font = new System.Drawing.Font("Calibri", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ThresholdConsecutiveDarkFramesInSecondsStart.FormattingEnabled = true;
            this.ThresholdConsecutiveDarkFramesInSecondsStart.Location = new System.Drawing.Point(268, 185);
            this.ThresholdConsecutiveDarkFramesInSecondsStart.Name = "ThresholdConsecutiveDarkFramesInSecondsStart";
            this.ThresholdConsecutiveDarkFramesInSecondsStart.Size = new System.Drawing.Size(52, 25);
            this.ThresholdConsecutiveDarkFramesInSecondsStart.TabIndex = 45;
            this.ThresholdConsecutiveDarkFramesInSecondsStart.SelectedIndexChanged += new System.EventHandler(this.ThresholdSetupValuesChanged);
            // 
            // ThresholdPixelScanPercentageSetupIncrement
            // 
            this.ThresholdPixelScanPercentageSetupIncrement.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ThresholdPixelScanPercentageSetupIncrement.Enabled = false;
            this.ThresholdPixelScanPercentageSetupIncrement.Font = new System.Drawing.Font("Calibri", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ThresholdPixelScanPercentageSetupIncrement.FormattingEnabled = true;
            this.ThresholdPixelScanPercentageSetupIncrement.Location = new System.Drawing.Point(428, 108);
            this.ThresholdPixelScanPercentageSetupIncrement.Name = "ThresholdPixelScanPercentageSetupIncrement";
            this.ThresholdPixelScanPercentageSetupIncrement.Size = new System.Drawing.Size(52, 25);
            this.ThresholdPixelScanPercentageSetupIncrement.TabIndex = 43;
            this.ThresholdPixelScanPercentageSetupIncrement.SelectedIndexChanged += new System.EventHandler(this.ThresholdSetupValuesChanged);
            // 
            // ThresholdPixelScanPercentageSetupEnd
            // 
            this.ThresholdPixelScanPercentageSetupEnd.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ThresholdPixelScanPercentageSetupEnd.Enabled = false;
            this.ThresholdPixelScanPercentageSetupEnd.Font = new System.Drawing.Font("Calibri", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ThresholdPixelScanPercentageSetupEnd.FormattingEnabled = true;
            this.ThresholdPixelScanPercentageSetupEnd.Location = new System.Drawing.Point(347, 108);
            this.ThresholdPixelScanPercentageSetupEnd.Name = "ThresholdPixelScanPercentageSetupEnd";
            this.ThresholdPixelScanPercentageSetupEnd.Size = new System.Drawing.Size(52, 25);
            this.ThresholdPixelScanPercentageSetupEnd.TabIndex = 40;
            this.ThresholdPixelScanPercentageSetupEnd.SelectedIndexChanged += new System.EventHandler(this.ThresholdSetupValuesChanged);
            // 
            // ThresholdPixelScanPercentageSetupStart
            // 
            this.ThresholdPixelScanPercentageSetupStart.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ThresholdPixelScanPercentageSetupStart.Enabled = false;
            this.ThresholdPixelScanPercentageSetupStart.Font = new System.Drawing.Font("Calibri", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ThresholdPixelScanPercentageSetupStart.FormattingEnabled = true;
            this.ThresholdPixelScanPercentageSetupStart.Location = new System.Drawing.Point(268, 108);
            this.ThresholdPixelScanPercentageSetupStart.Name = "ThresholdPixelScanPercentageSetupStart";
            this.ThresholdPixelScanPercentageSetupStart.Size = new System.Drawing.Size(52, 25);
            this.ThresholdPixelScanPercentageSetupStart.TabIndex = 39;
            this.ThresholdPixelScanPercentageSetupStart.SelectedIndexChanged += new System.EventHandler(this.ThresholdSetupValuesChanged);
            // 
            // ThresholdSecondsSkipIncrement
            // 
            this.ThresholdSecondsSkipIncrement.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ThresholdSecondsSkipIncrement.Enabled = false;
            this.ThresholdSecondsSkipIncrement.Font = new System.Drawing.Font("Calibri", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ThresholdSecondsSkipIncrement.FormattingEnabled = true;
            this.ThresholdSecondsSkipIncrement.Location = new System.Drawing.Point(428, 147);
            this.ThresholdSecondsSkipIncrement.Name = "ThresholdSecondsSkipIncrement";
            this.ThresholdSecondsSkipIncrement.Size = new System.Drawing.Size(52, 25);
            this.ThresholdSecondsSkipIncrement.TabIndex = 37;
            this.ThresholdSecondsSkipIncrement.SelectedIndexChanged += new System.EventHandler(this.ThresholdSetupValuesChanged);
            // 
            // ThresholdSecondsSkipSetupEnd
            // 
            this.ThresholdSecondsSkipSetupEnd.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ThresholdSecondsSkipSetupEnd.Enabled = false;
            this.ThresholdSecondsSkipSetupEnd.Font = new System.Drawing.Font("Calibri", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ThresholdSecondsSkipSetupEnd.FormattingEnabled = true;
            this.ThresholdSecondsSkipSetupEnd.Location = new System.Drawing.Point(347, 147);
            this.ThresholdSecondsSkipSetupEnd.Name = "ThresholdSecondsSkipSetupEnd";
            this.ThresholdSecondsSkipSetupEnd.Size = new System.Drawing.Size(52, 25);
            this.ThresholdSecondsSkipSetupEnd.TabIndex = 34;
            this.ThresholdSecondsSkipSetupEnd.SelectedIndexChanged += new System.EventHandler(this.ThresholdSetupValuesChanged);
            // 
            // ThresholdSecondsSkipSetupStart
            // 
            this.ThresholdSecondsSkipSetupStart.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ThresholdSecondsSkipSetupStart.Enabled = false;
            this.ThresholdSecondsSkipSetupStart.Font = new System.Drawing.Font("Calibri", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ThresholdSecondsSkipSetupStart.FormattingEnabled = true;
            this.ThresholdSecondsSkipSetupStart.Location = new System.Drawing.Point(268, 147);
            this.ThresholdSecondsSkipSetupStart.Name = "ThresholdSecondsSkipSetupStart";
            this.ThresholdSecondsSkipSetupStart.Size = new System.Drawing.Size(52, 25);
            this.ThresholdSecondsSkipSetupStart.TabIndex = 33;
            this.ThresholdSecondsSkipSetupStart.SelectedIndexChanged += new System.EventHandler(this.ThresholdSetupValuesChanged);
            // 
            // ThresholdDarkPixelsPerFrameAsPercentageSetupIncrement
            // 
            this.ThresholdDarkPixelsPerFrameAsPercentageSetupIncrement.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ThresholdDarkPixelsPerFrameAsPercentageSetupIncrement.Font = new System.Drawing.Font("Calibri", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ThresholdDarkPixelsPerFrameAsPercentageSetupIncrement.FormattingEnabled = true;
            this.ThresholdDarkPixelsPerFrameAsPercentageSetupIncrement.Location = new System.Drawing.Point(433, 70);
            this.ThresholdDarkPixelsPerFrameAsPercentageSetupIncrement.Name = "ThresholdDarkPixelsPerFrameAsPercentageSetupIncrement";
            this.ThresholdDarkPixelsPerFrameAsPercentageSetupIncrement.Size = new System.Drawing.Size(47, 25);
            this.ThresholdDarkPixelsPerFrameAsPercentageSetupIncrement.TabIndex = 32;
            this.ThresholdDarkPixelsPerFrameAsPercentageSetupIncrement.SelectedIndexChanged += new System.EventHandler(this.ThresholdSetupValuesChanged);
            // 
            // ThresholdDarkPixelsPerFrameAsPercentageSetupEnd
            // 
            this.ThresholdDarkPixelsPerFrameAsPercentageSetupEnd.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ThresholdDarkPixelsPerFrameAsPercentageSetupEnd.Font = new System.Drawing.Font("Calibri", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ThresholdDarkPixelsPerFrameAsPercentageSetupEnd.FormattingEnabled = true;
            this.ThresholdDarkPixelsPerFrameAsPercentageSetupEnd.Location = new System.Drawing.Point(347, 70);
            this.ThresholdDarkPixelsPerFrameAsPercentageSetupEnd.Name = "ThresholdDarkPixelsPerFrameAsPercentageSetupEnd";
            this.ThresholdDarkPixelsPerFrameAsPercentageSetupEnd.Size = new System.Drawing.Size(47, 25);
            this.ThresholdDarkPixelsPerFrameAsPercentageSetupEnd.TabIndex = 29;
            this.ThresholdDarkPixelsPerFrameAsPercentageSetupEnd.SelectedIndexChanged += new System.EventHandler(this.ThresholdSetupValuesChanged);
            // 
            // ThresholdDarkPixelsPerFrameAsPercentageSetupStart
            // 
            this.ThresholdDarkPixelsPerFrameAsPercentageSetupStart.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ThresholdDarkPixelsPerFrameAsPercentageSetupStart.Font = new System.Drawing.Font("Calibri", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ThresholdDarkPixelsPerFrameAsPercentageSetupStart.FormattingEnabled = true;
            this.ThresholdDarkPixelsPerFrameAsPercentageSetupStart.Location = new System.Drawing.Point(273, 70);
            this.ThresholdDarkPixelsPerFrameAsPercentageSetupStart.Name = "ThresholdDarkPixelsPerFrameAsPercentageSetupStart";
            this.ThresholdDarkPixelsPerFrameAsPercentageSetupStart.Size = new System.Drawing.Size(47, 25);
            this.ThresholdDarkPixelsPerFrameAsPercentageSetupStart.TabIndex = 28;
            this.ThresholdDarkPixelsPerFrameAsPercentageSetupStart.SelectedIndexChanged += new System.EventHandler(this.ThresholdSetupValuesChanged);
            // 
            // ThresholdIndividualPixelBrightnessSetupIncrement
            // 
            this.ThresholdIndividualPixelBrightnessSetupIncrement.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ThresholdIndividualPixelBrightnessSetupIncrement.Font = new System.Drawing.Font("Calibri", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ThresholdIndividualPixelBrightnessSetupIncrement.FormattingEnabled = true;
            this.ThresholdIndividualPixelBrightnessSetupIncrement.Location = new System.Drawing.Point(433, 32);
            this.ThresholdIndividualPixelBrightnessSetupIncrement.Name = "ThresholdIndividualPixelBrightnessSetupIncrement";
            this.ThresholdIndividualPixelBrightnessSetupIncrement.Size = new System.Drawing.Size(47, 25);
            this.ThresholdIndividualPixelBrightnessSetupIncrement.TabIndex = 27;
            this.ThresholdIndividualPixelBrightnessSetupIncrement.SelectedIndexChanged += new System.EventHandler(this.ThresholdSetupValuesChanged);
            // 
            // ThresholdIndividualPixelBrightnessSetupEnd
            // 
            this.ThresholdIndividualPixelBrightnessSetupEnd.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ThresholdIndividualPixelBrightnessSetupEnd.Font = new System.Drawing.Font("Calibri", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ThresholdIndividualPixelBrightnessSetupEnd.FormattingEnabled = true;
            this.ThresholdIndividualPixelBrightnessSetupEnd.Location = new System.Drawing.Point(347, 32);
            this.ThresholdIndividualPixelBrightnessSetupEnd.Name = "ThresholdIndividualPixelBrightnessSetupEnd";
            this.ThresholdIndividualPixelBrightnessSetupEnd.Size = new System.Drawing.Size(47, 25);
            this.ThresholdIndividualPixelBrightnessSetupEnd.TabIndex = 24;
            this.ThresholdIndividualPixelBrightnessSetupEnd.SelectedIndexChanged += new System.EventHandler(this.ThresholdSetupValuesChanged);
            // 
            // ThresholdIndividualPixelBrightnessSetupStart
            // 
            this.ThresholdIndividualPixelBrightnessSetupStart.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ThresholdIndividualPixelBrightnessSetupStart.Font = new System.Drawing.Font("Calibri", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ThresholdIndividualPixelBrightnessSetupStart.FormattingEnabled = true;
            this.ThresholdIndividualPixelBrightnessSetupStart.Location = new System.Drawing.Point(273, 32);
            this.ThresholdIndividualPixelBrightnessSetupStart.Name = "ThresholdIndividualPixelBrightnessSetupStart";
            this.ThresholdIndividualPixelBrightnessSetupStart.Size = new System.Drawing.Size(47, 25);
            this.ThresholdIndividualPixelBrightnessSetupStart.TabIndex = 14;
            this.ThresholdIndividualPixelBrightnessSetupStart.SelectedIndexChanged += new System.EventHandler(this.ThresholdSetupValuesChanged);
            // 
            // ResetButton
            // 
            this.ResetButton.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ResetButton.Location = new System.Drawing.Point(259, 10);
            this.ResetButton.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.ResetButton.Name = "ResetButton";
            this.ResetButton.Size = new System.Drawing.Size(115, 32);
            this.ResetButton.TabIndex = 120;
            this.ResetButton.Text = "&Reset";
            this.ResetButton.UseVisualStyleBackColor = true;
            this.ResetButton.Click += new System.EventHandler(this.ResetButton_Click);
            // 
            // ProgressTimer
            // 
            this.ProgressTimer.Interval = 2000;
            this.ProgressTimer.Tick += new System.EventHandler(this.ProgressTimer_Tick);
            // 
            // TestButton
            // 
            this.TestButton.Location = new System.Drawing.Point(482, 570);
            this.TestButton.Name = "TestButton";
            this.TestButton.Size = new System.Drawing.Size(107, 47);
            this.TestButton.TabIndex = 121;
            this.TestButton.Text = "Test";
            this.TestButton.UseVisualStyleBackColor = true;
            this.TestButton.Visible = false;
            this.TestButton.Click += new System.EventHandler(this.TestButton_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.TuningResultsDirectoryBrowseButton);
            this.groupBox3.Controls.Add(this.TuningResultsDirectoryTextBox);
            this.groupBox3.Font = new System.Drawing.Font("Calibri", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox3.Location = new System.Drawing.Point(20, 56);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(574, 70);
            this.groupBox3.TabIndex = 122;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Log Directory";
            // 
            // TuningResultsDirectoryBrowseButton
            // 
            this.TuningResultsDirectoryBrowseButton.Font = new System.Drawing.Font("Calibri", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TuningResultsDirectoryBrowseButton.Location = new System.Drawing.Point(519, 27);
            this.TuningResultsDirectoryBrowseButton.Margin = new System.Windows.Forms.Padding(2);
            this.TuningResultsDirectoryBrowseButton.Name = "TuningResultsDirectoryBrowseButton";
            this.TuningResultsDirectoryBrowseButton.Size = new System.Drawing.Size(35, 28);
            this.TuningResultsDirectoryBrowseButton.TabIndex = 4;
            this.TuningResultsDirectoryBrowseButton.Text = "...";
            this.TuningResultsDirectoryBrowseButton.UseVisualStyleBackColor = true;
            this.TuningResultsDirectoryBrowseButton.Click += new System.EventHandler(this.TuningResultsDirectoryBrowseButton_Click);
            // 
            // TuningResultsDirectoryTextBox
            // 
            this.TuningResultsDirectoryTextBox.Font = new System.Drawing.Font("Calibri", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TuningResultsDirectoryTextBox.Location = new System.Drawing.Point(23, 27);
            this.TuningResultsDirectoryTextBox.Name = "TuningResultsDirectoryTextBox";
            this.TuningResultsDirectoryTextBox.Size = new System.Drawing.Size(457, 24);
            this.TuningResultsDirectoryTextBox.TabIndex = 0;
            this.TuningResultsDirectoryTextBox.Text = "C:\\";
            this.TuningResultsDirectoryTextBox.Leave += new System.EventHandler(this.TuningResultsDirectoryTextBox_Leave);
            // 
            // TrimButton
            // 
            this.TrimButton.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TrimButton.Location = new System.Drawing.Point(398, 10);
            this.TrimButton.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.TrimButton.Name = "TrimButton";
            this.TrimButton.Size = new System.Drawing.Size(115, 32);
            this.TrimButton.TabIndex = 123;
            this.TrimButton.Text = "&Trim";
            this.TrimButton.UseVisualStyleBackColor = true;
            this.TrimButton.Click += new System.EventHandler(this.TrimButton_Click);
            // 
            // OpenFileDialog
            // 
            this.OpenFileDialog.FileName = "openFileDialog1";
            // 
            // AdjustConcurrentScanCount
            // 
            this.AdjustConcurrentScanCount.Interval = 30000;
            this.AdjustConcurrentScanCount.Tick += new System.EventHandler(this.AdjustConcurrentScanCount_Tick);
            // 
            // TuningForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(662, 592);
            this.Controls.Add(this.TrimButton);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.TestButton);
            this.Controls.Add(this.ResetButton);
            this.Controls.Add(groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.RunButton);
            this.Controls.Add(this.CancelTestButton);
            this.Font = new System.Drawing.Font("Calibri", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "TuningForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Tune Threshold Parameters";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.TuningForm_FormClosing);
            this.Load += new System.EventHandler(this.TuningForm_Load);
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button CancelTestButton;
        private System.Windows.Forms.Button RunButton;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label PassProgressLabelValue;
        private System.Windows.Forms.Label CompletedPassesLabelValue;
        private System.Windows.Forms.Label PassProgressLabel;
        private System.Windows.Forms.Button ResetButton;
        private System.Windows.Forms.Timer ProgressTimer;
        private System.Windows.Forms.Label TimeRemainingValueLabel;
        private System.Windows.Forms.Button TestButton;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox TuningResultsDirectoryTextBox;
        private System.Windows.Forms.Button TuningResultsDirectoryBrowseButton;
        private System.Windows.Forms.FolderBrowserDialog TuningResultsBrowserDialog;
        private System.Windows.Forms.Button TrimButton;
        private System.Windows.Forms.OpenFileDialog OpenFileDialog;
        private System.Windows.Forms.SaveFileDialog SaveFileDialog;
        private System.Windows.Forms.Label ConcurrentScansRunningLabelValue;
        private System.Windows.Forms.Timer AdjustConcurrentScanCount;
        private System.Windows.Forms.ComboBox ThresholdIndividualPixelBrightnessSetupStart;
        private System.Windows.Forms.Label CurrentSecondsSkipLabelValue;
        private System.Windows.Forms.Label CurrentIndividualPixelBrightnessThresholdLabelValue;
        private System.Windows.Forms.Label CurrentDarknessThresholdPercentageLabelValue;
        private System.Windows.Forms.ComboBox ThresholdIndividualPixelBrightnessSetupEnd;
        private System.Windows.Forms.ComboBox ThresholdIndividualPixelBrightnessSetupIncrement;
        private System.Windows.Forms.ComboBox ThresholdSecondsSkipIncrement;
        private System.Windows.Forms.ComboBox ThresholdSecondsSkipSetupEnd;
        private System.Windows.Forms.ComboBox ThresholdSecondsSkipSetupStart;
        private System.Windows.Forms.ComboBox ThresholdDarkPixelsPerFrameAsPercentageSetupIncrement;
        private System.Windows.Forms.ComboBox ThresholdDarkPixelsPerFrameAsPercentageSetupEnd;
        private System.Windows.Forms.ComboBox ThresholdDarkPixelsPerFrameAsPercentageSetupStart;
        private System.Windows.Forms.ComboBox ThresholdPixelScanPercentageSetupIncrement;
        private System.Windows.Forms.ComboBox ThresholdPixelScanPercentageSetupEnd;
        private System.Windows.Forms.ComboBox ThresholdPixelScanPercentageSetupStart;
        private System.Windows.Forms.Label CurrentPixelScanPercentageLabelValue;
        private System.Windows.Forms.Label CurrentConsecutiveDarkFramesInSecondsLabelValue;
        private System.Windows.Forms.ComboBox ThresholdConsecutiveDarkFramesInSecondsIncrement;
        private System.Windows.Forms.ComboBox ThresholdConsecutiveDarkFramesInSecondsEnd;
        private System.Windows.Forms.ComboBox ThresholdConsecutiveDarkFramesInSecondsStart;

    }
}


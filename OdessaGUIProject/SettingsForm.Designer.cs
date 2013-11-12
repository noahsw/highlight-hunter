namespace OdessaGUIProject
{
    sealed partial class SettingsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsForm));
            this.label1 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.helpAdvancedOptionsButton = new System.Windows.Forms.PictureBox();
            this.helpScanSensitivity = new System.Windows.Forms.PictureBox();
            this.helpHighlightLength = new System.Windows.Forms.PictureBox();
            this.activateLinkLabel = new System.Windows.Forms.LinkLabel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pictureBox6 = new System.Windows.Forms.PictureBox();
            this.pictureBox7 = new System.Windows.Forms.PictureBox();
            this.pictureBox8 = new System.Windows.Forms.PictureBox();
            this.pictureBox9 = new System.Windows.Forms.PictureBox();
            this.okPictureButtonControl = new OdessaGUIProject.UI_Controls.PictureButtonControl();
            this.proGlyphPictureBox = new System.Windows.Forms.PictureBox();
            this.label4 = new System.Windows.Forms.Label();
            this.currentLicenseLabel = new System.Windows.Forms.Label();
            this.helpLicenseButton = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.highlightDurationMinutesTextBox = new System.Windows.Forms.TextBox();
            this.sensitivitySliderControl = new OdessaGUIProject.UI_Controls.SensitivitySliderControl();
            this.saveAsProRes = new OdessaGUIProject.UI_Controls.PictureCheckBoxControl();
            this.ignoreEarlyHighlights = new OdessaGUIProject.UI_Controls.PictureCheckBoxControl();
            this.highlightDurationSecondsTextBox = new System.Windows.Forms.TextBox();
            this.pictureBox4 = new System.Windows.Forms.PictureBox();
            this.pictureBox5 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.helpAdvancedOptionsButton)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.helpScanSensitivity)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.helpHighlightLength)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox8)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox9)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.proGlyphPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.helpLicenseButton)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox5)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(153)))), ((int)(((byte)(153)))));
            this.label1.Location = new System.Drawing.Point(63, 82);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 17);
            this.label1.TabIndex = 1;
            this.label1.Text = "minutes";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(153)))), ((int)(((byte)(153)))));
            this.label5.Location = new System.Drawing.Point(188, 82);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(56, 17);
            this.label5.TabIndex = 3;
            this.label5.Text = "seconds";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(153)))), ((int)(((byte)(153)))));
            this.label2.Location = new System.Drawing.Point(20, 274);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(315, 17);
            this.label2.TabIndex = 5;
            this.label2.Text = "Ignore highlights in the first 10 seconds of recording";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(153)))), ((int)(((byte)(153)))));
            this.label6.Location = new System.Drawing.Point(20, 311);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(272, 17);
            this.label6.TabIndex = 7;
            this.label6.Text = "Convert all saved highlights to ProRes format";
            // 
            // helpAdvancedOptionsButton
            // 
            this.helpAdvancedOptionsButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.helpAdvancedOptionsButton.Image = global::OdessaGUIProject.Properties.Resources.settings_help_icon_small;
            this.helpAdvancedOptionsButton.Location = new System.Drawing.Point(150, 242);
            this.helpAdvancedOptionsButton.Name = "helpAdvancedOptionsButton";
            this.helpAdvancedOptionsButton.Size = new System.Drawing.Size(12, 13);
            this.helpAdvancedOptionsButton.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.helpAdvancedOptionsButton.TabIndex = 48;
            this.helpAdvancedOptionsButton.TabStop = false;
            this.helpAdvancedOptionsButton.Click += new System.EventHandler(this.helpAdvancedOptionsButton_Click);
            // 
            // helpScanSensitivity
            // 
            this.helpScanSensitivity.Cursor = System.Windows.Forms.Cursors.Hand;
            this.helpScanSensitivity.Image = global::OdessaGUIProject.Properties.Resources.settings_help_icon_small;
            this.helpScanSensitivity.Location = new System.Drawing.Point(137, 147);
            this.helpScanSensitivity.Name = "helpScanSensitivity";
            this.helpScanSensitivity.Size = new System.Drawing.Size(12, 13);
            this.helpScanSensitivity.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.helpScanSensitivity.TabIndex = 49;
            this.helpScanSensitivity.TabStop = false;
            this.helpScanSensitivity.Click += new System.EventHandler(this.helpScanSensitivity_Click);
            // 
            // helpHighlightLength
            // 
            this.helpHighlightLength.Cursor = System.Windows.Forms.Cursors.Hand;
            this.helpHighlightLength.Image = global::OdessaGUIProject.Properties.Resources.settings_help_icon_small;
            this.helpHighlightLength.Location = new System.Drawing.Point(196, 48);
            this.helpHighlightLength.Name = "helpHighlightLength";
            this.helpHighlightLength.Size = new System.Drawing.Size(12, 13);
            this.helpHighlightLength.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.helpHighlightLength.TabIndex = 50;
            this.helpHighlightLength.TabStop = false;
            this.helpHighlightLength.Click += new System.EventHandler(this.helpHighlightLength_Click);
            // 
            // activateLinkLabel
            // 
            this.activateLinkLabel.ActiveLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(153)))), ((int)(((byte)(204)))));
            this.activateLinkLabel.BackColor = System.Drawing.Color.Transparent;
            this.activateLinkLabel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.activateLinkLabel.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.activateLinkLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(153)))), ((int)(((byte)(153)))));
            this.activateLinkLabel.LinkArea = new System.Windows.Forms.LinkArea(96, 36);
            this.activateLinkLabel.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.activateLinkLabel.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(153)))), ((int)(((byte)(204)))));
            this.activateLinkLabel.Location = new System.Drawing.Point(20, 424);
            this.activateLinkLabel.Name = "activateLinkLabel";
            this.activateLinkLabel.Size = new System.Drawing.Size(400, 57);
            this.activateLinkLabel.TabIndex = 13;
            this.activateLinkLabel.TabStop = true;
            this.activateLinkLabel.Text = "To unlock professional features like saving without watermarks and exporting to v" +
    "ideo software, upgrade to Highlight Hunter Pro now!";
            this.activateLinkLabel.UseCompatibleTextRendering = true;
            this.activateLinkLabel.VisitedLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(153)))), ((int)(((byte)(204)))));
            this.activateLinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.activateLinkLabel_LinkClicked);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImage = global::OdessaGUIProject.Properties.Resources.settings_2px_section_divider;
            this.pictureBox1.Location = new System.Drawing.Point(0, 124);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(440, 2);
            this.pictureBox1.TabIndex = 58;
            this.pictureBox1.TabStop = false;
            // 
            // pictureBox6
            // 
            this.pictureBox6.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox6.Image = global::OdessaGUIProject.Properties.Resources.settings_sectiontitle_license;
            this.pictureBox6.Location = new System.Drawing.Point(20, 372);
            this.pictureBox6.Name = "pictureBox6";
            this.pictureBox6.Size = new System.Drawing.Size(51, 13);
            this.pictureBox6.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox6.TabIndex = 61;
            this.pictureBox6.TabStop = false;
            // 
            // pictureBox7
            // 
            this.pictureBox7.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox7.Image = global::OdessaGUIProject.Properties.Resources.settings_sectiontitle_advancedoptions;
            this.pictureBox7.Location = new System.Drawing.Point(20, 242);
            this.pictureBox7.Name = "pictureBox7";
            this.pictureBox7.Size = new System.Drawing.Size(123, 15);
            this.pictureBox7.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox7.TabIndex = 62;
            this.pictureBox7.TabStop = false;
            // 
            // pictureBox8
            // 
            this.pictureBox8.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox8.Image = global::OdessaGUIProject.Properties.Resources.settings_sectiontitle_scansensitivity;
            this.pictureBox8.Location = new System.Drawing.Point(20, 147);
            this.pictureBox8.Name = "pictureBox8";
            this.pictureBox8.Size = new System.Drawing.Size(110, 17);
            this.pictureBox8.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox8.TabIndex = 63;
            this.pictureBox8.TabStop = false;
            // 
            // pictureBox9
            // 
            this.pictureBox9.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox9.Image = global::OdessaGUIProject.Properties.Resources.settings_sectiontitle_highlightlength;
            this.pictureBox9.Location = new System.Drawing.Point(20, 47);
            this.pictureBox9.Name = "pictureBox9";
            this.pictureBox9.Size = new System.Drawing.Size(171, 17);
            this.pictureBox9.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox9.TabIndex = 64;
            this.pictureBox9.TabStop = false;
            // 
            // okPictureButtonControl
            // 
            this.okPictureButtonControl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.okPictureButtonControl.AutoSize = true;
            this.okPictureButtonControl.DefaultImage = global::OdessaGUIProject.Properties.Resources.button_short_green_ok_normal;
            this.okPictureButtonControl.HoverImage = global::OdessaGUIProject.Properties.Resources.button_short_green_ok_hover;
            this.okPictureButtonControl.Location = new System.Drawing.Point(311, 484);
            this.okPictureButtonControl.MouseDownImage = global::OdessaGUIProject.Properties.Resources.button_short_green_ok_click;
            this.okPictureButtonControl.Name = "okPictureButtonControl";
            this.okPictureButtonControl.Size = new System.Drawing.Size(112, 44);
            this.okPictureButtonControl.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.okPictureButtonControl.TabIndex = 14;
            this.okPictureButtonControl.Tooltip = null;
            this.okPictureButtonControl.Click += new System.EventHandler(this.okPictureButtonControl_Click);
            // 
            // proGlyphPictureBox
            // 
            this.proGlyphPictureBox.BackColor = System.Drawing.Color.Transparent;
            this.proGlyphPictureBox.Image = global::OdessaGUIProject.Properties.Resources.icons_proglyph;
            this.proGlyphPictureBox.Location = new System.Drawing.Point(295, 311);
            this.proGlyphPictureBox.Name = "proGlyphPictureBox";
            this.proGlyphPictureBox.Size = new System.Drawing.Size(24, 17);
            this.proGlyphPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.proGlyphPictureBox.TabIndex = 66;
            this.proGlyphPictureBox.TabStop = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(153)))), ((int)(((byte)(153)))));
            this.label4.Location = new System.Drawing.Point(20, 400);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(149, 17);
            this.label4.TabIndex = 11;
            this.label4.Text = "You are currently using: ";
            // 
            // currentLicenseLabel
            // 
            this.currentLicenseLabel.AutoSize = true;
            this.currentLicenseLabel.BackColor = System.Drawing.Color.Transparent;
            this.currentLicenseLabel.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.currentLicenseLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(153)))), ((int)(((byte)(153)))));
            this.currentLicenseLabel.Location = new System.Drawing.Point(165, 400);
            this.currentLicenseLabel.Name = "currentLicenseLabel";
            this.currentLicenseLabel.Size = new System.Drawing.Size(144, 17);
            this.currentLicenseLabel.TabIndex = 12;
            this.currentLicenseLabel.Text = "Highlight Hunter Free";
            // 
            // helpLicenseButton
            // 
            this.helpLicenseButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.helpLicenseButton.Image = global::OdessaGUIProject.Properties.Resources.settings_help_icon_small;
            this.helpLicenseButton.Location = new System.Drawing.Point(78, 372);
            this.helpLicenseButton.Name = "helpLicenseButton";
            this.helpLicenseButton.Size = new System.Drawing.Size(12, 13);
            this.helpLicenseButton.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.helpLicenseButton.TabIndex = 69;
            this.helpLicenseButton.TabStop = false;
            this.helpLicenseButton.Click += new System.EventHandler(this.helpLicenseButton_Click);
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackgroundImage = global::OdessaGUIProject.Properties.Resources.settings_2px_section_divider;
            this.pictureBox2.Location = new System.Drawing.Point(0, 221);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(440, 2);
            this.pictureBox2.TabIndex = 70;
            this.pictureBox2.TabStop = false;
            // 
            // pictureBox3
            // 
            this.pictureBox3.BackgroundImage = global::OdessaGUIProject.Properties.Resources.settings_2px_section_divider;
            this.pictureBox3.Location = new System.Drawing.Point(0, 349);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(440, 2);
            this.pictureBox3.TabIndex = 71;
            this.pictureBox3.TabStop = false;
            // 
            // highlightDurationMinutesTextBox
            // 
            this.highlightDurationMinutesTextBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(26)))), ((int)(((byte)(26)))));
            this.highlightDurationMinutesTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.highlightDurationMinutesTextBox.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.highlightDurationMinutesTextBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.highlightDurationMinutesTextBox.Location = new System.Drawing.Point(29, 76);
            this.highlightDurationMinutesTextBox.MaxLength = 2;
            this.highlightDurationMinutesTextBox.Name = "highlightDurationMinutesTextBox";
            this.highlightDurationMinutesTextBox.Size = new System.Drawing.Size(25, 26);
            this.highlightDurationMinutesTextBox.TabIndex = 0;
            this.highlightDurationMinutesTextBox.Text = "00";
            this.highlightDurationMinutesTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.highlightDurationMinutesTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.highlightDurationMinutesTextBox_KeyPress);
            // 
            // sensitivitySliderControl
            // 
            this.sensitivitySliderControl.AutoSize = true;
            this.sensitivitySliderControl.BackColor = System.Drawing.Color.Transparent;
            this.sensitivitySliderControl.Location = new System.Drawing.Point(20, 172);
            this.sensitivitySliderControl.Name = "sensitivitySliderControl";
            this.sensitivitySliderControl.Size = new System.Drawing.Size(403, 47);
            this.sensitivitySliderControl.TabIndex = 4;
            // 
            // saveAsProRes
            // 
            this.saveAsProRes.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("saveAsProRes.BackgroundImage")));
            this.saveAsProRes.Checked = false;
            this.saveAsProRes.Cursor = System.Windows.Forms.Cursors.Hand;
            this.saveAsProRes.Location = new System.Drawing.Point(362, 311);
            this.saveAsProRes.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.saveAsProRes.Name = "saveAsProRes";
            this.saveAsProRes.Size = new System.Drawing.Size(51, 21);
            this.saveAsProRes.TabIndex = 8;
            this.saveAsProRes.CheckedChanged += new System.EventHandler(this.saveAsProRes_CheckedChanged);
            // 
            // ignoreEarlyHighlights
            // 
            this.ignoreEarlyHighlights.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ignoreEarlyHighlights.BackgroundImage")));
            this.ignoreEarlyHighlights.Checked = false;
            this.ignoreEarlyHighlights.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ignoreEarlyHighlights.Location = new System.Drawing.Point(364, 274);
            this.ignoreEarlyHighlights.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ignoreEarlyHighlights.Name = "ignoreEarlyHighlights";
            this.ignoreEarlyHighlights.Size = new System.Drawing.Size(51, 21);
            this.ignoreEarlyHighlights.TabIndex = 6;
            // 
            // highlightDurationSecondsTextBox
            // 
            this.highlightDurationSecondsTextBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(26)))), ((int)(((byte)(26)))));
            this.highlightDurationSecondsTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.highlightDurationSecondsTextBox.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.highlightDurationSecondsTextBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.highlightDurationSecondsTextBox.Location = new System.Drawing.Point(154, 76);
            this.highlightDurationSecondsTextBox.MaxLength = 2;
            this.highlightDurationSecondsTextBox.Name = "highlightDurationSecondsTextBox";
            this.highlightDurationSecondsTextBox.Size = new System.Drawing.Size(25, 26);
            this.highlightDurationSecondsTextBox.TabIndex = 2;
            this.highlightDurationSecondsTextBox.Text = "15";
            this.highlightDurationSecondsTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.highlightDurationSecondsTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.highlightDurationSecondsTextBox_KeyPress);
            // 
            // pictureBox4
            // 
            this.pictureBox4.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox4.Image = global::OdessaGUIProject.Properties.Resources.settings_duration_textbox;
            this.pictureBox4.Location = new System.Drawing.Point(20, 73);
            this.pictureBox4.Name = "pictureBox4";
            this.pictureBox4.Size = new System.Drawing.Size(42, 33);
            this.pictureBox4.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox4.TabIndex = 72;
            this.pictureBox4.TabStop = false;
            // 
            // pictureBox5
            // 
            this.pictureBox5.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox5.Image = global::OdessaGUIProject.Properties.Resources.settings_duration_textbox;
            this.pictureBox5.Location = new System.Drawing.Point(145, 73);
            this.pictureBox5.Name = "pictureBox5";
            this.pictureBox5.Size = new System.Drawing.Size(42, 33);
            this.pictureBox5.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox5.TabIndex = 73;
            this.pictureBox5.TabStop = false;
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.BackgroundImage = global::OdessaGUIProject.Properties.Resources.background_pattern_102x102_tile;
            this.ClientSize = new System.Drawing.Size(440, 540);
            this.Controls.Add(this.highlightDurationSecondsTextBox);
            this.Controls.Add(this.highlightDurationMinutesTextBox);
            this.Controls.Add(this.pictureBox3);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.helpLicenseButton);
            this.Controls.Add(this.currentLicenseLabel);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.proGlyphPictureBox);
            this.Controls.Add(this.okPictureButtonControl);
            this.Controls.Add(this.pictureBox9);
            this.Controls.Add(this.pictureBox8);
            this.Controls.Add(this.pictureBox7);
            this.Controls.Add(this.pictureBox6);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.activateLinkLabel);
            this.Controls.Add(this.sensitivitySliderControl);
            this.Controls.Add(this.helpHighlightLength);
            this.Controls.Add(this.helpScanSensitivity);
            this.Controls.Add(this.helpAdvancedOptionsButton);
            this.Controls.Add(this.saveAsProRes);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.ignoreEarlyHighlights);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pictureBox4);
            this.Controls.Add(this.pictureBox5);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SettingsForm";
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Settings";
            this.Load += new System.EventHandler(this.Settings_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.SettingsForm_Paint);
            this.Resize += new System.EventHandler(this.SettingsForm_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.helpAdvancedOptionsButton)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.helpScanSensitivity)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.helpHighlightLength)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox8)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox9)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.proGlyphPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.helpLicenseButton)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox5)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label2;
        private UI_Controls.PictureCheckBoxControl ignoreEarlyHighlights;
        private System.Windows.Forms.Label label6;
        private UI_Controls.PictureCheckBoxControl saveAsProRes;
        private System.Windows.Forms.PictureBox helpAdvancedOptionsButton;
        private System.Windows.Forms.PictureBox helpScanSensitivity;
        private System.Windows.Forms.PictureBox helpHighlightLength;
        private UI_Controls.SensitivitySliderControl sensitivitySliderControl;
        private System.Windows.Forms.LinkLabel activateLinkLabel;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureBox6;
        private System.Windows.Forms.PictureBox pictureBox7;
        private System.Windows.Forms.PictureBox pictureBox8;
        private System.Windows.Forms.PictureBox pictureBox9;
        private UI_Controls.PictureButtonControl okPictureButtonControl;
        private System.Windows.Forms.PictureBox proGlyphPictureBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label currentLicenseLabel;
        private System.Windows.Forms.PictureBox helpLicenseButton;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.PictureBox pictureBox3;
        private System.Windows.Forms.TextBox highlightDurationMinutesTextBox;
        private System.Windows.Forms.TextBox highlightDurationSecondsTextBox;
        private System.Windows.Forms.PictureBox pictureBox4;
        private System.Windows.Forms.PictureBox pictureBox5;
    }
}
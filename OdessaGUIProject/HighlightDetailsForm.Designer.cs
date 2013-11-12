namespace OdessaGUIProject
{
    partial class HighlightDetailsForm
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.Panel rightPanel;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HighlightDetailsForm));
            this.shareButtonTutorialBubble = new OdessaGUIProject.UI_Controls.TutorialBubble();
            this.installQuickTimeLinkLabel = new System.Windows.Forms.LinkLabel();
            this.removeHighlightPictureButtonControl = new OdessaGUIProject.UI_Controls.PictureButtonControl();
            this.titleTextBox = new System.Windows.Forms.TextBox();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.nextHighlightPictureButtonControl = new OdessaGUIProject.UI_Controls.PictureButtonControl();
            this.previousHighlightPictureButtonControl = new OdessaGUIProject.UI_Controls.PictureButtonControl();
            this.saveToDiskButton = new OdessaGUIProject.UI_Controls.PublishButtonControl();
            this.shareToFacebookButton = new OdessaGUIProject.UI_Controls.PublishButtonControl();
            this.tickBoxLocationsTimer = new System.Windows.Forms.Timer(this.components);
            this.zoomOutStartTimer = new System.Windows.Forms.Timer(this.components);
            this.zoomOutEndTimer = new System.Windows.Forms.Timer(this.components);
            this.highlightDurationLabel = new System.Windows.Forms.Label();
            this.playHeadBox = new System.Windows.Forms.PictureBox();
            this.highlightTimelinePictureBox = new System.Windows.Forms.PictureBox();
            this.startTickBox = new System.Windows.Forms.PictureBox();
            this.endTickBox = new System.Windows.Forms.PictureBox();
            this.timelineBox = new System.Windows.Forms.PictureBox();
            this.timelineLeftEndPictureBox = new System.Windows.Forms.PictureBox();
            this.playStatePicture = new System.Windows.Forms.PictureBox();
            this.timelineRightEndPictureBox = new System.Windows.Forms.PictureBox();
            this.bookmarkLocationPictureBox = new System.Windows.Forms.PictureBox();
            this.donateContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.donateFalsePositiveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.donateIntentionalMarkToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.donateMissedMarkToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.verticalDividerPictureBox = new System.Windows.Forms.PictureBox();
            this.videoPlayer = new OdessaGUIProject.Players.GenericPlayerControl();
            this.bookmarkFlagTutorialBubble = new OdessaGUIProject.UI_Controls.TutorialBubble();
            this.handlesTutorialBubble = new OdessaGUIProject.UI_Controls.TutorialBubble();
            this.closeDetailsTutorialBubble = new OdessaGUIProject.UI_Controls.TutorialBubble();
            rightPanel = new System.Windows.Forms.Panel();
            rightPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.playHeadBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.highlightTimelinePictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.startTickBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.endTickBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.timelineBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.timelineLeftEndPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.playStatePicture)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.timelineRightEndPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bookmarkLocationPictureBox)).BeginInit();
            this.donateContextMenuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.verticalDividerPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // rightPanel
            // 
            rightPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            rightPanel.BackColor = System.Drawing.Color.Transparent;
            rightPanel.Controls.Add(this.shareButtonTutorialBubble);
            rightPanel.Controls.Add(this.installQuickTimeLinkLabel);
            rightPanel.Controls.Add(this.removeHighlightPictureButtonControl);
            rightPanel.Controls.Add(this.titleTextBox);
            rightPanel.Controls.Add(this.pictureBox3);
            rightPanel.Controls.Add(this.pictureBox2);
            rightPanel.Controls.Add(this.pictureBox1);
            rightPanel.Controls.Add(this.nextHighlightPictureButtonControl);
            rightPanel.Controls.Add(this.previousHighlightPictureButtonControl);
            rightPanel.Controls.Add(this.saveToDiskButton);
            rightPanel.Controls.Add(this.shareToFacebookButton);
            rightPanel.Location = new System.Drawing.Point(799, 50);
            rightPanel.Name = "rightPanel";
            rightPanel.Size = new System.Drawing.Size(373, 500);
            rightPanel.TabIndex = 1;
            rightPanel.MouseClick += new System.Windows.Forms.MouseEventHandler(this.rightPanel_MouseClick);
            // 
            // shareButtonTutorialBubble
            // 
            this.shareButtonTutorialBubble.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.shareButtonTutorialBubble.BackColor = System.Drawing.Color.Transparent;
            this.shareButtonTutorialBubble.Font = new System.Drawing.Font("Open Sans", 8.25F);
            this.shareButtonTutorialBubble.Location = new System.Drawing.Point(87, 54);
            this.shareButtonTutorialBubble.Name = "shareButtonTutorialBubble";
            this.shareButtonTutorialBubble.Size = new System.Drawing.Size(193, 144);
            this.shareButtonTutorialBubble.TabIndex = 117;
            this.shareButtonTutorialBubble.TutorialProgress = OdessaGUIProject.UI_Helpers.TutorialProgress.TutorialShareButton;
            this.shareButtonTutorialBubble.Visible = false;
            // 
            // installQuickTimeLinkLabel
            // 
            this.installQuickTimeLinkLabel.ActiveLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(153)))), ((int)(((byte)(204)))));
            this.installQuickTimeLinkLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.installQuickTimeLinkLabel.AutoSize = true;
            this.installQuickTimeLinkLabel.BackColor = System.Drawing.Color.Transparent;
            this.installQuickTimeLinkLabel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.installQuickTimeLinkLabel.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.installQuickTimeLinkLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(153)))), ((int)(((byte)(153)))));
            this.installQuickTimeLinkLabel.LinkArea = new System.Windows.Forms.LinkArea(0, 33);
            this.installQuickTimeLinkLabel.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.installQuickTimeLinkLabel.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(153)))), ((int)(((byte)(204)))));
            this.installQuickTimeLinkLabel.Location = new System.Drawing.Point(140, 481);
            this.installQuickTimeLinkLabel.Name = "installQuickTimeLinkLabel";
            this.installQuickTimeLinkLabel.Size = new System.Drawing.Size(220, 17);
            this.installQuickTimeLinkLabel.TabIndex = 93;
            this.installQuickTimeLinkLabel.TabStop = true;
            this.installQuickTimeLinkLabel.Text = "We recommend installing QuickTime";
            this.installQuickTimeLinkLabel.Visible = false;
            this.installQuickTimeLinkLabel.VisitedLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(153)))), ((int)(((byte)(204)))));
            this.installQuickTimeLinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.installQuickTimeLinkLabel_LinkClicked);
            // 
            // removeHighlightPictureButtonControl
            // 
            this.removeHighlightPictureButtonControl.AutoSize = true;
            this.removeHighlightPictureButtonControl.DefaultImage = global::OdessaGUIProject.Properties.Resources.editpanel_button_videoselect_delete;
            this.removeHighlightPictureButtonControl.HoverImage = global::OdessaGUIProject.Properties.Resources.editpanel_button_videoselect_delete_hover;
            this.removeHighlightPictureButtonControl.Location = new System.Drawing.Point(153, 12);
            this.removeHighlightPictureButtonControl.MouseDownImage = null;
            this.removeHighlightPictureButtonControl.Name = "removeHighlightPictureButtonControl";
            this.removeHighlightPictureButtonControl.Size = new System.Drawing.Size(58, 52);
            this.removeHighlightPictureButtonControl.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Normal;
            this.removeHighlightPictureButtonControl.TabIndex = 1;
            this.removeHighlightPictureButtonControl.Tooltip = "Remove highlight";
            this.removeHighlightPictureButtonControl.Click += new System.EventHandler(this.removeHighlightPictureButtonControl_Click);
            // 
            // titleTextBox
            // 
            this.titleTextBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(26)))), ((int)(((byte)(26)))));
            this.titleTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.titleTextBox.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.titleTextBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.titleTextBox.Location = new System.Drawing.Point(19, 120);
            this.titleTextBox.Name = "titleTextBox";
            this.titleTextBox.Size = new System.Drawing.Size(331, 26);
            this.titleTextBox.TabIndex = 3;
            this.titleTextBox.TextChanged += new System.EventHandler(this.titleTextBox_TextChanged);
            this.titleTextBox.Leave += new System.EventHandler(this.titleTextBox_Leave);
            // 
            // pictureBox3
            // 
            this.pictureBox3.Image = global::OdessaGUIProject.Properties.Resources.editpanel_field_background_videotitle;
            this.pictureBox3.Location = new System.Drawing.Point(8, 111);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(352, 43);
            this.pictureBox3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox3.TabIndex = 92;
            this.pictureBox3.TabStop = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = global::OdessaGUIProject.Properties.Resources.editpanel_smallheading_sharesave;
            this.pictureBox2.Location = new System.Drawing.Point(8, 183);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(73, 10);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox2.TabIndex = 91;
            this.pictureBox2.TabStop = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::OdessaGUIProject.Properties.Resources.editpanel_smallheading_title;
            this.pictureBox1.Location = new System.Drawing.Point(8, 95);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(28, 9);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 90;
            this.pictureBox1.TabStop = false;
            // 
            // nextHighlightPictureButtonControl
            // 
            this.nextHighlightPictureButtonControl.AutoSize = true;
            this.nextHighlightPictureButtonControl.DefaultImage = global::OdessaGUIProject.Properties.Resources.editpanel_button_videoselect_right;
            this.nextHighlightPictureButtonControl.HoverImage = global::OdessaGUIProject.Properties.Resources.editpanel_button_videoselect_right_hover;
            this.nextHighlightPictureButtonControl.Location = new System.Drawing.Point(211, 12);
            this.nextHighlightPictureButtonControl.MouseDownImage = null;
            this.nextHighlightPictureButtonControl.Name = "nextHighlightPictureButtonControl";
            this.nextHighlightPictureButtonControl.Size = new System.Drawing.Size(148, 52);
            this.nextHighlightPictureButtonControl.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Normal;
            this.nextHighlightPictureButtonControl.TabIndex = 2;
            this.nextHighlightPictureButtonControl.Tooltip = "Next highlight";
            this.nextHighlightPictureButtonControl.Click += new System.EventHandler(this.nextHighlightPictureBox_Click);
            // 
            // previousHighlightPictureButtonControl
            // 
            this.previousHighlightPictureButtonControl.AutoSize = true;
            this.previousHighlightPictureButtonControl.DefaultImage = global::OdessaGUIProject.Properties.Resources.editpanel_button_videoselect_left;
            this.previousHighlightPictureButtonControl.HoverImage = global::OdessaGUIProject.Properties.Resources.editpanel_button_videoselect_left_hover;
            this.previousHighlightPictureButtonControl.Location = new System.Drawing.Point(7, 12);
            this.previousHighlightPictureButtonControl.MouseDownImage = null;
            this.previousHighlightPictureButtonControl.Name = "previousHighlightPictureButtonControl";
            this.previousHighlightPictureButtonControl.Size = new System.Drawing.Size(146, 52);
            this.previousHighlightPictureButtonControl.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Normal;
            this.previousHighlightPictureButtonControl.TabIndex = 0;
            this.previousHighlightPictureButtonControl.Tooltip = "Previous highlight";
            this.previousHighlightPictureButtonControl.Click += new System.EventHandler(this.previousHighlightPictureBox_Click);
            // 
            // saveToDiskButton
            // 
            this.saveToDiskButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("saveToDiskButton.BackgroundImage")));
            this.saveToDiskButton.Location = new System.Drawing.Point(8, 240);
            this.saveToDiskButton.Name = "saveToDiskButton";
            this.saveToDiskButton.Size = new System.Drawing.Size(352, 42);
            this.saveToDiskButton.TabIndex = 5;
            // 
            // shareToFacebookButton
            // 
            this.shareToFacebookButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("shareToFacebookButton.BackgroundImage")));
            this.shareToFacebookButton.Location = new System.Drawing.Point(8, 199);
            this.shareToFacebookButton.Name = "shareToFacebookButton";
            this.shareToFacebookButton.Size = new System.Drawing.Size(352, 42);
            this.shareToFacebookButton.TabIndex = 4;
            this.shareToFacebookButton.AdvanceTutorial += new System.EventHandler(this.shareToFacebookButton_AdvanceTutorial);
            // 
            // tickBoxLocationsTimer
            // 
            this.tickBoxLocationsTimer.Interval = 200;
            this.tickBoxLocationsTimer.Tick += new System.EventHandler(this.tickBoxLocationsTimer_Tick);
            // 
            // zoomOutStartTimer
            // 
            this.zoomOutStartTimer.Interval = 500;
            this.zoomOutStartTimer.Tick += new System.EventHandler(this.zoomOutStartTimer_Tick);
            // 
            // zoomOutEndTimer
            // 
            this.zoomOutEndTimer.Interval = 500;
            this.zoomOutEndTimer.Tick += new System.EventHandler(this.zoomOutEndTimer_Tick);
            // 
            // highlightDurationLabel
            // 
            this.highlightDurationLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.highlightDurationLabel.AutoSize = true;
            this.highlightDurationLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(57)))), ((int)(((byte)(57)))));
            this.highlightDurationLabel.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.highlightDurationLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.highlightDurationLabel.Location = new System.Drawing.Point(740, 523);
            this.highlightDurationLabel.Margin = new System.Windows.Forms.Padding(0);
            this.highlightDurationLabel.Name = "highlightDurationLabel";
            this.highlightDurationLabel.Size = new System.Drawing.Size(36, 20);
            this.highlightDurationLabel.TabIndex = 2;
            this.highlightDurationLabel.Text = "0:30";
            this.highlightDurationLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // playHeadBox
            // 
            this.playHeadBox.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.playHeadBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(44)))), ((int)(((byte)(44)))));
            this.playHeadBox.Image = global::OdessaGUIProject.Properties.Resources.editpanel_playbackcontrols_playhead;
            this.playHeadBox.Location = new System.Drawing.Point(226, 513);
            this.playHeadBox.Name = "playHeadBox";
            this.playHeadBox.Size = new System.Drawing.Size(7, 37);
            this.playHeadBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.playHeadBox.TabIndex = 107;
            this.playHeadBox.TabStop = false;
            this.playHeadBox.Visible = false;
            this.playHeadBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.playHeadBox_MouseDown);
            this.playHeadBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.playHeadBox_MouseMove);
            this.playHeadBox.MouseUp += new System.Windows.Forms.MouseEventHandler(this.playHeadBox_MouseUp);
            // 
            // highlightTimelinePictureBox
            // 
            this.highlightTimelinePictureBox.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.highlightTimelinePictureBox.BackgroundImage = global::OdessaGUIProject.Properties.Resources.editpanel_playbackcontrols_timeline_yeshighlight_tile_horizontal;
            this.highlightTimelinePictureBox.Location = new System.Drawing.Point(144, 516);
            this.highlightTimelinePictureBox.Name = "highlightTimelinePictureBox";
            this.highlightTimelinePictureBox.Size = new System.Drawing.Size(151, 32);
            this.highlightTimelinePictureBox.TabIndex = 113;
            this.highlightTimelinePictureBox.TabStop = false;
            this.highlightTimelinePictureBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.timeline_MouseDown);
            this.highlightTimelinePictureBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.timeline_MouseMove);
            this.highlightTimelinePictureBox.MouseUp += new System.Windows.Forms.MouseEventHandler(this.timeline_MouseUp);
            // 
            // startTickBox
            // 
            this.startTickBox.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.startTickBox.Image = global::OdessaGUIProject.Properties.Resources.editpanel_playbackcontrols_handle_left;
            this.startTickBox.Location = new System.Drawing.Point(129, 511);
            this.startTickBox.Name = "startTickBox";
            this.startTickBox.Size = new System.Drawing.Size(15, 43);
            this.startTickBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.startTickBox.TabIndex = 104;
            this.startTickBox.TabStop = false;
            this.startTickBox.Visible = false;
            this.startTickBox.DoubleClick += new System.EventHandler(this.startTickBox_DoubleClick);
            this.startTickBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.trimTickBox_MouseDown);
            this.startTickBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.startTickBox_MouseMove);
            this.startTickBox.MouseUp += new System.Windows.Forms.MouseEventHandler(this.trimTickBox_MouseUp);
            // 
            // endTickBox
            // 
            this.endTickBox.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.endTickBox.Image = global::OdessaGUIProject.Properties.Resources.editpanel_playbackcontrols_handle_right;
            this.endTickBox.Location = new System.Drawing.Point(294, 511);
            this.endTickBox.Name = "endTickBox";
            this.endTickBox.Size = new System.Drawing.Size(15, 43);
            this.endTickBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.endTickBox.TabIndex = 105;
            this.endTickBox.TabStop = false;
            this.endTickBox.Visible = false;
            this.endTickBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.trimTickBox_MouseDown);
            this.endTickBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.endTickBox_MouseMove);
            this.endTickBox.MouseUp += new System.Windows.Forms.MouseEventHandler(this.trimTickBox_MouseUp);
            // 
            // timelineBox
            // 
            this.timelineBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.timelineBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.timelineBox.BackgroundImage = global::OdessaGUIProject.Properties.Resources.editpanel_playbackcontrols_timeline_nohighlight_tile_horizontal;
            this.timelineBox.Location = new System.Drawing.Point(62, 516);
            this.timelineBox.Name = "timelineBox";
            this.timelineBox.Size = new System.Drawing.Size(666, 32);
            this.timelineBox.TabIndex = 106;
            this.timelineBox.TabStop = false;
            this.timelineBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.timeline_MouseDown);
            this.timelineBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.timeline_MouseMove);
            this.timelineBox.MouseUp += new System.Windows.Forms.MouseEventHandler(this.timeline_MouseUp);
            // 
            // timelineLeftEndPictureBox
            // 
            this.timelineLeftEndPictureBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.timelineLeftEndPictureBox.Image = global::OdessaGUIProject.Properties.Resources.editpanel_playbackcontrols_timeline_leftend;
            this.timelineLeftEndPictureBox.Location = new System.Drawing.Point(50, 516);
            this.timelineLeftEndPictureBox.Name = "timelineLeftEndPictureBox";
            this.timelineLeftEndPictureBox.Size = new System.Drawing.Size(12, 32);
            this.timelineLeftEndPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.timelineLeftEndPictureBox.TabIndex = 111;
            this.timelineLeftEndPictureBox.TabStop = false;
            // 
            // playStatePicture
            // 
            this.playStatePicture.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.playStatePicture.BackColor = System.Drawing.Color.Transparent;
            this.playStatePicture.Cursor = System.Windows.Forms.Cursors.Hand;
            this.playStatePicture.Image = global::OdessaGUIProject.Properties.Resources.editpanel_playbackcontrols_pause;
            this.playStatePicture.Location = new System.Drawing.Point(18, 516);
            this.playStatePicture.Name = "playStatePicture";
            this.playStatePicture.Size = new System.Drawing.Size(32, 32);
            this.playStatePicture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.playStatePicture.TabIndex = 108;
            this.playStatePicture.TabStop = false;
            this.playStatePicture.Text = "transPicture1";
            this.playStatePicture.Click += new System.EventHandler(this.playStatePicture_Click);
            // 
            // timelineRightEndPictureBox
            // 
            this.timelineRightEndPictureBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.timelineRightEndPictureBox.Image = global::OdessaGUIProject.Properties.Resources.editpanel_playbackcontrols_timeline_rightend;
            this.timelineRightEndPictureBox.Location = new System.Drawing.Point(728, 516);
            this.timelineRightEndPictureBox.Name = "timelineRightEndPictureBox";
            this.timelineRightEndPictureBox.Size = new System.Drawing.Size(56, 32);
            this.timelineRightEndPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.timelineRightEndPictureBox.TabIndex = 112;
            this.timelineRightEndPictureBox.TabStop = false;
            // 
            // bookmarkLocationPictureBox
            // 
            this.bookmarkLocationPictureBox.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.bookmarkLocationPictureBox.BackColor = System.Drawing.Color.Transparent;
            this.bookmarkLocationPictureBox.Image = global::OdessaGUIProject.Properties.Resources.playback_hand_marker;
            this.bookmarkLocationPictureBox.Location = new System.Drawing.Point(507, 484);
            this.bookmarkLocationPictureBox.Name = "bookmarkLocationPictureBox";
            this.bookmarkLocationPictureBox.Size = new System.Drawing.Size(20, 62);
            this.bookmarkLocationPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.bookmarkLocationPictureBox.TabIndex = 110;
            this.bookmarkLocationPictureBox.TabStop = false;
            // 
            // donateContextMenuStrip
            // 
            this.donateContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.donateFalsePositiveToolStripMenuItem,
            this.donateIntentionalMarkToolStripMenuItem,
            this.donateMissedMarkToolStripMenuItem});
            this.donateContextMenuStrip.Name = "donateContextMenuStrip";
            this.donateContextMenuStrip.Size = new System.Drawing.Size(162, 70);
            // 
            // donateFalsePositiveToolStripMenuItem
            // 
            this.donateFalsePositiveToolStripMenuItem.Name = "donateFalsePositiveToolStripMenuItem";
            this.donateFalsePositiveToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
            this.donateFalsePositiveToolStripMenuItem.Text = "False Positive";
            this.donateFalsePositiveToolStripMenuItem.Click += new System.EventHandler(this.donateFalsePositiveToolStripMenuItem_Click);
            // 
            // donateIntentionalMarkToolStripMenuItem
            // 
            this.donateIntentionalMarkToolStripMenuItem.Name = "donateIntentionalMarkToolStripMenuItem";
            this.donateIntentionalMarkToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
            this.donateIntentionalMarkToolStripMenuItem.Text = "Intentional Mark";
            this.donateIntentionalMarkToolStripMenuItem.Click += new System.EventHandler(this.donateIntentionalMarkToolStripMenuItem_Click);
            // 
            // donateMissedMarkToolStripMenuItem
            // 
            this.donateMissedMarkToolStripMenuItem.Name = "donateMissedMarkToolStripMenuItem";
            this.donateMissedMarkToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
            this.donateMissedMarkToolStripMenuItem.Text = "Missed Mark";
            this.donateMissedMarkToolStripMenuItem.Click += new System.EventHandler(this.donateMissedMarkToolStripMenuItem_Click);
            // 
            // verticalDividerPictureBox
            // 
            this.verticalDividerPictureBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.verticalDividerPictureBox.BackgroundImage = global::OdessaGUIProject.Properties.Resources.editpanel_verticaldivider_2px;
            this.verticalDividerPictureBox.Location = new System.Drawing.Point(793, 50);
            this.verticalDividerPictureBox.Name = "verticalDividerPictureBox";
            this.verticalDividerPictureBox.Size = new System.Drawing.Size(2, 499);
            this.verticalDividerPictureBox.TabIndex = 114;
            this.verticalDividerPictureBox.TabStop = false;
            // 
            // videoPlayer
            // 
            this.videoPlayer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.videoPlayer.BackColor = System.Drawing.Color.Black;
            this.videoPlayer.Location = new System.Drawing.Point(18, 50);
            this.videoPlayer.Name = "videoPlayer";
            this.videoPlayer.Size = new System.Drawing.Size(766, 431);
            this.videoPlayer.TabIndex = 0;
            this.videoPlayer.InitializationError += new System.EventHandler(this.videoPlayer_InitializationError);
            this.videoPlayer.OpenStateChanged += new System.EventHandler<OdessaGUIProject.Players.OpenStateChangedEventArgs>(this.videoPlayer_OpenStateChanged);
            this.videoPlayer.PlayerError += new System.EventHandler<OdessaGUIProject.Players.PlayerErrorEventArgs>(this.videoPlayer_PlayerError);
            this.videoPlayer.PlayStateChanged += new System.EventHandler<OdessaGUIProject.Players.PlayStateChangedEventArgs>(this.videoPlayer_PlayStateChanged);
            // 
            // bookmarkFlagTutorialBubble
            // 
            this.bookmarkFlagTutorialBubble.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.bookmarkFlagTutorialBubble.BackColor = System.Drawing.Color.Transparent;
            this.bookmarkFlagTutorialBubble.Font = new System.Drawing.Font("Open Sans", 8.25F);
            this.bookmarkFlagTutorialBubble.Location = new System.Drawing.Point(420, 288);
            this.bookmarkFlagTutorialBubble.Name = "bookmarkFlagTutorialBubble";
            this.bookmarkFlagTutorialBubble.Size = new System.Drawing.Size(193, 194);
            this.bookmarkFlagTutorialBubble.TabIndex = 115;
            this.bookmarkFlagTutorialBubble.TutorialProgress = OdessaGUIProject.UI_Helpers.TutorialProgress.TutorialBookmarkFlag;
            this.bookmarkFlagTutorialBubble.Visible = false;
            this.bookmarkFlagTutorialBubble.Advance += new System.EventHandler(this.bookmarkFlagTutorialBubble_Advance);
            // 
            // handlesTutorialBubble
            // 
            this.handlesTutorialBubble.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.handlesTutorialBubble.BackColor = System.Drawing.Color.Transparent;
            this.handlesTutorialBubble.Font = new System.Drawing.Font("Open Sans", 8.25F);
            this.handlesTutorialBubble.Location = new System.Drawing.Point(110, 316);
            this.handlesTutorialBubble.Name = "handlesTutorialBubble";
            this.handlesTutorialBubble.Size = new System.Drawing.Size(193, 194);
            this.handlesTutorialBubble.TabIndex = 116;
            this.handlesTutorialBubble.TutorialProgress = OdessaGUIProject.UI_Helpers.TutorialProgress.TutorialHandles;
            this.handlesTutorialBubble.Visible = false;
            this.handlesTutorialBubble.Advance += new System.EventHandler(this.handlesTutorialBubble_Advance);
            // 
            // closeDetailsTutorialBubble
            // 
            this.closeDetailsTutorialBubble.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.closeDetailsTutorialBubble.BackColor = System.Drawing.Color.Transparent;
            this.closeDetailsTutorialBubble.Font = new System.Drawing.Font("Open Sans", 8.25F);
            this.closeDetailsTutorialBubble.Location = new System.Drawing.Point(990, 46);
            this.closeDetailsTutorialBubble.Name = "closeDetailsTutorialBubble";
            this.closeDetailsTutorialBubble.Size = new System.Drawing.Size(193, 144);
            this.closeDetailsTutorialBubble.TabIndex = 119;
            this.closeDetailsTutorialBubble.TutorialProgress = OdessaGUIProject.UI_Helpers.TutorialProgress.TutorialCloseDetails;
            this.closeDetailsTutorialBubble.Visible = false;
            this.closeDetailsTutorialBubble.Advance += new System.EventHandler(this.closeDetailsTutorialBubble_Advance);
            // 
            // HighlightDetailsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(44)))), ((int)(((byte)(44)))));
            this.BackgroundImage = global::OdessaGUIProject.Properties.Resources.background_pattern_102x102_tile;
            this.ClientSize = new System.Drawing.Size(1184, 563);
            this.Controls.Add(this.closeDetailsTutorialBubble);
            this.Controls.Add(this.handlesTutorialBubble);
            this.Controls.Add(this.bookmarkFlagTutorialBubble);
            this.Controls.Add(this.verticalDividerPictureBox);
            this.Controls.Add(this.playHeadBox);
            this.Controls.Add(this.highlightTimelinePictureBox);
            this.Controls.Add(this.highlightDurationLabel);
            this.Controls.Add(this.startTickBox);
            this.Controls.Add(this.endTickBox);
            this.Controls.Add(this.timelineBox);
            this.Controls.Add(this.timelineLeftEndPictureBox);
            this.Controls.Add(this.playStatePicture);
            this.Controls.Add(rightPanel);
            this.Controls.Add(this.timelineRightEndPictureBox);
            this.Controls.Add(this.videoPlayer);
            this.Controls.Add(this.bookmarkLocationPictureBox);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MinimumSize = new System.Drawing.Size(600, 300);
            this.Name = "HighlightDetailsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Highlight Details (1 of 1)";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.HighlightDetailsForm_FormClosing);
            this.Load += new System.EventHandler(this.HighlightDetailsForm_Load);
            this.Shown += new System.EventHandler(this.HighlightDetailsForm_Shown);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.HighlightDetailsForm_Paint);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.HighlightDetailsForm_KeyPress);
            this.Resize += new System.EventHandler(this.HighlightDetailsForm_Resize);
            rightPanel.ResumeLayout(false);
            rightPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.playHeadBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.highlightTimelinePictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.startTickBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.endTickBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.timelineBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.timelineLeftEndPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.playStatePicture)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.timelineRightEndPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bookmarkLocationPictureBox)).EndInit();
            this.donateContextMenuStrip.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.verticalDividerPictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox titleTextBox;
        private System.Windows.Forms.Timer tickBoxLocationsTimer;
        private Players.GenericPlayerControl videoPlayer;
        private System.Windows.Forms.Timer zoomOutStartTimer;
        private System.Windows.Forms.Timer zoomOutEndTimer;
        private UI_Controls.PublishButtonControl shareToFacebookButton;
        private UI_Controls.PublishButtonControl saveToDiskButton;
        private UI_Controls.PictureButtonControl previousHighlightPictureButtonControl;
        private UI_Controls.PictureButtonControl nextHighlightPictureButtonControl;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.PictureBox pictureBox3;
        private System.Windows.Forms.Label highlightDurationLabel;
        private System.Windows.Forms.PictureBox playHeadBox;
        private System.Windows.Forms.PictureBox highlightTimelinePictureBox;
        private System.Windows.Forms.PictureBox startTickBox;
        private System.Windows.Forms.PictureBox endTickBox;
        private System.Windows.Forms.PictureBox timelineBox;
        private System.Windows.Forms.PictureBox timelineLeftEndPictureBox;
        private System.Windows.Forms.PictureBox playStatePicture;
        private System.Windows.Forms.PictureBox timelineRightEndPictureBox;
        private System.Windows.Forms.PictureBox bookmarkLocationPictureBox;
        private System.Windows.Forms.ContextMenuStrip donateContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem donateFalsePositiveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem donateIntentionalMarkToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem donateMissedMarkToolStripMenuItem;
        private UI_Controls.PictureButtonControl removeHighlightPictureButtonControl;
        private System.Windows.Forms.PictureBox verticalDividerPictureBox;
        private System.Windows.Forms.LinkLabel installQuickTimeLinkLabel;
        private UI_Controls.TutorialBubble bookmarkFlagTutorialBubble;
        private UI_Controls.TutorialBubble handlesTutorialBubble;
        private UI_Controls.TutorialBubble shareButtonTutorialBubble;
        private UI_Controls.TutorialBubble closeDetailsTutorialBubble;
    }
}
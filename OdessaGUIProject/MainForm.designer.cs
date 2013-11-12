

namespace OdessaGUIProject
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.HelpContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.supportCenterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openTutorialToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.troubleshootingLogsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.shareContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.facebookToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.twitterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mainPanel = new System.Windows.Forms.Panel();
            this.titlePictureBox = new System.Windows.Forms.PictureBox();
            this.helpPictureButtonControl = new OdessaGUIProject.UI_Controls.PictureButtonControl();
            this.settingsPictureButtonControl = new OdessaGUIProject.UI_Controls.PictureButtonControl();
            this.win7ProgressBar = new OdessaGUIProject.Windows7ProgressBar();
            this.breadcrumbsControl = new OdessaGUIProject.UI_Controls.BreadcrumbsControl();
            this.tutorialEscapeHatch = new OdessaGUIProject.UI_Controls.TutorialEscapeHatch();
            this.HelpContextMenuStrip.SuspendLayout();
            this.shareContextMenuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.titlePictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // HelpContextMenuStrip
            // 
            this.HelpContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.supportCenterToolStripMenuItem,
            this.openTutorialToolStripMenuItem,
            this.troubleshootingLogsToolStripMenuItem,
            this.toolStripSeparator2,
            this.aboutToolStripMenuItem});
            this.HelpContextMenuStrip.Name = "contextMenuStrip1";
            this.HelpContextMenuStrip.Size = new System.Drawing.Size(201, 98);
            // 
            // supportCenterToolStripMenuItem
            // 
            this.supportCenterToolStripMenuItem.Name = "supportCenterToolStripMenuItem";
            this.supportCenterToolStripMenuItem.Size = new System.Drawing.Size(200, 22);
            this.supportCenterToolStripMenuItem.Text = "&Support Center";
            this.supportCenterToolStripMenuItem.Click += new System.EventHandler(this.supportCenterToolStripMenuItem_Click);
            // 
            // openTutorialToolStripMenuItem
            // 
            this.openTutorialToolStripMenuItem.Name = "openTutorialToolStripMenuItem";
            this.openTutorialToolStripMenuItem.Size = new System.Drawing.Size(200, 22);
            this.openTutorialToolStripMenuItem.Text = "&Tutorial";
            this.openTutorialToolStripMenuItem.Click += new System.EventHandler(this.openTutorialToolStripMenuItem_Click);
            // 
            // troubleshootingLogsToolStripMenuItem
            // 
            this.troubleshootingLogsToolStripMenuItem.Name = "troubleshootingLogsToolStripMenuItem";
            this.troubleshootingLogsToolStripMenuItem.Size = new System.Drawing.Size(200, 22);
            this.troubleshootingLogsToolStripMenuItem.Text = "Troubleshooting &Logs";
            this.troubleshootingLogsToolStripMenuItem.Click += new System.EventHandler(this.troubleshootingLogsToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(197, 6);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(200, 22);
            this.aboutToolStripMenuItem.Text = "&About Highlight Hunter";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // shareContextMenuStrip
            // 
            this.shareContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.facebookToolStripMenuItem,
            this.twitterToolStripMenuItem});
            this.shareContextMenuStrip.Name = "shareContextMenuStrip";
            this.shareContextMenuStrip.Size = new System.Drawing.Size(126, 48);
            // 
            // facebookToolStripMenuItem
            // 
            this.facebookToolStripMenuItem.Image = global::OdessaGUIProject.Properties.Resources.fb16x16;
            this.facebookToolStripMenuItem.Name = "facebookToolStripMenuItem";
            this.facebookToolStripMenuItem.Size = new System.Drawing.Size(125, 22);
            this.facebookToolStripMenuItem.Text = "&Facebook";
            this.facebookToolStripMenuItem.Click += new System.EventHandler(this.facebookToolStripMenuItem_Click);
            // 
            // twitterToolStripMenuItem
            // 
            this.twitterToolStripMenuItem.Image = global::OdessaGUIProject.Properties.Resources.twitter16x16;
            this.twitterToolStripMenuItem.Name = "twitterToolStripMenuItem";
            this.twitterToolStripMenuItem.Size = new System.Drawing.Size(125, 22);
            this.twitterToolStripMenuItem.Text = "&Twitter";
            this.twitterToolStripMenuItem.Click += new System.EventHandler(this.twitterToolStripMenuItem_Click);
            // 
            // mainPanel
            // 
            this.mainPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.mainPanel.BackColor = System.Drawing.Color.Transparent;
            this.mainPanel.Location = new System.Drawing.Point(18, 192);
            this.mainPanel.Name = "mainPanel";
            this.mainPanel.Size = new System.Drawing.Size(1240, 561);
            this.mainPanel.TabIndex = 1;
            // 
            // titlePictureBox
            // 
            this.titlePictureBox.BackColor = System.Drawing.Color.Transparent;
            this.titlePictureBox.Image = global::OdessaGUIProject.Properties.Resources.header_main_logo;
            this.titlePictureBox.Location = new System.Drawing.Point(18, 46);
            this.titlePictureBox.Name = "titlePictureBox";
            this.titlePictureBox.Size = new System.Drawing.Size(333, 50);
            this.titlePictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.titlePictureBox.TabIndex = 8;
            this.titlePictureBox.TabStop = false;
            // 
            // helpPictureButtonControl
            // 
            this.helpPictureButtonControl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.helpPictureButtonControl.AutoSize = true;
            this.helpPictureButtonControl.DefaultImage = global::OdessaGUIProject.Properties.Resources.header_icon_help;
            this.helpPictureButtonControl.HoverImage = null;
            this.helpPictureButtonControl.Location = new System.Drawing.Point(1226, 57);
            this.helpPictureButtonControl.Margin = new System.Windows.Forms.Padding(5, 8, 5, 8);
            this.helpPictureButtonControl.MouseDownImage = null;
            this.helpPictureButtonControl.Name = "helpPictureButtonControl";
            this.helpPictureButtonControl.Size = new System.Drawing.Size(28, 41);
            this.helpPictureButtonControl.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.helpPictureButtonControl.TabIndex = 3;
            this.helpPictureButtonControl.Tooltip = null;
            this.helpPictureButtonControl.Click += new System.EventHandler(this.helpPictureButtonControl_Click);
            // 
            // settingsPictureButtonControl
            // 
            this.settingsPictureButtonControl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.settingsPictureButtonControl.AutoSize = true;
            this.settingsPictureButtonControl.DefaultImage = global::OdessaGUIProject.Properties.Resources.header_icon_settings;
            this.settingsPictureButtonControl.HoverImage = null;
            this.settingsPictureButtonControl.Location = new System.Drawing.Point(1138, 57);
            this.settingsPictureButtonControl.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.settingsPictureButtonControl.MouseDownImage = null;
            this.settingsPictureButtonControl.Name = "settingsPictureButtonControl";
            this.settingsPictureButtonControl.Size = new System.Drawing.Size(57, 41);
            this.settingsPictureButtonControl.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.settingsPictureButtonControl.TabIndex = 2;
            this.settingsPictureButtonControl.Tooltip = null;
            this.settingsPictureButtonControl.Click += new System.EventHandler(this.settingsPictureButtonControl_Click);
            // 
            // win7ProgressBar
            // 
            this.win7ProgressBar.ContainerControl = this;
            this.win7ProgressBar.Location = new System.Drawing.Point(541, 330);
            this.win7ProgressBar.Name = "win7ProgressBar";
            this.win7ProgressBar.Size = new System.Drawing.Size(113, 21);
            this.win7ProgressBar.TabIndex = 4;
            this.win7ProgressBar.Visible = false;
            // 
            // breadcrumbsControl
            // 
            this.breadcrumbsControl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.breadcrumbsControl.BackColor = System.Drawing.Color.Transparent;
            this.breadcrumbsControl.Location = new System.Drawing.Point(18, 120);
            this.breadcrumbsControl.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.breadcrumbsControl.Name = "breadcrumbsControl";
            this.breadcrumbsControl.Size = new System.Drawing.Size(1240, 42);
            this.breadcrumbsControl.TabIndex = 0;
            this.breadcrumbsControl.ReviewClicked += new System.EventHandler(this.breadcrumbsControl_ReviewClicked);
            this.breadcrumbsControl.ScanClicked += new System.EventHandler(this.breadcrumbsControl_ScanClicked);
            this.breadcrumbsControl.SelectVideosClicked += new System.EventHandler(this.breadcrumbsControl_SelectVideosClicked_1);
            // 
            // tutorialEscapeHatch
            // 
            this.tutorialEscapeHatch.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.tutorialEscapeHatch.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("tutorialEscapeHatch.BackgroundImage")));
            this.tutorialEscapeHatch.Font = new System.Drawing.Font("Open Sans", 8.25F);
            this.tutorialEscapeHatch.Location = new System.Drawing.Point(378, 31);
            this.tutorialEscapeHatch.Name = "tutorialEscapeHatch";
            this.tutorialEscapeHatch.Size = new System.Drawing.Size(520, 40);
            this.tutorialEscapeHatch.TabIndex = 9;
            this.tutorialEscapeHatch.Visible = false;
            this.tutorialEscapeHatch.TutorialExited += new System.EventHandler(this.tutorialEscapeHatch_TutorialExited);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(44)))), ((int)(((byte)(44)))));
            this.BackgroundImage = global::OdessaGUIProject.Properties.Resources.background_pattern_102x102_tile;
            this.ClientSize = new System.Drawing.Size(1276, 774);
            this.Controls.Add(this.tutorialEscapeHatch);
            this.Controls.Add(this.mainPanel);
            this.Controls.Add(this.win7ProgressBar);
            this.Controls.Add(this.helpPictureButtonControl);
            this.Controls.Add(this.settingsPictureButtonControl);
            this.Controls.Add(this.titlePictureBox);
            this.Controls.Add(this.breadcrumbsControl);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Franklin Gothic Book", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.MinimumSize = new System.Drawing.Size(954, 639);
            this.Name = "MainForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Highlight Hunter";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.Shown += new System.EventHandler(this.MainForm_Shown);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.MainForm_Paint);
            this.DoubleClick += new System.EventHandler(this.MainForm_DoubleClick);
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.MainForm_MouseClick);
            this.Resize += new System.EventHandler(this.MainForm_Resize);
            this.HelpContextMenuStrip.ResumeLayout(false);
            this.shareContextMenuStrip.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.titlePictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip HelpContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem supportCenterToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem openTutorialToolStripMenuItem;
        private System.Windows.Forms.PictureBox titlePictureBox;
        private System.Windows.Forms.ToolStripMenuItem troubleshootingLogsToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip shareContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem facebookToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem twitterToolStripMenuItem;
        private UI_Controls.BreadcrumbsControl breadcrumbsControl;
        private Windows7ProgressBar win7ProgressBar;
        private System.Windows.Forms.Panel mainPanel;
        private UI_Controls.PictureButtonControl settingsPictureButtonControl;
        private UI_Controls.PictureButtonControl helpPictureButtonControl;
        private UI_Controls.TutorialEscapeHatch tutorialEscapeHatch;
    }
}


namespace OdessaGUIProject.UI_Controls
{
    sealed partial class ReviewVideosControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.exportContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.exportToWindowsLiveMovieMakerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportToAdobePremiereToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportToSonyVegasToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hostPanel = new System.Windows.Forms.Panel();
            this.highlightsFoundTutorialBubble = new OdessaGUIProject.UI_Controls.TutorialBubble();
            this.innerHighlightsPanel = new System.Windows.Forms.TableLayoutPanel();
            this.proGlyphPictureBox = new System.Windows.Forms.PictureBox();
            this.startOverTutorialBubble = new OdessaGUIProject.UI_Controls.TutorialBubble();
            this.exportLinkLabel = new OdessaGUIProject.UI_Controls.CustomLinkLabel();
            this.startOverLinkLabel = new OdessaGUIProject.UI_Controls.CustomLinkLabel();
            this.saveAllLinkLabel = new OdessaGUIProject.UI_Controls.CustomLinkLabel();
            this.exportContextMenuStrip.SuspendLayout();
            this.hostPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.proGlyphPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // exportContextMenuStrip
            // 
            this.exportContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exportToWindowsLiveMovieMakerToolStripMenuItem,
            this.exportToAdobePremiereToolStripMenuItem,
            this.exportToSonyVegasToolStripMenuItem});
            this.exportContextMenuStrip.Name = "exportContextMenuStrip";
            this.exportContextMenuStrip.Size = new System.Drawing.Size(220, 70);
            // 
            // exportToWindowsLiveMovieMakerToolStripMenuItem
            // 
            this.exportToWindowsLiveMovieMakerToolStripMenuItem.Image = global::OdessaGUIProject.Properties.Resources.MovieMaker;
            this.exportToWindowsLiveMovieMakerToolStripMenuItem.Name = "exportToWindowsLiveMovieMakerToolStripMenuItem";
            this.exportToWindowsLiveMovieMakerToolStripMenuItem.Size = new System.Drawing.Size(219, 22);
            this.exportToWindowsLiveMovieMakerToolStripMenuItem.Text = "&Windows Live Movie Maker";
            this.exportToWindowsLiveMovieMakerToolStripMenuItem.Click += new System.EventHandler(this.exportToWindowsLiveMovieMakerToolStripMenuItem_Click);
            // 
            // exportToAdobePremiereToolStripMenuItem
            // 
            this.exportToAdobePremiereToolStripMenuItem.Image = global::OdessaGUIProject.Properties.Resources.pr_icon_24;
            this.exportToAdobePremiereToolStripMenuItem.Name = "exportToAdobePremiereToolStripMenuItem";
            this.exportToAdobePremiereToolStripMenuItem.Size = new System.Drawing.Size(219, 22);
            this.exportToAdobePremiereToolStripMenuItem.Text = "&Adobe Premiere";
            this.exportToAdobePremiereToolStripMenuItem.Click += new System.EventHandler(this.exportToAdobePremiereToolStripMenuItem_Click);
            // 
            // exportToSonyVegasToolStripMenuItem
            // 
            this.exportToSonyVegasToolStripMenuItem.Image = global::OdessaGUIProject.Properties.Resources.export_appicon_sony_vegas;
            this.exportToSonyVegasToolStripMenuItem.Name = "exportToSonyVegasToolStripMenuItem";
            this.exportToSonyVegasToolStripMenuItem.Size = new System.Drawing.Size(219, 22);
            this.exportToSonyVegasToolStripMenuItem.Text = "&Sony Vegas";
            this.exportToSonyVegasToolStripMenuItem.Click += new System.EventHandler(this.exportToSonyVegasToolStripMenuItem_Click);
            // 
            // hostPanel
            // 
            this.hostPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.hostPanel.AutoScroll = true;
            this.hostPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(85)))), ((int)(((byte)(85)))), ((int)(((byte)(85)))));
            this.hostPanel.Controls.Add(this.highlightsFoundTutorialBubble);
            this.hostPanel.Controls.Add(this.innerHighlightsPanel);
            this.hostPanel.Location = new System.Drawing.Point(0, 0);
            this.hostPanel.Name = "hostPanel";
            this.hostPanel.Size = new System.Drawing.Size(1243, 572);
            this.hostPanel.TabIndex = 0;
            // 
            // highlightsFoundTutorialBubble
            // 
            this.highlightsFoundTutorialBubble.BackColor = System.Drawing.Color.Transparent;
            this.highlightsFoundTutorialBubble.Font = new System.Drawing.Font("Open Sans", 8.25F);
            this.highlightsFoundTutorialBubble.Location = new System.Drawing.Point(259, 53);
            this.highlightsFoundTutorialBubble.Name = "highlightsFoundTutorialBubble";
            this.highlightsFoundTutorialBubble.Size = new System.Drawing.Size(203, 134);
            this.highlightsFoundTutorialBubble.TabIndex = 117;
            this.highlightsFoundTutorialBubble.TutorialProgress = OdessaGUIProject.UI_Helpers.TutorialProgress.TutorialHighlightsFound;
            this.highlightsFoundTutorialBubble.Visible = false;
            this.highlightsFoundTutorialBubble.Advance += new System.EventHandler(this.highlightsFoundTutorialBubble_Advance);
            // 
            // innerHighlightsPanel
            // 
            this.innerHighlightsPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.innerHighlightsPanel.AutoSize = true;
            this.innerHighlightsPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.innerHighlightsPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(85)))), ((int)(((byte)(85)))), ((int)(((byte)(85)))));
            this.innerHighlightsPanel.ColumnCount = 1;
            this.innerHighlightsPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.innerHighlightsPanel.Location = new System.Drawing.Point(0, 0);
            this.innerHighlightsPanel.Margin = new System.Windows.Forms.Padding(0);
            this.innerHighlightsPanel.Name = "innerHighlightsPanel";
            this.innerHighlightsPanel.Size = new System.Drawing.Size(1243, 0);
            this.innerHighlightsPanel.TabIndex = 0;
            // 
            // proGlyphPictureBox
            // 
            this.proGlyphPictureBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.proGlyphPictureBox.BackColor = System.Drawing.Color.Transparent;
            this.proGlyphPictureBox.Cursor = System.Windows.Forms.Cursors.Hand;
            this.proGlyphPictureBox.Image = global::OdessaGUIProject.Properties.Resources.icons_proglyph;
            this.proGlyphPictureBox.Location = new System.Drawing.Point(441, 594);
            this.proGlyphPictureBox.Name = "proGlyphPictureBox";
            this.proGlyphPictureBox.Size = new System.Drawing.Size(24, 17);
            this.proGlyphPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.proGlyphPictureBox.TabIndex = 43;
            this.proGlyphPictureBox.TabStop = false;
            this.proGlyphPictureBox.Click += new System.EventHandler(this.proGlyphPictureBox_Click);
            // 
            // startOverTutorialBubble
            // 
            this.startOverTutorialBubble.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.startOverTutorialBubble.BackColor = System.Drawing.Color.Transparent;
            this.startOverTutorialBubble.Font = new System.Drawing.Font("Open Sans", 8.25F);
            this.startOverTutorialBubble.Location = new System.Drawing.Point(1050, 442);
            this.startOverTutorialBubble.Name = "startOverTutorialBubble";
            this.startOverTutorialBubble.Size = new System.Drawing.Size(193, 144);
            this.startOverTutorialBubble.TabIndex = 116;
            this.startOverTutorialBubble.TutorialProgress = OdessaGUIProject.UI_Helpers.TutorialProgress.TutorialStartOver;
            this.startOverTutorialBubble.Visible = false;
            this.startOverTutorialBubble.Advance += new System.EventHandler(this.startOverTutorialBubble_Advance);
            // 
            // exportLinkLabel
            // 
            this.exportLinkLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.exportLinkLabel.AutoSize = true;
            this.exportLinkLabel.BackColor = System.Drawing.Color.Transparent;
            this.exportLinkLabel.Image = global::OdessaGUIProject.Properties.Resources.icons_16px_open_in_software;
            this.exportLinkLabel.LinkText = "Open all in editing software...";
            this.exportLinkLabel.Location = new System.Drawing.Point(233, 587);
            this.exportLinkLabel.Name = "exportLinkLabel";
            this.exportLinkLabel.Size = new System.Drawing.Size(209, 29);
            this.exportLinkLabel.TabIndex = 2;
            this.exportLinkLabel.Click += new System.EventHandler(this.exportLinkLabel_Click);
            // 
            // startOverLinkLabel
            // 
            this.startOverLinkLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.startOverLinkLabel.AutoSize = true;
            this.startOverLinkLabel.BackColor = System.Drawing.Color.Transparent;
            this.startOverLinkLabel.Image = global::OdessaGUIProject.Properties.Resources.icons_16px_startover;
            this.startOverLinkLabel.LinkText = "Start over...";
            this.startOverLinkLabel.Location = new System.Drawing.Point(1136, 587);
            this.startOverLinkLabel.Name = "startOverLinkLabel";
            this.startOverLinkLabel.Size = new System.Drawing.Size(104, 29);
            this.startOverLinkLabel.TabIndex = 44;
            this.startOverLinkLabel.Click += new System.EventHandler(this.startOverLinkLabel_Click);
            // 
            // saveAllLinkLabel
            // 
            this.saveAllLinkLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.saveAllLinkLabel.AutoSize = true;
            this.saveAllLinkLabel.BackColor = System.Drawing.Color.Transparent;
            this.saveAllLinkLabel.Image = global::OdessaGUIProject.Properties.Resources.icons_16px_saveall;
            this.saveAllLinkLabel.LinkText = "Save all as videos...";
            this.saveAllLinkLabel.Location = new System.Drawing.Point(3, 587);
            this.saveAllLinkLabel.Name = "saveAllLinkLabel";
            this.saveAllLinkLabel.Size = new System.Drawing.Size(183, 29);
            this.saveAllLinkLabel.TabIndex = 1;
            this.saveAllLinkLabel.Click += new System.EventHandler(this.saveAllLinkLabel_Click);
            // 
            // ReviewVideosControl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.Controls.Add(this.startOverTutorialBubble);
            this.Controls.Add(this.hostPanel);
            this.Controls.Add(this.exportLinkLabel);
            this.Controls.Add(this.startOverLinkLabel);
            this.Controls.Add(this.saveAllLinkLabel);
            this.Controls.Add(this.proGlyphPictureBox);
            this.DoubleBuffered = true;
            this.Name = "ReviewVideosControl";
            this.Size = new System.Drawing.Size(1243, 633);
            this.Load += new System.EventHandler(this.ReviewVideosControl_Load);
            this.Resize += new System.EventHandler(this.ReviewVideosControl_Resize);
            this.exportContextMenuStrip.ResumeLayout(false);
            this.hostPanel.ResumeLayout(false);
            this.hostPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.proGlyphPictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip exportContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem exportToWindowsLiveMovieMakerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportToAdobePremiereToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportToSonyVegasToolStripMenuItem;
        private System.Windows.Forms.Panel hostPanel;
        private System.Windows.Forms.TableLayoutPanel innerHighlightsPanel;
        private CustomLinkLabel saveAllLinkLabel;
        private CustomLinkLabel exportLinkLabel;
        private System.Windows.Forms.PictureBox proGlyphPictureBox;
        private CustomLinkLabel startOverLinkLabel;
        private TutorialBubble startOverTutorialBubble;
        private TutorialBubble highlightsFoundTutorialBubble;
    }
}

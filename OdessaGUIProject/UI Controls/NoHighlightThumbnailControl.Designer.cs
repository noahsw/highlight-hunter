namespace OdessaGUIProject.UI_Controls
{
    partial class NoHighlightThumbnailControl
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
            this.statusLabel = new System.Windows.Forms.Label();
            this.openSupportLinkLabel = new System.Windows.Forms.LinkLabel();
            this.rescanLinkLabel = new System.Windows.Forms.LinkLabel();
            this.thumbnailMaskPicture = new System.Windows.Forms.PictureBox();
            this.thumbnailBox = new System.Windows.Forms.PictureBox();
            this.durationLabel = new System.Windows.Forms.Label();
            this.dividerPictureBox = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.thumbnailMaskPicture)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.thumbnailBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dividerPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // statusLabel
            // 
            this.statusLabel.AutoSize = true;
            this.statusLabel.BackColor = System.Drawing.Color.Transparent;
            this.statusLabel.Cursor = System.Windows.Forms.Cursors.Default;
            this.statusLabel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.statusLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.statusLabel.Location = new System.Drawing.Point(279, 62);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(558, 21);
            this.statusLabel.TabIndex = 1;
            this.statusLabel.Text = "No highlights were found in this video. Did you cover the lens for a full second?" +
    "";
            // 
            // openSupportLinkLabel
            // 
            this.openSupportLinkLabel.ActiveLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(204)))), ((int)(((byte)(51)))));
            this.openSupportLinkLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.openSupportLinkLabel.AutoSize = true;
            this.openSupportLinkLabel.BackColor = System.Drawing.Color.Transparent;
            this.openSupportLinkLabel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.openSupportLinkLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.openSupportLinkLabel.LinkArea = new System.Windows.Forms.LinkArea(0, 32);
            this.openSupportLinkLabel.LinkBehavior = System.Windows.Forms.LinkBehavior.AlwaysUnderline;
            this.openSupportLinkLabel.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(153)))), ((int)(((byte)(204)))));
            this.openSupportLinkLabel.Location = new System.Drawing.Point(279, 118);
            this.openSupportLinkLabel.Name = "openSupportLinkLabel";
            this.openSupportLinkLabel.Size = new System.Drawing.Size(237, 21);
            this.openSupportLinkLabel.TabIndex = 3;
            this.openSupportLinkLabel.TabStop = true;
            this.openSupportLinkLabel.Text = "Open Highlight Hunter Support...";
            this.openSupportLinkLabel.Visible = false;
            this.openSupportLinkLabel.VisitedLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(204)))), ((int)(((byte)(51)))));
            this.openSupportLinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.openSupportLinkLabel_LinkClicked);
            // 
            // rescanLinkLabel
            // 
            this.rescanLinkLabel.ActiveLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(204)))), ((int)(((byte)(51)))));
            this.rescanLinkLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.rescanLinkLabel.AutoSize = true;
            this.rescanLinkLabel.BackColor = System.Drawing.Color.Transparent;
            this.rescanLinkLabel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rescanLinkLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.rescanLinkLabel.LinkArea = new System.Windows.Forms.LinkArea(0, 41);
            this.rescanLinkLabel.LinkBehavior = System.Windows.Forms.LinkBehavior.AlwaysUnderline;
            this.rescanLinkLabel.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(153)))), ((int)(((byte)(204)))));
            this.rescanLinkLabel.Location = new System.Drawing.Point(279, 118);
            this.rescanLinkLabel.Name = "rescanLinkLabel";
            this.rescanLinkLabel.Size = new System.Drawing.Size(283, 21);
            this.rescanLinkLabel.TabIndex = 2;
            this.rescanLinkLabel.TabStop = true;
            this.rescanLinkLabel.Text = "Rescan with more forgiving sensitivity...";
            this.rescanLinkLabel.Visible = false;
            this.rescanLinkLabel.VisitedLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(204)))), ((int)(((byte)(51)))));
            this.rescanLinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.rescanLinkLabel_LinkClicked);
            // 
            // thumbnailMaskPicture
            // 
            this.thumbnailMaskPicture.BackColor = System.Drawing.Color.Transparent;
            this.thumbnailMaskPicture.Image = global::OdessaGUIProject.Properties.Resources.review_video_thumb_rightside_point_mask;
            this.thumbnailMaskPicture.Location = new System.Drawing.Point(260, 0);
            this.thumbnailMaskPicture.Name = "thumbnailMaskPicture";
            this.thumbnailMaskPicture.Size = new System.Drawing.Size(13, 153);
            this.thumbnailMaskPicture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.thumbnailMaskPicture.TabIndex = 16;
            this.thumbnailMaskPicture.TabStop = false;
            this.thumbnailMaskPicture.Text = "transPicture1";
            // 
            // thumbnailBox
            // 
            this.thumbnailBox.BackColor = System.Drawing.Color.Transparent;
            this.thumbnailBox.Cursor = System.Windows.Forms.Cursors.Default;
            this.thumbnailBox.Image = global::OdessaGUIProject.Properties.Resources.review_novideos_question_mark_icon;
            this.thumbnailBox.Location = new System.Drawing.Point(0, 0);
            this.thumbnailBox.Name = "thumbnailBox";
            this.thumbnailBox.Size = new System.Drawing.Size(259, 153);
            this.thumbnailBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.thumbnailBox.TabIndex = 1;
            this.thumbnailBox.TabStop = false;
            // 
            // durationLabel
            // 
            this.durationLabel.AutoSize = true;
            this.durationLabel.BackColor = System.Drawing.Color.Transparent;
            this.durationLabel.Font = new System.Drawing.Font("Open Sans", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.durationLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.durationLabel.Location = new System.Drawing.Point(269, 0);
            this.durationLabel.Name = "durationLabel";
            this.durationLabel.Size = new System.Drawing.Size(161, 65);
            this.durationLabel.TabIndex = 0;
            this.durationLabel.Text = "Darn.";
            // 
            // dividerPictureBox
            // 
            this.dividerPictureBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dividerPictureBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(44)))), ((int)(((byte)(44)))));
            this.dividerPictureBox.Location = new System.Drawing.Point(0, 153);
            this.dividerPictureBox.Name = "dividerPictureBox";
            this.dividerPictureBox.Size = new System.Drawing.Size(875, 1);
            this.dividerPictureBox.TabIndex = 18;
            this.dividerPictureBox.TabStop = false;
            // 
            // NoHighlightThumbnailControl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(85)))), ((int)(((byte)(85)))), ((int)(((byte)(85)))));
            this.Controls.Add(this.dividerPictureBox);
            this.Controls.Add(this.thumbnailMaskPicture);
            this.Controls.Add(this.rescanLinkLabel);
            this.Controls.Add(this.openSupportLinkLabel);
            this.Controls.Add(this.statusLabel);
            this.Controls.Add(this.thumbnailBox);
            this.Controls.Add(this.durationLabel);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "NoHighlightThumbnailControl";
            this.Size = new System.Drawing.Size(875, 154);
            this.Load += new System.EventHandler(this.NoHighlightThumbnailControl_Load);
            ((System.ComponentModel.ISupportInitialize)(this.thumbnailMaskPicture)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.thumbnailBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dividerPictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox thumbnailBox;
        private System.Windows.Forms.Label statusLabel;
        private System.Windows.Forms.LinkLabel openSupportLinkLabel;
        private System.Windows.Forms.LinkLabel rescanLinkLabel;
        private System.Windows.Forms.PictureBox thumbnailMaskPicture;
        private System.Windows.Forms.Label durationLabel;
        private System.Windows.Forms.PictureBox dividerPictureBox;
    }
}

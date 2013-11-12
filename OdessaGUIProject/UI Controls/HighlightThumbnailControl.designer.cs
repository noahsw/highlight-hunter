namespace OdessaGUIProject.UI_Controls
{
    partial class HighlightThumbnailControl
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
            this.titleLabel = new System.Windows.Forms.Label();
            this.durationLabel = new System.Windows.Forms.Label();
            this.thumbnailMaskPicture = new System.Windows.Forms.PictureBox();
            this.removePicture = new System.Windows.Forms.PictureBox();
            this.thumbnailBox = new System.Windows.Forms.PictureBox();
            this.dividerPictureBox = new System.Windows.Forms.PictureBox();
            this.statusTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)(this.thumbnailMaskPicture)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.removePicture)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.thumbnailBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dividerPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // titleLabel
            // 
            this.titleLabel.AutoSize = true;
            this.titleLabel.BackColor = System.Drawing.Color.Transparent;
            this.titleLabel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.titleLabel.Font = new System.Drawing.Font("Open Sans", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.titleLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(153)))), ((int)(((byte)(153)))));
            this.titleLabel.Location = new System.Drawing.Point(394, 19);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(53, 28);
            this.titleLabel.TabIndex = 1;
            this.titleLabel.Text = "Title";
            this.titleLabel.Click += new System.EventHandler(this.titleLabel_Click);
            // 
            // durationLabel
            // 
            this.durationLabel.AutoSize = true;
            this.durationLabel.BackColor = System.Drawing.Color.Transparent;
            this.durationLabel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.durationLabel.Font = new System.Drawing.Font("Open Sans", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.durationLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.durationLabel.Location = new System.Drawing.Point(269, 0);
            this.durationLabel.Name = "durationLabel";
            this.durationLabel.Size = new System.Drawing.Size(123, 65);
            this.durationLabel.TabIndex = 0;
            this.durationLabel.Text = "0:30";
            this.durationLabel.Click += new System.EventHandler(this.durationLabel_Click);
            // 
            // thumbnailMaskPicture
            // 
            this.thumbnailMaskPicture.BackColor = System.Drawing.Color.Transparent;
            this.thumbnailMaskPicture.Image = global::OdessaGUIProject.Properties.Resources.review_video_thumb_rightside_point_mask;
            this.thumbnailMaskPicture.Location = new System.Drawing.Point(258, 0);
            this.thumbnailMaskPicture.Name = "thumbnailMaskPicture";
            this.thumbnailMaskPicture.Size = new System.Drawing.Size(12, 153);
            this.thumbnailMaskPicture.TabIndex = 7;
            this.thumbnailMaskPicture.TabStop = false;
            this.thumbnailMaskPicture.Text = "transPicture1";
            // 
            // removePicture
            // 
            this.removePicture.BackColor = System.Drawing.Color.Transparent;
            this.removePicture.Cursor = System.Windows.Forms.Cursors.Hand;
            this.removePicture.Image = global::OdessaGUIProject.Properties.Resources.review_video_hover_deleteicon;
            this.removePicture.Location = new System.Drawing.Point(237, 3);
            this.removePicture.Name = "removePicture";
            this.removePicture.Size = new System.Drawing.Size(20, 21);
            this.removePicture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.removePicture.TabIndex = 3;
            this.removePicture.TabStop = false;
            this.removePicture.Text = "transPicture1";
            this.removePicture.Visible = false;
            this.removePicture.Click += new System.EventHandler(this.removePicture_Click);
            // 
            // thumbnailBox
            // 
            this.thumbnailBox.BackColor = System.Drawing.Color.Transparent;
            this.thumbnailBox.Cursor = System.Windows.Forms.Cursors.Hand;
            this.thumbnailBox.Image = global::OdessaGUIProject.Properties.Resources.review_novideos_question_mark_icon;
            this.thumbnailBox.Location = new System.Drawing.Point(0, 0);
            this.thumbnailBox.Name = "thumbnailBox";
            this.thumbnailBox.Size = new System.Drawing.Size(259, 153);
            this.thumbnailBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.thumbnailBox.TabIndex = 0;
            this.thumbnailBox.TabStop = false;
            this.thumbnailBox.Click += new System.EventHandler(this.thumbnailBox_Click);
            this.thumbnailBox.MouseEnter += new System.EventHandler(this.thumbnailBox_MouseEnter);
            this.thumbnailBox.MouseLeave += new System.EventHandler(this.thumbnailBox_MouseLeave);
            // 
            // dividerPictureBox
            // 
            this.dividerPictureBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dividerPictureBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(44)))), ((int)(((byte)(44)))));
            this.dividerPictureBox.Location = new System.Drawing.Point(0, 153);
            this.dividerPictureBox.Name = "dividerPictureBox";
            this.dividerPictureBox.Size = new System.Drawing.Size(804, 1);
            this.dividerPictureBox.TabIndex = 8;
            this.dividerPictureBox.TabStop = false;
            // 
            // statusTableLayoutPanel
            // 
            this.statusTableLayoutPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.statusTableLayoutPanel.ColumnCount = 1;
            this.statusTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.statusTableLayoutPanel.Location = new System.Drawing.Point(399, 53);
            this.statusTableLayoutPanel.Margin = new System.Windows.Forms.Padding(0);
            this.statusTableLayoutPanel.Name = "statusTableLayoutPanel";
            this.statusTableLayoutPanel.RowCount = 2;
            this.statusTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.statusTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.statusTableLayoutPanel.Size = new System.Drawing.Size(358, 88);
            this.statusTableLayoutPanel.TabIndex = 2;
            // 
            // HighlightThumbnailControl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(85)))), ((int)(((byte)(85)))), ((int)(((byte)(85)))));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.Controls.Add(this.titleLabel);
            this.Controls.Add(this.thumbnailBox);
            this.Controls.Add(this.statusTableLayoutPanel);
            this.Controls.Add(this.dividerPictureBox);
            this.Controls.Add(this.thumbnailMaskPicture);
            this.Controls.Add(this.removePicture);
            this.Controls.Add(this.durationLabel);
            this.Cursor = System.Windows.Forms.Cursors.Default;
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "HighlightThumbnailControl";
            this.Size = new System.Drawing.Size(804, 154);
            this.Load += new System.EventHandler(this.HighlightThumbnailControl_Load);
            ((System.ComponentModel.ISupportInitialize)(this.thumbnailMaskPicture)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.removePicture)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.thumbnailBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dividerPictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox thumbnailBox;
        private System.Windows.Forms.Label titleLabel;
        private System.Windows.Forms.Label durationLabel;
        private System.Windows.Forms.PictureBox removePicture;
        private System.Windows.Forms.PictureBox thumbnailMaskPicture;
        private System.Windows.Forms.PictureBox dividerPictureBox;
        private System.Windows.Forms.TableLayoutPanel statusTableLayoutPanel;
    }
}

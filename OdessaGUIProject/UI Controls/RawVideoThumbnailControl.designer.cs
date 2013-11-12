namespace OdessaGUIProject.UI_Controls
{
    partial class RawVideoThumbnailControl
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
            this.removePicture = new OdessaGUIProject.UI_Controls.PictureButtonControl();
            this.SuspendLayout();
            // 
            // titleLabel
            // 
            this.titleLabel.BackColor = System.Drawing.Color.Transparent;
            this.titleLabel.Cursor = System.Windows.Forms.Cursors.Default;
            this.titleLabel.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.titleLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.titleLabel.Location = new System.Drawing.Point(3, 158);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(266, 21);
            this.titleLabel.TabIndex = 0;
            this.titleLabel.Text = "File name";
            this.titleLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // removePicture
            // 
            this.removePicture.AutoSize = true;
            this.removePicture.Cursor = System.Windows.Forms.Cursors.Hand;
            this.removePicture.DefaultImage = global::OdessaGUIProject.Properties.Resources.review_video_hover_deleteicon;
            this.removePicture.HoverImage = global::OdessaGUIProject.Properties.Resources.review_video_hover_deleteicon_hover;
            this.removePicture.Location = new System.Drawing.Point(3, 3);
            this.removePicture.MouseDownImage = null;
            this.removePicture.Name = "removePicture";
            this.removePicture.Size = new System.Drawing.Size(20, 21);
            this.removePicture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Normal;
            this.removePicture.TabIndex = 1;
            this.removePicture.Tooltip = null;
            this.removePicture.Visible = false;
            this.removePicture.Click += new System.EventHandler(this.removePicture_Click);
            // 
            // RawVideoThumbnailControl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(85)))), ((int)(((byte)(85)))), ((int)(((byte)(85)))));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.Controls.Add(this.removePicture);
            this.Controls.Add(this.titleLabel);
            this.DoubleBuffered = true;
            this.Name = "RawVideoThumbnailControl";
            this.Size = new System.Drawing.Size(272, 184);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.RawVideoThumbnailControl_Paint);
            this.MouseEnter += new System.EventHandler(this.RawVideoThumbnailControl_MouseEnter);
            this.MouseLeave += new System.EventHandler(this.RawVideoThumbnailControl_MouseLeave);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label titleLabel;
        private PictureButtonControl removePicture;
    }
}

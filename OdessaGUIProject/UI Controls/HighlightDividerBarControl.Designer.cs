namespace OdessaGUIProject.UI_Controls
{
    partial class HighlightDividerBarControl
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
            this.inputVideoNameLabel = new System.Windows.Forms.Label();
            this.highlightCountLabel = new System.Windows.Forms.Label();
            this.dividerPictureBox = new System.Windows.Forms.PictureBox();
            this.dividerLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dividerPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // inputVideoNameLabel
            // 
            this.inputVideoNameLabel.AutoSize = true;
            this.inputVideoNameLabel.BackColor = System.Drawing.Color.Transparent;
            this.inputVideoNameLabel.Font = new System.Drawing.Font("Open Sans", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.inputVideoNameLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.inputVideoNameLabel.Location = new System.Drawing.Point(3, 10);
            this.inputVideoNameLabel.Name = "inputVideoNameLabel";
            this.inputVideoNameLabel.Size = new System.Drawing.Size(83, 22);
            this.inputVideoNameLabel.TabIndex = 0;
            this.inputVideoNameLabel.Text = "GOPR002";
            this.inputVideoNameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // highlightCountLabel
            // 
            this.highlightCountLabel.AutoSize = true;
            this.highlightCountLabel.BackColor = System.Drawing.Color.Transparent;
            this.highlightCountLabel.Font = new System.Drawing.Font("Open Sans", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.highlightCountLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.highlightCountLabel.Location = new System.Drawing.Point(92, 10);
            this.highlightCountLabel.Name = "highlightCountLabel";
            this.highlightCountLabel.Size = new System.Drawing.Size(150, 22);
            this.highlightCountLabel.TabIndex = 1;
            this.highlightCountLabel.Text = "5 highlights found:";
            this.highlightCountLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // dividerPictureBox
            // 
            this.dividerPictureBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dividerPictureBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(44)))), ((int)(((byte)(44)))));
            this.dividerPictureBox.Location = new System.Drawing.Point(0, 40);
            this.dividerPictureBox.Name = "dividerPictureBox";
            this.dividerPictureBox.Size = new System.Drawing.Size(421, 1);
            this.dividerPictureBox.TabIndex = 9;
            this.dividerPictureBox.TabStop = false;
            // 
            // dividerLabel
            // 
            this.dividerLabel.AutoSize = true;
            this.dividerLabel.BackColor = System.Drawing.Color.Transparent;
            this.dividerLabel.Font = new System.Drawing.Font("Open Sans", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dividerLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.dividerLabel.Location = new System.Drawing.Point(82, 9);
            this.dividerLabel.Margin = new System.Windows.Forms.Padding(0);
            this.dividerLabel.Name = "dividerLabel";
            this.dividerLabel.Size = new System.Drawing.Size(19, 22);
            this.dividerLabel.TabIndex = 10;
            this.dividerLabel.Text = "|";
            this.dividerLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // HighlightDividerBarControl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.BackgroundImage = global::OdessaGUIProject.Properties.Resources.review_divider_bar_5px_horizontal_tile;
            this.Controls.Add(this.inputVideoNameLabel);
            this.Controls.Add(this.highlightCountLabel);
            this.Controls.Add(this.dividerLabel);
            this.Controls.Add(this.dividerPictureBox);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "HighlightDividerBarControl";
            this.Size = new System.Drawing.Size(421, 41);
            ((System.ComponentModel.ISupportInitialize)(this.dividerPictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label inputVideoNameLabel;
        private System.Windows.Forms.Label highlightCountLabel;
        private System.Windows.Forms.PictureBox dividerPictureBox;
        private System.Windows.Forms.Label dividerLabel;
    }
}

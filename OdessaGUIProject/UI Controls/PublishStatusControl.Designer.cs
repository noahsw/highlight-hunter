namespace OdessaGUIProject.UI_Controls
{
    sealed partial class PublishStatusControl
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
            this.statusPictureBox = new System.Windows.Forms.PictureBox();
            this.statusLinkLabel = new System.Windows.Forms.LinkLabel();
            ((System.ComponentModel.ISupportInitialize)(this.statusPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // statusPictureBox
            // 
            this.statusPictureBox.BackColor = System.Drawing.Color.Transparent;
            this.statusPictureBox.Image = global::OdessaGUIProject.Properties.Resources.review_sharestatus_icon_success;
            this.statusPictureBox.Location = new System.Drawing.Point(0, 5);
            this.statusPictureBox.Name = "statusPictureBox";
            this.statusPictureBox.Size = new System.Drawing.Size(21, 21);
            this.statusPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.statusPictureBox.TabIndex = 0;
            this.statusPictureBox.TabStop = false;
            this.statusPictureBox.Click += new System.EventHandler(this.statusPictureBox_Click);
            // 
            // statusLinkLabel
            // 
            this.statusLinkLabel.ActiveLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(153)))), ((int)(((byte)(204)))));
            this.statusLinkLabel.BackColor = System.Drawing.Color.Transparent;
            this.statusLinkLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.statusLinkLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(153)))), ((int)(((byte)(153)))));
            this.statusLinkLabel.LinkArea = new System.Windows.Forms.LinkArea(10, 8);
            this.statusLinkLabel.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.statusLinkLabel.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.statusLinkLabel.Location = new System.Drawing.Point(28, 6);
            this.statusLinkLabel.Name = "statusLinkLabel";
            this.statusLinkLabel.Size = new System.Drawing.Size(222, 20);
            this.statusLinkLabel.TabIndex = 0;
            this.statusLinkLabel.TabStop = true;
            this.statusLinkLabel.Text = "Shared to facebook";
            this.statusLinkLabel.UseCompatibleTextRendering = true;
            this.statusLinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.statusLinkLabel_LinkClicked);
            this.statusLinkLabel.MouseEnter += new System.EventHandler(this.statusLinkLabel_MouseEnter);
            this.statusLinkLabel.MouseLeave += new System.EventHandler(this.statusLinkLabel_MouseLeave);
            // 
            // PublishStatusControl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(85)))), ((int)(((byte)(85)))), ((int)(((byte)(85)))));
            this.Controls.Add(this.statusLinkLabel);
            this.Controls.Add(this.statusPictureBox);
            this.DoubleBuffered = true;
            this.Name = "PublishStatusControl";
            this.Size = new System.Drawing.Size(258, 31);
            ((System.ComponentModel.ISupportInitialize)(this.statusPictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox statusPictureBox;
        private System.Windows.Forms.LinkLabel statusLinkLabel;
    }
}

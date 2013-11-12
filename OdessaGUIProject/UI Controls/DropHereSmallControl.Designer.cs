namespace OdessaGUIProject.UI_Controls
{
    partial class DropHereSmallControl
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
            this.browseLinkLabel = new System.Windows.Forms.LinkLabel();
            this.selectFromCameraLinkLabel = new System.Windows.Forms.LinkLabel();
            this.dropHerePictureBox = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.dropHerePictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // browseLinkLabel
            // 
            this.browseLinkLabel.ActiveLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(153)))), ((int)(((byte)(204)))));
            this.browseLinkLabel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.browseLinkLabel.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.browseLinkLabel.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.browseLinkLabel.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.browseLinkLabel.Location = new System.Drawing.Point(117, 101);
            this.browseLinkLabel.Name = "browseLinkLabel";
            this.browseLinkLabel.Size = new System.Drawing.Size(139, 23);
            this.browseLinkLabel.TabIndex = 1;
            this.browseLinkLabel.TabStop = true;
            this.browseLinkLabel.Text = "Browse computer...";
            this.browseLinkLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.browseLinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.browseLinkLabel_LinkClicked);
            this.browseLinkLabel.MouseEnter += new System.EventHandler(this.browseLinkLabel_MouseEnter);
            this.browseLinkLabel.MouseLeave += new System.EventHandler(this.browseLinkLabel_MouseLeave);
            // 
            // selectFromCameraLinkLabel
            // 
            this.selectFromCameraLinkLabel.ActiveLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(153)))), ((int)(((byte)(204)))));
            this.selectFromCameraLinkLabel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.selectFromCameraLinkLabel.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.selectFromCameraLinkLabel.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.selectFromCameraLinkLabel.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.selectFromCameraLinkLabel.Location = new System.Drawing.Point(117, 79);
            this.selectFromCameraLinkLabel.Name = "selectFromCameraLinkLabel";
            this.selectFromCameraLinkLabel.Size = new System.Drawing.Size(139, 23);
            this.selectFromCameraLinkLabel.TabIndex = 0;
            this.selectFromCameraLinkLabel.TabStop = true;
            this.selectFromCameraLinkLabel.Text = "Select from camera...";
            this.selectFromCameraLinkLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.selectFromCameraLinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.selectFromCameraLinkLabel_LinkClicked);
            this.selectFromCameraLinkLabel.MouseEnter += new System.EventHandler(this.selectFromCameraLinkLabel_MouseEnter);
            this.selectFromCameraLinkLabel.MouseLeave += new System.EventHandler(this.selectFromCameraLinkLabel_MouseLeave);
            // 
            // dropHerePictureBox
            // 
            this.dropHerePictureBox.Image = global::OdessaGUIProject.Properties.Resources.select_droptarget_small;
            this.dropHerePictureBox.Location = new System.Drawing.Point(10, 16);
            this.dropHerePictureBox.Name = "dropHerePictureBox";
            this.dropHerePictureBox.Size = new System.Drawing.Size(272, 140);
            this.dropHerePictureBox.TabIndex = 0;
            this.dropHerePictureBox.TabStop = false;
            // 
            // DropHereSmallControl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(85)))), ((int)(((byte)(85)))), ((int)(((byte)(85)))));
            this.Controls.Add(this.browseLinkLabel);
            this.Controls.Add(this.selectFromCameraLinkLabel);
            this.Controls.Add(this.dropHerePictureBox);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "DropHereSmallControl";
            this.Size = new System.Drawing.Size(292, 193);
            this.Load += new System.EventHandler(this.DropHereSmallControl_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dropHerePictureBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox dropHerePictureBox;
        private System.Windows.Forms.LinkLabel browseLinkLabel;
        private System.Windows.Forms.LinkLabel selectFromCameraLinkLabel;
    }
}


namespace OdessaGUIProject.UI_Controls
{
    partial class PublishButtonControl
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
            this.cancelButton = new OdessaGUIProject.UI_Controls.PictureButtonControl();
            this.iconPicture = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.iconPicture)).BeginInit();
            this.SuspendLayout();
            // 
            // statusLabel
            // 
            this.statusLabel.BackColor = System.Drawing.Color.Transparent;
            this.statusLabel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.statusLabel.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.statusLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.statusLabel.Location = new System.Drawing.Point(46, 0);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(306, 42);
            this.statusLabel.TabIndex = 1;
            this.statusLabel.Text = "Share to Facebook";
            this.statusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.statusLabel.Click += new System.EventHandler(this.statusLabel_Click);
            this.statusLabel.MouseEnter += new System.EventHandler(this.statusLabel_MouseEnter);
            this.statusLabel.MouseLeave += new System.EventHandler(this.statusLabel_MouseLeave);
            // 
            // cancelButton
            // 
            this.cancelButton.AutoSize = true;
            this.cancelButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.cancelButton.DefaultImage = global::OdessaGUIProject.Properties.Resources.review_video_hover_deleteicon;
            this.cancelButton.HoverImage = global::OdessaGUIProject.Properties.Resources.review_video_hover_deleteicon_hover;
            this.cancelButton.Location = new System.Drawing.Point(325, 11);
            this.cancelButton.MouseDownImage = null;
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(20, 21);
            this.cancelButton.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Normal;
            this.cancelButton.TabIndex = 2;
            this.cancelButton.Tooltip = null;
            this.cancelButton.Visible = false;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // iconPicture
            // 
            this.iconPicture.BackColor = System.Drawing.Color.Transparent;
            this.iconPicture.Cursor = System.Windows.Forms.Cursors.Hand;
            this.iconPicture.Image = global::OdessaGUIProject.Properties.Resources.icons_42px_facebook;
            this.iconPicture.Location = new System.Drawing.Point(0, 0);
            this.iconPicture.Name = "iconPicture";
            this.iconPicture.Size = new System.Drawing.Size(42, 42);
            this.iconPicture.TabIndex = 3;
            this.iconPicture.TabStop = false;
            this.iconPicture.Click += new System.EventHandler(this.iconPicture_Click);
            this.iconPicture.MouseEnter += new System.EventHandler(this.iconPicture_MouseEnter);
            this.iconPicture.MouseLeave += new System.EventHandler(this.iconPicture_MouseLeave);
            // 
            // PublishButtonControl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.BackgroundImage = global::OdessaGUIProject.Properties.Resources.editpanel_share_element;
            this.Controls.Add(this.iconPicture);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.statusLabel);
            this.Name = "PublishButtonControl";
            this.Size = new System.Drawing.Size(352, 42);
            this.Load += new System.EventHandler(this.PublishButtonControl_Load);
            ((System.ComponentModel.ISupportInitialize)(this.iconPicture)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label statusLabel;
        private PictureButtonControl cancelButton;
        private System.Windows.Forms.PictureBox iconPicture;
    }
}

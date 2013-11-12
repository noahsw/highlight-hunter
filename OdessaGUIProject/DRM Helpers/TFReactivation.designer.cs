

namespace OdessaGUIProject.DRM_Helpers
{
    sealed partial class TFReactivation
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
            OdessaGUIProject.TransLabel transLabel1;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TFReactivation));
            this.lblCode = new OdessaGUIProject.TransLabel();
            this.label1 = new OdessaGUIProject.TransLabel();
            this.label4 = new OdessaGUIProject.TransLabel();
            this.label3 = new OdessaGUIProject.TransLabel();
            this.tbConfPass = new System.Windows.Forms.TextBox();
            this.tbNewPass = new System.Windows.Forms.TextBox();
            this.tbCurrentPass = new System.Windows.Forms.TextBox();
            this.sendPasswordLinkLabel = new System.Windows.Forms.LinkLabel();
            this.cancelButton = new OdessaGUIProject.UI_Controls.PictureButtonControl();
            this.activateButton = new OdessaGUIProject.UI_Controls.PictureButtonControl();
            transLabel1 = new OdessaGUIProject.TransLabel();
            this.SuspendLayout();
            // 
            // transLabel1
            // 
            transLabel1.AutoSize = true;
            transLabel1.BackColor = System.Drawing.Color.Transparent;
            transLabel1.Font = new System.Drawing.Font("Open Sans", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            transLabel1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            transLabel1.Location = new System.Drawing.Point(23, 219);
            transLabel1.Name = "transLabel1";
            transLabel1.Size = new System.Drawing.Size(331, 18);
            transLabel1.TabIndex = 4;
            transLabel1.Text = "As a security precaution, please enter a new password.";
            // 
            // lblCode
            // 
            this.lblCode.AutoSize = true;
            this.lblCode.BackColor = System.Drawing.Color.Transparent;
            this.lblCode.Font = new System.Drawing.Font("Open Sans", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCode.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.lblCode.Location = new System.Drawing.Point(24, 44);
            this.lblCode.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblCode.Name = "lblCode";
            this.lblCode.Size = new System.Drawing.Size(39, 18);
            this.lblCode.TabIndex = 0;
            this.lblCode.Text = "code";
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Open Sans", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.label1.Location = new System.Drawing.Point(23, 74);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(410, 78);
            this.label1.TabIndex = 1;
            this.label1.Text = "This code has already been activated and must be re-activated on this machine. Pl" +
    "ease enter your current password to re-activate.";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("Open Sans", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.label4.Location = new System.Drawing.Point(23, 297);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(146, 18);
            this.label4.TabIndex = 7;
            this.label4.Text = "Confirm new Password";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Open Sans", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.label3.Location = new System.Drawing.Point(23, 265);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(174, 18);
            this.label3.TabIndex = 5;
            this.label3.Text = "New (never-used) Password";
            // 
            // tbConfPass
            // 
            this.tbConfPass.Font = new System.Drawing.Font("Open Sans", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbConfPass.Location = new System.Drawing.Point(230, 294);
            this.tbConfPass.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.tbConfPass.MaxLength = 16;
            this.tbConfPass.Name = "tbConfPass";
            this.tbConfPass.PasswordChar = '*';
            this.tbConfPass.Size = new System.Drawing.Size(199, 25);
            this.tbConfPass.TabIndex = 8;
            this.tbConfPass.UseSystemPasswordChar = true;
            // 
            // tbNewPass
            // 
            this.tbNewPass.Font = new System.Drawing.Font("Open Sans", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbNewPass.Location = new System.Drawing.Point(230, 261);
            this.tbNewPass.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.tbNewPass.MaxLength = 16;
            this.tbNewPass.Name = "tbNewPass";
            this.tbNewPass.PasswordChar = '*';
            this.tbNewPass.Size = new System.Drawing.Size(199, 25);
            this.tbNewPass.TabIndex = 6;
            this.tbNewPass.UseSystemPasswordChar = true;
            // 
            // tbCurrentPass
            // 
            this.tbCurrentPass.AllowDrop = true;
            this.tbCurrentPass.Font = new System.Drawing.Font("Open Sans", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbCurrentPass.Location = new System.Drawing.Point(26, 139);
            this.tbCurrentPass.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.tbCurrentPass.MaxLength = 16;
            this.tbCurrentPass.Name = "tbCurrentPass";
            this.tbCurrentPass.PasswordChar = '*';
            this.tbCurrentPass.Size = new System.Drawing.Size(199, 25);
            this.tbCurrentPass.TabIndex = 2;
            this.tbCurrentPass.UseSystemPasswordChar = true;
            // 
            // sendPasswordLinkLabel
            // 
            this.sendPasswordLinkLabel.ActiveLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(153)))), ((int)(((byte)(204)))));
            this.sendPasswordLinkLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.sendPasswordLinkLabel.AutoSize = true;
            this.sendPasswordLinkLabel.BackColor = System.Drawing.Color.Transparent;
            this.sendPasswordLinkLabel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.sendPasswordLinkLabel.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sendPasswordLinkLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(153)))), ((int)(((byte)(204)))));
            this.sendPasswordLinkLabel.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.sendPasswordLinkLabel.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(153)))), ((int)(((byte)(204)))));
            this.sendPasswordLinkLabel.Location = new System.Drawing.Point(248, 147);
            this.sendPasswordLinkLabel.Name = "sendPasswordLinkLabel";
            this.sendPasswordLinkLabel.Size = new System.Drawing.Size(106, 17);
            this.sendPasswordLinkLabel.TabIndex = 37;
            this.sendPasswordLinkLabel.TabStop = true;
            this.sendPasswordLinkLabel.Text = "Send Password...";
            this.sendPasswordLinkLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.sendPasswordLinkLabel.VisitedLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(153)))), ((int)(((byte)(204)))));
            this.sendPasswordLinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.sendPasswordLinkLabel_LinkClicked);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.AutoSize = true;
            this.cancelButton.DefaultImage = global::OdessaGUIProject.Properties.Resources.button_gray_cancel_normal;
            this.cancelButton.HoverImage = global::OdessaGUIProject.Properties.Resources.button_gray_cancel_hover;
            this.cancelButton.Location = new System.Drawing.Point(297, 354);
            this.cancelButton.MouseDownImage = global::OdessaGUIProject.Properties.Resources.button_gray_cancel_click;
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(192, 43);
            this.cancelButton.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.cancelButton.TabIndex = 43;
            this.cancelButton.Tooltip = null;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // activateButton
            // 
            this.activateButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.activateButton.AutoSize = true;
            this.activateButton.DefaultImage = global::OdessaGUIProject.Properties.Resources.button_green_activate_normal;
            this.activateButton.HoverImage = global::OdessaGUIProject.Properties.Resources.button_green_activate_hover;
            this.activateButton.Location = new System.Drawing.Point(99, 354);
            this.activateButton.MouseDownImage = global::OdessaGUIProject.Properties.Resources.button_green_activate_click;
            this.activateButton.Name = "activateButton";
            this.activateButton.Size = new System.Drawing.Size(192, 43);
            this.activateButton.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.activateButton.TabIndex = 42;
            this.activateButton.Tooltip = null;
            this.activateButton.Click += new System.EventHandler(this.btnReactivate_Click);
            // 
            // TFReactivation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.BackgroundImage = global::OdessaGUIProject.Properties.Resources.background_pattern_102x102_tile;
            this.ClientSize = new System.Drawing.Size(501, 409);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.activateButton);
            this.Controls.Add(this.sendPasswordLinkLabel);
            this.Controls.Add(transLabel1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tbConfPass);
            this.Controls.Add(this.tbNewPass);
            this.Controls.Add(this.tbCurrentPass);
            this.Controls.Add(this.lblCode);
            this.Controls.Add(this.label1);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Open Sans", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TFReactivation";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Activation";
            this.Load += new System.EventHandler(this.TFReactivation_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.TFReactivation_Paint);
            this.Resize += new System.EventHandler(this.TFReactivation_Resize);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal TransLabel lblCode;
        private TransLabel label1;
        private TransLabel label4;
        private TransLabel label3;
        internal System.Windows.Forms.TextBox tbConfPass;
        internal System.Windows.Forms.TextBox tbNewPass;
        internal System.Windows.Forms.TextBox tbCurrentPass;
        private System.Windows.Forms.LinkLabel sendPasswordLinkLabel;
        private UI_Controls.PictureButtonControl cancelButton;
        private UI_Controls.PictureButtonControl activateButton;
    }
}
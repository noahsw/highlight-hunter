

namespace OdessaGUIProject.DRM_Helpers
{
    sealed partial class TFActivation
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TFActivation));
            this.lblCode = new OdessaGUIProject.TransLabel();
            this.label2 = new OdessaGUIProject.TransLabel();
            this.tbPass = new System.Windows.Forms.TextBox();
            this.tbConfPass = new System.Windows.Forms.TextBox();
            this.label3 = new OdessaGUIProject.TransLabel();
            this.label4 = new OdessaGUIProject.TransLabel();
            this.label5 = new OdessaGUIProject.TransLabel();
            this.tbEmail = new System.Windows.Forms.TextBox();
            this.tbConfEmail = new System.Windows.Forms.TextBox();
            this.label6 = new OdessaGUIProject.TransLabel();
            this.label7 = new OdessaGUIProject.TransLabel();
            this.activateButton = new OdessaGUIProject.UI_Controls.PictureButtonControl();
            this.cancelButton = new OdessaGUIProject.UI_Controls.PictureButtonControl();
            this.SuspendLayout();
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
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Open Sans", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.label2.Location = new System.Drawing.Point(23, 75);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(447, 47);
            this.label2.TabIndex = 1;
            this.label2.Text = "Please provide a password and confirm. You will need this password if you need to" +
    " re-install the software.";
            // 
            // tbPass
            // 
            this.tbPass.Font = new System.Drawing.Font("Open Sans", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbPass.Location = new System.Drawing.Point(107, 131);
            this.tbPass.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.tbPass.MaxLength = 16;
            this.tbPass.Name = "tbPass";
            this.tbPass.PasswordChar = '*';
            this.tbPass.Size = new System.Drawing.Size(123, 25);
            this.tbPass.TabIndex = 3;
            this.tbPass.UseSystemPasswordChar = true;
            // 
            // tbConfPass
            // 
            this.tbConfPass.Font = new System.Drawing.Font("Open Sans", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbConfPass.Location = new System.Drawing.Point(322, 131);
            this.tbConfPass.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.tbConfPass.MaxLength = 16;
            this.tbConfPass.Name = "tbConfPass";
            this.tbConfPass.PasswordChar = '*';
            this.tbConfPass.Size = new System.Drawing.Size(123, 25);
            this.tbConfPass.TabIndex = 5;
            this.tbConfPass.UseSystemPasswordChar = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Open Sans", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.label3.Location = new System.Drawing.Point(24, 134);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(66, 18);
            this.label3.TabIndex = 2;
            this.label3.Text = "Password";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("Open Sans", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.label4.Location = new System.Drawing.Point(257, 134);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(57, 18);
            this.label4.TabIndex = 4;
            this.label4.Text = "Confirm";
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Font = new System.Drawing.Font("Open Sans", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.label5.Location = new System.Drawing.Point(23, 184);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(446, 42);
            this.label5.TabIndex = 6;
            this.label5.Text = "Enter an email address in case you ever lose your password.";
            // 
            // tbEmail
            // 
            this.tbEmail.Font = new System.Drawing.Font("Open Sans", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbEmail.Location = new System.Drawing.Point(106, 222);
            this.tbEmail.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.tbEmail.MaxLength = 75;
            this.tbEmail.Name = "tbEmail";
            this.tbEmail.Size = new System.Drawing.Size(186, 25);
            this.tbEmail.TabIndex = 8;
            // 
            // tbConfEmail
            // 
            this.tbConfEmail.Font = new System.Drawing.Font("Open Sans", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbConfEmail.Location = new System.Drawing.Point(106, 252);
            this.tbConfEmail.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.tbConfEmail.MaxLength = 75;
            this.tbConfEmail.Name = "tbConfEmail";
            this.tbConfEmail.Size = new System.Drawing.Size(186, 25);
            this.tbConfEmail.TabIndex = 10;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Font = new System.Drawing.Font("Open Sans", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.label6.Location = new System.Drawing.Point(23, 224);
            this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(40, 18);
            this.label6.TabIndex = 7;
            this.label6.Text = "Email";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.Font = new System.Drawing.Font("Open Sans", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.label7.Location = new System.Drawing.Point(23, 254);
            this.label7.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(57, 18);
            this.label7.TabIndex = 9;
            this.label7.Text = "Confirm";
            // 
            // activateButton
            // 
            this.activateButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.activateButton.AutoSize = true;
            this.activateButton.DefaultImage = global::OdessaGUIProject.Properties.Resources.button_green_activate_normal;
            this.activateButton.HoverImage = global::OdessaGUIProject.Properties.Resources.button_green_activate_hover;
            this.activateButton.Location = new System.Drawing.Point(86, 347);
            this.activateButton.MouseDownImage = global::OdessaGUIProject.Properties.Resources.button_green_activate_click;
            this.activateButton.Name = "activateButton";
            this.activateButton.Size = new System.Drawing.Size(192, 43);
            this.activateButton.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.activateButton.TabIndex = 40;
            this.activateButton.Tooltip = null;
            this.activateButton.Click += new System.EventHandler(this.activateButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.AutoSize = true;
            this.cancelButton.DefaultImage = global::OdessaGUIProject.Properties.Resources.button_gray_cancel_normal;
            this.cancelButton.HoverImage = global::OdessaGUIProject.Properties.Resources.button_gray_cancel_hover;
            this.cancelButton.Location = new System.Drawing.Point(284, 347);
            this.cancelButton.MouseDownImage = global::OdessaGUIProject.Properties.Resources.button_gray_cancel_click;
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(192, 43);
            this.cancelButton.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.cancelButton.TabIndex = 41;
            this.cancelButton.Tooltip = null;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // TFActivation
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.BackgroundImage = global::OdessaGUIProject.Properties.Resources.background_pattern_102x102_tile;
            this.ClientSize = new System.Drawing.Size(488, 402);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.activateButton);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.tbConfEmail);
            this.Controls.Add(this.tbEmail);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tbConfPass);
            this.Controls.Add(this.tbPass);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblCode);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TFActivation";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Activation";
            this.Load += new System.EventHandler(this.TFActivation_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.TFActivation_Paint);
            this.Resize += new System.EventHandler(this.TFActivation_Resize);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private TransLabel label2;
        private TransLabel label3;
        private TransLabel label4;
        private TransLabel label5;
        private TransLabel label6;
        private TransLabel label7;
        internal System.Windows.Forms.TextBox tbPass;
        internal System.Windows.Forms.TextBox tbConfPass;
        internal System.Windows.Forms.TextBox tbEmail;
        internal System.Windows.Forms.TextBox tbConfEmail;
        internal TransLabel lblCode;
        private UI_Controls.PictureButtonControl activateButton;
        private UI_Controls.PictureButtonControl cancelButton;
    }
}
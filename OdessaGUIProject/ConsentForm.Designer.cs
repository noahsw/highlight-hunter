namespace OdessaGUIProject
{
    sealed partial class ConsentForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConsentForm));
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.transLabel1 = new OdessaGUIProject.TransLabel();
            this.transLabel2 = new OdessaGUIProject.TransLabel();
            this.disagreeRadioButton = new OdessaGUIProject.CustomRadioButton();
            this.agreeRadioButton = new OdessaGUIProject.CustomRadioButton();
            this.continueButton = new OdessaGUIProject.UI_Controls.PictureButtonControl();
            this.SuspendLayout();
            // 
            // richTextBox1
            // 
            this.richTextBox1.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.richTextBox1.Location = new System.Drawing.Point(23, 161);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(462, 179);
            this.richTextBox1.TabIndex = 2;
            this.richTextBox1.Text = resources.GetString("richTextBox1.Text");
            // 
            // transLabel1
            // 
            this.transLabel1.AutoSize = true;
            this.transLabel1.BackColor = System.Drawing.Color.Transparent;
            this.transLabel1.Font = new System.Drawing.Font("Franklin Gothic Book", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.transLabel1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.transLabel1.Location = new System.Drawing.Point(19, 46);
            this.transLabel1.Name = "transLabel1";
            this.transLabel1.Size = new System.Drawing.Size(265, 24);
            this.transLabel1.TabIndex = 0;
            this.transLabel1.Text = "End User License Agreement";
            // 
            // transLabel2
            // 
            this.transLabel2.BackColor = System.Drawing.Color.Transparent;
            this.transLabel2.Font = new System.Drawing.Font("Franklin Gothic Book", 12F);
            this.transLabel2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.transLabel2.Location = new System.Drawing.Point(19, 87);
            this.transLabel2.Name = "transLabel2";
            this.transLabel2.Size = new System.Drawing.Size(461, 71);
            this.transLabel2.TabIndex = 1;
            this.transLabel2.Text = "Please take a moment to read the license agreement now. If you accept the terms b" +
    "elow, click \"I Agree\", then \"Continue\". Otherwise close this window.";
            // 
            // disagreeRadioButton
            // 
            this.disagreeRadioButton.BackColor = System.Drawing.Color.Transparent;
            this.disagreeRadioButton.Checked = true;
            this.disagreeRadioButton.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.disagreeRadioButton.Location = new System.Drawing.Point(23, 349);
            this.disagreeRadioButton.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.disagreeRadioButton.Name = "disagreeRadioButton";
            this.disagreeRadioButton.Size = new System.Drawing.Size(232, 34);
            this.disagreeRadioButton.TabIndex = 3;
            this.disagreeRadioButton.Text = "I Do Not Agree";
            // 
            // agreeRadioButton
            // 
            this.agreeRadioButton.BackColor = System.Drawing.Color.Transparent;
            this.agreeRadioButton.Checked = false;
            this.agreeRadioButton.Font = new System.Drawing.Font("Franklin Gothic Book", 14F);
            this.agreeRadioButton.Location = new System.Drawing.Point(23, 395);
            this.agreeRadioButton.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.agreeRadioButton.Name = "agreeRadioButton";
            this.agreeRadioButton.Size = new System.Drawing.Size(237, 34);
            this.agreeRadioButton.TabIndex = 4;
            this.agreeRadioButton.Text = "I Agree";
            // 
            // continueButton
            // 
            this.continueButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.continueButton.AutoSize = true;
            this.continueButton.DefaultImage = global::OdessaGUIProject.Properties.Resources.button_green_continue_normal;
            this.continueButton.HoverImage = global::OdessaGUIProject.Properties.Resources.button_green_continue_hover;
            this.continueButton.Location = new System.Drawing.Point(303, 445);
            this.continueButton.MouseDownImage = global::OdessaGUIProject.Properties.Resources.button_green_continue_click;
            this.continueButton.Name = "continueButton";
            this.continueButton.Size = new System.Drawing.Size(192, 43);
            this.continueButton.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.continueButton.TabIndex = 40;
            this.continueButton.Tooltip = null;
            this.continueButton.Click += new System.EventHandler(this.continueButton_Click);
            // 
            // ConsentForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.BackgroundImage = global::OdessaGUIProject.Properties.Resources.background_pattern_102x102_tile;
            this.ClientSize = new System.Drawing.Size(507, 500);
            this.Controls.Add(this.continueButton);
            this.Controls.Add(this.agreeRadioButton);
            this.Controls.Add(this.disagreeRadioButton);
            this.Controls.Add(this.transLabel2);
            this.Controls.Add(this.transLabel1);
            this.Controls.Add(this.richTextBox1);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ConsentForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Highlight Hunter - License Agreement";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ConsentForm_FormClosing);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.ConsentForm_Paint);
            this.Resize += new System.EventHandler(this.ConsentForm_Resize);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox richTextBox1;
        private TransLabel transLabel1;
        private TransLabel transLabel2;
        private CustomRadioButton disagreeRadioButton;
        private CustomRadioButton agreeRadioButton;
        private UI_Controls.PictureButtonControl continueButton;
    }
}


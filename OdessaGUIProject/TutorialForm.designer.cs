

namespace OdessaGUIProject
{
    sealed partial class TutorialForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TutorialForm));
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pictureBox4 = new System.Windows.Forms.PictureBox();
            this.likeThisPicture = new System.Windows.Forms.PictureBox();
            this.notLikeThisPicture = new System.Windows.Forms.PictureBox();
            this.continueButton = new OdessaGUIProject.UI_Controls.PictureButtonControl();
            this.coverLikeThisLabel = new OdessaGUIProject.TransLabel();
            this.notLikeThisLabel = new OdessaGUIProject.TransLabel();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.likeThisPicture)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.notLikeThisPicture)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.Image = global::OdessaGUIProject.Properties.Resources.tutorial_welcome_bold_text;
            this.pictureBox1.Location = new System.Drawing.Point(29, 106);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(547, 65);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // pictureBox4
            // 
            this.pictureBox4.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox4.Image = global::OdessaGUIProject.Properties.Resources.tutorial_welcome_subhead_text;
            this.pictureBox4.Location = new System.Drawing.Point(29, 211);
            this.pictureBox4.Name = "pictureBox4";
            this.pictureBox4.Size = new System.Drawing.Size(508, 85);
            this.pictureBox4.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox4.TabIndex = 3;
            this.pictureBox4.TabStop = false;
            // 
            // likeThisPicture
            // 
            this.likeThisPicture.BackColor = System.Drawing.Color.Transparent;
            this.likeThisPicture.Cursor = System.Windows.Forms.Cursors.Hand;
            this.likeThisPicture.Image = global::OdessaGUIProject.Properties.Resources.tutorial_likethis_110x110;
            this.likeThisPicture.Location = new System.Drawing.Point(683, 61);
            this.likeThisPicture.Name = "likeThisPicture";
            this.likeThisPicture.Size = new System.Drawing.Size(110, 110);
            this.likeThisPicture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.likeThisPicture.TabIndex = 8;
            this.likeThisPicture.TabStop = false;
            this.likeThisPicture.Click += new System.EventHandler(this.likeThisPicture_Click);
            // 
            // notLikeThisPicture
            // 
            this.notLikeThisPicture.BackColor = System.Drawing.Color.Transparent;
            this.notLikeThisPicture.Cursor = System.Windows.Forms.Cursors.Hand;
            this.notLikeThisPicture.Image = global::OdessaGUIProject.Properties.Resources.tutorial_notlikethis_110x110;
            this.notLikeThisPicture.Location = new System.Drawing.Point(683, 211);
            this.notLikeThisPicture.Name = "notLikeThisPicture";
            this.notLikeThisPicture.Size = new System.Drawing.Size(110, 110);
            this.notLikeThisPicture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.notLikeThisPicture.TabIndex = 9;
            this.notLikeThisPicture.TabStop = false;
            this.notLikeThisPicture.Click += new System.EventHandler(this.notLikeThisPicture_Click);
            // 
            // continueButton
            // 
            this.continueButton.AutoSize = true;
            this.continueButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.continueButton.DefaultImage = global::OdessaGUIProject.Properties.Resources.button_green_starttour_normal;
            this.continueButton.HoverImage = global::OdessaGUIProject.Properties.Resources.button_green_starttour_hover;
            this.continueButton.Location = new System.Drawing.Point(601, 380);
            this.continueButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.continueButton.MouseDownImage = global::OdessaGUIProject.Properties.Resources.button_green_starttour_click;
            this.continueButton.Name = "continueButton";
            this.continueButton.Size = new System.Drawing.Size(196, 46);
            this.continueButton.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.continueButton.TabIndex = 6;
            this.continueButton.Tooltip = null;
            this.continueButton.Click += new System.EventHandler(this.continueButton_Click);
            // 
            // coverLikeThisLabel
            // 
            this.coverLikeThisLabel.BackColor = System.Drawing.Color.Transparent;
            this.coverLikeThisLabel.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.coverLikeThisLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(153)))), ((int)(((byte)(153)))));
            this.coverLikeThisLabel.Location = new System.Drawing.Point(683, 172);
            this.coverLikeThisLabel.Name = "coverLikeThisLabel";
            this.coverLikeThisLabel.Size = new System.Drawing.Size(110, 23);
            this.coverLikeThisLabel.TabIndex = 10;
            this.coverLikeThisLabel.Text = "Cover like this";
            this.coverLikeThisLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // notLikeThisLabel
            // 
            this.notLikeThisLabel.BackColor = System.Drawing.Color.Transparent;
            this.notLikeThisLabel.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.notLikeThisLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(153)))), ((int)(((byte)(153)))));
            this.notLikeThisLabel.Location = new System.Drawing.Point(683, 322);
            this.notLikeThisLabel.Name = "notLikeThisLabel";
            this.notLikeThisLabel.Size = new System.Drawing.Size(110, 23);
            this.notLikeThisLabel.TabIndex = 11;
            this.notLikeThisLabel.Text = "not like this";
            this.notLikeThisLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // TutorialForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.BackgroundImage = global::OdessaGUIProject.Properties.Resources.background_pattern_102x102_tile;
            this.ClientSize = new System.Drawing.Size(819, 451);
            this.Controls.Add(this.notLikeThisLabel);
            this.Controls.Add(this.coverLikeThisLabel);
            this.Controls.Add(this.notLikeThisPicture);
            this.Controls.Add(this.likeThisPicture);
            this.Controls.Add(this.continueButton);
            this.Controls.Add(this.pictureBox4);
            this.Controls.Add(this.pictureBox1);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Franklin Gothic Book", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.Name = "TutorialForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "How to use Highlight Hunter";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.TutorialForm_FormClosed);
            this.Load += new System.EventHandler(this.TutorialForm_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.TutorialForm_Paint);
            this.Resize += new System.EventHandler(this.TutorialForm_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.likeThisPicture)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.notLikeThisPicture)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureBox4;
        private UI_Controls.PictureButtonControl continueButton;
        private System.Windows.Forms.PictureBox likeThisPicture;
        private System.Windows.Forms.PictureBox notLikeThisPicture;
        private TransLabel coverLikeThisLabel;
        private TransLabel notLikeThisLabel;

    }
}
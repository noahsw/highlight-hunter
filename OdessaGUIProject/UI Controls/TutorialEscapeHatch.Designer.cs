namespace OdessaGUIProject.UI_Controls
{
    partial class TutorialEscapeHatch
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
            this.exitTourButton = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.progressLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.exitTourButton)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // exitTourButton
            // 
            this.exitTourButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.exitTourButton.BackgroundImage = global::OdessaGUIProject.Properties.Resources.tutorial_escape_hatch_1px_horizontal_tile;
            this.exitTourButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.exitTourButton.Image = global::OdessaGUIProject.Properties.Resources.tutorial_escape_hatch_exit_tour_text;
            this.exitTourButton.Location = new System.Drawing.Point(537, 0);
            this.exitTourButton.Name = "exitTourButton";
            this.exitTourButton.Size = new System.Drawing.Size(118, 40);
            this.exitTourButton.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.exitTourButton.TabIndex = 1;
            this.exitTourButton.TabStop = false;
            this.exitTourButton.Click += new System.EventHandler(this.exitTourButton_Click);
            // 
            // pictureBox2
            // 
            this.pictureBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox2.Image = global::OdessaGUIProject.Properties.Resources.tutorial_escape_hatch_1px_vertical_divider;
            this.pictureBox2.Location = new System.Drawing.Point(533, 0);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(2, 40);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox2.TabIndex = 2;
            this.pictureBox2.TabStop = false;
            // 
            // progressLabel
            // 
            this.progressLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.progressLabel.BackColor = System.Drawing.Color.Transparent;
            this.progressLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.progressLabel.ForeColor = System.Drawing.Color.White;
            this.progressLabel.Location = new System.Drawing.Point(7, 0);
            this.progressLabel.Name = "progressLabel";
            this.progressLabel.Size = new System.Drawing.Size(373, 40);
            this.progressLabel.TabIndex = 3;
            this.progressLabel.Text = "You are taking the Highlight Hunter tour (step 1 of 8)";
            this.progressLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // TutorialEscapeHatch
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.BackgroundImage = global::OdessaGUIProject.Properties.Resources.tutorial_escape_hatch_1px_horizontal_tile;
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.exitTourButton);
            this.Controls.Add(this.progressLabel);
            this.Name = "TutorialEscapeHatch";
            this.Size = new System.Drawing.Size(655, 40);
            this.Load += new System.EventHandler(this.TutorialEscapeHatch_Load);
            ((System.ComponentModel.ISupportInitialize)(this.exitTourButton)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox exitTourButton;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Label progressLabel;


    }
}

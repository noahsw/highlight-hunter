using System.Drawing;

namespace OdessaGUIProject.UI_Controls
{
    sealed partial class TutorialBubble
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
            this.tooltipImage = new System.Windows.Forms.PictureBox();
            this.bubbleLabel = new OdessaGUIProject.TransLabel();
            this.nextButton = new OdessaGUIProject.UI_Controls.PictureButtonControl();
            ((System.ComponentModel.ISupportInitialize)(this.tooltipImage)).BeginInit();
            this.SuspendLayout();
            // 
            // tooltipImage
            // 
            this.tooltipImage.Image = global::OdessaGUIProject.Properties.Resources.tooltip_large_left;
            this.tooltipImage.Location = new System.Drawing.Point(0, 0);
            this.tooltipImage.Name = "tooltipImage";
            this.tooltipImage.Size = new System.Drawing.Size(203, 184);
            this.tooltipImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.tooltipImage.TabIndex = 0;
            this.tooltipImage.TabStop = false;
            // 
            // bubbleLabel
            // 
            this.bubbleLabel.BackColor = System.Drawing.Color.Transparent;
            this.bubbleLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bubbleLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(85)))), ((int)(((byte)(85)))), ((int)(((byte)(85)))));
            this.bubbleLabel.Location = new System.Drawing.Point(19, 10);
            this.bubbleLabel.Name = "bubbleLabel";
            this.bubbleLabel.Size = new System.Drawing.Size(172, 136);
            this.bubbleLabel.TabIndex = 2;
            this.bubbleLabel.Text = "Click \"Scan All Videos\" to scan the sample video for highlights.\r\n\r\nIf this were " +
    "your own footage, every video you dropped in would be scanned.";
            // 
            // nextButton
            // 
            this.nextButton.AutoSize = true;
            this.nextButton.DefaultImage = global::OdessaGUIProject.Properties.Resources.next_button_normal;
            this.nextButton.HoverImage = global::OdessaGUIProject.Properties.Resources.next_button_hover;
            this.nextButton.Location = new System.Drawing.Point(73, 136);
            this.nextButton.MouseDownImage = global::OdessaGUIProject.Properties.Resources.next_button_click;
            this.nextButton.Name = "nextButton";
            this.nextButton.Size = new System.Drawing.Size(53, 23);
            this.nextButton.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Normal;
            this.nextButton.TabIndex = 1;
            this.nextButton.Tooltip = null;
            this.nextButton.Click += new System.EventHandler(this.nextButton_Click);
            // 
            // TutorialBubble
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.nextButton);
            this.Controls.Add(this.bubbleLabel);
            this.Controls.Add(this.tooltipImage);
            this.DoubleBuffered = true;
            this.Name = "TutorialBubble";
            this.Size = new System.Drawing.Size(245, 238);
            this.Load += new System.EventHandler(this.TutorialBubble_Load);
            ((System.ComponentModel.ISupportInitialize)(this.tooltipImage)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox tooltipImage;
        private PictureButtonControl nextButton;
        private TransLabel bubbleLabel;
    }
}

namespace OdessaGUIProject.UI_Controls
{
    partial class ScanControl
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
            this.components = new System.ComponentModel.Container();
            this.updateProgressTimer = new System.Windows.Forms.Timer(this.components);
            this.timeRemainingLabel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.statusLabel = new System.Windows.Forms.Label();
            this.formProgressBar = new System.Windows.Forms.ProgressBar();
            this.panel = new System.Windows.Forms.Panel();
            this.cancelButton = new OdessaGUIProject.UI_Controls.PictureButtonControl();
            this.panel.SuspendLayout();
            this.SuspendLayout();
            // 
            // updateProgressTimer
            // 
            this.updateProgressTimer.Interval = 500;
            this.updateProgressTimer.Tick += new System.EventHandler(this.updateProgressTimer_Tick);
            // 
            // timeRemainingLabel
            // 
            this.timeRemainingLabel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.timeRemainingLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(85)))), ((int)(((byte)(85)))), ((int)(((byte)(85)))));
            this.timeRemainingLabel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.timeRemainingLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.timeRemainingLabel.Location = new System.Drawing.Point(807, 306);
            this.timeRemainingLabel.Name = "timeRemainingLabel";
            this.timeRemainingLabel.Size = new System.Drawing.Size(251, 26);
            this.timeRemainingLabel.TabIndex = 3;
            this.timeRemainingLabel.Text = "3min left";
            this.timeRemainingLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(85)))), ((int)(((byte)(85)))), ((int)(((byte)(85)))));
            this.label1.Font = new System.Drawing.Font("Segoe UI", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.label1.Location = new System.Drawing.Point(169, 190);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(506, 37);
            this.label1.TabIndex = 0;
            this.label1.Text = "Sit back while we hunt for highlights...";
            // 
            // statusLabel
            // 
            this.statusLabel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.statusLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(85)))), ((int)(((byte)(85)))), ((int)(((byte)(85)))));
            this.statusLabel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.statusLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.statusLabel.Location = new System.Drawing.Point(171, 306);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(583, 26);
            this.statusLabel.TabIndex = 2;
            this.statusLabel.Text = "Starting...";
            // 
            // formProgressBar
            // 
            this.formProgressBar.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.formProgressBar.Location = new System.Drawing.Point(175, 257);
            this.formProgressBar.Name = "formProgressBar";
            this.formProgressBar.Size = new System.Drawing.Size(883, 36);
            this.formProgressBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.formProgressBar.TabIndex = 1;
            // 
            // panel
            // 
            this.panel.AllowDrop = true;
            this.panel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel.AutoScroll = true;
            this.panel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(84)))), ((int)(((byte)(84)))));
            this.panel.Controls.Add(this.formProgressBar);
            this.panel.Controls.Add(this.statusLabel);
            this.panel.Controls.Add(this.label1);
            this.panel.Controls.Add(this.timeRemainingLabel);
            this.panel.Cursor = System.Windows.Forms.Cursors.Default;
            this.panel.Location = new System.Drawing.Point(0, 0);
            this.panel.Name = "panel";
            this.panel.Size = new System.Drawing.Size(1243, 572);
            this.panel.TabIndex = 0;
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cancelButton.AutoSize = true;
            this.cancelButton.DefaultImage = global::OdessaGUIProject.Properties.Resources.button_gray_goback_normal;
            this.cancelButton.HoverImage = global::OdessaGUIProject.Properties.Resources.button_gray_goback_hover;
            this.cancelButton.Location = new System.Drawing.Point(3, 587);
            this.cancelButton.MouseDownImage = global::OdessaGUIProject.Properties.Resources.button_gray_goback_click;
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(192, 43);
            this.cancelButton.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Normal;
            this.cancelButton.TabIndex = 1;
            this.cancelButton.Tooltip = null;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // ScanControl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.panel);
            this.Font = new System.Drawing.Font("Franklin Gothic Book", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "ScanControl";
            this.Size = new System.Drawing.Size(1243, 633);
            this.Load += new System.EventHandler(this.ScanControl_Load);
            this.panel.ResumeLayout(false);
            this.panel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer updateProgressTimer;
        private System.Windows.Forms.Label timeRemainingLabel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label statusLabel;
        private System.Windows.Forms.ProgressBar formProgressBar;
        private System.Windows.Forms.Panel panel;
        private PictureButtonControl cancelButton;
    }
}

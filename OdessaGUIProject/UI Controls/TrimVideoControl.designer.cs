using OdessaPCControls;
namespace OdessaGUIProject.UI_Controls
{
    partial class TrimVideoControl
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
            this.currentPositionLabel = new System.Windows.Forms.Label();
            this.playerStatusTimer = new System.Windows.Forms.Timer(this.components);
            this.tickBoxLocationsTimer = new System.Windows.Forms.Timer(this.components);
            this.playHeadBox = new System.Windows.Forms.PictureBox();
            this.playStatePicture = new System.Windows.Forms.PictureBox();
            this.endTickBox = new System.Windows.Forms.PictureBox();
            this.startTickBox = new System.Windows.Forms.PictureBox();
            this.timelineBox = new System.Windows.Forms.PictureBox();
            this.shareToFacebookCheckBox = new System.Windows.Forms.CheckBox();
            this.saveToDiskCheckBox = new System.Windows.Forms.CheckBox();
            this.titleTextBox = new System.Windows.Forms.TextBox();
            this.facebookPicture = new System.Windows.Forms.PictureBox();
            this.savePicture = new System.Windows.Forms.PictureBox();
            this.saveToDiskButton = new OdessaPCControls.TransLabelButton();
            this.transLabel1 = new OdessaPCControls.TransLabel();
            this.shareToFacebookButton = new OdessaPCControls.TransLabelButton();
            this.transLabel2 = new OdessaPCControls.TransLabel();
            ((System.ComponentModel.ISupportInitialize)(this.playHeadBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.playStatePicture)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.endTickBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.startTickBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.timelineBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.facebookPicture)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.savePicture)).BeginInit();
            this.SuspendLayout();
            // 
            // currentPositionLabel
            // 
            this.currentPositionLabel.AutoSize = true;
            this.currentPositionLabel.Font = new System.Drawing.Font("Franklin Gothic Book", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.currentPositionLabel.ForeColor = System.Drawing.Color.White;
            this.currentPositionLabel.Location = new System.Drawing.Point(558, 387);
            this.currentPositionLabel.Name = "currentPositionLabel";
            this.currentPositionLabel.Size = new System.Drawing.Size(90, 17);
            this.currentPositionLabel.TabIndex = 45;
            this.currentPositionLabel.Text = "00:00 / 00:00";
            this.currentPositionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // playerStatusTimer
            // 
            this.playerStatusTimer.Interval = 200;
            this.playerStatusTimer.Tick += new System.EventHandler(this.playerStatusTimer_Tick);
            // 
            // tickBoxLocationsTimer
            // 
            this.tickBoxLocationsTimer.Interval = 200;
            this.tickBoxLocationsTimer.Tick += new System.EventHandler(this.tickBoxLocationsTimer_Tick);
            // 
            // playHeadBox
            // 
            this.playHeadBox.BackColor = System.Drawing.Color.Transparent;
            this.playHeadBox.Image = global::OdessaGUIProject.Properties.Resources.playhead;
            this.playHeadBox.Location = new System.Drawing.Point(208, 383);
            this.playHeadBox.Name = "playHeadBox";
            this.playHeadBox.Size = new System.Drawing.Size(7, 27);
            this.playHeadBox.TabIndex = 49;
            this.playHeadBox.TabStop = false;
            this.playHeadBox.Visible = false;
            this.playHeadBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.playHeadBox_MouseDown);
            this.playHeadBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.playHeadBox_MouseMove);
            this.playHeadBox.MouseUp += new System.Windows.Forms.MouseEventHandler(this.playHeadBox_MouseUp);
            // 
            // playStatePicture
            // 
            this.playStatePicture.BackColor = System.Drawing.Color.Transparent;
            this.playStatePicture.Image = global::OdessaGUIProject.Properties.Resources.pause;
            this.playStatePicture.Location = new System.Drawing.Point(12, 380);
            this.playStatePicture.Name = "playStatePicture";
            this.playStatePicture.Size = new System.Drawing.Size(30, 31);
            this.playStatePicture.TabIndex = 50;
            this.playStatePicture.TabStop = false;
            this.playStatePicture.Text = "transPicture1";
            this.playStatePicture.Click += new System.EventHandler(this.playStatePicture_Click);
            // 
            // endTickBox
            // 
            this.endTickBox.BackColor = System.Drawing.Color.Transparent;
            this.endTickBox.Image = global::OdessaGUIProject.Properties.Resources.right_handle;
            this.endTickBox.Location = new System.Drawing.Point(276, 375);
            this.endTickBox.Name = "endTickBox";
            this.endTickBox.Size = new System.Drawing.Size(16, 44);
            this.endTickBox.TabIndex = 47;
            this.endTickBox.TabStop = false;
            this.endTickBox.Visible = false;
            this.endTickBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.endTickBox_MouseDown);
            this.endTickBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.endTickBox_MouseMove);
            this.endTickBox.MouseUp += new System.Windows.Forms.MouseEventHandler(this.endTickBox_MouseUp);
            // 
            // startTickBox
            // 
            this.startTickBox.BackColor = System.Drawing.Color.Transparent;
            this.startTickBox.Image = global::OdessaGUIProject.Properties.Resources.left_handle;
            this.startTickBox.Location = new System.Drawing.Point(111, 375);
            this.startTickBox.Name = "startTickBox";
            this.startTickBox.Size = new System.Drawing.Size(16, 44);
            this.startTickBox.TabIndex = 46;
            this.startTickBox.TabStop = false;
            this.startTickBox.Visible = false;
            this.startTickBox.DoubleClick += new System.EventHandler(this.startTickBox_DoubleClick);
            this.startTickBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.startTickBox_MouseDown);
            this.startTickBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.startTickBox_MouseMove);
            this.startTickBox.MouseUp += new System.Windows.Forms.MouseEventHandler(this.startTickBox_MouseUp);
            // 
            // timelineBox
            // 
            this.timelineBox.BackColor = System.Drawing.Color.Transparent;
            this.timelineBox.Image = global::OdessaGUIProject.Properties.Resources.timeline;
            this.timelineBox.Location = new System.Drawing.Point(55, 391);
            this.timelineBox.Name = "timelineBox";
            this.timelineBox.Size = new System.Drawing.Size(460, 10);
            this.timelineBox.TabIndex = 48;
            this.timelineBox.TabStop = false;
            this.timelineBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.timelineBox_MouseDown);
            // 
            // shareToFacebookCheckBox
            // 
            this.shareToFacebookCheckBox.AutoSize = true;
            this.shareToFacebookCheckBox.Font = new System.Drawing.Font("Franklin Gothic Book", 12F);
            this.shareToFacebookCheckBox.ForeColor = System.Drawing.Color.White;
            this.shareToFacebookCheckBox.Location = new System.Drawing.Point(581, 250);
            this.shareToFacebookCheckBox.Name = "shareToFacebookCheckBox";
            this.shareToFacebookCheckBox.Size = new System.Drawing.Size(153, 25);
            this.shareToFacebookCheckBox.TabIndex = 51;
            this.shareToFacebookCheckBox.Text = "Share to Facebook";
            this.shareToFacebookCheckBox.UseVisualStyleBackColor = true;
            this.shareToFacebookCheckBox.Visible = false;
            this.shareToFacebookCheckBox.CheckedChanged += new System.EventHandler(this.shareToFacebookCheckBox_CheckedChanged);
            // 
            // saveToDiskCheckBox
            // 
            this.saveToDiskCheckBox.AutoSize = true;
            this.saveToDiskCheckBox.Checked = true;
            this.saveToDiskCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.saveToDiskCheckBox.Font = new System.Drawing.Font("Franklin Gothic Book", 12F);
            this.saveToDiskCheckBox.ForeColor = System.Drawing.Color.White;
            this.saveToDiskCheckBox.Location = new System.Drawing.Point(581, 184);
            this.saveToDiskCheckBox.Name = "saveToDiskCheckBox";
            this.saveToDiskCheckBox.Size = new System.Drawing.Size(148, 25);
            this.saveToDiskCheckBox.TabIndex = 52;
            this.saveToDiskCheckBox.Text = "Save to Computer";
            this.saveToDiskCheckBox.UseVisualStyleBackColor = true;
            this.saveToDiskCheckBox.Visible = false;
            this.saveToDiskCheckBox.CheckedChanged += new System.EventHandler(this.saveToDiskCheckBox_CheckedChanged);
            // 
            // titleTextBox
            // 
            this.titleTextBox.Font = new System.Drawing.Font("Franklin Gothic Book", 12F);
            this.titleTextBox.Location = new System.Drawing.Point(57, 432);
            this.titleTextBox.Name = "titleTextBox";
            this.titleTextBox.Size = new System.Drawing.Size(263, 26);
            this.titleTextBox.TabIndex = 53;
            this.titleTextBox.TextChanged += new System.EventHandler(this.titleTextBox_TextChanged);
            this.titleTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.titleTextBox_KeyPress);
            this.titleTextBox.Leave += new System.EventHandler(this.titleTextBox_Leave);
            // 
            // facebookPicture
            // 
            this.facebookPicture.BackColor = System.Drawing.Color.Transparent;
            this.facebookPicture.Image = global::OdessaGUIProject.Properties.Resources.fb16x16;
            this.facebookPicture.Location = new System.Drawing.Point(499, 437);
            this.facebookPicture.Name = "facebookPicture";
            this.facebookPicture.Size = new System.Drawing.Size(16, 16);
            this.facebookPicture.TabIndex = 55;
            this.facebookPicture.TabStop = false;
            this.facebookPicture.Text = "transPicture2";
            // 
            // savePicture
            // 
            this.savePicture.BackColor = System.Drawing.Color.Transparent;
            this.savePicture.Image = global::OdessaGUIProject.Properties.Resources.save16x16;
            this.savePicture.Location = new System.Drawing.Point(329, 437);
            this.savePicture.Name = "savePicture";
            this.savePicture.Size = new System.Drawing.Size(16, 16);
            this.savePicture.TabIndex = 57;
            this.savePicture.TabStop = false;
            this.savePicture.Text = "transPicture2";
            // 
            // saveToDiskButton
            // 
            this.saveToDiskButton.AutoSize = true;
            this.saveToDiskButton.BackColor = System.Drawing.Color.Transparent;
            this.saveToDiskButton.FlatAppearance.BorderSize = 0;
            this.saveToDiskButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.saveToDiskButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.saveToDiskButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.saveToDiskButton.Font = new System.Drawing.Font("Franklin Gothic Book", 12F);
            this.saveToDiskButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(204)))), ((int)(((byte)(51)))));
            this.saveToDiskButton.IsClicked = false;
            this.saveToDiskButton.IsHovering = false;
            this.saveToDiskButton.Location = new System.Drawing.Point(347, 430);
            this.saveToDiskButton.Name = "saveToDiskButton";
            this.saveToDiskButton.Size = new System.Drawing.Size(137, 31);
            this.saveToDiskButton.TabIndex = 58;
            this.saveToDiskButton.Text = "Save to computer";
            this.saveToDiskButton.UseVisualStyleBackColor = false;
            this.saveToDiskButton.Click += new System.EventHandler(this.saveToDiskButton_Click);
            // 
            // transLabel1
            // 
            this.transLabel1.AutoSize = true;
            this.transLabel1.BackColor = System.Drawing.Color.Transparent;
            this.transLabel1.Font = new System.Drawing.Font("Franklin Gothic Book", 12F);
            this.transLabel1.ForeColor = System.Drawing.Color.White;
            this.transLabel1.Location = new System.Drawing.Point(8, 435);
            this.transLabel1.Name = "transLabel1";
            this.transLabel1.Size = new System.Drawing.Size(43, 21);
            this.transLabel1.TabIndex = 54;
            this.transLabel1.Text = "Title:";
            // 
            // shareToFacebookButton
            // 
            this.shareToFacebookButton.AutoSize = true;
            this.shareToFacebookButton.BackColor = System.Drawing.Color.Transparent;
            this.shareToFacebookButton.FlatAppearance.BorderSize = 0;
            this.shareToFacebookButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.shareToFacebookButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.shareToFacebookButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.shareToFacebookButton.Font = new System.Drawing.Font("Franklin Gothic Book", 12F);
            this.shareToFacebookButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(204)))), ((int)(((byte)(51)))));
            this.shareToFacebookButton.IsClicked = false;
            this.shareToFacebookButton.IsHovering = false;
            this.shareToFacebookButton.Location = new System.Drawing.Point(515, 430);
            this.shareToFacebookButton.Name = "shareToFacebookButton";
            this.shareToFacebookButton.Size = new System.Drawing.Size(144, 31);
            this.shareToFacebookButton.TabIndex = 56;
            this.shareToFacebookButton.Text = "Share to Facebook";
            this.shareToFacebookButton.UseVisualStyleBackColor = false;
            this.shareToFacebookButton.Click += new System.EventHandler(this.shareToFacebookButton_Click);
            // 
            // transLabel2
            // 
            this.transLabel2.AutoSize = true;
            this.transLabel2.BackColor = System.Drawing.Color.Transparent;
            this.transLabel2.Font = new System.Drawing.Font("Franklin Gothic Book", 12F);
            this.transLabel2.ForeColor = System.Drawing.Color.White;
            this.transLabel2.Location = new System.Drawing.Point(124, 184);
            this.transLabel2.Name = "transLabel2";
            this.transLabel2.Size = new System.Drawing.Size(90, 21);
            this.transLabel2.TabIndex = 60;
            this.transLabel2.Text = "transLabel2";
            // 
            // TrimVideoControl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.Controls.Add(this.transLabel2);
            this.Controls.Add(this.saveToDiskButton);
            this.Controls.Add(this.savePicture);
            this.Controls.Add(this.facebookPicture);
            this.Controls.Add(this.transLabel1);
            this.Controls.Add(this.titleTextBox);
            this.Controls.Add(this.saveToDiskCheckBox);
            this.Controls.Add(this.shareToFacebookCheckBox);
            this.Controls.Add(this.playHeadBox);
            this.Controls.Add(this.playStatePicture);
            this.Controls.Add(this.endTickBox);
            this.Controls.Add(this.startTickBox);
            this.Controls.Add(this.currentPositionLabel);
            this.Controls.Add(this.timelineBox);
            this.Controls.Add(this.shareToFacebookButton);
            this.Name = "TrimVideoControl";
            this.Size = new System.Drawing.Size(662, 475);
            this.Load += new System.EventHandler(this.TrimVideoControl_Load);
            ((System.ComponentModel.ISupportInitialize)(this.playHeadBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.playStatePicture)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.endTickBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.startTickBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.timelineBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.facebookPicture)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.savePicture)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox playHeadBox;
        private System.Windows.Forms.PictureBox endTickBox;
        private System.Windows.Forms.PictureBox startTickBox;
        private System.Windows.Forms.Label currentPositionLabel;
        private System.Windows.Forms.PictureBox timelineBox;
        private System.Windows.Forms.Timer playerStatusTimer;
        private System.Windows.Forms.Timer tickBoxLocationsTimer;
        private System.Windows.Forms.PictureBox playStatePicture;
        private System.Windows.Forms.CheckBox shareToFacebookCheckBox;
        private System.Windows.Forms.CheckBox saveToDiskCheckBox;
        private System.Windows.Forms.TextBox titleTextBox;
        private TransLabel transLabel1;
        private System.Windows.Forms.PictureBox facebookPicture;
        private TransLabelButton shareToFacebookButton;
        private System.Windows.Forms.PictureBox savePicture;
        private TransLabelButton saveToDiskButton;
        private TransLabel transLabel2;
    }
}

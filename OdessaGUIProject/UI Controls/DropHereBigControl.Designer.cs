namespace OdessaGUIProject.UI_Controls
{
    partial class DropHereBigControl
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
            this.dropPictureBox = new System.Windows.Forms.PictureBox();
            this.browseComputerPictureButtonControl = new OdessaGUIProject.UI_Controls.PictureButtonControl();
            this.selectFromCameraPictureButtonControl = new OdessaGUIProject.UI_Controls.PictureButtonControl();
            ((System.ComponentModel.ISupportInitialize)(this.dropPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // dropPictureBox
            // 
            this.dropPictureBox.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.dropPictureBox.Image = global::OdessaGUIProject.Properties.Resources.select_droptarget_large;
            this.dropPictureBox.Location = new System.Drawing.Point(0, 0);
            this.dropPictureBox.Name = "dropPictureBox";
            this.dropPictureBox.Size = new System.Drawing.Size(436, 224);
            this.dropPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.dropPictureBox.TabIndex = 1;
            this.dropPictureBox.TabStop = false;
            // 
            // browseComputerPictureButtonControl
            // 
            this.browseComputerPictureButtonControl.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.browseComputerPictureButtonControl.AutoSize = true;
            this.browseComputerPictureButtonControl.DefaultImage = global::OdessaGUIProject.Properties.Resources.select_button_browse;
            this.browseComputerPictureButtonControl.HoverImage = global::OdessaGUIProject.Properties.Resources.select_button_browse_hover;
            this.browseComputerPictureButtonControl.Location = new System.Drawing.Point(129, 299);
            this.browseComputerPictureButtonControl.MouseDownImage = global::OdessaGUIProject.Properties.Resources.select_button_browse;
            this.browseComputerPictureButtonControl.Name = "browseComputerPictureButtonControl";
            this.browseComputerPictureButtonControl.Size = new System.Drawing.Size(177, 32);
            this.browseComputerPictureButtonControl.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.browseComputerPictureButtonControl.TabIndex = 1;
            this.browseComputerPictureButtonControl.Tooltip = null;
            this.browseComputerPictureButtonControl.Click += new System.EventHandler(this.browseComputerPictureButtonControl_Click);
            // 
            // selectFromCameraPictureButtonControl
            // 
            this.selectFromCameraPictureButtonControl.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.selectFromCameraPictureButtonControl.AutoSize = true;
            this.selectFromCameraPictureButtonControl.DefaultImage = global::OdessaGUIProject.Properties.Resources.select_button_camera;
            this.selectFromCameraPictureButtonControl.HoverImage = global::OdessaGUIProject.Properties.Resources.select_button_camera_hover;
            this.selectFromCameraPictureButtonControl.Location = new System.Drawing.Point(129, 257);
            this.selectFromCameraPictureButtonControl.MouseDownImage = global::OdessaGUIProject.Properties.Resources.select_button_camera;
            this.selectFromCameraPictureButtonControl.Name = "selectFromCameraPictureButtonControl";
            this.selectFromCameraPictureButtonControl.Size = new System.Drawing.Size(177, 32);
            this.selectFromCameraPictureButtonControl.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.selectFromCameraPictureButtonControl.TabIndex = 0;
            this.selectFromCameraPictureButtonControl.Tooltip = null;
            this.selectFromCameraPictureButtonControl.Click += new System.EventHandler(this.selectFromCameraPictureButtonControl_Click);
            // 
            // DropHereBigControl
            // 
            this.AllowDrop = true;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(85)))), ((int)(((byte)(85)))), ((int)(((byte)(85)))));
            this.Controls.Add(this.browseComputerPictureButtonControl);
            this.Controls.Add(this.selectFromCameraPictureButtonControl);
            this.Controls.Add(this.dropPictureBox);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "DropHereBigControl";
            this.Size = new System.Drawing.Size(436, 338);
            ((System.ComponentModel.ISupportInitialize)(this.dropPictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox dropPictureBox;
        private PictureButtonControl selectFromCameraPictureButtonControl;
        private PictureButtonControl browseComputerPictureButtonControl;
    }
}

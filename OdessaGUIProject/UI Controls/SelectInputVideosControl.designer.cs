namespace OdessaGUIProject.UI_Controls
{
    sealed partial class SelectInputVideosControl
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
            this.cameraContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.inputFilesPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.dropHereSmallControl = new OdessaGUIProject.UI_Controls.DropHereSmallControl();
            this.workareaBottomPictureBox = new System.Windows.Forms.PictureBox();
            this.workareaRightPictureBox = new System.Windows.Forms.PictureBox();
            this.workareaTopPictureBox = new System.Windows.Forms.PictureBox();
            this.workareaLeftPictureBox = new System.Windows.Forms.PictureBox();
            this.dropHereBigControl = new OdessaGUIProject.UI_Controls.DropHereBigControl();
            this.scanButton = new OdessaGUIProject.UI_Controls.PictureButtonControl();
            this.sampleVideoTutorialBubble = new OdessaGUIProject.UI_Controls.TutorialBubble();
            this.scanButtonTutorialBubble = new OdessaGUIProject.UI_Controls.TutorialBubble();
            this.inputFilesPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.workareaBottomPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.workareaRightPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.workareaTopPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.workareaLeftPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // cameraContextMenu
            // 
            this.cameraContextMenu.Name = "cameraContextMenu";
            this.cameraContextMenu.Size = new System.Drawing.Size(61, 4);
            this.cameraContextMenu.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.cameraContextMenu_ItemClicked);
            // 
            // inputFilesPanel
            // 
            this.inputFilesPanel.AllowDrop = true;
            this.inputFilesPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.inputFilesPanel.AutoScroll = true;
            this.inputFilesPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(85)))), ((int)(((byte)(85)))), ((int)(((byte)(85)))));
            this.inputFilesPanel.Controls.Add(this.dropHereSmallControl);
            this.inputFilesPanel.Location = new System.Drawing.Point(2, 3);
            this.inputFilesPanel.Name = "inputFilesPanel";
            this.inputFilesPanel.Padding = new System.Windows.Forms.Padding(10);
            this.inputFilesPanel.Size = new System.Drawing.Size(1239, 567);
            this.inputFilesPanel.TabIndex = 0;
            this.inputFilesPanel.DragDrop += new System.Windows.Forms.DragEventHandler(this.inputFilesPanel_DragDrop);
            this.inputFilesPanel.DragEnter += new System.Windows.Forms.DragEventHandler(this.inputFilesPanel_DragEnter);
            this.inputFilesPanel.DragLeave += new System.EventHandler(this.inputFilesPanel_DragLeave);
            // 
            // dropHereSmallControl
            // 
            this.dropHereSmallControl.AllowDrop = true;
            this.dropHereSmallControl.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(85)))), ((int)(((byte)(85)))), ((int)(((byte)(85)))));
            this.dropHereSmallControl.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dropHereSmallControl.Location = new System.Drawing.Point(13, 13);
            this.dropHereSmallControl.Name = "dropHereSmallControl";
            this.dropHereSmallControl.Size = new System.Drawing.Size(292, 193);
            this.dropHereSmallControl.TabIndex = 0;
            this.dropHereSmallControl.Visible = false;
            this.dropHereSmallControl.BrowseComputerHandler += new System.EventHandler(this.dropHereSmallControl_BrowseComputerHandler);
            this.dropHereSmallControl.SelectFromCameraHandler += new System.EventHandler(this.dropHereSmallControl_SelectFromCameraHandler);
            this.dropHereSmallControl.DragDrop += new System.Windows.Forms.DragEventHandler(this.dropHereSmallControl_DragDrop);
            this.dropHereSmallControl.DragEnter += new System.Windows.Forms.DragEventHandler(this.dropHereSmallControl_DragEnter);
            this.dropHereSmallControl.DragLeave += new System.EventHandler(this.dropHereSmallControl_DragLeave);
            // 
            // workareaBottomPictureBox
            // 
            this.workareaBottomPictureBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.workareaBottomPictureBox.BackColor = System.Drawing.Color.Transparent;
            this.workareaBottomPictureBox.BackgroundImage = global::OdessaGUIProject.Properties.Resources.workarea_bottom_2px_tile_horizontal;
            this.workareaBottomPictureBox.Location = new System.Drawing.Point(1, 570);
            this.workareaBottomPictureBox.Name = "workareaBottomPictureBox";
            this.workareaBottomPictureBox.Size = new System.Drawing.Size(1241, 2);
            this.workareaBottomPictureBox.TabIndex = 43;
            this.workareaBottomPictureBox.TabStop = false;
            // 
            // workareaRightPictureBox
            // 
            this.workareaRightPictureBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.workareaRightPictureBox.BackColor = System.Drawing.Color.Transparent;
            this.workareaRightPictureBox.BackgroundImage = global::OdessaGUIProject.Properties.Resources.workarea_rightside_2px_vertical_tile;
            this.workareaRightPictureBox.Location = new System.Drawing.Point(1241, 3);
            this.workareaRightPictureBox.Name = "workareaRightPictureBox";
            this.workareaRightPictureBox.Size = new System.Drawing.Size(2, 567);
            this.workareaRightPictureBox.TabIndex = 42;
            this.workareaRightPictureBox.TabStop = false;
            // 
            // workareaTopPictureBox
            // 
            this.workareaTopPictureBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.workareaTopPictureBox.BackColor = System.Drawing.Color.Transparent;
            this.workareaTopPictureBox.BackgroundImage = global::OdessaGUIProject.Properties.Resources.workarea_top_3px_tile_horizontal;
            this.workareaTopPictureBox.Location = new System.Drawing.Point(1, 0);
            this.workareaTopPictureBox.Name = "workareaTopPictureBox";
            this.workareaTopPictureBox.Size = new System.Drawing.Size(1241, 3);
            this.workareaTopPictureBox.TabIndex = 41;
            this.workareaTopPictureBox.TabStop = false;
            // 
            // workareaLeftPictureBox
            // 
            this.workareaLeftPictureBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.workareaLeftPictureBox.BackColor = System.Drawing.Color.Transparent;
            this.workareaLeftPictureBox.BackgroundImage = global::OdessaGUIProject.Properties.Resources.workarea_leftside_2px_vertical_tile;
            this.workareaLeftPictureBox.Location = new System.Drawing.Point(0, 3);
            this.workareaLeftPictureBox.Name = "workareaLeftPictureBox";
            this.workareaLeftPictureBox.Size = new System.Drawing.Size(2, 567);
            this.workareaLeftPictureBox.TabIndex = 40;
            this.workareaLeftPictureBox.TabStop = false;
            // 
            // dropHereBigControl
            // 
            this.dropHereBigControl.AllowDrop = true;
            this.dropHereBigControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dropHereBigControl.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(85)))), ((int)(((byte)(85)))), ((int)(((byte)(85)))));
            this.dropHereBigControl.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dropHereBigControl.Location = new System.Drawing.Point(394, 265);
            this.dropHereBigControl.Name = "dropHereBigControl";
            this.dropHereBigControl.Size = new System.Drawing.Size(465, 344);
            this.dropHereBigControl.TabIndex = 1;
            this.dropHereBigControl.BrowseComputerHandler += new System.EventHandler(this.dropHereBigControl_BrowseComputerHandler);
            this.dropHereBigControl.SelectFromCameraHandler += new System.EventHandler(this.dropHereBigControl_SelectFromCameraHandler);
            this.dropHereBigControl.DragDrop += new System.Windows.Forms.DragEventHandler(this.dropHereBigControl_DragDrop);
            this.dropHereBigControl.DragEnter += new System.Windows.Forms.DragEventHandler(this.dropHereBigControl_DragEnter);
            this.dropHereBigControl.DragLeave += new System.EventHandler(this.dropHereBigControl_DragLeave);
            // 
            // scanButton
            // 
            this.scanButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.scanButton.AutoSize = true;
            this.scanButton.DefaultImage = global::OdessaGUIProject.Properties.Resources.button_green_scanvideos_normal;
            this.scanButton.HoverImage = global::OdessaGUIProject.Properties.Resources.button_green_scanvideos_hover;
            this.scanButton.Location = new System.Drawing.Point(1050, 589);
            this.scanButton.MouseDownImage = global::OdessaGUIProject.Properties.Resources.button_green_scanvideos_click;
            this.scanButton.Name = "scanButton";
            this.scanButton.Size = new System.Drawing.Size(192, 43);
            this.scanButton.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Normal;
            this.scanButton.TabIndex = 3;
            this.scanButton.Tooltip = null;
            this.scanButton.Visible = false;
            this.scanButton.Click += new System.EventHandler(this.scanButton_Click);
            // 
            // sampleVideoTutorialBubble
            // 
            this.sampleVideoTutorialBubble.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(85)))), ((int)(((byte)(85)))), ((int)(((byte)(85)))));
            this.sampleVideoTutorialBubble.Font = new System.Drawing.Font("Open Sans", 8.25F);
            this.sampleVideoTutorialBubble.Location = new System.Drawing.Point(296, 9);
            this.sampleVideoTutorialBubble.Name = "sampleVideoTutorialBubble";
            this.sampleVideoTutorialBubble.Size = new System.Drawing.Size(203, 184);
            this.sampleVideoTutorialBubble.TabIndex = 44;
            this.sampleVideoTutorialBubble.TutorialProgress = OdessaGUIProject.UI_Helpers.TutorialProgress.TutorialAddSampleVideo;
            this.sampleVideoTutorialBubble.Visible = false;
            this.sampleVideoTutorialBubble.Advance += new System.EventHandler(this.sampleVideoTutorialBubble_Advance);
            // 
            // scanButtonTutorialBubble
            // 
            this.scanButtonTutorialBubble.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.scanButtonTutorialBubble.BackColor = System.Drawing.Color.Transparent;
            this.scanButtonTutorialBubble.Font = new System.Drawing.Font("Open Sans", 8.25F);
            this.scanButtonTutorialBubble.Location = new System.Drawing.Point(1050, 439);
            this.scanButtonTutorialBubble.Name = "scanButtonTutorialBubble";
            this.scanButtonTutorialBubble.Size = new System.Drawing.Size(193, 144);
            this.scanButtonTutorialBubble.TabIndex = 45;
            this.scanButtonTutorialBubble.TutorialProgress = OdessaGUIProject.UI_Helpers.TutorialProgress.TutorialScanButton;
            this.scanButtonTutorialBubble.Visible = false;
            this.scanButtonTutorialBubble.Advance += new System.EventHandler(this.scanButtonTutorialBubble_Advance);
            // 
            // SelectInputVideosControl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(44)))), ((int)(((byte)(44)))));
            this.Controls.Add(this.scanButton);
            this.Controls.Add(this.scanButtonTutorialBubble);
            this.Controls.Add(this.sampleVideoTutorialBubble);
            this.Controls.Add(this.workareaBottomPictureBox);
            this.Controls.Add(this.workareaRightPictureBox);
            this.Controls.Add(this.workareaTopPictureBox);
            this.Controls.Add(this.workareaLeftPictureBox);
            this.Controls.Add(this.dropHereBigControl);
            this.Controls.Add(this.inputFilesPanel);
            this.DoubleBuffered = true;
            this.Name = "SelectInputVideosControl";
            this.Size = new System.Drawing.Size(1243, 633);
            this.Load += new System.EventHandler(this.SelectInputVideosControl_Load);
            this.inputFilesPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.workareaBottomPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.workareaRightPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.workareaTopPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.workareaLeftPictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip cameraContextMenu;
        private System.Windows.Forms.FlowLayoutPanel inputFilesPanel;
        private DropHereSmallControl dropHereSmallControl;
        private DropHereBigControl dropHereBigControl;
        private PictureButtonControl scanButton;
        private System.Windows.Forms.PictureBox workareaLeftPictureBox;
        private System.Windows.Forms.PictureBox workareaTopPictureBox;
        private System.Windows.Forms.PictureBox workareaRightPictureBox;
        private System.Windows.Forms.PictureBox workareaBottomPictureBox;
        private TutorialBubble sampleVideoTutorialBubble;
        private TutorialBubble scanButtonTutorialBubble;
    }
}

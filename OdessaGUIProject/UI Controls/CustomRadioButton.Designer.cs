namespace OdessaGUIProject
{
    sealed partial class CustomRadioButton
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
            this.imgState = new System.Windows.Forms.PictureBox();
            this.labelMain = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.imgState)).BeginInit();
            this.SuspendLayout();
            // 
            // imgState
            // 
            this.imgState.Image = OdessaGUIProject.Properties.Resources.radio_selected;
            this.imgState.Location = new System.Drawing.Point(2, 4);
            this.imgState.Name = "imgState";
            this.imgState.Size = new System.Drawing.Size(22, 22);
            this.imgState.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.imgState.TabIndex = 0;
            this.imgState.TabStop = false;
            this.imgState.MouseDown += new System.Windows.Forms.MouseEventHandler(this.imgState_MouseDown);
            this.imgState.MouseUp += new System.Windows.Forms.MouseEventHandler(this.imgState_MouseUp);
            // 
            // labelMain
            // 
            this.labelMain.AutoSize = true;
            this.labelMain.BackColor = System.Drawing.Color.Transparent;
            this.labelMain.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelMain.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.labelMain.Location = new System.Drawing.Point(30, 3);
            this.labelMain.Name = "labelMain";
            this.labelMain.Size = new System.Drawing.Size(109, 25);
            this.labelMain.TabIndex = 1;
            this.labelMain.Text = "transLabel1";
            this.labelMain.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.labelMain.MouseDown += new System.Windows.Forms.MouseEventHandler(this.labelMain_MouseDown);
            this.labelMain.MouseUp += new System.Windows.Forms.MouseEventHandler(this.labelMain_MouseUp);
            // 
            // CustomRadioButton
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.labelMain);
            this.Controls.Add(this.imgState);
            this.Name = "CustomRadioButton";
            this.Size = new System.Drawing.Size(244, 30);
            ((System.ComponentModel.ISupportInitialize)(this.imgState)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox imgState;
        private System.Windows.Forms.Label labelMain;
    }
}

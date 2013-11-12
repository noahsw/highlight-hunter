using System;
using System.Windows.Forms;
using OdessaGUIProject.Properties;
using OdessaGUIProject.UI_Helpers;

namespace OdessaGUIProject.UI_Controls
{
    public partial class DropHereBigControl : UserControl
    {
        public DropHereBigControl()
        {
            InitializeComponent();

            DesignLanguage.ApplyCustomFont(this.Controls);
        }

        public event EventHandler BrowseComputerHandler;

        public event EventHandler SelectFromCameraHandler;

        public void ResetState()
        {
            dropPictureBox.Image = Resources.select_droptarget_large;
        }

        public void ShowDropState()
        {
            dropPictureBox.Image = Resources.select_droptarget_large_active;
        }

        private void browseComputerPictureButtonControl_Click(object sender, EventArgs e)
        {
            if (BrowseComputerHandler != null)
                BrowseComputerHandler(sender, e);
        }

        private void selectFromCameraLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
        }

        private void selectFromCameraPictureButtonControl_Click(object sender, EventArgs e)
        {
            if (SelectFromCameraHandler != null)
                SelectFromCameraHandler(sender, e);
        }
    }
}
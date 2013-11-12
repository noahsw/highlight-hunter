using System;
using System.Windows.Forms;
using OdessaGUIProject.Properties;
using OdessaGUIProject.UI_Helpers;

namespace OdessaGUIProject.UI_Controls
{
    public partial class DropHereSmallControl : UserControl
    {
        public DropHereSmallControl()
        {
            InitializeComponent();
        }

        public event EventHandler BrowseComputerHandler;

        public event EventHandler SelectFromCameraHandler;

        internal void ResetState()
        {
            dropHerePictureBox.Image = Resources.select_droptarget_small;
        }

        internal void ShowDropState()
        {
            dropHerePictureBox.Image = Resources.select_droptarget_small_active;
        }

        private void browseLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (BrowseComputerHandler != null)
                BrowseComputerHandler(sender, e);
        }

        private void browseLinkLabel_MouseEnter(object sender, EventArgs e)
        {
            browseLinkLabel.LinkColor = DesignLanguage.SecondaryBlue;
        }

        private void browseLinkLabel_MouseLeave(object sender, EventArgs e)
        {
            browseLinkLabel.LinkColor = DesignLanguage.LightGray;
        }

        private void DropHereSmallControl_Load(object sender, EventArgs e)
        {
            DesignLanguage.ApplyCustomFont(this.Controls);
        }

        private void selectFromCameraLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (SelectFromCameraHandler != null)
                SelectFromCameraHandler(sender, e);
        }

        private void selectFromCameraLinkLabel_MouseEnter(object sender, EventArgs e)
        {
            selectFromCameraLinkLabel.LinkColor = DesignLanguage.SecondaryBlue;
        }

        private void selectFromCameraLinkLabel_MouseLeave(object sender, EventArgs e)
        {
            selectFromCameraLinkLabel.LinkColor = DesignLanguage.LightGray;
        }
    }
}
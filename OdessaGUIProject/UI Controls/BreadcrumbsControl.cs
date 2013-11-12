using System;
using System.Drawing;
using System.Windows.Forms;
using OdessaGUIProject.Properties;
using OdessaGUIProject.UI_Helpers;

namespace OdessaGUIProject.UI_Controls
{
    public partial class BreadcrumbsControl : UserControl
    {
        // Achieving transparency with labels without flickering:
        // http://www.codeproject.com/Articles/25048/How-to-Use-Transparent-Images-and-Labels-in-Window

        private Color disabledColor = Color.FromArgb(85, 85, 85);

        private Color enabledColor = Color.FromArgb(204, 204, 204);

        /// <summary>
        /// Represents the width of the controls that don't move
        /// </summary>
        private int fixedControlWidths;

        public BreadcrumbsControl()
        {
            InitializeComponent();

            fixedControlWidths = leftEndPictureBox.Width + firstArrowPictureBox.Width + secondArrowPictureBox.Width + rightEndPictureBox.Width;

            DesignLanguage.ApplyCustomFont(this.Controls);
        }

        public event EventHandler ReviewClicked;

        public event EventHandler ScanClicked;

        public event EventHandler SelectVideosClicked;

        public void SwitchToReview()
        {
            leftEndPictureBox.Image = Resources.breadcrumbs_inactive_left_end;
            selectPictureBox.Image = Resources.breadcrumbs_text_addvideos;
            selectVideosPanel.BackgroundImage = Resources.breadcrumbs_inactive_1px_tile;
            firstArrowPictureBox.Image = Resources.breadcrumbs_both_inactive_forward_arrow;
            scanPictureBox.Image = Resources.breadcrumbs_text_scanhighlights;
            scanPanel.BackgroundImage = Resources.breadcrumbs_inactive_1px_tile;
            secondArrowPictureBox.Image = Resources.breadcrumbs_second_active_forward_arrow;
            reviewPictureBox.Image = Resources.breadcrumbs_text_saveshare_active;
            reviewPanel.BackgroundImage = Resources.breadcrumbs_active_1px_tile;
            rightEndPictureBox.Image = Resources.breadcrumbs_active_right_end;
        }

        public void SwitchToScan()
        {
            leftEndPictureBox.Image = Resources.breadcrumbs_inactive_left_end;
            selectPictureBox.Image = Resources.breadcrumbs_text_addvideos;
            selectVideosPanel.BackgroundImage = Resources.breadcrumbs_inactive_1px_tile;
            firstArrowPictureBox.Image = Resources.breadcrumbs_second_active_forward_arrow;
            scanPictureBox.Image = Resources.breadcrumbs_text_scanhighlights_active;
            scanPanel.BackgroundImage = Resources.breadcrumbs_active_1px_tile;
            secondArrowPictureBox.Image = Resources.breadcrumbs_first_active_forward_arrow;
            reviewPictureBox.Image = Resources.breadcrumbs_text_saveshare;
            reviewPanel.BackgroundImage = Resources.breadcrumbs_inactive_1px_tile;
            rightEndPictureBox.Image = Resources.breadcrumbs_inactive_right_end;
        }

        public void SwitchToSelect()
        {
            leftEndPictureBox.Image = Resources.breadcrumbs_active_left_end;
            selectPictureBox.Image = Resources.breadcrumbs_text_addvideos_active;
            selectVideosPanel.BackgroundImage = Resources.breadcrumbs_active_1px_tile;
            firstArrowPictureBox.Image = Resources.breadcrumbs_first_active_forward_arrow;
            scanPictureBox.Image = Resources.breadcrumbs_text_scanhighlights;
            scanPanel.BackgroundImage = Resources.breadcrumbs_inactive_1px_tile;
            secondArrowPictureBox.Image = Resources.breadcrumbs_both_inactive_forward_arrow;
            reviewPictureBox.Image = Resources.breadcrumbs_text_saveshare;
            reviewPanel.BackgroundImage = Resources.breadcrumbs_inactive_1px_tile;
            rightEndPictureBox.Image = Resources.breadcrumbs_inactive_right_end;
        }

        internal void DisableReview()
        {
            //reviewLabelButton.Enabled = false;
            //reviewLabelButton.LinkVisited = false;
        }

        private void BreadcrumbsControl_Load(object sender, EventArgs e)
        {
        }

        private void BreadcrumbsControl_Resize(object sender, EventArgs e)
        {
            SuspendLayout();

            var stretchedControlWidths = this.Width - fixedControlWidths;

            selectVideosPanel.Width = (int)(stretchedControlWidths / 3f);
            scanPanel.Width = (int)(stretchedControlWidths / 3f);
            reviewPanel.Width = this.Width - selectVideosPanel.Width - scanPanel.Width - fixedControlWidths;
            firstArrowPictureBox.Left = selectVideosPanel.Right;
            scanPanel.Left = firstArrowPictureBox.Right;
            secondArrowPictureBox.Left = scanPanel.Right;
            reviewPanel.Left = secondArrowPictureBox.Right;

            ResumeLayout();
        }

        private void reviewLabelButton_Click(object sender, EventArgs e)
        {
            if (ReviewClicked != null)
                ReviewClicked(sender, e);
        }

        private void scanLabelButton_Click(object sender, EventArgs e)
        {
            if (ScanClicked != null)
                ScanClicked(sender, e);
        }

        private void selectVideosLabelButton_Click(object sender, EventArgs e)
        {
            if (SelectVideosClicked != null)
                SelectVideosClicked(sender, e);
        }
    }
}
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using OdessaGUIProject.UI_Helpers;

namespace OdessaGUIProject.UI_Controls
{
    [DefaultEvent("Click")]
    public sealed partial class CustomLinkLabel : UserControl
    {
        private Image image;

        public CustomLinkLabel()
        {
            InitializeComponent();

            this.BackColor = Color.Transparent; // so we can see what it looks like in the Designer

            pictureBox.Image = image;

            DesignLanguage.ApplyCustomFont(this.Controls);
        }

        public Image Image
        {
            get { return image; }
            set
            {
                image = value;
                pictureBox.Image = image;
            }
        }

        public string LinkText
        {
            get
            {
                return linkLabel.Text;
            }
            set
            {
                linkLabel.Text = value;
                this.Size = GetPreferredSize(Size.Empty);
                PerformLayout();
            }
        }

        private void linkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.OnClick(e);
        }

        private void linkLabel_MouseEnter(object sender, EventArgs e)
        {
            linkLabel.LinkColor = DesignLanguage.SecondaryBlue;
        }

        private void linkLabel_MouseLeave(object sender, EventArgs e)
        {
            linkLabel.LinkColor = DesignLanguage.LightGray;
        }

        private void pictureBox_Click(object sender, EventArgs e)
        {
            this.OnClick(e);
        }

        private void pictureBox_MouseEnter(object sender, EventArgs e)
        {
            linkLabel.LinkColor = DesignLanguage.SecondaryBlue;
        }

        private void pictureBox_MouseLeave(object sender, EventArgs e)
        {
            linkLabel.LinkColor = DesignLanguage.LightGray;
        }

        public override Size GetPreferredSize(Size proposedSize)
        {
            return new Size(linkLabel.Right + 2, this.Height); // 5 for buffer
        }

        private void CustomLinkLabel_Load(object sender, EventArgs e)
        {

        }

    }
}
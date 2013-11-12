using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace OdessaGUIProject.UI_Controls
{
    [DefaultEvent("Click")]
    public partial class PictureButtonControl : UserControl
    {
        private Color backColor;
        private Image defaultImage;

        private PictureBoxSizeMode sizeMode;

        private string tooltip;

        public PictureButtonControl()
        {
            InitializeComponent();

            mainPictureBox.Image = DefaultImage;
        }

        [Category("Appearance")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public override Color BackColor
        {
            get { return backColor; }
            set
            {
                backColor = value;
                mainPictureBox.BackColor = backColor;
            }
        }

        public Image DefaultImage
        {
            get
            {
                return defaultImage;
            }
            set
            {
                defaultImage = value;
                mainPictureBox.Image = DefaultImage;

                if (sizeMode == PictureBoxSizeMode.AutoSize)
                    this.Size = mainPictureBox.Size;
            }
        }

        public Image HoverImage { get; set; }

        public Image MouseDownImage { get; set; }

        public PictureBoxSizeMode SizeMode
        {
            get { return sizeMode; }
            set
            {
                sizeMode = value;
                mainPictureBox.SizeMode = sizeMode;
                if (sizeMode == PictureBoxSizeMode.AutoSize)
                    this.Size = mainPictureBox.Size;
            }
        }

        public string Tooltip
        {
            get { return tooltip; }
            set
            {
                tooltip = value;
                var newTooltip = new ToolTip();
                newTooltip.SetToolTip(mainPictureBox, tooltip);
            }
        }

        private void mainPictureBox_Click(object sender, EventArgs e)
        {
            this.OnClick(e);
        }

        private void mainPictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (MouseDownImage != null)
                mainPictureBox.Image = MouseDownImage;
        }

        private void mainPictureBox_MouseEnter(object sender, EventArgs e)
        {
            if (HoverImage != null)
                mainPictureBox.Image = HoverImage;
        }

        private void mainPictureBox_MouseHover(object sender, EventArgs e)
        {
        }

        private void mainPictureBox_MouseLeave(object sender, EventArgs e)
        {
            mainPictureBox.Image = DefaultImage;
        }

        private void mainPictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            mainPictureBox.Image = DefaultImage;
        }

        private void PictureButtonControl_Load(object sender, EventArgs e)
        {
        }
    }
}
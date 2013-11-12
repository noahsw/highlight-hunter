using System;
using System.Drawing;
using System.Windows.Forms;

namespace OdessaGUIProject.UI_Controls
{
    public sealed partial class CustomPictureBox : Control
    {
        private Image image;

        private bool isTrulyTransparent;

        public CustomPictureBox()
        {
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
        }

        public Image Image
        {
            get
            {
                return image;
            }
            set
            {
                image = value;
                RecreateHandle();
            }
        }

        public bool IsTrulyTransparent
        {
            get { return isTrulyTransparent; }
            set
            {
                isTrulyTransparent = value;
                Invalidate();
            }
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;

                if (IsTrulyTransparent)
                    cp.ExStyle = 0x00000020; //WS_EX_TRANSPARENT

                return cp;
            }
        }

        //Hack
        public void Redraw()
        {
            RecreateHandle();
        }

        protected override void OnMove(EventArgs e)
        {
            RecreateHandle();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            //if (IsTrulyTransparent)
            //{
            Graphics g = e.Graphics;
            //g.Clear(BackColor);

            if (image != null)
                g.DrawImageUnscaled(Image, 0, 0);

            //}
            //else
            //base.OnPaint(e);
        }

        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
            if (!IsTrulyTransparent)
                base.OnPaintBackground(pevent);

            //else, do nothing
            //Parent.Invalidate(Bounds, false);
        }

        /*
        protected void InvalidateEx()
        {
            if (Parent == null)
                return;

            Rectangle rc = new Rectangle(Location, Size);
            Parent.Invalidate(rc, true);
        }

         */
    }
}
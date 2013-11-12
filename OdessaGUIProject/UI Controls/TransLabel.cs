using System;
using System.Drawing;
using System.Windows.Forms;

namespace OdessaGUIProject
{
    public sealed partial class TransLabel : Label
    {
        public TransLabel()
        {
            //InitializeComponent();

            SetStyle(ControlStyles.Opaque, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, false);
            Font = new Font("Segoe UI", 12f, FontStyle.Regular);
            ForeColor = Color.White;
            BackColor = Color.Transparent;
        }

        public override string Text
        {
            get
            {
                return base.Text;
            }
            set
            {
                base.Text = value;
                if (Parent != null)
                    Parent.Invalidate(Bounds, false);
            }
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle = 0x00000020; //WS_EX_TRANSPARENT
                return cp;
            }
        }

        protected override void OnMove(EventArgs e)
        {
            RecreateHandle();
        }

        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
            //do nothing
        }

        /*
        protected override void OnPaint(PaintEventArgs e)
        {
            /*
            Graphics g = e.Graphics;

            Rectangle rect = new Rectangle(0, 0, this.Width, this.Height);
            Brush brush = new SolidBrush(Color.FromArgb(30, Color.Black));

            g.FillRectangle(brush, rect);

            g.Dispose();
        }
        */
    }
}
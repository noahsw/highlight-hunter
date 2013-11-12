using System;
using System.Drawing;
using System.Windows.Forms;
using NLog;

namespace OdessaGUIProject
{
    public sealed partial class TransButton : Button
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly Color _odessaGreen = Color.FromArgb(153, 204, 51);

        public TransButton()
        {
            SetStyle(ControlStyles.Opaque, true);
            SetStyle(ControlStyles.DoubleBuffer, true);
            ForeColor = _odessaGreen;

            BackColor = Color.Transparent;
            FlatStyle = FlatStyle.Flat;
            FlatAppearance.BorderSize = 0;
            FlatAppearance.MouseDownBackColor = _odessaGreen;
            FlatAppearance.MouseOverBackColor = Color.Transparent;

            TextAlign = ContentAlignment.MiddleCenter;
            Font = new Font("Segoe UI", 18f, FontStyle.Regular);

            BackColor = Color.Transparent;
            BackgroundImage = Properties.Resources._30_percent_opacity;

            AutoSize = false;
            Width = 180;
            Height = 40;

            Alpha = 50;
            IsHovering = false;
        }

        public int Alpha { get; set; }

        public bool IsClicked { get; set; }

        public bool IsHovering { get; set; }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (IsHovering == false)
                return;

            Logger.Trace("called");

            IsClicked = true;

            BackgroundImage = null;
            ForeColor = Color.Black;

            base.OnMouseDown(e);
            //this.Invalidate();
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            //Refresh();
            //logger.Info("called");

            if (IsHovering == false)
            {
                IsHovering = true;
                Alpha = 200;

                BackgroundImage = Properties.Resources._50_percent_opacity;
            }

            //base.OnMouseEnter(e);
            //this.Invalidate();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            //logger.Info("called");

            if (IsHovering)
            {
                IsHovering = false;
                Alpha = 30;

                BackgroundImage = Properties.Resources._30_percent_opacity;
            }

            //base.OnMouseLeave(e);
            //this.Invalidate();
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            Logger.Trace("called");

            IsClicked = false;

            BackgroundImage = Properties.Resources._50_percent_opacity;
            ForeColor = _odessaGreen;

            base.OnMouseUp(e);

            // this.Focus(); // this was causing us to freeze
            //this.Invalidate();
        }

        protected override void OnMove(EventArgs e)
        {
            RecreateHandle();
        }

        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
            //do nothing
            //base.OnPaintBackground(pevent);
        }

        /*
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle = 0x00000020; //WS_EX_TRANSPARENT
                return cp;
            }
        }

        protected override void OnPaint(PaintEventArgs pevent)
        {
            logger.Info("called with IsHovering={0} and IsClicked={1}", IsHovering, IsClicked);

            base.OnPaint(pevent);
            return;

            /*
            Graphics g = pevent.Graphics;

            Image backgroundImage;
            if (IsHovering)
            {
                backgroundImage = Properties.Resources.bkdg_50percent_opaque;
                //g.DrawImage(Properties.Resources.bkdg_50percent_opaque, 0, 0, Width, Height);
            }
            else
            {
                backgroundImage = Properties.Resources.bkdg_30percent_opaque;
                //g.DrawImage(Properties.Resources.bkdg_30percent_opaque, 0, 0, Width, Height);
            }

            if (IsClicked)
            {
                ForeColor = Color.FromArgb(155, Color.Black);
                Brush brush = new SolidBrush(odessaGreen);
                g.FillRectangle(brush, 0, 0, Width, Height);
            }
            else
            {
                ForeColor = odessaGreen;
                this.BackgroundImage = backgroundImage;
                //g.DrawImageUnscaled(backgroundImage, 0, 0, Width, Height);
            }

            //Parent.Invalidate();
            //g.FillRectangle(Brushes.White, 0, 0, Width, Height);
            //g.CompositingMode = CompositingMode.SourceCopy;
            //g.FillRectangle(Brushes.Transparent, 0, 0, Width, Height);

            Rectangle rect = new Rectangle(0, 0, this.Width, this.Height);

            Brush brush = new SolidBrush(Color.FromArgb(Alpha, Color.Black));
            g.FillRectangle(brush, rect);

            //base.OnPaint(pevent);
        }
    */
        /*
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            this.OverrideCursor = Cursors.Arrow;
        }
        */
    }
}
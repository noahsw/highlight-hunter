using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace OdessaGUIProject.UI_Controls
{
    internal class RoundedCornersPanel : Panel
    {
        internal RoundedCornersPanel()
        {
            SetStyle(ControlStyles.Opaque, true);
            SetStyle(ControlStyles.DoubleBuffer, true);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Console.WriteLine(e.ClipRectangle.ToString() + " - " + this.ClientRectangle);
            if (e.ClipRectangle != this.ClientRectangle)
            {
                base.OnPaint(e);
                return; // this is not the panel. this is a control within the panel
            }

            Graphics v = e.Graphics;
            DrawRoundRect(v, new SolidBrush(Color.FromArgb(85, 85, 85)), e.ClipRectangle.Left, e.ClipRectangle.Top, e.ClipRectangle.Width - 1, e.ClipRectangle.Height - 1, 5);
            //Without rounded corners
            //e.Graphics.DrawRectangle(Pens.Blue, e.ClipRectangle.Left, e.ClipRectangle.Top, e.ClipRectangle.Width - 1, e.ClipRectangle.Height - 1);

            base.OnPaint(e);
        }

        private void DrawRoundRect(Graphics g, Brush p, float X, float Y, float width, float height, float radius)
        {
            GraphicsPath gp = new GraphicsPath();
            //Upper-right arc:
            gp.AddArc(X + width - (radius * 2), Y, radius * 2, radius * 2, 270, 90);
            //Lower-right arc:
            gp.AddArc(X + width - (radius * 2), Y + height - (radius * 2), radius * 2, radius * 2, 0, 90);
            //Lower-left arc:
            gp.AddArc(X, Y + height - (radius * 2), radius * 2, radius * 2, 90, 90);
            //Upper-left arc:
            gp.AddArc(X, Y, radius * 2, radius * 2, 180, 90);
            gp.CloseFigure();
            g.FillPath(p, gp);
            gp.Dispose();
        }
    }
}
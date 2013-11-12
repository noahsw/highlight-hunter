using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using OdessaGUIProject.Properties;

namespace OdessaGUIProject.UI_Helpers
{
    public class BorderlessWindow : IDisposable
    {
        internal bool IsFormActive = true;
        internal bool IsMinimizable = true;
        internal bool IsResizable = true;
        internal WindowMenu SystemMenu;
        private readonly ToolTip ButtonTip;
        private Form form;

        private GraphicsPath topEdgePath, topLeftEdgePath, topRightEdgePath,
            titleBarPath, closeButtonPath, maxButtonPath, minButtonPath, 
            minimizeDividerPath, maximizeDividerPath, closeDividerPath,
            leftEdge, bottomEdge, rightEdge, bottomLeftEdge, bottomRightEdge;
        //iconButtonPath,

        internal BorderlessWindow(Form owner, bool resizable, bool minimizable)
        {
            form = owner;

            this.IsResizable = resizable;
            this.IsMinimizable = minimizable;

            this.topEdgePath = new GraphicsPath();
            this.topLeftEdgePath = new GraphicsPath();
            this.topRightEdgePath = new GraphicsPath();
            this.titleBarPath = new GraphicsPath();
            this.closeButtonPath = new GraphicsPath();
            this.closeDividerPath = new GraphicsPath();
            this.maxButtonPath = new GraphicsPath();
            this.maximizeDividerPath = new GraphicsPath();
            this.minButtonPath = new GraphicsPath();
            this.minimizeDividerPath = new GraphicsPath();
            //this.iconButtonPath = new GraphicsPath();
            this.leftEdge = new GraphicsPath();
            this.rightEdge = new GraphicsPath();
            this.bottomEdge = new GraphicsPath();
            this.bottomLeftEdge = new GraphicsPath();
            this.bottomRightEdge = new GraphicsPath();

            BuildPaths();

            //this.Padding = new Padding(5, (int)titleBar.GetBounds().Height + 7, 5, 5);

            this.SystemMenu = new WindowMenu(form);
            this.SystemMenu.SystemEvent += new WindowMenuEventHandler(SystemMenu_SystemEvent);

            ButtonTip = new ToolTip();
        }

        internal delegate void NCWinMessageEventArgs(int msg, IntPtr wParam, IntPtr lParam);

        internal event NCWinMessageEventArgs SendNCWinMessage;

        public void Dispose()
        {
            Dispose(true);

            // Use SupressFinalize in case a subclass
            // of this type implements a finalizer.
            GC.SuppressFinalize(this);
        }

        internal void BuildPaths()
        {
            int edgeSize = SystemInformation.CaptionHeight + 2;
            int buttonSize = 27;
            int buttonPadding = 2;

            //Top Sizing Edge
            topEdgePath.Reset();
            topEdgePath.AddRectangle(new Rectangle(edgeSize, 0, form.Width - (edgeSize * 2), 5));

            //Top-Left Sizing Edge
            topLeftEdgePath.Reset();
            topLeftEdgePath.AddRectangle(new Rectangle(0, 0, edgeSize, edgeSize));
            topLeftEdgePath.AddRectangle(new Rectangle(5, 5, edgeSize - 5, edgeSize - 5));

            //Top-Right Sizing Edge
            topRightEdgePath.Reset();
            topRightEdgePath.AddRectangle(new Rectangle(form.Width - edgeSize, 0, edgeSize, edgeSize));
            topRightEdgePath.AddRectangle(new Rectangle(form.Width - edgeSize, 5, edgeSize - 5, edgeSize - 5));

            //Close Button
            closeButtonPath.Reset();
            closeButtonPath.AddRectangle(new Rectangle(form.Width - (buttonSize + buttonPadding), buttonPadding, buttonSize, buttonSize));
            //closeButtonPath.AddRectangle(closeButton.ClientRectangle);

            //Close divider (to the left of Close button)
            closeDividerPath.Reset();
            closeDividerPath.AddRectangle(new Rectangle(form.Width - (buttonPadding + buttonSize + buttonPadding), 0, 2, 31));

            //Maximize Button
            if (IsResizable)
            {
                maxButtonPath.Reset();
                maxButtonPath.AddRectangle(new Rectangle(form.Width - ((buttonSize + buttonPadding) * 2), buttonPadding, buttonSize, buttonSize));
                //maxButtonPath.AddRectangle(maximizeButton.ClientRectangle);

                //Maximize divider (to the left of Maximize button)
                maximizeDividerPath.Reset();
                maximizeDividerPath.AddRectangle(new Rectangle(form.Width - (2 * buttonPadding + 2 * buttonSize + buttonPadding), 0, 2, 31));
            }

            //Minimize Button
            if (IsMinimizable)
            {
                minButtonPath.Reset();
                minButtonPath.AddRectangle(new Rectangle(form.Width - ((buttonSize + buttonPadding) * 3), buttonPadding, buttonSize, buttonSize));
                //minButtonPath.AddRectangle(minimizeButton.ClientRectangle);

                //Minimize divider (to the left of Minimize button)
                minimizeDividerPath.Reset();
                minimizeDividerPath.AddRectangle(new Rectangle(form.Width - (3 * buttonPadding + 3 * buttonSize + buttonPadding), 0, 2, 31));
            }

            /* Disabled because it looks like shit
            //Window Menu Button (Icon)
            iconButtonPath.Reset();
            //iconButton.AddRectangle(new Rectangle(8, 8, buttonSize, buttonSize));
            iconButtonPath.AddRectangle(new Rectangle(4, 8, 24, 31 - 8));
             */

            //TitleBar
            titleBarPath.Reset();
            titleBarPath.AddRectangle(new Rectangle(0, 0, form.Width, 31));
            //titleBarPath.AddRectangle(new Rectangle();

            //Remove Titlebar Buttons from TitleBar Path
            titleBarPath.AddPath(closeButtonPath, false);
            if (IsResizable)
                titleBarPath.AddPath(maxButtonPath, false);
            if (IsMinimizable)
                titleBarPath.AddPath(minButtonPath, false);
            //titleBarPath.AddPath(iconButtonPath, false);

            //Left Sizing Edge
            leftEdge.Reset();
            leftEdge.AddRectangle(new Rectangle(0, edgeSize, 5, form.Height - (edgeSize * 2)));

            //Right Sizing Edge
            rightEdge.Reset();
            rightEdge.AddRectangle(new Rectangle(form.Width - 5, edgeSize, 5, form.Width - (edgeSize * 2)));

            //Bottom Sizing Edge
            bottomEdge.Reset();
            bottomEdge.AddRectangle(new Rectangle(edgeSize, form.Height - 5, form.Width - (edgeSize * 2), 5));

            //Bottom-Left Sizing Edge
            bottomLeftEdge.Reset();
            bottomLeftEdge.AddRectangle(new Rectangle(0, form.Height - edgeSize, edgeSize, edgeSize));
            bottomLeftEdge.AddRectangle(new Rectangle(5, form.Height - edgeSize, edgeSize - 5, edgeSize - 5));

            //Bottom-Right Sizing Edge
            bottomRightEdge.Reset();
            bottomRightEdge.AddRectangle(new Rectangle(form.Width - edgeSize, form.Height - edgeSize, edgeSize, edgeSize));
            bottomRightEdge.AddRectangle(new Rectangle(form.Width - edgeSize, form.Height - edgeSize, edgeSize - 5, edgeSize - 5));
        }

        internal IntPtr OnNonClientHitTest(IntPtr lParam)
        {
            USER32.NCHitTestResult result = USER32.NCHitTestResult.HTCLIENT;

            Point point = form.PointToClient(new Point(lParam.ToInt32()));

            if (this.titleBarPath.IsVisible(point))
            {
                result = USER32.NCHitTestResult.HTCAPTION;
            }

            if (form.WindowState == FormWindowState.Normal && IsResizable)
            {
                if (this.topLeftEdgePath.IsVisible(point))
                    result = USER32.NCHitTestResult.HTTOPLEFT;
                else if (this.topEdgePath.IsVisible(point))
                    result = USER32.NCHitTestResult.HTTOP;
                else if (this.topRightEdgePath.IsVisible(point))
                    result = USER32.NCHitTestResult.HTTOPRIGHT;
                else if (this.leftEdge.IsVisible(point))
                    result = USER32.NCHitTestResult.HTLEFT;
                else if (this.rightEdge.IsVisible(point))
                    result = USER32.NCHitTestResult.HTRIGHT;
                else if (this.bottomLeftEdge.IsVisible(point))
                    result = USER32.NCHitTestResult.HTBOTTOMLEFT;
                else if (this.bottomEdge.IsVisible(point))
                    result = USER32.NCHitTestResult.HTBOTTOM;
                else if (this.bottomRightEdge.IsVisible(point))
                    result = USER32.NCHitTestResult.HTBOTTOMRIGHT;
            }

            if (this.closeButtonPath.IsVisible(point))
                result = USER32.NCHitTestResult.HTBORDER; // if this is HTCLOSE, the user must double-click to close. not good
            else if (IsResizable && this.maxButtonPath.IsVisible(point))
                result = USER32.NCHitTestResult.HTBORDER;
            else if (IsMinimizable && this.minButtonPath.IsVisible(point))
                result = USER32.NCHitTestResult.HTBORDER;
            //else if (this.iconButtonPath.IsVisible(point))
            //    result = USER32.NCHitTestResult.HTBORDER;

            return (IntPtr)result;
        }

        internal void OnNonClientLButtonUp(IntPtr lParam)
        {
            USER32.SysCommand code = USER32.SysCommand.SC_DEFAULT;
            Point point = form.PointToClient(new Point(lParam.ToInt32()));

            /*
            if (this.iconButtonPath.IsVisible(point))
            {
                SendNCWinMessage(USER32.WM_GETSYSMENU, IntPtr.Zero, lParam);
            }
            else
            {
             */ 
                if (this.closeButtonPath.IsVisible(point))
                    code = USER32.SysCommand.SC_CLOSE;
                else if (IsResizable && this.maxButtonPath.IsVisible(point))
                    code = form.WindowState == FormWindowState.Normal ? USER32.SysCommand.SC_MAXIMIZE : USER32.SysCommand.SC_RESTORE;
                else if (IsMinimizable && this.minButtonPath.IsVisible(point))
                    code = USER32.SysCommand.SC_MINIMIZE;

                SendNCWinMessage(USER32.WM_SYSCOMMAND, (IntPtr)code, IntPtr.Zero);
            //}
        }

        internal void OnNonClientMouseMove(IntPtr lParam)
        {
            //Logger.Trace("called");

            Point point = form.PointToClient(new Point(lParam.ToInt32()));
            String tooltip;
            if (this.closeButtonPath.IsVisible(point))
            {
                tooltip = "Close";
            }
            else if (IsResizable && this.maxButtonPath.IsVisible(point))
            {
                tooltip = form.WindowState == FormWindowState.Normal ? "Maximize" : "Restore";
            }
            else if (IsMinimizable && this.minButtonPath.IsVisible(point))
                tooltip = "Minimize";
            else
                tooltip = string.Empty;

            if (ButtonTip.GetToolTip(form) != tooltip)
                ButtonTip.SetToolTip(form, tooltip);

            form.Invalidate(Rectangle.Round(titleBarPath.GetBounds()));
        }

        internal void OnNonClientRButtonUp(IntPtr lParam)
        {
            if (this.titleBarPath.IsVisible(form.PointToClient(new Point(lParam.ToInt32()))))
                SendNCWinMessage(USER32.WM_GETSYSMENU, IntPtr.Zero, lParam);
        }

        internal void PaintForm(PaintEventArgs e, string mainTitle, string subTitle)
        {
            Rectangle rc;

            if (titleBarPath != null) // added null check in case this was disposed
            {
                rc = Rectangle.Round(titleBarPath.GetBounds());
                using (TextureBrush brush = new TextureBrush(Resources.titlebar_tile, WrapMode.Tile))
                    e.Graphics.FillRectangle(brush, rc);
            }

            /*
            e.Graphics.FillPath(Brushes.Orange, leftEdge);
            e.Graphics.FillPath(Brushes.Orange, topEdgePath);
            e.Graphics.FillPath(Brushes.Orange, rightEdge);
            e.Graphics.FillPath(Brushes.Orange, bottomEdge);

            e.Graphics.FillPath(Brushes.Coral, topLeftEdgePath);
            e.Graphics.FillPath(Brushes.Coral, topRightEdgePath);
            e.Graphics.FillPath(Brushes.Coral, bottomLeftEdge);
            e.Graphics.FillPath(Brushes.Coral, bottomRightEdge);
            */

            // Icon
            /* Disabled because it looks like shit
            rc = Rectangle.Round(iconButtonPath.GetBounds());
            using (Bitmap bm = Resources.icon_16x16)
            {
                e.Graphics.DrawImageUnscaled(bm, rc.X, rc.Y);
            }
             */ 

            Point mousePt = form.PointToClient(Cursor.Position);

            // Minimize button
            if (IsMinimizable && minButtonPath != null && minimizeDividerPath != null)
            {
                rc = Rectangle.Round(minButtonPath.GetBounds());
                if (this.minButtonPath.IsVisible(mousePt))
                    using (Bitmap bm = Resources.titlebar_minimize_icon_hover)
                        e.Graphics.DrawImage(bm, rc);
                else
                    using (Bitmap bm = Resources.titlebar_minimize_icon)
                        e.Graphics.DrawImage(bm, rc);

                rc = Rectangle.Round(minimizeDividerPath.GetBounds());
                using (Bitmap bm = Resources.titlebar_2px_vertical_divider)
                    e.Graphics.DrawImage(bm, rc);
            }

            // Maximize button
            rc = Rectangle.Round(maxButtonPath.GetBounds());
            if (IsResizable && maxButtonPath != null)
            {
                if (this.maxButtonPath.IsVisible(mousePt))
                    using (Bitmap bm = form.WindowState == FormWindowState.Normal ? Resources.titlebar_maximize_icon_hover : Resources.titlebar_restore_icon_hover)
                    {
                        e.Graphics.DrawImage(bm, rc);
                    }
                else
                    using (Bitmap bm = form.WindowState == FormWindowState.Normal ? Resources.titlebar_maximize_icon : Resources.titlebar_restore_icon)
                    {
                        e.Graphics.DrawImage(bm, rc);
                    }
            }

            if (maximizeDividerPath != null)
            {
                rc = Rectangle.Round(maximizeDividerPath.GetBounds());
                using (Bitmap bm = Resources.titlebar_2px_vertical_divider)
                    e.Graphics.DrawImage(bm, rc);
            }

            // Close button
            if (closeButtonPath != null && closeDividerPath != null)
            {
                rc = Rectangle.Round(closeButtonPath.GetBounds());
                if (this.closeButtonPath.IsVisible(mousePt))
                    using (Bitmap bm = Resources.titlebar_close_icon_hover)
                        e.Graphics.DrawImage(bm, rc);
                else
                    using (Bitmap bm = Resources.titlebar_close_icon)
                        e.Graphics.DrawImage(bm, rc);

                rc = Rectangle.Round(closeDividerPath.GetBounds());
                using (Bitmap bm = Resources.titlebar_2px_vertical_divider)
                    e.Graphics.DrawImage(bm, rc);
            }

            /*
            using (Pen myPen = new Pen(Color.Orange, 2))
            {
                rc = this.DisplayRectangle;
                e.Graphics.DrawLine(myPen, rc.Left, rc.Top - 2, rc.Right, rc.Top - 2);
            }
             */

            Size titleSize;
            var drawingPoint = new Point(3, 6);
            using (var font = new Font(DesignLanguage.BoldFont, 10, FontStyle.Bold))
            {
                titleSize = TextRenderer.MeasureText(mainTitle, font);
                TextRenderer.DrawText(e.Graphics, mainTitle, font,
                    drawingPoint, DesignLanguage.LightGray, Color.Transparent);
            }

            using (var font = new Font(DesignLanguage.NormalFont, 10, FontStyle.Regular))
            {
                if (String.IsNullOrEmpty(subTitle) == false)
                    TextRenderer.DrawText(e.Graphics, subTitle, font, new Point(drawingPoint.X + titleSize.Width - 3, drawingPoint.Y),
                        DesignLanguage.LightGray, Color.Transparent, TextFormatFlags.NoPrefix); // NoPrefix ignores amperstand
            }

            if (!IsFormActive && titleBarPath != null) // draw it as overlay
            {
                rc = Rectangle.Round(titleBarPath.GetBounds());
                //using (TextureBrush brush = new TextureBrush(Resources.titlebar_inactive_overlay_1px_transparent, WrapMode.Tile))
                using (var brush = new SolidBrush(Color.FromArgb(20, Color.White)))
                    e.Graphics.FillRectangle(brush, rc);
            }

            /*
            RectangleF textRect = titleBarPath.GetBounds();
            textRect.X += iconButtonPath.GetBounds().Width + 3;
            textRect.Width = minButtonPath.GetBounds().Left - textRect.Left;
            TextRenderer.DrawText(e.Graphics, this.Text, SystemFonts.CaptionFont, Rectangle.Round(textRect), Color.DarkGoldenrod, TextFormatFlags.EndEllipsis | TextFormatFlags.VerticalCenter);
            */
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                bottomEdge.Dispose();
                topEdgePath.Dispose();
                topLeftEdgePath.Dispose();
                topRightEdgePath.Dispose();
                titleBarPath.Dispose();
                closeButtonPath.Dispose();
                closeDividerPath.Dispose();
                maxButtonPath.Dispose();
                maximizeDividerPath.Dispose();
                minButtonPath.Dispose();
                minimizeDividerPath.Dispose();
                //iconButtonPath.Dispose();
                leftEdge.Dispose();
                bottomEdge.Dispose();
                rightEdge.Dispose();
                bottomLeftEdge.Dispose();
                bottomRightEdge.Dispose();

                SystemMenu.Dispose();

                ButtonTip.Dispose();
            }
        }

        protected void SystemMenu_SystemEvent(object sender, WindowMenuEventArgs ev)
        {
            SendNCWinMessage(USER32.WM_SYSCOMMAND, (IntPtr)ev.SystemCommand, IntPtr.Zero);
        }
    }
}
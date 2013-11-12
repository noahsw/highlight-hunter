using System;
using System.Drawing;
using System.Windows.Forms;

namespace OdessaGUIProject.UI_Helpers
{
    public delegate void WindowMenuEventHandler(object sender, WindowMenuEventArgs ev);

    public class WindowMenuEventArgs : EventArgs
    {
        private readonly int systemCommand;

        public WindowMenuEventArgs(int command)
            : base()
        {
            this.systemCommand = command;
        }

        public int SystemCommand
        {
            get { return (int)this.systemCommand; }
        }
    }

    internal sealed class WindowMenu : ContextMenu
    {
        private MenuItem menuRestore, menuMove, menuSize, menuMin, menuMax, menuSep, menuClose;
        private Form Owner;

        public WindowMenu(Form owner)
            : base()
        {
            Owner = owner;

            menuRestore = CreateMenuItem("Restore");
            menuMove = CreateMenuItem("Move");
            menuSize = CreateMenuItem("Size");
            menuMin = CreateMenuItem("Minimize");
            menuMax = CreateMenuItem("Maximize");
            menuSep = CreateMenuItem("-");
            menuClose = CreateMenuItem("Close", Shortcut.AltF4);

            this.MenuItems.AddRange(new MenuItem[] { menuRestore, menuMove, menuSize, menuMin, menuMax, menuSep, menuClose });

            menuClose.DefaultItem = true;
        }

        public event WindowMenuEventHandler SystemEvent;

        protected override void OnPopup(EventArgs e)
        {
            switch (Owner.WindowState)
            {
                case FormWindowState.Normal:
                    menuRestore.Enabled = false;
                    menuMax.Enabled = true;
                    menuMin.Enabled = true;
                    menuMove.Enabled = true;
                    menuSize.Enabled = true;
                    break;

                case FormWindowState.Minimized:
                    menuRestore.Enabled = true;
                    menuMax.Enabled = true;
                    menuMin.Enabled = false;
                    menuMove.Enabled = false;
                    menuSize.Enabled = false;
                    break;

                case FormWindowState.Maximized:
                    menuRestore.Enabled = true;
                    menuMax.Enabled = false;
                    menuMin.Enabled = true;
                    menuMove.Enabled = false;
                    menuSize.Enabled = false;
                    break;
            }
            base.OnPopup(e);
        }

        private MenuItem CreateMenuItem(string text)
        {
            return CreateMenuItem(text, Shortcut.None);
        }

        private MenuItem CreateMenuItem(string text, Shortcut shortcut)
        {
            MenuItem item = new MenuItem(text, OnWindowMenuClick, shortcut);
            item.OwnerDraw = true;
            item.MeasureItem += new MeasureItemEventHandler(item_MeasureItem);
            item.DrawItem += new DrawItemEventHandler(item_DrawItem);
            return item;
        }

        private void item_DrawItem(object sender, DrawItemEventArgs e)
        {
            MenuItem item = this.MenuItems[e.Index];

            if (item.Enabled)
                e.DrawBackground();
            else
                e.Graphics.FillRectangle(SystemBrushes.Menu, e.Bounds);

            if (e.Index == 5)
                e.Graphics.DrawLine(SystemPens.GrayText, e.Bounds.Left + 2, e.Bounds.Top + 3, e.Bounds.Right - 2, e.Bounds.Top + 3);
            else
            {
                TextFormatFlags format = TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPadding;

                Color textColor = item.Enabled ? SystemColors.MenuText : SystemColors.GrayText;

                using (Font marlettFont = new Font("Marlett", 10))
                {
                    Rectangle GlyphRect = e.Bounds;
                    GlyphRect.Width = GlyphRect.Height;

                    if (item == menuRestore)
                        TextRenderer.DrawText(e.Graphics, "2", marlettFont, GlyphRect, textColor, format);
                    else if (item == menuMin)
                        TextRenderer.DrawText(e.Graphics, "0", marlettFont, GlyphRect, textColor, format);
                    else if (item == menuMax)
                        TextRenderer.DrawText(e.Graphics, "1", marlettFont, GlyphRect, textColor, format);
                    else if (item == menuClose)
                        TextRenderer.DrawText(e.Graphics, "r", marlettFont, GlyphRect, textColor, format);
                }

                format &= TextFormatFlags.Left | ~TextFormatFlags.HorizontalCenter;

                Rectangle textRect = new Rectangle(e.Bounds.Left + e.Bounds.Height + 3, e.Bounds.Top, e.Bounds.Width - e.Bounds.Height - 3, e.Bounds.Height);

                TextRenderer.DrawText(e.Graphics, item.Text, SystemFonts.MenuFont, textRect, textColor, format);

                if (item == menuClose)
                {
                    String shortcut = "Alt+F4";
                    Size shortcutSize = TextRenderer.MeasureText(shortcut, SystemFonts.MenuFont);
                    textRect.X = textRect.Right - shortcutSize.Width - 13;
                    TextRenderer.DrawText(e.Graphics, shortcut, SystemFonts.MenuFont, textRect, textColor, format);
                }
            }
        }

        private void item_MeasureItem(object sender, MeasureItemEventArgs e)
        {
            MenuItem item = this.MenuItems[e.Index];
            String itemText = item.Text;
            itemText += "/tAlt+F4";
            Size itemSize = TextRenderer.MeasureText(itemText, SystemFonts.MenuFont);
            e.ItemHeight = e.Index == 5 ? 8 : itemSize.Height + 7;
            e.ItemWidth = itemSize.Width + itemSize.Height + 23;
        }

        private void OnWindowMenuClick(object sender, EventArgs e)
        {
            switch (this.MenuItems.IndexOf((MenuItem)sender))
            {
                case 0:
                    SendSysCommand(USER32.SysCommand.SC_RESTORE);
                    break;

                case 1:
                    SendSysCommand(USER32.SysCommand.SC_MOVE);
                    break;

                case 2:
                    SendSysCommand(USER32.SysCommand.SC_SIZE);
                    break;

                case 3:
                    SendSysCommand(USER32.SysCommand.SC_MINIMIZE);
                    break;

                case 4:
                    SendSysCommand(USER32.SysCommand.SC_MAXIMIZE);
                    break;

                case 6:
                    SendSysCommand(USER32.SysCommand.SC_CLOSE);
                    break;
            }
        }

        private void SendSysCommand(USER32.SysCommand command)
        {
            if (this.SystemEvent != null)
            {
                WindowMenuEventArgs ev = new WindowMenuEventArgs((int)command);
                this.SystemEvent(this, ev);
            }
        }
    }
}
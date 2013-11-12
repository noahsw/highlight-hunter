using System;

namespace OdessaGUIProject.UI_Helpers
{
    internal static class USER32
    {
// ReSharper disable InconsistentNaming
        public const int WM_ENTERMENULOOP = 0x211;

        public const int WM_EXITMENULOOP = 0x212;

        //********** Undocumented message **********
        public const int WM_GETSYSMENU = 0x313;

        public const int WM_NCACTIVATE = 0x86;

        public const int WM_NCHITTEST = 0x84;

        public const int WM_NULL = 0x0;

        public const int WM_SYSCOMMAND = 0x112;

        public enum NCHitTestResult
        {
            HTERROR = (-2),
            HTTRANSPARENT,
            HTNOWHERE,
            HTCLIENT,
            HTCAPTION,
            HTSYSMENU,
            HTGROWBOX,
            HTMENU,
            HTHSCROLL,
            HTVSCROLL,
            HTMINBUTTON,
            HTMAXBUTTON,
            HTLEFT,
            HTRIGHT,
            HTTOP,
            HTTOPLEFT,
            HTTOPRIGHT,
            HTBOTTOM,
            HTBOTTOMLEFT,
            HTBOTTOMRIGHT,
            HTBORDER,
            HTOBJECT,
            HTCLOSE,
            HTHELP
        }

        public enum NCMouseMessage
        {
            WM_NCMOUSEMOVE = 0xA0,
            WM_NCLBUTTONDOWN = 0xA1,
            WM_NCLBUTTONUP = 0xA2,
            WM_NCLBUTTONDBLCLK = 0xA3,
            WM_NCRBUTTONDOWN = 0xA4,
            WM_NCRBUTTONUP = 0xA5,
            WM_NCRBUTTONDBLCLK = 0xA6,
            WM_NCMBUTTONDOWN = 0xA7,
            WM_NCMBUTTONUP = 0xA8,
            WM_NCMBUTTONDBLCLK = 0xA9,

            WM_NCXBUTTONDOWN = 0xAB,
            WM_NCXBUTTONUP = 0xAC,
            WM_NCXBUTTONDBLCLK = 0xAD
        }

        public enum SysCommand
        {
            SC_SIZE = 0xF000,
            SC_MOVE = 0xF010,
            SC_MINIMIZE = 0xF020,
            SC_MAXIMIZE = 0xF030,
            SC_NEXTWINDOW = 0xF040,
            SC_PREVWINDOW = 0xF050,
            SC_CLOSE = 0xF060,
            SC_VSCROLL = 0xF070,
            SC_HSCROLL = 0xF080,
            SC_MOUSEMENU = 0xF090,
            SC_KEYMENU = 0xF100,
            SC_ARRANGE = 0xF110,
            SC_RESTORE = 0xF120,
            SC_TASKLIST = 0xF130,
            SC_SCREENSAVE = 0xF140,
            SC_HOTKEY = 0xF150,
            SC_DEFAULT = 0xF160,
            SC_MONITORPOWER = 0xF170,
            SC_CONTEXTHELP = 0xF180,
            SC_SEPARATOR = 0xF00F
        }
// ReSharper restore InconsistentNaming

        //*****************************************

        public static IntPtr MakeLParam(int LoWord, int HiWord)
        {
            return (IntPtr)(((HiWord << 16) | (LoWord & 0xFFFF)));
        }
    }
}
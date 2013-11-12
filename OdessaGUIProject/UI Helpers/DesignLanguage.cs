using System;
using System.Drawing;
using System.Drawing.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using OdessaGUIProject.Properties;

namespace OdessaGUIProject.UI_Helpers
{
    internal static class DesignLanguage
    {

        [DllImport("gdi32")]
        private static extern IntPtr
         AddFontMemResourceEx(IntPtr pbFont,
         uint cbFont,
         IntPtr pdv,
         [In] ref uint pcFonts);

        [DllImport("gdi32")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool
         RemoveFontMemResourceEx(IntPtr fh);

        private static IntPtr[] m_hFont;

        internal static FontFamily BoldFont;

        /// <summary>
        /// #CCCCCC => 204, 204, 204
        /// </summary>
        internal static Color LightGray = Color.FromArgb(204, 204, 204);

        /// <summary>
        /// 50, 50, 50
        /// </summary>
        internal static Color MidGray = Color.FromArgb(50, 50, 50);

        /// <summary>
        /// #999999 => 153, 153, 153
        /// </summary>
        internal static Color DarkGray = Color.FromArgb(153, 153, 153);

        internal static FontFamily NormalFont;
        internal static Color PrimaryGreen = Color.FromArgb(153, 204, 51);

        /// <summary>
        /// 51, 153, 204
        /// </summary>
        internal static Color SecondaryBlue = Color.FromArgb(51, 153, 204);

        private static bool isInitialized;
        private static readonly PrivateFontCollection pfc = new PrivateFontCollection();

        internal static void ApplyCustomFont(Control.ControlCollection controls)
        {
            if (isInitialized == false)
                InitializeCollection();

            foreach (Control c in controls)
            {
                var p = c as Panel;
                if (p != null)
                {
                    ApplyCustomFont(p.Controls);
                    continue;
                }

                ApplyCustomFont(c);

                /* REMED this out because we may pay a penalty to convert variable c into various control types

                Label l = c as Label;
                if (l != null)
                {
                    ApplyCustomFont(l);
                    continue;
                }

                LinkLabel ll = c as LinkLabel;
                if (ll != null)
                {
                    ApplyCustomFont(ll);
                    continue;
                }

                TextBox t = c as TextBox;
                if (t != null)
                {
                    ApplyCustomFont(t);
                    continue;
                }

                CustomRadioButton crb = c as CustomRadioButton;
                if (crb != null)
                {
                    ApplyCustomFont(crb);
                    continue;
                }

                RichTextBox rt = c as RichTextBox;
                if (rt != null)
                {
                    ApplyCustomFont(rt);
                    continue;
                }
                */
            }
        }

        internal static void ApplyCustomFont(Control control)
        {
            if (control.Font.Bold)
                control.Font = new Font(BoldFont, control.Font.Size, control.Font.Style);
            else
                control.Font = new Font(NormalFont, control.Font.Size, control.Font.Style);
        }

        private static void AddFont(byte[] fontData, int index)
        {
            //var m_pFont = GCHandle.Alloc(fontData, GCHandleType.Pinned);
            //m_PFC.AddMemoryFont(m_pFont.AddrOfPinnedObject(), rsxLen);
            uint rsxCnt = 1; /* We're only installing one font. */

            // This is where we do the actual "registration" to get the font handle
            IntPtr fontPtr = Marshal.AllocCoTaskMem(fontData.Length);

            Marshal.Copy(fontData, 0, fontPtr, fontData.Length);

            pfc.AddMemoryFont(fontPtr, fontData.Length);

            m_hFont[index] = AddFontMemResourceEx(fontPtr, (uint)fontData.Length, IntPtr.Zero, ref rsxCnt);

            Marshal.FreeCoTaskMem(fontPtr);
        }

        private static void InitializeCollection()
        {
            m_hFont = new IntPtr[2];

            AddFont(Resources.OpenSans_Regular, 0); // better for normal
            NormalFont = pfc.Families[0];

            AddFont(Resources.OpenSans_Bold, 1); // better for bold
            BoldFont = pfc.Families[0];

            isInitialized = true;
        }

        internal static void Dispose()
        {
            pfc.Dispose();
            RemoveFontMemResourceEx(m_hFont[0]);
            RemoveFontMemResourceEx(m_hFont[1]);
        }
    }
}
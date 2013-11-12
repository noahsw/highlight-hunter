using System;
using System.Windows.Forms;

namespace OdessaGUIProject.UI_Helpers
{
    // http://stackoverflow.com/questions/302663/cursor-current-vs-this-cursor-in-net-c
    public class HourGlass : IDisposable
    {
        public HourGlass()
        {
            //logger.Trace("HourGlass created");
            Enabled = true;
        }

        public static bool Enabled
        {
            get { return Application.UseWaitCursor; }
            set
            {
                if (value == Application.UseWaitCursor) return;
                Application.UseWaitCursor = value;
                Form f = Form.ActiveForm;
                if (f != null && !f.InvokeRequired && f.Handle != null)   // Send WM_SETCURSOR
                    SendMessage(f.Handle, 0x20, f.Handle, (IntPtr)1);
            }
        }

        public void Dispose()
        {
            Enabled = false;
            //logger.Trace("HourGlass disposed");
        }

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wp, IntPtr lp);
    }
}
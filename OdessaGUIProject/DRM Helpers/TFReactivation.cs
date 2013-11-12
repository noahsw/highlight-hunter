// software_DNA C# Code Sample
// Version 3.3.5
// January 12 2007

using System;
using System.Drawing;
using System.Security.Permissions;
using System.Windows.Forms;
using OdessaGUIProject.UI_Helpers;

namespace OdessaGUIProject.DRM_Helpers
{
    public sealed partial class TFReactivation : Form
    {
        private readonly BorderlessWindow borderlessWindow;

        public TFReactivation()
        {
            InitializeComponent();

            DesignLanguage.ApplyCustomFont(this.Controls);

            #region Borderless window

            borderlessWindow = new BorderlessWindow(this, false, false);
            borderlessWindow.IsResizable = false;
            borderlessWindow.SendNCWinMessage += SendNCWinMessage;
            this.MaximizedBounds = this.DisplayRectangle;

            #endregion Borderless window
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                borderlessWindow.Dispose();
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Borderless Window

        protected override CreateParams CreateParams
        {
            [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)] // http://msdn.microsoft.com/library/ms182305(VS.100).aspx
            get
            {
                const int WS_MINIMIZEBOX = 0x20000;
                const int CS_DROPSHADOW = 0x20000;
                const int CS_DBLCLKS = 0x8;

                System.Windows.Forms.CreateParams cParams = base.CreateParams;

                int ClassFlags = CS_DBLCLKS;
                int OSVER = Environment.OSVersion.Version.Major * 10;
                OSVER += Environment.OSVersion.Version.Minor;
                if (OSVER >= 51) ClassFlags = CS_DROPSHADOW | CS_DBLCLKS;

                cParams.ClassStyle = ClassFlags;
                cParams.Style |= WS_MINIMIZEBOX;
                return cParams;
            }
        }

        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        protected override void WndProc(ref Message m)
        {
            if (this.WindowState == FormWindowState.Maximized)
            {
                if (m.Msg == USER32.WM_SYSCOMMAND &&
                    m.WParam.ToInt32() == (int)USER32.SysCommand.SC_MOVE ||
                    m.Msg == (int)USER32.NCMouseMessage.WM_NCLBUTTONDOWN &&
                    m.WParam.ToInt32() == (int)USER32.NCHitTestResult.HTCAPTION)
                {
                    m.Msg = USER32.WM_NULL;
                }
            }

            base.WndProc(ref m);

            //Logger.Trace("Message from MainForm: " + m.Msg);

            switch (m.Msg)
            {
                case (int)USER32.WM_GETSYSMENU:
                    borderlessWindow.SystemMenu.Show(this, this.PointToClient(new Point(m.LParam.ToInt32())));
                    break;

                case USER32.WM_NCACTIVATE:
                    borderlessWindow.IsFormActive = m.WParam.ToInt32() != 0;
                    this.Invalidate();
                    break;

                case USER32.WM_NCHITTEST:
                    m.Result = borderlessWindow.OnNonClientHitTest(m.LParam);
                    break;

                case (int)USER32.NCMouseMessage.WM_NCLBUTTONUP:
                    borderlessWindow.OnNonClientLButtonUp(m.LParam);
                    break;

                case (int)USER32.NCMouseMessage.WM_NCRBUTTONUP:
                    borderlessWindow.OnNonClientRButtonUp(m.LParam);
                    break;

                case (int)USER32.NCMouseMessage.WM_NCMOUSEMOVE:
                    borderlessWindow.OnNonClientMouseMove(m.LParam);
                    break;
                default:
                    break;
            }
        }

        private void SendNCWinMessage(int msg, IntPtr wParam, IntPtr lParam)
        {
            Message message = Message.Create(this.Handle, msg, wParam, lParam);
            this.WndProc(ref message);
        }

        #endregion Borderless Window

        private void btnReactivate_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.None;

            // input fields should be validated here (non-blank and legal characters
            if ((tbCurrentPass.Text.Trim() == "") | (tbNewPass.Text.Trim() == "") | (tbConfPass.Text.Trim() == ""))
            {
                MessageBox.Show("Please enter all the information.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (tbNewPass.Text.Trim() != tbConfPass.Text.Trim())
            {
                MessageBox.Show("Your New and Confirm passwords must match", "Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (Protection.IsValidPassword(tbCurrentPass.Text) == false)
            {
                ProtectionWarnings.WarnAboutInvalidPassword();
                tbCurrentPass.Focus();
                return;
            }

            if (Protection.IsValidPassword(tbNewPass.Text) == false)
            {
                ProtectionWarnings.WarnAboutInvalidPassword();
                tbNewPass.Focus();
                return;
            }

            //return
            DialogResult = DialogResult.OK;
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void sendPasswordLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // resend password to the email address used at activation of this code
            string associatedProductKey = Protection.GetAssociatedProductKey(lblCode.Text);
            if (!Protection.IsValidProductKey(associatedProductKey, lblCode.Text))
                return;

            int err = DNA.DNA_SendPassword(associatedProductKey, lblCode.Text);
            if (err == 0)
            {
                MessageBox.Show("Password has been sent", "Information",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("No Email address available to send Password", "Information",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void TFReactivation_Load(object sender, EventArgs e)
        {
        }

        private void TFReactivation_Paint(object sender, PaintEventArgs e)
        {
            borderlessWindow.PaintForm(e, "Activation", "");
        }

        private void TFReactivation_Resize(object sender, EventArgs e)
        {
            if (this.Created)
            {
                borderlessWindow.BuildPaths();
                this.Invalidate();
            }
        }
    }
}
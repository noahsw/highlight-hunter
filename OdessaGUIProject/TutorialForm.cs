using System;
using System.Drawing;
using System.Security.Permissions;
using System.Windows.Forms;
using NLog;
using OdessaGUIProject.UI_Helpers;

namespace OdessaGUIProject
{
    public sealed partial class TutorialForm : Form
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly BorderlessWindow borderlessWindow;

        public TutorialForm()
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

        internal EventHandler CloseHandler { get; set; }

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

        private void continueButton_Click(object sender, EventArgs e)
        {
            //Properties.Settings.Default.ShowFirstRunScreen = checkBoxShowEverytime.Checked;
            //Properties.Settings.Default.Save();

            Close();
        }

        private void likeThisPicture_Click(object sender, EventArgs e)
        {
            PlayTutorialVideo();
        }

        private void notLikeThisPicture_Click(object sender, EventArgs e)
        {
            PlayTutorialVideo();
        }

        private void PlayTutorialVideo()
        {
            BrowserHelper.LaunchBrowser(BrowserHelper.Host + "/tutorial-redirect.php");
        }

        private void TutorialForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Properties.Settings.Default.ShowFirstRunScreenV2 = false;
            Properties.Settings.Default.Save();

            if (CloseHandler != null)
                CloseHandler.Invoke(sender, e);
        }

        private void TutorialForm_Load(object sender, EventArgs e)
        {
            DesignLanguage.ApplyCustomFont(this);
            coverLikeThisLabel.Width = notLikeThisLabel.Width = likeThisPicture.Width;

            // set this to check locally for perf reasons
            /* REMED to match Mac functionality for now
            if (Protection.GetLicenseStatus(localOnly: true) == Protection.ActivationState.Unlicensed)
            {
                continueButton.Text = "Begin &Trial";
                emailTextBox.Visible = true;
            }
            */
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

        private void TutorialForm_Paint(object sender, PaintEventArgs e)
        {
            borderlessWindow.PaintForm(e, "How to use Highlight Hunter", "");
        }

        private void TutorialForm_Resize(object sender, EventArgs e)
        {
            if (this.Created)
            {
                borderlessWindow.BuildPaths();
                this.Invalidate();
            }
        }
    }
}
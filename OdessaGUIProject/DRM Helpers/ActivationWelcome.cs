using System;
using System.Diagnostics;
using System.Drawing;
using System.Security.Permissions;
using System.Windows.Forms;
using NLog;
using OdessaGUIProject.UI_Helpers;
using GaDotNet.Common.Helpers;

namespace OdessaGUIProject.DRM_Helpers
{
    public partial class ActivationWelcome : Form
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly BorderlessWindow borderlessWindow;

        public ActivationWelcome()
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

        private void activateCodeTextBox_MouseClick(object sender, MouseEventArgs e)
        {
            if (activateRadioButton.Checked == false)
                activateRadioButton.Checked = true;
        }

        private void activateCodeTextBox_TextChanged(object sender, EventArgs e)
        {
            if (activateRadioButton.Checked == false)
                activateRadioButton.Checked = true;
        }

        private void activationPictureBox_Click(object sender, EventArgs e)
        {
            BrowserHelper.LaunchBrowser("http://support.highlighthunter.com/customer/portal/articles/325266-what-is-activation-", "activationquestion");
        }

        private void ActivationWelcome_Load(object sender, EventArgs e)
        {
            var activationToolTip = new ToolTip();
            activationToolTip.SetToolTip(activationQuestionPictureBox, "What is an activation code?");

            switch (Protection.GetLicenseStatus())
            {
                case Protection.ActivationState.Trial:
                    int daysLeft = Protection.GetDaysLeftInTrial();
                    trialRadioButton.Text = "Try Pro version for " + daysLeft + " more day" + (daysLeft == 1 ? "" : "s");
                    trialRadioButton.Checked = true;
                    break;

                case Protection.ActivationState.Activated:
                    trialRadioButton.Enabled = false;
                    trialRadioButton.Checked = false;
                    freeVersionRadioButton.Enabled = false;
                    activateRadioButton.Checked = true;
                    break;

                case Protection.ActivationState.TrialExpired:
                    trialRadioButton.Text = "Trial expired!";
                    trialRadioButton.Enabled = false;
                    trialRadioButton.Checked = false;
                    purchaseRadioButton.Checked = true;
                    break;

                case Protection.ActivationState.Unlicensed:
                    freeVersionRadioButton.Checked = false;
                    trialRadioButton.Checked = true;
                    break;

                default:
                    Logger.Error("Case not accounted for!");
                    Debug.Assert(false, "Case not accounted for!");
                    break;
            }
        }

        private void ActivationWelcome_Paint(object sender, PaintEventArgs e)
        {
            borderlessWindow.PaintForm(e, "Activation Options", "");
        }

        private void ActivationWelcome_Resize(object sender, EventArgs e)
        {
            if (this.Created)
            {
                borderlessWindow.BuildPaths();
                this.Invalidate();
            }
        }

        private void continueButton_Click(object sender, EventArgs e)
        {
            Protection.ActivationState originalActivationState = Protection.GetLicenseStatus();

            if (trialRadioButton.Checked)
            {
                if (Protection.GetLicenseStatus() != Protection.ActivationState.Unlicensed)
                { // already under trial or in an invalid state
                    Close();
                    return;
                }

                Protection.BeginTrial();
            }

            if (purchaseRadioButton.Checked)
            {
                Purchase();
            }

            if (activateRadioButton.Checked)
            {
                Protection.ActivateApp(activateCodeTextBox.Text, this);
                activateCodeTextBox.Focus();
            }

            if (freeVersionRadioButton.Checked)
            {
                Close();
            }

            // only close if license state changed
            if (Protection.GetLicenseStatus() != originalActivationState)
                Close();
        }

        private void emailTextBox_MouseClick(object sender, MouseEventArgs e)
        {
            if (trialRadioButton.Checked == false)
                trialRadioButton.Checked = true;
        }

        private void emailTextBox_TextChanged(object sender, EventArgs e)
        {
            if (trialRadioButton.Checked == false)
                trialRadioButton.Checked = true;
        }

        private void Purchase()
        {
            BrowserHelper.LaunchPurchasingOptions();
        }

        private void RunFreeVersion()
        {
            if (Protection.GetLicenseStatus() == Protection.ActivationState.Trial)
            { // warn user if they want to end evaluation
                int daysLeft = Protection.GetDaysLeftInTrial();
                if (MessageBox.Show("You still have " + daysLeft + " day" + (daysLeft == 1 ? "" : "s") + " left in your trial!" + Environment.NewLine + Environment.NewLine +
                    "Are you sure you want to end your trial and run the limited version?",
                    "End trial?",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question)
                    == DialogResult.Yes)
                {
                    if (Protection.EndTrial())
                    {
                        MessageBox.Show("Your trial has been ended! Don't worry, you can always purchase an activation code at a later time.",
                            "Trial ended",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Unfortunately we hit a snag ending your trial. Don't worry, this won't limit what you can do.",
                            "Error ending trial",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                    }
                }
            }

            DialogResult = DialogResult.OK;
        }

        private void trialQuestionPictureBox_Click(object sender, EventArgs e)
        {
            BrowserHelper.LaunchBrowser("http://support.highlighthunter.com/customer/portal/articles/325284-what-is-the-email-address-for-and-is-it-mandatory-", "trialquestion");
        }

        private void ActivationWelcome_Shown(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.HasOpenedActivationOptions == false)
            {
                if (AnalyticsHelper.FireEvent("First activation options"))
                {
                    Properties.Settings.Default.HasOpenedActivationOptions = true;
                    Properties.Settings.Default.Save();
                }
            }
        }
    }
}
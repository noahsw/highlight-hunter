using System;
using System.Drawing;
using System.Globalization;
using System.Security.Permissions;
using System.Windows.Forms;
using OdessaGUIProject.DRM_Helpers;
using OdessaGUIProject.UI_Helpers;
using OdessaGUIProject.Workers;

namespace OdessaGUIProject
{
    public sealed partial class SettingsForm : Form
    {
        private readonly BorderlessWindow borderlessWindow;

        public SettingsForm()
        {
            InitializeComponent();

            DesignLanguage.ApplyCustomFont(this.Controls);

            #region Borderless window

            borderlessWindow = new BorderlessWindow(this, false, false);
            borderlessWindow.SendNCWinMessage += SendNCWinMessage;
            this.MaximizedBounds = this.DisplayRectangle;

            #endregion Borderless window


            var highlightDurationTooltip = new ToolTip();
            highlightDurationTooltip.SetToolTip(helpHighlightLength, "How far back before you cover the lens should we start the highlight? Click additional help.");

            var scanSensitivityTooltip = new ToolTip();
            scanSensitivityTooltip.SetToolTip(helpScanSensitivity, "How sensitive should the scanning engine be when looking for marks? Click additional help.");

            var advancedOptionsTooltip = new ToolTip();
            advancedOptionsTooltip.SetToolTip(helpAdvancedOptionsButton, "Advanced settings that let you take total control over Highlight Hunter. Click additional help.");

            var licenseTooltip = new ToolTip();
            licenseTooltip.SetToolTip(helpLicenseButton, "Upgrade to Highlight Hunter Pro to enjoy several professional-level features. Click for additional help.");

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

        private void activateLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            using (var dimmerMask = new DimmerMask(this))
            {
                dimmerMask.Show(this);
                using (var awForm = new ActivationWelcome())
                {
                    awForm.ShowDialog();
                }
            }

            UpdateLicenseUI();
        }


        private void okPictureButtonControl_Click(object sender, EventArgs e)
        {
            //string newHandThresholdOption = ((MainModel.DetectionThresholds) detectionThresholdTrackBar.Value).ToString();
            Properties.Settings.Default.HandThresholdOption = (int)sensitivitySliderControl.DetectionThreshold;
            MainModel.DetectionThreshold = sensitivitySliderControl.DetectionThreshold;

            Properties.Settings.Default.IgnoreEarlyHighlights = ignoreEarlyHighlights.Checked;

            var captureDurationMin = 0;
            var captureDurationSec = 0;
            bool isDurationValid = true;
            if (int.TryParse(highlightDurationMinutesTextBox.Text, out captureDurationMin) == false)
                captureDurationMin = 0;
            if (int.TryParse(highlightDurationSecondsTextBox.Text, out captureDurationSec) == false)
                captureDurationSec = 0;

            var captureDurationInSeconds = captureDurationMin * 60 + captureDurationSec;
            if (captureDurationInSeconds < 1 || !isDurationValid)
            {
                MessageBox.Show("Sorry - the default highlight length must be at least 1 second.", "Default highlight length", MessageBoxButtons.OK, MessageBoxIcon.Error);
                highlightDurationSecondsTextBox.Focus();
                return;
            }
            Properties.Settings.Default.CaptureDurationInSeconds = captureDurationInSeconds;

            if (saveAsProRes.Checked)
                Properties.Settings.Default.SaveOutputFormat = SaveWorker.OutputFormats.ProRes.ToString();
            else
                Properties.Settings.Default.SaveOutputFormat = SaveWorker.OutputFormats.Original.ToString();

            Properties.Settings.Default.Save();

            Close();
        }

        private void saveAsProRes_CheckedChanged(object sender, EventArgs e)
        {
            if (saveAsProRes.Checked)
            {
                var activationState = Protection.GetLicenseStatus();
                if (activationState == Protection.ActivationState.TrialExpired || activationState == Protection.ActivationState.Unlicensed)
                {
                    if (MessageBox.Show("Sorry, but that feature is only available in Highlight Hunter Pro." + Environment.NewLine + Environment.NewLine +
                        "Would you like to learn more about the Pro version?", "Pro version required", MessageBoxButtons.YesNo, MessageBoxIcon.Hand)
                        == DialogResult.Yes)
                    {
                        using (var awForm = new ActivationWelcome())
                        {
                            awForm.ShowDialog();
                        }
                        UpdateLicenseUI();
                    }
                }

                // see if status changed
                activationState = Protection.GetLicenseStatus();
                if (activationState == Protection.ActivationState.TrialExpired || activationState == Protection.ActivationState.Unlicensed)
                    saveAsProRes.Checked = false;
            }
        }

        private void Settings_Load(object sender, EventArgs e)
        {
            sensitivitySliderControl.DetectionThreshold = (MainModel.DetectionThresholds)Properties.Settings.Default.HandThresholdOption;

            ignoreEarlyHighlights.Checked = Properties.Settings.Default.IgnoreEarlyHighlights;

            highlightDurationMinutesTextBox.Text = (Properties.Settings.Default.CaptureDurationInSeconds / 60).ToString(CultureInfo.CurrentCulture);
            highlightDurationSecondsTextBox.Text = (Properties.Settings.Default.CaptureDurationInSeconds % 60).ToString(CultureInfo.CurrentCulture);

            var outputFormat = (SaveWorker.OutputFormats)Enum.Parse(typeof(SaveWorker.OutputFormats), Properties.Settings.Default.SaveOutputFormat);
            saveAsProRes.Checked = (outputFormat == SaveWorker.OutputFormats.ProRes);

            UpdateLicenseUI();
        }

        private void UpdateLicenseUI()
        {
            var activationState = Protection.GetLicenseStatus();
            switch (activationState)
            {
                case Protection.ActivationState.Activated:
                    currentLicenseLabel.Text = "Highlight Hunter Pro";
                    activateLinkLabel.Visible = false;
                    break;

                case Protection.ActivationState.Trial:
                    currentLicenseLabel.Text = "Highlight Hunter Pro Trial";
                    break;

                case Protection.ActivationState.TrialExpired:
                    currentLicenseLabel.Text = "Highlight Hunter Free (Trial Expired)";
                    break;
            }
        }

        private void SettingsForm_Paint(object sender, PaintEventArgs e)
        {
            borderlessWindow.PaintForm(e, "Settings", "");
        }

        private void SettingsForm_Resize(object sender, EventArgs e)
        {
            if (this.Created)
            {
                borderlessWindow.BuildPaths();
                this.Invalidate();
            }
        }

        private void helpHighlightLength_Click(object sender, EventArgs e)
        {
            BrowserHelper.LaunchBrowser("http://support.highlighthunter.com/customer/portal/articles/659089-what-is-the-default-highlight-length-", "settings-highlight-length");
        }

        private void helpScanSensitivity_Click(object sender, EventArgs e)
        {
            BrowserHelper.LaunchBrowser("http://support.highlighthunter.com/customer/portal/articles/659096-what-does-scan-sensitivity-mean-", "settings-scan-sensitivity");
        }

        private void helpAdvancedOptionsButton_Click(object sender, EventArgs e)
        {
            BrowserHelper.LaunchBrowser("http://support.highlighthunter.com/customer/portal/articles/659113-what-are-the-advanced-options-", "settings-advanced-options");
        }

        private void helpLicenseButton_Click(object sender, EventArgs e)
        {
            BrowserHelper.LaunchBrowser("http://support.highlighthunter.com/customer/portal/articles/325266-what-is-activation-", "settings-license");
        }

        private void highlightDurationMinutesTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsDigit(e.KeyChar) || e.KeyChar == '\b')
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }

        private void highlightDurationSecondsTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsDigit(e.KeyChar) || e.KeyChar == '\b')
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }


    }
}
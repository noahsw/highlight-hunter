using System;
using System.Windows.Forms;

namespace OdessaGUIProject.DRM_Helpers
{
    public static class ProtectionWarnings
    {
        public static void WarnAboutInvalidPassword()
        {
            MessageBox.Show("Woops! Passwords must be between 4 and 16 characters long and can only include numbers, letters, dashes, and the @ sign.",
                "Invalid password",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public static void WarnAboutLockout()
        {
            string lockoutDate = DNA.DNA_Param("LAST_RESULT_VALUE");
            if (MessageBox.Show("Woops! You've been locked out due to too many activations in a certain time period." + Environment.NewLine + Environment.NewLine +
                "You must wait until " + lockoutDate + " to activate again." + Environment.NewLine + Environment.NewLine +
                "If you think we screwed up, click Yes to contact Support.",
                "Locked out",
                MessageBoxButtons.YesNo, MessageBoxIcon.Error) == DialogResult.Yes)
            {
                SupportHelper.OpenKBArticle("Lockout");
            }
        }

        public static void WarnAboutOtherError(string method, int error)
        {
            if (MessageBox.Show("Woops! Something didn't go as planned and unfortunately we can't exactly figure it out." + Environment.NewLine + Environment.NewLine +
                "Is it okay if we bring you to Support and you tell us what happened?",
                "Unexpected error",
                MessageBoxButtons.YesNo, MessageBoxIcon.Error) == DialogResult.Yes)
            {
                SupportHelper.OpenKBArticle(method + error);
            }
        }
    }
}
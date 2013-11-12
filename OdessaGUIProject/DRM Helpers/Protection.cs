#define TEST_PRODUCT_KEY // used when we want to test against softworkz

using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows.Forms;
using GaDotNet.Common.Helpers;
using NLog;
using OdessaGUIProject.UI_Helpers;

namespace OdessaGUIProject.DRM_Helpers
{
    internal static class Protection
    {
        public const string ProductKey_PA2 = "P0002393-QAB:CyouFs3kYsgbslVFqCac6OXouoSa+EiHPcqOpCXo/cD3XFgdT610cmT5C8thdvOZqet6Wk5dH9XJR6NKB6pefr";
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        //***********
        //  Declaration of Product_Key - you would receive this by email after creating your product
        //  configuration in the software_DNA control Panel
        //***********

        // ReSharper disable InconsistentNaming
#if DEBUG && TEST_PRODUCT_KEY
        public const string ProductKey_PA1 = "P0002432-QAB:Dxsrc4QGf9IWWLQV/A8vqNujMIzn0WENaOH13OFM7YKsbsLif4YSu5K6Qriwow1dIZdCaHiQpRG4y3Ku+TNG7B";
#else
        public const string ProductKey_PA1 = "P0002392-QAB:CYgFPFtOKmWkXhr6FizyBHbn2pmL9ztqM6vmz0goJ6IqlJrZngwRCTcMAcYtoCbYv84EqK3J6FZNswwtnsxc0X";
#endif
        // ReSharper restore InconsistentNaming

        //***********
        //		Declaration of MD5 Hash Code of DNA in shuffled format.
        //		Never put MD5 Code in clear text.
        //  	Declaration of software_DNA Product Key for this application
        //      MD5 for Build 603 of DNA.DLL 5.0.1
        //***********

        //public const string unShuffledMD5 = "4CC6A0B875536EF7662D5035179A796D";

#pragma warning disable 169
        private const string ShuffledMD5V400 = "CJj9zkPlrtsoIpsY2/uqSo0SkUPEUbZOEypTT43e6c/1GPBFKXtmTbzk+OXjVDva";
        private const string ShuffledMD5V501 = "f1ZLfFxRani/qDc0SKxOB5rpm+Nbbt0dvheA5DJBVbNhmBfYuO9SclLzbs0n1lWb";
#pragma warning restore 169

        internal enum ActivationState
        {
            Activated,
            Trial,
            TrialExpired,
            Unlicensed,
        }

        internal static void ActivateApp(string activationCode, Form activationForm)
        {
            if (activationCode.Length == 0)
            {
                MessageBox.Show("Please enter an Activation Code.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string associatedProductKey = GetAssociatedProductKey(activationCode);
            if (!IsValidProductKey(associatedProductKey, activationCode))
            {
                Logger.Error("Invalid product key. User probably entered invalid activationCode (" + activationCode + ")");
                return;
            }

            // upgrading from evaluation scenario: if existing CDM, rename it so we can get a new license
            bool didArchive = ArchiveLicenseFile();

            // activate new code
            int err;
            using (new HourGlass())
            {
                err = DNA.DNA_Query(associatedProductKey, activationCode);
            }

            Logger.Info("Response from DNA_Query: " + err);

            switch (err)
            {
                case DNA.ERR_NO_CONNECTION:
                case DNA.ERR_CONNECTION_LOST:
                    AnalyticsHelper.FireEvent("Activation - no connection");
                    if (RetryEvaluatingAfterConnectionWarning())
                    {
                        ActivateApp(activationCode, activationForm);
                    }
                    break;

                case DNA.ERR_ACTIVATION_EXPECTED:
                    ActivationExpected(activationCode, activationForm);
                    break;

                case DNA.ERR_REACTIVATION_EXPECTED:
                    ReactivationExpected(activationCode, activationForm);
                    break;

                default:
                    AnalyticsHelper.FireEvent("Activation - other error", err);
                    ProtectionWarnings.WarnAboutOtherError("ErrorQuerying", err);
                    break;
            }

            Logger.Info(CultureInfo.InvariantCulture, "DRM Code: " + GetActivationCode());

            if (GetLicenseStatus() == ActivationState.Unlicensed)
            { // user tried activating but it didn't work. reinstate their evaluation code if available.
                if (didArchive)
                    ReinstateArchivedLicenseFile();
            }

            Logger.Info("DRM Code: " + GetActivationCode());
        }

        internal static bool BeginTrial()
        {
            // make sure the app is unlicensed
            if (GetLicenseStatus() != ActivationState.Unlicensed)
            { // either already under an evaluation or shouldn't be activating in the first place
                Logger.Info("Either already under trial or shouldn't be activating in the first place");
                return true;
            }

            int err;

            using (new HourGlass())
            {
                err = DNA.DNA_EvaluateNow(ProductKey_PA1);
            }

            Logger.Info("Response from DNA_EvaluateNow: " + err);

            switch (err)
            {
                case DNA.ERR_NO_ERROR: // success!

                    AnalyticsHelper.FireEvent("Trial - success");

                    MessageBox.Show("Thank you for trying out Highlight Hunter Pro! Enjoy!",
                        "Great success!",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    break;

                case DNA.ERR_NO_CONNECTION:
                case DNA.ERR_CONNECTION_LOST:
                    AnalyticsHelper.FireEvent("Trial - no connection");
                    if (RetryEvaluatingAfterConnectionWarning())
                    {
                        BeginTrial();
                    }
                    break;

                case DNA.ERR_EVAL_CODE_ALREADY_SENT:
                case DNA.ERR_EVAL_CODE_UNAVAILABLE:
                    AnalyticsHelper.FireEvent("Trial - code unavailable");
                    MessageBox.Show("Woops! It looks like this computer's trial period has expired." + Environment.NewLine + Environment.NewLine +
                        "The app will continue to run but watermarks will be applied to your highlights.",
                        "Trial period expired",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;

                case DNA.ERR_LOCKOUT:
                    AnalyticsHelper.FireEvent("Trial - lockout");
                    ProtectionWarnings.WarnAboutLockout();
                    break;

                default:
                    AnalyticsHelper.FireEvent("Trial - other error", err);
                    if (MessageBox.Show("Woops! An error prevented us from starting your trial. If this happened unexpectedly, click Yes to file a support ticket.",
                        "Error",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Error) == DialogResult.Yes)
                    {
                        SupportHelper.OpenKBArticle("ErrorStartingTrial");
                    }
                    break;
            }

            Logger.Info("DRM Code: " + GetActivationCode());

            if (err == DNA.ERR_NO_ERROR)
                return true;

            return false;
        }

        /// <summary>
        /// Called when user wants to end evaluation early.
        /// </summary>
        /// <returns>Success?</returns>
        internal static bool EndTrial()
        {
            try
            {
                File.Delete(GetCDMPathName());
                Logger.Info("License file deleted successfully");
                return true;
            }
            catch (Exception ex)
            {
                Logger.Info("Exception when deleting license file: " + ex.ToString());
                return false;
            }
        }

        internal static string GetActivationCode()
        {
            string code = DNA.DNA_Param("ACTIVATION_CODE");
            return code;
        }

        internal static string GetAssociatedProductKey(string activationCode)
        {
            string associatedProductKey;
            if (activationCode.StartsWith("PA1", StringComparison.Ordinal))
            {
                associatedProductKey = ProductKey_PA1;
            }
            else if (activationCode.StartsWith("PA2", StringComparison.Ordinal))
            {
                associatedProductKey = ProductKey_PA2;
            }
            else
            {
                associatedProductKey = "";
            }
            return associatedProductKey;
        }

        internal static string GetCDMPathName()
        {
            OperatingSystem os = Environment.OSVersion;
            Version vs = os.Version;

            string commonPath;
            if (vs.Major >= 6)
            { // win7 or vista
                commonPath = Environment.GetEnvironmentVariable("PUBLIC");

                /* this requires .NET 4
                // This should give you something like C:\Users\Public\Documents
                string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments);

                var directory = new DirectoryInfo(documentsPath);

                // Now this should give you something like C:\Users\Public
                string commonPath = directory.Parent.FullName;
                 */
            }
            else
            {
                commonPath = Environment.GetEnvironmentVariable("ALLUSERSPROFILE"); // C:\Documents and Settings\All Users
            }

            if (commonPath != null)
            {
                string path = Path.Combine(commonPath, // Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData)
                                           Application.ProductName);
                path = Path.Combine(path, Application.ProductName + ".CDM");

                Logger.Info("CDM outputPath: " + path);

                return path;
            }

            return "";
        }

        ///  <summary>
        /// 
        ///  </summary>
        ///  <param name="expiryDateParam">This is always MM/DD/YYYY</param>
        /// <param name="today"></param>
        /// <returns></returns>
        internal static int GetDaysLeftInTrial(string expiryDateParam = null, DateTime today = default(DateTime))
        {
            if (String.IsNullOrEmpty(expiryDateParam))
                expiryDateParam = DNA.DNA_Param("EXPIRY_DATE");  // this is always MM/DD/YYYY

            if (today == default(DateTime))
                today = DateTime.Now;

            // we can't use .TryParse because it won't work in cultures whose date is DD/MM/YYYY

            int firstSlash = expiryDateParam.IndexOf("/", StringComparison.Ordinal);
            if (firstSlash < 1)
            {
                Logger.Error("Invalid EXPIRY_DATE: " + expiryDateParam);
                return 0;
            }
            int month = Convert.ToInt16(expiryDateParam.Substring(0, firstSlash));

            int secondSlash = expiryDateParam.IndexOf("/", firstSlash + 1, StringComparison.Ordinal);
            if (secondSlash < 1)
            {
                Logger.Error("Invalid EXPIRY_DATE: " + expiryDateParam);
                return 0;
            }
            int day = Convert.ToInt16(expiryDateParam.Substring(firstSlash + 1, secondSlash - firstSlash - 1));

            int year = Convert.ToInt16(expiryDateParam.Substring(secondSlash + 1));

            try
            {
                DateTime expiryDate = new DateTime(year, month, day);

                int daysLeft = (int)(expiryDate - today).TotalDays + 1;
                if (daysLeft < 0)
                    daysLeft = 0;
                return daysLeft;
            }
            catch (Exception ex)
            {
                Logger.Error("Exception checking expiry date: " + ex);
                return 0;
            }
        }

        internal static string GetINIPathName()
        {
            // Now this should give you something like C:\Users\Public
            var directoryInfo = (new DirectoryInfo(GetCDMPathName())).Parent;
            if (directoryInfo != null)
            {
                string commonPath = directoryInfo.FullName;

                string path = Path.Combine(commonPath, Application.ProductName + ".INI");

#if DEBUG
                Logger.Info("INI outputPath: " + path);
#endif

                return path;
            }

            return "";
        }

        /// <summary>
        /// Determines whether license is evaluation, activation, or unlicensed
        /// </summary>
        /// <returns></returns>
        internal static ActivationState GetLicenseStatus(bool localOnly = false)
        {
            var code = GetActivationCode();
            if (code == "")
                return ActivationState.Unlicensed;

            string associatedProductKey = GetAssociatedProductKey(code);

            ActivationState activationState;

            int err;
            var r = new Random();
            switch (r.Next(1, 5))
            {
                case 1:
                    if (localOnly)
                        err = DNA.DNA_ValidateCDM(associatedProductKey);
                    else
                        err = DNA.DNA_Validate(associatedProductKey);             // Option 2 and 3
                    break;

                case 2:
                    if (localOnly)
                        err = DNA.DNA_ValidateCDM2(associatedProductKey);
                    else
                        err = DNA.DNA_Validate2(associatedProductKey);             // Option 2 and 3
                    break;

                case 3:
                    if (localOnly)
                        err = DNA.DNA_ValidateCDM3(associatedProductKey);
                    else
                        err = DNA.DNA_Validate3(associatedProductKey);             // Option 2 and 3
                    break;

                case 4:
                    if (localOnly)
                        err = DNA.DNA_ValidateCDM4(associatedProductKey);
                    else
                        err = DNA.DNA_Validate4(associatedProductKey);             // Option 2 and 3
                    break;
                default:
                    if (localOnly)
                        err = DNA.DNA_ValidateCDM5(associatedProductKey);
                    else
                        err = DNA.DNA_Validate5(associatedProductKey);             // Option 2 and 3
                    break;
            }
            Logger.Info("Response from DNA_Validate: " + err);

            if (err == DNA.ERR_VALIDATION_WARNING)
            { // warn user they must connect to internet
                // this will only happen if anti-fraud is turned on
                if (MessageBox.Show("Woops! We couldn't connect to the Internet to verify your license. Mind connecting for us real quick?",
                    "Connect to the Internet",
                    MessageBoxButtons.RetryCancel,
                    MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    return GetLicenseStatus(localOnly);
                }
            }

            if (err == DNA.ERR_CDM_HAS_EXPIRED)
            {
                activationState = ActivationState.TrialExpired;
            }
            else if (err == DNA.ERR_NO_ERROR)
            {
                // license is valid but let's see if it's activated or an evaluation
                string param = DNA.DNA_Param("ACTIVATION_CODE");
                if (String.IsNullOrEmpty(param))
                    activationState = ActivationState.Unlicensed;
                else
                {
                    param = DNA.DNA_Param("EVAL_CODE");
                    if (param == "1")
                    {
                        // it's an evaluation code. let's make sure it hasn't expired
                        if (GetDaysLeftInTrial() > 0)
                            activationState = ActivationState.Trial;
                        else
                            activationState = ActivationState.TrialExpired;
                    }
                    else
                    {
                        // it's an activation code
                        activationState = ActivationState.Activated;
                    }
                }
            }
            else
            {
                // license isn't valid
                activationState = ActivationState.Unlicensed;

                if (err == DNA.ERR_INVALID_CDM)
                { // CDM is invalid. Probably because we activated with RELEASE build and now we're using DEBUG build, or vice-versa
                    Logger.Info("Deleting CDM to try to avoid AccessViolationExceptions");
                    File.Delete(GetCDMPathName());
                }
            }


            return activationState;
        }

        /// <summary>
        /// This is run every time the app starts
        /// </summary>
        internal static void Initialize()
        {
            if (CheckMD5() == false)
            {
                Logger.Error("Invalid DNA.DLL file");
                Application.Exit();
            }

            //************************
            //  Certain API calls must be issued before any other API calls:
            //
            //   - DNA_CDMPathName and DNA_INIPathName to change the location for these files
            //          The default is the application's directory
            //   - DNA_SetLanguage  (used with DNA_ProtectionOK only)
            //
            //
            //************************
            DNA.DNA_SetCDMPathName(GetCDMPathName());
            DNA.DNA_SetINIPathName(GetINIPathName());
            DNA.DNA_SetLanguage(0);

            Logger.Info("Initialized successfully");
            Logger.Info("DRM Code: " + GetActivationCode());
        }

        internal static bool IsValidPassword(string password)
        {
            if (password.Length < 4 || password.Length > 16)
                return false;

            const string validCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLKMNOPQRSTUVWXYZ0123456789-@";

            foreach (char chr in password)
            {
                if (validCharacters.Contains(chr.ToString(CultureInfo.InvariantCulture)) == false)
                    return false;
            }

            return true;
        }

        internal static bool IsValidProductKey(string productKey, string activationCode, bool suppressMessageBox = false)
        {
            if (productKey.Length == 0)
            {
                if (suppressMessageBox == false)
                {
                    if (MessageBox.Show("Woops! It doesn't look like this is a valid Activation Code." + Environment.NewLine + Environment.NewLine +
                        "It may, however, be a coupon code instead. Would you like to redeem this coupon code?",
                        "Invalid activation code",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Error,
                        MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                    {
                        BrowserHelper.LaunchPurchasingOptions(activationCode);
                    }
                }
                return false;
            }

            return true;
        }

        internal static bool ReinstateArchivedLicenseFile()
        {
            string archivePathName = GetCDMPathName() + ".OLD";

            if (File.Exists(archivePathName))
            {
                try
                {
                    if (File.Exists(GetCDMPathName()))
                        File.Delete(GetCDMPathName());

                    File.Move(archivePathName, GetCDMPathName());
                    Logger.Info("License file reinstated successfully");
                    return true;
                }
                catch (Exception ex)
                {
                    Logger.Error("Exception while renaming existing CDM file! " + ex.ToString());
                    if (MessageBox.Show("An error prevented us from reinstating your trial. If this happened unexpectedly, click Yes to file a support ticket.",
                        "Error",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Error) == DialogResult.Yes)
                    {
                        SupportHelper.OpenKBArticle("ErrorReinstating");
                    }
                }
            }

            return false;
        }

        private static void ActivationExpected(string activationCode, Form activationForm)
        {
            // get new password and email from user
            //*************
            // Activation is required
            //   need to capture a password, and optional email address for resending a lost password
            //*************

            string associatedProductKey = GetAssociatedProductKey(activationCode);
            if (!IsValidProductKey(associatedProductKey, activationCode))
            {
                Logger.Error("Invalid product key with activation code: " + activationCode);
                return;
            }

            //** code here to pop up a window asking for a password and email address

            string newPass, newEmail;

            using (var dimmerMask = new DimmerMask(activationForm))
            {
                dimmerMask.Show(activationForm);

                var formAct = new TFActivation
                                  {
                                      lblCode = { Text = activationCode }
                                  };
                formAct.ShowDialog(activationForm);
                try
                {
                    if (formAct.DialogResult == DialogResult.OK)
                    {
                        newPass = formAct.tbPass.Text;
                        newEmail = formAct.tbEmail.Text;
                    }
                    else
                    {
                        return;
                    }
                }
                finally
                {
                    formAct.Dispose();
                }
            }

            int err;
            using (new HourGlass())
            {
                err = DNA.DNA_Activate(associatedProductKey, activationCode, newPass, newEmail);
            }

            Logger.Info("Response to DNA_Activate: " + err);

            switch (err)
            {
                case DNA.ERR_NO_ERROR:
                    AnalyticsHelper.FireEvent("Activation - success");

                    AnalyticsHelper.FireEvent("Activation - days left in trial", GetDaysLeftInTrial());

                    MessageBox.Show("Thank you for purchasing Highlight Hunter Pro." + Environment.NewLine + Environment.NewLine +
                        "Enjoy!",
                        "Great success!",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                    break;

                case DNA.ERR_NO_CONNECTION:
                case DNA.ERR_CONNECTION_LOST:
                    AnalyticsHelper.FireEvent("Activation - no connection");
                    if (RetryActivatingAfterConnectionWarning())
                        ActivationExpected(activationCode, activationForm);
                    break;

                case DNA.ERR_LOCKOUT:
                    AnalyticsHelper.FireEvent("Activation - lockout");
                    ProtectionWarnings.WarnAboutLockout();
                    break;

                // intentionally don't provide more detailed information to user in case they're trying to hack

                default:
                    AnalyticsHelper.FireEvent("Activation - other error", err);
                    ProtectionWarnings.WarnAboutOtherError("ErrorActivating", err);
                    break;
            }
        }

        private static bool ArchiveLicenseFile()
        {
            string archivePathName = GetCDMPathName() + ".OLD";

            if (File.Exists(GetCDMPathName()))
            {
                try
                {
                    if (File.Exists(archivePathName))
                        File.Delete(archivePathName);

                    File.Move(GetCDMPathName(), archivePathName);
                    Logger.Info("License file archived successfully");
                    return true;
                }
                catch (Exception ex)
                {
                    Logger.Error("Exception while renaming existing CDM file! " + ex.ToString());
                    if (MessageBox.Show("An error prevented us from activating " + Application.ProductName + ". If this happened unexpectedly, click Yes to file a support ticket.",
                        "Error",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Error) == DialogResult.Yes)
                    {
                        SupportHelper.OpenKBArticle("ErrorArchiving");
                    }
                }
            }

            return false;
        }

        private static bool CheckMD5()
        {
            //************************
            //		Step 1: Authenticate the DLL using MD5
            //
            //    	- unshuffle MD5 using your own shuffle function
            //      - calculate the MD5 of DLL on user's computer using the MD5File method
            //          provided in DNA_INT.cs
            //      - compare MD5 Hash Codes
            //      - if no match, return FALSE. If matches, return TRUE
            //
            //***********************

            string md5DigestHex = DNA.MD5File(Application.StartupPath + "\\DNA.DLL");
            if (md5DigestHex.Length == 0)
            {
                Logger.Error("DNA.DLL file not found");
                MessageBox.Show("DNA.DLL file was not found.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            var s = new StringBuilder();
            s.Append("Chu");
            s.Append("ckT");
            s.Append("ayl");
            s.Append("or44");
            string sharedSecret = s.ToString();

            if (md5DigestHex != Crypto.DecryptStringAES(ShuffledMD5V400, sharedSecret))
            {
                return false;
            }
            return true;
        }

        private static void ReactivationExpected(string activationCode, Form activationForm)
        {
            // get existing password from user
            //***************
            // Reactivation is required.
            //***************

            string associatedProductKey = GetAssociatedProductKey(activationCode);
            if (!IsValidProductKey(associatedProductKey, activationCode))
            {
                Logger.Error("Invalid product key with activation code: " + activationCode);
                return;
            }

            //** code here to pop up a window asking for information

            string currPass, newPass;

            using (var dimmerMask = new DimmerMask(activationForm))
            {
                dimmerMask.Show(activationForm);

                var formReact = new TFReactivation
                                {
                                    lblCode = { Text = activationCode }
                                };
                formReact.ShowDialog();
                try
                {
                    if (formReact.DialogResult == DialogResult.OK)
                    {
                        currPass = formReact.tbCurrentPass.Text;
                        newPass = formReact.tbNewPass.Text;
                    }
                    else
                    {
                        return;
                    }
                }
                finally
                {
                    formReact.Dispose();
                }
            }

            int err;
            using (new HourGlass())
            {
                err = DNA.DNA_Reactivate(associatedProductKey, activationCode, currPass, newPass);
            }

            Logger.Info("Response to DNA_Reactivate: " + err);

            switch (err)
            {
                case DNA.ERR_NO_ERROR:
                    AnalyticsHelper.FireEvent("Reactivation - success");
                    MessageBox.Show("Activation successful! Thank you for purchasing Highlight Hunter Pro." + Environment.NewLine + Environment.NewLine +
                        "Enjoy!",
                        "Activation successful",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                    break;

                case DNA.ERR_INVALID_NEW_PASSWORD:
                    AnalyticsHelper.FireEvent("Reactivation - invalid new password");
                    if (MessageBox.Show("Woops! For security reasons, you must choose a password that you haven't used before." + Environment.NewLine + Environment.NewLine +
                        "Try again?",
                        "Password already used",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Error) == DialogResult.Yes)
                    {
                        ReactivationExpected(activationCode, activationForm);
                    }
                    break;

                case DNA.ERR_INVALID_PASSWORD:
                    AnalyticsHelper.FireEvent("Reactivation - invalid password");
                    if (MessageBox.Show("Woops! It doesn't look like the original password is correct. If you forgot it, click Send Password." + Environment.NewLine + Environment.NewLine +
                        "Try again?",
                        "Wrong password",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Error) == DialogResult.Yes)
                    {
                        ReactivationExpected(activationCode, activationForm);
                    }
                    break;

                case DNA.ERR_NO_CONNECTION:
                case DNA.ERR_CONNECTION_LOST:
                    AnalyticsHelper.FireEvent("Reactivation - no connection");
                    if (RetryActivatingAfterConnectionWarning())
                        ReactivationExpected(activationCode, activationForm);
                    break;

                case DNA.ERR_LOCKOUT:
                    AnalyticsHelper.FireEvent("Reactivation - lockout");
                    ProtectionWarnings.WarnAboutLockout();
                    break;

                // intentionally don't provide more detailed information to user in case they're trying to hack

                default:
                    AnalyticsHelper.FireEvent("Reactivation - other error", err);
                    ProtectionWarnings.WarnAboutOtherError("ErrorReactivating", err);
                    break;
            }
        }

        private static bool RetryActivatingAfterConnectionWarning()
        {
            return (MessageBox.Show("We had trouble connecting to the Internet to activate " + Application.ProductName + ". Please make sure you're connected and your firewall or router is not interfering." + Environment.NewLine + Environment.NewLine +
                "Try again?",
                "No Internet connection",
                MessageBoxButtons.RetryCancel, MessageBoxIcon.Error)
                == DialogResult.Retry);
        }

        private static bool RetryEvaluatingAfterConnectionWarning()
        {
            return (MessageBox.Show("We had trouble connecting to the Internet to start your trial. Please make sure you're connected and your firewall or router is not interfering." + Environment.NewLine + Environment.NewLine +
                "Try again?",
                "No Internet connection",
                MessageBoxButtons.RetryCancel, MessageBoxIcon.Error)
                == DialogResult.Retry);
        }

        /* NO LONGER USED
        public static int DNA_ProtectionOK_SOURCE()
        {
            //***********************
            //  Code that implements the full "Custom" approach to software_DNA including:
            //		- DNA Client authentication
            //		- the DNA_Validate API Call
            //      - the DNA_Query, DNA_Activate and DNA_Reactivate calls with custom dialog boxes
            //
            //	This code would be located in your main routine and executed during start up
            //    of the application and also when an "Activate" button is clicked by the user
            //
            //
            //************************

            int err;

            //***********************
            //
            //		Step 2: Attempt to Validate
            //
            //		- if successful (err=0), then continue main program
            //      - if err <=0, then Level 3 warning, display warning to user and continue program
            //	    - if err > 0, then need to determine next steps
            //
            //***********************

            err = DNA.DNA_Validate(ProductKey);
            logger.Info("Response from DNA_Validate: " + err);

            if (err == 0) {
                // validated successfully

                return err;
            }

            if (err < 0) {
                Debug.Assert(err < 0, "Level3 protection somehow activated. This should never happen.");
                //******************
                //
                //		For Level 3 protection only
                //		code here to pop up a window or message to inform user that he must
                //		connect to the Internet and validate before <date>
                //		<date> is retrieved using DNA.DNA_Param API Call
                //
                //******************

                return err;
            }

            //***********************
            //
            //		Step 3: Is this an Activation or Re-activation ?
            //
            //			- get the Activation Code from the user
            //			- check with server if activation code is new or not using DNA_Query
            //			- if Reactivation, get info and re-activate
            //			- warn the user if this is a duplicated password
            //			- if Activation, get info and activate
            //
            //      If any errors in Activation, or Re-activation, goto DEMO Mode or exit
            //
            //***********************

            ActivationWelcome awForm = new ActivationWelcome();
            awForm.ShowDialog();
            if (awForm.DialogResult == DialogResult.Cancel || awForm.DialogResult == DialogResult.Abort)
            {
                if (MessageBox.Show("Sorry but you gotta either evaluate the software for 30 days or buy an activation code." + Environment.NewLine + Environment.NewLine +
                    "Try again?", "Woops", MessageBoxButtons.RetryCancel, MessageBoxIcon.Information) == DialogResult.Retry)
                {
                    return DNA_ProtectionOK_SOURCE();
                }
                else
                {
                    Application.Exit();
                }
            }

            return err;
        }
        */
    }
}
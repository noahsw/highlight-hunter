using System;
using System.Reflection;
using System.Windows.Forms;
using OdessaGUIProject.DRM_Helpers;

namespace OdessaGUIProject
{
    sealed partial class AboutBox : Form
    {
        public AboutBox()
        {
            InitializeComponent();
            Text = String.Format("About " + AssemblyTitle);
            labelProductName.Text = AssemblyProduct;

            labelVersion.Text = String.Format("Version " + MainModel.ApplicationVersion) + ". Patent Pending.";
            labelCopyright.Text = AssemblyCopyright;
            textBoxDescription.Text = AssemblyDescription;

            switch (Protection.GetLicenseStatus())
            {
                case Protection.ActivationState.Activated:
                    labelLicense.Text = "Activated with code " + Protection.GetActivationCode();
                    break;

                case Protection.ActivationState.Trial:
                    int daysLeft = Protection.GetDaysLeftInTrial();
                    labelLicense.Text = "Trial with " + daysLeft + " day" + (daysLeft == 1 ? "" : "s") + " left";
                    break;

                case Protection.ActivationState.TrialExpired:
                    labelLicense.Text = "Trial expired";
                    break;

                case Protection.ActivationState.Unlicensed:
                    labelLicense.Text = "Not licensed";
                    break;
            }
        }

        #region Assembly Attribute Accessors

        public string AssemblyCompany
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyCompanyAttribute)attributes[0]).Company;
            }
        }

        public string AssemblyCopyright
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyCopyrightAttribute)attributes[0]).Copyright;
            }
        }

        public string AssemblyDescription
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyDescriptionAttribute)attributes[0]).Description;
            }
        }

        public string AssemblyProduct
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyProductAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyProductAttribute)attributes[0]).Product;
            }
        }

        public string AssemblyTitle
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
                if (attributes.Length > 0)
                {
                    var titleAttribute = (AssemblyTitleAttribute)attributes[0];
                    if (titleAttribute.Title != "")
                    {
                        return titleAttribute.Title;
                    }
                }
                return System.IO.Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase);
            }
        }

        #endregion Assembly Attribute Accessors
    }
}
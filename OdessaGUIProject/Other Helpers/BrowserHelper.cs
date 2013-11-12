using System;
using System.Diagnostics;
using System.Windows.Forms;
using NLog;

namespace OdessaGUIProject
{
    internal static class BrowserHelper
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

#if DEBUG

        /// <summary>
        /// Without slash at the end
        /// </summary>
        internal static string Host = "http://test.highlighthunter.com";

#else
        /// <summary>
        /// Without slash at the end
        /// </summary>
        internal static string Host = "http://www.highlighthunter.com";
#endif

        internal static bool LaunchBrowser(string url, string term = "")
        {
            string startingSymbol = (url.Contains("?") ? "&" : "?");
            try
            {
                var p = new Process();
                if (term.Length == 0)
                    p.StartInfo.FileName = url;
                else
                    p.StartInfo.FileName = url + startingSymbol + "utm_source=app" + Application.ProductVersion +
                                            "&utm_medium=app&utm_term=" + term + "&utm_campaign=app";

                p.Start();
                return true;
            }
            catch (Exception ex)
            {
                Logger.Error("Error launching browser: " + ex);
                return false;
            }
        }

        internal static void LaunchPurchasingOptions(string couponCode = "")
        {
            LaunchBrowser(Host + "/purchase-redirect.php?coupon=" + couponCode, "buynow");
        }
    }
}
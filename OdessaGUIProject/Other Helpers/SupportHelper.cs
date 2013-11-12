using NLog;

namespace OdessaGUIProject
{
    public static class SupportHelper
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public static void OpenKBArticle(string id)
        {
            Logger.Info("Opening support for " + id);

            BrowserHelper.LaunchBrowser(BrowserHelper.Host + "/support-redirect.php?id=" + id, id);
        }
    }
}
using System;
using System.Windows.Forms;
using NLog;
using NLog.Config;
using NLog.Targets;
using Zayko.Dialogs.UnhandledExceptionDlg;
using OdessaGUIProject.Properties;

namespace OdessaGUIProject
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main(string[] args)
        {
            LogManager.ThrowExceptions = true;

#if DEBUG

            var log4viewTarget = new NLogViewerTarget()
            {
                Name = "log4view",
                Address = "udp://127.0.0.1:877",
                IncludeCallSite = true,
                IncludeSourceInfo = true
            };
            var log4viewRule = new LoggingRule("*", LogLevel.Trace, log4viewTarget);
            LogManager.Configuration.AddTarget("log4view", log4viewTarget);
            LogManager.Configuration.LoggingRules.Add(log4viewRule);

            var consoleTarget = new ColoredConsoleTarget()
            {
                Name = "console",
                Layout = "${date} ${callsite:includeSourcePath=false}: ${message}"
            };
            var consoleRule = new LoggingRule("*", LogLevel.Trace, consoleTarget);
            LogManager.Configuration.AddTarget("console", consoleTarget);
            LogManager.Configuration.LoggingRules.Add(consoleRule);

            LogManager.ReconfigExistingLoggers();

#endif

            UnhandledExceptionDlg exDlg = new UnhandledExceptionDlg();

#if RELEASE
            GaDotNet.Common.Data.ConfigurationSettings.GoogleAccountCode = "UA-28160719-3";
#else
            GaDotNet.Common.Data.ConfigurationSettings.GoogleAccountCode = "UA-28160719-2";
#endif
            GaDotNet.Common.Data.ConfigurationSettings.AppProductVersion = MainModel.ApplicationVersion;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);


            #region upgrade settings
            if (Settings.Default.MustUpgradeSettings)
            {
                try
                {
                    Settings.Default.Upgrade();
                    Settings.Default.MustUpgradeSettings = false;
                    Settings.Default.ShowFirstRunScreenV2 = true; // always make sure they see tutorial since it changed in v2
                    Settings.Default.Save();
                }
                catch (Exception)
                {
                    // never crash here. it's not worth it.
                    // this was crashing for me when I tried running a non-installed copy of HH (just copying the Release directory to another machine)
                }
            }
            #endregion

            //Application.Run(new TestForm());

            Application.Run(new MainForm());
        }
    }
}
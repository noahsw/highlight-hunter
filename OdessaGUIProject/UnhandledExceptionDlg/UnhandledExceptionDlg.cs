/**
 *                  UnhandledExceptionDlg Class v. 1.1
 *
 *                      Copyright (c)2006 Vitaly Zayko
 *
 * History:
 * September 26, 2006 - Added "ThreadException" handler, "SetUnhandledExceptionMode", OnShowErrorReport event
 *                      and updated the Demo and code comments;
 * August 29, 2006 - Updated information about Microsoft Windows Error Reporting service and its link;
 * July 18, 2006 - Initial release.
 *
 */

/** More info on MSDN:
 * http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dnbda/html/exceptdotnet.asp
 * http://msdn2.microsoft.com/en-us/library/system.windows.forms.application.threadexception.aspx
 * http://msdn2.microsoft.com/en-us/library/system.appdomain.unhandledexception.aspx
 * http://msdn2.microsoft.com/en-us/library/system.windows.forms.unhandledexceptionmode.aspx
 */

using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using OdessaGUIProject.UI_Helpers;
using System.Collections.Generic;
using OdessaGUIProject.Other_Helpers;
using OdessaGUIProject;

namespace Zayko.Dialogs.UnhandledExceptionDlg
{
    internal class SendExceptionClickEventArgs : System.EventArgs
    {
        public bool RestartApp;
        public bool SendExceptionDetails;
        public Exception UnhandledException;

        public SendExceptionClickEventArgs(bool SendDetailsArg, Exception ExceptionArg, bool RestartAppArg)
        {
            this.SendExceptionDetails = SendDetailsArg;     // TRUE if user clicked on "Send Error Report" button and FALSE if on "Don't Send"
            this.UnhandledException = ExceptionArg;         // Used to store captured exception
            this.RestartApp = RestartAppArg;                // Contains user's request: should the App to be restarted or not
        }
    }

    /// <summary>
    /// Class for catching unhandled exception with UI dialog.
    /// </summary>
    internal class UnhandledExceptionDlg
    {
        private bool _dorestart = true;

        /// <summary>
        /// Default constructor
        /// </summary>
        public UnhandledExceptionDlg()
        {
            // Add the event handler for handling UI thread exceptions to the event:
            Application.ThreadException += new ThreadExceptionEventHandler(ThreadExceptionFunction);

            // Set the unhandled exception mode to force all Windows Forms errors to go through our handler:
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);

            // Add the event handler for handling non-UI thread exceptions to the event:
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(UnhandledExceptionFunction);

            // Add handling of OnShowErrorReport.
            // If you skip this then link to report details won't be showing.
            OnShowErrorReport += delegate(object sender, SendExceptionClickEventArgs ar)
            {
                System.Windows.Forms.MessageBox.Show(
                    "\n" + ar.UnhandledException.Message + "\n" + ar.UnhandledException.StackTrace +
                    "\n" + (ar.RestartApp ? "This App will be restarted." : "This App will be terminated!"));
            };

            // Implement your sending protocol here. You can use any information from System.Exception
            OnSendExceptionClick += delegate(object sender, SendExceptionClickEventArgs ar)
            {
                // User clicked on "Send Error Report" button:
                if (ar.SendExceptionDetails)
                {
                    using (new HourGlass())
                    {
                        
                        string url = BrowserHelper.Host + "/exception-report.php";

                        var data = new Dictionary<string, string>();
                        data.Add("message", ar.UnhandledException.Message);
                        data.Add("stacktrace", ar.UnhandledException.StackTrace);

                        PostDataHelper.PostData(url, data, true);

                    }

                    MessageBox.Show("Error report submitted. Thanks!");
                }

                // User wants to restart the App:
                if (ar.RestartApp)
                {
                    Console.WriteLine("The App will be restarted...");
                    System.Diagnostics.Process.Start(System.Windows.Forms.Application.ExecutablePath);
                }

                Application.Exit();

            };
        }

        public delegate void SendExceptionClickHandler(object sender, SendExceptionClickEventArgs args);

        /// <summary>
        /// Occurs when user clicks on "Send Error report" button
        /// </summary>
        public event SendExceptionClickHandler OnSendExceptionClick;

        //public delegate void ShowErrorReportHandler(object sender, System.EventArgs args);
        /// <summary>
        /// Occurs when user clicks on "click here" link lable to get data that will be send
        /// </summary>
        public event SendExceptionClickHandler OnShowErrorReport;

        /// <summary>
        /// Set to true if you want to restart your App after falure
        /// </summary>
        public bool RestartApp
        {
            get { return _dorestart; }
            set { _dorestart = value; }
        }

        /// <summary>
        /// Raise Exception Dialog box for both UI and non-UI Unhandled Exceptions
        /// </summary>
        /// <param name="e">Catched exception</param>
        private void ShowUnhandledExceptionDlg(Exception e)
        {
            Exception unhandledException = e;

            if (unhandledException == null)
                unhandledException = new Exception("Unknown unhandled Exception was occurred!");

            UnhandledExDlgForm exDlgForm = new UnhandledExDlgForm();
            try
            {
                string appName = Application.ProductName;
                exDlgForm.Text = appName;
                exDlgForm.labelTitle.Text = String.Format(exDlgForm.labelTitle.Text, appName);
                exDlgForm.checkBoxRestart.Text = String.Format(exDlgForm.checkBoxRestart.Text, appName);
                exDlgForm.checkBoxRestart.Checked = this.RestartApp;

                // Do not show link label if OnShowErrorReport is not handled
                exDlgForm.labelLinkTitle.Visible = (OnShowErrorReport != null);
                exDlgForm.linkLabelData.Visible = (OnShowErrorReport != null);

                // Disable the Button if OnSendExceptionClick event is not handled
                exDlgForm.buttonSend.Enabled = (OnSendExceptionClick != null);

                // Attach reflection to checkbox checked status
                exDlgForm.checkBoxRestart.CheckedChanged += delegate(object o, EventArgs ev)
                {
                    this._dorestart = exDlgForm.checkBoxRestart.Checked;
                };

                // Handle clicks on report link label
                exDlgForm.linkLabelData.LinkClicked += delegate(object o, LinkLabelLinkClickedEventArgs ev)
                {
                    if (OnShowErrorReport != null)
                    {
                        SendExceptionClickEventArgs ar = new SendExceptionClickEventArgs(true, unhandledException, _dorestart);
                        OnShowErrorReport(this, ar);
                    }
                };

                // Show the Dialog box:
                bool sendDetails = (exDlgForm.ShowDialog() == System.Windows.Forms.DialogResult.Yes);

                if (OnSendExceptionClick != null)
                {
                    SendExceptionClickEventArgs ar = new SendExceptionClickEventArgs(sendDetails, unhandledException, _dorestart);
                    OnSendExceptionClick(this, ar);
                }
            }
            finally
            {
                exDlgForm.Dispose();
            }
        }

        /// <summary>
        /// Handle the UI exceptions by showing a dialog box
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ThreadExceptionFunction(Object sender, ThreadExceptionEventArgs e)
        {
            // Suppress the Dialog in Debug mode:
            //#if !DEBUG
            ShowUnhandledExceptionDlg(e.Exception);
            //#endif
        }

        /// <summary>
        /// Handle the UI exceptions by showing a dialog box
        /// </summary>
        /// <param name="sender">Sender Object</param>
        /// <param name="args">Passing arguments: original exception etc.</param>
        private void UnhandledExceptionFunction(Object sender, UnhandledExceptionEventArgs args)
        {
            // Suppress the Dialog in Debug mode:
            //#if !DEBUG
            ShowUnhandledExceptionDlg((Exception)args.ExceptionObject);
            //#endif
        }
    }
}
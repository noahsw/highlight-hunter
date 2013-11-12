using System;
using System.ComponentModel;
using System.Windows.Forms;
using System.Xml;
using NLog;

namespace OdessaGUIProject
{
    internal class UpdateChecker
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly CancelEventHandler _isSafeToPromptHandler;

        internal UpdateChecker(CancelEventHandler isSafeToPromptHandler)
        {
            _isSafeToPromptHandler = isSafeToPromptHandler;
        }

        internal void CheckForUpdate()
        {
            // if this is our first time checking, save today's date and quit
            if (Properties.Settings.Default.LastUpdateCheckDate == DateTime.MinValue)
            {
                Logger.Info("This is our first time loading so don't check.");
                Properties.Settings.Default.LastUpdateCheckDate = DateTime.Today;
                Properties.Settings.Default.Save();
                return;
            }

            if (IsTimeToCheck() == false)
            {
                Logger.Info("Not time to check for updates yet");
                return;
            }

            try
            { // never crash just for checking if there's an update
                var bwUpdateCheck = new BackgroundWorker();
                bwUpdateCheck.DoWork += bwUpdateCheck_DoWork;
                bwUpdateCheck.RunWorkerCompleted += bwUpdateCheck_RunWorkerCompleted;
                bwUpdateCheck.RunWorkerAsync();
            }
            catch (Exception ex)
            {
                Logger.Error("Exception checking for update: " + ex);
            }
        }

        private static UpdateResult GetUpdateResult()
        {
            var updateResult = new UpdateResult();

            XmlTextReader reader = null;
            try
            {
                string url = BrowserHelper.Host + "/updatecheck.php?platform=pc&version=" + Application.ProductVersion;

#if DEBUG
                //uncomment this if you want to check live site
                //url = "http://test.highlighthunter.com/updatecheck.php?platform=pc&version=1.2.0.0";
#endif

                // Load the reader with the data file and ignore all white space nodes.
                reader = new XmlTextReader(url)
                             {
                                 WhitespaceHandling = WhitespaceHandling.None
                             };

                //logger.Info("Contents of update check: " + reader.ReadContentAsString());

                reader.ReadStartElement("updateResults");

                updateResult.IsUpdateAvailable = Boolean.Parse(reader.ReadElementString("isUpdateAvailable"));
                updateResult.LatestVersion = reader.ReadElementString("latestVersion");
                updateResult.WhatsNew = reader.ReadElementString("whatsNew");
                updateResult.DownloadPage = reader.ReadElementString("downloadPage");

                /*
                // Parse the file and display each of the nodes.
                while (reader.Read())
                {
                    switch (reader.NodeType)
                    {
                        case XmlNodeType.Element:
                            Console.WriteLine("<{0}>", reader.Name);
                            break;

                        case XmlNodeType.Text:
                            Console.WriteLine(reader.Value);
                            break;

                        case XmlNodeType.CDATA:
                            Console.WriteLine("<![CDATA[{0}]]>", reader.Value);
                            break;

                        case XmlNodeType.ProcessingInstruction:
                            Console.WriteLine("<?{0} {1}?>", reader.Name, reader.Value);
                            break;

                        case XmlNodeType.Comment:
                            Console.WriteLine("<!--{0}-->", reader.Value);
                            break;

                        case XmlNodeType.XmlDeclaration:
                            Console.WriteLine("<?xml version='1.0'?>");
                            break;

                        case XmlNodeType.Document:
                            break;

                        case XmlNodeType.DocumentType:
                            Console.WriteLine("<!DOCTYPE {0} [{1}]", reader.Name, reader.Value);
                            break;

                        case XmlNodeType.EntityReference:
                            Console.WriteLine(reader.Name);
                            break;

                        case XmlNodeType.EndElement:
                            Console.WriteLine("</{0}>", reader.Name);
                            break;
                    }
                }
                 */
            }
            catch (Exception ex)
            {
                Logger.Error("Exception checking for update: " + ex.ToString());
            }

            finally
            {
                if (reader != null)
                    reader.Close();
            }

            return updateResult;
        }

        private static bool IsTimeToCheck()
        {
            bool ret;

            var frequency = new TimeSpan(1, 0, 0, 0); // check every day

            var diff = DateTime.Today - Properties.Settings.Default.LastUpdateCheckDate;
            if (diff > frequency)
                ret = true;
            else
                ret = false;

#if DEBUG
            // we still want to run the code above to make sure it works, but we want to overwrite things in the DEBUG build
            ret = true;
#endif

            return ret;
        }

        private void bwUpdateCheck_DoWork(object sender, DoWorkEventArgs e)
        {
            UpdateResult updateResult = GetUpdateResult();

            e.Result = updateResult;
        }

        private void bwUpdateCheck_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            var updateResult = (UpdateResult)e.Result;

            if (updateResult.IsUpdateAvailable)
            {
                Logger.Info("Update available!");
                PromptUserForUpdate(updateResult);
            }
            else
                Logger.Info("No update available.");

            Properties.Settings.Default.LastUpdateCheckDate = DateTime.Today;
            Properties.Settings.Default.Save();
        }

        private void PromptUserForUpdate(UpdateResult updateResult)
        {
            var eventArgs = new CancelEventArgs();

            _isSafeToPromptHandler.Invoke(this, eventArgs);

            if (eventArgs.Cancel)
                return;

            try
            { // we should never crash app due to this
                if (MessageBox.Show("Here's what's new in Highlight Hunter " + updateResult.LatestVersion + ":" + Environment.NewLine + Environment.NewLine +
                    updateResult.WhatsNew + Environment.NewLine + Environment.NewLine +
                    "Download it now?",
                    "Update available!",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Information) == DialogResult.Yes)
                {
                    BrowserHelper.LaunchBrowser(updateResult.DownloadPage, "updatecheck");
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }

        private struct UpdateResult
        {
            public string DownloadPage;
            public bool IsUpdateAvailable;
            public string LatestVersion;
            public string WhatsNew;
        }
    }
}
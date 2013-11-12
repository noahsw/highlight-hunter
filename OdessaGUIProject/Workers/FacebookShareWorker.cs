using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using Facebook;
using GaDotNet.Common.Helpers;
using NLog;
using OdessaGUIProject.DRM_Helpers;
using OdessaGUIProject.Other_Helpers;

namespace OdessaGUIProject.Workers
{
    internal class FacebookShareWorker : PublishWorker
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private FacebookClient facebookClient;
        private bool uploadCompleted;

        internal FacebookShareWorker(HighlightObject highlightObject)
            : base(highlightObject)
        {
            
        }

        internal override void PublishWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            #region Check for cancellation

            if (CheckForCancelledWorker())
            {
                PublishWorkerResult = PublishWorkerResults.Cancelled;
                facebookClient = null;
                return;
            }

            if (activationState == Protection.ActivationState.Activated)
                AnalyticsHelper.FireEvent("Each share - activation state - activated");
            else if (activationState == Protection.ActivationState.Trial)
                AnalyticsHelper.FireEvent("Each share - activation state - trial");
            else if (activationState == Protection.ActivationState.TrialExpired)
                AnalyticsHelper.FireEvent("Each share - activation state - trial expired");
            else if (activationState == Protection.ActivationState.Unlicensed)
                AnalyticsHelper.FireEvent("Each share - activation state - unlicensed");


            #endregion Check for cancellation

            #region Prepare video. We always do this, even if the user already saved it, because Facebook has formats it prefers

            using (var saveWorker = new SaveWorker(HighlightObject))
            {
                saveWorker.ProgressChanged += saveWorker_ProgressChanged;

#if RELEASE
                saveWorker.HideOutputFile = true;
#endif

                saveWorker.OutputFormat = SaveWorker.OutputFormats.Facebook;

                try
                {
                    string tempOutputFilePath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + HighlightObject.InputFileObject.SourceFileInfo.Extension);
                    saveWorker.RunWorkerAsync(tempOutputFilePath);

                    // try connecting while we start the saving
                    if (InitializeFacebookClient() == false)
                    {
                        PublishWorkerResult = PublishWorkerResults.UnableToAuthenticate;
                        saveWorker.CancelAsync();
                        return;
                    }

                    while (saveWorker.IsBusy)
                    { // wait for the scanworker to finish
                        if (CheckForCancelledWorker())
                        {
                            saveWorker.CancelAsync();
                            while (saveWorker.IsBusy) { System.Windows.Forms.Application.DoEvents(); } // DoEvents is required: http://social.msdn.microsoft.com/Forums/en-US/clr/thread/ad9a9a02-8a11-4bb8-b50a-613bcaa46886
                            PublishWorkerResult = PublishWorkerResults.Cancelled;
                            facebookClient = null;
                            return;
                        }

                        System.Threading.Thread.Sleep(500);
                    }
                }
                catch (InvalidOperationException ex)
                {
                    Logger.Error("Error starting SaveWorker! " + ex.ToString());
                }

                if (saveWorker.PublishWorkerResult != PublishWorkerResults.Success ||
                    saveWorker.OutputFileInfo == null)
                {
                    ErrorMessage = saveWorker.ErrorMessage;
                    PublishWorkerResult = PublishWorkerResults.UnableToShare;
                    facebookClient = null;
                    return;
                }

            #endregion Prepare video. We always do this, even if the user already saved it, because Facebook has formats it prefers

                #region Upload to Facebook

                var parameters = new Dictionary<string, object>();
                var bytes = File.ReadAllBytes(saveWorker.OutputFileInfo.FullName);

                parameters["source"] = new FacebookMediaObject { ContentType = "video/mpeg", FileName = "video.mp4" }.SetValue(bytes);
                parameters["title"] = HighlightObject.Title;

                // let's be more subtle and not spam their video.
                // it looks like Facebook is using the description field in the newsfeed instead of the title. lame.
                //parameters["description"] = "Found with Highlight Hunter. Download free for Mac and PC at www.HighlightHunter.com.";
                parameters["description"] = HighlightObject.Title;

                facebookClient.PostAsync("/me/videos", parameters);

                Logger.Info("Posted in background");

                trackFirstShare();

                while (!uploadCompleted)
                {
                    if (CheckForCancelledWorker())
                    {
                        facebookClient.CancelAsync();
                        PublishWorkerResult = PublishWorkerResults.Cancelled;
                        break;
                    }

                    Thread.Sleep(500); // this is on a background thread
                }

                #endregion Upload to Facebook

                #region Cleanup

                try
                {
                    saveWorker.OutputFileInfo.Delete();
                }
                catch (Exception ex)
                {
                    Logger.Error("Unable to delete " + ex);
                }

                #endregion Cleanup
            }

            facebookClient = null;

            // result is determined by facebookClient_PostCompleted
            AnalyticsHelper.FireEvent("Each share - Facebook - result - " + PublishWorkerResult);
        }

        internal override bool ViewResult(ref string errorMessage)
        {
            if (HighlightObject.FacebookActivityId.Length == 0)
            {
                Logger.Error("No FacebookActivityId!");
                errorMessage = "Facebook didn't tell us where they uploaded your video. Try opening your news feed and see if it's there.";
                return false;
            }

            if (Properties.Settings.Default.SeenFacebookEncodingWarning == false)
            {
                MessageBox.Show("Your video has been uploaded but sometimes Facebook needs a few minutes before you can watch it. Grab a sandwich and you should be good to go." + Environment.NewLine + Environment.NewLine +
                    "We won't bug you about this again.", "Heads up", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Properties.Settings.Default.SeenFacebookEncodingWarning = true;
                Properties.Settings.Default.Save();
            }

            string url = "https://www.facebook.com/photo.php?v=" + HighlightObject.FacebookActivityId;

            if (BrowserHelper.LaunchBrowser(url) == false)
            {
                errorMessage = "We had trouble launching your browser to " + url;
                return false;
            }

            return true;
        }

        private void facebookClient_PostCompleted(object sender, FacebookApiEventArgs e)
        {
            Logger.Info("Called");

            if (e.Cancelled)
            {
                Logger.Info("Cancelled");
                PublishWorkerResult = PublishWorkerResults.Cancelled;
            }
            else if (e.Error != null)
            {
                Logger.Error("Error uploading: " + e.Error);
                ErrorMessage = e.Error.ToString();
                PublishWorkerResult = PublishWorkerResults.UnableToShare;
            }
            else
            {
                /* format:
                {
                    "id": "xxxxx19208xxxxx"
                }
                 */
                string jsonResponse = e.GetResultData().ToString();

                Logger.Info("Response from Facebook: " + jsonResponse); 

                var structuredResponse = (FacebookJSONResponse)facebookClient.DeserializeJson(jsonResponse, typeof(FacebookJSONResponse));

                /* This works, but it requires extra assemblies
                var ser = new DataContractJsonSerializer(typeof(FacebookJSONResponse));
                var stream = new MemoryStream(Encoding.UTF8.GetBytes(jsonResponse));
                var structuredResponse = (FacebookJSONResponse)ser.ReadObject(stream);
                stream.Close();
                 */

                Logger.Info("FacebookActivityId = " + structuredResponse.id);

                HighlightObject.FacebookActivityId = structuredResponse.id;

                PublishWorkerResult = PublishWorkerResults.Success;
            }

            uploadCompleted = true;
        }

        private void facebookClient_UploadProgressChanged(object sender, FacebookUploadProgressChangedEventArgs e)
        {
            ReportProgress(20 + (int)(e.ProgressPercentage * 0.80));
        }

        private bool InitializeFacebookClient()
        {
            // upload to facebook
            Logger.Info("Connecting to Facebook");

            facebookClient = new FacebookClient(Properties.Settings.Default.FacebookAccessToken);
            facebookClient.PostCompleted += new EventHandler<FacebookApiEventArgs>(facebookClient_PostCompleted);
            facebookClient.UploadProgressChanged += new EventHandler<FacebookUploadProgressChangedEventArgs>(facebookClient_UploadProgressChanged);

            #region make sure access token is still valid

            try
            {
                FacebookHelper.TriggerFacebookConnect(facebookClient);
                return true;
            }
            catch (FacebookOAuthException)
            { // token expired
                Logger.Error("Facebook access token expired!");

                Properties.Settings.Default.FacebookAccessToken = "";
                Properties.Settings.Default.Save();

                PublishWorkerResult = PublishWorkerResults.UnableToAuthenticate;

                facebookClient = null;
                return false;

                /* we don't do this here because we can't launch form from background thread
                var facebookHelper = new FacebookHelper();
                if (facebookHelper.LoginToFacebook() == false)
                {
                    Logger.Error("User cancelled Facebook authentication.");
                    PublishWorkerResult = PublishWorkerResults.UnableToAuthenticate;
                    return;
                }

                // they tried logging in, let's see if it worked this time
                try
                {
                    facebookClient.Get("me/friends");
                }
                catch (FacebookOAuthException)
                {
                    Logger.Error("Still couldn't authenticate, even after logging in again.");
                    PublishWorkerResult = PublishWorkerResults.UnableToAuthenticate;
                    return;
                }
                 */
            }

            #endregion make sure access token is still valid
        }

        private void saveWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            ReportProgress((int)(e.ProgressPercentage * 0.20));
        }

        private void trackFirstShare()
        {
            if (Properties.Settings.Default.HasSharedVideo == false)
            {
                if (AnalyticsHelper.FireEvent("First share"))
                {
                    Properties.Settings.Default.HasSharedVideo = true;
                    Properties.Settings.Default.Save();
                }
            }

            if (Properties.Settings.Default.HasSharedVideoToFacebook == false)
            {
                if (AnalyticsHelper.FireEvent("First share to facebook"))
                {
                    Properties.Settings.Default.HasSharedVideoToFacebook = true;
                    Properties.Settings.Default.Save();
                }
            }
        }
    }
}
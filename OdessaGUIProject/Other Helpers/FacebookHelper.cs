using System;
using System.Runtime.Serialization;
using System.Windows.Forms;
using NLog;
using System.ComponentModel;
using Facebook;
using System.Collections.Generic;

namespace OdessaGUIProject.Other_Helpers
{
    internal class FacebookHelper
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        internal static bool LoginToFacebook()
        {
            const string appId = "228803620534906";
            string extendedPermissions = "publish_actions,email";

            var fbLoginDialog = new FacebookLoginDialog(appId, extendedPermissions);
            fbLoginDialog.ShowDialog();

            var facebookOAuthResult = fbLoginDialog.FacebookOAuthResult;

            if (facebookOAuthResult != null)
            {
                if (facebookOAuthResult.IsSuccess)
                {
                    Properties.Settings.Default.FacebookAccessToken = facebookOAuthResult.AccessToken;
                    Properties.Settings.Default.FacebookAccessTokenExpiration = facebookOAuthResult.Expires;
                    Properties.Settings.Default.Save();

                    Logger.Info("Logged in to Facebook with access token " + Properties.Settings.Default.FacebookAccessToken);
                    Logger.Info("Access token expires at " + Properties.Settings.Default.FacebookAccessTokenExpiration);

                    try
                    {
                        var captureEmailAddressWorker = new BackgroundWorker();
                        captureEmailAddressWorker.DoWork += new DoWorkEventHandler(captureEmailAddressWorker_DoWork);
                        captureEmailAddressWorker.RunWorkerAsync();
                    }
                    catch { }

                    return true;
                }
                else
                {
                    Properties.Settings.Default.FacebookAccessToken = "";

                    MessageBox.Show("Ah bummer -- we weren't able to sign you in to Facebook." + Environment.NewLine + Environment.NewLine +
                        "Here's what Facebook told us: " + facebookOAuthResult.ErrorDescription,
                        "Error logging in", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Logger.Error("Error signing into Facebook: " + facebookOAuthResult.Error + " - " + facebookOAuthResult.ErrorDescription);
                    return false;
                }
            }
            else
                return false;
        }

        static void captureEmailAddressWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                var client = new FacebookClient(Properties.Settings.Default.FacebookAccessToken);
                TriggerFacebookConnect(client);
            }
            catch { }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        /// <exception cref="FacebookOAuthException">Thrown when access token is invalid</exception>
        internal static void TriggerFacebookConnect(FacebookClient client)
        {
         
            // Get the user's profile information
            var result = (IDictionary<string, object>)client.Get("/me",
                new
                {
                    fields = "name,first_name,last_name,email",
                });

            client = null;

            var name = (string)result["name"];
            var firstName = (string)result["first_name"];
            var lastName = (string)result["last_name"];
            var email = (string)result["email"];

            // Send it to us
            var url = BrowserHelper.Host + "/facebook-connect-trigger.php";
            var data = new Dictionary<string, string>();

            data.Add("name", name);
            data.Add("firstName", firstName);
            data.Add("lastName", lastName);
            data.Add("email", email);

            PostDataHelper.PostData(url, data);

        }

    }

    [DataContract]
    internal class FacebookJSONResponse
    {
        [DataMember]
        internal string id { get; set; }
    }


}
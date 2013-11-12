namespace OdessaGUIProject
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Forms;
    using Facebook;
    using OdessaGUIProject.UI_Helpers;

    internal partial class FacebookLoginDialog : Form
    {
        protected readonly FacebookClient _fb;
        private readonly Uri _loginUrl;

        internal FacebookLoginDialog(string appId, string extendedPermissions)
            : this(new FacebookClient(), appId, extendedPermissions)
        {
        }

        internal FacebookLoginDialog(FacebookClient fb, string appId, string extendedPermissions)
        {
            if (fb == null)
                throw new ArgumentNullException("fb");
            if (string.IsNullOrEmpty(appId))
                throw new ArgumentNullException("appId");

            _fb = fb;
            _loginUrl = GenerateLoginUrl(appId, extendedPermissions);

            InitializeComponent();

            DesignLanguage.ApplyCustomFont(this.Controls);
        }

        internal FacebookOAuthResult FacebookOAuthResult { get; private set; }

        private void FacebookLoginDialog_Load(object sender, EventArgs e)
        {
            // make sure to use AbsoluteUri.
            webBrowser.Navigate(_loginUrl.AbsoluteUri);
        }

        private Uri GenerateLoginUrl(string appId, string extendedPermissions)
        {
            // for .net 3.5
            var parameters = new Dictionary<string, object>();
            parameters["client_id"] = appId;
            parameters["redirect_uri"] = "https://www.facebook.com/connect/login_success.html";
            parameters["response_type"] = "token";
            parameters["display"] = "popup";
            if (!string.IsNullOrEmpty(extendedPermissions))
                parameters["scope"] = extendedPermissions;

            /*
            dynamic parameters = new ExpandoObject();
            parameters.client_id = appId;
            parameters.redirect_uri = "https://www.facebook.com/connect/login_success.html";

            // The requested response: an access token (token), an authorization code (code), or both (code token).
            parameters.response_type = "token";

            // list of additional display modes can be found at http://developers.facebook.com/docs/reference/dialogs/#display
            parameters.display = "popup";

            // add the 'scope' parameter only if we have extendedPermissions.
            if (!string.IsNullOrEmpty(extendedPermissions))
                parameters.scope = extendedPermissions;
             */

            // when the Form is loaded navigate to the login url.
            return _fb.GetLoginUrl(parameters);
        }

        private void webBrowser_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            loadingLabel.Visible = false;

            // whenever the browser navigates to a new url, try parsing the url.
            // the url may be the result of OAuth 2.0 authentication.

            FacebookOAuthResult oauthResult;
            if (_fb.TryParseOAuthCallbackUrl(e.Url, out oauthResult))
            {
                // The url is the result of OAuth 2.0 authentication
                FacebookOAuthResult = oauthResult;
                DialogResult = FacebookOAuthResult.IsSuccess ? DialogResult.OK : DialogResult.No;
            }
            else
            {
                // The url is NOT the result of OAuth 2.0 authentication.
                FacebookOAuthResult = null;
            }
        }

        private void FacebookLoginDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            webBrowser.Stop();
        }
    }
}
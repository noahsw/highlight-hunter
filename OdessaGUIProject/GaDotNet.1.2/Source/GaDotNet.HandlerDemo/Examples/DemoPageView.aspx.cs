using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GaDotNet.Common;
using GaDotNet.Common.Data;
using GaDotNet.Common.Helpers;

namespace GaDotNet.HandlerDemo.Examples
{
	public partial class DemoPageView : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{

		}

		protected void Button1_Click(object sender, EventArgs e)
		{
			string pageURL = String.Format("/demo-page-view-{0}.aspx", DateTime.Now.ToString("HH-mm"));

			GooglePageView pageView = new GooglePageView("Demo Page View",txtDomainName.Text, pageURL);

			TrackingRequest request = new RequestFactory().BuildRequest(pageView, HttpContext.Current);

			GoogleTracking.FireTrackingEvent(request);

			litResult.Text = "Done!";
		}
	}
}
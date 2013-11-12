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
	public partial class DemoEvent : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{

		}

		protected void Button1_Click(object sender, EventArgs e)
		{
			GoogleEvent googleEvent = new GoogleEvent(txtDomain.Text,"Demo","Demo Testing","Demo Event - "+DateTime.Now.ToString("HH:mm:ss"),100);

			TrackingRequest request = new RequestFactory().BuildRequest(googleEvent, HttpContext.Current);
			
			GoogleTracking.FireTrackingEvent(request);

			litResult.Text = "Done!";
		}
	}
}
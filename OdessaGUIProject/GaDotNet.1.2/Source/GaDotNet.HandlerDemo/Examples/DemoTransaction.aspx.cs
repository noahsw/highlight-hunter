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
	public partial class DemoTransaction : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{

		}

		protected void Button1_Click(object sender, EventArgs e)
		{
			GoogleTransaction trans = new GoogleTransaction();

			trans.Affiliation = "Affiliated to X";
			trans.City = "Sydney";
			trans.Country = "Australia";
			trans.OrderID = "1";
			trans.ProductName = String.Format("Demo: Example Product - {0} ", DateTime.Now.ToString("HH:mm:ss"));
			trans.ProductSku = "DEMOSKU1234";
			trans.ProductVariant = "Red";
			trans.Quantity = 2;
			trans.ShippingCost = (decimal)12.50;
			trans.State = "NSW";
			trans.TaxCost = (decimal) 1.00;
			trans.TotalCost = 10;
			trans.UnitPrice = 5;

			TrackingRequest request = new RequestFactory().BuildRequest(trans);

			request.RequestedByIpAddress = Request.UserHostAddress;

			GoogleTracking.FireTrackingEvent(request);

			litResult.Text = "Done!";
		}
	}
}
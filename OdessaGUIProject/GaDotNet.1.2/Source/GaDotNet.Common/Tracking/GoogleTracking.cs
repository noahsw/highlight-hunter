/*
 * Facebook Google Analytics Tracker
 * Copyright 2010 Doug Rathbone
 * http://www.diaryofaninja.com
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/

using System.Net;
//using System.Web;
using GaDotNet.Common.Data;
using GaDotNet.Common.Helpers;

namespace GaDotNet.Common.Tracking
{
    internal class GoogleTracking
	{
        /*
	    /// <summary>
	    /// Tracks the page view  with GA and stream a GIF image
	    /// </summary>
	    /// <param name="context">The context.</param>
	    public static void TrackPageViewWithImage(HttpContext context)
		{
			//build request
			TrackingRequest request = new RequestFactory().BuildRequest(context);

			FireTrackingEvent(request);

			ShowTrackingImage(context);

		}
		/// <summary>
		/// Tracks the page view and streams a GIF image.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="pageView">The page view.</param>
		public static void TrackPageViewWithImage(HttpContext context, GooglePageView pageView)
		{
			//build request
			TrackingRequest request = new RequestFactory().BuildRequest(pageView, context);

			FireTrackingEvent(request);

			ShowTrackingImage(context);
		}

		/// <summary>
		/// Shows the tracking image.
		/// </summary>
		/// <param name="context">The context.</param>
		private static void ShowTrackingImage(HttpContext context)
		{
			//data to show a 1x1 pixel transparent gif
			byte[] gifData = {
					  0x47, 0x49, 0x46, 0x38, 0x39, 0x61,
					  0x01, 0x00, 0x01, 0x00, 0x80, 0xff,
					  0x00, 0xff, 0xff, 0xff, 0x00, 0x00,
					  0x00, 0x2c, 0x00, 0x00, 0x00, 0x00,
					  0x01, 0x00, 0x01, 0x00, 0x00, 0x02,
					  0x02, 0x44, 0x01, 0x00, 0x3b
				  };

			context.Response.ContentType = "image/gif";
			context.Response.AddHeader("Cache-Control", "private, no-cache, no-cache=Set-Cookie, proxy-revalidate");
			context.Response.AddHeader("Pragma", "no-cache");
			context.Response.AddHeader("Expires", "Wed, 17 Sep 1975 21:32:10 GMT");
			context.Response.Buffer = false;
			context.Response.OutputStream.Write(gifData, 0, gifData.Length);
			context.Response.End();
		}
         */
 

		/// <summary>
		/// Fires the tracking event with Google Analytics
		/// </summary>
		/// <param name="request">The request.</param>
        internal static void FireTrackingEvent(TrackingRequest request)
		{
		    //send the request to google
			WebRequest requestForGaGif = WebRequest.Create(request.TrackingGifUri);
		    using (requestForGaGif.GetResponse())
		    {
		        //ignore response
		    }
		}
	}
}

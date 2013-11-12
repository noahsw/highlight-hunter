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

//using System.Web;
using GaDotNet.Common.Data;

namespace GaDotNet.Common.Helpers
{
	/// <summary>
	/// The TrackingRequest factory - this helps you build your request with whatever data
	/// </summary>
    public static class RequestFactory
	{


        /*
	    /// <summary>
	    /// Builds the tracking request.
	    /// </summary>
	    /// <param name="context">The HTTP context.</param>
	    /// <returns></returns>
	    public TrackingRequest BuildRequest(HttpContext context)
		{
	        var r = new TrackingRequest
	                    {
	                        PageTitle = context.Request.QueryString["pagetitle"],
	                        PageDomain = context.Request.QueryString["domain"],
	                        AnalyticsAccountCode =
	                            context.Request.QueryString["ua"] ?? ConfigurationSettings.GoogleAccountCode,
	                        PageUrl = context.Request.QueryString["url"],
	                        RequestedByIpAddress = context.Request.UserHostAddress
	                    };



	        return r;
		}
         */ 

		/// <summary>
		/// Builds the request from a page view request and the appSettings 'GoogleAnalyticsAccountCode'
		/// </summary>
		/// <param name="pageView">The page view.</param>
		/// <returns></returns>
        internal static TrackingRequest BuildRequest(GooglePageView pageView)
		{
		    var r = new TrackingRequest
		                {
		                    PageTitle = pageView.PageTitle,
		                    PageDomain = pageView.DomainName,
		                    AnalyticsAccountCode = ConfigurationSettings.GoogleAccountCode,
		                    PageUrl = pageView.Url
		                };


		    return r;
		}

        /*
		/// <summary>
		/// Builds the request.
		/// </summary>
		/// <param name="pageView">The page view.</param>
		/// <param name="context">The context.</param>
		/// <returns></returns>
		public TrackingRequest BuildRequest(GooglePageView pageView, HttpContext context)
		{
			var r = BuildRequest(pageView);

			// add users IP address
			r.RequestedByIpAddress = context.Request.UserHostAddress;

			return r;
		}
        */


		/// <summary>
		/// Builds the tracking request from a Google Event.
		/// </summary>
		/// <param name="googleEvent">The google event.</param>
		/// <returns></returns>
        internal static TrackingRequest BuildRequest(GoogleEvent googleEvent)
		{
		    var r = new TrackingRequest
		                {
		                    AnalyticsAccountCode = ConfigurationSettings.GoogleAccountCode,
                            TrackingEvent = googleEvent
		                };


		    return r;
		}

        /*
		/// <summary>
		/// Builds the tracking request from a Google Event.
		/// </summary>
		/// <param name="googleEvent">The google event.</param>
		/// <param name="context">The context.</param>
		/// <returns></returns>
		public TrackingRequest BuildRequest(GoogleEvent googleEvent, HttpContext context)
		{
			var r = BuildRequest(googleEvent);

			r.RequestedByIpAddress = context.Request.UserHostAddress;

			return r;
		}
         */ 

		/// <summary>
		/// Builds the tracking request from a Google Transaction.
		/// </summary>
		/// <param name="googleTransaction">The google transaction.</param>
		/// <returns></returns>
        internal static TrackingRequest BuildRequest(GoogleTransaction googleTransaction)
		{
		    var r = new TrackingRequest
		                {
		                    AnalyticsAccountCode = ConfigurationSettings.GoogleAccountCode,
		                    TrackingTransaction = googleTransaction
		                };


		    return r;
		}
	}
}

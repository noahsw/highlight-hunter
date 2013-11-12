using System.IO;
using GaDotNet.Common.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Web;
using GaDotNet.Common.Data;

namespace GaDotNet.Tests
{
    
    
    /// <summary>
    ///This is a test class for RequestFactoryTest and is intended
    ///to contain all RequestFactoryTest Unit Tests
    ///</summary>
	[TestClass()]
	public class RequestFactoryTest
	{


		private TestContext testContextInstance;

		/// <summary>
		///Gets or sets the test context which provides
		///information about and functionality for the current test run.
		///</summary>
		public TestContext TestContext
		{
			get
			{
				return testContextInstance;
			}
			set
			{
				testContextInstance = value;
			}
		}

		#region Additional test attributes
		// 
		//You can use the following additional attributes as you write your tests:
		//
		//Use ClassInitialize to run code before running the first test in the class
		//[ClassInitialize()]
		//public static void MyClassInitialize(TestContext testContext)
		//{
		//}
		//
		//Use ClassCleanup to run code after all tests in a class have run
		//[ClassCleanup()]
		//public static void MyClassCleanup()
		//{
		//}
		//
		//Use TestInitialize to run code before running each test
		//[TestInitialize()]
		//public void MyTestInitialize()
		//{
		//}
		//
		//Use TestCleanup to run code after each test has run
		//[TestCleanup()]
		//public void MyTestCleanup()
		//{
		//}
		//
		#endregion


		/// <summary>
		/// A test for BuildRequest from a web context
		/// </summary>
		[TestMethod()]
		public void BuildRequest_From_HttpContext()
		{

			//build fake context with fake request
			string queryString = "pagetitle=My%20Page%20Title&domain=mydomain.com&url=/test.aspx";
			HttpRequest request = new HttpRequest("/tracker.asmx", "http://localhost/tracker.asmx", queryString);
			StringWriter sw = new StringWriter();
			HttpResponse response = new HttpResponse(sw);
			HttpContext context = new HttpContext(request, response);


			TrackingRequest actual = new RequestFactory().BuildRequest(context);


			Assert.AreEqual(actual.PageDomain, "mydomain.com");
			Assert.AreEqual(actual.PageUrl, "/test.aspx");
			Assert.AreEqual(actual.PageTitle, "My Page Title");
		}

		/// <summary>
		///A test for BuildRequest
		///</summary>
		[TestMethod()]
		public void BuildRequest_From_Google_Transaction()
		{
			GoogleTransaction googleTransaction = new GoogleTransaction();
			googleTransaction.Affiliation = "Affiliation";

			TrackingRequest actual = new RequestFactory().BuildRequest(googleTransaction);

			Assert.AreEqual(actual.TrackingTransaction, googleTransaction);
		}

		/// <summary>
		///A test for BuildRequest from a Google PageView
		///</summary>
		[TestMethod()]
		public void BuildRequest_From_Google_PageView()
		{
			GooglePageView pageView = new GooglePageView("My Page Title","mydomain.com","/test.aspx");
			TrackingRequest actual = new RequestFactory().BuildRequest(pageView);

			Assert.AreEqual(actual.PageDomain, "mydomain.com");
			Assert.AreEqual(actual.PageUrl, "/test.aspx");
			Assert.AreEqual(actual.PageTitle, "My Page Title");
		}

		/// <summary>
		///A test for BuildRequest
		///</summary>
		[TestMethod()]
		public void BuildRequest_From_Google_Event()
		{
			GoogleEvent googleEvent = new GoogleEvent("mydomain.com","My Category","My Action", "My Label", 100);
			TrackingRequest actual = new RequestFactory().BuildRequest(googleEvent);
			
			Assert.AreEqual(actual.TrackingEvent, googleEvent);
		}
	}
}

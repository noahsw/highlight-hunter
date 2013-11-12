using GaDotNet.Common.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace GaDotNet.Tests
{
    
    
    /// <summary>
    ///This is a test class for GoogleHashHelperTest and is intended
    ///to contain all GoogleHashHelperTest Unit Tests
    ///</summary>
	[TestClass()]
	public class GoogleHashHelperTest
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
		///A test for ConvertToUnixTimestamp to check a day in seconds
		///</summary>
		[TestMethod()]
		public void ConvertToUnixTimestampTest()
		{
			DateTime value = new DateTime(1970, 1, 2);
			int expected = 46800; 
			int actual;
			actual = GoogleHashHelper.ConvertToUnixTimestamp(value);
			Assert.AreEqual(expected, actual);
		}
	}
}

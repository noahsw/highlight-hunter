using Microsoft.VisualStudio.TestTools.UnitTesting;
using OdessaPCTestHelpers;
using System.Diagnostics;

namespace TestProject
{
    /// <summary>
    /// Summary description for EngineFindDarkFramesUnitTest
    /// </summary>
    [TestClass]
    public class EngineFindDarkFramesUnitTest
    {

        private TestContext testContextInstance;

        private FindDarkFramesHelper findDarkFrames;

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

        [TestInitialize]
        public void Initialize()
        {
        }

        [TestCleanup]
        public void Cleanup()
        {
            //findDarkFrames.Unload();
            //findDarkFrames = null;
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion


        [TestMethod]
        public void FindDarkFramesGOPR0014()
        {
            FindDarkFramesHelper.TestResult tr = findDarkFrames.FindDarkFramesTest("GOPR0_paddle_014.MP4");
            if (tr.Failed) Assert.Fail();
        }


        [TestMethod]
        public void FindDarkFramesGideon_webcam_1()
        {
            using (findDarkFrames = new FindDarkFramesHelper())
            {
#if DEBUG
                findDarkFrames.SetCustomDetectionThreshold(100, 75, 3, 0.35f, 1f);
#endif
                FindDarkFramesHelper.TestResult tr = findDarkFrames.FindDarkFramesTest("Gideon_webcam.m4v.highlightObject.00.03.09.to.00.03.25.m4v");
                if (tr.Failed) Assert.Fail();
            }
        }

        [TestMethod]
        public void FindDarkFramesTubing_1()
        {
            using (findDarkFrames = new FindDarkFramesHelper())
            {
                findDarkFrames.SetCustomDetectionThreshold(70, 80, 3, 0.35f, 0.3f);
                FindDarkFramesHelper.TestResult tr = findDarkFrames.FindDarkFramesTest("tubing GO019316 highlight #10 - 11.50 to 12.00.MP4");
                if (tr.Failed) Assert.Fail();
            }
        }

        [TestMethod]
        public void FindDarkFramesGideon_webcam_2()
        {
            FindDarkFramesHelper.TestResult tr = findDarkFrames.FindDarkFramesTest("Gideon_webcam.m4v.highlightObject.00.06.29.to.00.06.44.m4v");
            if (tr.Failed) Assert.Fail();
        }

        [TestMethod]
        public void FindDarkFramesGideon_webcam_3()
        {
            FindDarkFramesHelper.TestResult tr = findDarkFrames.FindDarkFramesTest("Gideon_webcam.m4v.highlightObject.00.24.15.to.00.24.30.m4v");
            if (tr.Failed) Assert.Fail();
        }

        [TestMethod]
        public void FindDarkFrames_Marius()
        {
            using (findDarkFrames = new FindDarkFramesHelper())
            {
                findDarkFrames.SetCustomDetectionThreshold(65, 55, 3, 0.35f, 1f);
                FindDarkFramesHelper.TestResult tr = findDarkFrames.FindDarkFramesTest("GOPR8417_marius.MP4");
                if (tr.Failed) Assert.Fail();
            }
        }


#if TODO
        [TestMethod]
        public void FindDarkFramesContourSample()
        {
            FileInfo InputFile = InputFiles["Contour_sample.MOV"];
            FindDarkFramesTest(InputFile, true);
        }

        [TestMethod]
        public void FindDarkFramesContourAllBlack()
        {
            FileInfo InputFile = InputFiles["Contour_allblack.MOV"];
            FindDarkFramesTest(InputFile, true);
        }

        [TestMethod]
        public void FindDarkFramesGOPR0015()
        {
            FileInfo InputFile = InputFiles["GOPR0015"];
            FindDarkFramesTest(InputFile, true);
        }

        [TestMethod]
        public void FindDarkFramesGOPR0016()
        {
            FileInfo InputFile = InputFiles["GOPR0016"];
            FindDarkFramesTest(InputFile, true);

            /* Test results
             * 
11/21/2010 6:16:09 PM: 	- Frame 1132 (00:00:18): False positive
11/21/2010 6:16:09 PM: 	- Frame 1798 (00:00:29): Found
11/21/2010 6:16:09 PM: 	- Frame 8051 (00:02:14): False positive
11/21/2010 6:16:09 PM: 	- Frame 10045 (00:02:47): False positive
11/21/2010 6:16:09 PM: 	- Frame 13906 (00:03:51): Found
11/21/2010 6:16:09 PM: 	- Frame 26339 (00:07:19): False positive (true false positive)
11/21/2010 6:16:09 PM: 	- Frame 26853 (00:07:27): Critical error: Not found (7:27 is collapsing into 7:30. 7:30 should not be caught. that would resolve this critical error)
11/21/2010 6:16:09 PM: 	- Frame 27133 (00:07:32): False positive
11/21/2010 6:16:09 PM: 	- Frame 27572 (00:07:39): Found
11/21/2010 6:16:09 PM: 	- Frame 29070 (00:08:04): Critical error: Not found
11/21/2010 6:16:09 PM: 	- Frame 29325 (00:08:09): False positive
11/21/2010 6:16:09 PM: 	- Frame 30717 (00:08:32): False positive
             * 
             */
        }

        [TestMethod]
        public void FindDarkFramesGOPR0020()
        {
            FileInfo InputFile = InputFiles["GOPR0020"];
            FindDarkFramesTest(InputFile, true);
            /*
11/27/2010 4:28:48 PM: 	- Frame 14775 (00:04:06): FalsePositive
11/27/2010 4:28:48 PM: 	- Frame 19360 (00:05:22): Found
             */ 
        }

        [TestMethod]
        public void FindDarkFramesGOPR0022()
        {
            FileInfo InputFile = InputFiles["GOPR0022"];
            FindDarkFramesTest(InputFile, true);
            /*
11/27/2010 4:11:52 PM: 	Test results summary:
11/27/2010 4:11:52 PM: 	- Frame 555 (00:00:09): FalsePositive
11/27/2010 4:11:52 PM: 	- Frame 9590 (00:02:39): Found
             */
        }

        [TestMethod]
        public void FindDarkFramesGOPR0023()
        {
            FileInfo InputFile = InputFiles["GOPR0023"];
            FindDarkFramesTest(InputFile, true);

            /*
11/27/2010 4:41:56 PM: 	Test results summary:
11/27/2010 4:41:56 PM: 	- Frame 7312 (00:02:01): Found
             */ 
        }

        [TestMethod]
        public void FindDarkFramesGOPR0024()
        {
            FileInfo InputFile = InputFiles["GOPR0024"];
            FindDarkFramesTest(InputFile, true);

            /*
11/27/2010 8:31:25 PM: 	Test results summary:
11/27/2010 8:31:25 PM: 	- Frame 229 (00:00:03): FalsePositive
11/27/2010 8:31:25 PM: 	- Frame 4272 (00:01:11): FalsePositive
11/27/2010 8:31:25 PM: 	- Frame 6298 (00:01:45): FalsePositive
             */ 
        }

        [TestMethod]
        public void FindDarkFramesGOPR0030_1()
        {
            FileInfo InputFile = InputFiles["GOPR0030_1"];
            FindDarkFramesTest(InputFile, true);

            /*
11/27/2010 8:39:25 PM: 	Test results summary:
11/27/2010 8:39:25 PM: 	- Frame 731 (00:00:12): FalsePositive
11/27/2010 8:39:25 PM: 	- Frame 1661 (00:00:27): FalsePositive
11/27/2010 8:39:25 PM: 	- Frame 2426 (00:00:40): FalsePositive
11/27/2010 8:39:25 PM: 	- Frame 3289 (00:00:54): FalsePositive
             */
        }

        [TestMethod]
        public void FindDarkFramesGOPR0030_2()
        {
            FileInfo InputFile = InputFiles["GOPR0030_2"];
            FindDarkFramesTest(InputFile, true);

            /*
11/27/2010 8:47:49 PM: 	Test results summary:
11/27/2010 8:47:49 PM: 	- Frame 179 (00:00:02): Found
             */ 
        }

        [TestMethod]
        public void FindDarkFramesGOPR0030_3()
        {
            FileInfo InputFile = InputFiles["GOPR0030_3"];
            FindDarkFramesTest(InputFile, true);

            /*
11/27/2010 8:50:51 PM: 	Test results summary:
11/27/2010 8:50:51 PM: 	- Frame 70 (00:00:01): FalsePositive
             */ 
        }

        [TestMethod]
        public void FindDarkFramesGOPR0030_4()
        {
            FileInfo InputFile = InputFiles["GOPR0030_4"];
            FindDarkFramesTest(InputFile, true);

            /*
11/27/2010 8:51:45 PM: 	Test results summary:
             */ 
        }

        [TestMethod]
        public void FindDarkFramesGOPR0030_5()
        {
            FileInfo InputFile = InputFiles["GOPR0030_5"];
            FindDarkFramesTest(InputFile, true);

            /*
11/27/2010 8:52:55 PM: 	Test results summary:
11/27/2010 8:52:55 PM: 	- Frame 1018 (00:00:16): Found
11/27/2010 8:52:55 PM: 	- Frame 1676 (00:00:27): FalsePositive
             */ 
        }

        [TestMethod]
        public void FindDarkFramesGOPR0030_6()
        {
            FileInfo InputFile = InputFiles["GOPR0030_6"];
            FindDarkFramesTest(InputFile, true);

            /*
11/27/2010 8:54:06 PM: 	Test results summary:
11/27/2010 8:54:06 PM: 	- Frame 719 (00:00:11): Found
             */ 
        }

        [TestMethod]
        public void FindDarkFramesGOPR0030_7()
        {
            FileInfo InputFile = InputFiles["GOPR0030_7"];
            FindDarkFramesTest(InputFile, true);

            /*
11/27/2010 8:55:35 PM: 	Test results summary:
11/27/2010 8:55:35 PM: 	- Frame 2217 (00:00:36): Found
             */ 
        }

        [TestMethod]
        public void FindDarkFramesGOPR0030_8()
        {
            FileInfo InputFile = InputFiles["GOPR0030_8"];
            FindDarkFramesTest(InputFile, true);

            /*
11/27/2010 8:57:04 PM: 	Test results summary:
11/27/2010 8:57:04 PM: 	- Frame 1130 (00:00:18): FalsePositive
11/27/2010 8:57:04 PM: 	- Frame 1798 (00:00:29): Found
             */
        }

        [TestMethod]
        public void FindDarkFramesGOPR0_snow_023()
        {
            FileInfo InputFile = InputFiles["GOPR0_snow_023.MP4"];
            FindDarkFramesTest(InputFile, true);

        }

        [TestMethod]
        public void FindDarkFramesGOPR0_bike_030_5()
        {
            //NativeOdessaMethods_Accessor.SetCustomDetectionThreshold(85, 90, 0.1f, 6);

            FileInfo InputFile = InputFiles["GOPR0_bike_030_5.MP4"];
            FindDarkFramesTest(InputFile, true);

        }

        [TestMethod]
        public void FindDarkFramesContour_snow_mini_avalanche()
        {
            FileInfo InputFile = InputFiles["Contour_snow_mini avalanche.MOV"];
            FindDarkFramesTest(InputFile, true);

        }

        [TestMethod]
        public void FindDarkFramesGOPR0_paddle_015_spliced_00_00_54_to_00_01_09()
        {
            FileInfo InputFile = InputFiles["GOPR0_paddle_015.MP4.spliced.00.00.54.to.00.01.09.MP4"];
            FindDarkFramesTest(InputFile, true);

        }

        [TestMethod]
        public void FindDarkFramesContour_snow_false_positive_shade_4()
        {
            //NativeOdessaMethods_Accessor.SetCustomDetectionThreshold(75, 75, 0.1f, 12);

            FileInfo InputFile = InputFiles["Contour_snow_false positive shade 4.MOV"];
            FindDarkFramesTest(InputFile, true);

        }
#endif

    }
}

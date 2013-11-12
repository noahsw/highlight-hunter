using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;

namespace TestProject
{
    
    
    /// <summary>
    ///This is a test class for EngineTest and is intended
    ///to contain all EngineTest Unit Tests
    ///</summary>
    [TestClass()]
    public class EngineTest
    {
        //private static Logging_Accessor Logger;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {
            //Logger = new Logging_Accessor("Odessa_EngineTest.log");
            Engine.Initialize();
        }
        //
        //Use ClassCleanup to run code after all tests in a class have run
        [ClassCleanup()]
        public static void MyClassCleanup()
        {
            //Logger.CloseLogger();
        }
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
        ///A test for CreateHighlightObjects
        ///</summary>
        [TestMethod()]
        public void FindVideoChunksTest()
        {

            const string filePath = @"D:\Projects\Odessa\OdessaCheckout\OdessaProductionSolution\TestProject\Test Videos\Contour_snow_speed.MOV";

            var sw = new ScanWorker
                         {InputFile = new FileInfo(filePath), FramesPerSecond = 59.94f, VideoDurationInSeconds = 200};

            var darkFrameNumbers = new List<long>
                                       {
                                           1040,
                                           1041,
                                           1042,
                                           1043,
                                           1044,
                                           1045,
                                           1046,
                                           1047,
                                           1048,
                                           1049,
                                           1050,
                                           1051,
                                           1052,
                                           1053,
                                           7778,
                                           7779,
                                           7780,
                                           7781,
                                           7782,
                                           7783,
                                           7784,
                                           7785,
                                           7786,
                                           7787
                                       };

            const int captureDurationInSeconds = 30;
            const bool ignoreEarlyHighlights = false;
            const bool useCaptureOffset = true;
            
            var expected = new List<VideoChunk>
                               {new VideoChunk(0, 1172, filePath), new VideoChunk(5989, 7906, filePath)};

            List<VideoChunk> actual = 
                Engine_Accessor.FindVideoChunks(sw, darkFrameNumbers, captureDurationInSeconds, ignoreEarlyHighlights, useCaptureOffset);

            if (actual.Count != expected.Count)
            {
                Assert.Fail("actual.Count != expected.Count");
            }

            for (int i = 0; i < expected.Count; i++)
            {
                if (expected[i].StartFrame != actual[i].StartFrame)
                {
                    Assert.Fail("expected[" + i + "].StartFrame = " + expected[i].StartFrame + " but actual[" + i + "].StartFrame = " + actual[i].StartFrame);
                }
                if (expected[i].EndFrame != actual[i].EndFrame)
                {
                    Assert.Fail("expected[" + i + "].EndFrame = " + expected[i].EndFrame + " but actual[" + i + "].EndFrame = " + actual[i].EndFrame);
                }
            }

            
        }

        
    }
}

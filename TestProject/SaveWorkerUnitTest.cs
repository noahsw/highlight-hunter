using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OdessaGUIProject;
using OdessaGUIProject.Workers;
using System.IO;
using System.ComponentModel;

namespace TestProject
{
    [TestClass]
    public class SaveWorkerUnitTest
    {

        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {
            MainModel_Accessor.Initialize();
        }

        //Use ClassCleanup to run code after all tests in a class have run
        [ClassCleanup()]
        public static void MyClassCleanup()
        {
            MainModel_Accessor.Dispose();
        }

        public static void TestSaveWorker(string inputFileName, SaveWorker_Accessor.OutputFormats outputFormat, bool useWatermark = false)
        {

            var inputFileObject = new InputFileObject_Accessor()
            {
                SourceFileInfo = new FileInfo(inputFileName),
            };

            var scanWorker = new ScanWorker_Accessor(inputFileObject);
            scanWorker.SetBitrate();
            scanWorker.SetFramesPerSecond();
            scanWorker.SetTotalFrames();
            scanWorker.SetVideoDimensions();
            scanWorker.SetVideoDuration();

            var highlightObject = new HighlightObject_Accessor()
            {
                InputFileObject = inputFileObject,
                StartTime = TimeSpan.FromSeconds(2),
                EndTime = TimeSpan.FromSeconds(8)
            };

            var saveWorker = new SaveWorker_Accessor(highlightObject);
            saveWorker.OutputFormat = outputFormat;
            saveWorker.ForceWatermark = useWatermark;

            var outputFileName = Path.Combine(Path.GetTempPath(), inputFileObject.SourceFileInfo.Name);
            saveWorker.PublishWorker_DoWork(null, new DoWorkEventArgs(outputFileName));

            var success = (saveWorker.PublishWorkerResult.value__ == PublishWorker_Accessor.PublishWorkerResults.Success.value__);

            // don't delete so we can play the files manually ourselves if we want. they'll be overwritten next time anyways.
            //if (saveWorker.OutputFileInfo != null && saveWorker.OutputFileInfo.Exists) // use OutputFileInfo because extension may change in process (for ProRes files)
            //    saveWorker.OutputFileInfo.Delete();

            if (!success)
                Assert.Fail(saveWorker.ErrorMessage);

        }

        #region IPhone
        [TestMethod]
        public void SaveIPhoneAsOriginal()
        {
            TestSaveWorker(@"D:\Projects\Odessa\Sample Files\Matt 30 second intro on iPhone.MOV", SaveWorker_Accessor.OutputFormats.Original, false);
        }

        [TestMethod]
        public void SaveIPhoneAsOriginalWithWatermark()
        {
            TestSaveWorker(@"D:\Projects\Odessa\Sample Files\Matt 30 second intro on iPhone.MOV", SaveWorker_Accessor.OutputFormats.Original, true);
        }

        [TestMethod]
        public void SaveIPhoneAsProRes()
        {
            TestSaveWorker(@"D:\Projects\Odessa\Sample Files\Matt 30 second intro on iPhone.MOV", SaveWorker_Accessor.OutputFormats.ProRes);
        }

        [TestMethod]
        public void SaveIPhoneAsFacebook()
        {
            TestSaveWorker(@"D:\Projects\Odessa\Sample Files\Matt 30 second intro on iPhone.MOV", SaveWorker_Accessor.OutputFormats.Facebook);
        }
        #endregion

        #region Contour_1280x720_30fps
        [TestMethod]
        public void SaveContour_1280x720_30fpsAsOriginal()
        {
            TestSaveWorker(@"D:\Projects\Odessa\Sample Files\Contour_1280x720_30fps.MOV", SaveWorker_Accessor.OutputFormats.Original, false);
        }

        [TestMethod]
        public void SaveContour_1280x720_30fpsAsOriginalWithWatermark()
        {
            TestSaveWorker(@"D:\Projects\Odessa\Sample Files\Contour_1280x720_30fps.MOV", SaveWorker_Accessor.OutputFormats.Original, true);
        }

        [TestMethod]
        public void SaveContour_1280x720_30fpsAsProRes()
        {
            TestSaveWorker(@"D:\Projects\Odessa\Sample Files\Contour_1280x720_30fps.MOV", SaveWorker_Accessor.OutputFormats.ProRes);
        }

        [TestMethod]
        public void SaveContour_1280x720_30fpsAsFacebook()
        {
            TestSaveWorker(@"D:\Projects\Odessa\Sample Files\Contour_1280x720_30fps.MOV", SaveWorker_Accessor.OutputFormats.Facebook);
        }
        #endregion

        #region Contour_1280x720_60fps.MOV
        [TestMethod]
        public void SaveContour_1280x720_60fpsAsOriginal()
        {
            TestSaveWorker(@"D:\Projects\Odessa\Sample Files\Contour_1280x720_60fps.MOV", SaveWorker_Accessor.OutputFormats.Original, false);
        }

        [TestMethod]
        public void SaveContour_1280x720_60fpsAsOriginalWithWatermark()
        {
            TestSaveWorker(@"D:\Projects\Odessa\Sample Files\Contour_1280x720_60fps.MOV", SaveWorker_Accessor.OutputFormats.Original, true);
        }

        [TestMethod]
        public void SaveContour_1280x720_60fpsAsProRes()
        {
            TestSaveWorker(@"D:\Projects\Odessa\Sample Files\Contour_1280x720_60fps.MOV", SaveWorker_Accessor.OutputFormats.ProRes);
        }

        [TestMethod]
        public void SaveContour_1280x720_60fpsAsFacebook()
        {
            TestSaveWorker(@"D:\Projects\Odessa\Sample Files\Contour_1280x720_60fps.MOV", SaveWorker_Accessor.OutputFormats.Facebook);
        }
        #endregion
        
        #region Contour_1280x960_30fps.MOV
        [TestMethod]
        public void SaveContour_1280x960_30fpsAsOriginal()
        {
            TestSaveWorker(@"D:\Projects\Odessa\Sample Files\Contour_1280x960_30fps.MOV", SaveWorker_Accessor.OutputFormats.Original, false);
        }

        [TestMethod]
        public void SaveContour_1280x960_30fpsAsOriginalWithWatermark()
        {
            TestSaveWorker(@"D:\Projects\Odessa\Sample Files\Contour_1280x960_30fps.MOV", SaveWorker_Accessor.OutputFormats.Original, true);
        }

        [TestMethod]
        public void SaveContour_1280x960_30fpsAsProRes()
        {
            TestSaveWorker(@"D:\Projects\Odessa\Sample Files\Contour_1280x960_30fps.MOV", SaveWorker_Accessor.OutputFormats.ProRes);
        }

        [TestMethod]
        public void SaveContour_1280x960_30fpsAsFacebook()
        {
            TestSaveWorker(@"D:\Projects\Odessa\Sample Files\Contour_1280x960_30fps.MOV", SaveWorker_Accessor.OutputFormats.Facebook);
        }
        #endregion

        #region Contour_1920_1080_30fps.MOV
        [TestMethod]
        public void SaveContour_1920_1080_30fpsAsOriginal()
        {
            TestSaveWorker(@"D:\Projects\Odessa\Sample Files\Contour_1920_1080_30fps.MOV", SaveWorker_Accessor.OutputFormats.Original, false);
        }

        [TestMethod]
        public void SaveContour_1920_1080_30fpsAsOriginalWithWatermark()
        {
            TestSaveWorker(@"D:\Projects\Odessa\Sample Files\Contour_1920_1080_30fps.MOV", SaveWorker_Accessor.OutputFormats.Original, true);
        }

        [TestMethod]
        public void SaveContour_1920_1080_30fpsAsProRes()
        {
            TestSaveWorker(@"D:\Projects\Odessa\Sample Files\Contour_1920_1080_30fps.MOV", SaveWorker_Accessor.OutputFormats.ProRes);
        }

        [TestMethod]
        public void SaveContour_1920_1080_30fpsAsFacebook()
        {
            TestSaveWorker(@"D:\Projects\Odessa\Sample Files\Contour_1920_1080_30fps.MOV", SaveWorker_Accessor.OutputFormats.Facebook);
        }
        #endregion

        #region epic_hd_vid_sm_06.MOV
        [TestMethod]
        public void SaveEpic_hd_vid_sm_06AsOriginal()
        {
            TestSaveWorker(@"D:\Projects\Odessa\Sample Files\epic_hd_vid_sm_06.MOV", SaveWorker_Accessor.OutputFormats.Original, false);
        }

        [TestMethod]
        public void SaveEpic_hd_vid_sm_06AsOriginalWithWatermark()
        {
            TestSaveWorker(@"D:\Projects\Odessa\Sample Files\epic_hd_vid_sm_06.MOV", SaveWorker_Accessor.OutputFormats.Original, true);
        }

        [TestMethod]
        public void SaveEpic_hd_vid_sm_06AsProRes()
        {
            TestSaveWorker(@"D:\Projects\Odessa\Sample Files\epic_hd_vid_sm_06.MOV", SaveWorker_Accessor.OutputFormats.ProRes);
        }

        [TestMethod]
        public void SaveEpic_hd_vid_sm_06AsFacebook()
        {
            TestSaveWorker(@"D:\Projects\Odessa\Sample Files\epic_hd_vid_sm_06.MOV", SaveWorker_Accessor.OutputFormats.Facebook);
        }
        #endregion

        #region GOPR_r1_848x480_30fps.MP4
        [TestMethod]
        public void SaveGOPR_r1_848x480_30fpsAsOriginal()
        {
            TestSaveWorker(@"D:\Projects\Odessa\Sample Files\GOPR_r1_848x480_30fps.MP4", SaveWorker_Accessor.OutputFormats.Original, false);
        }

        [TestMethod]
        public void SaveGOPR_r1_848x480_30fpsAsOriginalWithWatermark()
        {
            TestSaveWorker(@"D:\Projects\Odessa\Sample Files\GOPR_r1_848x480_30fps.MP4", SaveWorker_Accessor.OutputFormats.Original, true);
        }

        [TestMethod]
        public void SaveGOPR_r1_848x480_30fpsAsProRes()
        {
            TestSaveWorker(@"D:\Projects\Odessa\Sample Files\GOPR_r1_848x480_30fps.MP4", SaveWorker_Accessor.OutputFormats.ProRes);
        }

        [TestMethod]
        public void SaveGOPR_r1_848x480_30fpsAsFacebook()
        {
            TestSaveWorker(@"D:\Projects\Odessa\Sample Files\GOPR_r1_848x480_30fps.MP4", SaveWorker_Accessor.OutputFormats.Facebook);
        }
        #endregion

        #region GOPR_r2_1280x720_30fps.MP4
        [TestMethod]
        public void SaveGOPR_r2_1280x720_30fpsAsOriginal()
        {
            TestSaveWorker(@"D:\Projects\Odessa\Sample Files\GOPR_r2_1280x720_30fps.MP4", SaveWorker_Accessor.OutputFormats.Original, false);
        }

        [TestMethod]
        public void SaveGOPR_r2_1280x720_30fpsAsOriginalWithWatermark()
        {
            TestSaveWorker(@"D:\Projects\Odessa\Sample Files\GOPR_r2_1280x720_30fps.MP4", SaveWorker_Accessor.OutputFormats.Original, true);
        }

        [TestMethod]
        public void SaveGOPR_r2_1280x720_30fpsAsProRes()
        {
            TestSaveWorker(@"D:\Projects\Odessa\Sample Files\GOPR_r2_1280x720_30fps.MP4", SaveWorker_Accessor.OutputFormats.ProRes);
        }

        [TestMethod]
        public void SaveGOPR_r2_1280x720_30fpsAsFacebook()
        {
            TestSaveWorker(@"D:\Projects\Odessa\Sample Files\GOPR_r2_1280x720_30fps.MP4", SaveWorker_Accessor.OutputFormats.Facebook);
        }
        #endregion

        #region GOPR_r3_1280x720_60fps.MP4
        [TestMethod]
        public void SaveGOPR_r3_1280x720_60fpsAsOriginal()
        {
            TestSaveWorker(@"D:\Projects\Odessa\Sample Files\GOPR_r3_1280x720_60fps.MP4", SaveWorker_Accessor.OutputFormats.Original, false);
        }

        [TestMethod]
        public void SaveGOPR_r3_1280x720_60fpsAsOriginalWithWatermark()
        {
            TestSaveWorker(@"D:\Projects\Odessa\Sample Files\GOPR_r3_1280x720_60fps.MP4", SaveWorker_Accessor.OutputFormats.Original, true);
        }

        [TestMethod]
        public void SaveGOPR_r3_1280x720_60fpsAsProRes()
        {
            TestSaveWorker(@"D:\Projects\Odessa\Sample Files\GOPR_r3_1280x720_60fps.MP4", SaveWorker_Accessor.OutputFormats.ProRes);
        }

        [TestMethod]
        public void SaveGOPR_r3_1280x720_60fpsAsFacebook()
        {
            TestSaveWorker(@"D:\Projects\Odessa\Sample Files\GOPR_r3_1280x720_60fps.MP4", SaveWorker_Accessor.OutputFormats.Facebook);
        }
        #endregion

        #region GOPR_r4_1280x960_30fps.MP4
        [TestMethod]
        public void SaveGOPR_r4_1280x960_30fpsAsOriginal()
        {
            TestSaveWorker(@"D:\Projects\Odessa\Sample Files\GOPR_r4_1280x960_30fps.MP4", SaveWorker_Accessor.OutputFormats.Original, false);
        }

        [TestMethod]
        public void SaveGOPR_r4_1280x960_30fpsAsOriginalWithWatermark()
        {
            TestSaveWorker(@"D:\Projects\Odessa\Sample Files\GOPR_r4_1280x960_30fps.MP4", SaveWorker_Accessor.OutputFormats.Original, true);
        }

        [TestMethod]
        public void SaveGOPR_r4_1280x960_30fpsAsProRes()
        {
            TestSaveWorker(@"D:\Projects\Odessa\Sample Files\GOPR_r4_1280x960_30fps.MP4", SaveWorker_Accessor.OutputFormats.ProRes);
        }

        [TestMethod]
        public void SaveGOPR_r4_1280x960_30fpsAsFacebook()
        {
            TestSaveWorker(@"D:\Projects\Odessa\Sample Files\GOPR_r4_1280x960_30fps.MP4", SaveWorker_Accessor.OutputFormats.Facebook);
        }
        #endregion

        #region GOPR_r5_1920x1080_30fps.MP4
        [TestMethod]
        public void SaveGOPR_r5_1920x1080_30fpsAsOriginal()
        {
            TestSaveWorker(@"D:\Projects\Odessa\Sample Files\GOPR_r5_1920x1080_30fps.MP4", SaveWorker_Accessor.OutputFormats.Original, false);
        }

        [TestMethod]
        public void SaveGOPR_r5_1920x1080_30fpsAsOriginalWithWatermark()
        {
            TestSaveWorker(@"D:\Projects\Odessa\Sample Files\GOPR_r5_1920x1080_30fps.MP4", SaveWorker_Accessor.OutputFormats.Original, true);
        }

        [TestMethod]
        public void SaveGOPR_r5_1920x1080_30fpsAsProRes()
        {
            TestSaveWorker(@"D:\Projects\Odessa\Sample Files\GOPR_r5_1920x1080_30fps.MP4", SaveWorker_Accessor.OutputFormats.ProRes);
        }

        [TestMethod]
        public void SaveGOPR_r5_1920x1080_30fpsAsFacebook()
        {
            TestSaveWorker(@"D:\Projects\Odessa\Sample Files\GOPR_r5_1920x1080_30fps.MP4", SaveWorker_Accessor.OutputFormats.Facebook);
        }
        #endregion

        #region Tachyon Sample.AVI
        [TestMethod]
        public void SaveTachyonSampleAsOriginal()
        {
            TestSaveWorker(@"D:\Projects\Odessa\Sample Files\Tachyon Sample.AVI", SaveWorker_Accessor.OutputFormats.Original, false);
        }

        [TestMethod]
        public void SaveTachyonSampleAsOriginalWithWatermark()
        {
            TestSaveWorker(@"D:\Projects\Odessa\Sample Files\Tachyon Sample.AVI", SaveWorker_Accessor.OutputFormats.Original, true);
        }

        [TestMethod]
        public void SaveTachyonSampleAsProRes()
        {
            TestSaveWorker(@"D:\Projects\Odessa\Sample Files\Tachyon Sample.AVI", SaveWorker_Accessor.OutputFormats.ProRes);
        }

        [TestMethod]
        public void SaveTachyonSampleAsFacebook()
        {
            TestSaveWorker(@"D:\Projects\Odessa\Sample Files\Tachyon Sample.AVI", SaveWorker_Accessor.OutputFormats.Facebook);
        }
        #endregion

        #region PanasonicLumixAVCHD.MTS
        [TestMethod]
        public void SavePanasonicLumixAVCHDAsOriginal()
        {
            TestSaveWorker(@"D:\Projects\Odessa\Sample Files\PanasonicLumixAVCHD.MTS", SaveWorker_Accessor.OutputFormats.Original, false);
        }

        [TestMethod]
        public void SavePanasonicLumixAVCHDAsOriginalWithWatermark()
        {
            TestSaveWorker(@"D:\Projects\Odessa\Sample Files\PanasonicLumixAVCHD.MTS", SaveWorker_Accessor.OutputFormats.Original, true);
        }

        [TestMethod]
        public void SavePanasonicLumixAVCHDAsProRes()
        {
            TestSaveWorker(@"D:\Projects\Odessa\Sample Files\PanasonicLumixAVCHD.MTS", SaveWorker_Accessor.OutputFormats.ProRes);
        }

        [TestMethod]
        public void SavePanasonicLumixAVCHDAsFacebook()
        {
            TestSaveWorker(@"D:\Projects\Odessa\Sample Files\PanasonicLumixAVCHD.MTS", SaveWorker_Accessor.OutputFormats.Facebook);
        }
        #endregion

        #region Pro Snowboarder Justin Morgan.MOV
        [TestMethod]
        public void SaveProSnowboarderJustinMorganAsOriginal()
        {
            TestSaveWorker(@"D:\Projects\Odessa\Sample Files\Pro snowboarder Justin Morgan.mov", SaveWorker_Accessor.OutputFormats.Original, false);
        }

        [TestMethod]
        public void SaveProSnowboarderJustinMorganAsOriginalWithWatermark()
        {
            TestSaveWorker(@"D:\Projects\Odessa\Sample Files\Pro snowboarder Justin Morgan.mov", SaveWorker_Accessor.OutputFormats.Original, true);
        }

        [TestMethod]
        public void SaveProSnowboarderJustinMorganAsProRes()
        {
            TestSaveWorker(@"D:\Projects\Odessa\Sample Files\Pro snowboarder Justin Morgan.mov", SaveWorker_Accessor.OutputFormats.ProRes);
        }

        [TestMethod]
        public void SaveProSnowboarderJustinMorganAsFacebook()
        {
            TestSaveWorker(@"D:\Projects\Odessa\Sample Files\Pro snowboarder Justin Morgan.mov", SaveWorker_Accessor.OutputFormats.Facebook);
        }
        #endregion

    }
}

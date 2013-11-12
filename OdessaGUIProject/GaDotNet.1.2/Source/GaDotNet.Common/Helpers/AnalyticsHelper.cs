using System;
using System.ComponentModel;
using System.IO;
using System.Net;
using GaDotNet.Common.Data;
using GaDotNet.Common.Tracking;
using NLog;
using System.Collections.Generic;
using System.Threading;

namespace GaDotNet.Common.Helpers
{
    
    public static class AnalyticsHelper
    {

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private static string _cachedIp = "";

        private static bool _errorRetrievingIp;

        private static bool _isWorking = false;
        
        private static object locker = new object();

        private static Queue<GoogleEvent> AnalyticsEventQueue = new Queue<GoogleEvent>();

        public static bool OptedIn { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventAction"></param>
        /// <param name="eventValue"></param>
        /// <returns>Whether user opted in to analytics or not</returns>
        public static bool FireEvent(string eventAction, int? eventValue = 0)
        {
            if (OptedIn == false)
                return false;

            Logger.Info("Fired event: {0} {1}", eventAction, eventValue);

            if (Properties.Settings.Default.UserGUID == Guid.Empty)
            {
                Properties.Settings.Default.UserGUID = Guid.NewGuid();
                Properties.Settings.Default.Save();
            }

            string guidValue = Properties.Settings.Default.UserGUID.ToString();

            var googleEvent = new GoogleEvent("highlighthunter.com",
                "PC " + ConfigurationSettings.AppProductVersion,
                eventAction,
                guidValue,
                eventValue);


            lock (locker)
            {
                AnalyticsEventQueue.Enqueue(googleEvent);

                if (_isWorking == false)
                {
                    _isWorking = true;
                    RunWorker();
                }

            }

            return true;
            
        }

        static void RunWorker()
        {
            var bwFireEventWorker = new BackgroundWorker();
            bwFireEventWorker.DoWork += BwFireEventWorkerDoWork;
            bwFireEventWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwFireEventWorker_RunWorkerCompleted);
            try
            {
                bwFireEventWorker.RunWorkerAsync(AnalyticsEventQueue.Dequeue());
            }
            catch (Exception ex)
            { // we never want to crash due to analytics
                Logger.Error("Exception when trying to run Analytics background worker: " + ex.ToString());
            }

        }

        static void bwFireEventWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (AnalyticsEventQueue.Count > 0)
            {
                RunWorker();
            }
            else
                _isWorking = false;
        }

        static void BwFireEventWorkerDoWork(object sender, DoWorkEventArgs e)
        {
            var googleEvent = (GoogleEvent)e.Argument;

            TrackingRequest request =
                RequestFactory.BuildRequest(googleEvent);

            request.RequestedByIpAddress = GetPublicIp();

#if DEBUG
            Logger.Debug("Google Analytics url: " + request.TrackingGifUrl);
#endif

            try
            { // never crash due to analytics tracking
                GoogleTracking.FireTrackingEvent(request);
            }
            catch (Exception ex)
            {
                Logger.Error("Exception while firing analytics event: " + ex);
            }

            Thread.Sleep(1000);

        }


        internal static string GetPublicIp()
        {

            if (_cachedIp.Length > 0)
                return _cachedIp;

            if (_errorRetrievingIp)
                return "";

            try
            {
                WebRequest request = WebRequest.Create("http://checkip.dyndns.org/");
                string direction = null;
                using (WebResponse response = request.GetResponse())
                {
                    Stream stream = response.GetResponseStream();
                    if (stream != null)
                    {
                        using (var streamReader = new StreamReader(stream))
                        {
                            direction = streamReader.ReadToEnd();
                        }
                    }
                }

                //Search for the ip in the html
                if (direction != null)
                {
                    int first = direction.IndexOf("Address: ", StringComparison.Ordinal) + 9;
                    int last = direction.LastIndexOf("</body>", StringComparison.Ordinal);
                    direction = direction.Substring(first, last - first);
                }

                _cachedIp = direction;

                return direction;
            }
            catch (Exception ex)
            {
                _errorRetrievingIp = true;
                Logger.Error("Couldn't get public IP: " + ex.ToString());
                return "";
            }
            
        }


        public static string GetFriendlyOsVersion()
        {
            String str = Environment.OSVersion.Version.ToString();
            String osName;
            if (str.Contains("5.1.2600"))
                osName = "Windows XP";
            else if (str.Contains("5.2.3790"))
                osName = "Windows XP Pro x64";
            else if (str.Contains("6.0.6000"))
                osName = "Windows Vista";
            else if (str.Contains("6.0.6001"))
                osName = "Windows Vista SP1";
            else if (str.Contains("6.0.6002"))
                osName = "Windows Vista SP2";
            else if (str.Contains("6.1.76"))
                osName = "Windows 7";
            else
                osName = str;

            return osName + " " + Environment.OSVersion.ServicePack;
        }

    }
}

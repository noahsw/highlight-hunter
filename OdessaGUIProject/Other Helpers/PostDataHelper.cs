using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using NLog;
using System.Windows.Forms;

namespace OdessaGUIProject.Other_Helpers
{
    static class PostDataHelper
    {

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        internal static void PostData(string url, Dictionary<string, string> data, bool includeDebugLogs = false)
        {

            try
            {

                data.Add("version", "PC " + Application.ProductVersion);

                string postData = "";
                foreach (var kvp in data)
                    postData += kvp.Key + "=" + Uri.EscapeDataString(kvp.Value) + "&";

                if (includeDebugLogs)
                {
                    foreach (KeyValuePair<string, string> kvp in debugLogs())
                    {
                        // it's already escaped. we have to do this because the debug log are too long for Uri.EscapeDataString()
                        postData += kvp.Key + "=" + kvp.Value + "&";
                    }
                }

                // Create a request using a URL that can receive a post.
                WebRequest request = WebRequest.Create(url);
                // Set the Method property of the request to POST.
                request.Method = "POST";
                // Create POST data and convert it to a byte array.

                byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                // Set the ContentType property of the WebRequest.
                request.ContentType = "application/x-www-form-urlencoded";
                // Set the ContentLength property of the WebRequest.
                request.ContentLength = byteArray.Length;
                // Get the request stream.
                Stream dataStream = request.GetRequestStream();
                // Write the data to the request stream.
                dataStream.Write(byteArray, 0, byteArray.Length);
                // Close the Stream object.
                dataStream.Close();
                // Get the response.
                WebResponse response = request.GetResponse();
                // Display the status.
                Console.WriteLine(((HttpWebResponse)response).StatusDescription);
                // Get the stream containing content returned by the server.
                dataStream = response.GetResponseStream();
                // Open the stream using a StreamReader for easy access.
                StreamReader reader = new StreamReader(dataStream);
                // Read the content.
                string responseFromServer = reader.ReadToEnd();
                // Display the content.
                Console.WriteLine(responseFromServer);
                // Clean up the streams.
                reader.Close();
                dataStream.Close();
                response.Close();

            }
            catch (Exception ex)
            {
                Logger.Error("Error while posting data: {0}", ex);
            }

        }

        private static Dictionary<string, string> debugLogs()
        {
            var dict = new Dictionary<string, string>();

            string[] files = Directory.GetFiles(Path.GetTempPath(), "HighlightHunter-*.log");
            var list = new List<FileInfo>();
            foreach (string file in files)
            {
                if (file.Contains(".Archive."))
                    continue;

                using (var fs = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    var text = new StringBuilder();
                    using (var sr = new StreamReader(fs))
                    {
                        List<String> lst = new List<string>();  
 
                        while (!sr.EndOfStream)  
                            text.AppendLine( Uri.EscapeDataString(sr.ReadLine()));
                    }
                    dict.Add(Path.GetFileName(file), text.ToString());
                }
                
            }

            return dict;
        }

    }
}

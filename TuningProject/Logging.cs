using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

namespace TuningHostProject
{
    internal class Logging
    {

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        private class MEMORYSTATUSEX
        {
            public uint dwLength;
            public uint dwMemoryLoad;
            public ulong ullTotalPhys;
            public ulong ullAvailPhys;
            public ulong ullTotalPageFile;
            public ulong ullAvailPageFile;
            public ulong ullTotalVirtual;
            public ulong ullAvailVirtual;
            public ulong ullAvailExtendedVirtual;
            public MEMORYSTATUSEX()
            {
                this.dwLength = (uint)Marshal.SizeOf(typeof(MEMORYSTATUSEX));
            }
        }

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern bool GlobalMemoryStatusEx([In, Out] MEMORYSTATUSEX lpBuffer);
        
        
        private StreamWriter LogWriter;

        /// <summary>
        /// The file path to where the log is being saved
        /// </summary>
        public string LogFilePath { get; set; }


        /// <summary>
        /// Create the class
        /// </summary>
        /// <param name="LogFileName"></param>
        internal Logging(string LogFileName)
        {
            InitLogger(LogFileName);
        }


        internal Logging()
        {
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="LogFileName"></param>
        /// <param name="Append"></param>
        /// <param name="WriteHeader"> </param>
        /// <param name="ParentDirectory"> </param>
        /// <returns>Returns whether a new file was created</returns>
        public bool InitLogger(string LogFileName = "Odessa_debug.log", bool Append = false, 
            bool WriteHeader = true, DirectoryInfo ParentDirectory = null)
        {

            bool ret = true;
            try
            {
                if (ParentDirectory == null)
                    ParentDirectory = new DirectoryInfo(Path.GetTempPath());
                this.LogFilePath = Path.Combine(ParentDirectory.FullName, LogFileName);

                ret = !(File.Exists(this.LogFilePath));
                LogWriter = new StreamWriter(LogFilePath, Append);

                if (WriteHeader)
                    WriteToLog("LoggingHelper.InitLogger(): Writing logs to " + LogFilePath);

            }
            catch (Exception ex)
            {
                Debug.WriteLine("LoggingHelper.InitLogger(): Error creating StreamWriter for " + LogFilePath + ": " + ex.Message);
            }

            return ret;
        }

        public void CloseLogger(bool WriteCloser = true)
        {
            if (LogWriter != null && WriteCloser)
            {
                WriteToLog("Logger.CloseLogger(): Closing down logger: " + LogFilePath);
            }

            try
            {
                LogWriter.Close();
                LogWriter = null;
            }
            catch (Exception)
            { // we don't care
                // Debug.WriteLine("Logger.CloseLogger(): Exception closing LogWriter: " + ex.ToString());
            }

        }

        ~Logging()
        {
            if (LogWriter != null)
                CloseLogger();
        }


        /// <summary>
        /// Writes a string out to the log file
        /// </summary>
        /// <param name="str"></param>
        /// <param name="PrintDateTime"></param>
        /// <returns>Returns whether write was successful</returns>
        [DebuggerStepThrough]
        public bool WriteToLog(string str, bool PrintDateTime = true)
        {

            string LogString = DateTime.Now + ": \t" + str;
            if (PrintDateTime == false)
                LogString = str;

            //Handled by Console.WriteLine()
            //Debug.WriteLine(LogString);

            Console.WriteLine(LogString);

            bool ret;
            if (LogWriter != null)
            {
                try
                {
                    LogWriter.WriteLine(LogString);
                    LogWriter.Flush();
                    ret = true;
                }
                catch
                {
                    ret = false;
                }
            }
            else
                ret = false;

            return ret;
        }

        public void WriteSystemInformation()
        {

            WriteToLog("Assembly Version: " + Assembly.GetExecutingAssembly().GetName().Version.ToString());

            WriteToLog("OS: " + Environment.OSVersion.ToString());

            WriteToLog("64-bit: " + Environment.Is64BitOperatingSystem.ToString());

            WriteToLog("Processor count: " + Environment.ProcessorCount.ToString());

            // RAM
            ulong installedMemory;
            MEMORYSTATUSEX memStatus = new MEMORYSTATUSEX();
            if (GlobalMemoryStatusEx(memStatus))
            {
                installedMemory = memStatus.ullTotalPhys;
                WriteToLog("Installed RAM: " + (installedMemory / 1024 / 1024).ToString("0,000") + "mb");
                WriteToLog("Available RAM: " + (memStatus.ullAvailPhys / 1024 / 1024).ToString("0,000") + "mb");
            } 

            
        
        }

    }
}

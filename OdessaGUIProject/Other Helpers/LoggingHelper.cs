using System;
using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices;
using NLog;

namespace OdessaGUIProject
{
    internal static class LoggingHelper
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        internal static void WriteSystemInformation()
        {
            Logger.Info("Assembly Version: " + Assembly.GetExecutingAssembly().GetName().Version);

            Logger.Info("OS: " + GaDotNet.Common.Helpers.AnalyticsHelper.GetFriendlyOsVersion());

            Logger.Info("Architecture: " + GetOSArchitecture());

            Logger.Info("Processor count: " + Environment.ProcessorCount.ToString(CultureInfo.InvariantCulture));

            // RAM
            var memStatus = new MEMORYSTATUSEX();
            if (GlobalMemoryStatusEx(memStatus))
            {
                ulong installedMemory = memStatus.ullTotalPhys;
                Logger.Info("Installed RAM: " + (installedMemory / 1024 / 1024).ToString("0,000", CultureInfo.InvariantCulture) + "mb");
                Logger.Info("Available RAM: " + (memStatus.ullAvailPhys / 1024 / 1024).ToString("0,000", CultureInfo.InvariantCulture) + "mb");
            }
        }

        private static string GetOSArchitecture()
        {
            string pa = Environment.GetEnvironmentVariable("PROCESSOR_ARCHITECTURE");
            return pa;
        }

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool GlobalMemoryStatusEx([In, Out] MEMORYSTATUSEX lpBuffer);

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        // ReSharper disable InconsistentNaming
        private class MEMORYSTATUSEX
        // ReSharper restore InconsistentNaming
        {
            // ReSharper disable NotAccessedField.Local
            private uint dwLength;

            // ReSharper restore NotAccessedField.Local
            public uint dwMemoryLoad;

#pragma warning disable 649
            public ulong ullTotalPhys;
#pragma warning restore 649
#pragma warning disable 649
            public ulong ullAvailPhys;
#pragma warning restore 649
            public ulong ullTotalPageFile;
            public ulong ullAvailPageFile;
            public ulong ullTotalVirtual;
            public ulong ullAvailVirtual;
            public ulong ullAvailExtendedVirtual;

            public MEMORYSTATUSEX()
            {
                dwLength = (uint)Marshal.SizeOf(typeof(MEMORYSTATUSEX));
            }
        }
    }
}
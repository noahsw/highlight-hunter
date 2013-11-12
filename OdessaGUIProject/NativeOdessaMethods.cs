using System;
using System.Runtime.InteropServices;

namespace OdessaGUIProject
{
    public static class NativeOdessaMethods
    {
        internal enum OdessaReturnCodes
        {
            Success = 0,
            AlreadyScanning = -1,
            NotAuthorized = -2,
            ErrorOpeningFile = -3,
            ErrorFindingStreamInformation = -4,
            ErrorFindingVideoStream = -5,
            UnsupportedCodec = -6,
            ErrorOpeningCodec = -7,
            ErrorAllocatedFrameStructure = -8,
            NoFramesFound = -9,
            ErrorCalculatingFramesPerSecond = -10,
            ErrorCalculatingDimensions = -11,
            InvalidThresholds = -12,
            ErrorWritingResults = -13,
            Authorized = -14,
            ErrorCalculatingDuration = -15,
            ErrorCalculatingTotalFrames = -16,
            ErrorCalculatingBitrate = -17,
            InvalidFormatContext = -18,
            ErrorDeterminingCodec = -19,
            NotEnoughFramesFound = -20
        }

        [DllImport("OdessaEngineV2Project.dll",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void CancelScan();

        [DllImport("OdessaEngineV2Project.dll",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Dispose();

        [DllImport("OdessaEngineV2Project.dll",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void FreePointer(IntPtr pointer);

        [DllImport("OdessaEngineV2Project.dll",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int GetBitrate(string inputfile, out int bitrate);

        [DllImport("OdessaEngineV2Project.dll",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int GetCodec(string inputfile, out string codec);

        [DllImport("OdessaEngineV2Project.dll",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int GetDimensions(string inputfile, out int width, out int height);

        [DllImport("OdessaEngineV2Project.dll",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int GetDuration(string inputfile, out double duration);

        [DllImport("OdessaEngineV2Project.dll",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int GetFramesPerSecond(string inputfile, out double fps);

        [DllImport("OdessaEngineV2Project.dll",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern long GetFramesProcessed();

        [DllImport("OdessaEngineV2Project.dll",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool GetPercentDone(out int percent);

        [DllImport("OdessaEngineV2Project.dll",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int GetTotalFrames(string inputfile, out long frames);

        [DllImport("OdessaEngineV2Project.dll",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int Initialize(string logpath);

        [DllImport("OdessaEngineV2Project.dll",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr Scan(string inputfile, out int listSize, out int returnCode);

        [DllImport("OdessaEngineV2Project.dll",
            CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void SetDetectionThreshold(int detectionThreshold);

#if TEST
		[DllImport("OdessaEngineV2Project.dll",
			CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
		public static extern void SetCustomDetectionThreshold(
			int thresholdIndividualPixelBrightness,
			int thresholdDarkPixelsPerFrameAsPercentage,
			int thresholdPixelScanPercentage,
			float thresholdSecondsSkip,
			float thresholdConsecutiveDarkFramesInSeconds);
#endif
        /*
		public static long[] GetFrames() {
			long[] result = null;
			int size;

			IntPtr arrayValue = IntPtr.Zero;
			try
			{
				arrayValue = GetList(out size);
				if (arrayValue != IntPtr.Zero)
				{
					result = new long[size];
					Marshal.Copy(arrayValue, result, 0, size);
				}
			}
			finally
			{
				// don't forget to free the list
				FreePointer(arrayValue);
			}

			return result;

			/*
			IntPtr arrayValue = IntPtr.Zero;
			int size = 0;
			var list = new List<int>();

			//bool b = GetList(out arrayValue, out size);
			arrayValue = GetList(out size);
			//if ( !GetFrames(out arrayValue, out size)) {
			//    return null;
			//}

			int[] result = new int[size];

			Marshal.Copy(arrayValue, result, 0, size);

			// call release of arrayValue

			/*
			var tableEntrySize = Marshal.SizeOf(typeof(int));
			for ( var i = 0; i < size; i++)
			{
				var cur = (int)Marshal.PtrToStructure(arrayValue, typeof(int));
				list.Add(cur);
				arrayValue = new IntPtr(arrayValue.ToInt32() + tableEntrySize);
			}

			 *             return result;
		}
	*/
    }
}
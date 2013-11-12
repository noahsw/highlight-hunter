// OdessaEngineV2Project.h

#include <stdint.h> // for int64_t

enum OdessaReturnCodes
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
	ErrorCalculatingFPS = -10,
	ErrorCalculatingDimensions = -11,
	InvalidThresholds = -12,
	ErrorWritingResults = -13,
	Authorized = -14,
	ErrorCalculatingDuration = -15,
	ErrorCalculatingTotalFrames = -16,
	ErrorCalculatingBitrate = -17,
	InvalidFormatContext = -18,
	ErrorDeterminingCodec = -19,
	NotEnoughFramesFound = -20,
};

// OdessaEngineV2Project.h

#if defined(__cplusplus) && defined(TARGET_OS_MAC)
extern "C" {
#endif
    
#ifdef WIN32
	extern "C" __declspec(dllexport) 
#endif
    int Initialize(const char *logpath);
    
#ifdef WIN32
	extern "C" __declspec(dllexport) 
#endif
    void Dispose();
    
#ifndef RELEASE // we don't want this publically exposed in the release build
#ifdef WIN32
	extern "C" __declspec(dllexport) 
#endif
	void SetCustomDetectionThreshold(
                                     int thresholdIndividualPixelBrightness, 
                                     int thresholdDarkPixelsPerFrameAsPercentage, 
                                     int thresholdPixelScanPercentage,
                                     float thresholdSecondsSkip,
                                     float thresholdConsecutiveDarkFramesInSeconds);
#endif
    
#ifdef WIN32
	extern "C" __declspec(dllexport) 
#endif
	void SetDetectionThreshold(int detectionThreshold);
    
#ifdef WIN32
	extern "C" __declspec(dllexport) 
#endif
	long long* Scan(const char* inputfile, int *listSize, int* returnCode); // const char* outputfile, 
    
#ifdef WIN32
	extern "C" __declspec(dllexport) 
#endif
    void FreePointer(long long* p);
    
#ifdef WIN32
	extern "C" __declspec(dllexport) 
#endif
    int ScanTest(int*[]);
    
#ifdef WIN32
	extern "C" __declspec(dllexport) 
#endif
    long long* GetList(unsigned long* pSize);
    
#ifdef WIN32
	extern "C" __declspec(dllexport) 
#endif
    void CancelScan();
    
#ifdef WIN32
	extern "C" __declspec(dllexport) 
#endif
	bool GetPercentDone(int *percent);  // C uses pointers instead of & as by reference
    
#ifdef WIN32
	extern "C" __declspec(dllexport) 
#endif
	long long GetFramesProcessed();
    
#ifdef WIN32
	extern "C" __declspec(dllexport) 
#endif
	int GetTotalFrames(const char *inputfile, int64_t *totalFramesReturned);
    
#ifdef WIN32
	extern "C" __declspec(dllexport) 
#endif
    int GetDuration(const char *inputfile, double *duration);
    
#ifdef WIN32
	extern "C" __declspec(dllexport) 
#endif
    int GetFramesPerSecond(const char *inputfile, double *fps);
    
#ifdef WIN32
	extern "C" __declspec(dllexport) 
#endif
	int GetBitrate(const char *inputfile, int *bitrate);
    
#ifdef WIN32
	extern "C" __declspec(dllexport) 
#endif
    int GetDimensions(const char *inputfile, int *width, int *height);
    
#ifdef WIN32
	extern "C" __declspec(dllexport) 
#endif
    int GetCodec(const char *inputfile, const char **codec);
	
#if defined(__cplusplus) && defined(TARGET_OS_MAC)
}
#endif

#ifdef __cplusplus // needed for Mac to compile
vector<int64_t> CollapseDarkFrameLocations(const vector<int64_t> input, double framesPerSecond, int frameSkip);

	#ifdef DEBUG
	int WriteJPEG (AVCodecContext *pCodecCtx, AVFrame *pFrame, int FrameNo);

	int SimplifiedScan(const char* inputfile);
	#endif

#endif

void InitLogger(const char *logpath);

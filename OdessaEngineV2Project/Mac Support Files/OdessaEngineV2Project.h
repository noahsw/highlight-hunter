// OdessaEngineV2Project.h

#ifdef __cplusplus
extern "C" {
#endif

    
    bool Initialize(const char *logpath);
    
    void Dispose();
    
#ifdef DEBUG // we don't want this publically exposed in the release build
	void SetCustomDetectionThreshold(
                                     int thresholdIndividualPixelBrightness, 
                                     int thresholdDarkPixelsPerFrameAsPercentage,
                                     float thresholdSecondsSkip,
                                     int thresholdPixelScanPercentage,
                                     float thresholdConsecutiveDarkFramesInSeconds);
#endif
    
	void SetDetectionThreshold(int detectionThreshold);
    
	long long* Scan(const char* inputfile, int *listSize, int* returnCode); // const char* outputfile, 
    
    void FreePointer(long long* p);
    
    int ScanTest(int*[]);
    
    long long* GetList(unsigned long* pSize);
    
    void CancelScan();
    
	bool GetPercentDone(int *percent);  // C uses pointers instead of & as by reference
    
	long long GetFramesProcessed();
    
	long long GetTotalFrames();
    
    int GetDuration(const char *inputfile, double *duration);
    
    int GetFramesPerSecond(const char *inputfile, double *fps);
    
	int GetBitrate(const char *inputfile, int *bitrate);
    
    int GetDimensions(const char *inputfile, int *width, int *height);
    
    int GetCodec(const char *inputfile, const char **codec);
	
#ifdef __cplusplus
}
#endif

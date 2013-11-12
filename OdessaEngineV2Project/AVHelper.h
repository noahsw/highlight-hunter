#include "Stdafx.h"
#include <fstream>

using namespace std;

class AVHelper {
    public:
        AVHelper(const char* inputfile, Log *pLogReference);
        ~AVHelper();

		int GetDuration(double *duration, bool useFallback);
		int GetTotalFrames(int64_t *totalFrames, bool useFallback);
		int GetFramesPerSecond(double *fps, bool useFallback);
		int GetDimensions(int *width, int *height);
		int GetBitrate(int *bitrate);
        int GetCodec(const char **codec);

		AVFormatContext* FormatContextPointer();
		AVCodecContext* CodecContextPointer();
		AVStream* StreamPointer();
		int VideoStreamIndex();

    private:
		int GetAVFormatContext();
		int GetAVStream();
		int GetAVCodecContext();
		int GetAVCodec();

		void CloseFormatContext();
		void CloseCodecContext();
        
		const char* _inputfile;
		Log* pLog;
		AVFormatContext *pFormatCtx;
		AVCodecContext *pCodecCtx;
		AVStream *pStream;
		AVCodec *pCodec;
		int videoStreamIndex;

};
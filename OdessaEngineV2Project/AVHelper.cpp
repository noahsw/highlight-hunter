
//#ifndef TARGET_OS_MAC
#include "Stdafx.h"
#include "AVHelper.h"
#include "OdessaEngineV2Project.h"
//#endif

#include <stdarg.h>


// Include the ffmpeg libraries
#ifdef WIN32
#pragma comment(lib, "avformat.lib")
#pragma comment(lib, "avutil.lib")
#pragma comment(lib, "avcodec.lib")
#endif

 
AVHelper::AVHelper(const char* inputfile, Log *pLogReference)
{
	pLog = pLogReference;
	_inputfile = inputfile;

	int returnCode;

	returnCode = GetAVFormatContext();
	if (returnCode != Success)
	{
		throw returnCode;
	}

	returnCode = GetAVStream();
	if (returnCode != Success)
	{
		throw returnCode;
	}

	returnCode = GetAVCodecContext();
	if (returnCode != Success)
	{
		throw returnCode;
	}

	returnCode = GetAVCodec();
	if (returnCode != Success)
	{
		throw returnCode;
	}

	pLog->Write("AVHelper::AVHelper(): Initialized successfully");

}


AVHelper::~AVHelper()
{
	CloseCodecContext();
	CloseFormatContext();

	pLog->Write("AVHelper::~AVHelper(): Disposed successfully");
}


int AVHelper::GetAVFormatContext()
{

	int             av_errno;
	char            av_errbuf[256];
	
	
	// Register all formats and codecs
	av_register_all();
	pLog->Write("AVHelper::GetAVFormatContext(): All formats and codecs registered");
	
	// Open video file
	pFormatCtx = avformat_alloc_context();
	av_errno = avformat_open_input(&pFormatCtx, _inputfile, NULL, NULL);

	pLog->Write("AVHelper::GetAVFormatContext(): Result of avformat_open_input: %d", av_errno);
	if (av_errno < 0)
	{
		av_strerror(av_errno, av_errbuf, 256);
		pLog->Write("AVHelper::GetAVFormatContext(): Couldn't open file! Error: %d %s", av_errno, av_errbuf);
		return ErrorOpeningFile;
	}
	pLog->Write("AVHelper::GetAVFormatContext(): File opened successfully");
	
	
	// Retrieve stream information
//#ifdef TARGET_OS_MAC
//	av_errno = avformat_find_stream_info(pFormatCtx, NULL); // recommended we use avformat_find_stream_info instead
//#endif
//#ifdef WIN32
	av_errno = av_find_stream_info(pFormatCtx);
//#endif
	if (av_errno < 0)
	{
		av_strerror(av_errno, av_errbuf, 256);
		pLog->Write("AVHelper::GetAVFormatContext(): Couldn't find stream information! Error: %d %s", av_errno, av_errbuf);
		return ErrorFindingStreamInformation;
	}
	pLog->Write("AVHelper::GetAVFormatContext(): Stream info retrieved successfully");

	return Success;

}



int AVHelper::GetAVStream()
{

	// Find the first video stream
	videoStreamIndex = -1;
	for(unsigned int i=0; i<pFormatCtx->nb_streams; i++)
	{
		if(pFormatCtx->streams[i]->codec->codec_type==AVMEDIA_TYPE_VIDEO)
		{
			videoStreamIndex = i;
			break;
		}
	}
	if(videoStreamIndex == -1)
	{
		pLog->Write("AVHelper::GetAVStream(): Couldn't find a video stream");
		return ErrorFindingVideoStream;
	}

	pStream = pFormatCtx->streams[videoStreamIndex];

	return Success;
}


int AVHelper::GetAVCodecContext()
{

	// Get a pointer to the codec context for the video stream
	pCodecCtx = pStream->codec;

	return Success;

}


int AVHelper::GetAVCodec()
{
	int             av_errno;
	char            av_errbuf[256];

	// Find the decoder for the video stream
	pCodec=avcodec_find_decoder(pCodecCtx->codec_id);
	if(pCodec==NULL)
	{
		pLog->Write("AVHelper::GetAVCodec(): Unsupported codec!");
		pLog->Write(pCodecCtx->codec_name);
		return UnsupportedCodec;
	}

	// Inform the codec that we can handle truncated bitstreams -- i.e.,
	// bitstreams where frame boundaries can fall in the middle of packets
	if(pCodec->capabilities & CODEC_CAP_TRUNCATED)
		pCodecCtx->flags|=CODEC_FLAG_TRUNCATED;

	// Open codec
//#ifdef TARGET_OS_MAC
//	av_errno = avcodec_open2(pCodecCtx, pCodec, NULL);
//#endif
//#ifdef WIN32
	av_errno = avcodec_open(pCodecCtx, pCodec); // recommended we use avcodec_open2 instead
//#endif
	if (av_errno<0)
	{
		av_strerror(av_errno, av_errbuf, 256);
		pLog->Write("AVHelper::GetAVCodec(): Couldn't open codec! Error: %d %s", av_errno, av_errbuf);
		return ErrorOpeningCodec;
	}

	pLog->Write("AVHelper::GetAVCodec(): Using codec: %s", pCodec->name);

	return Success;
}



int AVHelper::GetDuration(double *duration, bool useFallback)
{
	if (!pFormatCtx)
		return InvalidFormatContext;

	if (!pFormatCtx->duration)
		return ErrorCalculatingDuration;

	*duration = double(pFormatCtx->duration) / AV_TIME_BASE;

	if (*duration == 0)
	{
		if (useFallback)
		{ // try using fps and total frames
			double fps = 0.0;
			int64_t totalFrames = 0;
			int returnCode;
			returnCode = GetFramesPerSecond(&fps, false);
			if (returnCode == Success)
			{
				returnCode = GetTotalFrames(&totalFrames, false);
				if (returnCode == Success)
				{
					*duration = (double)totalFrames / fps;
				}
				else
					return ErrorCalculatingDuration;
			}
			else
				return ErrorCalculatingDuration;

		}
		else
			return ErrorCalculatingDuration;
	}

	pLog->Write("AVHelper::GetDuration(): Returning %f", *duration);
	return Success;
}


int AVHelper::GetCodec(const char **codec)
{
	if (pCodec == NULL)
		return ErrorDeterminingCodec;
		
	*codec = pCodec->name;
	
	pLog->Write("AVHelper::GetCodec(): Returning %s", *codec);
	return Success;
}


int AVHelper::GetBitrate(int *bitrate)
{
	// get bitrate
	*bitrate = pFormatCtx->bit_rate;

	if (*bitrate == 0)
		return ErrorCalculatingBitrate;

	pLog->Write("AVHelper::GetBitrate(): Returning %d", *bitrate);
	return Success;
}


int AVHelper::GetDimensions(int *width, int *height)
{
	*width = pCodecCtx->width;
	*height = pCodecCtx->height;

	pLog->Write("AVHelper::GetDimensions(): videoWidth = %d", *width);
	pLog->Write("AVHelper::GetDimensions(): videoHeight = %d", *height);

	if (*width <= 0 || *height <= 0)
		return ErrorCalculatingDimensions;

	return Success;
	
}


int AVHelper::GetTotalFrames(int64_t *totalFrames, bool useFallback)
{
	*totalFrames = pStream->nb_frames;

	if (*totalFrames == 0)
	{
		// see if we can use duration and fps instead
		if (useFallback)
		{
			double fps = 0.0;
			double duration = 0.0;
			int returnCode;

			returnCode = GetFramesPerSecond(&fps, false);
			if (returnCode == Success)
			{
				returnCode = GetDuration(&duration, false);
				if (returnCode == Success)
				{
					*totalFrames = (int64_t)(duration * fps);
				}
				else
					return ErrorCalculatingTotalFrames;
			}
			else
				return ErrorCalculatingTotalFrames;
		}
		else
			return ErrorCalculatingTotalFrames;
	}

	pLog->Write("AVHelper::GetTotalFrames(): returning %d", *totalFrames);

	return Success;
}


int AVHelper::GetFramesPerSecond(double *fps, bool useFallback)
{
	
	int returnCode;

	// Calculate frames per second
	double framesPerSecond = 0.0;
	if (pStream->avg_frame_rate.den > 0)
	{
		framesPerSecond = 
			(double)pStream->avg_frame_rate.num / 
			(double)pStream->avg_frame_rate.den;
	}
	if (framesPerSecond == 0)
	{
		// use r_frame_rate instead
		if (pStream->r_frame_rate.den > 0)
		{
			framesPerSecond =
				(double)pStream->r_frame_rate.num /
				(double)pStream->r_frame_rate.den;
		}

		if (framesPerSecond == 0)
		{
			// get duration
			double duration = 0;
			returnCode = GetDuration(&duration, false);
			if (returnCode == Success && duration > 0)
			{
				int64_t totalFrames = 0;
				returnCode = GetTotalFrames(&totalFrames, false);
				if (returnCode == Success)
				{
					framesPerSecond = totalFrames / duration;
				}
				else
					return returnCode;
			}
			else
			{
				return returnCode;
			}

		}
	}

	*fps = framesPerSecond;

	pLog->Write("AVHelper::GetFramesPerSecond(): returning %f", *fps);

	return Success;
}



AVFormatContext* AVHelper::FormatContextPointer()
{
	return pFormatCtx;
}


AVCodecContext* AVHelper::CodecContextPointer()
{
	return pCodecCtx;
}

AVStream* AVHelper::StreamPointer()
{
	return pStream;
}

int AVHelper::VideoStreamIndex()
{
	return videoStreamIndex;
}


void AVHelper::CloseFormatContext()
{
	// we don't have to call this because av_close_input_file takes care of it
	//avformat_free_context(pFormatCtx);
	
	// Close the video file
	av_close_input_file(pFormatCtx);

	
	pLog->Write("Scan(): Closed av handles successfully");
}


void AVHelper::CloseCodecContext()
{
	// Close the codec
	avcodec_close(pCodecCtx);
}

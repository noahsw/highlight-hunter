// This is the main DLL file.

//using namespace std;

//#include <tchar.h>
//#include <wchar.h>

#include "stdafx.h"

//#include "Log.h"
#include "OdessaEngineV2Project.h"
#include "Authorization.h"
#include "AVHelper.h"

// Include the ffmpeg libraries
#ifdef WIN32
#pragma comment(lib, "avformat.lib")
#pragma comment(lib, "avutil.lib")
#pragma comment(lib, "avcodec.lib")

//#pragma comment(lib, "avdevice.lib")
//#pragma comment(lib, "swscale.lib")
//#pragma comment(lib, "avfilter.lib")
#endif

#ifdef DEBUG
//#include <UnitTest++.h>
#endif


// Whether engine is currently scanning. Used because we currently can't scan more than one file at once.
bool isScanning = false;

// Keeps track of the total frames in the video
// Used to calculate progress
int64_t totalFrames = 0;

// Keeps track of the current frame being scanned
// Used to calculate progress
int64_t frameLocation = 0;

bool isAuthorized = false;

// Keeps track of whether the scan has been asked to cancel
bool isCancelled = false;

// A pointer to our logging class
Log *pLog;


enum OdessaDetectionThresholds
{
	Strictest = 0,
	Stricter = 1,
	Medium = 2,
	Looser = 3,
	Loosest = 4
};



/// <summary>
/// A value of 0 would catch absolute black pixels.
/// </summary> 
int ThresholdIndividualPixelBrightness[sizeof(OdessaDetectionThresholds) + 1] = { 50, 65, 70, 80, 100 }; // stricter to looser

int CurrentThresholdIndividualPixelBrightness = ThresholdIndividualPixelBrightness[2]; // set to medium


/// <summary>
/// This is the percentage of pixels per frame that must be "dark" for the entire frame to be dark
/// "dark" is defined by ThresholdIndividualPixelBrightness above
/// stricter to looser
/// </summary> 
int ThresholdDarkPixelsPerFrameAsPercentage[sizeof(OdessaDetectionThresholds) + 1] = { 95, 80, 80, 75, 50 }; // 90% of pixels must be dark. any less and we count it as a light frame

int CurrentThresholdDarkPixelsPerFrameAsPercentage = ThresholdDarkPixelsPerFrameAsPercentage[2];


// This is the % of pixels to scan per frame
int ThresholdPixelScanPercentage[sizeof(OdessaDetectionThresholds) + 1] = { 3, 3, 3, 3, 3 }; // 100% down to 5%

int CurrentThresholdPixelScanPercentage = ThresholdPixelScanPercentage[2]; // set to medium


// This is how often we should scan a frame of video
// This doesn't have a predictable affect on strict vs. loose. Instead, we should make sure a certain number of consecutive dark frames
// exist to constitute a bookmark
// When we set this to an odd number of frames (0.40 seconds => scan every 23 frames at 60fps) we had lots of frames that couldn't be decoded
float ThresholdSecondsSkip[sizeof(OdessaDetectionThresholds) + 1] = { 0.35f, 0.35f, 0.35f, 0.35f, 0.35f };

float CurrentThresholdSecondsSkip = ThresholdSecondsSkip[2]; // set to medium


/// <summary>
/// This is how many dark frames in a row there need to be to count
/// I believe we should fix this and not have it adjustable by the user because I think there will be less variability between users
/// </summary>
float ThresholdConsecutiveDarkFramesInSeconds[sizeof(OdessaDetectionThresholds) + 1] = { 1.20f, 0.30f, 0.0f, 0.0f, 0.0f }; // stricter to loose

float CurrentThresholdConsecutiveDarkFramesInSeconds = ThresholdConsecutiveDarkFramesInSeconds[2];


// Set the detection threshold to one of the 5 choices
void SetDetectionThreshold(int detectionThreshold)
{
	pLog->Write("Setting detection threshold to %d", detectionThreshold);
	CurrentThresholdIndividualPixelBrightness = ThresholdIndividualPixelBrightness[detectionThreshold];
	CurrentThresholdDarkPixelsPerFrameAsPercentage = ThresholdDarkPixelsPerFrameAsPercentage[detectionThreshold];
	CurrentThresholdPixelScanPercentage = ThresholdPixelScanPercentage[detectionThreshold];
	CurrentThresholdSecondsSkip = ThresholdSecondsSkip[detectionThreshold];
	CurrentThresholdConsecutiveDarkFramesInSeconds = ThresholdConsecutiveDarkFramesInSeconds[detectionThreshold];
}


// Set the detection thresholds to custom values (used by the tuning scanner)
#ifndef RELEASE
void SetCustomDetectionThreshold(
	int thresholdIndividualPixelBrightness, 
	int thresholdDarkPixelsPerFrameAsPercentage, 
	int thresholdPixelScanPercentage,
	float thresholdSecondsSkip,
	float thresholdConsecutiveDarkFramesInSeconds
	)
{

	pLog->Write("Setting CurrentThresholdIndividualPixelBrightness to %d", thresholdIndividualPixelBrightness);
	CurrentThresholdIndividualPixelBrightness = thresholdIndividualPixelBrightness;

	pLog->Write("Setting CurrentThresholdDarkPixelsPerFrameAsPercentage to %d", thresholdDarkPixelsPerFrameAsPercentage);
	CurrentThresholdDarkPixelsPerFrameAsPercentage = thresholdDarkPixelsPerFrameAsPercentage;

	pLog->Write("Setting CurrentPixelScanPercentage to %d", thresholdPixelScanPercentage);
	CurrentThresholdPixelScanPercentage = thresholdPixelScanPercentage;

	pLog->Write("Setting CurrentThresholdSecondsSkip to %f", thresholdSecondsSkip);
	CurrentThresholdSecondsSkip = thresholdSecondsSkip;
	
	pLog->Write("Setting ConsecutiveDarkFramesInSeconds to %d", thresholdConsecutiveDarkFramesInSeconds);
	CurrentThresholdConsecutiveDarkFramesInSeconds = thresholdConsecutiveDarkFramesInSeconds;

}
#endif





#ifdef DEBUG
/*
 int main(int argc, char *argv[])  // used for debugging
{
	//UnitTest::RunAllTests();
	
	Scan("D:\\Projects\\Odessa\\Sample Files\\Contour sample.MOV", "D:\\Projects\\Odessa\\Sample Files\\output.txt");


	int i = 0;
	cin >> i;
 

}
*/
/*
TEST(FailSpectacularly)
{
	CHECK(false);
}
*/
#endif


long long* Scan(const char* inputfile, int *listSize, int *returnCode) // const char* outputfile, 
{

	pLog->Write("Scan(): Scanning %s", inputfile);

	*listSize = 0;

	AVHelper		*pHelper;
	AVFrame         *pFrameYUV;
	int             numBytes;
	uint8_t         *buffer;
	int				width;
	int				height;
	
	AVPacket		packet;
	int isFrameFinished = 0;
	
	vector<int64_t> darkFrameLocations;
	
	/*
#ifdef DEBUG
	SimplifiedScan(inputfile);
	*returnCode = Success;
	return (long long*)malloc(darkFrameLocations.size() * sizeof(long long));
#endif
	*/


	pLog->Write("Scan(): isScanning = %d", isScanning);
	if (isScanning == true)
	{
		// already scanning!
		*returnCode = AlreadyScanning;
		return NULL;
	}

	pLog->Write("Scan(): isAuthorized = %d", isAuthorized);
	if (isAuthorized == false)
	{
		*returnCode = NotAuthorized;
		return NULL;
	}
	
	
	
//	totalFrames = 0; // we should use the value from SetTotalFrames
	frameLocation = 0;
	isScanning = true;
	isCancelled = false;

	
	clock_t start = clock();

#if DEBUG
	//av_log_set_level(AV_LOG_DEBUG); // this slows things down quite a bit
#endif


	try
	{
		pHelper = new AVHelper(inputfile, pLog);
	}
	catch (int errorCode)
	{
		*returnCode = errorCode;
		pLog->Write("Scan(): exception creating AVHelper: %d", errorCode);
		return NULL;
	}


	// calculate dimensions
	*returnCode = pHelper->GetDimensions(&width, &height);
	if (*returnCode != Success)
	{
		isScanning = false;
		pLog->Write("Scan(): unable to calculate dimensions");
		return NULL;
	}


	// Allocate an AVFrame structure
	pFrameYUV=avcodec_alloc_frame();
	if(pFrameYUV==NULL)
	{
		isScanning = false;
		pLog->Write("Scan(): Could not allocate frame structure");
		*returnCode = ErrorAllocatedFrameStructure;
		return NULL;
	}
	pLog->Write("Scan(): Allocated frame structure successfully");

	// Determine required buffer size and allocate buffer
	// originally:  	PIX_FMT_YUV420P
	numBytes=avpicture_get_size(PIX_FMT_YUV420P, width, height);
	buffer=(uint8_t *)av_malloc(numBytes*sizeof(uint8_t));
	//buffer=new uint8_t[numBytes];

	// Assign appropriate parts of buffer to image planes in pFrameYUV
	// originally:  PIX_FMT_RGB24	PIX_FMT_YUV420P
	avpicture_fill((AVPicture *)pFrameYUV, buffer, PIX_FMT_YUV420P, width, height);
	pLog->Write("Scan(): Allocated frame buffer successfully");
	

	
	// Calculate fps
	double fps = 0.0;
	*returnCode = pHelper->GetFramesPerSecond(&fps, true);
	if (*returnCode != Success)
	{
		isScanning = false;
		pLog->Write("Scan(): unable to calculate framesPerSecond");
		return NULL;
	}
	

#ifndef RELEASE
	pLog->Write("Scan(): CurrentThresholdIndividualPixelBrightness = %d", CurrentThresholdIndividualPixelBrightness);
	pLog->Write("Scan(): CurrentThresholdDarkPixelsPerFrameAsPercentage = %d", CurrentThresholdDarkPixelsPerFrameAsPercentage);
	pLog->Write("Scan(): CurrentThresholdSecondsSkip = %f", CurrentThresholdSecondsSkip);
	pLog->Write("Scan(): CurrentPixelScanPercentage = %d", CurrentThresholdPixelScanPercentage);
#endif
	
	// Verify threshold values
	if (CurrentThresholdPixelScanPercentage <= 0 || CurrentThresholdPixelScanPercentage > 100 ||
		CurrentThresholdIndividualPixelBrightness <= 0 || CurrentThresholdIndividualPixelBrightness > 256 ||
		CurrentThresholdDarkPixelsPerFrameAsPercentage <= 0 || CurrentThresholdDarkPixelsPerFrameAsPercentage > 100 ||
		CurrentThresholdSecondsSkip <= 0.001) // our tolerance
	{
		isScanning = false;
		pLog->Write("Scan(): unable to verify threshold values");
		*returnCode = InvalidThresholds;
		return NULL;
	}


	// calculate how many pixels across to skip
	int pixelSkipWidth = 100 / CurrentThresholdPixelScanPercentage;
	int pixelSkipHeight = 100 / CurrentThresholdPixelScanPercentage;
#ifndef RELEASE
	pLog->Write("Scan(): pixelSkipWidth = %d", pixelSkipWidth);
	pLog->Write("Scan(): pixelSkipHeight = %d", pixelSkipHeight);
#endif

	// Calculate number of allowed bright pixels
	int MaxNumberOfBrightPixels = (100 - CurrentThresholdDarkPixelsPerFrameAsPercentage) * 
		width / pixelSkipWidth * height / pixelSkipHeight / 100;
#ifndef RELEASE
	pLog->Write("Scan(): totalPixels = %d", (width * height));
	int pixelsToBeScanned = width * height * CurrentThresholdPixelScanPercentage / 100; // OLD: (pCodecCtx->width / pixelSkipWidth + 1) * (pCodecCtx->height / pixelSkipHeight + 1);
	pLog->Write("Scan(): pixelsToBeScanned = %d", pixelsToBeScanned);
	pLog->Write("Scan(): MaxNumberOfBrightPixels = %d", MaxNumberOfBrightPixels);
#endif

	// calculate margins (so we don't scan the edge of the video)
	int leftMargin = (width % pixelSkipWidth) / 2;
	int topMargin = (height % pixelSkipHeight) / 2;
#ifndef RELEASE
	pLog->Write("Scan(): leftMargin = %d", leftMargin);
	pLog->Write("Scan(): topMargin = %d", topMargin);
#endif


		
	int frameSkip = (int)(fps * CurrentThresholdSecondsSkip);
	if (frameSkip == 0)
		frameSkip = 1;

	pLog->Write("Scan(): Scanning every %d frames (once every %f seconds)", frameSkip, CurrentThresholdSecondsSkip);

	pLog->Write("Scan(): Scanning...");

	AVFormatContext* pFormatCtx;
	pFormatCtx = pHelper->FormatContextPointer();

	int videoStreamIndex = pHelper->VideoStreamIndex();
	int64_t frameScanCount = 0;

	AVCodecContext* pCodecCtx;
	pCodecCtx = pHelper->CodecContextPointer();
	
	bool forceFrameScan = false; // if we land on a frame that we can't scan, we should try scanning the next one instead of skipping frameSkip

	//TEMP
	//frameSkip = 1;

	while (av_read_frame(pFormatCtx, &packet) >= 0) 
	{

		// Is this a packet from the video stream?
		if (packet.stream_index == videoStreamIndex)
		{
			if ( (frameLocation % frameSkip == 0) || forceFrameScan)
			{
			
				if (isCancelled)
				{ // user asked to cancel the scan. let's break out of the loop.
					av_free_packet(&packet);
					break;
				}
			
			
				// Decode video frame
				avcodec_decode_video2(pCodecCtx, pFrameYUV, &isFrameFinished, &packet);

	#if DEBUG
				//WriteJPEG(pCodecCtx, pFrameYUV, frameLocation);
	#endif

				// Did we get a video frame?
				if (isFrameFinished) {
				
					forceFrameScan = false;

#if DEBUG
					//pLog->Write("Frame finished: %d", frameLocation);
#endif

					frameScanCount += 1; // used to know whether we actually were able to scan any frames
	
					int numberOfBrightPixels = 0;

					for (int y = topMargin; y < height; y += pixelSkipHeight)
					{
						uint8_t *p = pFrameYUV->data[0] + y * pFrameYUV->linesize[0]; // this gets Y' in YUV

						for (int x = leftMargin; x < width; x += pixelSkipWidth)
						{

							if (p[x] > CurrentThresholdIndividualPixelBrightness)
							{
								numberOfBrightPixels++;
							}

	#if DEBUG
							//pLog->Write("Frame %d at %d, %d has Y'=%d\n", frameLocation, x, y, p[x]);
	#endif

						}

						// see if we already have too many bright pixels
						if (numberOfBrightPixels >= MaxNumberOfBrightPixels)
						{
							break;
						}
					
					}

					// done scanning frame
	#if DEBUG
					//pLog->Write("Frame %d", frameLocation); // split because our ->Write function can't handle multiple arguments yet
					//pLog->Write(" (bright pixels: %d)", numberOfBrightPixels);
	#endif

					// see if this one is dark enough
					if (numberOfBrightPixels < MaxNumberOfBrightPixels)
					{
	#if DEBUG
						//pLog->Write("Dark frame %d ", frameLocation); // split because our ->Write function can't handle multiple arguments yet
						//pLog->Write(" (bright pixels: %d)", numberOfBrightPixels);
						double timeStamp = frameLocation / fps;
						pLog->Write("Dark frame at %f", timeStamp);
	#endif

						darkFrameLocations.push_back(frameLocation);
					}
	#if DEBUG
					else
					{
						//pLog->Write("Frame not dark: %d", frameLocation);
					}
	#endif

				} // whether we successfully decoded the frame
				else
				{
					forceFrameScan = true; // we couldn't decode this frame so try reading the next one (don't skip frameSkip)

					//frameLocation--; // don't advance frame location if we couldn't decode this frame

	#if DEBUG
					pLog->Write("Frame *NOT* finished: %d", frameLocation);
	#endif
				}


				/*
				double pts;
				if(packet.dts != AV_NOPTS_VALUE) {
				  pts = packet.dts;
				} else {
				  pts = 0;
				}
				pts *= av_q2d(pHelper->StreamPointer()->time_base);

				pLog->Write("DTS: %d", packet.dts);
				pLog->Write("PTS: %d", packet.pts);
				pLog->Write("Time: %f", pts);
				*/


			} // whether we were supposed to decode the frame

			frameLocation++;

		} // whether the packet was a video frame
		
		av_free_packet(&packet);

	} // for each available frame

	pLog->Write("Total frames scanned: %d", frameScanCount);

	//delete [] buffer;
	av_free(buffer);

	// Free the YUV frame
	av_free(pFrameYUV);

	
	// prepare returned array
#ifndef RELEASE
	pLog->Write("Scan(): Before collapsing:");
	for (unsigned int i = 0; i < darkFrameLocations.size(); i++)
	{
		pLog->Write("%lld", darkFrameLocations[i]);
	}
#endif



	// Collapse dark frames that are next to one another
	vector<int64_t> collapsedDarkFrameLocations = 
		CollapseDarkFrameLocations(darkFrameLocations, fps, frameSkip);
	
	

	// prepare returned array
	long long* returnList = (long long*)malloc(collapsedDarkFrameLocations.size() * sizeof(long long));
#ifndef RELEASE
	pLog->Write("Scan(): Result of collapsing:");
#endif
	for (unsigned int i = 0; i < collapsedDarkFrameLocations.size(); i++)
	{
		returnList[i] = collapsedDarkFrameLocations[i];

#ifndef RELEASE
		pLog->Write("%lld", collapsedDarkFrameLocations[i]);
#endif

//		fprintf(pFile, "%lld\n", collapsedDarkFrameLocations[i]); // write to file that will be read by caller
	}
	*listSize = (int)collapsedDarkFrameLocations.size();
	


	double elapsed = ((double)clock() - start) / CLOCKS_PER_SEC;
	pLog->Write("Scan(): Time elapsed: %f secs", elapsed);

	// print scan multiplier. this is just for shits and giggles so we shouldn't bomb the whole scan just because we couldn't calculate duration
	double duration = 0.0;
	if (pHelper->GetDuration(&duration, true) == Success)
	{
		pLog->Write("Scan(): Video duration: %f secs", duration);
		if (elapsed > 0)
			pLog->Write("Scan(): Scan multiplier: %fx", duration / elapsed);
	}

	// uncomment this if you want a pause before command line window closes
	//cin >> i;
	
	delete pHelper;
	
	isScanning = false;

	if (frameScanCount > 0)
	{
		int64_t minimumFramesNeededToScan = totalFrames / frameSkip / 10; // if we can't scan 10% of what we think we should be scanning, we probably didn't really scan this video
		if (frameScanCount < minimumFramesNeededToScan)
		{
			pLog->Write("Not enough frames found! Required: %d", minimumFramesNeededToScan);
			*returnCode = NotEnoughFramesFound;
		}
		else
			*returnCode = Success;
	}
	else
	{
		pLog->Write("Scan(): No frames were found to scan!");
		*returnCode = NoFramesFound;
	}

	pLog->Write("Scan(): Done!");

	return returnList;

}



#ifndef RELEASE

int WriteJPEG (AVCodecContext *pCodecCtx, AVFrame *pFrame, int FrameNo)
{ 
         AVCodecContext         *pOCodecCtx; 
         AVCodec                *pOCodec; 
         uint8_t                *Buffer; 
         int                     BufSiz; 
         int                     BufSizActual; 
         PixelFormat			ImgFmt = PIX_FMT_YUVJ420P; //for the newer ffmpeg version, this int to pixelformat 
         FILE                   *JPEGFile; 
         char                    JPEGFName[256]; 

         BufSiz = avpicture_get_size (ImgFmt,pCodecCtx->width,pCodecCtx->height ); 

         Buffer = (uint8_t *)malloc ( BufSiz ); 
         if ( Buffer == NULL ) 
                 return ( 0 ); 
         memset ( Buffer, 0, BufSiz ); 

         pOCodecCtx = avcodec_alloc_context ( ); 
         if ( !pOCodecCtx ) { 
                  free ( Buffer ); 
                  return ( 0 ); 
                  } 

         pOCodecCtx->bit_rate      = pCodecCtx->bit_rate; 
         pOCodecCtx->width         = pCodecCtx->width; 
         pOCodecCtx->height        = pCodecCtx->height; 
         pOCodecCtx->pix_fmt       = ImgFmt; 
         pOCodecCtx->codec_id      = CODEC_ID_MJPEG; 
         pOCodecCtx->codec_type    = AVMEDIA_TYPE_VIDEO; 
         pOCodecCtx->time_base.num = pCodecCtx->time_base.num; 
         pOCodecCtx->time_base.den = pCodecCtx->time_base.den; 

         pOCodec = avcodec_find_encoder ( pOCodecCtx->codec_id ); 
         if ( !pOCodec ) { 
                  free ( Buffer ); 
                 return ( 0 ); 
                  } 
         if ( avcodec_open ( pOCodecCtx, pOCodec ) < 0 ) { 
                  free ( Buffer ); 
                  return ( 0 ); 
                 } 

                pOCodecCtx->mb_lmin        = pOCodecCtx->lmin = 
pOCodecCtx->qmin * FF_QP2LAMBDA; 
         pOCodecCtx->mb_lmax        = pOCodecCtx->lmax = 
pOCodecCtx->qmax * FF_QP2LAMBDA; 
         pOCodecCtx->flags          = CODEC_FLAG_QSCALE; 
         pOCodecCtx->global_quality = pOCodecCtx->qmin * FF_QP2LAMBDA; 

         pFrame->pts     = 1; 
         pFrame->quality = pOCodecCtx->global_quality; 
         BufSizActual = avcodec_encode_video( 
pOCodecCtx,Buffer,BufSiz,pFrame ); 

         sprintf( JPEGFName, "frame-%06d.jpg", FrameNo ); 
         JPEGFile = fopen(JPEGFName, "wb" ); 
         fwrite ( Buffer, 1, BufSizActual, JPEGFile ); 
         fclose ( JPEGFile ); 

         avcodec_close ( pOCodecCtx ); 
         free ( Buffer ); 
         return ( BufSizActual ); 
} 


int SimplifiedScan(const char* inputfile)
{

	AVFormatContext *pFormatCtx;
	AVStream		*pStream;
	AVFrame         *pFrameYUV;
	AVCodecContext  *pCodecCtx;
	AVCodec			*pCodec;
	AVPacket packet;

	int             numBytes;
	uint8_t         *buffer;
	int				width;
	int				height;
	int				av_errno;
	
	int isFrameFinished = 0;
	
	int frameLocation = 0;

	av_register_all();
	
	// Open video file
	pFormatCtx = avformat_alloc_context();
	av_errno = avformat_open_input(&pFormatCtx, inputfile, NULL, NULL);

	if (av_errno < 0)
	{
		return ErrorOpeningFile;
	}
	
	// Retrieve stream information
//#ifdef TARGET_OS_MAC
//	av_errno = avformat_find_stream_info(pFormatCtx, NULL); // recommended we use avformat_find_stream_info instead
//#endif
//#ifdef WIN32
	av_errno = av_find_stream_info(pFormatCtx);
//#endif
	if (av_errno < 0)
	{
		return ErrorFindingStreamInformation;
	}

	// Find the first video stream
	int videoStreamIndex = -1;
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


	// Get a pointer to the codec context for the video stream
	pCodecCtx = pStream->codec;

	// Find the decoder for the video stream
	pCodec=avcodec_find_decoder(pCodecCtx->codec_id);
	if(pCodec==NULL)
	{
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
	av_errno = avcodec_open(pCodecCtx, pCodec);
//#endif
	if (av_errno<0)
	{
		return ErrorOpeningCodec;
	}


	width = pCodecCtx->width;
	height = pCodecCtx->height;


	// Allocate an AVFrame structure
	pFrameYUV=avcodec_alloc_frame();
	if(pFrameYUV==NULL)
	{
		return NULL;
	}



	// Determine required buffer size and allocate buffer
	numBytes=avpicture_get_size(PIX_FMT_YUV420P, width, height);
	buffer=(uint8_t *)av_malloc(numBytes*sizeof(uint8_t));

	// Assign appropriate parts of buffer to image planes in pFrameYUV
	avpicture_fill((AVPicture *)pFrameYUV, buffer, PIX_FMT_YUV420P, width, height);
	
	int frameSkip = 10;
	
	while (av_read_frame(pFormatCtx, &packet) >= 0) 
	{

		// Is this a packet from the video stream?
		if (packet.stream_index == videoStreamIndex && frameLocation++ % frameSkip == 0)
		{
			
			// Decode video frame
			avcodec_decode_video2(pCodecCtx, pFrameYUV, &isFrameFinished, &packet);

#if DEBUG
			WriteJPEG(pCodecCtx, pFrameYUV, frameLocation);
#endif

			// Did we get a video frame?
			if (isFrameFinished) {
				
				// done scanning frame
#if DEBUG
				pLog->Write("Frame finished %d", frameLocation);
#endif
			}
			else
				pLog->Write("Frame *NOT* finished %d", frameLocation);


		} // whether we were supposed to decode the frame
		
		av_free_packet(&packet);

	} // for each available frame

	//delete [] buffer;
	av_free(buffer);

	// Free the YUV frame
	av_free(pFrameYUV);

	// Close the codec
	avcodec_close(pCodecCtx);

	// Close the video file
	av_close_input_file(pFormatCtx);

	return 0;

}

#endif // #ifndef RELEASE


int GetDuration(const char *inputfile, double *duration)
{
	pLog->Write("GetDuration(): Getting duration of %s", inputfile);
	
	AVHelper *pHelper;
	try
	{
		pHelper = new AVHelper(inputfile, pLog);
	}
	catch (int errorCode)
	{
		pLog->Write("GetDuration(): exception creating AVHelper: %d", errorCode);
		return errorCode;
	}

	int returnCode = pHelper->GetDuration(duration, true);

	delete pHelper;
	
	pLog->Write("GetDuration(): Returning %f", *duration);
	return returnCode;
}


int GetDimensions(const char *inputfile, int *width, int *height)
{
	pLog->Write("GetDimensions(): Getting dimensions of %s", inputfile);
	
	AVHelper *pHelper;
	try
	{
		pHelper = new AVHelper(inputfile, pLog);
	}
	catch (int errorCode)
	{
		pLog->Write("GetDimensions(): exception creating AVHelper: %d", errorCode);
		return errorCode;
	}

	int returnCode = pHelper->GetDimensions(width, height);

	delete pHelper;
	
	pLog->Write("GetDimensions(): Returning %d x %d", *width, *height);
	return returnCode;
}




// Returns bitrate in b/s because that's what ffmpeg takes as its argument
int GetBitrate(const char *inputfile, int *bitrate)
{
	pLog->Write("GetBitrate(): Getting bitrate of %s", inputfile);
	
	AVHelper *pHelper;
	try
	{
		pHelper = new AVHelper(inputfile, pLog);
	}
	catch (int errorCode)
	{
		pLog->Write("GetBitrate(): exception creating AVHelper: %d", errorCode);
		return errorCode;
	}

	int returnCode = pHelper->GetBitrate(bitrate);

	delete pHelper;
	
	pLog->Write("GetBitrate(): Returning %d", bitrate);
	return returnCode;
}




int GetFramesPerSecond(const char *inputfile, double *fps)
{
	pLog->Write("GetFramesPerSecond(): Getting frames per second of %s", inputfile);
	
	AVHelper *pHelper;
	try
	{
		pHelper = new AVHelper(inputfile, pLog);
	}
	catch (int errorCode)
	{
		pLog->Write("GetFramesPerSecond(): exception creating AVHelper: %d", errorCode);
		return errorCode;
	}

	int returnCode = pHelper->GetFramesPerSecond(fps, true);

	delete pHelper;
	
	pLog->Write("GetFramesPerSecond(): Returning %f", *fps);

	return returnCode;

}



int GetCodec(const char *inputfile, const char **codec)
{
	pLog->Write("GetCodec(): Getting codec of %s", inputfile);
	
	AVHelper *pHelper;
	try
	{
		pHelper = new AVHelper(inputfile, pLog);
	}
	catch (int errorCode)
	{
		pLog->Write("GetCodec(): exception creating AVHelper: %d", errorCode);
		return errorCode;
	}
	
	int returnCode = pHelper->GetCodec(codec);
	
	delete pHelper;
	
	pLog->Write("GetCodec(): Returning %s", *codec);
	
	return returnCode;
}


int GetTotalFrames(const char *inputfile, int64_t *totalFramesReturned)
{
	pLog->Write("GetTotalFrames(): Getting total frames of %s", inputfile);
	
	AVHelper *pHelper;
	try
	{
		pHelper = new AVHelper(inputfile, pLog);
	}
	catch (int errorCode)
	{
		pLog->Write("GetTotalFrames(): exception creating AVHelper: %d", errorCode);
		return errorCode;
	}
	
	// Get number of frames
	int returnCode = pHelper->GetTotalFrames(totalFramesReturned, true);
	if (returnCode == Success)
	{
		totalFrames = *totalFramesReturned;
	}

	delete pHelper;

	pLog->Write("GetTotalFrames(): Returning %lld", *totalFramesReturned);

	return returnCode;
}


int Initialize(const char *logpath)
{
	InitLogger(logpath);
	
	pLog->Write("Initialize(): Logger initialized with %s", logpath);

	Authorization *auth = new Authorization(pLog);
	isAuthorized = auth->IsAuthorized();
	auth = NULL;
	
	
	pLog->Write("avformat config: %s", avformat_configuration());
	pLog->Write("avformat version: %d.%d.%d", LIBAVFORMAT_VERSION_MAJOR, LIBAVFORMAT_VERSION_MINOR, LIBAVFORMAT_VERSION_MICRO);
	pLog->Write("avutil config: %s", avutil_configuration());
	pLog->Write("avutil version: %d.%d.%d", LIBAVUTIL_VERSION_MAJOR, LIBAVUTIL_VERSION_MINOR, LIBAVUTIL_VERSION_MICRO);
	pLog->Write("avcodec config: %s", avcodec_configuration());
	pLog->Write("avcodec version: %d.%d.%d", LIBAVCODEC_VERSION_MAJOR, LIBAVCODEC_VERSION_MINOR, LIBAVCODEC_VERSION_MICRO);
	


	pLog->Write("Initialize(): isAuthorized = %d", isAuthorized);
	
	if (isAuthorized)
		return Authorized;
	else
		return NotAuthorized;

}

void CancelScan()
{
	isCancelled = true;
	
	pLog->Write("CancelScan(): scan asked to be cancelled");
}

void InitLogger(const char *logpath)
{
	pLog = new Log(logpath);
	
	pLog->Write("InitLogger(): Logger initialized");
}

void Dispose()
{
	pLog->Write("Dispose(): Shutting down OdessaEngine");
	pLog->~Log();
	
}



bool GetPercentDone(int *percent)
{
	if (isScanning)
	{
		if (totalFrames > 0)
		{
			*percent = (int)(frameLocation * 100 / totalFrames);
			return true;
		}
		else
		{
			*percent = 0;
			return false;
		}
	}
	else
	{
		*percent = 0;
		return false;
	}

}


long long GetFramesProcessed()
{
	//pLog->Write("GetFramesProcessed(): returning %ld", frameLocation);
	return frameLocation;
}


void FreePointer(long long* p)
{
	delete p;
}




vector<int64_t> CollapseDarkFrameLocations(const vector<int64_t> input, double framesPerSecond, int frameSkip)
{

	vector<int64_t> workingInput = input;
	vector<int64_t> output; // = input;

	int consecutiveFramesRequired = (int)(CurrentThresholdConsecutiveDarkFramesInSeconds * framesPerSecond);

	pLog->Write("consecutiveFramesRequired: %d", consecutiveFramesRequired);

	/*
	This makes sure there are enough dark frames in a row.
	It doesn't account for when there's a quick gap between frames. That might have unintented consequences but let's wait for the tuning results before we conclude that.
	*/
	if (consecutiveFramesRequired == 0)
	{
		output = workingInput;
	}
	else
	{
		while (workingInput.size() > 0)
		{
			int64_t targetFrame = workingInput[0] + consecutiveFramesRequired;
			unsigned int i = 1;
			bool success = false;
			while (workingInput.size() > i)
			{
				int64_t expected = workingInput[0] + (frameSkip * i);
				if (workingInput[i] != expected)
					break;
				
				if (expected > targetFrame)
				{
					// success. keep going though in case there are more consecutive frames
					success = true;
				}

				i++;
			}

			if (success)
				output.push_back(workingInput[0]);

		
			//delete everything up to index (i-1)
			for (unsigned int j = 0; j < i; j++)
			{
				workingInput.erase(workingInput.begin());
			}
		}
	}
	
	
	// Collapse highlights that are within [frameSkip] seconds of each other
	int highlightFrameGap = frameSkip;


	// We assume no one holds their hand in front of camera more than once every 5 seconds
	// Therefore black frames can't be that close together
	const int captureSpacingInSeconds = 5; // 5 seconds
	int captureSpacingInFrames = (int)(captureSpacingInSeconds * framesPerSecond);

	// highlightFrameGap should be the maximum of [frameSkip] and [captureSpacingInFrames]
	if (captureSpacingInFrames > highlightFrameGap)
		highlightFrameGap = captureSpacingInFrames;

	unsigned long i = output.size() - 1;
	while (i > 0 && output.size() > 1)
	{
		if (output[i] - output[i - 1] <= highlightFrameGap )
		{
			output.erase(output.begin() + i);
			//i--;
		}
		i--;
	}
	

	return output;
}

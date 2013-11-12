//
//  SaveWorker.m
//  OdessaMacGUIApp
//
//  Created by Noah Spitzer-Williams on 9/4/12.
//  Copyright (c) 2012 Authentically Digital LLC. All rights reserved.
//

#import "SaveWorker.h"

#if DEBUG
static const int ddLogLevel = LOG_LEVEL_VERBOSE | LOG_LEVEL_INFO | LOG_LEVEL_ERROR | LOG_LEVEL_WARN;
#else
static const int ddLogLevel = LOG_LEVEL_INFO | LOG_LEVEL_ERROR | LOG_LEVEL_WARN;
#endif


@interface SaveWorker ()

@property BOOL forceWatermark;
@property (strong) NSString* ffmpegArguments;
@property (strong) NSMutableString* ffmpegOutput;

@end




@implementation SaveWorker

+ (SaveWorker*)workerWithHighlight:(HighlightObject*) highlightObject
{
    SaveWorker* newWorker = [[super alloc] init];
    if (newWorker)
    {
        newWorker.highlightObject = highlightObject;
        newWorker.outputFormat = OutputFormatOriginal;
        newWorker.ffmpegOutput = [NSMutableString new];
        
        unsigned long long framesToSave = highlightObject.duration / highlightObject.inputFileObject.videoDuration * highlightObject.inputFileObject.totalFrames;
        newWorker.totalProgressUnits = framesToSave;
    }
    return newWorker;
}

- (void)main
{
    DDLogInfo(@"called with outputFormat=%ld", self.outputFormat);
    
    if (self.outputFormat != OutputFormatFacebook)
        DDLogInfo(@"started saving %@", self.highlightObject.title);
    
    if (self.outputFormat != OutputFormatFacebook)
    { // don't track if facebook sharer is using this
#ifdef APPSTORE_FREE
        [AnalyticsHelper fireEvent:@"Each save - activation state - unlicensed" eventValue:0];
#endif
        
#ifdef APPSTORE_PREMIUM
        [AnalyticsHelper fireEvent:@"Each save - activation state - activated" eventValue:0];
#endif
        
        [self trackFirstSave];
        
    }
    
    
    NSURL* outputFileURL = [self spliceVideo:self.outputFileURL isReencoding:NO];
    
    if (self.publishWorkerResult == PublishWorkerResultSuccess)
        self.outputFileURL = outputFileURL;
    else if (self.publishWorkerResult != PublishWorkerResultCancelled)
        [self saveFailed];
    
    if (self.outputFormat != OutputFormatFacebook)
        [AnalyticsHelper fireEvent:[NSString stringWithFormat:@"Each save - result - %@", [self getFriendlyPublishWorkerResult]] eventValue:0];
    
    [super reportProgress:100];
}


- (NSURL*)spliceVideo: (NSURL*)outputFileURL isReencoding:(bool)isReencoding;
{
    
    if (self.outputFormat != OutputFormatFacebook)
        DDLogInfo(@"Called with isReencoding=%d", isReencoding);
    else
        DDLogInfo(@"Called with outputFileURL=%@ and isReencoding=%d", [outputFileURL path], isReencoding);
    
    
    if (self.highlightObject.inputFileObject.framesPerSecond < 1)
    {
        DDLogError(@"Frames per second equals zero!");
        self.errorMessage = @"Could not retrieve frame rate of video.";
        self.publishWorkerResult = PublishWorkerResultUnableToSplice;
        return nil;
    }
    
    if (self.highlightObject.inputFileObject.bitrate == 0)
    {
        DDLogError(@"Bitrate equals zero!");
        self.errorMessage = @"Could not retrieve bitrate of video.";
        self.publishWorkerResult = PublishWorkerResultUnableToSplice;
        return nil;
    }
    
    
    if (self.outputFormat == OutputFormatProRes)
    {
        return [self spliceAsProRes:outputFileURL];
    }
    
    
    
    
    // verify license status
    bool useWatermarks = false;
    
#ifdef APPSTORE_FREE
    useWatermarks = true;
#endif
    
#ifndef APPSTORE
    // BEGINNING OF getLicenseStatus
    
    char* associatedProductKey = [Protection getAssociatedProductKey: [Protection getActivationCode]];
    
    enum ActivationStates activationState;
    
    int err;
    switch (arc4random() % 5)
    {
        case 1:
            err = DNA_Validate(associatedProductKey);
            break;
        case 2:
            err = DNA_Validate2(associatedProductKey);
            break;
        case 3:
            err = DNA_Validate3(associatedProductKey);
            break;
        case 4:
            err = DNA_Validate4(associatedProductKey);
            break;
        default:
            err = DNA_Validate5(associatedProductKey);
            break;
    }
    
    DDLogInfo(@"Response from DNA_Validate: %d (%s)", err, associatedProductKey);
    
    if (err == ERR_VALIDATION_WARNING)
    { // warn user they must connect to Internet
        // this shouldn't happen because we aren't using anti-fraud
    }
    
    
    if (err == ERR_CDM_HAS_EXPIRED)
    {
        activationState = TrialExpired;
    }
    else if (err != ERR_NO_ERROR)
    {
        activationState = Unlicensed;
    }
    else
    {
        char msg[256];
        err = DNA_Param("ACTIVATION_CODE", msg, sizeof(msg));
        if (strlen(msg) == 0 || err != ERR_NO_ERROR)
            activationState = Unlicensed;
        else
        {
            err = DNA_Param("EVAL_CODE", msg, sizeof(msg));
            if (err != ERR_NO_ERROR)
                activationState = Unlicensed;
            else
            {
                if (strcmp(msg, "1") == 0)
                { // it's an trial code. let's make sure it hasn't expired
                    err = DNA_Param("EXPIRY_DATE", msg, sizeof(msg));
                    
                    NSDate* expiryDate = [Protection getNSDateFromSoftworkzDate:msg];
                    
                    if (expiryDate && err == ERR_NO_ERROR)
                    {
                        if ([expiryDate timeIntervalSinceNow] > 0)
                            activationState = Trial;
                        else
                            activationState = TrialExpired;
                    }
                    else
                    { // invalid date
                        activationState = Unlicensed;
                    }
                    
                }
                else
                { // it's an activation code
                    activationState = Activated;
                }
            }
            
        }
        
    }
    
    // END OF getLicenseStatus
    
    
    if (activationState == Unlicensed || activationState == TrialExpired)
        useWatermarks = true;
#endif
    
    
    if (!useWatermarks)
    {
        DDLogInfo(@"Splicing from %@ to %@", self.highlightObject.friendlyBeginTime, self.highlightObject.friendlyEndTime);
    }
    
    if (self.outputFormat != OutputFormatFacebook)
        DDLogInfo(@"Writing to %@", outputFileURL.path);
    
    
    
    
    
    
    
    NSURL* ffmpegPath = [[NSBundle mainBundle] URLForResource:@"ffmpeg" withExtension:nil];
    
    
    
    // setup ffmpeg arguments
    NSMutableArray *arguments = [NSMutableArray arrayWithObjects:@"-ss", [NSString stringWithFormat:@"%f", self.highlightObject.beginTime], @"-t", [NSString stringWithFormat:@"%f", self.highlightObject.duration], @"-y", @"-i", [self.highlightObject.inputFileObject.sourceURL path], @"-acodec", @"copy", @"-copyts", @"-copytb", @"1", nil];
    
    [super reportProgress:1];
    
    if (useWatermarks && self.outputFormat != OutputFormatFacebook)
    {
        // save watermark to disk
        NSInteger videoWidth = self.highlightObject.inputFileObject.videoWidth;
        NSInteger videoHeight = self.highlightObject.inputFileObject.videoHeight;
        
        DDLogInfo(@"size of video is %ld x %ld", videoWidth, videoHeight);
        
        NSURL* watermarkPath = nil;
        if ([self getPathToWatermark:videoWidth height:videoHeight url:&watermarkPath] == false)
        {
            DDLogError(@"exiting due to watermarkPath=nil");
            self.publishWorkerResult = PublishWorkerResultUnableToSplice;
            return nil;
        }

        
        NSString* watermarkArg = [NSString stringWithFormat:@"movie=%@ [watermark]; [in][watermark]overlay=0:0 [out]", [watermarkPath lastPathComponent]];
        
        [arguments addObjectsFromArray:@[@"-vf", watermarkArg]];
       
    }
    
    
    [super reportProgress:2];
    
    // Figure out specific bitrate and framerate flags
    
    BOOL needsSpecificBitrateAndFPS = YES;
    
    double frameRate = self.highlightObject.inputFileObject.framesPerSecond;
    
    switch (self.outputFormat)
    {
        case OutputFormatOriginal:
        {
            if (!useWatermarks && !isReencoding)
            {
                needsSpecificBitrateAndFPS = NO;
                [arguments addObjectsFromArray:@[@"-vcodec", @"copy"]];
            }
            else
            {
                needsSpecificBitrateAndFPS = YES;
                // don't need to specify vcodec because ffmpeg will use extension of file
            }
            break;
        }
            
        case OutputFormatFacebook:
        {
            needsSpecificBitrateAndFPS = YES;
            // According to Facebook, they want H.264
            [arguments addObjectsFromArray:@[@"-vcodec", @"mpeg4"]];
            
            if (frameRate > 30)
                frameRate = 30;
            
            NSInteger videoWidth = self.highlightObject.inputFileObject.videoWidth;
            NSInteger videoHeight = self.highlightObject.inputFileObject.videoHeight;
            BOOL isResized = NO;
            
            if (videoWidth > 1280)
            {
                videoHeight = videoHeight * 1280.0 / videoWidth;
                videoWidth = 1280;
                isResized = YES;
            }
            if (videoHeight > 1280)
            {
                videoWidth = videoWidth * 1280.0 / videoHeight;
                videoHeight = 1280;
                isResized = YES;
            }
            
            // make sure height and width are always divisible by 2
            if (videoWidth % 2 > 0)
                videoWidth -= 1;
            if (videoHeight % 2 > 0)
                videoHeight -= 1;
            
            if (isResized)
                [arguments addObjectsFromArray:@[@"-s", [NSString stringWithFormat:@"%ldx%ld", videoWidth, videoHeight]]];
            break;
        }
            
        case OutputFormatProRes:
        {
            needsSpecificBitrateAndFPS = NO;
            [arguments addObjectsFromArray:@[@"-vcodec", @"prores"]];
        }
    }
    
    if (needsSpecificBitrateAndFPS)
    {
        NSNumberFormatter* formatter = [NSNumberFormatter new];
        [formatter setMaximumFractionDigits:2];
        [formatter setDecimalSeparator:@"."];
        [formatter setHasThousandSeparators:NO];
        NSString* fpsArg = [formatter stringFromNumber:@(frameRate)];
        
        NSString* bitrateArg = [[NSNumber numberWithDouble:self.highlightObject.inputFileObject.bitrate] stringValue];
        
        [arguments addObjectsFromArray:@[@"-r", fpsArg, @"-b:v", bitrateArg]];
    }
    
    
    // Verify validity of output path
    if ([outputFileURL.lastPathComponent.pathExtension isEqualToString:@""])
    {
        NSAssert(false, @"When do we hit this?");
        outputFileURL = [outputFileURL URLByAppendingPathExtension:self.highlightObject.inputFileObject.sourceURL.lastPathComponent.pathExtension];
    }
    
    if (self.outputFormat == OutputFormatProRes)
    {
        if ([outputFileURL.lastPathComponent.pathExtension.uppercaseString isEqualToString:@"MOV"] == NO)
        {
            NSAssert(false, @"When do we hit this?");
            outputFileURL = [NSURL fileURLWithPath:[NSString stringWithFormat:@"%@.%@", outputFileURL.lastPathComponent.stringByDeletingPathExtension, outputFileURL.lastPathComponent.pathExtension]];
        }
    }
        
    
        
    [arguments addObjectsFromArray:@[outputFileURL.path]];
        
    
    [super reportProgress:3];
    
    // start ffmpeg
    NSTask *task = [NSTask new];
    [task setLaunchPath:[ffmpegPath path]];
    NSString* curDir = [[MainModel getParentFolder:ffmpegPath] path];
    [task setCurrentDirectoryPath:curDir];
    [task setArguments:arguments];
    self.ffmpegArguments = [NSString stringWithFormat:@"%@", arguments];
    
    NSPipe *pipe = [NSPipe pipe];
    [task setStandardError:pipe];
    
    DDLogInfo(@"running %@ with arguments %@", ffmpegPath, arguments);
    
    @try
    {
        [task launch];
        
        if (task)
        {
            
            NSDate *loopUntil = [NSDate dateWithTimeIntervalSinceNow:0.5];
            while ([task isRunning] && [[NSRunLoop currentRunLoop] runMode: NSDefaultRunLoopMode beforeDate:loopUntil])
            {
                loopUntil = [NSDate dateWithTimeIntervalSinceNow:0.5];
                if ([super checkForCancelledWorker])
                {
                    DDLogInfo(@"Cancelled while splicing");
                    [task terminate];
                    return nil;
                }
                
                NSData *data = [[pipe fileHandleForReading] availableData];
                
                NSString *string = [[NSString alloc] initWithData:data encoding:NSUTF8StringEncoding];
                
                [self parseFfmpegOutput:string];
                
            }
                        
            // no longer needed because we wait for task to end above
            //[task waitUntilExit];
            
            // REMED because this blocked the task
            //NSData *data = [[pipe fileHandleForReading] readDataToEndOfFile];
            
            //NSString *string = [[[NSString alloc] initWithData:data encoding:NSUTF8StringEncoding] autorelease];
            //DDLogInfo(@"Ffmpeg results:\n%@", string);
            
            
        }
    }
    @catch (NSException *exception)
    {
        DDLogError(@"Exception %@ while launching ffmpeg: %@", [exception name], [exception reason]);
        
        self.publishWorkerResult = PublishWorkerResultUnableToSplice;
        
    }
    
    
    
    // make sure outputfile looks good
    NSFileManager *fileManager = [NSFileManager defaultManager];
    
    if ([fileManager fileExistsAtPath:outputFileURL.path])
    {
        // make sure it's a valid video
        if ([self isValidVideo:outputFileURL])
        {
            self.publishWorkerResult = PublishWorkerResultSuccess;
            return outputFileURL;
        }
        else
        {
            if (!isReencoding)
            {
                DDLogInfo(@"Output file doesn't seem valid. Let's try reencoding.");
                
                NSURL* outputReencodedURL = [self spliceVideo:outputFileURL isReencoding:YES];
                if (![self isValidVideo:outputReencodedURL])
                {
                    DDLogError(@"Even re-encoding didn't work. Something else is wrong!");
                    self.publishWorkerResult = PublishWorkerResultUnableToSplice;
                    self.errorMessage = @"Error while saving video.";
                    return nil;
                }
                else
                {
                    // PublishWorkerResult would be set by second SpliceVideo call
                    return outputReencodedURL;
                }
            }
            else
            {
                DDLogInfo(@"Reencoding failed as well.");
                self.publishWorkerResult = PublishWorkerResultUnableToSplice;
                self.errorMessage = @"Error while saving video.";
                return nil;
            }
        }
    }
    else
    {
        DDLogError(@"Output file doesn't exist");
        self.publishWorkerResult = PublishWorkerResultUnableToSplice;
        self.errorMessage = @"Error while saving video.";
    }

    return nil;
    
}


- (void)parseFfmpegOutput: (NSString*)output
{
    
    if ([output isEqualToString:@""])
        return;
    
    DDLogInfo(@"Ffmpeg results:\n%@", output);
    
    [self.ffmpegOutput appendString:output];
    
    // frame=   19 fps=0.0 q=4.4 size=     430kB time=00:00:00.50 bitrate=7041.0kbits/s
    
    NSRange fpsIndex = [output rangeOfString:@" fps="];
    if (fpsIndex.location != NSNotFound)
    {
        NSString* beforeFps = [output substringToIndex:fpsIndex.location];
        NSRange frameIndex = [beforeFps rangeOfString:@" " options:NSBackwardsSearch];
        if (frameIndex.location == NSNotFound)
            frameIndex = [beforeFps rangeOfString:@"=" options:NSBackwardsSearch];
        if (frameIndex.location != NSNotFound)
        {
            NSString* frameAsString = [beforeFps substringFromIndex:frameIndex.location + 1];
            NSInteger processedFrames = [frameAsString integerValue];
            if (processedFrames > 0)
            {
                NSInteger newProgress = 100.0 * processedFrames / self.totalProgressUnits;
                if (newProgress > 3)
                    [super reportProgress:newProgress];
            }
        }
    }
    
}



- (bool) isValidVideo : (NSURL*) url
{
    
    NSFileManager* fileManager = [NSFileManager defaultManager];
    NSDictionary *attrs = [fileManager attributesOfItemAtPath:[url path] error: NULL];
    unsigned long long result = [attrs fileSize];
    
    // TODO: use ScanWorker here for non Pro-Res videos
    
    if (result > 1024 * 1024)
        return true;
    else
        return false;
    
}
- (bool) getPathToWatermark: (NSInteger)videoWidth height:(NSInteger)videoHeight url:(NSURL**) url
{
    
    if ( labs(videoWidth - 1920) < 30 && labs(videoHeight - 1080) < 30)
    {
        videoWidth = 1920;
        videoHeight = 1080;
    }
    else if ( labs(videoWidth - 1280) < 30 && labs(videoHeight - 720) < 30)
    {
        videoWidth = 1280;
        videoHeight = 720;
    }
    else if ( labs(videoWidth - 1280) < 30 && labs(videoHeight - 960) < 30 )
    {
        videoWidth = 1280;
        videoHeight = 960;
    }
    else if ( labs(videoWidth - 848) < 30 && labs(videoHeight - 640) < 30 )
    {
        videoWidth = 848;
        videoHeight = 640;
    }
    else
    {
        videoWidth = 766;
        videoHeight = 427;
    }
    
    NSString* filename = [NSString stringWithFormat:@"watermark-%ldx%ld.png", videoWidth, videoHeight];
    DDLogVerbose(@"getPathToWatermark: Looking for %@", filename);
    //NSString* filename = [NSString stringWithString:@"848x480png"];
    
    NSURL* watermarkPath = [[NSBundle mainBundle] URLForImageResource:filename];
    
    bool isUsingDefaultWatermark = false;
    if (watermarkPath == nil)
    {
        isUsingDefaultWatermark = true;
        watermarkPath = [[NSBundle mainBundle] URLForImageResource:@"watermark-766x427.png"];
    }
    
    NSData *contents = [NSData dataWithContentsOfURL:watermarkPath];
    if (!contents)
    {
        DDLogVerbose(@"Couldn't get contents for %@", watermarkPath);
        return false;
    }
    
    NSString* hash = [CustomMD5 md5StringFromData:contents];
    
    DDLogVerbose(@"hash for %@ is %@", watermarkPath, hash);
    
    // we use absolute value because sometimes videos report a bigger size than they actuall are
    
    // verify the watermark image hasn't been tampered with
    if (videoWidth == 1920 && videoHeight == 1080 && [hash isNotEqualTo:@"44C9FD622779548CFB175B9156C42D45"])
        return false;
    if (videoWidth == 1280 && videoHeight == 720 && [hash isNotEqualTo:@"C05DE23D2C48D9CEDC1F1E270FF2A61F"])
        return false;
    if (videoWidth == 1280 && videoHeight == 960 && [hash isNotEqualTo:@"3184E22D6B0747BFA4878A40316DB0FB"])
        return false;
    if (videoWidth == 848 && videoHeight == 640 && [hash isNotEqualTo:@"0D7861FE7584CFDE51FBD75AAA10F5E9"])
        return false;
    if ( ((videoWidth == 766 && videoHeight == 427) || isUsingDefaultWatermark) && [hash isNotEqualTo:@"F70D015988C86D236B305307A0421A2C"])
        return false;
    
    
    
    *url = watermarkPath;
    
    return true;
}



- (NSURL*)spliceAsProRes:(NSURL*)outputFileURL
{
    // This only works on 10.7 so make sure the classes exist
    if (!NSClassFromString(@"AVAsset") || !NSClassFromString(@"AVAssetExportSession"))
    {
        self.errorMessage = @"Saving to ProRes is only available in OSX 10.7 and above.";
        self.publishWorkerResult = PublishWorkerResultUnableToSplice;
        return nil;
    }
    
    
    // this will fail if file already exists so lets delete it first
    NSFileManager* fileManager = [NSFileManager defaultManager];
    [fileManager removeItemAtURL:outputFileURL error:nil];
    
    AVAsset *anAsset = [AVAsset assetWithURL:self.highlightObject.inputFileObject.sourceURL];
    NSArray *compatiblePresets = [AVAssetExportSession exportPresetsCompatibleWithAsset:anAsset];
    if ([compatiblePresets containsObject:AVAssetExportPresetAppleProRes422LPCM]) {
        AVAssetExportSession *exportSession = [AVAssetExportSession exportSessionWithAsset:anAsset presetName:AVAssetExportPresetAppleProRes422LPCM];
        
        exportSession.outputURL = outputFileURL;
        exportSession.outputFileType = AVFileTypeQuickTimeMovie;
        
        CMTime start = CMTimeMakeWithSeconds(self.highlightObject.beginTime, 600);
        CMTime duration = CMTimeMakeWithSeconds(self.highlightObject.duration, 600);
        CMTimeRange range = CMTimeRangeMake(start, duration);
        exportSession.timeRange = range;
        
        [exportSession exportAsynchronouslyWithCompletionHandler:^{
            
            switch ([exportSession status]) {
                case AVAssetExportSessionStatusFailed:
                case AVAssetExportSessionStatusUnknown:
                    self.publishWorkerResult = PublishWorkerResultUnableToSplice;
                    self.errorMessage = [[exportSession error] localizedDescription];
                    DDLogError(@"Export failed: %@", self.errorMessage);
                    break;
                case AVAssetExportSessionStatusCancelled:
                    self.publishWorkerResult = PublishWorkerResultCancelled;
                    DDLogInfo(@"Export canceled");
                    break;
                case AVAssetExportSessionStatusCompleted:
                    self.publishWorkerResult = PublishWorkerResultSuccess;
                    DDLogInfo(@"Export successful");
                    break;
            }
            
            [super reportProgress:100];
        }];
        
        //self.exportProResTimer = [NSTimer scheduledTimerWithTimeInterval:0.2 target:self selector:@selector(updateProResProgress:) userInfo:exportSession repeats:YES];
        
        while (self.publishWorkerResult == PublishWorkerResultNotFinished)
        {
            
            NSDate *loopUntil = [NSDate dateWithTimeIntervalSinceNow:0.2];
            [[NSRunLoop currentRunLoop] runMode: NSDefaultRunLoopMode beforeDate:loopUntil];
        
            [super reportProgress:exportSession.progress * 100];
            
            if ([super checkForCancelledWorker])
            {
                DDLogInfo(@"Cancelled while splicing");
                [exportSession cancelExport];
                return nil;
            }
            
        }
        
        return outputFileURL;
    }
    else
    {
        self.publishWorkerResult = PublishWorkerResultUnableToSplice;
        self.errorMessage = @"ProRes export not available for this file.";
        return nil;
    }
    
}


- (void)updateProResProgress:(NSTimer*)exportProResTimer
{ // progress is 0 to 1
    AVAssetExportSession* exportSession = [exportProResTimer userInfo];
    
    [super reportProgress:exportSession.progress * 100];
}



- (void)viewResult
{
    NSFileManager *fileManager = [NSFileManager defaultManager];
        
    if (self.outputFileURL != nil && [fileManager fileExistsAtPath:self.outputFileURL.path])
    {
        
        [[NSWorkspace sharedWorkspace] openURL:self.outputFileURL];
        
    }
}


- (void)trackFirstSave
{
    NSUserDefaults* standardUserDefaults = [NSUserDefaults standardUserDefaults];
    if (standardUserDefaults)
    {
        if ([standardUserDefaults boolForKey:@"HasSavedVideo"] == NO)
        {
            if ([AnalyticsHelper fireEvent:@"First save" eventValue:0])
            {
                [standardUserDefaults setBool:YES forKey:@"HasSavedVideo"];
                [standardUserDefaults synchronize];
            }
        }
    }

}

- (void)saveFailed
{
    NSString* errorCode = [NSString stringWithFormat:@"Error Message: %@\n\nFFmpeg Arguments: %@\n\nFFmpeg Output: %@", self.errorMessage, self.ffmpegArguments, self.ffmpegOutput];
    MediaInfoWorker* mediaInfoWorker = [MediaInfoWorker workerWithURL:self.highlightObject.inputFileObject.sourceURL errorCode:errorCode];
    NSOperationQueue* operationQueue = [NSOperationQueue new];
    [operationQueue addOperation:mediaInfoWorker];
}

@end

//
//  ScanWorkerOperation.m
//  Mac Test App
//
//  Created by Noah Spitzer-Williams on 9/22/11.
//  Copyright 2011 Authentically Digital LLC. All rights reserved.
//

#import "ScanWorker.h"

#if DEBUG
static const int ddLogLevel = LOG_LEVEL_VERBOSE | LOG_LEVEL_INFO | LOG_LEVEL_ERROR | LOG_LEVEL_WARN;
#else
static const int ddLogLevel = LOG_LEVEL_INFO | LOG_LEVEL_ERROR | LOG_LEVEL_WARN;
#endif

@interface ScanWorker ()

@property u_int32_t workerId;

@end


@implementation ScanWorker

- (id)init
{
    self = [super init];
    if (self != nil)
    {
        [self setScanWorkerResult:ScanWorkerResultNotFinished];
        self.workerId = arc4random();
    }
    
    return self;
}

+ (ScanWorker *)workerWithInputFileObject:(InputFileObject *)inputFileObject
{
    ScanWorker* newWorker = [ScanWorker new];
    [newWorker setInputFileObject:inputFileObject];
    
    
    return newWorker;
}

- (void)main {
    
    if ([self checkForCancelledWorker]) { return; }
    
    DDLogInfo(@"%d: starting", self.workerId);
    

    self.scanStartDate = [NSDate date];
    
    
    // TOTAL FRAMES
    if ([self setTotalFrames] == NO)
    {
        DDLogError(@"ScanWorker[%d]: Total frames = 0 in %@", self.workerId, self.inputFileObject.sourceURL.path);
    }
    
    if ([self checkForCancelledWorker]) { return; }
    
    
    // FRAMES PER SECOND
    if ([self setFramesPerSecond] == NO || self.inputFileObject.framesPerSecond < 1)
    {
        DDLogError(@"ScanWorker[%d]: Frames per second = 0 in %@", self.workerId, self.inputFileObject.sourceURL.path);
        self.scanWorkerResult = ScanWorkerResultUnableToDetermineFramesPerSecond;
        [self scanFailed];
        return;
    }
    
    if ([self checkForCancelledWorker]) { return; }
    
    
    // DURATION
    if ([self setVideoDuration] == NO || self.inputFileObject.videoDuration < 1)
    {
        DDLogError(@"ScanWorker[%d]: Unable to get video duration in %@", self.workerId, self.inputFileObject.sourceURL.path);
        self.scanWorkerResult = ScanWorkerResultUnableToDetermineVideoDuration;
        [self scanFailed];
        return;
    }
    
    if ([self checkForCancelledWorker]) { return; }
    
    
    // BITRATE
    if ([self setBitrate] == NO || self.inputFileObject.bitrate == 0)
    {
        DDLogError(@"ScanWorker[%d]: Unable to determine bitrate in %@", self.workerId, self.inputFileObject.sourceURL.path);
        self.scanWorkerResult = ScanWorkerResultUnableToDetermineBitrate;
        [self scanFailed];
        return;
    }
    
    if ([self checkForCancelledWorker]) { return; }
    
    
    // DIMENSIONS
    if ([self setVideoDimensions] == NO || self.inputFileObject.videoWidth == 0 || self.inputFileObject.videoHeight == 0)
    {
        DDLogError(@"ScanWorker[%d]: Unable to determine dimensions of %@", self.workerId, self.inputFileObject.sourceURL.path);
        self.scanWorkerResult = ScanWorkerResultUnableToDetermineDimensions;
        [self scanFailed];
        return;
    }
    
    
    
    if ([self checkForCancelledWorker]) { return; }
    
    
    
    // SCAN
    int bookmarkCount = 0;
    long long* bookmarkList;
    int returnCode = 0;
    
    NSThread *timerThread = [[NSThread alloc] initWithTarget:self selector:@selector(startProgressTimerThread) object:nil];
    [timerThread start];
    
    bookmarkList = Scan(self.inputFileObject.sourceURL.path.UTF8String, &bookmarkCount, &returnCode);
    
    [self.scanProgressTimer invalidate];
    
    
    if (returnCode != Success)
    {
        self.odessaReturnCode = returnCode;
        DDLogError(@"Return code from engine: %d", returnCode);
        [AnalyticsHelper fireEvent:[NSString stringWithFormat:@"Each video - scan result - UnableToScan - %d", returnCode] eventValue:0];
        [AnalyticsHelper fireEvent:[NSString stringWithFormat:@"Each video - scan result - UnableToScan - %d - %@", returnCode, self.inputFileObject.sourceURL.lastPathComponent] eventValue:0];
        self.scanWorkerResult = ScanWorkerResultUnableToScan;
        FreePointer(bookmarkList);
        [self scanFailed];
        return;
    }

    self.totalHighlightsFound = [self createHighlightObjects:bookmarkList bookmarkCount:bookmarkCount];
    
        
    [AnalyticsHelper fireEvent:@"Each video - highlights found" eventValue:@(bookmarkCount)];
    
    if ([self checkForCancelledWorker]) { FreePointer(bookmarkList); return; }
    
    
    FreePointer(bookmarkList);
    
    if (self.scanWorkerResult == ScanWorkerResultNotFinished) // if no error, say we're successful!
        [self setScanWorkerResult:ScanWorkerResultSuccess];
    
    self.scanStopDate = [NSDate date];
    
    
    NSTimeInterval scanDuration = [self.scanStopDate timeIntervalSinceDate:self.scanStartDate];
    
    DDLogInfo(@"Result of scan is %i", returnCode);
    DDLogInfo(@"Scan time: %f", scanDuration);
    
    
    
    
    if (self.scanWorkerResult == ScanWorkerResultSuccess)
    {
        [self trackVideoDuration];
        [self trackScanDuration];
        [self trackScanMultiplier];
        [self trackTotalVideosScanned];
        [self trackTotalHoursScanned];
        [self trackFirstHighlightFound];
        
    }
    
    [self trackScanResult];
    
    
    DDLogInfo(@"ScanWorker done");
     
}

- (void)scanFailed
{
    self.totalHighlightsFound = 0;
    self.scanStopDate = [NSDate date];

    MediaInfoWorker* mediaInfoWorker = [MediaInfoWorker workerWithURL:self.inputFileObject.sourceURL odessaReturnCode:self.odessaReturnCode];
    NSOperationQueue* operationQueue = [NSOperationQueue new];
    [operationQueue addOperation:mediaInfoWorker];
}

- (void)trackScanResult
{
    [AnalyticsHelper fireEvent:[NSString stringWithFormat:@"Each video - scan result - %@", [ScanWorker getFriendlyScanResult: self.scanWorkerResult]] eventValue:0];
    
}

- (void)trackVideoDuration
{
    
    [AnalyticsHelper fireEvent:@"Each video - video duration in mins" eventValue:@(floor(self.inputFileObject.videoDuration / 60))];
    
}

- (void)trackScanDuration
{
 
    NSTimeInterval scanDuration = [self.scanStopDate timeIntervalSinceDate:self.scanStartDate];
    [AnalyticsHelper fireEvent:@"Each video - scan duration in mins" eventValue:@(floor(scanDuration / 60))];
    
}

- (void)trackScanMultiplier
{
    NSTimeInterval scanDuration = [self.scanStopDate timeIntervalSinceDate:self.scanStartDate];
    
    if (scanDuration > 0)
    {
        float scanMultiplier = self.inputFileObject.videoDuration / scanDuration;
        DDLogInfo(@"Scan multiplier: %f", scanMultiplier);
     
        [AnalyticsHelper fireEvent:@"Each video - scan multiplier" eventValue:@(floor(scanMultiplier))];
    
    }
    
}


- (void) trackTotalVideosScanned
{
    NSUserDefaults *standardUserDefaults = [NSUserDefaults standardUserDefaults];
	NSInteger total = 0;
	
	if (standardUserDefaults)
    {
        total = [standardUserDefaults integerForKey:@"TotalVideosScanned"];
        total += 1;
        [standardUserDefaults setInteger:total forKey:@"TotalVideosScanned"];
        [standardUserDefaults synchronize];
        
        [AnalyticsHelper fireEvent:[NSString stringWithFormat:@"Total videos scanned - %ld", total] eventValue:0];
        
        if ([standardUserDefaults boolForKey:@"HasRegisteredScan"] == NO)
        {
            if ([AnalyticsHelper fireEvent:@"First scan" eventValue:0])
            {
                [standardUserDefaults setBool:YES forKey:@"HasRegisteredScan"];
                [standardUserDefaults synchronize];
            }
        }
    }
    
}

- (void)trackTotalHoursScanned
{
    NSUserDefaults *standardUserDefaults = [NSUserDefaults standardUserDefaults];
	double minutesScanned = 0;
	
	if (standardUserDefaults)
    {
        NSNumber* oldHoursScanned = [NSNumber numberWithDouble:[standardUserDefaults doubleForKey:@"TotalMinutesScanned"] / 60];

        minutesScanned = [standardUserDefaults doubleForKey:@"TotalMinutesScanned"] + (self.inputFileObject.videoDuration / 60);
        [standardUserDefaults setDouble:minutesScanned forKey:@"TotalMinutesScanned"];
        [standardUserDefaults synchronize];
        
        NSNumber* newHoursScanned = [NSNumber numberWithDouble:minutesScanned / 60];
        
        if ([oldHoursScanned integerValue] != [newHoursScanned integerValue]) // because we want to wait until they hit the next tier and not double-count them
            [AnalyticsHelper fireEvent:[NSString stringWithFormat:@"Total hours scanned - %ld", [newHoursScanned integerValue]] eventValue:0];
    }

}

- (void)trackFirstHighlightFound
{
    NSUserDefaults *standardUserDefaults = [NSUserDefaults standardUserDefaults];
	if (standardUserDefaults)
    {
        BOOL isAlreadyFound = [standardUserDefaults boolForKey:@"HasFoundHighlight"];
        if (isAlreadyFound == NO)
        {
            if ([AnalyticsHelper fireEvent:@"First highlight found" eventValue:0])
            {
                [standardUserDefaults setBool:YES forKey:@"HasFoundHighlight"];
                [standardUserDefaults synchronize];
            }
        }
    }
}


- (void)startProgressTimerThread
{
    @autoreleasepool {
        
        NSRunLoop *runLoop = [NSRunLoop currentRunLoop];
        self.scanProgressTimer = [NSTimer scheduledTimerWithTimeInterval:0.2 target:self selector:@selector(scanProgressTimerFired:) userInfo:nil repeats:YES];
        [runLoop run];
            
    }
}

- (void)scanProgressTimerFired:(NSTimer*)timer
{
    self.processedFrames = GetFramesProcessed();
    //DDLogInfo(@"processed Frames: %ld", self.processedFrames);
}

- (int)createHighlightObjects:(long long*)bookmarkList bookmarkCount:(int)bookmarkCount
{
    
#ifdef DEBUG
    DDLogInfo(@"Bookmarks returned:");
    
    for (int i = 0; i < bookmarkCount; i++)
    {
        long long value = (long long)bookmarkList[i];
        DDLogInfo(@"Bookmark %d = frame %lld", i, value);
    }
#endif
    
    
    // IGNORE EARLY BOOKMARKS
    NSMutableArray* validBookmarkList = [NSMutableArray new];
    
    if (self.mainModel.ignoreEarlyHighlights)
    { // only add frames to validBookmarkList if it's after 10 second mark
        DDLogInfo(@"Ignoring early highlights");
        
        int frameNumberAtTenSeconds = self.inputFileObject.framesPerSecond * 10;
        
        for (int i = 0; i < bookmarkCount; i++)
        {
            long long value = (long long)bookmarkList[i];
            if (value > frameNumberAtTenSeconds)
            {
                [validBookmarkList addObject:@(value)];
            }
        }
        
        // update count
        bookmarkCount = (int)[validBookmarkList count];
        
    }
    else
    { // not ignoring early highlights
        DDLogInfo(@"NOT ignoring early highlights");
        for (int i = 0; i < bookmarkCount; i++)
        {
            [validBookmarkList addObject:@((long long)bookmarkList[i])];
        }
    }
    
#ifdef DEBUG
    DDLogInfo(@"Bookmarks after cleanup:");
    for (int i = 0; i < bookmarkCount; i++)
    {
        DDLogInfo(@"Bookmark %d = frame %@", i, validBookmarkList[i]);
    }
#endif

    // now lets create the objects
    for (int i = 0; i < bookmarkCount; i++)
    {
        double secondsBeforeBookmark = 2.0;
        
        NSNumber* frameNumber = validBookmarkList[i];
        
        NSTimeInterval bookmarkTimeInterval = [frameNumber doubleValue] / self.inputFileObject.framesPerSecond;
        
        NSTimeInterval endTimeInterval = bookmarkTimeInterval - secondsBeforeBookmark;
        if (endTimeInterval < 0)
            endTimeInterval = bookmarkTimeInterval;
        if (endTimeInterval < secondsBeforeBookmark)
            endTimeInterval = secondsBeforeBookmark; // so endTime is never 0
        
        NSTimeInterval beginTimeInterval = endTimeInterval - self.mainModel.captureDurationInSeconds;
        if (beginTimeInterval < 0)
            beginTimeInterval = 0;
        
        HighlightObject* highlightObject = [HighlightObject new];
        highlightObject.inputFileObject = self.inputFileObject;
        highlightObject.bookmarkTime = bookmarkTimeInterval;
        highlightObject.beginTime = beginTimeInterval;
        highlightObject.endTime = endTimeInterval;
        highlightObject.title = [self generateTitle];
        [highlightObject generateThumbnail:self.thumbnailGenerator];
        [self.mainModel.highlightObjects addObject:highlightObject];
        
#ifdef DEBUG
        DDLogInfo(@"Adding new highlight from %f to %f with bookmark = %f", beginTimeInterval, endTimeInterval, bookmarkTimeInterval);
#endif
    }
    
    
    return bookmarkCount;
}


- (NSString*)generateTitle
{
    NSString* title;
    NSInteger highlightCount = 1;
    for (HighlightObject* highlightObject in self.mainModel.highlightObjects)
    {
        if (highlightObject.inputFileObject == self.inputFileObject)
        {
            highlightObject.title = [NSString stringWithFormat:@"%@ highlight #%ld", self.inputFileObject.sourceURL.lastPathComponent.stringByDeletingPathExtension, highlightCount++];
        }
    }
    
    if (highlightCount > 1)
        title = [NSString stringWithFormat:@"%@ highlight #%ld", self.inputFileObject.sourceURL.lastPathComponent.stringByDeletingPathExtension, highlightCount];
    else
        title = [NSString stringWithFormat:@"%@ highlight", self.inputFileObject.sourceURL.lastPathComponent.stringByDeletingPathExtension];
    
    return title;
}



- (bool)checkForCancelledWorker
{
    if (self.isCancelled)
    {
        DDLogInfo(@"ScanWorker cancelled!");
        [self setScanWorkerResult:ScanWorkerResultCancelled];
        return true;
    }
    else
        return false;
}


- (void)cancelWorker
{
    [self cancel];
    
    // tell Odessa engine to cancel
    CancelScan();
    
}





+ (NSString*) getFriendlyScanResult: (enum ScanWorkerResults)result
{
    switch (result)
    {
        case ScanWorkerResultNotFinished:
            return @"Not finished"; break;
        case ScanWorkerResultUnableToDetermineVideoDuration:
            return @"Unable to determine video duration"; break;
        case ScanWorkerResultUnableToDetermineFramesPerSecond:
            return @"Unable to determine frame rate"; break;
        case ScanWorkerResultUnableToDetermineBitrate:
            return @"Unable to determine bitrate"; break;
        case ScanWorkerResultUnableToDetermineDimensions:
            return @"Unable to determine video dimensions"; break;
        case ScanWorkerResultUnableToScan:
            return @"Unable to scan"; break;
        case ScanWorkerResultUnableToSplice:
            return @"Unable to splice"; break;
        case ScanWorkerResultSuccess:
            return @"Success"; break;
        case ScanWorkerResultCancelled:
            return @"Cancelled"; break;
        default:
            assert(false);
            return @"Unknown";
            break;
    }
}


     
- (BOOL)setTotalFrames
{
    
    long long returnedFrames = 0;
    
    int returnCode = GetTotalFrames(self.inputFileObject.sourceURL.path.UTF8String, &returnedFrames);
    
    if (returnCode == Success)
    {
        DDLogInfo(@"ScanWorker[%d]: total frames: %lld", self.workerId, returnedFrames);
        self.inputFileObject.totalFrames = returnedFrames;
        return true;
    }

    self.odessaReturnCode = returnCode;
    DDLogError(@"ScanWorker[%d]: Error while getting total frames in %@. returnCode = %d", self.workerId, self.inputFileObject.sourceURL.path, returnCode);
    [AnalyticsHelper fireEvent:[NSString stringWithFormat:@"Each video - scan result - UnableToDetermineTotalFrames - %d", returnCode] eventValue:0];
    [AnalyticsHelper fireEvent:[NSString stringWithFormat:@"Each video - scan result - UnableToDetermineTotalFrames - %d - %@", returnCode, self.inputFileObject.sourceURL.lastPathComponent] eventValue:0];
    
    return false;

    
}

- (BOOL)setFramesPerSecond
{
    double framesPerSecond;
    
    int returnCode = GetFramesPerSecond(self.inputFileObject.sourceURL.path.UTF8String, &framesPerSecond);
    if (returnCode == Success)
    {
        DDLogInfo(@"ScanWorker[%d]: FPS: %f", self.workerId, framesPerSecond);
        self.inputFileObject.framesPerSecond = framesPerSecond;
        return true;
    }
    
    self.odessaReturnCode = returnCode;
    DDLogError(@"ScanWorker[%d]: Error while getting frames per second in %@. returnCode = %d", self.workerId, self.inputFileObject.sourceURL.path, returnCode);
    [AnalyticsHelper fireEvent:[NSString stringWithFormat:@"Each video - scan result - UnableToDetermineFramesPerSecond - %d", returnCode] eventValue:0];
    [AnalyticsHelper fireEvent:[NSString stringWithFormat:@"Each video - scan result - UnableToDetermineFramesPerSecond - %d - %@", returnCode, self.inputFileObject.sourceURL.lastPathComponent] eventValue:0];
    
    return false;

}

- (BOOL)setBitrate
{
    int bitrate;
    int returnCode = GetBitrate(self.inputFileObject.sourceURL.path.UTF8String, &bitrate);
    if (returnCode == Success)
    {
        DDLogInfo(@"ScanWorker[%d]: bitrate: %d", self.workerId, bitrate);
        self.inputFileObject.bitrate = bitrate;
        return true;
    }
    
    self.odessaReturnCode = returnCode;
    DDLogError(@"ScanWorker[%d]: Error while getting bitrate in %@. returnCode = %d", self.workerId, self.inputFileObject.sourceURL.path, returnCode);
    [AnalyticsHelper fireEvent:[NSString stringWithFormat:@"Each video - scan result - UnableToDetermineBitrate - %d", returnCode] eventValue:0];
    [AnalyticsHelper fireEvent:[NSString stringWithFormat:@"Each video - scan result - UnableToDetermineBitrate - %d - %@", returnCode, self.inputFileObject.sourceURL.lastPathComponent] eventValue:0];
    
    return false;
    
}

- (BOOL)setVideoDuration
{
    double duration;
    int returnCode = GetDuration(self.inputFileObject.sourceURL.path.UTF8String, &duration);
    if (returnCode == Success)
    {
        DDLogInfo(@"ScanWorker[%d]: video duration: %f", self.workerId, duration);
        self.inputFileObject.videoDuration = duration;
        return true;
    }
    
    self.odessaReturnCode = returnCode;
    DDLogError(@"ScanWorker[%d]: Error while getting duration of %@. returnCode = %d", self.workerId, self.inputFileObject.sourceURL.path, returnCode);
    [AnalyticsHelper fireEvent:[NSString stringWithFormat:@"Each video - scan result - UnableToDetermineVideoDuration - %d", returnCode] eventValue:0];
    [AnalyticsHelper fireEvent:[NSString stringWithFormat:@"Each video - scan result - UnableToDetermineVideoDuration - %d - %@", returnCode, self.inputFileObject.sourceURL.lastPathComponent] eventValue:0];
    
    return false;
}

- (BOOL)setVideoDimensions
{
    int height;
    int width;
    int returnCode = GetDimensions(self.inputFileObject.sourceURL.path.UTF8String, &width, &height);
    if (returnCode == Success)
    {
        DDLogInfo(@"ScanWorker[%d]: dimensions: %d x %d", self.workerId, width, height);
        self.inputFileObject.videoWidth = width;
        self.inputFileObject.videoHeight = height;
        return true;
    }
    
    self.odessaReturnCode = returnCode;
    DDLogError(@"ScanWorker[%d]: Error while getting dimensions of %@. returnCode = %d", self.workerId, self.inputFileObject.sourceURL.path, returnCode);
    [AnalyticsHelper fireEvent:[NSString stringWithFormat:@"Each video - scan result - UnableToDetermineDimensions - %d", returnCode] eventValue:0];
    [AnalyticsHelper fireEvent:[NSString stringWithFormat:@"Each video - scan result - UnableToDetermineDimensions - %d - %@", returnCode, self.inputFileObject.sourceURL.lastPathComponent] eventValue:0];
    
    return false;

}

@end

//
//  ScanWorkerHost.m
//  OdessaMacGUIApp
//
//  Created by Noah Spitzer-Williams on 8/29/12.
//  Copyright (c) 2012 Authentically Digital LLC. All rights reserved.
//

#import "ScanWorkerHost.h"

#if DEBUG
static const int ddLogLevel = LOG_LEVEL_VERBOSE | LOG_LEVEL_INFO | LOG_LEVEL_ERROR | LOG_LEVEL_WARN;
#else
static const int ddLogLevel = LOG_LEVEL_INFO | LOG_LEVEL_ERROR | LOG_LEVEL_WARN;
#endif


@interface ScanWorkerHost ()

@property unsigned long long inputFilesTotalSizeInKBytes;

@property unsigned long long inputFilesCompletedInKBytes;

@property (strong) NSDate* scanStartTime;

@property (strong) ThumbnailGenerator* thumbnailGenerator;

@property (unsafe_unretained) InputFileObject* activeInputFileObject; // weak because this is a loose reference to it

@property (unsafe_unretained) ScanWorker* activeScanWorker;

@property NSInteger completedScanWorkerCount;

@end


@implementation ScanWorkerHost


- (void)main
{
    DDLogInfo(@"called");
    
    [self.delegate scanWorkerHostStatusUpdated:@"Starting..."];
    
    self.inputFilesTotalSizeInKBytes = [self getInputFilesTotalSize];
    self.inputFilesCompletedInKBytes = 0;
    self.scanStartTime = [NSDate date];
    
    [self.mainModel.highlightObjects removeAllObjects];
    
    self.thumbnailGenerator = [ThumbnailGenerator new];
    
    NSOperationQueue* scanWorkerOperationQueue = [NSOperationQueue new];
    [scanWorkerOperationQueue setMaxConcurrentOperationCount:1];
    
    for (InputFileObject* inputFileObject in self.mainModel.inputFileObjects)
    {
        if (self.isCancelled)
            break;
        
        if (inputFileObject.isSmallDropHereControl)
            continue;
        
        NSString* status = [NSString stringWithFormat:@"Scanning %@...", inputFileObject.sourceURL.lastPathComponent.stringByDeletingPathExtension];
        [self.delegate scanWorkerHostStatusUpdated:status];
        
        ScanWorker* scanWorker = [ScanWorker workerWithInputFileObject:inputFileObject];
        [scanWorker setDelegate:self];
        [scanWorker setMainModel:self.mainModel];
        [scanWorker setThumbnailGenerator:self.thumbnailGenerator];
        
        self.activeInputFileObject = inputFileObject;
        self.activeScanWorker = scanWorker;
        
        [scanWorkerOperationQueue addOperations:@[scanWorker] waitUntilFinished:NO];
        
        while (!scanWorker.isFinished)
        {
            if (self.isCancelled)
                [scanWorker cancelWorker];
            
            [NSThread sleepForTimeInterval:0.5];
        }
        
        
        inputFileObject.scanWorkerResult = scanWorker.scanWorkerResult;
        
        self.completedScanWorkerCount++;
    }
    
    DDLogInfo(@"Highlights found: %ld", self.mainModel.highlightObjects.count);
    
    [self trackAnalytics];
    
    if (self.isCancelled == NO)
    {
        // in case we scan really fast, wait a few seconds so the user sees what happened
        NSTimeInterval minScanTime = 4; // this should be under 5 so we always show "Calculating time remaining..."
        while (-[self.scanStartTime timeIntervalSinceNow] < minScanTime)
            [NSThread sleepForTimeInterval:0.2];
        
        [self.delegate scanWorkerHostFinished];
    }
    else
        [self.delegate scanWorkerHostCancelled];
}


- (void)scanWorkerProgressUpdated:(NSInteger)progress
{
    // doesn't really matter since we use timer
}


- (BOOL)getProgressValue:(NSInteger*)progress
{
    if (self.activeInputFileObject == nil || self.inputFilesTotalSizeInKBytes == 0)
        return NO;
    
    self.inputFilesCompletedInKBytes = 0;
    for (InputFileObject* inputFileObject in self.mainModel.inputFileObjects)
    {
        if (inputFileObject.isSmallDropHereControl)
            continue;
        
        if (inputFileObject.scanWorkerResult == ScanWorkerResultNotFinished ||
            inputFileObject == self.activeInputFileObject)
            continue;
        
        self.inputFilesCompletedInKBytes += inputFileObject.sourceURL.fileSizeInKBytes;
    }
    
    unsigned long long scanWorkersCompletedInKBytes = 0;
    if (self.activeInputFileObject != nil)
    {
        if (self.activeInputFileObject.totalFrames > 0)
        {
  //          DDLogInfo(@"processed frames: %lld", self.activeScanWorker.processedFrames);
            scanWorkersCompletedInKBytes += (unsigned long long)(self.activeScanWorker.processedFrames / (double)self.activeInputFileObject.totalFrames * self.activeInputFileObject.sourceURL.fileSizeInKBytes);
//            DDLogInfo(@"fileSize: %lld", self.activeInputFileObject.sourceURL.fileSize);
            //DDLogVerbose(@"scanWorkersCompletedInKBytes: %lld", scanWorkersCompletedInKBytes);
        }
    }
    
    unsigned long long totalKBytesProcessed = self.inputFilesCompletedInKBytes + scanWorkersCompletedInKBytes;
    
    if (totalKBytesProcessed > 0)
    {
        *progress = (NSInteger)(totalKBytesProcessed * 100 / (double)self.inputFilesTotalSizeInKBytes);
        return YES;
    }
    *progress = 0;
    return NO;
}


- (NSString *)getTimeRemaining
{
    NSTimeInterval elapsedTime = -[self.scanStartTime timeIntervalSinceNow];
    
    if (elapsedTime < 5)
        return @"Calculating time remaining...";
    
    NSInteger currentProgressValue = 0;
    
    if ([self getProgressValue:&currentProgressValue] == false)
        return @"Calculating time remaining...";
    
    if (currentProgressValue > 0)
    {
        NSTimeInterval totalEstimatedTime = elapsedTime * 100 / currentProgressValue;
        
        NSTimeInterval timeRemaining = totalEstimatedTime - elapsedTime;
        
        NSInteger minutes = ((long)timeRemaining / 60) % 60;
        NSInteger hours = timeRemaining / 3600;
        
        if (hours > 0)
            return [NSString stringWithFormat:@"%ldh %ldm left", hours, minutes];
        else if (minutes > 0)
            return [NSString stringWithFormat:@"%ldm left", minutes];
        else
            return @"<1m left";
    }
    
    return @"Calculating time remaining...";
}


- (void)trackAnalytics
{
    
    NSTimeInterval totalScanDuration = -[self.scanStartTime timeIntervalSinceNow];
    if (totalScanDuration > 0)
    {
        NSTimeInterval totalVideoDuration = 0;
        
        NSURL* sampleVideoURL = [MainModel getURLToSampleVideo];
        BOOL isSampleVideoIncluded = NO;
        BOOL areRealVideosIncluded = NO;
        
        for (InputFileObject *inputFileObject in self.mainModel.inputFileObjects)
        {
            if (inputFileObject.scanWorkerResult != ScanWorkerResultSuccess)
                continue; // we only care about good scans
            
            if (inputFileObject.isSmallDropHereControl)
                continue;
            
            if ([inputFileObject.sourceURL.path isEqualToString:sampleVideoURL.path])
                isSampleVideoIncluded = YES;
            else
            {
                areRealVideosIncluded = YES;
            }
            
            totalVideoDuration += inputFileObject.videoDuration;
        }
        
        if (totalVideoDuration > 0 && areRealVideosIncluded)
        {
            [AnalyticsHelper fireEvent:@"Each scan - highlights found" eventValue:@(self.mainModel.highlightObjects.count)];
            
            NSNumber* scanMultiplier = [NSNumber numberWithDouble:totalVideoDuration / totalScanDuration];
            DDLogInfo(@"Average scan multiplier = %@", scanMultiplier);
            
            [AnalyticsHelper fireEvent:@"Each scan - scan multiplier" eventValue:scanMultiplier];
            
            [AnalyticsHelper fireEvent:@"Each scan - video duration in minutes" eventValue:@(floor(totalVideoDuration / 60))];
            
        }
        
        if (isSampleVideoIncluded)
        {
            NSUserDefaults* standardUserDefaults = [NSUserDefaults standardUserDefaults];
            if (standardUserDefaults)
            {
                if ([standardUserDefaults boolForKey:@"HasScannedSampleVideo"] == NO)
                {
                    if ([AnalyticsHelper fireEvent:@"First scan sample video" eventValue:0])
                    {
                        [standardUserDefaults setBool:YES forKey:@"HasScannedSampleVideo"];
                        [standardUserDefaults synchronize];
                    }
                }
            }
        }
        
    }
    

}


- (unsigned long long) getInputFilesTotalSize
{
    DDLogInfo(@"called");
    
    unsigned long long inputFilesSize = 0;
    for (InputFileObject* inputFileObject in self.mainModel.inputFileObjects)
    {
        if (inputFileObject.isSmallDropHereControl)
            continue;
        
        inputFilesSize += inputFileObject.sourceURL.fileSizeInKBytes;
    }
    
    return inputFilesSize;
}

@end

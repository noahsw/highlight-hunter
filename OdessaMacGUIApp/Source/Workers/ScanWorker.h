//
//  ScanWorkerOperation.h
//  Mac Test App
//
//  Created by Noah Spitzer-Williams on 9/22/11.
//  Copyright 2011 Authentically Digital LLC. All rights reserved.
//

#import <Foundation/Foundation.h>
#import "../CoreModels/HighlightObject.h"
#import "InputFileObject.h"

#import "ScanWorkerDelegate.h"

#import "MainModel.h"
#import "../Encryption/CustomMD5.h"

#import "AnalyticsHelper.h"

#import "OdessaEngineV2Project.h"

#import "MediaInfoWorker.h"

@class ThumbnailGenerator;

#import "DDLog.h"


@interface ScanWorker : NSOperation


enum ScanWorkerResults
{
    ScanWorkerResultNotFinished,
    ScanWorkerResultUnableToDetermineVideoDuration,
    ScanWorkerResultUnableToDetermineFramesPerSecond,
    ScanWorkerResultUnableToDetermineBitrate,
    ScanWorkerResultUnableToDetermineDimensions,
    ScanWorkerResultUnableToScan,
    ScanWorkerResultUnableToSplice,
    ScanWorkerResultSuccess,
    ScanWorkerResultCancelled
};


@property enum ScanWorkerResults scanWorkerResult;

@property (unsafe_unretained) InputFileObject* inputFileObject;

@property (unsafe_unretained) MainModel* mainModel;

@property (unsafe_unretained) ThumbnailGenerator* thumbnailGenerator;

@property long long processedFrames;

@property (strong) NSDate* scanStartDate;
@property (strong) NSDate* scanStopDate;

@property NSInteger odessaReturnCode;

@property NSInteger totalHighlightsFound;

@property (strong) NSTimer* scanProgressTimer;

@property (unsafe_unretained) id<ScanWorkerDelegate> delegate;




+ (ScanWorker*)workerWithInputFileObject:(InputFileObject*)inputFileObject;

- (void) cancelWorker;

+ (NSString*) getFriendlyScanResult: (enum ScanWorkerResults)result;

- (BOOL)setTotalFrames;
- (BOOL)setFramesPerSecond;
- (BOOL)setBitrate;
- (BOOL)setVideoDuration;
- (BOOL)setVideoDimensions;

@end

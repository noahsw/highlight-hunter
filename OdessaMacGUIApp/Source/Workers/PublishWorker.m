//
//  PublishWorker.m
//  OdessaMacGUIApp
//
//  Created by Noah Spitzer-Williams on 9/4/12.
//  Copyright (c) 2012 Authentically Digital LLC. All rights reserved.
//

#import "PublishWorker.h"

#if DEBUG
static const int ddLogLevel = LOG_LEVEL_VERBOSE | LOG_LEVEL_INFO | LOG_LEVEL_ERROR | LOG_LEVEL_WARN;
#else
static const int ddLogLevel = LOG_LEVEL_INFO | LOG_LEVEL_ERROR | LOG_LEVEL_WARN;
#endif

@interface PublishWorker ()

@property BOOL askedToCancel;

@end


@implementation PublishWorker

- (void)viewResult
{
    // overwritten by subclasses
    NSAssert(false, @"This should never be called!");
}


- (bool)checkForCancelledWorker
{
    if (self.askedToCancel)
    {
        DDLogInfo(@"PublishWorker cancelled!");
        self.publishWorkerResult = PublishWorkerResultCancelled;
        return true;
    }
    else
        return false;
}


- (void)cancelWorker
{
    self.askedToCancel = true;
    
    
}

- (void)reportProgress:(NSInteger) newProgress
{
    if (newProgress == self.progress && newProgress != 100) // check for 100 because sometimes we use that to update UI
        return; // so we don't update UI if progress hasn't changed
    
    self.progress = newProgress;
    
    [self.workerProgressDelegateForDetails workerProgressUpdated:newProgress];
    [self.workerProgressDelegateForPublishStatus workerProgressUpdated:newProgress];
}

- (NSString*)getFriendlyPublishWorkerResult
{
    switch (self.publishWorkerResult)
    {
        case PublishWorkerResultCancelled: return @"Cancelled";
        case PublishWorkerResultNotFinished: return @"Not Finished";
        case PublishWorkerResultSuccess: return @"Success";
        case PublishWorkerResultUnableToAuthenticate: return @"Unable to Authenticate";
        case PublishWorkerResultUnableToShare: return @"Unable to Share";
        case PublishWorkerResultUnableToSplice: return @"Unable to Splice";
        default:
            NSAssert(false, @"Missing case!");
            return @"Undefined";
    }
}


@end

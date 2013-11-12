//
//  PublishWorker.h
//  OdessaMacGUIApp
//
//  Created by Noah Spitzer-Williams on 9/4/12.
//  Copyright (c) 2012 Authentically Digital LLC. All rights reserved.
//

#import <Foundation/Foundation.h>
#import "PublishWorkerProtocol.h"
#import "WorkerProgressDelegate.h"
#import "DDLog.h"
@class HighlightObject;

@interface PublishWorker : NSOperation <PublishWorkerProtocol>

enum PublishWorkerType
{
    PublishWorkerTypeNone, // used by PublishStatus to signify Save and Share button
    PublishWorkerTypeFacebook,
    PublishWorkerTypeSave
};

enum PublishWorkerResult
{
    PublishWorkerResultNotFinished,
    PublishWorkerResultUnableToSplice,
    PublishWorkerResultUnableToShare,
    PublishWorkerResultUnableToAuthenticate,
    PublishWorkerResultSuccess,
    PublishWorkerResultCancelled
};

@property enum PublishWorkerResult publishWorkerResult;

@property (strong) NSString* errorMessage;

@property (unsafe_unretained) HighlightObject* highlightObject;

@property NSInteger progress;

@property (strong) NSURL* outputFileURL;

@property unsigned long long totalProgressUnits;

@property (unsafe_unretained) id<WorkerProgressDelegate> workerProgressDelegateForDetails;
@property (unsafe_unretained) id<WorkerProgressDelegate> workerProgressDelegateForPublishStatus;

- (bool)checkForCancelledWorker;
- (void)cancelWorker;
- (void)reportProgress:(NSInteger) newProgress;
- (NSString*)getFriendlyPublishWorkerResult;

@end

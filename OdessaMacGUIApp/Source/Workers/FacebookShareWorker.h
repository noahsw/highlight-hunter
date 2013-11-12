//
//  FacebookShareWorker.h
//  OdessaMacGUIApp
//
//  Created by Noah Spitzer-Williams on 9/4/12.
//  Copyright (c) 2012 Authentically Digital LLC. All rights reserved.
//

#import "PublishWorker.h"
#import "DDLog.h"
#import "AnalyticsHelper.h"
#import <PhFacebook/PhFacebook.h>
#import "SaveWorker.h"
#import "WorkerProgressDelegate.h"
#import "BrowserHelper.h"
#import "NSURL+Helpers.h"
#import "../SBJson/Classes/SBJson.h"
#import <ASIHTTPRequest/ASIFormDataRequest.h>
#import "NSAlert+SynchronousSheet.h"

@interface FacebookShareWorker : PublishWorker <WorkerProgressDelegate, PhFacebookDelegate>

@property (strong) PhFacebook* facebookClient;

+ (FacebookShareWorker*)workerWithHighlight:(HighlightObject*) highlightObject;

+ (void)handleEncodingWarning:(NSWindow*)window;

@end

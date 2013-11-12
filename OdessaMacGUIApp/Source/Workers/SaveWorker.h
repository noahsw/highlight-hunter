//
//  SaveWorker.h
//  OdessaMacGUIApp
//
//  Created by Noah Spitzer-Williams on 9/4/12.
//  Copyright (c) 2012 Authentically Digital LLC. All rights reserved.
//

#import "PublishWorker.h"
#import "DDLog.h"
#import "HighlightObject.h"
#import "AnalyticsHelper.h"
#import "MainModel.h"
#import "InputFileObject.h"
#import <AVFoundation/AVFoundation.h>
#import "MediaInfoWorker.h"
#import "../Encryption/CustomMD5.h"

@interface SaveWorker : PublishWorker

enum {
    OutputFormatOriginal,
    OutputFormatFacebook,
    OutputFormatProRes
};

@property NSInteger outputFormat;

+ (SaveWorker*)workerWithHighlight:(HighlightObject*) highlightObject;

@end

//
//  MediaInfoWorker.h
//  OdessaMacGUIApp
//
//  Created by Noah Spitzer-Williams on 9/26/12.
//  Copyright (c) 2012 Authentically Digital LLC. All rights reserved.
//

#import <Foundation/Foundation.h>
#import "MainModel.h"
#import "DDLog.h"
#import <ASIHTTPRequest/ASIFormDataRequest.h>
#import "BrowserHelper.h"
#import "OdessaEngineV2Project.h"

@interface MediaInfoWorker : NSOperation

@property (strong) NSURL* url;

@property NSInteger odessaReturnCode;

@property (strong) NSString* errorCode;

+ (MediaInfoWorker*)workerWithURL:(NSURL*)url odessaReturnCode:(NSInteger)odessaReturnCode;

+ (MediaInfoWorker*)workerWithURL:(NSURL*)url errorCode:(NSString*)errorCode;

@end

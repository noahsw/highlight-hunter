//
//  ScanWorkerHost.h
//  OdessaMacGUIApp
//
//  Created by Noah Spitzer-Williams on 8/29/12.
//  Copyright (c) 2012 Authentically Digital LLC. All rights reserved.
//

#import <Foundation/Foundation.h>

#import "MainModel.h"

#import "DDLog.h"

#import "ScanWorker.h"

#import "ScanWorkerDelegate.h"
#import "ScanWorkerHostDelegate.h"

#import "NSURL+Helpers.h"

#import "AnalyticsHelper.h"

@interface ScanWorkerHost : NSOperation <ScanWorkerDelegate>

@property (unsafe_unretained) MainModel* mainModel;

@property (unsafe_unretained) id<ScanWorkerHostDelegate> delegate;

- (BOOL)getProgressValue:(NSInteger*)progress;

- (NSString*)getTimeRemaining;

@end

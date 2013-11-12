//
//  BigDropHereImageView.h
//  OdessaMacGUIApp
//
//  Created by Noah Spitzer-Williams on 9/6/12.
//  Copyright (c) 2012 Authentically Digital LLC. All rights reserved.
//

#import <Cocoa/Cocoa.h>
#import "InputFilesDropDelegate.h"
#import "BigDropHereDelegate.h"
#import "DDLog.h"
#import "AnalyticsHelper.h"

@interface BigDropHereImageView : NSImageView <NSDraggingDestination>

@property (unsafe_unretained) id<InputFilesDropDelegate> inputFilesDropDelegate;

@property (unsafe_unretained) id<BigDropHereDelegate> bigDropHereDelegate;

@end

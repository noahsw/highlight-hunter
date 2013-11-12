//
//  InputVideoThumbnailView.h
//  OdessaMacGUIApp
//
//  Created by Noah Spitzer-Williams on 9/19/12.
//  Copyright (c) 2012 Authentically Digital LLC. All rights reserved.
//

#import <Cocoa/Cocoa.h>
#import "../CoreModels/InputFileObject.h"
#import "InputFilesDropDelegate.h"
#import "SmallDropHereDelegate.h"
#import "DDLog.h"
#import "AnalyticsHelper.h"

@interface InputVideoThumbnailView : NSView <NSDraggingDestination>

@property (unsafe_unretained) id<InputFilesDropDelegate> inputFilesDropDelegate;

@property (unsafe_unretained) id<SmallDropHereDelegate> smallDropHereDelegate;

@end

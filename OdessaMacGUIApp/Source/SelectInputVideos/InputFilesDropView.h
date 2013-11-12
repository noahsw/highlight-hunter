//
//  InputFilesDropView.h
//  OdessaMacPrototype
//
//  Created by Noah Spitzer-Williams on 8/23/12.
//
//

#import <Cocoa/Cocoa.h>
#import "../CoreModels/InputFileObject.h"
#import "InputFilesDropDelegate.h"
#import "BigDropHereDelegate.h"
#import "SmallDropHereDelegate.h"
#import "DDLog.h"
#import "AnalyticsHelper.h"

@interface InputFilesDropView : NSScrollView <NSDraggingDestination>

@property (unsafe_unretained) id<InputFilesDropDelegate> inputFilesDropDelegate;

@property (unsafe_unretained) id<BigDropHereDelegate> bigDropHereDelegate;

@property (unsafe_unretained) id<SmallDropHereDelegate> smallDropHereDelegate;

@end

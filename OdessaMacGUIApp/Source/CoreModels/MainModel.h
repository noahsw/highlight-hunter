//
//  MainModel.h
//  OdessaMacPrototype
//
//  Created by Noah Spitzer-Williams on 8/28/12.
//
//

#import <Foundation/Foundation.h>
@class InputFileObject;

#import "../CocoaLumberjack/DDLog.h"

@interface MainModel : NSObject

@property (strong) NSMutableArray* inputFileObjects;

@property (strong) NSMutableArray* highlightObjects;


// Settings
@property NSTimeInterval captureDurationInSeconds;

@property BOOL ignoreEarlyHighlights;

@property NSInteger detectionThreshold;


- (void)addInputFileObjectsObject:(InputFileObject *)object;

+ (NSURL*) getURLToSampleVideo;

+ (NSURL*) getDefaultFolderURL;

+ (NSURL*) getParentFolder: (NSURL*) url;

@end

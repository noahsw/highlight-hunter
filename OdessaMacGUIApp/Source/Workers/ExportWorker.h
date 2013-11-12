//
//  ExportWorker.h
//  OdessaMacGUIApp
//
//  Created by Noah Spitzer-Williams on 10/1/12.
//  Copyright (c) 2012 Authentically Digital LLC. All rights reserved.
//

#import <Foundation/Foundation.h>
#import "InputFileObject.h"
#import "HighlightObject.h"
#import "DDLog.h"
#import "NSAlert+SynchronousSheet.h"
#import "AnalyticsHelper.h"

@interface ExportWorker : NSObject

@property (strong) NSMutableArray* highlightObjects;

@property (strong) NSString* projectFileName;

- (void)exportHighlights:(NSWindow*)window;

- (NSString*)appName;

- (BOOL)exportHighlightsToFile:(NSURL*)outputURL;

- (NSString*)allowedFileExtension;

- (NSArray*)unsupportedExtensions;

- (NSString*)launchAppMessage;

- (NSArray*)inputFileObjectsWithHighlights;

@end

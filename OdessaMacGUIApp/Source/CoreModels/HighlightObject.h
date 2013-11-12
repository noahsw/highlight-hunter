//
//  HighlightObject.h
//  OdessaMacGUIApp
//
//  Created by Noah Spitzer-Williams on 8/22/12.
//  Copyright (c) 2012 Authentically Digital LLC. All rights reserved.
//

#import <Foundation/Foundation.h>
#import "ThumbnailGenerator.h"
#import "ThumbnailGeneratorDelegate.h"
#import "InputFileObject.h"
#import "PublishStatusDelegate.h"
@class SaveWorker; // because we have circular definitions
@class FacebookShareWorker; // because we have circular definitions

@interface HighlightObject : NSObject <ThumbnailGeneratorDelegate>
{
    NSTimeInterval beginTime;
    NSTimeInterval endTime;
    
    SaveWorker* saveWorker;
    FacebookShareWorker* facebookShareWorker;
}

@property (strong) NSString* title;

@property (strong) NSImage* thumbnailImage;

@property NSTimeInterval beginTime;
@property NSTimeInterval endTime;
@property NSTimeInterval bookmarkTime;

@property BOOL hasBeenReviewed;

@property (unsafe_unretained) InputFileObject* inputFileObject;

@property (unsafe_unretained) id<PublishStatusDelegate> publishStatusDelegate;

@property (strong) SaveWorker* saveWorker;
@property (strong) FacebookShareWorker* facebookShareWorker;

@property (strong) NSString* facebookActivityId; 

- (NSString*) friendlyBeginTime; // hh:mm:ss
- (NSString*) friendlyEndTime; // hh:mm:ss

- (NSTimeInterval)duration;
- (NSString*)friendlyDuration;

- (void)generateThumbnail:(ThumbnailGenerator*)thumbnailGenerator;

@end

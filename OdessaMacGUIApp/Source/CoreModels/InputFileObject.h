//
//  InputFileObject.h
//  OdessaMacPrototype
//
//  Created by Noah Spitzer-Williams on 8/20/12.
//
//

#import <Foundation/Foundation.h>
#import "../InterfaceHelpers/ThumbnailGenerator/ThumbnailGenerator.h"
#import "../InterfaceHelpers/ThumbnailGenerator/ThumbnailGeneratorDelegate.h"
@class ScanWorker;

@interface InputFileObject : NSObject <ThumbnailGeneratorDelegate>
{
    //NSURL* sourceURL;
}

@property (strong) NSURL* sourceURL;

@property (strong) NSString* title;

@property (strong) NSImage* thumbnailImage;

@property BOOL isSmallDropHereControl;

@property BOOL showRemoveButton;
@property BOOL removeButtonAlwaysHidden;

@property NSTimeInterval videoDuration; // in seconds
@property double framesPerSecond;
@property long long totalFrames;
@property int bitrate; // why is this int and not NSInteger?
@property NSInteger videoWidth;
@property NSInteger videoHeight;

@property NSInteger scanWorkerResult;

- (void)generateThumbnail:(ThumbnailGenerator*)thumbnailGenerator;

@end

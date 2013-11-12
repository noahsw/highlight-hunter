//
//  ThumbnailQueueItem.h
//  OdessaMacPrototype
//
//  Created by Noah Spitzer-Williams on 8/28/12.
//
//

#import <Foundation/Foundation.h>
#import "ThumbnailGeneratorDelegate.h"
#import "DDlog.h"
#import "MainModel.h"

@interface ThumbnailOperation : NSOperation

@property NSTimeInterval seekInSeconds;

@property NSSize size;

@property (unsafe_unretained) NSURL* sourceURL;

@property (unsafe_unretained) id<ThumbnailGeneratorDelegate> delegate;

@end

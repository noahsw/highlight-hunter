//
//  ThumbnailGenerator.h
//  OdessaMacPrototype
//
//  Created by Noah Spitzer-Williams on 8/28/12.
//
//

#import <Foundation/Foundation.h>
#import "ThumbnailOperation.h"

@interface ThumbnailGenerator : NSObject

@property (strong) NSOperationQueue* operationQueue;

- (void)generateThumbnail:(ThumbnailOperation*)thumbnailOperation;


@end

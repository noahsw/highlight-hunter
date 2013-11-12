//
//  ThumbnailGenerator.m
//  OdessaMacPrototype
//
//  Created by Noah Spitzer-Williams on 8/28/12.
//
//

#import "ThumbnailGenerator.h"

@implementation ThumbnailGenerator

- (id)init
{
    self = [super init];
    if (self)
    {
        self.operationQueue = [NSOperationQueue new];
        [self.operationQueue setMaxConcurrentOperationCount:1];
    }
    return self;
}

- (void)generateThumbnail:(ThumbnailOperation *)thumbnailOperation
{
    
    [self.operationQueue addOperation:thumbnailOperation];
 
    
}

@end

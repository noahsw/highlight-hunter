//
//  InputFileObject.m
//  OdessaMacPrototype
//
//  Created by Noah Spitzer-Williams on 8/20/12.
//
//

#import "InputFileObject.h"

@implementation InputFileObject

- (id)init
{
    self = [super init];
    if (self)
    {
        [self setThumbnailImage:[NSImage imageNamed:@"review-novideos-question-mark-icon.png"]];
    }
    return self;
}

- (void)generateThumbnail:(ThumbnailGenerator *)thumbnailGenerator
{
    
    ThumbnailOperation* thumbnailOperation = [ThumbnailOperation new];
    [thumbnailOperation setDelegate:self];
    [thumbnailOperation setSourceURL:self.sourceURL];
    [thumbnailOperation setSeekInSeconds:5];
    [thumbnailOperation setSize:NSMakeSize(272, 153)];
    
    [thumbnailGenerator generateThumbnail:thumbnailOperation];
    
}
                                                         
                                                        
- (void)thumbnailGenerated:(NSImage *)thumbnailImage
{
    self.thumbnailImage = thumbnailImage;
}

@end

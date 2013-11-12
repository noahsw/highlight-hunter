//
//  ThumbnailGeneratorDelegate.h
//  OdessaMacPrototype
//
//  Created by Noah Spitzer-Williams on 8/28/12.
//
//

#import <Foundation/Foundation.h>

@protocol ThumbnailGeneratorDelegate <NSObject>

- (void)thumbnailGenerated:(NSImage*)thumbnailImage;

@end

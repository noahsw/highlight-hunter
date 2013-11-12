//
//  NSImageView+Coordinates.h
//  OdessaMacPrototype
//
//  Created by Noah Spitzer-Williams on 8/22/12.
//
//

#import <Foundation/Foundation.h>

@interface NSImageView (Coordinates)

- (NSInteger)leftCoordinate;
- (NSInteger)rightCoordinate;
- (NSInteger)width;

- (void)setLeftCoordinate:(NSInteger)left;

- (void)setWidth:(NSInteger)width;

@end

//
//  NSImageView+Coordinates.m
//  OdessaMacPrototype
//
//  Created by Noah Spitzer-Williams on 8/22/12.
//
//

#import "NSImageView+Coordinates.h"

@implementation NSImageView (Coordinates)

- (NSInteger)leftCoordinate
{
    return self.frame.origin.x;
}

- (NSInteger)rightCoordinate
{
    return self.frame.origin.x + self.frame.size.width - 1; // TODO: -1 is a hack because we were getting weird pixel gaps
}

- (NSInteger)width
{
    return self.frame.size.width;
}

- (void)setLeftCoordinate:(NSInteger)left
{
    [self setNeedsDisplayInRect:self.frame];
    
    NSPoint newOrigin = NSMakePoint(left, self.frame.origin.y);
    [self setFrameOrigin:newOrigin];
    
    [self setNeedsDisplay:YES];
}

- (void)setWidth:(NSInteger)width
{
    [self setNeedsDisplayInRect:self.frame];
    
    NSSize newSize = NSMakeSize(width, self.frame.size.height);
    [self setFrameSize:newSize];
    
    [self setNeedsDisplay:YES];
}



@end

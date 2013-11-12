//
//  TimelineView.m
//  OdessaMacPrototype
//
//  Created by Noah Spitzer-Williams on 8/27/12.
//
//

#import "TimelineView.h"

@implementation TimelineView

- (void)mouseDown:(NSEvent *)theEvent
{
    NSPoint location = [theEvent locationInWindow];
    
    [self.delegate timelineClickedAtCoordinate:location];
    
}

@end

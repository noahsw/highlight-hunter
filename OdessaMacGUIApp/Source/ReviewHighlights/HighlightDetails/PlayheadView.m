//
//  PlayheadView.m
//  OdessaMacPrototype
//
//  Created by Noah Spitzer-Williams on 8/23/12.
//
//

#import "PlayheadView.h"

#if DEBUG
static const int ddLogLevel = LOG_LEVEL_VERBOSE | LOG_LEVEL_INFO | LOG_LEVEL_ERROR | LOG_LEVEL_WARN;
#else
static const int ddLogLevel = LOG_LEVEL_INFO | LOG_LEVEL_ERROR | LOG_LEVEL_WARN;
#endif


@implementation PlayheadView

- (id)initWithFrame:(NSRect)frame
{
    self = [super initWithFrame:frame];
    if (self) {
        // Initialization code here.
    }
    
    return self;
}

- (void)mouseDown:(NSEvent *)theEvent
{
    DDLogVerbose(@"called mouseDown");
    
    // store the starting mouse-down location;
    [self setStartDragLocation:[theEvent locationInWindow]];
    DDLogVerbose(@"startDragLocation = %f, %f", self.startDragLocation.x, self.startDragLocation.y);
    
    [self setStartFrameOrigin: self.frame.origin];
    
    DDLogVerbose(@"startFrameOrigin = %f, %f", self.startFrameOrigin.x, self.startFrameOrigin.y);
    
    [self.delegate playheadDragStarted];
    
    // set the cursor to the closed hand cursor
    // for the duration of the drag
    [[NSCursor closedHandCursor] push];
    
}

-(void)mouseDragged:(NSEvent *)theEvent
{
    NSPoint newDragLocation=[theEvent locationInWindow];
    
    DDLogVerbose(@"newDragLocation = %f, %f,", newDragLocation.x, newDragLocation.y);
    
    CGFloat deltaX = newDragLocation.x - self.startDragLocation.x;
    
    NSPoint origin = NSMakePoint(self.startFrameOrigin.x + deltaX, self.startFrameOrigin.y);
    
    
    if (origin.x < self.leftCoordinateBounds)
        return;
    if (origin.x > self.rightCoordinateBounds)
        return;
    
    [self setFrameOrigin:origin];
    
    [self.delegate playheadDragged];
    
    DDLogVerbose(@"moving to %f, %f", origin.x, origin.y);
}

-(void)mouseUp:(NSEvent *)event
{
    DDLogVerbose(@"called mouseUp");
    
    [self.delegate playheadDragEnded];
    
    // finished dragging, restore the cursor
    [NSCursor pop];
    
    // the item has moved, we need to reset our cursor
    // rectangle
    [[self window] invalidateCursorRectsForView:self];
}


@end

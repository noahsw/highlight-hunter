//
//  EndTickView.m
//  OdessaMacGUIApp
//
//  Created by Noah Spitzer-Williams on 9/4/12.
//  Copyright (c) 2012 Authentically Digital LLC. All rights reserved.
//

#import "EndTickView.h"

#if DEBUG
static const int ddLogLevel = LOG_LEVEL_VERBOSE | LOG_LEVEL_INFO | LOG_LEVEL_ERROR | LOG_LEVEL_WARN;
#else
static const int ddLogLevel = LOG_LEVEL_INFO | LOG_LEVEL_ERROR | LOG_LEVEL_WARN;
#endif

@interface EndTickView ()

@property NSPoint startDragLocation;
@property NSPoint startFrameOrigin;

@end

@implementation EndTickView

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
    
    [self.delegate endTickDragStarted];
    
    // set the cursor to the closed hand cursor
    // for the duration of the drag
    [[NSCursor closedHandCursor] push];
    
}

-(void)mouseDragged:(NSEvent *)theEvent
{
    NSPoint newDragLocation=[theEvent locationInWindow];
    
    //DDLogVerbose(@"newDragLocation = %f, %f,", newDragLocation.x, newDragLocation.y);
    
    CGFloat deltaX = newDragLocation.x - self.startDragLocation.x;
    
    NSPoint origin = NSMakePoint(self.startFrameOrigin.x + deltaX, self.startFrameOrigin.y);
    
    if (origin.x > self.rightCoordinateBounds - 20)
    {
        if (self.zoomOutTimer == nil)
        {
            self.zoomOutTimer = [NSTimer scheduledTimerWithTimeInterval:0.5 target:self.delegate selector:@selector(endZoomOut) userInfo:nil repeats:YES];
        }
    }
    else if (self.zoomOutTimer != nil)
    {
        [self.zoomOutTimer invalidate];
        self.zoomOutTimer = nil;
    }
    
    if (origin.x < self.leftCoordinateBounds)
    {
        DDLogVerbose(@"beyond bounds");
        return;
    }
    if (origin.x > self.rightCoordinateBounds)
    {
        DDLogVerbose(@"beyond bounds");
        return;
    }
    
    [self.delegate setHighlightEndTimeForCoordinate:NSMakePoint(self.rightCoordinate, 0)];
    [self.delegate endTickDragged];
    
    [self setFrameOrigin:origin];
    
    //DDLogVerbose(@"called mouseDragged and moving to %f, %f", origin.x, origin.y);
}

-(void)mouseUp:(NSEvent *)event
{
    DDLogVerbose(@"called mouseUp");
    
    if (self.zoomOutTimer != nil)
    {
        [self.zoomOutTimer invalidate];
        self.zoomOutTimer = nil;
    }
    
    [self.delegate endTickDragEnded];
    
    // finished dragging, restore the cursor
    [NSCursor pop];
    
    // the item has moved, we need to reset our cursor
    // rectangle
    [[self window] invalidateCursorRectsForView:self];
}


@end

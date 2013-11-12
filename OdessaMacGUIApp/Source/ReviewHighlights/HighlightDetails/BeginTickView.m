//
//  StartTickView.m
//  OdessaMacPrototype
//
//  Created by Noah Spitzer-Williams on 8/24/12.
//
//

#import "BeginTickView.h"

#if DEBUG
static const int ddLogLevel = LOG_LEVEL_VERBOSE | LOG_LEVEL_INFO | LOG_LEVEL_ERROR | LOG_LEVEL_WARN;
#else
static const int ddLogLevel = LOG_LEVEL_INFO | LOG_LEVEL_ERROR | LOG_LEVEL_WARN;
#endif



@interface BeginTickView ()

@property NSPoint startDragLocation;
@property NSPoint startFrameOrigin;

@end


@implementation BeginTickView

- (id)initWithFrame:(NSRect)frame
{
    self = [super initWithFrame:frame];
    if (self) {
        // Initialization code here.
        self.isDragging = NO;
    }
    
    return self;
}

- (void)mouseDown:(NSEvent *)theEvent
{
    DDLogVerbose(@"called mouseDown");
    
    if ([theEvent clickCount] > 1)
    {
        self.isDragging = NO;
        [self.delegate seekToHighlightBeginning];
    }
    else
    {
        // store the starting mouse-down location;
        [self setStartDragLocation:[theEvent locationInWindow]];
        DDLogVerbose(@"startDragLocation = %f, %f", self.startDragLocation.x, self.startDragLocation.y);
        
        [self setStartFrameOrigin: self.frame.origin];
        
        DDLogVerbose(@"startFrameOrigin = %f, %f", self.startFrameOrigin.x, self.startFrameOrigin.y);
        
    }
    
}

-(void)mouseDragged:(NSEvent *)theEvent
{
    if (self.isDragging == NO)
    { // only called beginTickDragStarted when we know we're actually dragging because it pauses the video
    
        [self.delegate beginTickDragStarted];
        
        // set the cursor to the closed hand cursor
        // for the duration of the drag
        [[NSCursor closedHandCursor] push];

        self.isDragging = YES; // put this here because in mouseDown, clickCount will always start at 1
        
    }
    
    
    NSPoint newDragLocation=[theEvent locationInWindow];
    
    //DDLogVerbose(@"newDragLocation = %f, %f,", newDragLocation.x, newDragLocation.y);
    
    CGFloat deltaX = newDragLocation.x - self.startDragLocation.x;
    
    NSPoint origin = NSMakePoint(self.startFrameOrigin.x + deltaX, self.startFrameOrigin.y);
    
    if (origin.x < self.leftCoordinateBounds + 20)
    {
        if (self.zoomOutTimer == nil)
        {
            self.zoomOutTimer = [NSTimer scheduledTimerWithTimeInterval:0.5 target:self.delegate selector:@selector(beginZoomOut) userInfo:nil repeats:YES];
        }
    }
    else if (self.zoomOutTimer != nil)
    {
        [self.zoomOutTimer invalidate];
        self.zoomOutTimer = nil;
    }
    
    if (origin.x < self.leftCoordinateBounds)
        return;
    if (origin.x > self.rightCoordinateBounds)
        return;
    
    [self.delegate setHighlightBeginTimeForCoordinate:NSMakePoint(self.rightCoordinate, 0)];
    [self.delegate beginTickDragged];
    
    [self setFrameOrigin:origin];
    
    //DDLogVerbose(@"called mouseDragged and moving to %f, %f", origin.x, origin.y);
}

-(void)mouseUp:(NSEvent *)event
{
    DDLogVerbose(@"called mouseUp");
    
    if (self.isDragging)
    {
        
        if (self.zoomOutTimer != nil)
        {
            [self.zoomOutTimer invalidate];
            self.zoomOutTimer = nil;
        }
        
        [self.delegate beginTickDragEnded];
        
        self.isDragging = NO;
        
    }

    // finished dragging, restore the cursor
    [NSCursor pop];
    
    // the item has moved, we need to reset our cursor
    // rectangle
    [[self window] invalidateCursorRectsForView:self];
    
}

@end

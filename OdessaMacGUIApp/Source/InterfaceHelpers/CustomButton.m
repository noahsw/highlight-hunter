//
//  CustomButton.m
//  OdessaMacGUIApp
//
//  Created by Noah Spitzer-Williams on 12/20/11.
//  Copyright (c) 2011 Authentically Digital LLC. All rights reserved.
//

#import "CustomButton.h"
#import "NSButton+TextColor.h"

#import "DDLog.h"
#if DEBUG
static const int ddLogLevel = LOG_LEVEL_INFO | LOG_LEVEL_ERROR | LOG_LEVEL_WARN;
#else
static const int ddLogLevel =  LOG_LEVEL_INFO | LOG_LEVEL_ERROR | LOG_LEVEL_WARN;
#endif

@implementation CustomButton


- (void) awakeFromNib
{
    [self setBordered:NO];
    
    if ([[self title] isEqualToString:@"BROWSE"])
        [self setFont:[NSFont boldSystemFontOfSize:13]];
    else
        [self setFont:[NSFont boldSystemFontOfSize:16]];
    
    [self setButtonType:NSMomentaryChangeButton];
    [[self cell] setHighlightsBy:NSNoCellMask];
    
    //[[self cell] setTrackingMode:NSTrackingInVisibleRect];
    
        
}

- (void) viewWillMoveToWindow:(NSWindow *)newWindow
{
    DDLogVerbose(@"called");
    NSTrackingArea* trackingArea = [[NSTrackingArea alloc] initWithRect:[self frame] options: (NSTrackingMouseEnteredAndExited | NSTrackingActiveAlways | NSTrackingInVisibleRect) owner:self userInfo:nil];
    [self addTrackingArea:trackingArea];
    
    
    isHovering = false;
    
    [super viewWillMoveToWindow:newWindow];
}

- (void) mouseEntered:(NSEvent *)theEvent
{
    DDLogVerbose(@"called");
    isHovering = true;
    [self setNeedsDisplay];
    [super mouseEntered:theEvent];
}

- (void) mouseExited:(NSEvent *)theEvent
{
    DDLogVerbose(@"called");
    isHovering = false;
    [self setNeedsDisplay];
    [super mouseExited:theEvent];
}

- (void) mouseDown:(NSEvent *)theEvent
{
    DDLogVerbose(@"called");
    [self setNeedsDisplay];
    [super mouseDown:theEvent];
}

- (void) mouseUp:(NSEvent *)theEvent
{
    DDLogVerbose(@"called");
    [self setNeedsDisplay];
    [super mouseUp:theEvent];
}



- (void) drawRect:(NSRect)dirtyRect
{
    
    DDLogVerbose(@"called. isHovering=%d. isHighlighted=%d", isHovering, [[self cell] isHighlighted]);
    
    //[[self cell] setBackgroundColor:[NSColor colorWithCalibratedRed:0 green:0 blue:0 alpha:0.2]];

    NSColor* backgroundColor;
    if (isHovering)
    {
        backgroundColor = [NSColor colorWithCalibratedWhite:0 alpha:0.5f];
    }
    else
    {
        backgroundColor = [NSColor colorWithCalibratedWhite:0 alpha:0.3f];
    }
    NSColor* odessaGreen = [NSColor colorWithCalibratedRed:0.6 green:0.8 blue:0.2 alpha:1.0];
    
    if ([[self cell] isHighlighted])
    {
        [self setTextColor:backgroundColor];
        [odessaGreen setFill];
    }
    else
    {
        [self setTextColor:odessaGreen];
        [backgroundColor setFill];
    }
        
    //NSRectFill(dirtyRect);
    NSRectFillUsingOperation(dirtyRect, NSCompositeSourceAtop);
    
    /*
                                         
    if ([[self cell] isHighlighted]) // whether user is clicking
    {
        [self setTextColor:[NSColor grayColor]];
    }
    else
    {
        
        if ([[self stringValue] isEqualToString:@"..."])
        {
            [self setTextColor:[NSColor whiteColor]];
        }
        else
        {
            
            NSColor* odessaGreen = [NSColor colorWithCalibratedRed:0.6 green:0.8 blue:0.2 alpha:1.0];
        
            [self setTextColor:odessaGreen];
        }
            
    }
     */
    
    [super drawRect:dirtyRect];
}


@end

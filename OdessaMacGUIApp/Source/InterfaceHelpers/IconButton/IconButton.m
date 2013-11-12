//
//  IconButton.m
//  OdessaMacGUIApp
//
//  Created by Noah Spitzer-Williams on 9/12/12.
//  Copyright (c) 2012 Authentically Digital LLC. All rights reserved.
//

#import "IconButton.h"

@implementation IconButton

- (id)initWithFrame:(NSRect)frame
{
    self = [super initWithFrame:frame];
    if (self) {
        // Initialization code here.
    }
    
    return self;
}

- (void)awakeFromNib
{
    [self createTrackingArea];
    self.isActionable = YES;
}

- (void)createTrackingArea
{
    NSTrackingAreaOptions focusTrackingAreaOptions = NSTrackingActiveInActiveApp;
    focusTrackingAreaOptions |= NSTrackingMouseEnteredAndExited;
    focusTrackingAreaOptions |= NSTrackingAssumeInside;
    focusTrackingAreaOptions |= NSTrackingInVisibleRect;
    
    NSTrackingArea *focusTrackingArea = [[NSTrackingArea alloc] initWithRect:NSZeroRect
                                                                     options:focusTrackingAreaOptions owner:self userInfo:nil];
    [self addTrackingArea:focusTrackingArea];
}

- (void)mouseEntered:(NSEvent *)theEvent
{
    [self showHoverState];
    [self.iconButtonDelegate showHoverState];
}

- (void)mouseExited:(NSEvent *)theEvent
{
    [self showRestingState];
    [self.iconButtonDelegate showRestingState];
}

- (void)showHoverState
{
    if (self.isActionable)
        [self setTextColor:[DesignLanguage lightGrayColor]];
}

- (void)showRestingState
{
    if (self.isActionable)
        [self setTextColor:[DesignLanguage midGrayColor]];
}


@end

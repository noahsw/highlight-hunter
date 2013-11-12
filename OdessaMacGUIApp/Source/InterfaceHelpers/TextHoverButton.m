//
//  BrowseButton.m
//  OdessaMacGUIApp
//
//  Created by Noah Spitzer-Williams on 9/17/12.
//  Copyright (c) 2012 Authentically Digital LLC. All rights reserved.
//

#import "TextHoverButton.h"

@implementation TextHoverButton

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
    [self setTextColor:[DesignLanguage lightGrayColor]];
}

- (void)mouseExited:(NSEvent *)theEvent
{
    [self setTextColor:[DesignLanguage midGrayColor]];
}

@end

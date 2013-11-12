//
//  PublishButtonView.m
//  OdessaMacGUIApp
//
//  Created by Noah Spitzer-Williams on 9/27/12.
//  Copyright (c) 2012 Authentically Digital LLC. All rights reserved.
//

#import "PublishButtonView.h"

@implementation PublishButtonView

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
    [self.publishButtonDelegate showHover];
}

- (void)mouseExited:(NSEvent *)theEvent
{
    [self.publishButtonDelegate showResting];
}

@end

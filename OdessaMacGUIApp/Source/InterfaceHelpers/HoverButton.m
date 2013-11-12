//
//  HoverButtonCell.m
//  OdessaMacGUIApp
//
//  Created by Noah Spitzer-Williams on 9/4/12.
//  Copyright (c) 2012 Authentically Digital LLC. All rights reserved.
//

#import "HoverButton.h"


@interface HoverButton ()

@property (strong) NSImage* oldImage;

@end



@implementation HoverButton

- (void)createTrackingArea
{
    NSTrackingAreaOptions focusTrackingAreaOptions = NSTrackingActiveInActiveApp;
    focusTrackingAreaOptions |= NSTrackingMouseEnteredAndExited;
    focusTrackingAreaOptions |= NSTrackingAssumeInside;
    focusTrackingAreaOptions |= NSTrackingInVisibleRect;
    
    NSTrackingArea *focusTrackingArea = [[NSTrackingArea alloc] initWithRect:NSZeroRect options:focusTrackingAreaOptions owner:self userInfo:nil];
    [self addTrackingArea:focusTrackingArea];
}

- (void)awakeFromNib
{
    [self createTrackingArea];
}


- (void)mouseEntered:(NSEvent* )event {
    if (self.hoverImage != nil && [self.hoverImage isValid])
    {
        self.oldImage = self.image;
        [self setImage:self.hoverImage];
    }
}

- (void)mouseExited:(NSEvent*)event
{
    if (self.oldImage != nil && [self.oldImage isValid])
    {
        [self setImage:self.oldImage];
        self.oldImage = nil;
    }
}


@end

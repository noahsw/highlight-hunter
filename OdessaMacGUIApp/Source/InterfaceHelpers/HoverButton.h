//
//  HoverButtonCell.h
//  OdessaMacGUIApp
//
//  Created by Noah Spitzer-Williams on 9/4/12.
//  Copyright (c) 2012 Authentically Digital LLC. All rights reserved.
//

#import <Cocoa/Cocoa.h>

@interface HoverButton : NSButton

@property (strong) NSImage* hoverImage;

- (void)createTrackingArea;

@end

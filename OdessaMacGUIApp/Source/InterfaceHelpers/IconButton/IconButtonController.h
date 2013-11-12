//
//  IconButtonController.h
//  OdessaMacGUIApp
//
//  Created by Noah Spitzer-Williams on 9/6/12.
//  Copyright (c) 2012 Authentically Digital LLC. All rights reserved.
//

#import <Cocoa/Cocoa.h>
#import "NSButton+TextColor.h"
#import "DesignLanguage.h"
#import "IconButton.h"


@interface IconButtonController : NSViewController {
}

@property (strong) IBOutlet IconButton *iconImage;

@property (strong) IBOutlet IconButton *button;

@property SEL actionSelector;

@property (unsafe_unretained) id actionTarget; // the host of this controller. needed so we know where to send selector

- (void)setIcon:(NSImage*)image;

- (void)setButtonText:(NSString*)text;

- (IBAction)takeAction:(id)sender;

@end

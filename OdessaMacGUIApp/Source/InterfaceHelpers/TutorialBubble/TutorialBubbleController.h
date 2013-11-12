//
//  TutorialBubbleController.h
//  OdessaMacGUIApp
//
//  Created by Noah Spitzer-Williams on 12/28/12.
//  Copyright (c) 2012 Authentically Digital LLC. All rights reserved.
//

#import <Cocoa/Cocoa.h>
#import "TutorialHelper.h"
#import "DDLog.h"
#import "HoverButton.h"

@interface TutorialBubbleController : NSViewController {
}

@property (strong) IBOutlet NSImageView *tooltipImage;

@property (strong) IBOutlet NSTextField *bubbleTextField;

@property (strong) IBOutlet HoverButton *nextButton;

@property SEL actionSelector;

@property (unsafe_unretained) id actionTarget; // the host of this controller. needed so we know where to send selector

- (IBAction)takeAction:(id)sender;

- (void)loadTooltip;

@end

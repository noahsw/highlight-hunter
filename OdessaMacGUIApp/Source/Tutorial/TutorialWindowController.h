//
//  TutorialWindowController.h
//  OdessaMacGUIApp
//
//  Created by Noah Spitzer-Williams on 11/2/11.
//  Copyright 2011 Authentically Digital LLC. All rights reserved.
//

#import <Cocoa/Cocoa.h>
#import "HoverButton.h"

@interface TutorialWindowController : NSWindowController {
}

@property (strong) IBOutlet HoverButton *closeButton;

- (IBAction)closeButtonPressed:(id)sender;



- (void)trialDidEndSelector;
- (void)closeWindow;


@end

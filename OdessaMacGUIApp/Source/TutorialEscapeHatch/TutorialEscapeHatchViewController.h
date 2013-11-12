//
//  TutorialEscapeHatchViewController.h
//  OdessaMacGUIApp
//
//  Created by Noah Spitzer-Williams on 1/5/13.
//  Copyright (c) 2013 Authentically Digital LLC. All rights reserved.
//

#import <Cocoa/Cocoa.h>
#import "TutorialHelper.h"
#import "TutorialEscapeHatchDelegate.h"

@interface TutorialEscapeHatchViewController : NSViewController {}

@property (strong) IBOutlet NSTextField *progressText;

@property (unsafe_unretained) id<TutorialEscapeHatchDelegate> delegate;

- (IBAction)exitTutorial:(id)sender;

- (void)refreshProgress;

@end

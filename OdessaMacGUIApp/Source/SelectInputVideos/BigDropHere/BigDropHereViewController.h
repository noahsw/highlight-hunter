//
//  BigDropHereViewController.h
//  OdessaMacGUIApp
//
//  Created by Noah Spitzer-Williams on 9/6/12.
//  Copyright (c) 2012 Authentically Digital LLC. All rights reserved.
//

#import <Cocoa/Cocoa.h>
#import "BigDropHereView.h"
#import "BigDropHereDelegate.h"
#import "BigDropHereImageView.h"
#import "InputFilesDropDelegate.h"
#import "GenericDropHereDelegate.h"
#import "HoverButton.h"

@interface BigDropHereViewController : NSViewController <BigDropHereDelegate> {
    id<InputFilesDropDelegate> inputFilesDropDelegate;
}

@property (strong) IBOutlet BigDropHereView *bigDropHereView;

@property (strong) IBOutlet BigDropHereImageView *dropHereImage;

@property (unsafe_unretained) id<InputFilesDropDelegate> inputFilesDropDelegate;

@property (unsafe_unretained) id<GenericDropHereDelegate> genericDropHereDelegate;

@property (strong) IBOutlet HoverButton*selectFromCameraButton;

@property (strong) IBOutlet HoverButton* browseComputerButton;

- (IBAction)selectFromCamera:(id)sender;

- (IBAction)browseComputer:(id)sender;

@end

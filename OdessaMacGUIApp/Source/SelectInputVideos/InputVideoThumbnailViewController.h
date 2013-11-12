//
//  InputVideoThumbnailViewController.h
//  OdessaMacGUIApp
//
//  Created by Noah Spitzer-Williams on 9/17/12.
//  Copyright (c) 2012 Authentically Digital LLC. All rights reserved.
//

#import <Cocoa/Cocoa.h>
#import "DDLog.h"
#import "GenericDropHereDelegate.h"
#import "NSButton+TextColor.h"
#import "DesignLanguage.h"
#import "TextHoverButton.h"
#import "SmallDropHereDelegate.h"
#import "HoverButton.h"
#import "RemoveInputFileDelegate.h"
#import "InputFileObject.h"
#import "TutorialHelper.h"

@interface InputVideoThumbnailViewController : NSCollectionViewItem <SmallDropHereDelegate> {
}



@property (unsafe_unretained) id<GenericDropHereDelegate> genericDropHereDelegate;

@property (unsafe_unretained) id<RemoveInputFileDelegate> removeInputFileDelegate;

@property (unsafe_unretained) IBOutlet TextHoverButton *browseComputerButton;
@property (unsafe_unretained) IBOutlet NSImageView *dropImage;

@property (unsafe_unretained) IBOutlet HoverButton *removeButton;


- (IBAction)browseComputer:(id)sender;
- (IBAction)removeVideo:(id)sender;
- (void)createTrackingArea;

@end

//
//  PublishStatusViewController.h
//  OdessaMacGUIApp
//
//  Created by Noah Spitzer-Williams on 8/30/12.
//  Copyright (c) 2012 Authentically Digital LLC. All rights reserved.
//

#import <Cocoa/Cocoa.h>
#import "AMCollectionViewItem.h"
#import "NSButton+TextColor.h"
#import "PublishStatusItem.h"
#import "DesignLanguage.h"
#import "WorkerProgressDelegate.h"
#import "HighlightDelegate.h"
#import "IconButton.h"
#import "FacebookShareWorker.h"
#import "AppStoreRating.h"

@interface PublishStatusViewItem : AMCollectionViewItem <WorkerProgressDelegate> {
}

@property (unsafe_unretained) HighlightObject* highlightObject;

@property NSInteger publishWorkerType;

@property (unsafe_unretained) PublishWorker* publishWorker;

@property (unsafe_unretained) id<HighlightDelegate> highlightDelegate;

@property (unsafe_unretained) NSWindow* window;

@property (strong) IBOutlet IconButton *imageButton;
@property (strong) IBOutlet IconButton *labelButton;
@property (strong) IBOutlet NSImageView *spinnerImage;

- (IBAction)buttonPressed:(id)sender;
@end

//
//  PublishButtonView.h
//  OdessaMacGUIApp
//
//  Created by Noah Spitzer-Williams on 9/4/12.
//  Copyright (c) 2012 Authentically Digital LLC. All rights reserved.
//

#import <Cocoa/Cocoa.h>
#import "NSButton+TextColor.h"
#import "HighlightObject.h"
#import "PublishWorker.h"
#import "DDLog.h"
#import "SaveWorker.h"
#import "InputFileObject.h"
#import "FacebookShareWorker.h"
#import "DesignLanguage.h"
#import "WorkerProgressDelegate.h"
#import "NSAlert+SynchronousSheet.h"
#import "PublishButtonDelegate.h"
#import "PublishButtonView.h"
#import "HoverButton.h"
#import "AppStoreRating.h"
#import "TutorialHelper.h"
#import "ShareButtonTutorialDelegate.h"

@interface PublishButtonViewController : NSViewController <WorkerProgressDelegate, PublishButtonDelegate>
{
    HighlightObject* highlightObject; // needed because we overwrite accessors
}


@property (unsafe_unretained) HighlightObject* highlightObject;

@property (unsafe_unretained) NSWindow* window;

@property (unsafe_unretained) id<ShareButtonTutorialDelegate> shareButtonTutorialDelegate;

@property (strong) IBOutlet PublishButtonView *publishButtonView;

@property NSInteger publishWorkerType;

@property (strong) IBOutlet NSButton *backgroundButton;
@property (strong) IBOutlet NSButton *iconButton;
@property (strong) IBOutlet NSButton *labelButton;
@property (strong) IBOutlet HoverButton *cancelButton;

- (IBAction)takeAction:(id)sender;

- (IBAction)cancelPublish:(id)sender;

- (void)detachWorkerProgressDelegate;

@end

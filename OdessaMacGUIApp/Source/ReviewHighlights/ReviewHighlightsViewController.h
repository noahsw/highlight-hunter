//
//  ReviewHighlightsView.h
//  OdessaMacGUIApp
//
//  Created by Noah Spitzer-Williams on 8/28/12.
//  Copyright (c) 2012 Authentically Digital LLC. All rights reserved.
//

#import <Cocoa/Cocoa.h>

#import "MainModel.h"
#import "DDLog.h"
#import "HighlightsCollectionView.h"
#import "IconButtonController.h"
#import "NSAlert+SynchronousSheet.h"
#import "ReviewHighlightsDelegate.h"
#import "HighlightDetails/HighlightDetailsDelegate.h"
#import "NoHighlightsObject.h"
#import "NoHighlightsDelegate.h"
#import "FinalCutProXExportWorker.h"
#import "TutorialBubbleController.h"
#import "TutorialHelper.h"
#import "TutorialEscapeHatchDelegate.h"

@interface ReviewHighlightsViewController : NSViewController <HighlightDetailsDelegate, NoHighlightsDelegate, TutorialEscapeHatchDelegate> {
}

@property (unsafe_unretained) NSWindow* window;

@property (unsafe_unretained) MainModel* mainModel;

@property (strong) IBOutlet HighlightsCollectionView *highlightsCollectionView;

@property (unsafe_unretained) id<ReviewHighlightsDelegate> reviewHighlightsDelegate;

@property (strong) IBOutlet IconButtonController *saveAllAsVideosButtonController;
@property (strong) IBOutlet NSView *saveAllAsVideosButton;

@property (strong) IBOutlet IconButtonController *exportButtonController;
@property (strong) IBOutlet NSView *exportButton;

@property (strong) IBOutlet IconButtonController *startOverButtonController;
@property (strong) IBOutlet NSView *startOverButton;

@property (unsafe_unretained) id<TutorialEscapeHatchDelegate> tutorialEscapeHatchDelegate;

@property (strong) IBOutlet TutorialBubbleController *highlightsFoundTutorialBubbleController;
@property (strong) IBOutlet NSView *highlightsFoundTutorialBubble;

@property (strong) IBOutlet TutorialBubbleController *startOverTutorialBubbleController;
@property (strong) IBOutlet NSView *startOverTutorialBubble;


@property (strong) IBOutlet NSButton *proButton;

- (bool)isPublishing;

- (void)cancelPublish;

- (IBAction)openProUpsell:(id)sender;

- (void)hideTutorialBubbles;

@end

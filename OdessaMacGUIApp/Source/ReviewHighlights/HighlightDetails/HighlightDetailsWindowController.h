//
//  HighlightDetailsWindow.h
//  OdessaMacPrototype
//
//  Created by Noah Spitzer-Williams on 8/9/12.
//
//

#import <Foundation/Foundation.h>
#import <QTKit/QTKit.h>
#import "PlayheadView.h"
#import "PlayheadViewDelegate.h"
#import "BeginTickView.h"
#import "BeginTickViewDelegate.h"
#import "EndTickView.h"
#import "EndTickViewDelegate.h"
#import "TimelineView.h"
#import "TimelineViewDelegate.h"
#import "InputFileObject.h"
#import "HighlightObject.h"

#import "MainModel.h"

#import "PublishButtonViewController.h"

#import "PublishWorker.h"

#import "HoverButton.h"

#import "NSAlert+SynchronousSheet.h"

#import "NSImageView+Coordinates.h"
#import "DDLog.h"
#import "HighlightDetailsDelegate.h"

#import "AnalyticsHelper.h"

#import "TutorialHelper.h"
#import "TutorialBubbleController.h"
#import "ShareButtonTutorialDelegate.h"
#import "TutorialEscapeHatchDelegate.h"

@interface HighlightDetailsWindowController : NSWindowController <NSWindowDelegate, BeginTickViewDelegate, EndTickViewDelegate, TimelineViewDelegate, PlayheadViewDelegate, ShareButtonTutorialDelegate>
{
    HighlightObject* currentHighlight; // needed because we're overwriting accessors
}

@property (unsafe_unretained) HighlightObject* currentHighlight;

@property (unsafe_unretained) MainModel* mainModel;

@property (strong) IBOutlet QTMovieView* movieView;

@property (unsafe_unretained) IBOutlet NSButton *playStateButton;

@property (unsafe_unretained) id<HighlightDetailsDelegate> highlightDetailsDelegate;

@property (unsafe_unretained) IBOutlet PlayheadView *playheadView;
@property (unsafe_unretained) IBOutlet TimelineView *timelineView;
@property (unsafe_unretained) IBOutlet BeginTickView *beginTickView;
@property (unsafe_unretained) IBOutlet EndTickView *endTickView;
@property (unsafe_unretained) IBOutlet TimelineView *highlightTimelineView;
@property (unsafe_unretained) IBOutlet NSTextField *highlightDurationField;
@property (strong) IBOutlet NSImageView *bookmarkTickView;

@property (strong) IBOutlet HoverButton *previousHighlightButton;
@property (strong) IBOutlet HoverButton *deleteHighlightButton;
@property (strong) IBOutlet HoverButton *nextHighlightButton;
@property (strong) IBOutlet NSTextField *highlightTitle;

@property (strong) IBOutlet PublishButtonViewController *shareToFacebookViewController;
@property (strong) IBOutlet NSView *shareToFacebookView;

@property (strong) IBOutlet PublishButtonViewController *saveToComputerViewController;
@property (strong) IBOutlet NSView *saveToComputerView;

@property (strong) IBOutlet TutorialBubbleController *bookmarkFlagTutorialBubbleController;
@property (strong) IBOutlet TutorialBubbleController *handlesTutorialBubbleController;
@property (strong) IBOutlet TutorialBubbleController *shareButtonTutorialBubbleController;
@property (strong) IBOutlet TutorialBubbleController *closeDetailsTutorialBubbleController;

@property (unsafe_unretained) id<TutorialEscapeHatchDelegate> tutorialEscapeHatchDelegate;

- (IBAction)changePlayState:(id)sender;

- (IBAction)deleteHighlight:(id)sender;
- (IBAction)showPreviousHighlight:(id)sender;
- (IBAction)showNextHighlight:(id)sender;

- (void)hideTutorialBubbles;

@end

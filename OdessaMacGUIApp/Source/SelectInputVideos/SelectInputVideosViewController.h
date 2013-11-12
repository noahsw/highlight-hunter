//
//  SelectInputVideosViewController.h
//  OdessaMacPrototype
//
//  Created by Noah Spitzer-Williams on 8/22/12.
//
//

#import <Cocoa/Cocoa.h>
#import "InputFilesDropView.h"
#import "../CoreModels/MainModel.h"
#import "HoverButton.h"
#import "ThumbnailGenerator.h"
#import "DDLog.h"
#import "IconButtonController.h"
#import "BigDropHereViewController.h"
#import "BigDropHereView.h"
#import "GenericDropHereDelegate.h"
#import "SelectInputVideosDelegate.h"
#import "HighlightObject.h"
#import "AnalyticsHelper.h"
#import "InputFilesCollectionView.h"
#import "InputVideoThumbnailViewController.h"
#import "InputVideoThumbnailView.h"
#import "WelcomeImageView.h"
#import "RemoveInputFileDelegate.h"
#import "TutorialBubbleController.h"
#import "TutorialHelper.h"
#import "TutorialEscapeHatchDelegate.h"

@interface SelectInputVideosViewController : NSViewController <InputFilesDropDelegate, GenericDropHereDelegate, RemoveInputFileDelegate>
{

}


@property (strong) IBOutlet NSArrayController *arrayController;

@property (unsafe_unretained) IBOutlet InputFilesDropView *inputFilesDropView;
@property (strong) IBOutlet InputFilesCollectionView *inputFilesCollectionView;
@property (strong) IBOutlet InputVideoThumbnailViewController *inputVideoThumbnailViewController;
@property (strong) IBOutlet InputVideoThumbnailView *inputVideoThumbnailView;

@property (unsafe_unretained) MainModel *mainModel;

@property (unsafe_unretained) id<SelectInputVideosDelegate> delegate;

@property (unsafe_unretained) id<TutorialEscapeHatchDelegate> tutorialEscapeHatchDelegate;

@property (strong) IBOutlet HoverButton *scanAllVideosButton;

@property (strong) IBOutlet IconButtonController *scanSampleVideoViewController;

@property (strong) IBOutlet TutorialBubbleController *sampleVideoTutorialBubbleController;
@property (strong) IBOutlet NSView *sampleVideoTutorialBubble;

@property (strong) IBOutlet TutorialBubbleController *scanButtonTutorialBubbleController;
@property (strong) IBOutlet NSView *scanButtonTutorialBubble;

@property (strong) IBOutlet BigDropHereViewController *bigDropHereViewController;
@property (strong) IBOutlet BigDropHereView *bigDropHereView;





- (IBAction)scanAllVideos:(id)sender;

- (void)scanSampleVideo;

- (void)initializeSampleVideoTutorialBubble; // public so we can reset tutorial

- (void)hideTutorialBubbles;

@end



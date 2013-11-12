//
//  AppController.h
//  OdessaMacGUIApp
//
//  Created by Noah Spitzer-Williams on 8/28/12.
//  Copyright (c) 2012 Authentically Digital LLC. All rights reserved.
//

#import "Breadcrumbs/BreadcrumbsViewController.h"

#import "CocoaLumberjack/DDLog.h"
#import "CocoaLumberjack/MyCustomLogFormatter.h"
#import "CocoaLumberjack/DDTTYLogger.h"
#import "CocoaLumberjack/DDFileLogger.h"

#import "CoreModels/MainModel.h"

#import "Preferences/Shared/MASPreferencesWindowController.h"
#import "Preferences/HighlightDurationPreferencesViewController.h"
#import "Preferences/DetectionThresholdPreferencesViewController.h"
#import "Preferences/AdvancedPreferencesViewController.h"

#import "SelectInputVideos/SelectInputVideosViewController.h"
#import "SelectInputVideos/SelectInputVideosDelegate.h"

#import "ScanVideos/ScanVideosViewController.h"
#import "ScanVideos/ScanVideosDelegate.h"

#import "ReviewHighlights/ReviewHighlightsViewController.h"
#import "ReviewHighlights/ReviewHighlightsDelegate.h"

#import "Tutorial/TutorialWindowController.h"

#import "BrowserHelper.h"

#import "UpdateChecker.h"

#import "OdessaEngineV2Project.h"

#include <stdlib.h> // for random numbers

#import <FeedbackReporter/FRFeedbackReporter.h>

#import <Foundation/Foundation.h>

#import "TutorialHelper.h"
#import "TutorialEscapeHatchViewController.h"
#import "TutorialEscapeHatchDelegate.h"

@interface AppController : NSObject <NSApplicationDelegate, FRFeedbackReporterDelegate, SelectInputVideosDelegate, ScanVideosDelegate, ReviewHighlightsDelegate, TutorialEscapeHatchDelegate> {
}


@property (strong) IBOutlet NSWindow *window;

@property (strong) MainModel* mainModel;

@property (strong) DDFileLogger* fileLogger;

@property (unsafe_unretained) IBOutlet BreadcrumbsView *breadcrumbsView;
@property (unsafe_unretained) IBOutlet BreadcrumbsViewController *breadcrumbsViewController;

@property (strong) NSViewController *mainViewController;

@property (strong) IBOutlet NSView *mainView;

@property (unsafe_unretained) IBOutlet NSButton *preferencesButton;

@property (unsafe_unretained) IBOutlet NSButton *activateButton;
@property (unsafe_unretained) IBOutlet NSButton *createFakeHighlightsButton;
@property (unsafe_unretained) IBOutlet NSButton *throwExceptionButton;

@property (strong) NSWindowController *preferencesWindowController;

@property (unsafe_unretained) IBOutlet NSMenuItem *upgradeToProMenuItem;
@property (unsafe_unretained) IBOutlet NSMenuItem *activateMenuItem;

@property (strong) TutorialWindowController* tutorialWindowController;

@property (unsafe_unretained) IBOutlet TutorialEscapeHatchViewController *tutorialEscapeHatchViewController;
@property (unsafe_unretained) IBOutlet NSView *tutorialEscapeHatchView;


// Keep references to them so we can call their methods
@property (strong) SelectInputVideosViewController* selectInputVideosViewController;
@property (strong) ScanVideosViewController* scanVideosViewController;
@property (strong) ReviewHighlightsViewController* reviewHighlightsViewController;

@property (unsafe_unretained) IBOutlet NSImageView *headerLogo;


- (IBAction)showPreferencesWindow:(id)sender;
- (IBAction)helpMenuPressed:(id)sender;
- (IBAction)viewDebugLogsPressed:(id)sender;
- (IBAction)upgradeToPro:(id)sender;
- (IBAction)showActivateWindow:(id)sender;
- (IBAction)showTutorialWindow:(id)sender;
- (IBAction)sendFeedback:(id)sender;

- (IBAction)shareTwitterButtonPressed:(id)sender;
- (IBAction)shareFacebookButtonPressed:(id)sender;

- (IBAction)createFakeHighlights:(id)sender;
- (IBAction)throwException:(id)sender;

+ (id)sharedAppController;

- (BOOL)isSafeToPrompt;

@end

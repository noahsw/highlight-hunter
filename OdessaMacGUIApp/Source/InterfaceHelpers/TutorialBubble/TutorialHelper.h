//
//  TutorialHelper.h
//  OdessaMacGUIApp
//
//  Created by Noah Spitzer-Williams on 12/30/12.
//  Copyright (c) 2012 Authentically Digital LLC. All rights reserved.
//

#import <Foundation/Foundation.h>
#import "AnalyticsHelper.h"

@interface TutorialHelper : NSObject


typedef enum _TutorialProgress {
    TutorialAddSampleVideo,
    TutorialScanButton,
    TutorialHighlightsFound,
    TutorialBookmarkFlag,
    TutorialHandles,
    TutorialShareButton,
    TutorialCloseDetails,
    TutorialStartOver,
    TutorialFinished
} TutorialProgress;

typedef enum _ArrowDirection {
    Bottom,
    Left,
    TopLeft,
    BottomLeft
} ArrowDirection;

+ (TutorialProgress)getProgress;

+ (void)advanceProgress;

+ (void)resetProgress;

+ (void)finish;

+ (void)tutorialStarted;

- (NSString*)getText;

- (NSPoint)getTextOrigin;

- (NSPoint)getNextButtonOrigin:(NSRect)tooltipFrame buttonFrame:(NSRect)buttonFrame;

- (BOOL)getIfNextButtonIsVisible;

- (ArrowDirection)getArrowDirection;

- (NSImage*)getTooltipImage;

@end

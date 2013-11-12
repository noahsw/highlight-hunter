//
//  TutorialHelper.m
//  OdessaMacGUIApp
//
//  Created by Noah Spitzer-Williams on 12/30/12.
//  Copyright (c) 2012 Authentically Digital LLC. All rights reserved.
//

#import "TutorialHelper.h"

@interface TutorialHelper ()

@property (unsafe_unretained) TutorialProgress progress;

@end

@implementation TutorialHelper

- (id)init
{
    self = [super init];
    if (self) {
        self.progress = [TutorialHelper getProgress];
    }
    
    return self;
}

+ (TutorialProgress)getProgress
{
    NSUserDefaults *standardUserDefaults = [NSUserDefaults standardUserDefaults];
	if (standardUserDefaults)
    {
        return (TutorialProgress)[standardUserDefaults integerForKey:@"TutorialProgress"];
    }
    return TutorialFinished;
}

+ (void)tutorialStarted
{
    [AnalyticsHelper fireEvent:[NSString stringWithFormat:@"Tutorial progress - %d - %@", TutorialAddSampleVideo, [TutorialHelper getFriendlyProgress:TutorialAddSampleVideo]] eventValue:[NSNumber numberWithInteger:TutorialAddSampleVideo]];
}

+ (void)advanceProgress
{
    NSUserDefaults *standardUserDefaults = [NSUserDefaults standardUserDefaults];
	if (standardUserDefaults)
    {
        TutorialProgress newProgress = [TutorialHelper getProgress] + 1;
        [standardUserDefaults setInteger:newProgress forKey:@"TutorialProgress"];
        [standardUserDefaults synchronize];
        
        [AnalyticsHelper fireEvent:[NSString stringWithFormat:@"Tutorial progress - %d - %@", newProgress, [TutorialHelper getFriendlyProgress:newProgress]] eventValue:[NSNumber numberWithInteger:newProgress]];
    }
}

+ (void)resetProgress
{
    NSUserDefaults *standardUserDefaults = [NSUserDefaults standardUserDefaults];
	if (standardUserDefaults)
    {
        [standardUserDefaults setInteger:TutorialAddSampleVideo forKey:@"TutorialProgress"];
        [standardUserDefaults synchronize];
    }
}

+ (void)finish
{
    NSUserDefaults *standardUserDefaults = [NSUserDefaults standardUserDefaults];
	if (standardUserDefaults)
    {
        [standardUserDefaults setInteger:TutorialFinished forKey:@"TutorialProgress"];
        [standardUserDefaults synchronize];
        
        [AnalyticsHelper fireEvent:@"Tutorial progress - Exit" eventValue:0];
    }
}

- (NSString *)getText
{
    switch (self.progress)
    {
        case TutorialAddSampleVideo:
            return @"Here is a sample video for you to try out.\n\nJustin marked two separate highlights by covering the lens after each one.";
        case TutorialScanButton:
            return @"Click \"Scan All Videos\" to scan the sample video for highlights.\n\nIf this were your own footage, every video you dropped in would be scanned.";
        case TutorialHighlightsFound:
            return @"These are the highlights that were detected in the footage.\n\nClick this first highlight to edit and share it.";
        case TutorialBookmarkFlag:
            return @"This marker represents when the lens was covered.\n\nBy default, the highlight ends just before this marker.";
        case TutorialHandles:
            return @"Move these handles to adjust where the highlight starts and ends.\n\nDrag each handle to the very end to include even more video.";
        case TutorialShareButton:
            return @"Click this button to share the highlight to Facebook.\n\n(Don't worry, we won't really share this demo highlight).";
        case TutorialCloseDetails:
            return @"Nice work. You've completed the tutorial!\n\nClose this window when you're ready to move on.";
        case TutorialStartOver:
            return @"Click \"Start over\" to go back to the beginning.\n\nIt's time to use your own footage!";
//        case TutorialUseOwnFootage:
//            return @"Great stuff. Now drop in your own footage to get started.\n\nIf you need more help, you can run the tutorial again."; // TODO: hyperlink "run tutorial again"
        default:
            [NSException raise:@"Invalid progress" format:@"Invalid progress"];
            return @"";
    }
}

- (BOOL)getIfNextButtonIsVisible
{
    switch (self.progress)
    {
        case TutorialAddSampleVideo:
            return YES;
        case TutorialScanButton:
            return NO;
        case TutorialHighlightsFound:
            return NO;
        case TutorialBookmarkFlag:
            return YES;
        case TutorialHandles:
            return YES;
        case TutorialShareButton:
            return NO;
        case TutorialCloseDetails:
            return NO;
        case TutorialStartOver:
            return NO;
        default:
            [NSException raise:@"Invalid progress" format:@"Invalid progress"];
            return YES;
    }
}

- (ArrowDirection)getArrowDirection
{
    switch (self.progress)
    {
        case TutorialAddSampleVideo:
            return Left;
        case TutorialScanButton:
            return Bottom;
        case TutorialHighlightsFound:
            return Left;
        case TutorialBookmarkFlag:
            return Bottom;
        case TutorialHandles:
            return BottomLeft;
        case TutorialShareButton:
            return Bottom;
        case TutorialCloseDetails:
            return TopLeft;
        case TutorialStartOver:
            return Bottom;
        default:
            [NSException raise:@"Invalid progress" format:@"Invalid progress"];
            return Bottom;
    }
}



- (NSSize)getTooltipSize
{
    BOOL isNextButtonVisible = [self getIfNextButtonIsVisible];
    ArrowDirection arrowDirection = [self getArrowDirection];
    
    if (isNextButtonVisible)
    {
        switch (arrowDirection)
        {
            case Bottom:
            case BottomLeft:
                return NSMakeSize(193, 194);
            case Left: return NSMakeSize(203, 184);
            case TopLeft:
            default: NSAssert(false, @"Unaccounted case"); return NSMakeSize(0, 0);
        }
    }
    else
    {
        switch (arrowDirection)
        {
            case Bottom: return NSMakeSize(193, 144);
            case Left: return NSMakeSize(203, 134);
            case TopLeft: return NSMakeSize(193, 144);
            case BottomLeft:
            default: NSAssert(false, @"Unaccounted case"); return NSMakeSize(0, 0);
        }
    }
    
}


- (NSImage*)getTooltipImage
{
    BOOL isNextButtonVisible = [self getIfNextButtonIsVisible];
    ArrowDirection arrowDirection = [self getArrowDirection];
    
    if (isNextButtonVisible)
    {
        switch (arrowDirection)
        {
            case Bottom: return [NSImage imageNamed:@"tooltip-large-bottom.png"];
            case Left: return [NSImage imageNamed:@"tooltip-large-left.png"];
            case BottomLeft: return [NSImage imageNamed:@"tooltip-large-bottomleft.png"];
            case TopLeft:
            default: NSAssert(false, @"Unaccounted case"); return nil;
        }
    }
    else
    {
        switch (arrowDirection)
        {
            case Bottom: return [NSImage imageNamed:@"tooltip-medium-bottom.png"];
            case Left: return [NSImage imageNamed:@"tooltip-medium-left.png"];
            case TopLeft: return [NSImage imageNamed:@"tooltip-medium-topleft.png"];
            case BottomLeft:
            default: NSAssert(false, @"Unaccounted case"); return nil;
        }
    }
}

- (NSPoint)getTextOrigin
{
    BOOL isNextButtonVisible = [self getIfNextButtonIsVisible];
    ArrowDirection arrowDirection = [self getArrowDirection];
    
    if (isNextButtonVisible)
    {
        switch (arrowDirection)
        {
            case Bottom:
            case BottomLeft:
                return NSMakePoint(10, 62);
            case Left:
                return NSMakePoint(20, 62);
            case TopLeft:
            default: NSAssert(false, @"Unaccounted case"); return NSMakePoint(0, 0);
        }
    }
    else
    {
        switch (arrowDirection)
        {
            case Bottom: return NSMakePoint(10, 62);
            case Left: return NSMakePoint(20, 62);
            case TopLeft: return NSMakePoint(8, 52);
            case BottomLeft:
            default: NSAssert(false, @"Unaccounted case"); return NSMakePoint(0, 0);
        }
    }
}

/*
 * This is relative to the tooltip image, not the frame
 */
- (NSPoint)getNextButtonOrigin:(NSRect)tooltipFrame buttonFrame:(NSRect)buttonFrame
{
    NSInteger x = [TutorialHelper getOriginXforCenterPoint:tooltipFrame :buttonFrame];
    NSInteger y = 0;
    
    ArrowDirection arrowDirection = [self getArrowDirection];
    
    switch (arrowDirection)
    {
        case Bottom:
        case BottomLeft:
            y = 30; break;
        case Left: y = 10; break;
        default: NSAssert(false, @"Unaccounted case");
    }
    
    return NSMakePoint(x, y);
}

+ (NSInteger)getOriginXforCenterPoint:(NSRect)anchorFrame :(NSRect)slaveFrame
{
    return anchorFrame.origin.x + ((anchorFrame.size.width - slaveFrame.size.width) / 2);
}

+ (NSString*)getFriendlyProgress:(TutorialProgress)progress
{
    switch (progress)
    {
        case TutorialAddSampleVideo: return @"AddSampleVideo";
        case TutorialScanButton: return @"ScanButton";
        case TutorialHighlightsFound: return @"HighlightsFound";
        case TutorialBookmarkFlag: return @"BookmarkFlag";
        case TutorialHandles: return @"Handles";
        case TutorialShareButton: return @"ShareButton";
        case TutorialCloseDetails: return @"CloseDetails";
        case TutorialStartOver: return @"StartOver";
        case TutorialFinished: return @"Finished";
        default: NSAssert(false, @"Unaccounted case");
    }
}


@end

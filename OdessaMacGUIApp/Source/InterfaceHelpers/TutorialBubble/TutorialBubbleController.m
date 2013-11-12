//
//  TutorialBubbleController.m
//  OdessaMacGUIApp
//
//  Created by Noah Spitzer-Williams on 12/28/12.
//  Copyright (c) 2012 Authentically Digital LLC. All rights reserved.
//

#import "TutorialBubbleController.h"

#if DEBUG
static const int ddLogLevel = LOG_LEVEL_VERBOSE | LOG_LEVEL_INFO | LOG_LEVEL_ERROR | LOG_LEVEL_WARN;
#else
static const int ddLogLevel = LOG_LEVEL_INFO | LOG_LEVEL_ERROR | LOG_LEVEL_WARN;
#endif


@interface TutorialBubbleController ()

@property (strong) TutorialHelper* tutorialHelper;

@property (unsafe_unretained) BOOL awoken;

@end


@implementation TutorialBubbleController

- (id)initWithNibName:(NSString *)nibNameOrNil bundle:(NSBundle *)nibBundleOrNil
{
    self = [super initWithNibName:nibNameOrNil bundle:nibBundleOrNil];
    if (self) {
        // Initialization code here.

    }
    
    return self;
}

- (void)awakeFromNib
{ // for some reason this is called before initWithNibName
    
    DDLogVerbose(@"called");
    
}

- (void)loadTooltip
{
    self.tutorialHelper = [TutorialHelper new];
    if (!self.tutorialHelper)
        return;
    
    [self setTooltipImage];
    [self setBubbleText];
    [self setIsNextButtonVisible];
    [self setTextAlignment];
}

- (void)setBubbleText
{
    NSAssert(self.tutorialHelper, @"tutorialHelper is nil");
    if (!self.tutorialHelper)
        return;

    [self.bubbleTextField setStringValue:[self.tutorialHelper getText]];
}

- (void)setIsNextButtonVisible
{
    NSAssert(self.tutorialHelper, @"tutorialHelper is nil");
    if (!self.tutorialHelper)
        return;

    [self.nextButton setHidden:![self.tutorialHelper getIfNextButtonIsVisible]];
    
    [self.nextButton setHoverImage:[NSImage imageNamed:@"next-button-hover.png"]];
}

- (void)setTooltipImage
{
    NSAssert(self.tutorialHelper, @"tutorialHelper is nil");
    if (!self.tutorialHelper)
        return;

    NSImage* image = [self.tutorialHelper getTooltipImage];
    
    NSAssert(image, @"image is nil");
    NSAssert(self.tooltipImage, @"tooltipImage is nil");
    if (!image)
        return;
    
    DDLogVerbose(@"frame: %fx%f", self.view.frame.size.width, self.view.frame.size.height);
    DDLogVerbose(@"image: %fx%f", image.size.width, image.size.height);

    [self.tooltipImage setImage:image];
    
    [self.tooltipImage setFrameSize:image.size];

    
    // align to the top of frame
    NSPoint bounds = NSMakePoint(0, self.view.frame.size.height - image.size.height);
    [self.tooltipImage setFrameOrigin:bounds];
    
    if ([self.tutorialHelper getIfNextButtonIsVisible])
    {
        NSPoint nextButtonFrameOrigin = [self.tutorialHelper getNextButtonOrigin:self.tooltipImage.frame buttonFrame:self.nextButton.frame];
        NSInteger offset = self.view.frame.size.height - image.size.height; // account for difference between tooltip frame and view frame (stupid hack)
        nextButtonFrameOrigin.y += offset;
        [self.nextButton setFrameOrigin:nextButtonFrameOrigin];
    }
}

- (void)setTextAlignment
{
    NSAssert(self.tutorialHelper, @"tutorialHelper is nil");
    if (!self.tutorialHelper)
        return;

    [self.bubbleTextField setFrameOrigin:[self.tutorialHelper getTextOrigin]];
}

- (IBAction)takeAction:(id)sender {
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Warc-performSelector-leaks"
    [self.actionTarget performSelector:self.actionSelector];
#pragma clang diagnostic pop
}

@end

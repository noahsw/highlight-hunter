//
//  InputVideoThumbnailViewController.m
//  OdessaMacGUIApp
//
//  Created by Noah Spitzer-Williams on 9/17/12.
//  Copyright (c) 2012 Authentically Digital LLC. All rights reserved.
//

#import "InputVideoThumbnailViewController.h"

#if DEBUG
static const int ddLogLevel = LOG_LEVEL_VERBOSE | LOG_LEVEL_INFO | LOG_LEVEL_ERROR | LOG_LEVEL_WARN;
#else
static const int ddLogLevel = LOG_LEVEL_INFO | LOG_LEVEL_ERROR | LOG_LEVEL_WARN;
#endif

@interface InputVideoThumbnailViewController ()

@end

@implementation InputVideoThumbnailViewController

- (id)initWithNibName:(NSString *)nibNameOrNil bundle:(NSBundle *)nibBundleOrNil
{
    self = [super initWithNibName:nibNameOrNil bundle:nibBundleOrNil];
    if (self) {
        // Initialization code here.
    }
    
    return self;
}

- (void)awakeFromNib
{
    [self.browseComputerButton setTextColor:[DesignLanguage midGrayColor]];
    
    [self.removeButton setHoverImage:[NSImage imageNamed:@"review-video-hover-deleteicon.png"]];
    
    [self createTrackingArea];

}

- (void)setRepresentedObject:(id)representedObject
{
    if (representedObject == nil)
        return;
    
    // See whether we should hide remove button because it's during tutorial and it's sample video
    TutorialProgress progress = [TutorialHelper getProgress];
    if ((progress == TutorialAddSampleVideo || progress == TutorialScanButton))
    {
        InputFileObject* inputFileObject = (InputFileObject*)representedObject;
        DDLogVerbose(@"%@", inputFileObject.sourceURL.path);
        if (inputFileObject != nil && [inputFileObject.sourceURL.path isEqualToString:[MainModel getURLToSampleVideo].path] == YES)
            inputFileObject.removeButtonAlwaysHidden = YES;
    }
    
    
    [super setRepresentedObject:representedObject];
}


- (void)createTrackingArea
{
    NSTrackingAreaOptions focusTrackingAreaOptions = NSTrackingActiveInActiveApp;
    focusTrackingAreaOptions |= NSTrackingMouseEnteredAndExited;
    focusTrackingAreaOptions |= NSTrackingAssumeInside;
    focusTrackingAreaOptions |= NSTrackingInVisibleRect;
    
    NSTrackingArea *focusTrackingArea = [[NSTrackingArea alloc] initWithRect:NSZeroRect options:focusTrackingAreaOptions owner:self userInfo:nil];
    [self.view addTrackingArea:focusTrackingArea];
    
    
}

- (IBAction)browseComputer:(id)sender {
    [self.genericDropHereDelegate browseComputer];
}

- (IBAction)removeVideo:(id)sender {
    InputFileObject* inputFileObject = (InputFileObject*)self.representedObject;
    [self.removeInputFileDelegate removeInputFile:inputFileObject];
}

- (void)smallDropHereDragEntered
{
    [self.dropImage setImage:[NSImage imageNamed:@"select-droptarget-small-active.png"]];
}

- (void)smallDropHereDragExited
{
    [self.dropImage setImage:[NSImage imageNamed:@"select-droptarget-small.png"]];
}

- (void)mouseEntered:(NSEvent *)theEvent
{
    InputFileObject* inputFileObject = (InputFileObject*)self.representedObject;
    if (inputFileObject.isSmallDropHereControl)
        return;
    
    if (!inputFileObject.removeButtonAlwaysHidden)
        inputFileObject.showRemoveButton = YES;


}

- (void)mouseExited:(NSEvent *)theEvent
{
    InputFileObject* inputFileObject = (InputFileObject*)self.representedObject;
    inputFileObject.showRemoveButton = NO;
}

@end

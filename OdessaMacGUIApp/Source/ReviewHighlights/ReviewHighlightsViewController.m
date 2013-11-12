//
//  ReviewHighlightsView.m
//  OdessaMacGUIApp
//
//  Created by Noah Spitzer-Williams on 8/28/12.
//  Copyright (c) 2012 Authentically Digital LLC. All rights reserved.
//

#import "ReviewHighlightsViewController.h"

#if DEBUG
static const int ddLogLevel = LOG_LEVEL_VERBOSE | LOG_LEVEL_INFO | LOG_LEVEL_ERROR | LOG_LEVEL_WARN;
#else
static const int ddLogLevel = LOG_LEVEL_INFO | LOG_LEVEL_ERROR | LOG_LEVEL_WARN;
#endif


@interface ReviewHighlightsViewController ()

@property (strong) NSMutableArray* reviewItems;

@end


@implementation ReviewHighlightsViewController

- (id)initWithNibName:(NSString *)nibNameOrNil bundle:(NSBundle *)nibBundleOrNil
{
    self = [super initWithNibName:nibNameOrNil bundle:nibBundleOrNil];
    if (self) {
        DDLogInfo(@"called");
    }
    
    return self;
}


- (void)awakeFromNib
{
    DDLogInfo(@"called");
    
    [self.highlightsCollectionView setMainModel:self.mainModel];
    self.reviewItems = [[NSMutableArray alloc] init];
    for (InputFileObject* inputFileObject in self.mainModel.inputFileObjects)
    {
        if (inputFileObject.isSmallDropHereControl)
            continue;
        
        [self.reviewItems addObject:inputFileObject];
        
        BOOL isHighlightFound = NO;
        for (HighlightObject* highlightObject in self.mainModel.highlightObjects)
        {
            if (highlightObject.inputFileObject == inputFileObject)
            {
                isHighlightFound = YES;
                [self.reviewItems addObject:highlightObject];
            }
        }
        
        if (!isHighlightFound)
        {
            NoHighlightsObject* noHighlightsObject = [NoHighlightsObject new];
            noHighlightsObject.inputFileObject = inputFileObject;
            [self.reviewItems addObject:noHighlightsObject];
        }
    }
    
    
	[self.highlightsCollectionView setContent:self.reviewItems];
    [self.highlightsCollectionView setDrawsBackground:NO];
    [self.highlightsCollectionView setHighlightDetailsDelegate:self];
    [self.highlightsCollectionView setTutorialEscapeHatchDelegate:self];
    [self.highlightsCollectionView setNoHighlightsDelegate:self];
    
    DDLogInfo(@"highlight Count: %ld", self.mainModel.highlightObjects.count);
    
    [self initializeSaveAllButton];
    [self initializeExportButton];
    [self initializeStartOverButton];
    
#ifdef APPSTORE_PREMIUM
    [self.proButton setHidden:YES];
#endif
    
    [self initializeHighlightsFoundTutorialBubble];
    [self initializeStartOverTutorialBubble];
    
}

- (void)initializeSaveAllButton
{
    self.saveAllAsVideosButtonController = [_saveAllAsVideosButtonController initWithNibName:@"IconButton" bundle:nil];
    
    [_saveAllAsVideosButton addSubview:[_saveAllAsVideosButtonController view]];
    
    [[self.saveAllAsVideosButtonController view] setFrameSize:self.saveAllAsVideosButton.bounds.size];
    
    [self.saveAllAsVideosButtonController setIcon:[NSImage imageNamed:@"icons-16px-saveall.png"]];
    
    [self.saveAllAsVideosButtonController setButtonText:@"Save all as videos..."];
    
    [self.saveAllAsVideosButtonController setActionTarget:self];
    [self.saveAllAsVideosButtonController setActionSelector:@selector(saveAllAsVideos)];
    
}

- (void)initializeExportButton
{
    
#ifdef APPSTORE_FREE
    [self.exportButton setHidden:YES];
    [self.proButton setHidden:YES];
    return;
#endif
    
    
    self.exportButtonController = [_exportButtonController initWithNibName:@"IconButton" bundle:nil];
    
    [_exportButton addSubview:[_exportButtonController view]];
    
    [[self.exportButtonController view] setFrameSize:self.exportButton.bounds.size];
    
    [self.exportButtonController setIcon:[NSImage imageNamed:@"icons-16px-open-in-software.png"]];
    
    [self.exportButtonController setButtonText:@"Save as Final Cut Pro X project..."];
    
    [self.exportButtonController setActionTarget:self];
    [self.exportButtonController setActionSelector:@selector(exportHighlights)];
    
}

- (void)initializeStartOverButton
{
    self.startOverButtonController = [_startOverButtonController initWithNibName:@"IconButton" bundle:nil];
    
    [_startOverButton addSubview:[_startOverButtonController view]];
    
    [[self.startOverButtonController view] setFrameSize:self.startOverButton.bounds.size];
    
    [self.startOverButtonController setIcon:[NSImage imageNamed:@"icons-16px-startover.png"]];
    
    [self.startOverButtonController setButtonText:@"Start over..."];
    
    [self.startOverButtonController setActionTarget:self];
    [self.startOverButtonController setActionSelector:@selector(startOver)];
    
}

- (void)initializeHighlightsFoundTutorialBubble
{
    TutorialProgress progress = [TutorialHelper getProgress];
    if (progress != TutorialHighlightsFound)
    {
        DDLogVerbose(@"Skipping TutorialHighlightsFound tutorial. Progress = %d", progress);
        return;
    }
    
    self.highlightsFoundTutorialBubbleController = [_highlightsFoundTutorialBubbleController initWithNibName:@"TutorialBubble" bundle:nil];
    
    [_highlightsFoundTutorialBubble addSubview:[_highlightsFoundTutorialBubbleController view]];
    
    [self.highlightsFoundTutorialBubbleController loadTooltip];
    
    [[self.highlightsFoundTutorialBubbleController view] setFrameSize:self.highlightsFoundTutorialBubble.bounds.size];
    
    [self.highlightsFoundTutorialBubbleController setActionTarget:self];
    [self.highlightsFoundTutorialBubbleController setActionSelector:@selector(advanceTutorialPastHighlightsFoundAndOpenDetails)];
    
    
}

- (void)initializeStartOverTutorialBubble
{
    TutorialProgress progress = [TutorialHelper getProgress];
    if (progress != TutorialStartOver)
    {
        DDLogVerbose(@"Skipping TutorialStartOver tutorial. Progress = %d", progress);
        return;
    }
    
    self.startOverTutorialBubbleController = [_startOverTutorialBubbleController initWithNibName:@"TutorialBubble" bundle:nil];
    
    [_startOverTutorialBubble addSubview:[_startOverTutorialBubbleController view]];
    
    [self.startOverTutorialBubbleController loadTooltip];

    [[self.startOverTutorialBubbleController view] setFrameSize:self.startOverTutorialBubble.bounds.size];
    
    [self.startOverTutorialBubbleController setActionTarget:self];
    [self.startOverTutorialBubbleController setActionSelector:@selector(advanceTutorialPastStartOver)];
    
    
}

- (void)refreshTutorialProgress
{
    [self.tutorialEscapeHatchDelegate refreshTutorialProgress];
}


- (void)advanceTutorialPastHighlightsFound
{
    [self.highlightsFoundTutorialBubble setHidden:YES];
    
    [TutorialHelper advanceProgress];
    
    [self.tutorialEscapeHatchDelegate refreshTutorialProgress];
    
}

- (void)advanceTutorialPastHighlightsFoundAndOpenDetails
{
    [self advanceTutorialPastHighlightsFound];
    
    if (self.mainModel.highlightObjects.count == 0)
        return;
    
    [self.highlightsCollectionView openHighlightDetails:self.mainModel.highlightObjects[0]];

}


- (void)advanceTutorialPastStartOver
{
    [self.startOverTutorialBubble setHidden:YES];
    
    [TutorialHelper advanceProgress];
    
    [self.tutorialEscapeHatchDelegate exitTutorial]; // this is the last step!

    [self startOver];
    
}


- (void)hideTutorialBubbles
{
    if (self.highlightsFoundTutorialBubble)
        [self.highlightsFoundTutorialBubble setHidden:YES];
    
    if (self.startOverTutorialBubble)
        [self.startOverTutorialBubble setHidden:YES];
    
    if (self.highlightsCollectionView && self.highlightsCollectionView.highlightDetailsWindowController)
        [self.highlightsCollectionView.highlightDetailsWindowController hideTutorialBubbles];
}






- (void)saveAllAsVideos
{
    
    NSString* errorMsg = @"Unfortunately, we must prompt you to save each highlight individually.";
    NSAlert *alert = [[NSAlert alloc] init];
    [alert addButtonWithTitle:@"Continue saving"];
    [alert addButtonWithTitle:@"Cancel"];
    [alert setMessageText:@"Continue?"];
    [alert setInformativeText:errorMsg];
    [alert setAlertStyle:NSWarningAlertStyle];
    NSInteger returnCode = [alert runModalSheetForWindow:[self window]];
    if (returnCode == NSAlertSecondButtonReturn)
    {
        return;
    }
    
    
    NSUserDefaults *standardUserDefaults = [NSUserDefaults standardUserDefaults];

    // figure out output format
    NSInteger outputFormat = OutputFormatOriginal;
    if (standardUserDefaults)
    {
        NSString* outputFormatSetting = [standardUserDefaults stringForKey:@"SaveOutputFormat"];
        if ([outputFormatSetting isEqualToString:@"ProRes"])
            outputFormat = OutputFormatProRes;
    }
    
    

    NSOperationQueue* operationQueue = [NSOperationQueue new];
    for (HighlightObject* highlightObject in self.mainModel.highlightObjects)
    {
        
        NSSavePanel* savePanel = [NSSavePanel savePanel];
        savePanel.message = @"Where should we save your highlight?";
        
        if (standardUserDefaults)
        {
            savePanel.directoryURL = [NSURL fileURLWithPath:[standardUserDefaults objectForKey:@"LastSavedPath"]];
        }
        else
            savePanel.directoryURL = [MainModel getDefaultFolderURL];
        
        savePanel.allowsOtherFileTypes = NO;
        
        NSString* defaultFileExtension;
        if (outputFormat == OutputFormatOriginal)
        {
            defaultFileExtension = highlightObject.inputFileObject.sourceURL.lastPathComponent.pathExtension;
        }
        else
        {
            defaultFileExtension = @"mov";
        }
        savePanel.allowedFileTypes = @[defaultFileExtension];
        
        NSString* defaultFileName = [NSString stringWithFormat:@"%@.%@", highlightObject.title, defaultFileExtension];
        savePanel.nameFieldStringValue = defaultFileName;
        
        DDLogInfo(@"defaultFileName: %@", defaultFileName);
        
        NSInteger result = [savePanel runModal];
        
        // can't use beginSheet because the code isn't blocked
//        [savePanel beginSheetModalForWindow:self.window completionHandler:^(NSInteger result) {
            if (result == NSFileHandlingPanelOKButton) {
                
                [standardUserDefaults setObject:[MainModel getParentFolder:savePanel.URL].path forKey:@"LastSavedPath"];
                [standardUserDefaults synchronize];
                
                DDLogInfo(@"Saved path: %@", savePanel.URL.path);
                
                highlightObject.title = savePanel.URL.lastPathComponent.stringByDeletingPathExtension;
                
                if (highlightObject.saveWorker != nil)
                    highlightObject.saveWorker = nil;
                
                highlightObject.saveWorker = [SaveWorker workerWithHighlight:highlightObject];
                highlightObject.saveWorker.outputFileURL = savePanel.URL;
                highlightObject.saveWorker.outputFormat = outputFormat;
                
                [operationQueue addOperation:highlightObject.saveWorker];
            }
 //       }];
        

    }
    
}


- (void)exportHighlights
{

#ifdef APPSTORE_FREE
    [BrowserHelper launchPurchasingOptions:@"export"];
    return;
#endif
    
    FinalCutProXExportWorker* finalCutProXExportWorker = [FinalCutProXExportWorker workerWithHighlights:self.mainModel.highlightObjects];
    
    [finalCutProXExportWorker exportHighlights:self.window];
    
    
}



- (void)startOver
{
    
    TutorialProgress progress = [TutorialHelper getProgress];
    if (progress == TutorialStartOver)
    {
        [self advanceTutorialPastStartOver];
        return; // so we don't call startOver twice
    }

    
    // see if we're mid publish
    if ([self isPublishing])
    {
        NSAlert *alert = [[NSAlert alloc] init];
        NSString* errorMsg = @"Highlight Hunter is busy working for you. Would you like to stop publishing and start over?";
        [alert addButtonWithTitle:@"Start over"];
        [alert addButtonWithTitle:@"Cancel"];
        [alert setMessageText:@"Stop publishing?"];
        [alert setInformativeText:errorMsg];
        [alert setAlertStyle:NSWarningAlertStyle];
        NSInteger returnCode = [alert runModalSheetForWindow:self.window];
        if (returnCode == NSAlertSecondButtonReturn)
        { // cancel button
            return;
        }
        
        // stop publishing
        [self cancelPublish];

    }
    
    
    [self.mainModel.highlightObjects removeAllObjects];
    [self.mainModel.inputFileObjects removeAllObjects];
    
    [self.reviewHighlightsDelegate startOver];
    
}


- (bool)isPublishing
{
    for (HighlightObject* highlightObject in self.mainModel.highlightObjects)
    {
        if (highlightObject.saveWorker != nil && highlightObject.saveWorker.isExecuting)
            return true;
        
        if (highlightObject.facebookShareWorker != nil && highlightObject.facebookShareWorker.isExecuting)
            return true;
    }
    
    return false;
}


- (void)cancelPublish
{
    for (HighlightObject* highlightObject in self.mainModel.highlightObjects)
    {
        if (highlightObject.saveWorker != nil && highlightObject.saveWorker.isExecuting)
            [highlightObject.saveWorker cancelWorker];
        
        if (highlightObject.facebookShareWorker != nil && highlightObject.facebookShareWorker.isExecuting)
            [highlightObject.facebookShareWorker cancelWorker];
    }
    
    // wait for them to finish
    while ([self isPublishing]) { }
}

- (IBAction)openProUpsell:(id)sender {
    [BrowserHelper launchBrowser:@"appstore-redirect.php" term:@"export" addDomain:YES];
}


- (void)removeHighlight:(HighlightObject *)highlightObject
{
    [self.mainModel.highlightObjects removeObject:highlightObject];
    
    [self.reviewItems removeObject:highlightObject];
    
    [self.highlightsCollectionView setContent:self.reviewItems]; // refresh view
    
}


- (void)rescanRequested
{
    [self.reviewHighlightsDelegate rescanRequested];
}



@end
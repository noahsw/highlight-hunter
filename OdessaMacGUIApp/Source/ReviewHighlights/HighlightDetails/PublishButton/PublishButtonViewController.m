//
//  PublishButtonView.m
//  OdessaMacGUIApp
//
//  Created by Noah Spitzer-Williams on 9/4/12.
//  Copyright (c) 2012 Authentically Digital LLC. All rights reserved.
//

#import "PublishButtonViewController.h"

#if DEBUG
static const int ddLogLevel = LOG_LEVEL_VERBOSE | LOG_LEVEL_INFO | LOG_LEVEL_ERROR | LOG_LEVEL_WARN;
#else
static const int ddLogLevel = LOG_LEVEL_INFO | LOG_LEVEL_ERROR | LOG_LEVEL_WARN;
#endif


@interface PublishButtonViewController ()

@property (unsafe_unretained) PublishWorker* publishWorker;

@property NSInteger workerState;

enum {
    WorkerStateInactive,
    WorkerStateWorking,
    WorkerStateCompleted,
    WorkerStateError
};

@end



@implementation PublishButtonViewController

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
    self.labelButton.textColor = [DesignLanguage lightGrayColor];
    
    [self.cancelButton setHoverImage:[NSImage imageNamed:@"review-video-hover-deleteicon.png"]];
    [self.cancelButton createTrackingArea];
    
    [self.publishButtonView setPublishButtonDelegate:self];
}

- (HighlightObject *)highlightObject
{
    return highlightObject;
}

- (void)setHighlightObject:(HighlightObject *)newHighlightObject
{
    highlightObject = newHighlightObject;
    
    switch (self.publishWorkerType)
    {
        case PublishWorkerTypeFacebook:
        {
            self.publishWorker = highlightObject.facebookShareWorker;
            [self.iconButton setImage:[NSImage imageNamed:@"icons-42px-facebook.png"]];
            break;
        }
            
        case PublishWorkerTypeSave:
        {
            self.publishWorker = highlightObject.saveWorker;
            [self.iconButton setImage:[NSImage imageNamed:@"icons-42px-savetocomputer.png"]];
            break;
        }
    }
    
    // if we're already scanning or saving, let's see what the progress is
    if (self.publishWorker != nil)
    {
        [self.publishWorker setWorkerProgressDelegateForDetails:self];
    }
    
    [self updateInterface];
}


- (IBAction)takeAction:(id)sender {
    
    if (self.publishWorker == nil)
    {
        [self publish];
    }
    else
    {
        switch (self.publishWorker.publishWorkerResult)
        {
            case PublishWorkerResultCancelled:
            case PublishWorkerResultUnableToAuthenticate:
            {
                [self publish];
                break;
            }
                
            case PublishWorkerResultNotFinished:
            {
                // Do nothing. User must press Cancel button to cancel
                break;
            }
                
            case PublishWorkerResultSuccess:
            {
                if (self.publishWorkerType == PublishWorkerTypeFacebook)
                {
                    [FacebookShareWorker handleEncodingWarning:self.window];
                }
                
                [self.publishWorker viewResult];
                
                [AppStoreRating handleAppStoreRatingPrompt:self.window];
                
                break;
            }
                
            case PublishWorkerResultUnableToShare:
            case PublishWorkerResultUnableToSplice:
            {
                NSString* errorMsg = [NSString stringWithFormat:@"Bummer -- we ran into some trouble. Here's the error message:\n\n%@\n\nWould you like to try again?", self.publishWorker.errorMessage];
                NSAlert *alert = [[NSAlert alloc] init];
                [alert addButtonWithTitle:@"Close"];
                [alert addButtonWithTitle:@"Try again"];
                [alert setMessageText:@"Error"];
                [alert setInformativeText:errorMsg];
                [alert setAlertStyle:NSWarningAlertStyle];
                NSInteger returnCode = [alert runModalSheetForWindow:self.window];
                if (returnCode == NSAlertSecondButtonReturn)
                { // cancel button
                    [self publish];
                }
                
            }
        }
    }
    
}

- (IBAction)cancelPublish:(id)sender {
    
    NSString* question;
    
    switch (self.publishWorkerType)
    {
        case PublishWorkerTypeFacebook:
            question = @"Stop uploading to Facebook?";
            break;
            
        case PublishWorkerTypeSave:
            question = @"Stop saving highlight clip?";
            break;
            
        default:
            NSAssert(false, @"Unaccounted for PublishWorkerType");
            break;
    }
    

    NSAlert *alert = [[NSAlert alloc] init];
    [alert addButtonWithTitle:@"No"];
    [alert addButtonWithTitle:@"Yes, stop"];
    [alert setMessageText:question];
    [alert setAlertStyle:NSWarningAlertStyle];
    NSInteger returnCode = [alert runModalSheetForWindow:self.window];
    if (returnCode == NSAlertSecondButtonReturn)
    {
        if (self.publishWorker != nil && self.publishWorker.isExecuting)
            [self.publishWorker cancelWorker];
    }

}

- (void)publish
{
    
    if (self.publishWorkerType == PublishWorkerTypeFacebook)
        [self shareHighlightToFacebook];
    else if (self.publishWorkerType == PublishWorkerTypeSave)
        [self saveHighlight];
    else
        NSAssert(false, @"Unaccounted for case in publish()");
  
}

- (void)shareHighlightToFacebook
{
    TutorialProgress progress = [TutorialHelper getProgress];
    if (progress == TutorialShareButton)
    { // don't actually share
        DDLogInfo(@"Simulating facebook sharing due to tutorial");
        [self.shareButtonTutorialDelegate advanceTutorialPastShareButton];
        return;
    }

    
    if (self.highlightObject.facebookShareWorker != nil)
        self.highlightObject.facebookShareWorker = nil;
    
    self.highlightObject.facebookShareWorker = [FacebookShareWorker workerWithHighlight:self.highlightObject];
    
    
    self.publishWorker = self.highlightObject.facebookShareWorker;
    
    [self startWorker];
    
}


- (void)saveHighlight
{
    
    [self showWorking:@"Saving as video file..."]; // show this so user sees response from click
    
    NSSavePanel* savePanel = [NSSavePanel savePanel];
    savePanel.message = @"Where should we save your highlight?";
    
    NSUserDefaults *standardUserDefaults = [NSUserDefaults standardUserDefaults];
    if (standardUserDefaults)
    {
        savePanel.directoryURL = [NSURL fileURLWithPath:[standardUserDefaults objectForKey:@"LastSavedPath"]];
    }
    else
        savePanel.directoryURL = [MainModel getDefaultFolderURL];
    
    // figure out output format
    NSInteger outputFormat = OutputFormatOriginal;
    if (standardUserDefaults)
    {
        NSString* outputFormatSetting = [standardUserDefaults stringForKey:@"SaveOutputFormat"];
        if ([outputFormatSetting isEqualToString:@"ProRes"])
            outputFormat = OutputFormatProRes;
    }
    
    savePanel.allowsOtherFileTypes = NO;
    
    NSString* defaultFileExtension;
    if (outputFormat == OutputFormatOriginal)
    {
        defaultFileExtension = self.highlightObject.inputFileObject.sourceURL.lastPathComponent.pathExtension;
        // don't allow other file types
        //            savePanel.allowedFileTypes = @[@"public.movie", @"MTS", @"mts", @"M2TS", @"m2ts"];
    }
    else
    {
        defaultFileExtension = @"mov";
    }
    savePanel.allowedFileTypes = @[defaultFileExtension];
    
    NSString* defaultFileName = [NSString stringWithFormat:@"%@.%@", self.highlightObject.title, defaultFileExtension];
    savePanel.nameFieldStringValue = defaultFileName;
    
    DDLogInfo(@"defaultFileName: %@", defaultFileName);
    
    //if ([savePanel runModal] == NSFileHandlingPanelCancelButton)
    //    return;
    
    [savePanel beginSheetModalForWindow:self.window completionHandler:^(NSInteger result) {
        if (result == NSFileHandlingPanelOKButton) {
            
            [standardUserDefaults setObject:[MainModel getParentFolder:savePanel.URL].path forKey:@"LastSavedPath"];
            [standardUserDefaults synchronize];
            
            DDLogInfo(@"Saved path: %@", savePanel.URL.path);
            
            self.highlightObject.title = savePanel.URL.lastPathComponent.stringByDeletingPathExtension;
            
            if (self.highlightObject.saveWorker != nil)
                self.highlightObject.saveWorker = nil;
            
            self.highlightObject.saveWorker = [SaveWorker workerWithHighlight:self.highlightObject];
            NSLog(@"%@", savePanel.URL.path);
            self.highlightObject.saveWorker.outputFileURL = savePanel.URL;
            self.highlightObject.saveWorker.outputFormat = outputFormat;
            self.publishWorker = self.highlightObject.saveWorker;
            
            [self startWorker];
        }
        else
        {
            [self updateInterface];
        }
    }];

}

- (void)startWorker
{
    DDLogInfo(@"Starting publishWorker");
    
    [self.publishWorker setWorkerProgressDelegateForDetails:self];
    
    NSOperationQueue* operationQueue = [NSOperationQueue new];
    [operationQueue addOperation:self.publishWorker];
    
    [self updateInterface];

    
}

- (void)updateInterface
{
    if (self.publishWorkerType == PublishWorkerTypeFacebook)
    {
        if (self.publishWorker == nil)
        {
            [self showNotStarted:@"Share highlight to facebook"];
        }
        else
        {
            switch (self.publishWorker.publishWorkerResult)
            {
                case PublishWorkerResultCancelled:
                {
                    [self showCancelled:@"Cancelled sharing (click to retry)"];
                    break;
                }
                    
                case PublishWorkerResultNotFinished:
                {
                    if (self.publishWorker.progress == 0)
                        [self showWorking:@"Logging into facebook..."];
                    else
                        [self showWorking:[NSString stringWithFormat:@"Sharing to facebook (%ld%%)...", self.publishWorker.progress]];
                    break;
                }
                    
                case PublishWorkerResultSuccess:
                {
                    [self showCompleted:@"Shared to facebook (click to view)"];
                    break;
                }
                    
                case PublishWorkerResultUnableToAuthenticate:
                {
                    [self showError:@"Click to login to facebook"];
                    break;
                }
                    
                case PublishWorkerResultUnableToShare:
                case PublishWorkerResultUnableToSplice:
                {
                    [self showError:@"Error sharing (details...)"];
                    break;
                }
            }
        }
    }
    else if (self.publishWorkerType == PublishWorkerTypeSave)
    {
        if (self.publishWorker == nil)
        {
            [self showNotStarted:@"Save highlight as video file"];
        }
        else
        {
            switch (self.publishWorker.publishWorkerResult)
            {
                case PublishWorkerResultCancelled:
                {
                    [self showCancelled:@"Cancelled saving (click to retry)"];
                    break;
                }
                    
                case PublishWorkerResultNotFinished:
                {
                    [self showWorking:[NSString stringWithFormat:@"Saving as video file (%ld%%)...", self.publishWorker.progress]];
                    break;
                }
                    
                case PublishWorkerResultSuccess:
                {
                    [self showCompleted:@"Saved as video file (click to view)"];
                    break;
                }
                    
                case PublishWorkerResultUnableToSplice:
                {
                    [self showError:@"Error saving (details...)"];
                    break;
                }
                    
                default:
                {
                    NSAssert(true, @"Case not implemented!");
                    break;
                }
            }
        }
    }


}


- (void)workerProgressUpdated:(NSInteger)newProgress
{
    [self updateInterface];
}


- (void)detachWorkerProgressDelegate
{
    if (self.publishWorker != nil)
        self.publishWorker.workerProgressDelegateForDetails = nil;
}


- (void)showNotStarted:(NSString*)label
{
    self.labelButton.title = label;
    self.labelButton.textColor = [DesignLanguage lightGrayColor];
    self.backgroundButton.image = [NSImage imageNamed:@"editpanel-share-element.png"];
    self.workerState = WorkerStateInactive;
    [self.cancelButton setHidden:YES];
}

- (void)showWorking:(NSString*)label
{
    self.labelButton.title = label;
    self.labelButton.textColor = [DesignLanguage lightGrayColor];
    self.backgroundButton.image = [NSImage imageNamed:@"editpanel-share-element-blue.png"];
    self.workerState = WorkerStateWorking;
    [self.cancelButton setHidden:NO];
}

- (void)showCompleted:(NSString*)label
{
    self.labelButton.title = label;
    self.labelButton.textColor = [DesignLanguage lightGrayColor];
    self.backgroundButton.image = [NSImage imageNamed:@"editpanel-share-element-green.png"];
    self.workerState = WorkerStateCompleted;
    [self.cancelButton setHidden:YES];
}

- (void)showCancelled:(NSString*)label
{
    self.labelButton.title = label;
    self.labelButton.textColor = [DesignLanguage lightGrayColor];
    self.backgroundButton.image = [NSImage imageNamed:@"editpanel-share-element.png"];
    self.workerState = WorkerStateInactive;
    [self.cancelButton setHidden:YES];
}

- (void)showError:(NSString*)label
{
    self.labelButton.title = label;
    self.labelButton.textColor = [DesignLanguage lightGrayColor];
    self.backgroundButton.image = [NSImage imageNamed:@"editpanel-share-element-red.png"];
    self.workerState = WorkerStateError;
    [self.cancelButton setHidden:YES];
}

- (void)showHover
{
    switch (self.workerState)
    {
        case WorkerStateInactive:
            self.backgroundButton.image = [NSImage imageNamed:@"editpanel-share-element-hover.png"];
            break;
        case WorkerStateWorking:
            // this state isn't clickable so we don't show hover
            break;
        case WorkerStateCompleted:
            self.backgroundButton.image = [NSImage imageNamed:@"editpanel-share-element-green-hover.png"];
            break;
        case WorkerStateError:
            self.backgroundButton.image = [NSImage imageNamed:@"editpanel-share-element-red-hover.png"];
            break;
    }
}

- (void)showResting
{
    switch (self.workerState)
    {
        case WorkerStateInactive:
            self.backgroundButton.image = [NSImage imageNamed:@"editpanel-share-element.png"];
            break;
        case WorkerStateWorking:
            // this state isn't clickable so we don't show hover
            break;
        case WorkerStateCompleted:
            self.backgroundButton.image = [NSImage imageNamed:@"editpanel-share-element-green.png"];
            break;
        case WorkerStateError:
            self.backgroundButton.image = [NSImage imageNamed:@"editpanel-share-element-red.png"];
            break;
    }
}

@end

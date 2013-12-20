//
//  AppController.m
//  OdessaMacGUIApp
//
//  Created by Noah Spitzer-Williams on 8/28/12.
//  Copyright (c) 2012 Authentically Digital LLC. All rights reserved.
//

#import "AppController.h"


#if DEBUG
static const int ddLogLevel = LOG_LEVEL_VERBOSE | LOG_LEVEL_INFO | LOG_LEVEL_ERROR | LOG_LEVEL_WARN;
#else
static const int ddLogLevel = LOG_LEVEL_INFO | LOG_LEVEL_ERROR | LOG_LEVEL_WARN;
#endif

@implementation AppController


static AppController *sharedAppController;

enum
{
    kSelectViewTag = 0,
    kScanViewTag,
    kReviewViewTag
};

NSString *const kSelectView = @"SelectInputVideosView";
NSString *const kScanView = @"ScanVideosView";
NSString *const kReviewView = @"ReviewHighlightsView";

- (id)init
{
    if (sharedAppController) {
        return sharedAppController;
    }
    
    self = [super init];
    if (self != nil)
    {
        sharedAppController = self;
        
        
        [self initializeLogger];
        
        [self setDefaultSettings];
        
        self.mainModel = [[MainModel alloc] init];
        DDLogInfo(@"MainModel loaded");
        
        
        NSDictionary* infoDict = [[NSBundle mainBundle] infoDictionary];
        DDLogInfo(@"Odessa version: %@ %@", [infoDict objectForKey:@"CFBundleShortVersionString"], [infoDict objectForKey:@"CFBundleVersion"]);
        
        
#ifndef APPSTORE
        // activate licensing
        [Protection init];
#endif
        
        // Initialize Odessa engine
        NSString* logpath = [NSString stringWithFormat:@"%@/%@", self.fileLogger.logFileManager.logsDirectory, @"engine.txt"];
        Initialize(logpath.UTF8String);
        DDLogInfo(@"Engine initialized");
        
        DDLogInfo(@"Done initializing");

    }
    
    return self;

}


+ (id)sharedAppController;
{
    if (!sharedAppController) {
        sharedAppController = [[AppController alloc] init];
    }
    return sharedAppController;
}



- (void)awakeFromNib
{
    
    [self initializeBreadcrumbs];
    
    [self initializeTutorialEscapeHatch];
    
    [self initializeMainView];
    
    [self applySkin];
    
    

    
    // set options for App Store releases
#ifdef APPSTORE_FREE
    [self.activateMenuItem setHidden:YES];
    [self.activateButton setHidden:YES];
    [self.headerLogo setImage:[NSImage imageNamed:@"header-main-logo.png"]];
    [self.window setTitle:@"Highlight Hunter"];
#endif
    
#ifdef APPSTORE_PREMIUM
    [self.activateMenuItem setHidden:YES];
    [self.activateButton setHidden:YES];
    [self.upgradeToProMenuItem setHidden:YES];
    [self.headerLogo setImage:[NSImage imageNamed:@"header-main-logo-pro.png"]];
    //[self.headerLogo sizeToFit];
    [self.window setTitle:@"Highlight Hunter Pro"];
#endif
    
    
    
}



- (void)initializeLogger
{
    // setup logger
    NSFileManager* fileManager = [NSFileManager defaultManager];
    NSString* bundleIdentifer = [[NSBundle mainBundle] bundleIdentifier];
    NSURL* appSupportDirectory = [fileManager URLsForDirectory:NSApplicationSupportDirectory inDomains:NSUserDomainMask][0];
    NSString* logDirectory = [NSString stringWithFormat:@"%@/%@", [appSupportDirectory path], bundleIdentifer];
    
    MyCustomLogFormatter* logFormatter = [[MyCustomLogFormatter alloc] init];
    DDLogFileManagerDefault* logFileManager = [[DDLogFileManagerDefault alloc] initWithLogsDirectory:logDirectory];
    self.fileLogger = [[DDFileLogger alloc] initWithLogFileManager:logFileManager];
    self.fileLogger.rollingFrequency = 60 * 60 * 24; // 24 hour rolling
    self.fileLogger.logFileManager.maximumNumberOfLogFiles = 7;
    [self.fileLogger setLogFormatter:logFormatter];
    [DDLog addLogger:self.fileLogger];
    
    [[DDTTYLogger sharedInstance] setLogFormatter:logFormatter];
    [DDLog addLogger:[DDTTYLogger sharedInstance]];
    DDLogInfo(@"Logger initialized! %@", self.fileLogger.logFileManager.logsDirectory);
    
}

- (void)initializeBreadcrumbs
{
    self.breadcrumbsViewController = [_breadcrumbsViewController initWithNibName:@"BreadcrumbsView" bundle:nil];
    [_breadcrumbsView addSubview:[_breadcrumbsViewController view]];
    
    [[self.breadcrumbsViewController view] setFrameSize:self.breadcrumbsView.bounds.size];
}

- (void)initializeTutorialEscapeHatch
{
    TutorialProgress progress = [TutorialHelper getProgress];
    if (progress == TutorialFinished)
        return;
    
    // check if app was closed midway through tutorial
    if (progress != TutorialAddSampleVideo)
    {
        [TutorialHelper resetProgress];
        [AnalyticsHelper fireEvent:@"Tutorial progress - Reset" eventValue:0];
    }
    
    self.tutorialEscapeHatchViewController = [_tutorialEscapeHatchViewController initWithNibName:@"TutorialEscapeHatchView" bundle:nil];
    [_tutorialEscapeHatchView addSubview:[_tutorialEscapeHatchViewController view]];
    
    [self.tutorialEscapeHatchViewController setDelegate:self];
    
    [[self.tutorialEscapeHatchViewController view] setFrameSize:self.tutorialEscapeHatchView.bounds.size];
    
    [self.tutorialEscapeHatchViewController refreshProgress];
    
    [self.tutorialEscapeHatchView setHidden:NO];
}

- (void)initializeMainView
{
    [self changeMainViewController:kSelectViewTag];
}

- (void)applySkin
{
    DDLogInfo(@"called");
    
    NSImage* backgroundImage = [NSImage imageNamed:@"background-pattern-102x102-tile.png"];
    if (backgroundImage == nil)
        DDLogError(@"Couldn't load background image!");
    else
        [self.window setBackgroundColor:[NSColor colorWithPatternImage:backgroundImage]];
    
    
    
    
}




- (void)setDefaultSettings
{
    DDLogVerbose(@"called");
    
    
    // to reset, uncomment these lines
#if DEBUG // so that we never ship this in RELEASE
    //NSString *appDomain = [[NSBundle mainBundle] bundleIdentifier];
    //[[NSUserDefaults standardUserDefaults] removePersistentDomainForName:appDomain];
#endif
    
    
    NSUserDefaults *defaults = [NSUserDefaults standardUserDefaults];
    
    
    NSDictionary *appDefaults = @{
    @"HasAppExpired": @"NO",
    @"ShowTutorialv2": @"1",
    @"CaptureDurationInSeconds": @"15",
    @"DetectionThreshold": @"2",
    @"IgnoreEarlyHighlights": @"NO",
    @"SaveOutputFormat" : @"Original",
    @"OptInAnalytics": @"YES",
    @"TotalLoads": @"0",
    @"UserUUID": @"",
    @"TotalVideosScanned": @"0",
    @"TotalMinutesScanned": @"0",
    @"HasAppLoadedYet": @"NO",
    @"EmailAddress": @"",
    @"FacebookAccessToken": @"",
    @"LastSavedPath": [MainModel getDefaultFolderURL].path,
    @"LastOpenPath": [MainModel getDefaultFolderURL].path,
    @"HasSelectedVideo": @"NO",
    @"HasScannedSampleVideo": @"NO",
    @"HasRegisteredScan": @"NO",
    @"HasFoundHighlight": @"NO",
    @"HasOpenedDetails": @"NO",
    @"HasSavedVideo": @"NO",
    @"HasSharedVideo": @"NO",
    @"HasSharedVideoToFacebook": @"NO",
    @"HasSeenFacebookEncodingWarning": @"NO",
    @"HasExportedHighlights": @"NO",
    @"LastAppStoreReviewPromptDate": @"",
    @"HasAddressedAppStoreReviewPrompt": @"NO",
    @"AppStoreReviewVersionNumber" : @"", // which version of the app they reviewed
    };
    
    [defaults registerDefaults:appDefaults];
    
    DDLogInfo(@"Default settings registered");
    
}


- (BOOL)isSafeToPrompt
{
    // We may not want to prompt while an OpenPanel is open but this should be fine for now
    
    return YES;
}


- (void)changeMainViewController:(NSInteger)tag
{
    [[_mainViewController view] removeFromSuperview];
    
    switch (tag)
    {
        case kSelectViewTag:
        {
            self.selectInputVideosViewController = [[SelectInputVideosViewController alloc] initWithNibName:kSelectView bundle:nil];
            [self.selectInputVideosViewController setMainModel:self.mainModel];
            [self.selectInputVideosViewController setDelegate:self];
            [self.selectInputVideosViewController setTutorialEscapeHatchDelegate:self];
            self.mainViewController = self.selectInputVideosViewController;
            [self.breadcrumbsViewController switchToSelect];
            
            break;
        }
        case kScanViewTag:
        {
            self.scanVideosViewController = [[ScanVideosViewController alloc] initWithNibName:kScanView bundle:nil];
            [self.scanVideosViewController setMainModel:self.mainModel];
            [self.scanVideosViewController setDelegate:self];
            self.mainViewController = self.scanVideosViewController;
            [self.breadcrumbsViewController switchToScan];
            break;
        }
        case kReviewViewTag:
        {
            self.reviewHighlightsViewController = [[ReviewHighlightsViewController alloc] initWithNibName:kReviewView bundle:nil];
            [self.reviewHighlightsViewController setMainModel:self.mainModel];
            [self.reviewHighlightsViewController setWindow:self.window];
            [self.reviewHighlightsViewController setReviewHighlightsDelegate:self];
            [self.reviewHighlightsViewController setTutorialEscapeHatchDelegate:self];
            self.mainViewController = self.reviewHighlightsViewController;
            [self.breadcrumbsViewController switchToReview];
            break;
        }
    }
    [_mainView addSubview:[_mainViewController view]];
    [[_mainViewController view] setFrame:[_mainView bounds]];
}



- (void)applicationDidFinishLaunching:(NSNotification *)aNotification
{
    
    DDLogInfo(@"called");
    
    [self handleScreenResolution];
    
    NSUserDefaults *standardUserDefaults = [NSUserDefaults standardUserDefaults];
	
    // turn analytics on by default, even if we've already asked them
    // Apple says we can turn it on by default (issue 541)
    [standardUserDefaults setBool:YES forKey:@"OptInAnalytics"];
    [standardUserDefaults synchronize];

    
    [self handleTutorial];
    
    // track app load (put this after we ask for analytics opt in)
    [self trackAppLoad];
    
    [self trackTotalAppLoads];
    
    [self trackFirstAppLoad];

    
    
    // check for updates
    UpdateChecker* updateChecker = [UpdateChecker new];
    [updateChecker checkForUpdate:[self window]];
    
    
}



- (void)trackAppLoad
{
    DDLogInfo(@"called");
    
    [AnalyticsHelper fireEvent:@"Load" eventValue:nil];
    
    NSProcessInfo *pinfo = [NSProcessInfo processInfo];
    NSString *version = [pinfo operatingSystemVersionString];
    [AnalyticsHelper fireEvent:[NSString stringWithFormat:@"Load - OS - Mac %@", version] eventValue:0];
    
}


- (void)trackTotalAppLoads
{
    DDLogInfo(@"called");
    
    NSUserDefaults *standardUserDefaults = [NSUserDefaults standardUserDefaults];
	NSInteger totalLoads = 0;
	
	if (standardUserDefaults)
    {
        totalLoads = [standardUserDefaults integerForKey:@"TotalLoads"];
        totalLoads += 1;
        [standardUserDefaults setInteger:totalLoads forKey:@"TotalLoads"];
        [standardUserDefaults synchronize];
        
        [AnalyticsHelper fireEvent:[NSString stringWithFormat:@"Total loads - %ld", totalLoads] eventValue:0];
    }
    
}

- (void)trackFirstAppLoad
{
    DDLogInfo(@"called");
    
    NSUserDefaults *standardUserDefaults = [NSUserDefaults standardUserDefaults];
	if (standardUserDefaults)
    {
        if ([standardUserDefaults boolForKey:@"HasAppLoadedYet"] == NO)
        {
            if ([AnalyticsHelper fireEvent:@"First load" eventValue:@1])
            {
                [standardUserDefaults setBool:YES forKey:@"HasAppLoadedYet"];
                [standardUserDefaults synchronize];
            }
        }
    }
}


- (void)handleScreenResolution
{
    // if window is too big for screen resolution, resize window frame to screen size so user sees all the controls
    int screenWidth = [[NSScreen mainScreen] visibleFrame].size.width;
    int screenHeight = [[NSScreen mainScreen] visibleFrame].size.height;
    
    if (self.window.frame.size.width > screenWidth ||
        self.window.frame.size.height > screenHeight)
    {
        [self.window setFrame:[[NSScreen mainScreen] visibleFrame] display:YES];
    }
}


#pragma mark Termination methods

- (BOOL)applicationShouldTerminateAfterLastWindowClosed:(NSApplication *)theApplication
{
    return YES;
}


- (NSApplicationTerminateReply)applicationShouldTerminate:(NSApplication *)sender
{
    if ([self checkIfScanningOrPublishing] == NSTerminateCancel)
        return NSTerminateCancel;
    
    if ([self checkHasScannedVideo] == NSTerminateCancel)
        return NSTerminateCancel;
    
    if ([self checkIfAllHighlightsHaveBeenReviewed] == NSTerminateCancel)
        return NSTerminateCancel;
    
    return NSTerminateNow;
}


- (NSApplicationTerminateReply)checkHasScannedVideo
{
    NSUserDefaults *standardUserDefaults = [NSUserDefaults standardUserDefaults];
	if (standardUserDefaults)
    {
        NSInteger total = [standardUserDefaults integerForKey:@"TotalVideosScanned"];
        if (total == 0)
        {
            NSString* errorMsg = @"Before you leave, do you want to try scanning a sample video?";
            NSAlert *alert = [[NSAlert alloc] init];
            [alert addButtonWithTitle:@"Scan a sample video"];
            [alert addButtonWithTitle:@"Quit"];
            [alert setMessageText:@"Scan a sample video"];
            [alert setInformativeText:errorMsg];
            [alert setAlertStyle:NSWarningAlertStyle];
            NSInteger returnCode = [alert runModalSheetForWindow:[self window]];
            if (returnCode == NSAlertSecondButtonReturn)
            {
                return NSTerminateNow;
            }
            
            if (self.selectInputVideosViewController != nil)
            {
                [self.selectInputVideosViewController scanSampleVideo];
            }
            
            return NSTerminateCancel;
        }
    }

    return NSTerminateNow;
}


- (NSApplicationTerminateReply)checkIfAllHighlightsHaveBeenReviewed
{
    BOOL isClosingWithoutReviewing = NO;
    
    for (HighlightObject* highlightObject in self.mainModel.highlightObjects)
    {
        if (highlightObject.hasBeenReviewed == NO)
        {
            isClosingWithoutReviewing = YES;
            break;
        }
    }
    
    if (isClosingWithoutReviewing)
    {
        NSString* errorMsg = @"Are you sure you want to quit Highlight Hunter?";
        NSAlert *alert = [[NSAlert alloc] init];
        [alert addButtonWithTitle:@"Review highlights"];
        [alert addButtonWithTitle:@"Quit"];
        [alert setMessageText:@"You haven't reviewed all your highlights."];
        [alert setInformativeText:errorMsg];
        [alert setAlertStyle:NSWarningAlertStyle];
        NSInteger returnCode = [alert runModalSheetForWindow:[self window]];
        if (returnCode == NSAlertSecondButtonReturn)
            return NSTerminateNow;
        else
            return NSTerminateCancel;
    }
    
    return NSTerminateNow;
}


- (NSApplicationTerminateReply)checkIfScanningOrPublishing
{

    BOOL isPublishing = NO;
    
    if (self.reviewHighlightsViewController != nil)
        isPublishing = [self.reviewHighlightsViewController isPublishing];
    
    BOOL isScanning = NO;
    if (self.scanVideosViewController != nil)
        isScanning = self.scanVideosViewController.isScanning;
    
    
    if (isScanning || isPublishing)
    {
        NSString* errorMsg = @"Are you sure you want to quit Highlight Hunter?";
        NSAlert *alert = [[NSAlert alloc] init];
        [alert addButtonWithTitle:@"Cancel"];
        [alert addButtonWithTitle:@"Quit"];
        [alert setMessageText:@"Highlight Hunter is busy working for you."];
        [alert setInformativeText:errorMsg];
        [alert setAlertStyle:NSWarningAlertStyle];
        NSInteger returnCode = [alert runModalSheetForWindow:[self window]];
        if (returnCode == NSAlertFirstButtonReturn)
            return NSTerminateCancel;
        
        if (self.scanVideosViewController != nil)
        {
            [self.scanVideosViewController cancelScan:nil];
        }
        
        if (self.reviewHighlightsViewController != nil)
        {
            [self.reviewHighlightsViewController cancelPublish];
        }
        
        return NSTerminateNow;
    }
    
    return NSTerminateNow;
    
}


#ifndef APPSTORE
- (NSWindowController *)activateWindowController
{
    if (_activateWindowController == nil)
    {
        _activateWindowController = [[ActivateWindowController alloc] init];
    }
    return _activateWindowController;
}
#endif


- (IBAction)shareTwitterButtonPressed:(id)sender {
    [BrowserHelper launchBrowser:@"https://twitter.com/intent/tweet?original_referer=http%3A%2F%2Fwww.highlighthunter.com%2F&source=tweetbutton&text=Highlight%20Hunter%20-%20Find%20the%20highlights%20in%20your%20videos%208%20times%20faster%20%40HighlightHunter&url=http%3A%2F%2Fwww.highlighthunter.com" term:@"" addDomain:NO];
}

- (IBAction)shareFacebookButtonPressed:(id)sender {
    
    [BrowserHelper launchBrowser:@"https://www.facebook.com/sharer/sharer.php?u=http%3A%2F%2Fwww.highlighthunter.com&t=I'm%20using%20Highlight+Hunter%20to%20find%20the%20highlights%20in%20my%20videos" term:@"" addDomain:NO];
}

- (IBAction)createFakeHighlights:(id)sender {
    
    
    ThumbnailGenerator* thumbnailGenerator = [ThumbnailGenerator new];
    
    InputFileObject* inputFileObject1 = [InputFileObject new];
    inputFileObject1.sourceURL = [MainModel getURLToSampleVideo];
    inputFileObject1.title = @"Pro snowboarder Justin Morgan";
    [self.mainModel.inputFileObjects addObject:inputFileObject1];
    
    ScanWorker* scanWorker1 = [ScanWorker workerWithInputFileObject:inputFileObject1];
    [scanWorker1 setMainModel:self.mainModel];
    [scanWorker1 setTotalFrames];
    [scanWorker1 setFramesPerSecond];
    [scanWorker1 setBitrate];
    [scanWorker1 setVideoDuration];
    [scanWorker1 setVideoDimensions];
    
    HighlightObject* highlightObject1 = [HighlightObject new];
    highlightObject1.inputFileObject = inputFileObject1;
    highlightObject1.beginTime = 5;
    highlightObject1.endTime = 20;
    highlightObject1.bookmarkTime = 22;
    highlightObject1.title = @"Pro snowboarder Justin Morgan highlight #1";
    [highlightObject1 generateThumbnail:thumbnailGenerator];
    [self.mainModel.highlightObjects addObject:highlightObject1];
    
    HighlightObject* highlightObject2 = [HighlightObject new];
    highlightObject2.inputFileObject = inputFileObject1;
    highlightObject2.beginTime = 47;
    highlightObject2.endTime = 62;
    highlightObject2.bookmarkTime = 64;
    highlightObject2.title = @"Pro snowboarder Justin Morgan highlight #2";
    [highlightObject2 generateThumbnail:thumbnailGenerator];
    [self.mainModel.highlightObjects addObject:highlightObject2];
    
    // second file
    
    InputFileObject* inputFileObject2 = [InputFileObject new];
    inputFileObject2.sourceURL = [MainModel getURLToSampleVideo]; //[NSURL fileURLWithPath:@"/Users/user/Movies/Sample Files/Long videos/GO021100.MP4"];
    inputFileObject2.title = @"GO021100";
    [self.mainModel.inputFileObjects addObject:inputFileObject2];
    
    ScanWorker* scanWorker2 = [ScanWorker workerWithInputFileObject:inputFileObject2];
    [scanWorker2 setMainModel:self.mainModel];
    [scanWorker2 setTotalFrames];
    [scanWorker2 setFramesPerSecond];
    [scanWorker2 setBitrate];
    [scanWorker2 setVideoDuration];
    [scanWorker2 setVideoDimensions];
    
    /*
    HighlightObject* highlightObject3 = [HighlightObject new];
    highlightObject3.inputFileObject = inputFileObject2;
    highlightObject3.beginTime = 5;
    highlightObject3.endTime = 20;
    highlightObject3.bookmarkTime = 22;
    highlightObject3.title = @"GO022100 highlight #1";
    [highlightObject3 generateThumbnail:thumbnailGenerator];
    [self.mainModel.highlightObjects addObject:highlightObject3];
    
    
    // third
    
    InputFileObject* inputFileObject3 = [InputFileObject new];
    inputFileObject3.sourceURL = [NSURL fileURLWithPath:@"/Users/user/Movies/Sample Files/Long videos/FILE0001.MOV"];
    inputFileObject3.title = @"FILE0001";
    [self.mainModel.inputFileObjects addObject:inputFileObject3];
    
    ScanWorker* scanWorker3 = [ScanWorker workerWithInputFileObject:inputFileObject3];
    [scanWorker3 setMainModel:self.mainModel];
    [scanWorker3 setTotalFrames];
    [scanWorker3 setFramesPerSecond];
    [scanWorker3 setBitrate];
    [scanWorker3 setVideoDuration];
    [scanWorker3 setVideoDimensions];
    */
    
    DDLogInfo(@"highlight count: %ld", self.mainModel.highlightObjects.count);
    
    [self changeMainViewController:kReviewViewTag];
    
}

- (IBAction)throwException:(id)sender {
    [NSException raise:@"TestException" format:@"Something went wrong"];
    
    /* other ways of doing it
     
     NSAutoreleasePool *pool = [[NSAutoreleasePool alloc] init];
     NSLog(@"exception in thread");
     [NSException raise:@"TestExceptionThread" format:@"Something went wrong"];
     [NSThread exit];
     [pool drain];
     
     
     
     
     [NSThread detachNewThreadSelector:@selector(threadWithException) toTarget:self withObject:nil];
     }
     */
    
}



- (void)handleTutorial
{
    
    NSUserDefaults *standardUserDefaults = [NSUserDefaults standardUserDefaults];
    if (standardUserDefaults) {
        NSInteger showTutorial = [standardUserDefaults integerForKey:@"ShowTutorialv2"];
        
#ifdef DEBUG
        //showTutorial = true;
#endif
        
        
        if (showTutorial)
        {
            [self showTutorialWindow:nil];
            
        }
        
    }
    
}


- (IBAction)showActivateWindow:(id)sender
{
#ifndef APPSTORE
    [self.activateWindowController showWindow:nil];
#endif
}

- (IBAction)showTutorialWindow:(id)sender
{
    
    [TutorialHelper resetProgress];
    
    [TutorialHelper tutorialStarted];
    
    if (self.selectInputVideosViewController != nil)
        [self.selectInputVideosViewController initializeSampleVideoTutorialBubble];
    
    [self initializeTutorialEscapeHatch];

    if (self.tutorialWindowController == nil)
        self.tutorialWindowController = [[TutorialWindowController alloc] init];
    
    [self.tutorialWindowController showWindow:nil];
    
}




- (IBAction)showPreferencesWindow:(id)sender
{
    if (self.preferencesWindowController == nil)
    {
        NSViewController *durationViewController = [[HighlightDurationPreferencesViewController alloc] init];
        NSViewController *detectionViewController = [[DetectionThresholdPreferencesViewController alloc] init];
        NSViewController *advancedViewController = [[AdvancedPreferencesViewController alloc] init];
        NSArray *controllers = @[detectionViewController, durationViewController, advancedViewController];
        
        NSString *title = NSLocalizedString(@"Preferences", @"Preferences");
        self.preferencesWindowController = [[MASPreferencesWindowController alloc] initWithViewControllers:controllers title:title];
    }

    [self.preferencesWindowController showWindow:nil];
}

- (IBAction)helpMenuPressed:(id)sender {
    
    [BrowserHelper launchBrowser:@"http://support.highlighthunter.com" term:@"help" addDomain:false];
    
}

- (IBAction)viewDebugLogsPressed:(id)sender {
    
    NSMutableArray* urls = [NSMutableArray new];
    
    NSFileManager* fileManager = [NSFileManager defaultManager];
    
    NSString* folder = self.fileLogger.logFileManager.logsDirectory;
    
    DDLogInfo(@"folder = %@", folder);
    NSArray* files = [fileManager contentsOfDirectoryAtPath:folder error:nil];
    
    for (int i=0; i<[files count]; i++)
    {
        NSString* fileName = files[i];
        
        //DDLogInfo(@"MainModel:getFilesInFolder(): %@", fileName);
        NSURL* url = [NSURL fileURLWithPathComponents:@[folder, fileName]];
        
        [urls addObject:url];
    }
    
    
    
    [[NSWorkspace sharedWorkspace] activateFileViewerSelectingURLs:urls];
}

- (IBAction)upgradeToPro:(id)sender {
    [BrowserHelper launchPurchasingOptions];
}




#pragma mark Delegates


- (void)scanStarted
{
    [self changeMainViewController:kScanViewTag];
    [self.scanVideosViewController startScan];
}

- (void)scanFinished
{
    [self changeMainViewController:kReviewViewTag];
}

- (void)scanCancelled
{
    [self changeMainViewController:kSelectViewTag];
}

- (void)startOver
{
    [self changeMainViewController:kSelectViewTag];
}

- (void)rescanRequested
{
    if (self.mainModel.highlightObjects.count > 0)
    {
        NSString* errorMsg = @"Proceeding will clear all existing highlights.\n\nAre you sure you want to continue?";
        NSAlert *alert = [[NSAlert alloc] init];
        [alert addButtonWithTitle:@"Rescan"];
        [alert addButtonWithTitle:@"Cancel"];
        [alert setMessageText:@"Clear existing highlights?"];
        [alert setInformativeText:errorMsg];
        [alert setAlertStyle:NSWarningAlertStyle];
        NSInteger response = [alert runModalSheetForWindow:[self window]];
        if (response == NSAlertSecondButtonReturn)
            return;
        
        [self.mainModel.highlightObjects removeAllObjects];
    }
    
    NSUserDefaults *standardUserDefaults = [NSUserDefaults standardUserDefaults];
    if (standardUserDefaults)
    {
        [standardUserDefaults setInteger:4 forKey:@"DetectionThreshold"];
        [standardUserDefaults setBool:NO forKey:@"IgnoreEarlyHighlights"];
        [standardUserDefaults synchronize];
     
        [self scanStarted];
    }
    
}


- (void)exitTutorial
{
    [self.tutorialEscapeHatchView setHidden:YES];
    
    if (self.selectInputVideosViewController)
        [self.selectInputVideosViewController hideTutorialBubbles];
    
    if (self.reviewHighlightsViewController)
        [self.reviewHighlightsViewController hideTutorialBubbles];
}

- (void)refreshTutorialProgress
{
    if (self.tutorialEscapeHatchViewController)
        [self.tutorialEscapeHatchViewController refreshProgress];
}


@end

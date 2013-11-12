//
//  HighlightDetailsWindow.m
//  OdessaMacPrototype
//
//  Created by Noah Spitzer-Williams on 8/9/12.
//
//

#import "HighlightDetailsWindowController.h"

#if DEBUG
static const int ddLogLevel = LOG_LEVEL_VERBOSE | LOG_LEVEL_INFO | LOG_LEVEL_ERROR | LOG_LEVEL_WARN;
#else
static const int ddLogLevel = LOG_LEVEL_INFO | LOG_LEVEL_ERROR | LOG_LEVEL_WARN;
#endif

static const NSTimeInterval zoomSecondsIncrement = 5;


@interface HighlightDetailsWindowController ()

// The movie position at the end of the timeline
@property NSTimeInterval zoomEndTime;

// The movie position at the start of the timeline
@property NSTimeInterval zoomBeginTime;

// The buffer space width on the timeline that must remain between the start and end markers
@property NSInteger timelineWidthBufferBetweenStartAndEnd;

@property NSTimeInterval selectedMovieDuration;
@property NSTimeInterval selectedMovieCurrentTime;
@property (strong) NSTimer* checkMovieTimeTimer;
@property BOOL selectedMovieIsPlaying;

// Whether user is manually seeking the video. Used to hide playhead while seeking.
@property BOOL isDragSeeking;

// Used to automatically pause movie when we hit end tick view
@property BOOL isPlayHeadBeforeEndTickView;

// Keep track of this so we can go back to it when we're done seeking
@property NSTimeInterval movieTimeBeforeSeeking;

@property BOOL isMoviePlayingBeforeSeeking;

// has movie loaded yet?
@property BOOL isMovieLoaded;

@property (strong) QTMovie* movie;

@property (strong) NSWindow* overlayWindow;

@property (strong) NSView *bookmarkFlagTutorialBubble;

@property (strong) NSView *handlesTutorialBubble;

@property (strong) NSView *shareButtonTutorialBubble;

@property (strong) NSView *closeDetailsTutorialBubble;

@end



@implementation HighlightDetailsWindowController

-(void)awakeFromNib
{
    NSImage* backgroundImage = [NSImage imageNamed:@"background-pattern-102x102-tile.png"];
    [[self window] setBackgroundColor:[NSColor colorWithPatternImage:backgroundImage]];
    
    NSTextView *fieldEditor = (NSTextView*)[self.window fieldEditor:YES forObject:self.highlightTitle];
    fieldEditor.insertionPointColor = [DesignLanguage lightGrayColor];
 
    [self.beginTickView setDelegate:self];
    [self.endTickView setDelegate:self];
    [self.timelineView setDelegate:self];
    [self.highlightTimelineView setDelegate:self];
    [self.playheadView setDelegate:self];
    
    [self.previousHighlightButton setHoverImage:[NSImage imageNamed:@"editpanel-button-videoselect-left-hover.png"]];
    [self.nextHighlightButton setHoverImage:[NSImage imageNamed:@"editpanel-button-videoselect-right-hover.png"]];
    [self.deleteHighlightButton setHoverImage:[NSImage imageNamed:@"editpanel-button-videoselect-delete-hover.png"]];
    
    [self initializeShareToFacebookButton];
    [self initializeSaveToComputerButton];
    
    [self updateTimelineBounds];
    
    [self initializeOverlayWindow];
    
    [self initializeBookmarkFlagTutorialBubble];
    [self initializeHandlesTutorialBubble];
    [self initializeShareButtonTutorialBubble];
    [self initializeCloseDetailsTutorialBubble];
    
    [self trackFirstDetails];
    
}

- (void)windowWillClose:(NSNotification *)notification
{
    [self pauseMovie];
    
    if (self.closeDetailsTutorialBubble && self.closeDetailsTutorialBubble.isHidden == NO)
        [self advanceTutorialPastCloseDetails];
}

- (void)initializeShareToFacebookButton
{
    self.shareToFacebookViewController = [self.shareToFacebookViewController initWithNibName:@"PublishButtonView" bundle:nil];
    [self.shareToFacebookViewController setPublishWorkerType:PublishWorkerTypeFacebook];
    [self.shareToFacebookViewController setHighlightObject:self.currentHighlight];
    [self.shareToFacebookViewController setWindow:self.window];
    [self.shareToFacebookViewController setShareButtonTutorialDelegate:self];
    
    [self.shareToFacebookView addSubview:[self.shareToFacebookViewController view]];
    
    [[self.shareToFacebookViewController view] setFrameSize:self.shareToFacebookView.bounds.size];
}

- (void)initializeSaveToComputerButton
{
    self.saveToComputerViewController = [self.saveToComputerViewController initWithNibName:@"PublishButtonView" bundle:nil];
    [self.saveToComputerViewController setPublishWorkerType:PublishWorkerTypeSave];
    [self.saveToComputerViewController setHighlightObject:self.currentHighlight];
    [self.saveToComputerViewController setWindow:self.window];

    [self.saveToComputerView addSubview:[self.saveToComputerViewController view]];
    
    [[self.saveToComputerViewController view] setFrameSize:self.saveToComputerView.bounds.size];
}

- (void)initializeOverlayWindow
{
    
    // NOTE: the following code to move the window to the
    // front of the screen list is necessary on Mac OS X 10.4
    // Tiger to ensure the overlay window is drawn at the
    // correct origin when created (see below)
    [[self.movieView window] makeKeyAndOrderFront:self];
    
    // Create an overlay window which will be attached as a child
    // window to our main window. We will create it directly on top
    // of our main window, so when we draw things they will appear
    // on top of our playing movie
    
    self.overlayWindow=[[NSWindow alloc] initWithContentRect:self.window.frame
                                                   styleMask:NSBorderlessWindowMask
                                                   backing:NSBackingStoreBuffered
                                                   defer:YES];
    [self.overlayWindow setOpaque:NO];
    [self.overlayWindow setHasShadow:YES];
    
    NSColor *transparentColor = [NSColor colorWithSRGBRed:1 green:1 blue:1 alpha:0];
    [self.overlayWindow setBackgroundColor:transparentColor];
    
    [self.overlayWindow orderFront:self];
    
    // add our overlay window as a child window of our main window
    [[self.movieView window] addChildWindow:self.overlayWindow ordered:NSWindowAbove];
    
    
}

- (void)initializeBookmarkFlagTutorialBubble
{
    TutorialProgress progress = [TutorialHelper getProgress];
    if (progress != TutorialBookmarkFlag)
    {
        DDLogVerbose(@"Skipping TutorialBookmarkFlag tutorial. Progress = %d", progress);
        return;
    }
    
    self.bookmarkFlagTutorialBubbleController = [_bookmarkFlagTutorialBubbleController initWithNibName:@"TutorialBubble" bundle:nil];
    
    [self.bookmarkFlagTutorialBubbleController setActionTarget:self];
    [self.bookmarkFlagTutorialBubbleController setActionSelector:@selector(advanceTutorialPastBookmarkFlag)];
    
    self.bookmarkFlagTutorialBubble = [_bookmarkFlagTutorialBubbleController view];
    
    [[self.overlayWindow contentView] addSubview:self.bookmarkFlagTutorialBubble];
    
    [self.bookmarkFlagTutorialBubbleController loadTooltip];
    
    [self.bookmarkFlagTutorialBubble setFrameSize:self.bookmarkFlagTutorialBubbleController.tooltipImage.frame.size];

    [self.bookmarkFlagTutorialBubble setHidden:NO];
    
    [self updateTutorialBubbleCoordinates];
    
}

- (void)initializeHandlesTutorialBubble
{
    TutorialProgress progress = [TutorialHelper getProgress];
    if (progress != TutorialHandles)
    {
        DDLogVerbose(@"Skipping TutorialHandles tutorial. Progress = %d", progress);
        return;
    }
    
    self.handlesTutorialBubbleController = [_handlesTutorialBubbleController initWithNibName:@"TutorialBubble" bundle:nil];
    
    [self.handlesTutorialBubbleController setActionTarget:self];
    [self.handlesTutorialBubbleController setActionSelector:@selector(advanceTutorialPastHandles)];

    self.handlesTutorialBubble = [_handlesTutorialBubbleController view];
    
    [[self.overlayWindow contentView] addSubview:self.handlesTutorialBubble];
    
    [self.handlesTutorialBubbleController loadTooltip];

    [self.handlesTutorialBubble setFrameSize:self.handlesTutorialBubbleController.tooltipImage.frame.size];

    [self.handlesTutorialBubble setHidden:NO];
    
    [self updateTutorialBubbleCoordinates];
    
}

- (void)initializeShareButtonTutorialBubble
{
    TutorialProgress progress = [TutorialHelper getProgress];
    if (progress != TutorialShareButton)
    {
        DDLogVerbose(@"Skipping TutorialShareButton tutorial. Progress = %d", progress);
        return;
    }
    
    self.shareButtonTutorialBubbleController = [_shareButtonTutorialBubbleController initWithNibName:@"TutorialBubble" bundle:nil];
    
    [self.shareButtonTutorialBubbleController setActionTarget:self];
    [self.shareButtonTutorialBubbleController setActionSelector:@selector(advanceTutorialPastShareButton)];
    
    self.shareButtonTutorialBubble = [_shareButtonTutorialBubbleController view];
    
    [[self.overlayWindow contentView] addSubview:self.shareButtonTutorialBubble];
    
    [self.shareButtonTutorialBubbleController loadTooltip];

    [self.shareButtonTutorialBubble setFrameSize:self.shareButtonTutorialBubbleController.tooltipImage.frame.size];

    [self.shareButtonTutorialBubble setHidden:NO];
    
    [self updateTutorialBubbleCoordinates];
    
}

- (void)initializeCloseDetailsTutorialBubble
{
    TutorialProgress progress = [TutorialHelper getProgress];
    if (progress != TutorialCloseDetails)
    {
        DDLogVerbose(@"Skipping TutorialCloseDetails tutorial. Progress = %d", progress);
        return;
    }
    
    self.closeDetailsTutorialBubbleController = [_closeDetailsTutorialBubbleController initWithNibName:@"TutorialBubble" bundle:nil];
    
    [self.closeDetailsTutorialBubbleController setActionTarget:self];
    [self.closeDetailsTutorialBubbleController setActionSelector:@selector(advanceTutorialPastCloseDetails)];
    
    self.closeDetailsTutorialBubble = [_closeDetailsTutorialBubbleController view];
    
    [[self.overlayWindow contentView] addSubview:self.closeDetailsTutorialBubble];
    
    [self.closeDetailsTutorialBubbleController loadTooltip];

    [self.closeDetailsTutorialBubble setFrameSize:self.closeDetailsTutorialBubbleController.tooltipImage.frame.size];

    [self.closeDetailsTutorialBubble setHidden:NO];
    
    [self updateTutorialBubbleCoordinates];
    
}


- (NSInteger)getOriginXforCenterPoint:(NSView*)anchor : (NSView*)slave
{
    return anchor.frame.origin.x + ((anchor.frame.size.width - slave.frame.size.width) / 2);
}


- (void)updateTutorialBubbleCoordinates
{
    
    if (self.bookmarkFlagTutorialBubble.isHidden == NO) // if tutorial isn't running, let's escape
    {
        NSInteger x = [self getOriginXforCenterPoint:self.bookmarkTickView :self.bookmarkFlagTutorialBubble];
        if (x < 0)
            x = 0;
        NSPoint newOrigin = NSMakePoint(x, self.bookmarkTickView.frame.origin.y + self.bookmarkTickView.bounds.size.height);
    
        [self.bookmarkFlagTutorialBubble setFrameOrigin:newOrigin];
        
        [self.overlayWindow setFrame:self.window.frame display:YES];
    }
    
    if (self.handlesTutorialBubble.isHidden == NO)
    {
        NSInteger x = [self getOriginXforCenterPoint:self.beginTickView :self.handlesTutorialBubble];
        x += 70; // account for BottomLeft arrow
        if (x < 0)
            x = 0;
        NSPoint newOrigin = NSMakePoint(x, self.beginTickView.frame.origin.y + self.beginTickView.bounds.size.height);
        
        [self.handlesTutorialBubble setFrameOrigin:newOrigin];
        
        [self.overlayWindow setFrame:self.window.frame display:YES];
    }
    
    if (self.shareButtonTutorialBubble.isHidden == NO)
    {
        NSPoint newOrigin = NSMakePoint([self getOriginXforCenterPoint:self.shareToFacebookView :self.shareButtonTutorialBubble],
                                        self.shareToFacebookView.frame.origin.y + self.shareToFacebookView.frame.size.height);
        
        [self.shareButtonTutorialBubble setFrameOrigin:newOrigin];
        
        [self.overlayWindow setFrame:self.window.frame display:YES];
    }
    
    if (self.closeDetailsTutorialBubble.isHidden == NO)
    {
        NSPoint newOrigin = NSMakePoint(3, self.window.frame.size.height - self.closeDetailsTutorialBubble.bounds.size.height - 38); // this number accounts for title bar
        
        [self.closeDetailsTutorialBubble setFrameOrigin:newOrigin];
        
        [self.overlayWindow setFrame:self.window.frame display:YES];
    }

}


- (HighlightObject *)currentHighlight
{
    return currentHighlight;
}

- (void)setCurrentHighlight:(HighlightObject *)newCurrentHighlight
{
    HighlightObject* oldHighlight = self.currentHighlight;
    
    if (oldHighlight != nil)
    { // unsubscribe from old events
        [self.saveToComputerViewController detachWorkerProgressDelegate];
        [self.shareToFacebookViewController detachWorkerProgressDelegate];
    }
    
    currentHighlight = newCurrentHighlight;
    
    DDLogInfo(@"switching to highlight %@", self.currentHighlight.title);
    
    NSAssert(self.currentHighlight != nil, @"currentHighlight is nil!");
    NSAssert(self.currentHighlight.endTime != 0, @"endTime is zero!");
    NSAssert(self.currentHighlight.inputFileObject != nil, @"inputFileObject is nil!");
    NSAssert(self.currentHighlight.inputFileObject.videoDuration != 0, @"videoDuration is zero!");
    
    DDLogVerbose(@"beginTime = %f", self.currentHighlight.beginTime);
    DDLogVerbose(@"endTime = %f", self.currentHighlight.endTime);
    
    NSString* windowTitle = [NSString stringWithFormat:@"Highlight Details (%ld of %ld)", [self.mainModel.highlightObjects indexOfObject:newCurrentHighlight] + 1, self.mainModel.highlightObjects.count];
    [self.window setTitle:windowTitle];
    
    [self setZoomBeginTime:self.currentHighlight.beginTime - 45];
    if (self.zoomBeginTime < 0)
        [self setZoomBeginTime:0];
    
    [self setZoomEndTime:self.currentHighlight.endTime + 15];
    if (self.zoomEndTime > self.currentHighlight.inputFileObject.videoDuration)
        [self setZoomEndTime:self.currentHighlight.inputFileObject.videoDuration];
    
    self.currentHighlight.hasBeenReviewed = YES;
    
    
    if (self.movieView == nil || [oldHighlight.inputFileObject.sourceURL.path isEqualToString:self.currentHighlight.inputFileObject.sourceURL.path] == NO)
    { // it's either the first video or a different video file
        [self loadMovie];
    }
    else
    { // same video file so let's just seek
        DDLogInfo(@"Same video file so let's just seek");
        [self seekMovie:self.currentHighlight.beginTime];
        [self playMovie];
    }
    
    [self updateTickCoordinates];
    
    [self.highlightTimelineView setHidden:NO];
    [self.beginTickView setHidden:NO];
    [self.endTickView setHidden:NO];
    [self.highlightDurationField setHidden:NO];
    [self.bookmarkTickView setHidden:NO];
    [self.playStateButton setEnabled:YES];
    
    [self.shareToFacebookViewController setHighlightObject:self.currentHighlight];
    [self.saveToComputerViewController setHighlightObject:self.currentHighlight];

    
}

- (void)loadMovie
{
    DDLogInfo(@"playing video: %@", self.currentHighlight.inputFileObject.sourceURL.path);
    
    NSError* err = nil;
    QTMovie* newMovie = [QTMovie movieWithAttributes:
                         @{
                            QTMovieURLAttribute: self.currentHighlight.inputFileObject.sourceURL,
                            QTMovieOpenForPlaybackAttribute: [NSNumber numberWithBool:YES],
                         }
                         error:&err];

    if (newMovie)
    {
        if (newMovie != self.movie)
        {
            NSNotificationCenter* notificationCenter = [NSNotificationCenter defaultCenter];
            
            if (self.movie)
            {
                /* unregister for movie notfications:
                 
                 QTMovieLoadStateDidChangeNotification
                 -Sent when the load state of a movie has changed.
                 QTMovieDidEndNotification
                 -Sent when the movie is “done” or at its end.
                 QTMovieRateDidChangeNotification
                 -Sent when the rate of a movie has changed.
                 */
				[notificationCenter removeObserver:self name:QTMovieLoadStateDidChangeNotification object:self.movie];
				[notificationCenter removeObserver:self name:QTMovieDidEndNotification object:self.movie];
				[notificationCenter removeObserver:self name:QTMovieRateDidChangeNotification object:self.movie];
                
            }
            
            [self setIsMovieLoaded:NO];
            
            [self.movieView setMovie:newMovie];
            [self setMovie:self.movieView.movie];
            
            if (self.movie)
            {
                [notificationCenter addObserver:self selector:@selector(movieLoadStateChanged:) name:QTMovieLoadStateDidChangeNotification object:self.movie];
                [notificationCenter addObserver:self selector:@selector(movieDidEnd:) name:QTMovieDidEndNotification object:self.movie];
                [notificationCenter addObserver:self selector:@selector(movieRateChanged:) name:QTMovieRateDidChangeNotification object:self.movie];
                
                if (!self.checkMovieTimeTimer)
                    self.checkMovieTimeTimer = [NSTimer scheduledTimerWithTimeInterval:0.25 target:self selector:@selector(checkMovieTime:) userInfo:nil repeats:YES];
            }
            else
            {
                [self.checkMovieTimeTimer invalidate];
            }
            
            // reset selectedMovieDuration
			// this will be updated to the new movie's duration in handleLoadStateChanged once the
			// movie's load state is QTMovieLoadStateLoaded
			[self setSelectedMovieDuration:0.0];
            
			
			[self handleLoadStateChanged];
			[self updateSelectedMovieCurrentTime];
			[self updateSelectedMovieIsPlaying];
            
            [self.movie autoplay];
            
            
        }
        
    }
    else
    {
        DDLogError(@"error %@", err);
        [AnalyticsHelper fireEvent:[NSString stringWithFormat:@"Quicktime Error - %@", err] eventValue:0];
    }
}

- (void)windowDidResize:(NSNotification *)notification
{
    [self updateTimelineBounds];
    [self updateTickCoordinates];
}


- (void)trackFirstDetails
{
    NSUserDefaults* standardUserDefaults = [NSUserDefaults standardUserDefaults];
    if (standardUserDefaults)
    {
        if ([standardUserDefaults boolForKey:@"HasOpenedDetails"] == NO)
        {
            if ([AnalyticsHelper fireEvent:@"First details" eventValue:0])
            {
                [standardUserDefaults setBool:YES forKey:@"HasOpenedDetails"];
                [standardUserDefaults synchronize];
            }
        }
    }
}


#pragma mark Movie

/*
 
 Because opening a movie file or URL may involve reading and processing large amounts of movie
 data, QTKit may take a non-negligible amount of time to make a QTMovie object ready for
 inspection and playback. Accordingly, you need to pay attention to the movie’s load states when
 opening a movie file or URL. These are the defined movie load states:
 
 QTMovieLoadStateError   = -1L, // an error occurred while loading the movie
 QTMovieLoadStateLoading = 1000, // the movie is loading
 QTMovieLoadStateLoaded = 2000, // the movie atom has loaded; it's safe to query movie properties
 QTMovieLoadStatePlayable = 10000, // the movie has loaded enough media data to begin playing
 QTMovieLoadStatePlaythroughOK = 20000, //the movie has loaded enough media data to play through to end
 QTMovieLoadStateComplete = 100000L // the movie has loaded completely
 
 */
- (void)handleLoadStateChanged
{
	QTMovie *movie = self.movie;
	if (movie)
	{
		NSInteger loadState = [[movie attributeForKey:QTMovieLoadStateAttribute] longValue];
		
        DDLogInfo(@"called with loadState=%ld", loadState);
        
		if (loadState == QTMovieLoadStateError)
		{
            [self.playStateButton setEnabled:NO];
            [self.playheadView setHidden:YES];
            
			// an error occurred while loading the movie
            if ([[self.currentHighlight.inputFileObject.sourceURL.pathExtension uppercaseString] isEqualToString:@"MTS"])
            {
                NSString* errorMsg = @"You'll still be able to save and share this highlight, but you won't be able to watch it.\n\nLame - we know!";
                NSAlert *alert = [[NSAlert alloc] init];
                [alert addButtonWithTitle:@"OK"];
                [alert setMessageText:@"Quicktime does not support MTS files"];
                [alert setInformativeText:errorMsg];
                [alert setAlertStyle:NSWarningAlertStyle];
                [alert runModalSheetForWindow:[self window]];
            }
            else
            {
                [self presentError:[movie attributeForKey:QTMovieLoadStateErrorAttribute]];
            }
		}
		
		if (loadState >= QTMovieLoadStateLoaded)
		{
            DDLogInfo(@"movie has loaded");
            
            [self setIsMovieLoaded:YES];
            
			// now that the movie is loaded, it can return information about its structure
			// at this point, it is safe to get the movie's duration
			QTTime duration = [movie duration];
			
			NSTimeInterval timeInterval;
			QTGetTimeInterval(duration, &timeInterval);
			[self setSelectedMovieDuration:timeInterval];
            
            [self seekMovie:self.currentHighlight.beginTime];
		}
	}
}

// This method called when the load state of a movie has changed
- (void)movieLoadStateChanged:(NSNotification *)notification
{
	[self handleLoadStateChanged];
}


- (void)updateSelectedMovieIsPlaying
{
	QTMovie *movie = self.movie;
	[self setSelectedMovieIsPlaying:(movie && [movie rate] != 0.0f)];
    
    if (self.selectedMovieIsPlaying)
        [self.playStateButton setState:NSOnState]; // show pause
    else
        [self.playStateButton setState:NSOffState]; // show play
    
    if (self.isDragSeeking == NO && self.isMovieLoaded)
        [self.playheadView setHidden:NO];
    
    DDLogInfo(@"movie is playing: %d, at time: %f", self.selectedMovieIsPlaying, self.selectedMovieCurrentTime);
}


- (void)seekMovie:(NSTimeInterval)movieTime
{
	if (self.selectedMovieCurrentTime != movieTime)
	{
		// Sets the movie’s current time setting to time.
		self.selectedMovieCurrentTime = movieTime;
		[[self movie] setCurrentTime:QTMakeTimeWithTimeInterval(self.selectedMovieCurrentTime)];
	}
}


- (void)updateSelectedMovieCurrentTime
{
	NSTimeInterval movieCurrentTime = 0.0;
	
	if (self.movieView)
	{
		QTGetTimeInterval([self.movie currentTime], &movieCurrentTime);
	}
	
	self.selectedMovieCurrentTime = movieCurrentTime;
    
    if (self.isDragSeeking == NO) // don't have the playhead follow the current time if we're drag seeking
        [self movePlayheadToTime:movieCurrentTime];
    
    if (self.selectedMovieCurrentTime < self.currentHighlight.endTime)
        self.isPlayHeadBeforeEndTickView = YES;
    else if (self.isPlayHeadBeforeEndTickView)
    {
        DDLogInfo(@"Playhead just hit endTick");
        [self pauseMovie];
        self.isPlayHeadBeforeEndTickView = NO;
    }
    
    if (self.playheadView.rightCoordinate > self.timelineView.rightCoordinate)
    { // player hit the end of zoomed region
        [self pauseMovie];
        [self seekMovie:self.currentHighlight.beginTime];
    }
}

// Sent when the rate of a movie has changed (QTMovieRateDidChangeNotification)
- (void)movieRateChanged:(NSNotification *)notification
{
	[self updateSelectedMovieIsPlaying];
}


// timer routine called periodically to update the selected movie current time
- (void)checkMovieTime:(NSTimer *)timer
{
	[self updateSelectedMovieCurrentTime];
    
}


// called for QTMovieDidEndNotification when the movie is “done” or at its end.
- (void)movieDidEnd:(NSNotification *)note
{
    DDLogInfo(@"movie ended");
}


- (IBAction)changePlayState:(id)sender {
    if (self.isMovieLoaded)
        [self.movie setRate:(self.selectedMovieIsPlaying ? 0.0f : 1.0f)];
}

- (IBAction)deleteHighlight:(id)sender {
    
    NSString* errorMsg = @"Are you sure you want to remove this highlight?\n\nYou can get it back by rescanning your videos.";
    NSAlert *alert = [[NSAlert alloc] init];
    [alert addButtonWithTitle:@"Remove highlight"];
    [alert addButtonWithTitle:@"Cancel"];
    [alert setMessageText:@"Remove highlight?"];
    [alert setInformativeText:errorMsg];
    [alert setAlertStyle:NSWarningAlertStyle];
    NSInteger returnCode = [alert runModalSheetForWindow:[self window]];
    if (returnCode == NSAlertSecondButtonReturn)
        return;
    
    // see if publishing
    if ([self isPublishing])
    {
        NSString* errorMsg = @"This highlight is currently being published.\n\nWould you like to stop publishing and remove the highlight?";
        NSAlert *alert = [[NSAlert alloc] init];
        [alert addButtonWithTitle:@"Cancel"];
        [alert addButtonWithTitle:@"Stop publishing"];
        [alert setMessageText:@"Stop publishing?"];
        [alert setInformativeText:errorMsg];
        [alert setAlertStyle:NSWarningAlertStyle];
        NSInteger returnCode = [alert runModalSheetForWindow:[self window]];
        if (returnCode == NSAlertFirstButtonReturn)
            return;
        
        [self cancelPublishing];
    }
    
    NSInteger newIndex = [self.mainModel.highlightObjects indexOfObject:self.currentHighlight];
    
    [self.highlightDetailsDelegate removeHighlight:self.currentHighlight];
    
    if (newIndex == self.mainModel.highlightObjects.count) // we deleted last one
        newIndex = 0; // move to front
    if (self.mainModel.highlightObjects.count == 0) // no highlights left
        [self close];
    else
        [self setCurrentHighlight:[self.mainModel.highlightObjects objectAtIndex:newIndex]];
    
    
    
}

- (bool)isPublishing
{
    if (self.currentHighlight.saveWorker != nil && self.currentHighlight.saveWorker.isExecuting)
        return true;
    
    if (self.currentHighlight.facebookShareWorker != nil && self.currentHighlight.facebookShareWorker.isExecuting)
        return true;
    
    return false;
}

- (void)cancelPublishing
{
    if (self.currentHighlight.saveWorker != nil && self.currentHighlight.saveWorker.isExecuting)
        [self.currentHighlight.saveWorker cancelWorker];
    
    if (self.currentHighlight.facebookShareWorker != nil && self.currentHighlight.facebookShareWorker.isExecuting)
        [self.currentHighlight.facebookShareWorker cancelWorker];
    
    while ([self isPublishing]) { }
}

- (IBAction)showPreviousHighlight:(id)sender {
    NSInteger currentIndex = [self.mainModel.highlightObjects indexOfObject:self.currentHighlight];
    NSInteger newIndex = currentIndex - 1;
    if (newIndex < 0)
        newIndex = self.mainModel.highlightObjects.count - 1;
    
    HighlightObject* newHighlight = [self.mainModel.highlightObjects objectAtIndex:newIndex];
    [self setCurrentHighlight:newHighlight];
}

- (IBAction)showNextHighlight:(id)sender {
    NSInteger currentIndex = [self.mainModel.highlightObjects indexOfObject:self.currentHighlight];
    NSInteger newIndex = (currentIndex + 1) % self.mainModel.highlightObjects.count;
    HighlightObject* newHighlight = [self.mainModel.highlightObjects objectAtIndex:newIndex];
    [self setCurrentHighlight:newHighlight];
}

- (void)playMovie
{
    [self.movie setRate:1.0f];
}

- (void)pauseMovie
{
    [self.movie setRate:0.0f];
}


# pragma mark Timeline functions

- (void)updateTimelineBounds
{
    DDLogVerbose(@"called");
    
    [self.playheadView setLeftCoordinateBounds:self.timelineView.leftCoordinate];
    [self.playheadView setRightCoordinateBounds:self.timelineView.rightCoordinate];
}

- (void)updateTickCoordinates
{
    if (self.currentHighlight != nil)
    {
        [self moveBeginTickToTime:self.currentHighlight.beginTime];
        [self moveEndTickToTime:self.currentHighlight.endTime];
        [self moveBookmarkTickToTime:self.currentHighlight.bookmarkTime];
        [self updateTutorialBubbleCoordinates];
        [self updateHighlightTimeline];
    }
    
    if (self.movieView != nil)
        [self movePlayheadToTime:self.selectedMovieCurrentTime];
}


- (NSPoint)getCoordinateFromTime:(NSTimeInterval)time
{
    CGFloat percentage = ((time - self.zoomBeginTime) / (self.zoomEndTime - self.zoomBeginTime));
    NSPoint coordinate = NSMakePoint(self.timelineView.leftCoordinate + (NSInteger)(percentage * self.timelineView.width), 0);
    return coordinate;
}

- (void)movePlayheadToTime:(NSTimeInterval)time
{
    NSInteger newLeft = [self getCoordinateFromTime:time].x - (self.playheadView.width / 2);
    [self.playheadView setLeftCoordinate:newLeft];
}

- (void)moveBeginTickToTime:(NSTimeInterval)time
{
    NSInteger newLeft = [self getCoordinateFromTime:time].x - (self.beginTickView.width / 2);
    [self.beginTickView setLeftCoordinate:newLeft];
}

- (void)moveEndTickToTime:(NSTimeInterval)time
{
    NSInteger newLeft = [self getCoordinateFromTime:time].x - (self.endTickView.width / 2);
    [self.endTickView setLeftCoordinate:newLeft];
}

- (void)moveBookmarkTickToTime:(NSTimeInterval)time
{
    NSInteger newLeft = [self getCoordinateFromTime:time].x - (self.bookmarkTickView.width / 2);
    [self.bookmarkTickView setLeftCoordinate:newLeft];
}

- (NSInteger)getTimelineWidthFromTime:(NSTimeInterval)time
{
    NSInteger width = (NSInteger)(self.timelineView.width * (time / (self.zoomEndTime - self.zoomBeginTime))) + 1; // +1 due to casting
    return width;
}

- (void)updateHighlightTimeline
{
    // -5 and +10 are so the highlight can overlap underneath. without this, we saw some weird spacing issues.
    [self.highlightTimelineView setLeftCoordinate:self.beginTickView.rightCoordinate - 5];
    [self.highlightTimelineView setWidth:self.endTickView.leftCoordinate - self.highlightTimelineView.leftCoordinate + 10];
    
    // figure out highlight duration
    NSTimeInterval interval = self.currentHighlight.endTime - self.currentHighlight.beginTime;
    long min = (long)interval / 60;    // divide two longs, truncates
    long sec = (long)interval % 60;    // remainder of long divide
    NSString* prettyDuration = [NSString stringWithFormat:@"%01ld:%02ld", min, sec];
    [self.highlightDurationField setStringValue:prettyDuration];
}

- (NSTimeInterval)getTimeFromCoordinate:(NSPoint)coordinate
{
    CGFloat percentage = (CGFloat)(coordinate.x - self.timelineView.leftCoordinate) / self.timelineView.width;
    NSTimeInterval time = percentage * (self.zoomEndTime - self.zoomBeginTime) + self.zoomBeginTime;
    return time;
}


#pragma mark BeginTickView Delegate

- (void)setHighlightBeginTimeForCoordinate:(NSPoint)coordinate
{
    NSTimeInterval newTime = [self getTimeFromCoordinate:coordinate];
    self.currentHighlight.beginTime = newTime;
    
    //DDLogVerbose(@"currentHighlight.beginTime = %f", newTime);
}

- (void)beginTickDragStarted
{
    [self setIsDragSeeking:YES];
    [self setIsMoviePlayingBeforeSeeking:self.selectedMovieIsPlaying];
    [self pauseMovie];
    [self.playheadView setHidden:YES]; // hide while seeking
    [self setMovieTimeBeforeSeeking:self.selectedMovieCurrentTime];
    
    // set bounds of beginTick to be 1 second from endTick
    NSTimeInterval maxBeginTime = self.currentHighlight.endTime - 1; // 1 second in between
    
    NSInteger beginRightBounds = [self getCoordinateFromTime:maxBeginTime].x - (self.beginTickView.width);
    [self.beginTickView setRightCoordinateBounds:beginRightBounds];
    
    DDLogVerbose(@"beginTick.rightCoordinateBounds = %ld", beginRightBounds);
    
    [self.beginTickView setLeftCoordinateBounds:self.timelineView.leftCoordinate];
}

- (void)beginTickDragged
{
    [self updateHighlightTimeline];
    [self seekMovie:self.currentHighlight.beginTime];
}

- (void)beginTickDragEnded
{
    [self setIsDragSeeking:NO];
    [self seekMovie:self.currentHighlight.beginTime];
    [self movePlayheadToTime:self.currentHighlight.beginTime]; // so it doesnt appear at the old position for a split second
    if (self.isMoviePlayingBeforeSeeking)
        [self playMovie];
    
    if (self.isMovieLoaded)
        [self.playheadView setHidden:NO];
    
    if (self.handlesTutorialBubble != nil)
        [self advanceTutorialPastHandles];
}

- (void)beginZoomOut
{
    NSTimeInterval newZoomBeginTime = self.zoomBeginTime - zoomSecondsIncrement;
    if (newZoomBeginTime < 0)
        newZoomBeginTime = 0;
    self.zoomBeginTime = newZoomBeginTime;
    
    NSTimeInterval newBeginTime = self.currentHighlight.beginTime - zoomSecondsIncrement;
    if (newBeginTime < 0)
        newBeginTime = 0;
    self.currentHighlight.beginTime = newBeginTime;
    
    DDLogInfo(@"updated zoomBeginTime to %f", self.zoomBeginTime);
    
    self.timelineWidthBufferBetweenStartAndEnd = [self getTimelineWidthFromTime:1];
    
    [self moveBeginTickToTime:self.currentHighlight.beginTime];
    [self moveEndTickToTime:self.currentHighlight.endTime];
    [self moveBookmarkTickToTime:self.currentHighlight.bookmarkTime];
    [self movePlayheadToTime:self.selectedMovieCurrentTime];
    [self updateHighlightTimeline];
    
}

- (void)seekToHighlightBeginning
{
    [self seekMovie:self.currentHighlight.beginTime];
    [self playMovie];
}

# pragma mark EndTickView Delegate

- (void)setHighlightEndTimeForCoordinate:(NSPoint)coordinate
{
    NSTimeInterval newTime = [self getTimeFromCoordinate:coordinate];
    self.currentHighlight.endTime = newTime;
    
    DDLogVerbose(@"currentHighlight.endTime = %f", newTime);
}

- (void)endTickDragStarted
{
    [self setIsDragSeeking:YES];
    [self setIsMoviePlayingBeforeSeeking:self.selectedMovieIsPlaying];
    [self pauseMovie];
    [self.playheadView setHidden:YES]; // hide while seeking
    [self setMovieTimeBeforeSeeking:self.selectedMovieCurrentTime];
    
    // set bounds of endTick to be 1 second from beginTick
    NSTimeInterval minEndTime = self.currentHighlight.beginTime + 1; // 1 second in between
    
    NSInteger endLeftBounds = [self getCoordinateFromTime:minEndTime].x;
    [self.endTickView setLeftCoordinateBounds:endLeftBounds];
    
    DDLogVerbose(@"endTick.leftCoordinateBounds = %ld", endLeftBounds);
    
    [self.endTickView setRightCoordinateBounds:self.timelineView.rightCoordinate];
}

- (void)endTickDragged
{
    [self updateHighlightTimeline];
    [self seekMovie:self.currentHighlight.endTime];
}

- (void)endTickDragEnded
{
    [self setIsDragSeeking:NO];
    [self seekMovie:self.movieTimeBeforeSeeking];
    if (self.isMoviePlayingBeforeSeeking)
        [self playMovie];
    
    if (self.isMovieLoaded)
        [self.playheadView setHidden:NO];
    
    if (self.handlesTutorialBubble != nil)
        [self advanceTutorialPastHandles];
}

- (void)endZoomOut
{
    NSTimeInterval newZoomEndTime = self.zoomEndTime + zoomSecondsIncrement;
    if (newZoomEndTime > self.currentHighlight.inputFileObject.videoDuration)
        newZoomEndTime = self.currentHighlight.inputFileObject.videoDuration;
    self.zoomEndTime = newZoomEndTime;
    
    NSTimeInterval newEndTime = self.currentHighlight.endTime + zoomSecondsIncrement;
    if (newEndTime > self.currentHighlight.inputFileObject.videoDuration)
        newEndTime = self.currentHighlight.inputFileObject.videoDuration;
    self.currentHighlight.endTime = newEndTime;
    
    DDLogInfo(@"updated zoomEndTime to %f", self.zoomEndTime);
    
    self.timelineWidthBufferBetweenStartAndEnd = [self getTimelineWidthFromTime:1];
    
    [self moveBeginTickToTime:self.currentHighlight.beginTime];
    [self moveEndTickToTime:self.currentHighlight.endTime];
    [self moveBookmarkTickToTime:self.currentHighlight.bookmarkTime];
    [self movePlayheadToTime:self.selectedMovieCurrentTime];
    [self updateHighlightTimeline];
    
}


# pragma mark Playhead Delegate

- (void)playheadDragStarted
{
    [self setIsDragSeeking:YES];
    [self setIsMoviePlayingBeforeSeeking:self.selectedMovieIsPlaying];
    [self pauseMovie];
}

- (void)playheadDragged
{
    NSTimeInterval newTime = [self getTimeFromCoordinate:NSMakePoint(self.playheadView.leftCoordinate + (self.playheadView.width / 2), 0)];
    [self seekMovie:newTime];
    
    
    [self setMovieTimeBeforeSeeking:self.selectedMovieCurrentTime];
    
}

- (void)playheadDragEnded
{
    [self setIsDragSeeking:NO];
    if (self.isMoviePlayingBeforeSeeking)
        [self playMovie];
}

- (void)timelineClickedAtCoordinate:(NSPoint)coordinate
{
    
    NSTimeInterval newTime = [self getTimeFromCoordinate:coordinate];
    
    [self seekMovie:newTime];
    
    if (self.isMoviePlayingBeforeSeeking)
        [self playMovie];
}


# pragma mark Tutorial Methods

- (void)advanceTutorialPastBookmarkFlag
{
    [self.bookmarkFlagTutorialBubble setHidden:YES];
    
    [TutorialHelper advanceProgress];
    
    [self.tutorialEscapeHatchDelegate refreshTutorialProgress];
    
    [self initializeHandlesTutorialBubble];
}

- (void)advanceTutorialPastHandles
{
    [self.handlesTutorialBubble setHidden:YES];
    
    [TutorialHelper advanceProgress];
    
    [self.tutorialEscapeHatchDelegate refreshTutorialProgress];

    [self initializeShareButtonTutorialBubble];
}

- (void)advanceTutorialPastShareButton
{
    [self.shareButtonTutorialBubble setHidden:YES];
    
    [TutorialHelper advanceProgress];
    
    [self.tutorialEscapeHatchDelegate refreshTutorialProgress];

    [self initializeCloseDetailsTutorialBubble];
}

- (void)advanceTutorialPastCloseDetails
{
    [self.closeDetailsTutorialBubble setHidden:YES];
    
    [TutorialHelper advanceProgress];
    
    [self.tutorialEscapeHatchDelegate refreshTutorialProgress];

    [self.highlightDetailsDelegate initializeStartOverTutorialBubble];
    
    [self close];
    
}

- (void)hideTutorialBubbles
{
    if (self.bookmarkFlagTutorialBubble)
        [self.bookmarkFlagTutorialBubble setHidden:YES];
    
    if (self.handlesTutorialBubble)
        [self.handlesTutorialBubble setHidden:YES];

    if (self.shareButtonTutorialBubble)
        [self.shareButtonTutorialBubble setHidden:YES];

    if (self.closeDetailsTutorialBubble)
        [self.closeDetailsTutorialBubble setHidden:YES];
}



@end

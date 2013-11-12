//
//  SelectInputVideosViewController.m
//  OdessaMacPrototype
//
//  Created by Noah Spitzer-Williams on 8/22/12.
//
//

#import "SelectInputVideosViewController.h"

#if DEBUG
static const int ddLogLevel = LOG_LEVEL_VERBOSE | LOG_LEVEL_INFO | LOG_LEVEL_ERROR | LOG_LEVEL_WARN;
#else
static const int ddLogLevel = LOG_LEVEL_INFO | LOG_LEVEL_ERROR | LOG_LEVEL_WARN;
#endif


@interface SelectInputVideosViewController ()

@property (strong) ThumbnailGenerator* thumbnailGenerator;

@end



@implementation SelectInputVideosViewController
@synthesize scanButtonTutorialBubbleController = _scanButtonTutorialBubbleController;
@synthesize scanButtonTutorialBubble = _scanButtonTutorialBubble;

- (id)initWithNibName:(NSString *)nibNameOrNil bundle:(NSBundle *)nibBundleOrNil
{
    self = [super initWithNibName:nibNameOrNil bundle:nibBundleOrNil];
    if (self) {
        // Initialization code here.        
        self.thumbnailGenerator = [ThumbnailGenerator new];
    }
    
    return self;
}

- (void)awakeFromNib
{
    
    [self.inputFilesDropView setInputFilesDropDelegate:self];
    
    [self.inputFilesCollectionView setGenericDropHereDelegate:self];
    [self.inputFilesCollectionView setRemoveInputFileDelegate:self];
    
    // these two seem to have no effect. the delegates are passed through the CollectionView
    [self.inputVideoThumbnailViewController setGenericDropHereDelegate:self];
    [self.inputVideoThumbnailViewController setRemoveInputFileDelegate:self];
    
    [self.inputVideoThumbnailView setInputFilesDropDelegate:self];
    [self.inputVideoThumbnailView setSmallDropHereDelegate:self.inputVideoThumbnailViewController];
    
    [self.inputFilesDropView setSmallDropHereDelegate:self.inputVideoThumbnailViewController];
    
    [self.scanAllVideosButton setHoverImage:[NSImage imageNamed:@"button-green-scanvideos-hover.png"]];
    
    [self initializeBigDropHereView];
    
    // in case there are any inputFileObjects from the previous scan (because user cancelled scan)
    if (self.mainModel.inputFileObjects.count > 0)
    {
        InputFileObject* smallDropHereObject;
        for (InputFileObject* inputFileObject in self.mainModel.inputFileObjects)
        {
            if (inputFileObject.isSmallDropHereControl == NO)
                [self.arrayController addObject:inputFileObject];
            else
                smallDropHereObject = inputFileObject;
        }
        
        if (smallDropHereObject != nil)
            [self.arrayController addObject:smallDropHereObject];
        
        [self.bigDropHereView setHidden:YES];
        [self.scanAllVideosButton setHidden:NO];
    }
    
    
    [self initializeSampleVideoTutorialBubble];
    
    [self initializeScanButtonTutorialBubble];
    

    
    /* this doesnt work
    [self.inputFilesDropView.layer setCornerRadius:30.0f];
    //[self.inputFilesDropView.layer setBorderColor: [NSColor lightGrayColor].CGColor];
    [self.inputFilesDropView.layer setBorderWidth:1.5f];
    //[self.inputFilesDropView.layer setShadowColor:[NSColor blackColor].CGColor];
    [self.inputFilesDropView.layer setShadowOpacity:0.8];
    [self.inputFilesDropView.layer setShadowRadius:3.0];
    [self.inputFilesDropView.layer setShadowOffset:CGSizeMake(2.0, 2.0)];
    
    self.inputFilesDropView.layer.masksToBounds = YES;
    //self.inputFilesDropView.layer.opaque = NO;
    
     
     [self.inputFilesDropView setNeedsDisplay:YES];
     */
    
}


- (void)initializeSampleVideoTutorialBubble
{
    TutorialProgress progress = [TutorialHelper getProgress];
    if (progress != TutorialAddSampleVideo)
    {
        DDLogVerbose(@"Skipping AddSampleVideo tutorial. Progress = %d", progress);
        return;
    }
    
    if (self.sampleVideoTutorialBubble.subviews.count > 0) // already exists. user tried restarting tutorial while it was already there
        return;
    
    self.sampleVideoTutorialBubbleController = [_sampleVideoTutorialBubbleController initWithNibName:@"TutorialBubble" bundle:nil];
    
    [_sampleVideoTutorialBubble addSubview:[_sampleVideoTutorialBubbleController view]];
    
    [self.sampleVideoTutorialBubbleController loadTooltip];
    
    [[self.sampleVideoTutorialBubbleController view] setFrameSize:self.sampleVideoTutorialBubble.bounds.size];

    [self.sampleVideoTutorialBubbleController setActionTarget:self];
    [self.sampleVideoTutorialBubbleController setActionSelector:@selector(advanceTutorialPastSampleVideo)];
    
    [self.sampleVideoTutorialBubble setHidden:NO];

    [self performSelector:@selector(scanSampleVideo) withObject:nil afterDelay:0.2];
    
}

- (void)initializeScanButtonTutorialBubble
{
    TutorialProgress progress = [TutorialHelper getProgress];
    if (progress != TutorialScanButton)
    {
        DDLogVerbose(@"Skipping TutorialScanButton tutorial. Progress = %d", progress);
        return;
    }
    
    self.scanButtonTutorialBubbleController = [_scanButtonTutorialBubbleController initWithNibName:@"TutorialBubble" bundle:nil];
    
    [_scanButtonTutorialBubble addSubview:[_scanButtonTutorialBubbleController view]];
    
    [self.scanButtonTutorialBubbleController loadTooltip];

    [[self.scanButtonTutorialBubbleController view] setFrameSize:self.scanButtonTutorialBubble.bounds.size];
    
    [self.scanButtonTutorialBubbleController setActionTarget:self];
    [self.scanButtonTutorialBubbleController setActionSelector:@selector(advanceTutorialPastScanButton)];
    
    [self.scanButtonTutorialBubble setHidden:NO];

    // make sure the sample video is there
    // this case happens when user loads app when this is the next tutorial step
    [self performSelectorInBackground:@selector(scanSampleVideo) withObject:nil];

    
}

- (void)initializeBigDropHereView
{
    self.bigDropHereViewController = [_bigDropHereViewController initWithNibName:@"BigDropHereView" bundle:nil];
    
    [_bigDropHereView addSubview:[_bigDropHereViewController view]];
    
    [[self.bigDropHereViewController view] setFrameSize:self.bigDropHereView.bounds.size];
    
    [self.bigDropHereViewController setInputFilesDropDelegate:self];
    [self.bigDropHereViewController setGenericDropHereDelegate:self];
    [self.inputFilesDropView setBigDropHereDelegate:self.bigDropHereViewController];
    

}

- (void)hideTutorialBubbles
{
    if (self.sampleVideoTutorialBubble)
        [self.sampleVideoTutorialBubble setHidden:YES];
    
    if (self.scanButtonTutorialBubble)
        [self.scanButtonTutorialBubble setHidden:YES];
    
    for (InputFileObject* inputFileObject in self.mainModel.inputFileObjects)
        inputFileObject.removeButtonAlwaysHidden = NO;
    
    if (self.mainModel.inputFileObjects > 0)
        [self addSmallDropHereObject];
}

- (void)scanSampleVideo
{
    
    [self inputFileAdded:[MainModel getURLToSampleVideo]];
    
}

- (void)advanceTutorialPastSampleVideo
{
    [self.sampleVideoTutorialBubble setHidden:YES];
    
    [TutorialHelper advanceProgress];
    
    [self.tutorialEscapeHatchDelegate refreshTutorialProgress];

    [self initializeScanButtonTutorialBubble];
    
}

- (void)advanceTutorialPastScanButton
{
    [self.scanButtonTutorialBubble setHidden:YES];
    
    [TutorialHelper advanceProgress];
    
    [self.tutorialEscapeHatchDelegate refreshTutorialProgress];
    
    [self scanAllVideos:nil];
}

- (void)selectFromCamera
{
    DDLogInfo(@"called");
    
    NSAlert* alert = [NSAlert alertWithMessageText:@"Not implemented yet" defaultButton:@"OK" alternateButton:nil otherButton:nil informativeTextWithFormat:@""];
    [alert runModal];
}

- (void)browseComputer
{
    DDLogInfo(@"called");
    
    NSOpenPanel* openPanel = [NSOpenPanel openPanel];
    openPanel.message = @"Which videos would you like to scan for highlights?";
    openPanel.canChooseFiles = YES;
    openPanel.canChooseDirectories = YES;
    openPanel.allowsMultipleSelection = YES;
    openPanel.allowedFileTypes = @[@"public.movie", @"MTS", @"mts", @"M2TS", @"m2ts"];
    openPanel.resolvesAliases = YES;
    
    NSUserDefaults *standardUserDefaults = [NSUserDefaults standardUserDefaults];
    if (standardUserDefaults)
    {
        openPanel.directoryURL = [NSURL fileURLWithPath:[standardUserDefaults objectForKey:@"LastOpenPath"]];
    }
    else
        openPanel.directoryURL = [MainModel getDefaultFolderURL];
    
    [openPanel beginSheetModalForWindow:self.view.window completionHandler:^(NSInteger result) {
        if (result == NSFileHandlingPanelOKButton)
        {
            [standardUserDefaults setObject:[MainModel getParentFolder:openPanel.URLs[0]].path forKey:@"LastOpenPath"];
            [standardUserDefaults synchronize];
            
            BOOL isVideoAdded = NO;
            for (NSURL* url in openPanel.URLs)
            {
                if ([self inputFileAdded:url])
                    isVideoAdded = YES;
            }
            
            if (isVideoAdded)
            {
                [AnalyticsHelper fireEvent:@"Each select - browse" eventValue:0];
            }

        }
    }];
        
}

- (void)addInputFilesDropViewObject:(id *)object
{
    DDLogInfo(@"called");
}

- (BOOL)inputFileAdded:(NSURL *)url
{
    DDLogInfo(@"called with %@", url);
    
    NSNumber *isDirectory = nil;
    NSError *error = nil;
    if (![url getResourceValue:&isDirectory forKey:NSURLIsDirectoryKey error:&error])
    {
        DDLogError(@"Error: %@", error);
        return false;
    }
    
    if ([isDirectory boolValue])
    {
        NSArray *properties = @[ NSURLLocalizedNameKey,
        NSURLNameKey];
        NSArray *array = [[NSFileManager defaultManager]
                          contentsOfDirectoryAtURL:url
                          includingPropertiesForKeys:properties
                          options:(NSDirectoryEnumerationSkipsHiddenFiles)
                          error:&error];
        if (array != nil)
        {
            for (NSURL* subFile in array)
            {
                [self inputFileAdded:subFile];
            }
        }
        if (error != nil)
        {
            DDLogError(@"Error: %@", error);
        }
        
        return false;
    }
    else
    { // not a directory, so let's add it
        
        
        if ([self isValidVideo:url] == NO)
            return false;
        
        if ([self isAlreadySelected:url])
            return false;
        
        if (self.mainModel.inputFileObjects.count == 0
            && [TutorialHelper getProgress] != TutorialAddSampleVideo)
        {
            [self addSmallDropHereObject];
        }
        
        InputFileObject* inputFileObject = [InputFileObject new];
        [inputFileObject setSourceURL:url];
        
        NSString* newTitle = url.lastPathComponent.stringByDeletingPathExtension;
        
        [inputFileObject setTitle:newTitle];
        
        [inputFileObject generateThumbnail:self.thumbnailGenerator];
        
        [self.mainModel addInputFileObjectsObject:inputFileObject];
        
        NSInteger insertedIndex = self.mainModel.inputFileObjects.count - 2;

        if ([TutorialHelper getProgress] == TutorialAddSampleVideo)
            insertedIndex++; // account for smallDropHere not existing

        [self.arrayController insertObject:inputFileObject atArrangedObjectIndex:insertedIndex];
        
//        [self moveSmallDropHereObjectToEnd];
                
        [self.bigDropHereView setHidden:YES];
        [self.scanAllVideosButton setHidden:NO];
        
        [self trackFirstSelect];
        
        return true;
    }
}


- (BOOL)isAlreadySelected:(NSURL*)url
{
    for (InputFileObject* inputFileObject in self.mainModel.inputFileObjects)
    {
        if ([inputFileObject.sourceURL.path isEqualToString:url.path])
             return true;
    }
    return false;
}


- (void)addSmallDropHereObject
{
    InputFileObject* smallDropHereObject = [InputFileObject new];
    smallDropHereObject.isSmallDropHereControl = YES;
    [self.mainModel addInputFileObjectsObject:smallDropHereObject];
    [self.arrayController addObject:smallDropHereObject];
}


- (BOOL)isValidVideo:(NSURL*)url
{
    NSWorkspace *workspace = [NSWorkspace sharedWorkspace];
    

    NSString *type;
    NSError *error;
    if ([url getResourceValue:&type forKey:NSURLTypeIdentifierKey error:&error]) {
        if ([workspace type:type conformsToType:@"public.movie"]) {
            // the URL points to a movie; do stuff here
            return true;
        }
        else
        {
            NSArray* supportedExtensions = @[@"MTS", @"M2TS"];
            NSString* ourExtension = url.pathExtension.uppercaseString;
            for (NSString* extension in supportedExtensions)
            {
                if ([extension isEqualToString:ourExtension])
                    return true;
            }
            
            return false;
        }
    } else {
        // handle error
        return false;
    }
}

         

- (IBAction)scanAllVideos:(id)sender {
    
    TutorialProgress progress = [TutorialHelper getProgress];
    if (progress == TutorialAddSampleVideo)
    { // skip two steps since user knows what they're doing
        [self advanceTutorialPastSampleVideo];
        progress = [TutorialHelper getProgress];
    }
    if (progress == TutorialScanButton)
    {
        [self advanceTutorialPastScanButton];
        return; // so we don't call scanStarted twice
    }

    [self.delegate scanStarted];
    
}


- (void)removeInputFile:(InputFileObject *)inputFileObject
{
    [self.mainModel.inputFileObjects removeObject:inputFileObject];
    [self.arrayController removeObject:inputFileObject];
    
    if (self.mainModel.inputFileObjects.count == 1)
    { // only small drop here object is left so reset everything
        [self.mainModel.inputFileObjects removeAllObjects];

        NSRange range = NSMakeRange(0, [[self.arrayController arrangedObjects] count]);
        [self.arrayController removeObjectsAtArrangedObjectIndexes:[NSIndexSet indexSetWithIndexesInRange:range]];
        
        [self.bigDropHereView setHidden:NO];
        [self.scanAllVideosButton setHidden:YES];
    }
}


- (void)trackFirstSelect
{
    NSUserDefaults* standardUserDefaults = [NSUserDefaults standardUserDefaults];
    if (standardUserDefaults)
    {
        if ([standardUserDefaults boolForKey:@"HasSelectedVideo"] == NO)
        {
            if ([AnalyticsHelper fireEvent:@"First select" eventValue:0])
            {
                [standardUserDefaults setBool:YES forKey:@"HasSelectedVideo"];
                [standardUserDefaults synchronize];
            }
        }
    }
    
}


@end

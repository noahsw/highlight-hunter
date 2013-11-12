//
//  ScanViewController.m
//  OdessaMacPrototype
//
//  Created by Noah Spitzer-Williams on 8/22/12.
//
//

#import "ScanVideosViewController.h"


#if DEBUG
static const int ddLogLevel = LOG_LEVEL_VERBOSE | LOG_LEVEL_INFO | LOG_LEVEL_ERROR | LOG_LEVEL_WARN;
#else
static const int ddLogLevel = LOG_LEVEL_INFO | LOG_LEVEL_ERROR | LOG_LEVEL_WARN;
#endif


@interface ScanVideosViewController ()

@property (strong) NSTimer* progressTimer;

@property (strong) ScanWorkerHost* scanWorkerHost;

@property NSInteger updateTimeRemainingCounter;

- (void)progressTimerFired:(NSTimer*)timer;

@end

@implementation ScanVideosViewController



- (id)initWithNibName:(NSString *)nibNameOrNil bundle:(NSBundle *)nibBundleOrNil
{
    self = [super initWithNibName:nibNameOrNil bundle:nibBundleOrNil];
    if (self) {
        // Initialization code here.
        self.updateTimeRemainingCounter = 0;
        self.isScanning = NO;
    }
    
    return self;
}

- (void)awakeFromNib
{
    [self.cancelButton setHoverImage:[NSImage imageNamed:@"button-gray-cancel-hover.png"]];
    
}

- (void)startScan
{
    DDLogInfo(@"called");
    
    [self loadSettings];
    [self trackSettingsUsed];
    
    
#ifdef APPSTORE_FREE
    [AnalyticsHelper fireEvent:@"Each scan - activation state - unlicensed" eventValue:0];
#endif
    
#ifdef APPSTORE_PREMIUM
    [AnalyticsHelper fireEvent:@"Each scan - activation state - activated" eventValue:0];
#endif
    
    
    // reset so we calculate progress appropriately
    for (InputFileObject* inputFileObject in self.mainModel.inputFileObjects)
    {
        inputFileObject.scanWorkerResult = ScanWorkerResultNotFinished;
    }
    
    self.isScanning = YES;
    
    [AnalyticsHelper fireEvent:@"Each scan - input file count" eventValue:@(self.mainModel.inputFileObjects.count - 1)]; // minus 1 because 1 is for the small drop
    
    [AnalyticsHelper fireEvent:@"Scan" eventValue:@(self.mainModel.inputFileObjects.count - 1)];

    // NSOperationQueue makes it easy to run this on a separate thread
    NSOperationQueue* operationQueue = [NSOperationQueue new];
    
    self.scanWorkerHost = [ScanWorkerHost new];
    [self.scanWorkerHost setMainModel:self.mainModel];
    [self.scanWorkerHost setDelegate:self];
    
    [operationQueue addOperation:self.scanWorkerHost];
    
    self.progressTimer = [NSTimer scheduledTimerWithTimeInterval:0.5 target:self selector:@selector(progressTimerFired:) userInfo:nil repeats:YES];
    
    
}

- (IBAction)cancelScan:(id)sender {
    
    [self.scanWorkerHost cancel];
    
}

- (void)loadSettings
{
    NSUserDefaults *standardUserDefaults = [NSUserDefaults standardUserDefaults];
    if (standardUserDefaults)
    {
        NSInteger detectionThreshold = 2;
        detectionThreshold = [standardUserDefaults integerForKey:@"DetectionThreshold"];
        [self.mainModel setDetectionThreshold:detectionThreshold];
        DDLogInfo(@"Set detectionThreshold to %ld", detectionThreshold);
        
        NSTimeInterval captureDuration = [standardUserDefaults integerForKey:@"CaptureDurationInSeconds"];
        [self.mainModel setCaptureDurationInSeconds:captureDuration];
        DDLogInfo(@"Set captureDurationInSeconds to %f", captureDuration);
        
        BOOL ignoreEarlyHighlights = [standardUserDefaults boolForKey:@"IgnoreEarlyHighlights"];
        [self.mainModel setIgnoreEarlyHighlights:ignoreEarlyHighlights];
        DDLogInfo(@"Set ignoreEarlyHighlights to %d", ignoreEarlyHighlights);
        
    }
    
}

- (void)trackSettingsUsed
{
    [AnalyticsHelper fireEvent:[NSString stringWithFormat:@"Each scan - setting - sensitivity - %ld", self.mainModel.detectionThreshold] eventValue:0];
    
    [AnalyticsHelper fireEvent:@"Each scan - setting - capture duration" eventValue:@(self.mainModel.captureDurationInSeconds)];
        
    [AnalyticsHelper fireEvent:@"Each scan - setting - ignore early highlights" eventValue:@(self.mainModel.ignoreEarlyHighlights)];
     
}

- (void)progressTimerFired:(NSTimer *)timer
{
    
    if (self.updateTimeRemainingCounter == 0)
        self.timeRemainingField.stringValue = [self.scanWorkerHost getTimeRemaining];
    
    self.updateTimeRemainingCounter = (self.updateTimeRemainingCounter++ % 10);
    
    NSInteger progressValue = 0;
    if ([self.scanWorkerHost getProgressValue:&progressValue])
    {
        self.progressIndicator.doubleValue = progressValue;
//        DDLogInfo(@"progressvalue: %ld", progressValue);
    }
}

- (void)scanWorkerHostProgressUpdated:(NSInteger)progress
{
    self.progressIndicator.doubleValue = progress;
}

- (void)scanWorkerHostStatusUpdated:(NSString *)status
{
    self.statusField.stringValue = status;
}

- (void)scanWorkerHostFinished
{
    DDLogInfo(@"finished!");
    
    [self.progressTimer invalidate];
    
    [NSApp requestUserAttention:NSInformationalRequest];
    
    [self.delegate scanFinished];
    
    
    self.isScanning = NO;
}

- (void)scanWorkerHostCancelled
{
    DDLogInfo(@"cancelled!");
    
    [self.progressTimer invalidate];
    [self.delegate scanCancelled];
    
    self.isScanning = NO;
}

@end

//
//  FacebookShareWorker.m
//  OdessaMacGUIApp
//
//  Created by Noah Spitzer-Williams on 9/4/12.
//  Copyright (c) 2012 Authentically Digital LLC. All rights reserved.
//

#import "FacebookShareWorker.h"

#if DEBUG
static const int ddLogLevel = LOG_LEVEL_VERBOSE | LOG_LEVEL_INFO | LOG_LEVEL_ERROR | LOG_LEVEL_WARN;
#else
static const int ddLogLevel = LOG_LEVEL_INFO | LOG_LEVEL_ERROR | LOG_LEVEL_WARN;
#endif


@interface FacebookShareWorker ()

// Represents when video is finished uploading
// Set to YES from async callback method requestResult
@property BOOL uploadCompleted;

// Represents when we have a valid auth token from FB API
// Set to YES from async callback method tokenResult
@property BOOL gotValidToken;
@property BOOL gotInvalidToken;

// Represents when we got user's info from FB API
// Set to YES from async callback method requestResult
@property BOOL gotUserInfo;

@end

#define kFBStoreAccessToken @"FBAStoreccessToken"
#define kFBStoreTokenExpiry @"FBStoreTokenExpiry"
#define kFBStoreAccessPermissions @"FBStoreAccessPermissions"


@implementation FacebookShareWorker



+ (FacebookShareWorker*)workerWithHighlight:(HighlightObject*) highlightObject
{
    FacebookShareWorker* newWorker = [[super alloc] init];
    if (newWorker)
    {
        newWorker.highlightObject = highlightObject;
        newWorker.facebookClient = [[PhFacebook alloc] initWithApplicationID: [FacebookShareWorker applicationID] delegate: newWorker];
        
    }
    return newWorker;
}


// Main method runs when NSOperation is told to run
- (void)main
{
    
#ifdef APPSTORE_FREE
    [AnalyticsHelper fireEvent:@"Each share - activation state - unlicensed" eventValue:0];
#endif
    
#ifdef APPSTORE_PREMIUM
    [AnalyticsHelper fireEvent:@"Each share - activation state - activated" eventValue:0];
#endif
    
    
    // Prepare video
    SaveWorker* saveWorker = [SaveWorker workerWithHighlight:self.highlightObject];
    
    NSString* filename = [NSString stringWithFormat:@"%@.%@", [FacebookShareWorker UUIDString], self.highlightObject.inputFileObject.sourceURL.pathExtension];
    NSURL* tempURL = [NSURL fileURLWithPathComponents:@[NSTemporaryDirectory(), filename]];
    
    saveWorker.outputFileURL = tempURL;
    saveWorker.outputFormat = OutputFormatFacebook;
    saveWorker.workerProgressDelegateForPublishStatus = self;

    NSOperationQueue* operationQueue = [NSOperationQueue new];
    [operationQueue addOperation:saveWorker];
    
    if ([self initializeFacebookClient] == NO)
    {
        DDLogError(@"Couldn't initialize Facebook client. Cancelling worker.");
        if (self.publishWorkerResult == PublishWorkerResultNotFinished) // might have already been set to cancel
            self.publishWorkerResult = PublishWorkerResultUnableToAuthenticate;
        [saveWorker cancelWorker];
        [super reportProgress:self.progress];
        return;
    }
    
    // wait for highlight to be trimmed by saveWorker before we try uploading it
    // this entire method is running as a background thread so we won't block the UI
    while (saveWorker.isExecuting)
    {
        if ([self checkForCancelledWorker])
        {
            [saveWorker cancelWorker];
            while (saveWorker.isExecuting) { } // wait to finish.
            self.publishWorkerResult = PublishWorkerResultCancelled;
            [super reportProgress:self.progress];
            return;
        }

        [NSThread sleepForTimeInterval:0.5];
    }
    
    DDLogInfo(@"SaveWorker done");
    
    // check if saveWorker reported an error
    if (saveWorker.publishWorkerResult != PublishWorkerResultSuccess ||
        saveWorker.outputFileURL == nil)
    {
        self.errorMessage = saveWorker.errorMessage;
        self.publishWorkerResult = PublishWorkerResultUnableToShare;
        [super reportProgress:self.progress];
        return;
    }
    
    
    // Upload to Facebook
    NSDictionary* params = @{
        @"source" : saveWorker.outputFileURL.path,
        @"title" : self.highlightObject.title,
        @"description" : self.highlightObject.title
        };
    
    [self.facebookClient sendRequest:@"/me/videos" params:params usePostRequest:YES];

    DDLogInfo(@"Posted Facebook video in background");
    
    [self trackFirstShare];
    
    // wait for requestResult callback to be called
    // this entire method is running as a background thread so we won't block the UI
    while (!self.uploadCompleted)
    {
        if ([self checkForCancelledWorker])
        {
            [self.facebookClient cancelRequest];
            self.publishWorkerResult = PublishWorkerResultCancelled;
            [super reportProgress:self.progress];
            break;
        }
        
        [NSThread sleepForTimeInterval:0.5];
    }
    
    
    // Cleanup the temporary trimmed highlight video
    NSFileManager* fileManager = [NSFileManager defaultManager];
    [fileManager removeItemAtURL:saveWorker.outputFileURL error:nil];
    
    
    [AnalyticsHelper fireEvent:[NSString stringWithFormat:@"Each share - Facebook - result - %@", [self getFriendlyPublishWorkerResult]] eventValue:0];
}


// Called when user wants to view video that was shared
- (void)viewResult
{
    if ([self.highlightObject.facebookActivityId isEqualToString:@""])
    {
        DDLogError(@"No FacebookActivityId!");
        return;
    }
    
    [BrowserHelper launchBrowser:[NSString stringWithFormat:@"https://www.facebook.com/photo.php?v=%@", self.highlightObject.facebookActivityId] term:@"" addDomain:NO];
}


// Initializes Facebook Client by asking user to log in and then verifying valid token
// Returns success or not
- (BOOL)initializeFacebookClient
{
    
    [self performSelectorOnMainThread:@selector(getAccessToken) withObject:nil waitUntilDone:YES];
    
    // wait until user logs in
    while (!self.gotValidToken && !self.gotInvalidToken)
    {
        if ([self checkForCancelledWorker])
        {
            [self.facebookClient cancelRequest];
            self.publishWorkerResult = PublishWorkerResultCancelled;
            [super reportProgress:self.progress];
            break;
        }
        
        [NSThread sleepForTimeInterval:0.2];
    }
    
    if (self.gotInvalidToken)
    {
        DDLogError(@"Couldn't get access token");
        return false;
    }
    
    if ([self triggerFacebookConnect])
    {
        return true;
    }
    else
    {
        DDLogError(@"Facebook access token expired!");
        
        self.publishWorkerResult = PublishWorkerResultUnableToAuthenticate;
        return false;
    }
    
}


// This method attempts to look up the user's info from Facebook
// This tests whether the authentication token is valid
// Returns success or not
- (BOOL)triggerFacebookConnect
{
    
    self.gotUserInfo = NO;
    
    NSDictionary* params = @{
    @"": @"name",
    @"": @"first_name",
    @"": @"last_name",
    @"": @"email"
    };
    
    [self.facebookClient sendRequest:@"/me" params:params usePostRequest:NO];
    
    while (!self.gotUserInfo)
    {
        if ([self checkForCancelledWorker])
        {
            [self.facebookClient cancelRequest];
            self.publishWorkerResult = PublishWorkerResultCancelled;
            [super reportProgress:self.progress];
            break;
        }
        
        [NSThread sleepForTimeInterval:0.2];
    }
    
    return true;
}


// Asks user to login to Facebook and sets access token in facebookClient
- (void)getAccessToken
{
    [self.facebookClient getAccessTokenForPermissions:@[@"publish_actions", @"email"] cached:YES modalWindow:nil];
}


// Called when user logs into Facebook
- (void)tokenResult:(NSDictionary *)result
{
    if ([[result valueForKey: @"valid"] boolValue])
    {
        DDLogInfo(@"Got a valid token");
        self.gotValidToken = YES;
    }
    else
    {
        DDLogError(@"Error getting token: %@", [result valueForKey:@"error"]);
        self.gotInvalidToken = YES;
    }
    
}


// Callback from FacebookClient
- (void)requestResult:(NSDictionary *)result
{
    NSString* jsonString = [result objectForKey:@"result"];
    
    if ([jsonString isEqualToString:@""] || jsonString == nil)
    { // Response was empty. Something bad happened but we don't know what.
        
        if (self.publishWorkerResult != PublishWorkerResultCancelled)
        {
            self.errorMessage = @"Something bad happened but we don't know what. You up for trying again?";
            self.publishWorkerResult = PublishWorkerResultUnableToShare;
        }
        
        self.uploadCompleted = YES;
        
        [super reportProgress:100]; // force update of controls
        
        return;
    }
    
    id jsonObject = [jsonString JSONValue];
    
    if ([jsonObject isKindOfClass:[NSArray class]]) {
        NSAssert(false, @"We don't expect an NSArray");
        
        DDLogInfo(@"its an array!");
        NSArray *jsonArray = (NSArray *)jsonObject;
        DDLogInfo(@"jsonArray - %@",jsonArray);
        
        return;
    }

    DDLogInfo(@"response is probably a dictionary");
    
    NSDictionary *jsonDictionary = (NSDictionary *)jsonObject;
    
    DDLogInfo(@"jsonDictionary - %@",jsonDictionary);
    
    if ([jsonDictionary objectForKey:@"email"] != nil)
    {
        
        // This is a response for asking Facebook for the user's email info
        // We post this to our web service so we can track usage
        
        @try
        { // we should never crash from this part
        
            [self postUserInfoToService:[jsonDictionary objectForKey:@"name"] first_name:[jsonDictionary objectForKey:@"first_name"] last_name:[jsonDictionary objectForKey:@"last_name"] email:[jsonDictionary objectForKey:@"email"]];
            
            DDLogInfo(@"Triggered");
            
        }
        @catch (NSException* ex) {
            DDLogError(@"Exception %@", ex);
            self.gotUserInfo = YES; // to break out of loop
        }
        
    }
    else if ([jsonDictionary objectForKey:@"id"] != nil)
    {
        
        // This is a response for posting the video
        // we only want to trigger this conditional if "email" is not part of jsonDictionary
        
        NSString* facebookActivityId = [jsonDictionary objectForKey:@"id"];
        self.highlightObject.facebookActivityId = facebookActivityId;
        
        DDLogInfo(@"FacebookActivityId: %@", facebookActivityId);
        
        self.publishWorkerResult = PublishWorkerResultSuccess;
        
        self.uploadCompleted = YES;
        
        [super reportProgress:100]; // force update of controls
    }
    else
    { // it was cancelled
        [super reportProgress:100]; // force update of controls
    }
    

}


// Called when saveWorker updates
- (void)workerProgressUpdated:(NSInteger)newProgress
{
    
    if (!self.gotValidToken)
        return; // hide progress until we have a valid token
    
    [super reportProgress:newProgress * 0.20];
}


// Called when facebookclient updates
- (void)uploadProgressChanged:(NSInteger)newProgress
{
    [super reportProgress: 20 + newProgress * 0.80];
}


// Analytics tracking
- (void)trackFirstShare
{
    NSUserDefaults* standardUserDefaults = [NSUserDefaults standardUserDefaults];
    if (standardUserDefaults)
    {
        if ([standardUserDefaults boolForKey:@"HasSharedVideo"] == NO)
        {
            if ([AnalyticsHelper fireEvent:@"First share" eventValue:0])
            {
                [standardUserDefaults setBool:YES forKey:@"HasSharedVideo"];
                [standardUserDefaults synchronize];
            }
        }
        
        if ([standardUserDefaults boolForKey:@"HasSharedVideoToFacebook"] == NO)
        {
            if ([AnalyticsHelper fireEvent:@"First share to facebook" eventValue:0])
            {
                [standardUserDefaults setBool:YES forKey:@"HasSharedVideoToFacebook"];
                [standardUserDefaults synchronize];
            }
        }
    }
    
}


// Posts user's info to our service so we can track usage
//
- (void)postUserInfoToService:(NSString*)name first_name:(NSString*)first_name last_name:(NSString*)last_name email:(NSString*)email
{
    if (name.length == 0 || first_name.length == 0 || last_name.length == 0 || email.length == 0)
        return;
    
    NSURL* url = [NSURL URLWithString:[NSString stringWithFormat:@"%@/facebook-connect-trigger.php", [BrowserHelper host]]];
    
    DDLogVerbose(@"Posting to %@", url);
    
    DDLogInfo(@"Triggering Facebook Connect");
    
    __unsafe_unretained ASIFormDataRequest *request = [ASIFormDataRequest requestWithURL:url];
    [request setPostValue:name forKey:@"name"];
    [request setPostValue:first_name forKey:@"firstName"];
    [request setPostValue:last_name forKey:@"lastName"];
    [request setPostValue:email forKey:@"email"];
    
    NSDictionary* infoDict = [[NSBundle mainBundle] infoDictionary];
    NSString* version = [NSString stringWithFormat:@"Mac %@", infoDict[@"CFBundleShortVersionString"]];
    [request setPostValue:version forKey:@"version"];
    
    [request setCompletionBlock:^{
        // Use when fetching text data
        NSString *responseString = [request responseString];
        DDLogVerbose(@"Response: %@", responseString);
        DDLogInfo(@"Facebook Connect responded");
        
        self.gotUserInfo = YES;
        // Use when fetching binary data
        //NSData *responseData = [request responseData];
    }];
    
    [request setFailedBlock:^{
        NSError *error = [request error];
        DDLogError(@"Error: %@", error);
        
        self.gotUserInfo = YES;
    }];
    
    [request startAsynchronous];
}


// Used to generate unique UUID for user
+ (NSString*)UUIDString
{
    CFUUIDRef uuidObj = CFUUIDCreate(nil);
    NSString *uuidString = (__bridge_transfer NSString*)CFUUIDCreateString(nil, uuidObj);
    CFRelease(uuidObj);
    return uuidString;
}


+ (NSString*)applicationID
{
    return @"228803620534906";
}


// Called when user tries viewing video for first time
+ (void)handleEncodingWarning:(NSWindow *)window
{
    NSUserDefaults* standardUserDefaults = [NSUserDefaults standardUserDefaults];
    if (standardUserDefaults)
    {
        if ([standardUserDefaults boolForKey:@"HasSeenFacebookEncodingWarning"] == NO)
        {
            NSString* errorMsg = @"Your video has been uploaded but sometimes Facebook needs a few minutes before you can watch it. Grab a sandwich and you should be good to go.\n\nWe won't bug you about this again.";
            NSAlert *alert = [[NSAlert alloc] init];
            [alert addButtonWithTitle:@"Continue"];
            [alert setMessageText:@"Heads up"];
            [alert setInformativeText:errorMsg];
            [alert setAlertStyle:NSInformationalAlertStyle];
            [alert runModalSheetForWindow:window];
            
            [standardUserDefaults setBool:YES forKey:@"HasSeenFacebookEncodingWarning"];
            [standardUserDefaults synchronize];
        }
    }
}

@end

//
//  UpdateCheckerWorker.m
//  OdessaMacGUIApp
//
//  Created by Noah Spitzer-Williams on 12/19/11.
//  Copyright (c) 2011 Authentically Digital LLC. All rights reserved.
//


#import "UpdateCheckerWorker.h"
#import "UpdateChecker.h"
#import "XMLReader.h"
#import "AppController.h"
#import "BrowserHelper.h"
#import "NSAlert+SynchronousSheet.h"

#import "DDLog.h"
static const int ddLogLevel = LOG_LEVEL_VERBOSE | LOG_LEVEL_INFO | LOG_LEVEL_ERROR | LOG_LEVEL_WARN;

@implementation UpdateCheckerWorker

@synthesize window;


- (void)main
{
    DDLogInfo(@"Checking for update from background thread");
    
    struct UpdateResult updateResult = [self getUpdateResult];
    
    if (updateResult.isUpdateAvailable)
    {
        DDLogInfo(@"Update available!");
        [self performSelectorOnMainThread:@selector(promptUserForUpdate:) withObject:[NSValue valueWithBytes:&updateResult objCType:@encode(struct UpdateResult)] waitUntilDone:YES];
        
    }
    else
    {
        DDLogInfo(@"No update available");
    }
    
}



- (struct UpdateResult) getUpdateResult
{
    NSDictionary* infoDict = [[NSBundle mainBundle] infoDictionary];
    
    NSURL *url = [NSURL URLWithString:[NSString stringWithFormat:@"http://www.highlighthunter.com/updatecheck.php?platform=mac&version=%@", [infoDict objectForKey:@"CFBundleShortVersionString"]]];

#if DEBUG
    url = [NSURL URLWithString:[NSString stringWithFormat:@"http://test.highlighthunter.com/updatecheck.php?platform=mac&version=%@", @"2.0.0"]]; //[infoDict objectForKey:@"CFBundleShortVersionString"]]];
#endif
    //    NSURLRequest *request = [NSURLRequest requestWithURL:url];
    
    NSError *error = nil;
    //NSURLResponse *response = nil;
    
    struct UpdateResult updateResult;
    @try {
        NSData* xmlData = [NSData dataWithContentsOfURL:url];
        //NSData *xmlData = [NSURLConnection sendSynchronousRequest:request returningResponse:&response error:&error];
        
        // Parse the XML Data into an NSDictionary
        NSDictionary* _xmlDictionary = [XMLReader dictionaryForXMLData:xmlData error:&error];
        
        NSString* isUpdateAvailable = (NSString*)[_xmlDictionary retrieveForPath:@"updateResults.isUpdateAvailable"];
        if ([isUpdateAvailable isEqualToString:@"true"]) // we have to do this because obj-c has trouble casting to (bool)
            updateResult.isUpdateAvailable = YES;
        else
            updateResult.isUpdateAvailable = NO;
        updateResult.latestVersion = (NSString*)[_xmlDictionary retrieveForPath:@"updateResults.latestVersion"];
        updateResult.whatsNew = (NSString*)[_xmlDictionary retrieveForPath:@"updateResults.whatsNew"];
        updateResult.downloadPage = (NSString*)[_xmlDictionary retrieveForPath:@"updateResults.downloadPage"];
        
    }
    @catch (NSException *exception) {
        DDLogInfo(@"Exception thrown: %@", exception);
    }
    
    
    
    
    return updateResult;
}



- (void) promptUserForUpdate: (NSValue*)value;
{
    
    if ([[AppController sharedAppController] isSafeToPrompt] == false)
        return;
    
    struct UpdateResult updateResult;
    [value getValue:&updateResult];
    
    NSString* alertMsg = [NSString stringWithFormat:@"Here's what's new in version %@:\n\n%@", updateResult.latestVersion, updateResult.whatsNew];
    NSAlert *alert = [[NSAlert alloc] init];
    [alert addButtonWithTitle:@"Download now"];
    [alert addButtonWithTitle:@"Not now"];
    [alert setMessageText:@"Update available!"];
    [alert setInformativeText:alertMsg];
    [alert setAlertStyle:NSInformationalAlertStyle];
    
    @try
    {
        NSInteger returnCode = [alert runModalSheetForWindow:[self window]];
    
        if (returnCode == NSAlertFirstButtonReturn)
        {
            [BrowserHelper launchBrowser:updateResult.downloadPage term:@"updatecheck" addDomain:NO];
        }
    }
    @catch (NSException* ex) {
        DDLogError(@"%@", ex);
    }
    
    //[alert beginSheetModalForWindow:window modalDelegate:self didEndSelector:@selector(updateAvailableAlertDidEnd:returnCode:contextInfo:) contextInfo:updateResult.downloadPage];
    
}





@end

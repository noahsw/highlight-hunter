//
//  UpdateChecker.m
//  OdessaMacGUIApp
//
//  Created by Noah Spitzer-Williams on 12/19/11.
//  Copyright (c) 2011 Authentically Digital LLC. All rights reserved.
//

#import "UpdateChecker.h"
#import "XMLReader.h"
#import "BrowserHelper.h"
#import "AppController.h"
#import "UpdateCheckerWorker.h"

#import "DDLog.h"


static const int ddLogLevel = LOG_LEVEL_VERBOSE | LOG_LEVEL_INFO | LOG_LEVEL_ERROR | LOG_LEVEL_WARN;

@implementation UpdateChecker

- (void) checkForUpdate: (NSWindow*)window
{
    
    // if this is our first time checking, save today's date and quit
    NSUserDefaults *standardUserDefaults = [NSUserDefaults standardUserDefaults];
    NSDate* lastUpdateCheckDate;
	if (standardUserDefaults) 
    {
        lastUpdateCheckDate = [standardUserDefaults objectForKey:@"LastUpdateCheckDate"];
        if (lastUpdateCheckDate == nil)
        {
            [standardUserDefaults setObject:[NSDate date] forKey:@"LastUpdateCheckDate"];
            [standardUserDefaults synchronize];
            DDLogInfo(@"We don't check for updates on first run");
            return;
        }
    }
    
    
    if ([self isTimeToCheck] == false)
    {
        DDLogInfo(@"Not time to check for updates yet");
        return;
    }
    
    DDLogInfo(@"Checking for update");
    
    // check for update
    NSOperationQueue* queue = [NSOperationQueue new];
    
    UpdateCheckerWorker* ucw = [UpdateCheckerWorker new];
    [ucw setWindow:window];
    
    [queue addOperation:ucw];
    
   
    
}


     
     

- (bool) isTimeToCheck
{
    
#ifdef DEBUG
    return true;
#endif
    
    NSUserDefaults *standardUserDefaults = [NSUserDefaults standardUserDefaults];
    NSDate* lastUpdateCheckDate;
	if (standardUserDefaults) 
    {
        lastUpdateCheckDate = [standardUserDefaults objectForKey:@"LastUpdateCheckDate"];
        if (lastUpdateCheckDate == nil)
            return false;
        
        NSCalendar *gregorian = [[NSCalendar alloc]
                                 initWithCalendarIdentifier:NSGregorianCalendar];
        
        NSUInteger unitFlags = NSDayCalendarUnit;
        
        NSDateComponents *components = [gregorian components:unitFlags
                                                    fromDate:lastUpdateCheckDate
                                                      toDate:[NSDate date] options:0];
        
        NSInteger days = [components day];
        
        
        if (days > 7)
            return true;
        else
            return false;
        
    }
    
    return false;

}




@end

//
//  BrowserHelper.m
//  OdessaMacGUIApp
//
//  Created by Noah Spitzer-Williams on 12/19/11.
//  Copyright (c) 2011 Authentically Digital LLC. All rights reserved.
//

#import "BrowserHelper.h"

#import "DDLog.h"

static const int ddLogLevel = LOG_LEVEL_VERBOSE | LOG_LEVEL_INFO | LOG_LEVEL_ERROR | LOG_LEVEL_WARN;

@implementation BrowserHelper

+ (NSString*) host
{
#if DEBUG
    return @"http://test.highlighthunter.com";
#else
    return @"http://www.highlighthunter.com";
#endif
}

+ (void) launchBrowser: (NSString*)page term:(NSString*)term addDomain:(bool)addDomain
{
    DDLogInfo(@"called");
    
    NSDictionary* infoDict = [[NSBundle mainBundle] infoDictionary];
    NSString* versionNum = infoDict[@"CFBundleShortVersionString"];
    
    NSMutableString *str;
    if (addDomain)
        str = [NSMutableString stringWithFormat:@"%@/%@", [BrowserHelper host], page];
    else
        str = [NSMutableString stringWithString:page];
    
    if ([term length] > 0)
    {
        // see if page already has question mark in it
        NSRange range = [page rangeOfString:@"?"];
        NSString* joiner;
        if (range.location == NSNotFound)
            joiner = @"?";
        else
            joiner = @"&";
            
        [str appendFormat:@"%@utm_source=app%@&utm_medium=app&utm_term=%@&utm_campaign=app", joiner, versionNum, term];
    }
    
    NSURL* url = [NSURL URLWithString:str];
    
    DDLogInfo(@"launching %@", str);
    
    [[NSWorkspace sharedWorkspace] openURL:url];
    
}


+ (void) launchPurchasingOptions
{
    [self launchPurchasingOptions:@""];
}


+ (void) launchPurchasingOptions: (NSString*)coupon
{
    DDLogInfo(@"called");
    
#ifdef APPSTORE
    [self launchBrowser:@"appstore-redirect.php" term:@"buynow" addDomain:YES];
#else
    [self launchOdessaWebsite:[NSString stringWithFormat:@"purchase-redirect.php?coupon=%@", coupon] term:@"buynow" addDomain:YES];
#endif
    
}



@end

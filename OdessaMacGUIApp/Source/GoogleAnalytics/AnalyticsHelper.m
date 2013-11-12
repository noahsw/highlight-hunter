//
//  AnalyticsHelper.m
//  OdessaMacGUIApp
//
//  Created by Noah Spitzer-Williams on 11/27/11.
//  Copyright (c) 2011 Authentically Digital LLC. All rights reserved.
//

#import "AnalyticsHelper.h"
#import "GoogleEvent.h"
#import "TrackingRequest.h"
#import "GoogleTracking.h"
#import "TrackingRequest.h"
#import "RequestFactory.h"
#import "MainModel.h"

#import "DDLog.h"
static const int ddLogLevel = LOG_LEVEL_VERBOSE | LOG_LEVEL_INFO | LOG_LEVEL_ERROR | LOG_LEVEL_WARN;

static NSOperationQueue* operationQueue;


@implementation AnalyticsHelper


+(BOOL) fireEvent: (NSString*)eventAction eventValue:(NSNumber*)eventValue
{
    NSUserDefaults *standardUserDefaults = [NSUserDefaults standardUserDefaults];
    if (standardUserDefaults) 
    {
        if ([standardUserDefaults boolForKey:@"OptInAnalytics"] == NO)
        {
            DDLogInfo(@"User doesn't want analytics tracking");
            return NO;
        }
    }
    
    NSDictionary* infoDict = [[NSBundle mainBundle] infoDictionary];
    NSString* eventCategory = [NSString stringWithFormat:@"Mac %@", infoDict[@"CFBundleShortVersionString"]];
    
    NSString* eventLabel = @"empty";
    if (standardUserDefaults)
    {
        NSString* userUUID = [standardUserDefaults stringForKey:@"UserUUID"];
        if ([userUUID length] == 0)
        { // generate one for the first time
            userUUID = [self UUIDString];
            [standardUserDefaults setObject:userUUID forKey:@"UserUUID"];
            [standardUserDefaults synchronize];
        }
        
        eventLabel = userUUID;
    }
    
    DDLogInfo(@"%@, %@, %@, %@", eventCategory, eventAction, eventLabel, eventValue);
    
    GoogleEvent* googleEvent = [[GoogleEvent alloc] initWithParams:@"highlighthunter.com" category:eventCategory action:eventAction label:eventLabel value:eventValue];
    
    
    if (googleEvent != nil)
    {
        RequestFactory* requestFactory = [RequestFactory new];
        TrackingRequest* request = [requestFactory buildRequest:googleEvent];
        
        if (operationQueue == nil)
        {
            operationQueue = [NSOperationQueue new];
            operationQueue.maxConcurrentOperationCount = 1;
        }
        
        GoogleTracking* trackingOperation = [GoogleTracking new];
        trackingOperation.request = request;
        
        [operationQueue addOperation:trackingOperation];
        
    }
    
    return YES;
    
}



+(NSString*)UUIDString
{
    DDLogInfo(@"called");
    
    CFUUIDRef  uuidObj = CFUUIDCreate(nil);
    NSString  *uuidString = (__bridge_transfer NSString*)CFUUIDCreateString(nil, uuidObj);
    CFRelease(uuidObj);
    return uuidString;
}



@end

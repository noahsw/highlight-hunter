//
//  GoogleTracking.m
//  OdessaMacGUIApp
//
//  Created by Noah Spitzer-Williams on 11/24/11.
//  Copyright (c) 2011 Authentically Digital LLC. All rights reserved.
//

#import "GoogleTracking.h"
#import "TrackingRequest.h"
#import "PublicIP.h"
#import "GoogleEvent.h"

#import "DDLog.h"
#import "DDFileLogger.h"
#import "DDTTYLogger.h"
static const int ddLogLevel = LOG_LEVEL_VERBOSE | LOG_LEVEL_INFO | LOG_LEVEL_ERROR | LOG_LEVEL_WARN;


@implementation GoogleTracking

- (id)init
{
    
    
    self = [super init];
    if (self) {
        
        
    }
    
    return self;
}



/*
 /// <summary>
 /// Fires the tracking event with Google Analytics
 /// </summary>
 /// <param name="request">The request.</param>
 public static void FireTrackingEvent(TrackingRequest request)
 {
 
 //send the request to google
 WebRequest requestForGaGif = WebRequest.Create(request.TrackingGifUri);
 using (WebResponse response = requestForGaGif.GetResponse())
 {
 //ignore response
 }
 }
 */

- (void)main
{
    [NSThread setThreadPriority:0.1];
    [NSThread sleepForTimeInterval:1];
    
    // send request to google
    [self.request setRequestedByIpAddress:[PublicIP getIPAddress]];
    
     
    // Create the request.
    NSString* url = [self.request trackingGifURL];
    
    GoogleEvent* trackingEvent = [self.request trackingEvent];
    
    @try {
        NSData * imageData = [NSData dataWithContentsOfURL:[NSURL URLWithString:url]];
        
        if (imageData && [imageData length] > 0)
            DDLogInfo(@"Analytics event (%@, %@, %@, %@): triggered successfully!", [trackingEvent category], [trackingEvent action], [trackingEvent label], [trackingEvent val]);
        else
            DDLogInfo(@"Analytics event (%@, %@, %@, %@): trigger error", [trackingEvent category], [trackingEvent action], [trackingEvent label], [trackingEvent val]);
    }
    @catch (NSException *exception) {
        DDLogInfo(@"Analytics event (%@, %@, %@, %@): exception thrown %@", [trackingEvent category], [trackingEvent action], [trackingEvent label], [trackingEvent val], exception);
    }
    
}




@end
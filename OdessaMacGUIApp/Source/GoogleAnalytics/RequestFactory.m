//
//  RequestFactory.m
//  OdessaMacGUIApp
//
//  Created by Noah Spitzer-Williams on 11/26/11.
//  Copyright (c) 2011 Authentically Digital LLC. All rights reserved.
//

#import "RequestFactory.h"
#import "TrackingRequest.h"
#import "PublicIP.h"

@implementation RequestFactory




- (TrackingRequest*) buildRequest: (GoogleEvent*) googleEvent
{
    TrackingRequest* tr = [TrackingRequest new];
    //GoogleEvent* eventCopy = [googleEvent copy];
    [tr setTrackingEvent:googleEvent];
    return tr;
}




@end

//
//  RequestFactory.h
//  OdessaMacGUIApp
//
//  Created by Noah Spitzer-Williams on 11/26/11.
//  Copyright (c) 2011 Authentically Digital LLC. All rights reserved.
//

#import "TrackingRequest.h"
#import "GoogleEvent.h"


@interface RequestFactory : NSObject
{
    
    
}


- (TrackingRequest*) buildRequest: (GoogleEvent*) googleEvent;

@end

//
//  AnalyticsHelper.h
//  OdessaMacGUIApp
//
//  Created by Noah Spitzer-Williams on 11/27/11.
//  Copyright (c) 2011 Authentically Digital LLC. All rights reserved.
//



@interface AnalyticsHelper : NSObject

+(BOOL) fireEvent: (NSString*)eventAction eventValue:(NSNumber*)eventValue;

@end

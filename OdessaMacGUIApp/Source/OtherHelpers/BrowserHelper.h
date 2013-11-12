//
//  BrowserHelper.h
//  OdessaMacGUIApp
//
//  Created by Noah Spitzer-Williams on 12/19/11.
//  Copyright (c) 2011 Authentically Digital LLC. All rights reserved.
//



@interface BrowserHelper : NSObject

+ (NSString*) host;

+ (void) launchBrowser: (NSString*)page term:(NSString*)term addDomain:(bool)addDomain;

+ (void) launchPurchasingOptions;
+ (void) launchPurchasingOptions: (NSString*)coupon;

@end

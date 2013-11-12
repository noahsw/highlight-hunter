//
//  AppStoreRating.m
//  OdessaMacGUIApp
//
//  Created by Noah Spitzer-Williams on 10/11/12.
//  Copyright (c) 2012 Authentically Digital LLC. All rights reserved.
//

#import "AppStoreRating.h"

@implementation AppStoreRating

+ (void)handleAppStoreRatingPrompt:(NSWindow*)window
{
    // see if they upgraded and we should reset their answer from before
    NSUserDefaults* standardUserDefaults = [NSUserDefaults standardUserDefaults];
    if (standardUserDefaults)
    {
        NSDictionary* infoDict = [[NSBundle mainBundle] infoDictionary];
        NSString* version = [infoDict objectForKey:@"CFBundleShortVersionString"];
        
        if ([[standardUserDefaults stringForKey:@"AppStoreReviewVersionNumber"] isEqualToString:version] == NO) // they upgraded to new version
        {
            [standardUserDefaults setBool:NO forKey:@"HasAddressedAppStoreReviewPrompt"];
            [standardUserDefaults setObject:version forKey:@"AppStoreReviewVersionNumber"];
            [standardUserDefaults synchronize];
        }
    }
    
    
    if ([AppStoreRating isTimeToPrompt])
    {
        [NSTimer scheduledTimerWithTimeInterval:3 target:self selector:@selector(promptForRating:) userInfo:window repeats:NO];
        
    }
}

+ (BOOL)isTimeToPrompt
{
    
    NSUserDefaults* standardUserDefaults = [NSUserDefaults standardUserDefaults];
    if (standardUserDefaults)
    {
        NSString* todayDate = [NSDateFormatter localizedStringFromDate:[NSDate date] dateStyle:NSDateFormatterShortStyle timeStyle:NSDateFormatterNoStyle];
        NSInteger totalLoads = [standardUserDefaults integerForKey:@"TotalLoads"];
        NSString* lastPromptDate = [standardUserDefaults stringForKey:@"LastAppStoreReviewPromptDate"];
        BOOL hasAddressedAppStoreReviewPrompt = [standardUserDefaults boolForKey:@"HasAddressedAppStoreReviewPrompt"];
        if (totalLoads >= 3 && !hasAddressedAppStoreReviewPrompt && ![todayDate isEqualToString:lastPromptDate])
        { // only prompt if they've loaded it 3 times and we havent prompted them yet today
            return YES;
        }
    }

    return NO;
}

+ (void)promptForRating:(NSTimer*)timer
{
#ifndef APPSTORE // only prompt if app store build
    return;
#endif
    
    NSWindow* window = [timer userInfo];
    
    
    NSString* msg = @"Do you mind quickly rating Highlight Hunter in the App Store?\n\nRatings are reset with every new version.";
    NSAlert *alert = [[NSAlert alloc] init];
    [alert addButtonWithTitle:@"Rate now"];
    [alert addButtonWithTitle:@"Not now"];
    [alert addButtonWithTitle:@"No thanks"];
    [alert setMessageText:@"Rate us in the App Store?"];
    [alert setInformativeText:msg];
    [alert setAlertStyle:NSInformationalAlertStyle];
    NSInteger returnCode = [alert runModalSheetForWindow:window];
    
    NSUserDefaults* standardUserDefaults = [NSUserDefaults standardUserDefaults];
    

    if (returnCode == NSAlertFirstButtonReturn)
    { // rate now
        #if APPSTORE_FREE
            NSURL* url = [NSURL URLWithString:@"macappstore://itunes.apple.com/app/id521035831?mt=12"];
        #else
            NSURL* url = [NSURL URLWithString:@"macappstore://itunes.apple.com/app/id521180800?mt=12"];
        #endif
        
        [[NSWorkspace sharedWorkspace] openURL:url];

        if (standardUserDefaults)
            [standardUserDefaults setBool:YES forKey:@"HasAddressedAppStoreReviewPrompt"];

    }
    else if (returnCode == NSAlertSecondButtonReturn)
    { // not now
        NSString* todayDate = [NSDateFormatter localizedStringFromDate:[NSDate date] dateStyle:NSDateFormatterShortStyle timeStyle:NSDateFormatterNoStyle];
        [standardUserDefaults setValue:todayDate forKey:@"LastAppStoreReviewPromptDate"];
    }
    else if (returnCode == NSAlertThirdButtonReturn)
    { // no thanks
        if (standardUserDefaults)
            [standardUserDefaults setBool:YES forKey:@"HasAddressedAppStoreReviewPrompt"];
    }

    [standardUserDefaults synchronize];
}

@end

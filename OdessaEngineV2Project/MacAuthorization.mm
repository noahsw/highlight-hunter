//
//  MacAuthorization.m
//  OdessaMacGUIApp
//
//  Created by Noah Spitzer-Williams on 10/14/11.
//  Copyright 2011 Authentically Digital LLC. All rights reserved.
//

#import "MacAuthorization.h"


@implementation MacAuthorization

- (id)init
{
    self = [super init];
    if (self) {
        // Initialization code here.
    }

    return self;
}

- (void)dealloc
{
    [super dealloc];
}


+ (NSString*) parentProcessName
{
    // Get our PSN
    OSStatus    err;
    ProcessSerialNumber    currPSN;
    err = GetCurrentProcess (&currPSN);
    if (!err) {
        // Get information about our process
        NSDictionary*    currDict = (NSDictionary*)ProcessInformationCopyDictionary (&currPSN, kProcessDictionaryIncludeAllInformationMask);

        // Get the PSN of the app that *launched* us.  Its not really the parent app, in the unix sense.
        long long    temp = [[currDict objectForKey:@"ParentPSN"] longLongValue];
        [currDict release];
        long long    hi = (temp >> 32) & 0x00000000FFFFFFFFLL;
        long long    lo = (temp >> 0) & 0x00000000FFFFFFFFLL;
        ProcessSerialNumber    parentPSN = {(unsigned long)hi, (unsigned long)lo};

        // Get info on the launching process
        NSDictionary*    parentDict = (NSDictionary*)ProcessInformationCopyDictionary (&parentPSN, kProcessDictionaryIncludeAllInformationMask);

        // Test the creator code of the launching app
        NSString *parentName = [[parentDict objectForKey: kCFBundleNameKey] retain];
        [parentDict release];

        return [parentName autorelease] ;
    }

    return nil;
}

@end

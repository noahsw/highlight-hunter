//
//  NSURL+NSURL_Helpers.m
//  OdessaMacGUIApp
//
//  Created by Noah Spitzer-Williams on 8/29/12.
//  Copyright (c) 2012 Authentically Digital LLC. All rights reserved.
//

#import "NSURL+Helpers.h"

@implementation NSURL (Helpers)

- (unsigned long long) fileSizeInBytes
{
    
    NSFileManager* fileManager = [NSFileManager defaultManager];
    NSDictionary *attrs = [fileManager attributesOfItemAtPath:[self path] error: NULL];
    unsigned long long result = [attrs fileSize];
    return result;
}

- (unsigned long long) fileSizeInKBytes
{
    return [self fileSizeInBytes] / 1024;
}

@end

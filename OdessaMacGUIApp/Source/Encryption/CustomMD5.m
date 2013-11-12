//
//  CustomMD5.m
//  OdessaMacGUIApp
//
//  Created by Noah Spitzer-Williams on 8/28/12.
//  Copyright (c) 2012 Authentically Digital LLC. All rights reserved.
//

#import "CustomMD5.h"

@implementation CustomMD5



+ (NSString *)md5StringFromData:(NSData *)data
{
    void *cData = malloc([data length]);
    unsigned char resultCString[16];
    [data getBytes:cData length:[data length]];
    
    CC_MD5(cData, (CC_LONG)[data length], resultCString);
    free(cData);
    
    NSString *result = [NSString stringWithFormat:
                        @"%02X%02X%02X%02X%02X%02X%02X%02X%02X%02X%02X%02X%02X%02X%02X%02X",
                        resultCString[0], resultCString[1], resultCString[2], resultCString[3],
                        resultCString[4], resultCString[5], resultCString[6], resultCString[7],
                        resultCString[8], resultCString[9], resultCString[10], resultCString[11],
                        resultCString[12], resultCString[13], resultCString[14], resultCString[15]
                        ];
    return result;
}


@end

//
//  MD5.h
//  OdessaMacGUIApp
//
//  Created by Noah Spitzer-Williams on 8/28/12.
//  Copyright (c) 2012 Authentically Digital LLC. All rights reserved.
//

#import <Foundation/Foundation.h>

#import <CommonCrypto/CommonDigest.h>

@interface CustomMD5 : NSObject

+ (NSString *)md5StringFromData:(NSData *)data;

@end

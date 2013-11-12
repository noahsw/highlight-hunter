//
//  NSURL+NSURL_Helpers.h
//  OdessaMacGUIApp
//
//  Created by Noah Spitzer-Williams on 8/29/12.
//  Copyright (c) 2012 Authentically Digital LLC. All rights reserved.
//

#import <Foundation/Foundation.h>

@interface NSURL (Helpers)

- (unsigned long long) fileSizeInBytes;
- (unsigned long long) fileSizeInKBytes;
@end

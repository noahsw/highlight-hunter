//
//  AppStoreRating.h
//  OdessaMacGUIApp
//
//  Created by Noah Spitzer-Williams on 10/11/12.
//  Copyright (c) 2012 Authentically Digital LLC. All rights reserved.
//

#import <Foundation/Foundation.h>
#import "NSAlert+SynchronousSheet.h"

@interface AppStoreRating : NSObject

+ (void)handleAppStoreRatingPrompt:(NSWindow*)window;

@end

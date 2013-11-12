//
//  ThumbnailButton.h
//  OdessaMacGUIApp
//
//  Created by Noah Spitzer-Williams on 9/12/12.
//  Copyright (c) 2012 Authentically Digital LLC. All rights reserved.
//

#import <Cocoa/Cocoa.h>
#import "ThumbnailDelegate.h"

@interface ThumbnailButton : NSButton

@property (unsafe_unretained) id<ThumbnailDelegate> thumbnailDelegate;

@end

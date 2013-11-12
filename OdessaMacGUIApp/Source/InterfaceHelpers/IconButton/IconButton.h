//
//  IconButton.h
//  OdessaMacGUIApp
//
//  Created by Noah Spitzer-Williams on 9/12/12.
//  Copyright (c) 2012 Authentically Digital LLC. All rights reserved.
//

#import <Cocoa/Cocoa.h>
#import "NSButton+TextColor.h"
#import "DesignLanguage.h"
#import "IconButtonDelegate.h"

@interface IconButton : NSButton <IconButtonDelegate>

@property BOOL isActionable;

@property (unsafe_unretained) id<IconButtonDelegate> iconButtonDelegate;

@end

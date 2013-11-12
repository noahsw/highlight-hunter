//
//  PublishButtonView.h
//  OdessaMacGUIApp
//
//  Created by Noah Spitzer-Williams on 9/27/12.
//  Copyright (c) 2012 Authentically Digital LLC. All rights reserved.
//

#import <Cocoa/Cocoa.h>
#import "PublishButtonDelegate.h"

@interface PublishButtonView : NSView

@property (unsafe_unretained) id<PublishButtonDelegate> publishButtonDelegate;

@end

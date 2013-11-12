//
//  EndTickView.h
//  OdessaMacGUIApp
//
//  Created by Noah Spitzer-Williams on 9/4/12.
//  Copyright (c) 2012 Authentically Digital LLC. All rights reserved.
//

#import <Cocoa/Cocoa.h>
#import "EndTickViewDelegate.h"
#import "NSImageView+Coordinates.h"
#import "DDLog.h"

@interface EndTickView : NSImageView

@property NSInteger leftCoordinateBounds;
@property NSInteger rightCoordinateBounds;

@property (unsafe_unretained) id<EndTickViewDelegate> delegate;

@property (strong) NSTimer* zoomOutTimer;

@end




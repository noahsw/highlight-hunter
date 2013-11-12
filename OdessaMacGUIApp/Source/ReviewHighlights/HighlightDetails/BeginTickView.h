//
//  StartTickView.h
//  OdessaMacPrototype
//
//  Created by Noah Spitzer-Williams on 8/24/12.
//
//

#import <Cocoa/Cocoa.h>
#import "BeginTickViewDelegate.h"
#import "NSImageView+Coordinates.h"
#import "DDLog.h"

@interface BeginTickView : NSImageView

@property NSInteger leftCoordinateBounds;
@property NSInteger rightCoordinateBounds;

@property BOOL isDragging;

//@property NSInteger leftZoomOutCoordinateBounds;

@property (strong) NSTimer* zoomOutTimer;

@property (unsafe_unretained) id<BeginTickViewDelegate> delegate;

@end




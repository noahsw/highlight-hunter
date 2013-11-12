//
//  PlayheadView.h
//  OdessaMacPrototype
//
//  Created by Noah Spitzer-Williams on 8/23/12.
//
//

#import <Cocoa/Cocoa.h>
#import "PlayheadViewDelegate.h"
#import "NSImageView+Coordinates.h"

#import "DDLog.h"


@interface PlayheadView : NSImageView

@property NSInteger leftCoordinateBounds;
@property NSInteger rightCoordinateBounds;

@property (unsafe_unretained) id<PlayheadViewDelegate> delegate;

@end


@interface PlayheadView ()

    @property NSPoint startDragLocation;
    @property NSPoint startFrameOrigin;

@end
//
//  TimelineView.h
//  OdessaMacPrototype
//
//  Created by Noah Spitzer-Williams on 8/27/12.
//
//

#import <Cocoa/Cocoa.h>
#import "TimelineViewDelegate.h"

@interface TimelineView : NSImageView

@property (unsafe_unretained) id<TimelineViewDelegate> delegate;

@end

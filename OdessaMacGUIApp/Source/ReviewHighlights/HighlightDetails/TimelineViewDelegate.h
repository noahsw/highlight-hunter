//
//  TimelineViewDelegate.h
//  OdessaMacPrototype
//
//  Created by Noah Spitzer-Williams on 8/27/12.
//
//

#import <Foundation/Foundation.h>

@protocol TimelineViewDelegate <NSObject>

- (void)timelineClickedAtCoordinate:(NSPoint)location;


@end

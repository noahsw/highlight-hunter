//
//  TickViewDelegate.h
//  OdessaMacPrototype
//
//  Created by Noah Spitzer-Williams on 8/27/12.
//
//

#import <Foundation/Foundation.h>

@protocol BeginTickViewDelegate <NSObject>

- (void)beginTickDragStarted;
- (void)beginTickDragged;
- (void)beginTickDragEnded;
- (void)seekToHighlightBeginning;

- (void)beginZoomOut;

- (void)setHighlightBeginTimeForCoordinate:(NSPoint)coordinate;


@end


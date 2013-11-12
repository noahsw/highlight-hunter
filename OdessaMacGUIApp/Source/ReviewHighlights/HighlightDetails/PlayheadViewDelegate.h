//
//  PlayheadViewDelegate.h
//  OdessaMacPrototype
//
//  Created by Noah Spitzer-Williams on 8/27/12.
//
//

#import <Foundation/Foundation.h>

@protocol PlayheadViewDelegate <NSObject>

- (void)playheadDragStarted;
- (void)playheadDragged;
- (void)playheadDragEnded;


@end

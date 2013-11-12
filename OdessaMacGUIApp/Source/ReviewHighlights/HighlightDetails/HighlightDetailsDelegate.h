//
//  HighlightDetailsDelegate.h
//  OdessaMacGUIApp
//
//  Created by Noah Spitzer-Williams on 9/12/12.
//  Copyright (c) 2012 Authentically Digital LLC. All rights reserved.
//

#import <Foundation/Foundation.h>
#import "HighlightObject.h"

@protocol HighlightDetailsDelegate <NSObject>

- (void)removeHighlight:(HighlightObject*)highlightObject;

- (void)advanceTutorialPastHighlightsFound;

- (void)initializeStartOverTutorialBubble;

@end

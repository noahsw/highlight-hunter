//
//  HighlightDelegate.h
//  OdessaMacGUIApp
//
//  Created by Noah Spitzer-Williams on 9/10/12.
//  Copyright (c) 2012 Authentically Digital LLC. All rights reserved.
//

#import <Foundation/Foundation.h>
@class HighlightObject;

@protocol HighlightDelegate <NSObject>

- (void)openHighlightDetails:(HighlightObject*)highlightObject;

@end

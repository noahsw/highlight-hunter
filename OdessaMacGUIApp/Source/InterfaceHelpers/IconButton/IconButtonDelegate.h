//
//  IconButtonDelegate.h
//  OdessaMacGUIApp
//
//  Created by Noah Spitzer-Williams on 9/20/12.
//  Copyright (c) 2012 Authentically Digital LLC. All rights reserved.
//

#import <Foundation/Foundation.h>

@protocol IconButtonDelegate <NSObject>

- (void)showHoverState;
- (void)showRestingState;

@end

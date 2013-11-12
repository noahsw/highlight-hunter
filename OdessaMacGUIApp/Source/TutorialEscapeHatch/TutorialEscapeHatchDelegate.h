//
//  TutorialEscapeHatchDelegate.h
//  OdessaMacGUIApp
//
//  Created by Noah Spitzer-Williams on 1/5/13.
//  Copyright (c) 2013 Authentically Digital LLC. All rights reserved.
//

#import <Foundation/Foundation.h>

@protocol TutorialEscapeHatchDelegate <NSObject>

- (void)refreshTutorialProgress;

@optional
- (void)exitTutorial;

@end

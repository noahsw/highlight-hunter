//
//  BigDropHereDelegate.h
//  OdessaMacGUIApp
//
//  Created by Noah Spitzer-Williams on 9/6/12.
//  Copyright (c) 2012 Authentically Digital LLC. All rights reserved.
//

#import <Foundation/Foundation.h>

@protocol BigDropHereDelegate <NSObject>

- (void)bigDropHereDragEntered;
- (void)bigDropHereDragExited;

@end

//
//  SmallDropHereDelegate.h
//  OdessaMacGUIApp
//
//  Created by Noah Spitzer-Williams on 9/17/12.
//  Copyright (c) 2012 Authentically Digital LLC. All rights reserved.
//

#import <Foundation/Foundation.h>

@protocol SmallDropHereDelegate <NSObject>

- (void)smallDropHereDragEntered;
- (void)smallDropHereDragExited;

@end

//
//  PublishStatusDelegate.h
//  OdessaMacGUIApp
//
//  Created by Noah Spitzer-Williams on 9/11/12.
//  Copyright (c) 2012 Authentically Digital LLC. All rights reserved.
//

#import <Foundation/Foundation.h>
#import "PublishStatusItem.h"

@protocol PublishStatusDelegate <NSObject>

- (void)publishStatusItemAdded:(PublishStatusItem*)publishStatusItem;

@end

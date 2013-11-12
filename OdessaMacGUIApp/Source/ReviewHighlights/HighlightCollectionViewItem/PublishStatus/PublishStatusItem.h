//
//  PublishStatusItem.h
//  OdessaMacGUIApp
//
//  Created by Noah Spitzer-Williams on 9/11/12.
//  Copyright (c) 2012 Authentically Digital LLC. All rights reserved.
//

#import <Foundation/Foundation.h>
#import "PublishWorker.h"
@class HighlightObject;

@interface PublishStatusItem : NSObject

@property (unsafe_unretained) HighlightObject* highlightObject;

@property NSInteger publishWorkerType;

@property (unsafe_unretained) PublishWorker* publishWorker;


@end

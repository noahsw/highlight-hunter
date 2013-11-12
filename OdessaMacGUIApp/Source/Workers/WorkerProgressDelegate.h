//
//  WorkerProgressDelegate.h
//  OdessaMacGUIApp
//
//  Created by Noah Spitzer-Williams on 9/5/12.
//  Copyright (c) 2012 Authentically Digital LLC. All rights reserved.
//

#import <Foundation/Foundation.h>

@protocol WorkerProgressDelegate <NSObject>

- (void)workerProgressUpdated:(NSInteger)newProgress;

@end

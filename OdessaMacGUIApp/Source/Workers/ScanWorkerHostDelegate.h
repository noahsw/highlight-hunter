//
//  ScanWorkerHostDelegate.h
//  OdessaMacGUIApp
//
//  Created by Noah Spitzer-Williams on 8/29/12.
//  Copyright (c) 2012 Authentically Digital LLC. All rights reserved.
//

#import <Foundation/Foundation.h>

@protocol ScanWorkerHostDelegate <NSObject>

- (void)scanWorkerHostProgressUpdated:(NSInteger)progress;

- (void)scanWorkerHostStatusUpdated:(NSString*)status;

- (void)scanWorkerHostFinished;

- (void)scanWorkerHostCancelled;

@end

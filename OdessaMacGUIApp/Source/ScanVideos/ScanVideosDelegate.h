//
//  ScanVideosDelegate.h
//  OdessaMacGUIApp
//
//  Created by Noah Spitzer-Williams on 8/29/12.
//  Copyright (c) 2012 Authentically Digital LLC. All rights reserved.
//

#import <Foundation/Foundation.h>

@protocol ScanVideosDelegate <NSObject>

- (void)scanFinished;
- (void)scanCancelled;

@end

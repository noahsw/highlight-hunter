//
//  ThumbnailDelegate.h
//  OdessaMacGUIApp
//
//  Created by Noah Spitzer-Williams on 9/12/12.
//  Copyright (c) 2012 Authentically Digital LLC. All rights reserved.
//

#import <Foundation/Foundation.h>

@protocol ThumbnailDelegate <NSObject>

- (void)thumbnailEntered;
- (void)thumbnailExited;

@end

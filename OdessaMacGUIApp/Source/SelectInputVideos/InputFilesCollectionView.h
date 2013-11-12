//
//  InputFilesCollectionView.h
//  OdessaMacGUIApp
//
//  Created by Noah Spitzer-Williams on 9/17/12.
//  Copyright (c) 2012 Authentically Digital LLC. All rights reserved.
//

#import <Cocoa/Cocoa.h>
#import "GenericDropHereDelegate.h"
#import "InputVideoThumbnailViewController.h"
#import "InputVideoThumbnailView.h"
#import "RemoveInputFileDelegate.h"

@interface InputFilesCollectionView : NSCollectionView

@property (unsafe_unretained) id<GenericDropHereDelegate> genericDropHereDelegate;

@property (unsafe_unretained) id<RemoveInputFileDelegate> removeInputFileDelegate;

@end

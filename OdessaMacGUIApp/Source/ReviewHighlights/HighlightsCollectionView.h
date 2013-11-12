//
//  HighlightsCollectionView.h
//  OdessaMacGUIApp
//
//  Created by Noah Spitzer-Williams on 9/10/12.
//  Copyright (c) 2012 Authentically Digital LLC. All rights reserved.
//

#import "AMCollectionView.h"
#import "DividerCollectionViewItem.h"
#import "HighlightCollectionViewItem.h"
#import "NoHighlightsCollectionViewItem.h"
#import "HighlightObject.h"
#import "InputFileObject.h"
#import "NoHighlightsObject.h"
#import "NoHighlightsDelegate.h"
#import "MainModel.h"

#import "HighlightDelegate.h"
#import "HighlightDetails/HighlightDetailsWindowController.h"
#import "HighlightDetails/HighlightDetailsDelegate.h"

#import "TutorialHelper.h"
#import "TutorialEscapeHatchDelegate.h"

@interface HighlightsCollectionView : AMCollectionView <HighlightDelegate, NoHighlightsDelegate>

@property (unsafe_unretained) MainModel* mainModel;

@property (strong) HighlightDetailsWindowController* highlightDetailsWindowController;

@property (unsafe_unretained) id<HighlightDetailsDelegate> highlightDetailsDelegate;

@property (unsafe_unretained) id<NoHighlightsDelegate> noHighlightsDelegate;

@property (unsafe_unretained) id<TutorialEscapeHatchDelegate> tutorialEscapeHatchDelegate;

@end

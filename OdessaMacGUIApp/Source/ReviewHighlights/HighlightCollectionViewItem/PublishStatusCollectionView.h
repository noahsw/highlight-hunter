//
//  PublishStatusCollectionView.h
//  OdessaMacGUIApp
//
//  Created by Noah Spitzer-Williams on 9/11/12.
//  Copyright (c) 2012 Authentically Digital LLC. All rights reserved.
//

#import "AMCollectionView.h"
#import "PublishStatusViewItem.h"
#import "HighlightDelegate.h"

@interface PublishStatusCollectionView : AMCollectionView

@property (unsafe_unretained) id<HighlightDelegate> highlightDelegate;

@end

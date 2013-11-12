//
//  HighlightCollectionViewItem.h
//  OdessaMacGUIApp
//
//  Created by Noah Spitzer-Williams on 9/10/12.
//  Copyright (c) 2012 Authentically Digital LLC. All rights reserved.
//

#import "AMCollectionViewItem.h"
#import "HighlightObject.h"
#import "HighlightDelegate.h"
#import "PublishStatusItem.h"
#import "PublishStatusCollectionView.h"
#import "PublishStatusDelegate.h"
#import "ThumbnailDelegate.h"
#import "ThumbnailButton.h"

@interface HighlightCollectionViewItem : AMCollectionViewItem <PublishStatusDelegate, HighlightDelegate, ThumbnailDelegate> {
}


@property (unsafe_unretained) HighlightObject* highlightObject;

@property (unsafe_unretained) id<HighlightDelegate> highlightDelegate;

@property (strong) IBOutlet PublishStatusCollectionView *publishStatusCollectionView;

@property (strong) NSMutableArray* publishStatusItems;

@property (strong) IBOutlet ThumbnailButton *thumbnailButton;
@property (strong) IBOutlet NSImageView *thumbnailMask;
@property (strong) IBOutlet NSImageView *thumbnailPlayOverlay;


- (IBAction)openHighlightDetails:(id)sender;

@end

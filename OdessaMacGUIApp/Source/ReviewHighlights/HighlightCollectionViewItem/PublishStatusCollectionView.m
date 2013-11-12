//
//  PublishStatusCollectionView.m
//  OdessaMacGUIApp
//
//  Created by Noah Spitzer-Williams on 9/11/12.
//  Copyright (c) 2012 Authentically Digital LLC. All rights reserved.
//

#import "PublishStatusCollectionView.h"

@implementation PublishStatusCollectionView

- (AMCollectionViewItem *)newItemForRepresentedObject:(id)object
{
    
    PublishStatusViewItem* publishStatusViewItem = [[PublishStatusViewItem  alloc] initWithCollectionView:self representedObject:object];
    
    [publishStatusViewItem setHighlightDelegate:self.highlightDelegate];
    [publishStatusViewItem setWindow:self.window];
    
    return publishStatusViewItem;

    
}

@end

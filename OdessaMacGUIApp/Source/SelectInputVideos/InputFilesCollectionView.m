//
//  InputFilesCollectionView.m
//  OdessaMacGUIApp
//
//  Created by Noah Spitzer-Williams on 9/17/12.
//  Copyright (c) 2012 Authentically Digital LLC. All rights reserved.
//

#import "InputFilesCollectionView.h"

@implementation InputFilesCollectionView

- (id)initWithFrame:(NSRect)frame
{
    self = [super initWithFrame:frame];
    if (self) {
        // Initialization code here.
    }
    
    return self;
}

- (NSCollectionViewItem *)newItemForRepresentedObject:(id)object
{
    // Get a copy of the item prototype, set represented object
    InputVideoThumbnailViewController *newItem = (InputVideoThumbnailViewController*)[[self itemPrototype] copy];
    [newItem setRepresentedObject:object];
    [newItem setGenericDropHereDelegate:self.genericDropHereDelegate];
    [newItem setRemoveInputFileDelegate:self.removeInputFileDelegate];
    
    // we're creating a new item right now so this is where we need to instantiate the tracking areas
    [newItem createTrackingArea];
    
    // this isn't working
    InputVideoThumbnailView *itemView = (InputVideoThumbnailView*)[newItem view];
    [itemView registerForDraggedTypes:@[NSFilenamesPboardType]];
    
    /* this never seemed to work for me. we had to put the delegate in the controller
    // Get the new item's view so you can mess with it
    InputVideoThumbnailView *itemView = (InputVideoThumbnailView*)[newItem view];
     
     [itemView setGenericDropHereDelegate:self.genericDropHereDelegate];
     
    */
    
    
    
    return newItem;
}
@end

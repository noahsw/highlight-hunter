//
//  HighlightCollectionViewItem.m
//  OdessaMacGUIApp
//
//  Created by Noah Spitzer-Williams on 9/10/12.
//  Copyright (c) 2012 Authentically Digital LLC. All rights reserved.
//

#import "HighlightCollectionViewItem.h"

@implementation HighlightCollectionViewItem

- (id)initWithCollectionView:(AMCollectionView *)theCollectionView representedObject:(id)theObject
{
	self = [super initWithCollectionView:theCollectionView representedObject:theObject];
	if (self != nil) {
		if ([NSBundle loadNibNamed:@"HighlightCollectionViewItem" owner:self]) {
            
            HighlightObject* highlightObject = (HighlightObject*)theObject;
            
            [highlightObject setPublishStatusDelegate:self];
            
            [self.publishStatusCollectionView setHighlightDelegate:self];
            
            
            self.publishStatusItems = [[NSMutableArray alloc] init];
            
            PublishStatusItem* publishStatusItem = [PublishStatusItem new];
            publishStatusItem.highlightObject = highlightObject;
            publishStatusItem.publishWorkerType = PublishWorkerTypeNone;
            
            [self.publishStatusItems addObject:publishStatusItem];
            
            
            [self.publishStatusCollectionView setContent:self.publishStatusItems];
            [self.publishStatusCollectionView setDrawsBackground:NO];
            
            [self.thumbnailButton setThumbnailDelegate:self];
            
		} else {
			self = nil;
		}
	}
	return self;
}

- (IBAction)openHighlightDetails:(id)sender {
    [self.highlightDelegate openHighlightDetails:self.highlightObject];
}

- (void)publishStatusItemAdded:(PublishStatusItem *)publishStatusItem
{
    [self.publishStatusItems addObject:publishStatusItem];
    
    [self.publishStatusCollectionView setContent:self.publishStatusItems];
}

- (void)thumbnailEntered
{
    [self.thumbnailPlayOverlay setHidden:NO];
    [self.thumbnailMask setImage:[NSImage imageNamed:@"review-video-thumb-rightside-point-mask-hover.png"]];
}

- (void)thumbnailExited
{
    [self.thumbnailPlayOverlay setHidden:YES];
    [self.thumbnailMask setImage:[NSImage imageNamed:@"review-video-thumb-rightside-point-mask.png"]];
}

@end

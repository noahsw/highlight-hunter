//
//  HighlightHeaderCollectionViewItem.m
//  OdessaMacGUIApp
//
//  Created by Noah Spitzer-Williams on 9/10/12.
//  Copyright (c) 2012 Authentically Digital LLC. All rights reserved.
//

#import "DividerCollectionViewItem.h"

@implementation DividerCollectionViewItem


- (id)initWithCollectionView:(AMCollectionView *)theCollectionView representedObject:(id)theObject
{
	self = [super initWithCollectionView:theCollectionView representedObject:theObject];
	if (self != nil) {
		if ([NSBundle loadNibNamed:@"DividerCollectionViewItem" owner:self]) {
            [self updateLabel];
            [self applySkin];
		} else {
			self = nil;
		}
	}
	return self;
}



- (void)applySkin
{
    /*
    NSImage* backgroundImage = [NSImage imageNamed:@"review-divider-bar-5px-horizontal-tile.png"];
    NSColor* backgroundColor = [NSColor colorWithPatternImage:backgroundImage];
    
    //[self.view setBackgroundColor:backgroundColor];
    
    [backgroundColor set];
    NSRectFill([self.view bounds]);
     
    [self.view setNeedsDisplay:YES];
     */
 /*
    NSImage* backgroundImage = [NSImage imageNamed:@"review-divider-bar-5px-horizontal-tile.png"];
    if (backgroundImage != nil)
    {
        backgroundImage
        CALayer *viewLayer = [CALayer layer];
        [viewLayer setBackgroundColor:];
        [view setWantsLayer:YES]; // view's backing store is using a Core Animation Layer
        [view setLayer:viewLayer];
        
        //[self.view setBackgroundColor:[NSColor colorWithPatternImage:backgroundImage]];
    }
  */
    
}

- (void)updateLabel
{
    NSInteger highlightCount = 0;
    
    for (HighlightObject* highlightObject in self.mainModel.highlightObjects)
    {
        if (highlightObject.inputFileObject == self.inputFileObject)
            highlightCount++;
    }
    
    NSString* ess = (highlightCount == 1 ? @"" : @"s");
    
    NSString* string = [NSString stringWithFormat:@"%@ | %ld highlight%@", self.inputFileObject.sourceURL.lastPathComponent.stringByDeletingPathExtension, highlightCount, ess];
    
    [self.headerLabel setStringValue:string];
}

@end

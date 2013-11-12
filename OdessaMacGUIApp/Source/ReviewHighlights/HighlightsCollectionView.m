//
//  HighlightsCollectionView.m
//  OdessaMacGUIApp
//
//  Created by Noah Spitzer-Williams on 9/10/12.
//  Copyright (c) 2012 Authentically Digital LLC. All rights reserved.
//

#import "HighlightsCollectionView.h"

@implementation HighlightsCollectionView


- (AMCollectionViewItem *)newItemForRepresentedObject:(id)object
{
    
    if ([object isKindOfClass:[InputFileObject class]])
    {
        DividerCollectionViewItem* header = [[DividerCollectionViewItem  alloc] initWithCollectionView:self representedObject:object];
        
        [header setInputFileObject:(InputFileObject*)object];
        [header setMainModel:self.mainModel];
        [header updateLabel];
        return header;
    
        
    }
    else if ([object isKindOfClass:[HighlightObject class]])
    {
        HighlightCollectionViewItem* highlight = [[HighlightCollectionViewItem alloc] initWithCollectionView:self representedObject:object];
        
        [highlight setHighlightObject:(HighlightObject*)object];
        [highlight setHighlightDelegate:self];
        
        return highlight;
    }
    else if ([object isKindOfClass:[NoHighlightsObject class]])
    {
        NoHighlightsCollectionViewItem* noHighlights = [[NoHighlightsCollectionViewItem alloc] initWithCollectionView:self representedObject:object];
        noHighlights.noHighlightsObject = (NoHighlightsObject*)object;
        noHighlights.noHighlightsDelegate = self;
        return noHighlights;
    }
    else
    {
        NSAssert(false, @"We should never get here!");
        return nil;
    }
        
}


- (void)openHighlightDetails:(HighlightObject *)highlightObject
{
    
    if ([TutorialHelper getProgress] == TutorialHighlightsFound)
    {
        [self.highlightDetailsDelegate advanceTutorialPastHighlightsFound];
    }
    
    
    if (self.highlightDetailsWindowController == nil)
    {
        self.highlightDetailsWindowController = [[HighlightDetailsWindowController alloc] initWithWindowNibName:@"HighlightDetailsWindow"];
    }
    self.highlightDetailsWindowController.mainModel = self.mainModel;
    self.highlightDetailsWindowController.highlightDetailsDelegate = self.highlightDetailsDelegate;
    self.highlightDetailsWindowController.tutorialEscapeHatchDelegate = self.tutorialEscapeHatchDelegate;
    
    [self.highlightDetailsWindowController showWindow:self];
    
    [self.highlightDetailsWindowController setCurrentHighlight:highlightObject];
    
    
}


- (void)rescanRequested
{
    [self.noHighlightsDelegate rescanRequested];
}


@end
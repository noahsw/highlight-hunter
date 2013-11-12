//
//  BreadcrumbsView.m
//  OdessaMacPrototype
//
//  Created by Noah Spitzer-Williams on 8/22/12.
//
//

#import "BreadcrumbsView.h"
#import "../InterfaceHelpers/NSImageView+Coordinates.h"

@implementation BreadcrumbsView

- (id)initWithFrame:(NSRect)frame
{
    self = [super initWithFrame:frame];
    if (self) {
        // Initialization code here.
        
    }
    
    return self;
}

- (void)awakeFromNib
{
    fixedControlWidths = _leftEndImage.frame.size.width + _firstArrowImage.frame.size.width + _secondArrowImage.frame.size.width + _rightEndImage.frame.size.width;
}

- (void)resizeSubviewsWithOldSize:(NSSize)oldSize
{
    //DDLogVerbose(@"called with frame.width=%f", self.frame.size.width);
    
    NSInteger stretchedControlWidths = self.frame.size.width - fixedControlWidths;
    
    [_selectImage setWidth:stretchedControlWidths / 3.0];
    [_selectBackground setWidth:_selectImage.frame.size.width];
    [_scanImage setWidth:stretchedControlWidths / 3.0];
    [_scanBackground setWidth:_scanImage.frame.size.width];
    [_reviewImage setWidth:self.frame.size.width - _selectImage.frame.size.width - _scanImage.frame.size.width - fixedControlWidths];
    [_reviewBackground setWidth:_reviewImage.frame.size.width];
    
    
    // these +1s seem to have no rhyme or reason but without them we don't stretch all the way across. with too many +1s, we have gaps.
    [_firstArrowImage setLeftCoordinate:_selectImage.rightCoordinate];
    [_scanImage setLeftCoordinate:_firstArrowImage.rightCoordinate + 1];
    [_scanBackground setLeftCoordinate:_firstArrowImage.rightCoordinate + 1];
    [_secondArrowImage setLeftCoordinate:_scanImage.rightCoordinate];
    [_reviewImage setLeftCoordinate:_secondArrowImage.rightCoordinate + 1];
    [_reviewBackground setLeftCoordinate:_secondArrowImage.rightCoordinate + 1];
    [_rightEndImage setLeftCoordinate:_reviewImage.rightCoordinate + 1];
    
}

- (void)switchToSelect
{
    [_leftEndImage setImage:[NSImage imageNamed:@"breadcrumbs-active-left-end.png"]];
    [_selectImage setImage:[NSImage imageNamed:@"breadcrumbs-text-addvideos-active.png"]];
    [_selectBackground setImage:[NSImage imageNamed:@"breadcrumbs-active-1px-tile.png"]];
    [_firstArrowImage setImage:[NSImage imageNamed:@"breadcrumbs-first-active-forward-arrow.png"]];
    [_scanImage setImage:[NSImage imageNamed:@"breadcrumbs-text-scanhighlights.png"]];
    [_scanBackground setImage:[NSImage imageNamed:@"breadcrumbs-inactive-1px-tile.png"]];
    [_secondArrowImage setImage:[NSImage imageNamed:@"breadcrumbs-both-inactive-forward-arrow.png"]];
    [_reviewImage setImage:[NSImage imageNamed:@"breadcrumbs-text-saveshare.png"]];
    [_reviewBackground setImage:[NSImage imageNamed:@"breadcrumbs-inactive-1px-tile.png"]];
    [_rightEndImage setImage:[NSImage imageNamed:@"breadcrumbs-inactive-right-end.png"]];
}

- (void)switchToScan
{
    [_leftEndImage setImage:[NSImage imageNamed:@"breadcrumbs-inactive-left-end.png"]];
    [_selectImage setImage:[NSImage imageNamed:@"breadcrumbs-text-addvideos.png"]];
    [_selectBackground setImage:[NSImage imageNamed:@"breadcrumbs-inactive-1px-tile.png"]];
    [_firstArrowImage setImage:[NSImage imageNamed:@"breadcrumbs-second-active-forward-arrow.png"]];
    [_scanImage setImage:[NSImage imageNamed:@"breadcrumbs-text-scanhighlights-active.png"]];
    [_scanBackground setImage:[NSImage imageNamed:@"breadcrumbs-active-1px-tile.png"]];
    [_secondArrowImage setImage:[NSImage imageNamed:@"breadcrumbs-first-active-forward-arrow.png"]];
    [_reviewImage setImage:[NSImage imageNamed:@"breadcrumbs-text-saveshare.png"]];
    [_reviewBackground setImage:[NSImage imageNamed:@"breadcrumbs-inactive-1px-tile.png"]];
    [_rightEndImage setImage:[NSImage imageNamed:@"breadcrumbs-inactive-right-end.png"]];
}

- (void)switchToReview
{
    [_leftEndImage setImage:[NSImage imageNamed:@"breadcrumbs-inactive-left-end.png"]];
    [_selectImage setImage:[NSImage imageNamed:@"breadcrumbs-text-addvideos.png"]];
    [_selectBackground setImage:[NSImage imageNamed:@"breadcrumbs-inactive-1px-tile.png"]];
    [_firstArrowImage setImage:[NSImage imageNamed:@"breadcrumbs-both-inactive-forward-arrow.png"]];
    [_scanImage setImage:[NSImage imageNamed:@"breadcrumbs-text-scanhighlights.png"]];
    [_scanBackground setImage:[NSImage imageNamed:@"breadcrumbs-inactive-1px-tile.png"]];
    [_secondArrowImage setImage:[NSImage imageNamed:@"breadcrumbs-second-active-forward-arrow.png"]];
    [_reviewImage setImage:[NSImage imageNamed:@"breadcrumbs-text-saveshare-active.png"]];
    [_reviewBackground setImage:[NSImage imageNamed:@"breadcrumbs-active-1px-tile.png"]];
    [_rightEndImage setImage:[NSImage imageNamed:@"breadcrumbs-active-right-end.png"]];
}
     

@end

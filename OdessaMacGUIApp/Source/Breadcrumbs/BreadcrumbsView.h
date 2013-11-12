//
//  BreadcrumbsView.h
//  OdessaMacPrototype
//
//  Created by Noah Spitzer-Williams on 8/22/12.
//
//

#import <Cocoa/Cocoa.h>

@interface BreadcrumbsView : NSView
{
    NSInteger fixedControlWidths;
}

@property (unsafe_unretained) IBOutlet NSImageView *leftEndImage;
@property (unsafe_unretained) IBOutlet NSImageView *selectImage;
@property (unsafe_unretained) IBOutlet NSImageView *selectBackground;
@property (unsafe_unretained) IBOutlet NSImageView *firstArrowImage;
@property (unsafe_unretained) IBOutlet NSImageView *scanBackground;
@property (unsafe_unretained) IBOutlet NSImageView *scanImage;
@property (unsafe_unretained) IBOutlet NSImageView *secondArrowImage;
@property (unsafe_unretained) IBOutlet NSImageView *reviewBackground;
@property (unsafe_unretained) IBOutlet NSImageView *reviewImage;
@property (unsafe_unretained) IBOutlet NSImageView *rightEndImage;

- (void)switchToSelect;
- (void)switchToScan;
- (void)switchToReview;

@end

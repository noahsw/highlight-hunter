//
//  BreadcrumbsViewController.h
//  OdessaMacPrototype
//
//  Created by Noah Spitzer-Williams on 8/7/12.
//
//

#import <Cocoa/Cocoa.h>
#import "BreadcrumbsView.h"

@interface BreadcrumbsViewController : NSViewController
{

}


@property (unsafe_unretained) IBOutlet BreadcrumbsView *breadcrumbsView;

- (void)switchToSelect;
- (void)switchToScan;
- (void)switchToReview;

@end

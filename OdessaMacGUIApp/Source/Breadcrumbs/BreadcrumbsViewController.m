//
//  BreadcrumbsViewController.m
//  OdessaMacPrototype
//
//  Created by Noah Spitzer-Williams on 8/7/12.
//
//

#import "BreadcrumbsViewController.h"
#import "BreadcrumbsView.h"

@interface BreadcrumbsViewController ()

@end

@implementation BreadcrumbsViewController

- (id)initWithNibName:(NSString *)nibNameOrNil bundle:(NSBundle *)nibBundleOrNil
{
    self = [super initWithNibName:nibNameOrNil bundle:nibBundleOrNil];
    if (self) {
        // Initialization code here.
        
    }
    
    return self;
}

- (void)switchToSelect
{
    [_breadcrumbsView switchToSelect];
}

- (void)switchToScan
{
    [_breadcrumbsView switchToScan];
}

- (void)switchToReview
{
    [_breadcrumbsView switchToReview];
}


@end

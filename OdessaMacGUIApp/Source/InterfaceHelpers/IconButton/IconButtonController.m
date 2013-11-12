//
//  IconButtonController.m
//  OdessaMacGUIApp
//
//  Created by Noah Spitzer-Williams on 9/6/12.
//  Copyright (c) 2012 Authentically Digital LLC. All rights reserved.
//

#import "IconButtonController.h"

@implementation IconButtonController

- (id)initWithNibName:(NSString *)nibNameOrNil bundle:(NSBundle *)nibBundleOrNil
{
    self = [super initWithNibName:nibNameOrNil bundle:nibBundleOrNil];
    if (self) {
        // Initialization code here.
        
    }
    
    return self;
}

- (void)awakeFromNib
{
    [self.iconImage setIconButtonDelegate:self.button];
}

- (void)setIcon:(NSImage *)image
{
    self.iconImage.image = image;
}

- (void)setButtonText:(NSString *)text
{
    self.button.title = text;
    [self.button setTextColor:[DesignLanguage midGrayColor]];
    
    //NSLog(@"width before: %f", self.button.frame.size.width);
    [self.button sizeToFit];
    //NSLog(@"width after: %f", self.button.frame.size.width);
    
    //NSLog(@"origin.x: %f", self.button.frame.origin.x);
    
    //[self.view setFrameSize:NSMakeSize(self.button.frame.origin.x + self.button.frame.size.width, self.view.frame.size.height)];
}

- (IBAction)takeAction:(id)sender
{
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Warc-performSelector-leaks"
    [self.actionTarget performSelector:self.actionSelector];
#pragma clang diagnostic pop
}


@end

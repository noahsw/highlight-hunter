//
//  TutorialEscapeHatchViewController.m
//  OdessaMacGUIApp
//
//  Created by Noah Spitzer-Williams on 1/5/13.
//  Copyright (c) 2013 Authentically Digital LLC. All rights reserved.
//

#import "TutorialEscapeHatchViewController.h"

@interface TutorialEscapeHatchViewController ()

@end

@implementation TutorialEscapeHatchViewController

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
    [self refreshProgress];
    
}

- (IBAction)exitTutorial:(id)sender {
    
    [TutorialHelper finish];
    
    [self.delegate exitTutorial];
}

- (void)refreshProgress
{
    TutorialProgress progress = [TutorialHelper getProgress];
    
    NSString* string = [NSString stringWithFormat:@"You are taking the Highlight Hunter tour (step %d of %d)", progress + 1, TutorialFinished];
    
    [self.progressText setStringValue:string];
}


@end

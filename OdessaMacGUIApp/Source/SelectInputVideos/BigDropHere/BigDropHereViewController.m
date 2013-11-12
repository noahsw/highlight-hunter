//
//  BigDropHereViewController.m
//  OdessaMacGUIApp
//
//  Created by Noah Spitzer-Williams on 9/6/12.
//  Copyright (c) 2012 Authentically Digital LLC. All rights reserved.
//

#import "BigDropHereViewController.h"

@interface BigDropHereViewController ()

@end

@implementation BigDropHereViewController

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
    [self.dropHereImage setBigDropHereDelegate:self];
 
    [self.bigDropHereView setBigDropHereDelegate:self];

    [self.selectFromCameraButton setHoverImage:[NSImage imageNamed:@"select-button-camera-hover.png"]];
    [self.browseComputerButton setHoverImage:[NSImage imageNamed:@"select-button-browse-hover.png"]];
    
}

- (void)bigDropHereDragEntered
{
    [self.dropHereImage setImage:[NSImage imageNamed:@"select-droptarget-large-active.png"]];
}

- (void)bigDropHereDragExited
{
    [self.dropHereImage setImage:[NSImage imageNamed:@"select-droptarget-large.png"]];
}

- (void)setInputFilesDropDelegate:(id<InputFilesDropDelegate>)newInputFilesDropDelegate
{
    inputFilesDropDelegate = newInputFilesDropDelegate;
    [self.dropHereImage setInputFilesDropDelegate:inputFilesDropDelegate];
    [self.bigDropHereView setInputFilesDropDelegate:inputFilesDropDelegate];
}

- (id<InputFilesDropDelegate>)inputFilesDropDelegate
{
    return inputFilesDropDelegate;
}

- (void)selectFromCamera:(id)sender
{
    [self.genericDropHereDelegate selectFromCamera];
}

- (void)browseComputer:(id)sender
{
    [self.genericDropHereDelegate browseComputer];
}

@end

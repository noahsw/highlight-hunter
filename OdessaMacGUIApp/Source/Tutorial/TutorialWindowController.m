//
//  TutorialWindowController.m
//  OdessaMacGUIApp
//
//  Created by Noah Spitzer-Williams on 11/2/11.
//  Copyright 2011 Authentically Digital LLC. All rights reserved.
//

#import "TutorialWindowController.h"

#import "../CocoaLumberjack/DDLog.h"

static const int ddLogLevel = LOG_LEVEL_VERBOSE | LOG_LEVEL_INFO | LOG_LEVEL_ERROR | LOG_LEVEL_WARN;

@implementation TutorialWindowController


- (id)init
{
    DDLogInfo(@"called");
    
    self = [super initWithWindowNibName:@"TutorialWindow"];
    if (self) {
        // Initialization code here.
    }
    
    return self;
}

- (void)awakeFromNib
{
    DDLogInfo(@"called");
    
    [self.closeButton setHoverImage:[NSImage imageNamed:@"button-green-starttour-hover.png"]];
    
    // Implement this method to handle any initialization after your window controller's window has been loaded from its nib file.
    
    
    // draw background
    
    NSImage* backgroundImage = [NSImage imageNamed:@"background-pattern-102x102-tile.png"];
    [[self window] setBackgroundColor:[NSColor colorWithPatternImage:backgroundImage]];
    
    
}

- (IBAction)closeButtonPressed:(id)sender {
    
    // save setting
    NSUserDefaults *standardUserDefaults = [NSUserDefaults standardUserDefaults];
	if (standardUserDefaults) 
    {
        NSInteger state = 0;
        [standardUserDefaults setInteger:state forKey:@"ShowTutorialv2"];
        [standardUserDefaults synchronize];
    }
    
    // see whether to begin trial
#ifndef APPSTORE
    NSInteger totalLoads = [standardUserDefaults integerForKey:@"TotalLoads"];
    if (totalLoads <= 1 && [Protection getLicenseStatus] == Unlicensed)
    {
        [[AppDelegate sharedAppDelegate] showActivateWindow];
    }
#endif
    
    //[NSApp stopModal]; // if a modal window. but it looks like shit when we do it this way.
    [self closeWindow];
    
}

- (void)trialDidEndSelector
{
    [self closeWindow];
}

- (void) closeWindow
{
    [self close];
    // don't dealloc, otherwise it will crash on a second view
}


@end

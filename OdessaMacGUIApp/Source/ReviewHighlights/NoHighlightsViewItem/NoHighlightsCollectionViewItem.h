//
//  NoHighlightsCollectionViewItem.h
//  OdessaMacGUIApp
//
//  Created by Noah Spitzer-Williams on 9/19/12.
//  Copyright (c) 2012 Authentically Digital LLC. All rights reserved.
//

#import "AMCollectionViewItem.h"
#import "InputFileObject.h"
#import "NoHighlightsDelegate.h"
#import "NoHighlightsObject.h"
#import "BrowserHelper.h"
#import "ScanWorker.h"
#import "NSButton+TextColor.h"
#import "DesignLanguage.h"

@interface NoHighlightsCollectionViewItem : AMCollectionViewItem {
    NoHighlightsObject* noHighlightsObject;
}

@property (unsafe_unretained) NoHighlightsObject* noHighlightsObject;

@property (unsafe_unretained) id<NoHighlightsDelegate> noHighlightsDelegate;

@property (strong) IBOutlet NSButton *supportButton;
@property (strong) IBOutlet NSTextField *statusField;
@property (strong) IBOutlet NSButton *rescanButton;

- (IBAction)openSupport:(id)sender;
- (IBAction)rescan:(id)sender;

@end

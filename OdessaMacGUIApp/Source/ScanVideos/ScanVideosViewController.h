//
//  ScanViewController.h
//  OdessaMacPrototype
//
//  Created by Noah Spitzer-Williams on 8/22/12.
//
//

#import <Cocoa/Cocoa.h>

#import "MainModel.h"

#import "HoverButton.h"
#import "ScanVideosDelegate.h"

#import "DDLog.h"

#import "../GoogleAnalytics/AnalyticsHelper.h"

#import "ScanWorkerHost.h"
#import "ScanWorkerHostDelegate.h"

@interface ScanVideosViewController : NSViewController <ScanWorkerHostDelegate> {

}



@property (unsafe_unretained) MainModel* mainModel;

@property (unsafe_unretained) id<ScanVideosDelegate> delegate;

@property BOOL isScanning;

@property (strong) IBOutlet NSProgressIndicator *progressIndicator;

@property (strong) IBOutlet NSTextField *statusField;

@property (strong) IBOutlet NSTextField *timeRemainingField;

@property (strong) IBOutlet HoverButton *cancelButton;

- (void)startScan;
- (IBAction)cancelScan:(id)sender;

@end

//
//  NoHighlightsCollectionViewItem.m
//  OdessaMacGUIApp
//
//  Created by Noah Spitzer-Williams on 9/19/12.
//  Copyright (c) 2012 Authentically Digital LLC. All rights reserved.
//

#import "NoHighlightsCollectionViewItem.h"

@implementation NoHighlightsCollectionViewItem
@synthesize statusField;
@synthesize rescanButton;

- (id)initWithCollectionView:(AMCollectionView *)theCollectionView representedObject:(id)theObject
{
	self = [super initWithCollectionView:theCollectionView representedObject:theObject];
	if (self != nil) {
		if ([NSBundle loadNibNamed:@"NoHighlightsCollectionViewItem" owner:self]) {
            [self.supportButton setTextColor:[DesignLanguage midGrayColor]];
            [self.rescanButton setTextColor:[DesignLanguage midGrayColor]];
                        
		} else {
			self = nil;
		}
	}
	return self;
}

- (NoHighlightsObject *)noHighlightsObject
{
    return noHighlightsObject;
}

- (void)setNoHighlightsObject:(NoHighlightsObject *)newNoHighlightsObject
{
    noHighlightsObject = newNoHighlightsObject;
    
    InputFileObject* inputFileObject = noHighlightsObject.inputFileObject;
    
    switch (inputFileObject.scanWorkerResult)
    {
        case ScanWorkerResultUnableToDetermineBitrate:
        case ScanWorkerResultUnableToDetermineDimensions:
        case ScanWorkerResultUnableToDetermineFramesPerSecond:
        case ScanWorkerResultUnableToDetermineVideoDuration:
        case ScanWorkerResultUnableToScan:
            statusField.stringValue = [NSString stringWithFormat:@"Error while scanning: %@.\nMake sure you scan the actual video files that came *straight off your camera*.", [ScanWorker getFriendlyScanResult:inputFileObject.scanWorkerResult]];
            [self.supportButton setHidden:NO];
            break;
            
        case ScanWorkerResultCancelled:
            statusField.stringValue = @"Cancelled by user";
            break;
            
        case ScanWorkerResultSuccess:
        {
            NSUserDefaults *standardUserDefaults = [NSUserDefaults standardUserDefaults];
            if (standardUserDefaults)
            {
                NSInteger detectionThreshold = 2;
                detectionThreshold = [standardUserDefaults integerForKey:@"DetectionThreshold"];
                
                BOOL ignoreEarlyHighlights = [standardUserDefaults boolForKey:@"IgnoreEarlyHighlights"];
                
                if (detectionThreshold < 4 || ignoreEarlyHighlights)
                {
                    [self.rescanButton setHidden:NO];
                }
            }
            
            break;
        }
    }

}

- (IBAction)openSupport:(id)sender {
    [BrowserHelper launchBrowser:@"http://support.highlighthunter.com/customer/portal/articles/307210-understanding-scan-errors" term:@"scanerror" addDomain:NO];
}

- (IBAction)rescan:(id)sender {
    
    [self.noHighlightsDelegate rescanRequested];
    
}


@end

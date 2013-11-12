//
//  PublishStatusViewController.m
//  OdessaMacGUIApp
//
//  Created by Noah Spitzer-Williams on 8/30/12.
//  Copyright (c) 2012 Authentically Digital LLC. All rights reserved.
//

#import "PublishStatusViewItem.h"

@interface PublishStatusViewItem ()

@end

@implementation PublishStatusViewItem

- (id)initWithCollectionView:(AMCollectionView *)theCollectionView representedObject:(id)theObject
{
	self = [super initWithCollectionView:theCollectionView representedObject:theObject];
	if (self != nil) {
		if ([NSBundle loadNibNamed:@"PublishStatusViewItem" owner:self]) {
            PublishStatusItem* publishStatusItem = (PublishStatusItem*)theObject;
            
            self.highlightObject = publishStatusItem.highlightObject;
            self.publishWorkerType = publishStatusItem.publishWorkerType;
            self.publishWorker = publishStatusItem.publishWorker;
            
            if (self.publishWorker != nil)
                [self.publishWorker setWorkerProgressDelegateForPublishStatus:self];
            
            [self updateInterface];
            
            [self.imageButton setIconButtonDelegate:self.labelButton]; // so when user scrolls over image, the label will highlight
            
		} else {
			self = nil;
		}
	}
	return self;
}

- (void)updateInterface
{
    
    if (self.publishWorkerType == PublishWorkerTypeNone)
    {
        [self showOpenHighlight];
    }
    if (self.publishWorkerType == PublishWorkerTypeFacebook)
    {
        switch (self.publishWorker.publishWorkerResult)
        {
            case PublishWorkerResultCancelled:
            {
                [self showCancelled:@"Cancelled sharing to facebook"];
                break;
            }
                
            case PublishWorkerResultNotFinished:
            {
                [self showWorking:[NSString stringWithFormat:@"Sharing to facebook (%ld%%)...", self.publishWorker.progress]];
                break;
            }
                
            case PublishWorkerResultSuccess:
            {
                [self showCompleted:@"Shared to facebook (click to view)"];
                break;
            }
                
            case PublishWorkerResultUnableToAuthenticate:
            {
                [self showError:@"Unable to login to facebook"];
                break;
            }
                
            case PublishWorkerResultUnableToShare:
            case PublishWorkerResultUnableToSplice:
            {
                [self showError:@"Error sharing to facebook"];
                break;
            }
        }
    }
    else if (self.publishWorkerType == PublishWorkerTypeSave)
    {
        switch (self.publishWorker.publishWorkerResult)
        {
            case PublishWorkerResultCancelled:
            {
                [self showCancelled:@"Cancelled saving as video file"];
                break;
            }
                
            case PublishWorkerResultNotFinished:
            {
                [self showWorking:[NSString stringWithFormat:@"Saving as video file (%ld%%)...", self.publishWorker.progress]];
                break;
            }
                
            case PublishWorkerResultSuccess:
            {
                [self showCompleted:@"Saved as video file (click to view)"];
                break;
            }
                
            case PublishWorkerResultUnableToSplice:
            {
                [self showError:@"Error saving as video file"];
                break;
            }
                
            default:
            {
                NSAssert(true, @"Case not implemented!");
                break;
            }
        }
    }
    

    
    [self.labelButton setTextColor:[DesignLanguage midGrayColor]];
}

- (void)workerProgressUpdated:(NSInteger)newProgress
{
    [self updateInterface];
}

- (void)showOpenHighlight
{
    self.labelButton.title = @"Save and share this highlight...";
    self.labelButton.textColor = [DesignLanguage midGrayColor];
    self.imageButton.image = [NSImage imageNamed:@"review-sharestatus-icon-share.png"];
    self.labelButton.isActionable = YES;
}

- (void)showWorking:(NSString*)label
{
    self.labelButton.title = label;
    self.labelButton.textColor = [DesignLanguage midGrayColor];
    [self.imageButton setHidden:YES];
    [self.spinnerImage setHidden:NO];
    self.labelButton.isActionable = NO;

}

- (void)showCompleted:(NSString*)label
{
    self.labelButton.title = label;
    self.labelButton.textColor = [DesignLanguage midGrayColor];
    self.imageButton.image = [NSImage imageNamed:@"review-sharestatus-icon-success.png"];
    [self.imageButton setHidden:NO];
    [self.spinnerImage setHidden:YES];
    self.labelButton.isActionable = YES;
    
}

- (void)showCancelled:(NSString*)label
{
    self.labelButton.title = label;
    self.labelButton.textColor = [DesignLanguage midGrayColor];
    self.imageButton.image = [NSImage imageNamed:@"review-sharestatus-icon-error.png"];
    [self.imageButton setHidden:NO];
    [self.spinnerImage setHidden:YES];
    self.labelButton.isActionable = NO;
}

- (void)showError:(NSString*)label
{
    self.labelButton.title = label;
    self.labelButton.textColor = [DesignLanguage midGrayColor];
    self.imageButton.image = [NSImage imageNamed:@"review-sharestatus-icon-error.png"];
    [self.imageButton setHidden:NO];
    [self.spinnerImage setHidden:YES];
    self.labelButton.isActionable = NO;
    
}


- (IBAction)buttonPressed:(id)sender {
    if (self.publishWorkerType == PublishWorkerTypeNone)
    {
        [self.highlightDelegate openHighlightDetails:self.highlightObject];
    }
    else if (self.publishWorker.publishWorkerResult == PublishWorkerResultSuccess)
    {
        
        if (self.publishWorkerType == PublishWorkerTypeFacebook)
        {
            [FacebookShareWorker handleEncodingWarning:self.window];
        }
        
        [self.publishWorker viewResult];
        
        [AppStoreRating handleAppStoreRatingPrompt:self.window];
        

    }
    
}
@end

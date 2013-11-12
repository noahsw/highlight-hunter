//
//  HighlightObject.m
//  OdessaMacGUIApp
//
//  Created by Noah Spitzer-Williams on 8/22/12.
//  Copyright (c) 2012 Authentically Digital LLC. All rights reserved.
//

#import "HighlightObject.h"

@implementation HighlightObject

- (id)init
{
    self = [super init];
    if (self)
    {
        self.thumbnailImage = [NSImage imageNamed:@"review-novideos-question-mark-icon.png"];
        self.hasBeenReviewed = NO;
    }
    return self;
}

+ (BOOL)automaticallyNotifiesObserversForKey:(NSString *)theKey {
    
    BOOL automatic = NO;
    if ([theKey isEqualToString:@"friendlyDuration"]) {
        automatic = NO;
    }
    else {
        automatic = [super automaticallyNotifiesObserversForKey:theKey];
    }
    return automatic;
}

- (SaveWorker *)saveWorker
{
    return saveWorker;
}

- (void)setSaveWorker:(SaveWorker *)newSaveWorker
{
    saveWorker = newSaveWorker;
    
    if (newSaveWorker == nil)
        return;
    
    self.hasBeenReviewed = YES;
    
    PublishStatusItem* publishStatusItem = [PublishStatusItem new];
    publishStatusItem.highlightObject = self;
    publishStatusItem.publishWorkerType = PublishWorkerTypeSave;
    publishStatusItem.publishWorker = (PublishWorker*)newSaveWorker;
    [self.publishStatusDelegate publishStatusItemAdded:publishStatusItem];
}

- (FacebookShareWorker *)facebookShareWorker
{
    return facebookShareWorker;
}

- (void)setFacebookShareWorker:(FacebookShareWorker *)newFacebookShareWorker
{
    facebookShareWorker = newFacebookShareWorker;
    
    if (newFacebookShareWorker == nil)
        return;
    
    self.hasBeenReviewed = YES;
    
    PublishStatusItem* publishStatusItem = [PublishStatusItem new];
    publishStatusItem.highlightObject = self;
    publishStatusItem.publishWorkerType = PublishWorkerTypeFacebook;
    publishStatusItem.publishWorker = (PublishWorker*)newFacebookShareWorker;
    [self.publishStatusDelegate publishStatusItemAdded:publishStatusItem];
}

- (NSTimeInterval)beginTime
{
    return beginTime;
}

- (void)setBeginTime:(NSTimeInterval)newBeginTime
{
    [self willChangeValueForKey:@"friendlyDuration"];
    [self willChangeValueForKey:@"friendlyBeginTime"];
    beginTime = newBeginTime;
    [self didChangeValueForKey:@"friendlyBeginTime"];
    [self didChangeValueForKey:@"friendlyDuration"];
}

- (NSTimeInterval)endTime
{
    return endTime;
}

- (void)setEndTime:(NSTimeInterval)newEndTime
{
    [self willChangeValueForKey:@"friendlyDuration"];
    [self willChangeValueForKey:@"friendlyEndTime"];
    endTime = newEndTime;
    [self didChangeValueForKey:@"friendlyEndTime"];
    [self didChangeValueForKey:@"friendlyDuration"];
}

- (NSString*)friendlyDuration
{
    return [self timeFormatted:self.duration];
}

- (NSString *)friendlyBeginTime
{
    return [self timeFormatted:self.beginTime];
}

- (NSString *)friendlyEndTime
{
    return [self timeFormatted:self.endTime];
}

- (NSTimeInterval)duration
{
    return self.endTime - self.beginTime;
}


- (NSString *)timeFormatted:(NSInteger)totalSeconds
{
    
    NSInteger seconds = totalSeconds % 60;
    NSInteger minutes = (totalSeconds / 60) % 60;
    //NSInteger hours = totalSeconds / 3600;
    
    return [NSString stringWithFormat:@"%ld:%02ld", minutes, seconds];
}



- (void)generateThumbnail:(ThumbnailGenerator *)thumbnailGenerator
{
    
    ThumbnailOperation* thumbnailOperation = [ThumbnailOperation new];
    [thumbnailOperation setDelegate:self];
    [thumbnailOperation setSourceURL:self.inputFileObject.sourceURL];
    [thumbnailOperation setSeekInSeconds:self.endTime];
    [thumbnailOperation setSize:NSMakeSize(272, 153)];
    
    [thumbnailGenerator generateThumbnail:thumbnailOperation];
    
}


- (void)thumbnailGenerated:(NSImage *)thumbnailImage
{
    self.thumbnailImage = thumbnailImage;
}


@end

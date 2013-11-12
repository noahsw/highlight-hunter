//
//  BigDropHereImageView.m
//  OdessaMacGUIApp
//
//  Created by Noah Spitzer-Williams on 9/6/12.
//  Copyright (c) 2012 Authentically Digital LLC. All rights reserved.
//

#import "BigDropHereImageView.h"

#if DEBUG
static const int ddLogLevel = LOG_LEVEL_VERBOSE | LOG_LEVEL_INFO | LOG_LEVEL_ERROR | LOG_LEVEL_WARN;
#else
static const int ddLogLevel = LOG_LEVEL_INFO | LOG_LEVEL_ERROR | LOG_LEVEL_WARN;
#endif

@implementation BigDropHereImageView

- (id)initWithFrame:(NSRect)frame
{
    self = [super initWithFrame:frame];
    if (self) {
        // Initialization code here.
    }
    
    return self;
}


- (void)awakeFromNib
{
    DDLogVerbose(@"registered");
    [self registerForDraggedTypes:@[NSFilenamesPboardType]];
}

- (NSDragOperation)draggingEntered:(id<NSDraggingInfo>)sender
{
    DDLogVerbose(@"entered");
    if ([sender draggingSourceOperationMask] & NSDragOperationLink) {
        [self.bigDropHereDelegate bigDropHereDragEntered];
        return NSDragOperationLink;
    }
    return NSDragOperationNone;
}

- (void)draggingEnded:(id<NSDraggingInfo>)sender
{ // called when user lets go
    DDLogVerbose(@"ended");
    [self.bigDropHereDelegate bigDropHereDragExited];
}

- (void)draggingExited:(id<NSDraggingInfo>)sender
{
    DDLogVerbose(@"exited");
    [self.bigDropHereDelegate bigDropHereDragExited];
}

- (BOOL)prepareForDragOperation:(id<NSDraggingInfo>)sender
{ // return NO if we aren't ready to accept a drop
    return YES;
}

- (BOOL)performDragOperation:(id<NSDraggingInfo>)sender
{
    NSPasteboard* pboard = [sender draggingPasteboard];
    NSString* errorDescription = nil;
    NSData *data = [pboard dataForType:NSFilenamesPboardType];
    NSArray *filenames = [NSPropertyListSerialization
                          propertyListFromData:data
                          mutabilityOption:kCFPropertyListImmutable
                          format:nil
                          errorDescription:&errorDescription];
    
    BOOL isVideoAdded = NO;
    
    for (NSString* filename in filenames)
    {
        NSURL* droppedURL = [NSURL fileURLWithPath:filename];
        DDLogInfo(@"dropped: %@", droppedURL);
        if ([self.inputFilesDropDelegate inputFileAdded:droppedURL])
            isVideoAdded = YES;
    }
    
    if (isVideoAdded)
    {
        [AnalyticsHelper fireEvent:@"Each select - drag and drop" eventValue:0];
    }
    
    return YES;
}

- (void)concludeDragOperation:(id<NSDraggingInfo>)sender
{ // redraw view
    [self setNeedsDisplay:YES];
}





@end

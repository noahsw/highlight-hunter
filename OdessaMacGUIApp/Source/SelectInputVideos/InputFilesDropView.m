//
//  InputFilesDropView.m
//  OdessaMacPrototype
//
//  Created by Noah Spitzer-Williams on 8/23/12.
//
//

#import "InputFilesDropView.h"

#if DEBUG
static const int ddLogLevel = LOG_LEVEL_VERBOSE | LOG_LEVEL_INFO | LOG_LEVEL_ERROR | LOG_LEVEL_WARN;
#else
static const int ddLogLevel = LOG_LEVEL_INFO | LOG_LEVEL_ERROR | LOG_LEVEL_WARN;
#endif

@implementation InputFilesDropView

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
    DDLogInfo(@"registered");
    [self registerForDraggedTypes:@[NSFilenamesPboardType]];
}


/* doesn't work. draws corners behind rectangle
- (void)drawRect:(NSRect)rect
{
    NSRect frameRect = [self bounds];
    
    if(rect.size.height < frameRect.size.height)
        return;
    NSRect newRect = NSMakeRect(rect.origin.x+2, rect.origin.y+2, rect.size.width-3, rect.size.height-3);
    
    NSBezierPath *textViewSurround = [NSBezierPath bezierPathWithRoundedRect:newRect xRadius:10 yRadius:10];
    [textViewSurround setLineWidth:5];
    [[NSColor blueColor] set];
    [textViewSurround stroke];
}
 */

- (NSDragOperation)draggingEntered:(id<NSDraggingInfo>)sender
{
    DDLogVerbose(@"entered");
    if ([sender draggingSourceOperationMask] & NSDragOperationLink) {
        [self.bigDropHereDelegate bigDropHereDragEntered];
        [self.smallDropHereDelegate smallDropHereDragEntered];
        return NSDragOperationLink;
    }
    return NSDragOperationNone;
}

- (void)draggingEnded:(id<NSDraggingInfo>)sender
{ // called when user lets go
    DDLogVerbose(@"ended");
    [self.bigDropHereDelegate bigDropHereDragExited];
    [self.smallDropHereDelegate smallDropHereDragExited];
}

- (void)draggingExited:(id<NSDraggingInfo>)sender
{
    DDLogVerbose(@"exited");
    [self.bigDropHereDelegate bigDropHereDragExited];
    [self.smallDropHereDelegate smallDropHereDragExited];
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

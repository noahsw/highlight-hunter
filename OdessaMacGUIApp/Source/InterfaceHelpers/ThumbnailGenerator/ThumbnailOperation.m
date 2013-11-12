//
//  ThumbnailQueueItem.m
//  OdessaMacPrototype
//
//  Created by Noah Spitzer-Williams on 8/28/12.
//
//

#import "ThumbnailOperation.h"

#if DEBUG
static const int ddLogLevel = LOG_LEVEL_VERBOSE | LOG_LEVEL_INFO | LOG_LEVEL_ERROR | LOG_LEVEL_WARN;
#else
static const int ddLogLevel = LOG_LEVEL_INFO | LOG_LEVEL_ERROR | LOG_LEVEL_WARN;
#endif

@implementation ThumbnailOperation

- (void)main
{
    NSString* uuid = [ThumbnailOperation UUIDString];
    
    NSURL* thumbnailURL = [NSURL fileURLWithPathComponents:@[NSTemporaryDirectory(), [NSString stringWithFormat:@"HH-thumbs-%@.png", uuid]]];
    
    //DDLogVerbose(@"Generating: %@", thumbnailURL.path);
    
    
    
    NSURL* ffmpegPath = [[NSBundle mainBundle] URLForResource:@"ffmpeg" withExtension:nil];
    
    // setup ffmpeg arguments
    NSString* friendlySize = [NSString stringWithFormat:@"%dx%d", (int)self.size.width, (int)self.size.height];
    
    NSArray *arguments = @[@"-ss", [NSString stringWithFormat:@"%f", self.seekInSeconds], @"-t", @"1", @"-i", self.sourceURL.path, @"-y", @"-vframes", @"1", @"-an", @"-s", friendlySize, thumbnailURL.path];
    
    // -t 1     duration of 1 second
    // -y       overwrite existing file
    // -filter:v yadif  deinterlace (not installed)
    // -vframes 1   record 1 video frame
    // -an      disable audio recording
    // -s       size of thumbnail
    
    
    // start ffmpeg
    NSTask *task = [NSTask new];
    [task setLaunchPath:[ffmpegPath path]];
    NSString* curDir = [[MainModel getParentFolder:ffmpegPath] path];
    [task setCurrentDirectoryPath:curDir];
    [task setArguments:arguments];
    
    NSPipe *pipe = [NSPipe pipe];
    [task setStandardError:pipe];
    
    //DDLogVerbose(@"running %@ with arguments %@", ffmpegPath, arguments);
    
    @try
    {
        [task launch];
        
        if (task)
        {
            
            NSDate *loopUntil = [NSDate dateWithTimeIntervalSinceNow:0.5];
            while ([task isRunning] && [[NSRunLoop currentRunLoop] runMode: NSDefaultRunLoopMode beforeDate:loopUntil])
            {
                loopUntil = [NSDate dateWithTimeIntervalSinceNow:0.5];
                
                /*
                NSData *data = [[pipe fileHandleForReading] availableData];
                
                NSString *string = [[NSString alloc] initWithData:data encoding:NSUTF8StringEncoding];
                
                DDLogVerbose(@"ffmpeg output:\n%@", string);
                 */
            }
            
        }
    }
    @catch (NSException *exception)
    {
        DDLogError(@"Exception %@ while launching ffmpeg: %@", [exception name], [exception reason]);
        DDLogError(@"Tried running %@ with arguments %@", ffmpegPath, arguments);
    }

    
    // make sure outputfile looks good
    NSFileManager *fileManager = [NSFileManager defaultManager];
    if ([fileManager fileExistsAtPath:thumbnailURL.path])
    {
        NSImage* image = [[NSImage alloc] initWithContentsOfURL:thumbnailURL];
        
        if (image != nil && [image isValid])
            [self.delegate thumbnailGenerated:image];
        else
            DDLogError(@"We generated an invalid thumbnail image");

        [fileManager removeItemAtURL:thumbnailURL error:nil];
    }
    
    
    
    
}


+(NSString*)UUIDString
{
    
    CFUUIDRef  uuidObj = CFUUIDCreate(nil);
    NSString  *uuidString = (__bridge_transfer NSString*)CFUUIDCreateString(nil, uuidObj);
    CFRelease(uuidObj);
    return uuidString;
}

     
@end

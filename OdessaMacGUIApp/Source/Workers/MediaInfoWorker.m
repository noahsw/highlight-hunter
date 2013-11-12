//
//  MediaInfoWorker.m
//  OdessaMacGUIApp
//
//  Created by Noah Spitzer-Williams on 9/26/12.
//  Copyright (c) 2012 Authentically Digital LLC. All rights reserved.
//

#import "MediaInfoWorker.h"

#if DEBUG
static const int ddLogLevel = LOG_LEVEL_VERBOSE | LOG_LEVEL_INFO | LOG_LEVEL_ERROR | LOG_LEVEL_WARN;
#else
static const int ddLogLevel = LOG_LEVEL_INFO | LOG_LEVEL_ERROR | LOG_LEVEL_WARN;
#endif


@implementation MediaInfoWorker

+ (MediaInfoWorker*)workerWithURL:(NSURL*)url odessaReturnCode:(NSInteger)odessaReturnCode
{
    MediaInfoWorker* newWorker = [MediaInfoWorker new];
    [newWorker setUrl:url];
    [newWorker setOdessaReturnCode:odessaReturnCode];
    
    return newWorker;
}

+ (MediaInfoWorker*)workerWithURL:(NSURL*)url errorCode:(NSString*)errorCode
{
    MediaInfoWorker* newWorker = [MediaInfoWorker new];
    [newWorker setUrl:url];
    [newWorker setErrorCode:errorCode];
    
    return newWorker;
}

- (void)main
{
    
    NSString* output = [self getMediaInfoOutput];
    
    if ([output isEqualToString:@""] == NO)
        [self uploadMediaInfoOutput:output];
    
}

- (NSString*)getMediaInfoOutput
{
    
    NSURL* mediaInfoURL = [MediaInfoWorker getMediaInfoURL];
    
    NSTask *task = [NSTask new];
    [task setLaunchPath:mediaInfoURL.path];
    NSString* curDir = [MainModel getParentFolder:mediaInfoURL].path;
    [task setCurrentDirectoryPath:curDir];
    
    NSArray* arguments = @[@"-f", self.url.path];
    
    [task setArguments:arguments];
    
    NSPipe *pipe = [NSPipe pipe];
    [task setStandardOutput:pipe];

    
    DDLogVerbose(@"running %@ with arguments %@", mediaInfoURL, arguments);
    
    @try
    {
        [task launch];
        
        if (task)
        {
            
            NSMutableString* output = [NSMutableString new];

            NSDate *loopUntil = [NSDate dateWithTimeIntervalSinceNow:0.5];
            while ([task isRunning] && [[NSRunLoop currentRunLoop] runMode: NSDefaultRunLoopMode beforeDate:loopUntil])
            {
                loopUntil = [NSDate dateWithTimeIntervalSinceNow:0.5];
                                
                NSData *data = [[pipe fileHandleForReading] availableData];
                
                NSString *string = [[NSString alloc] initWithData:data encoding:NSUTF8StringEncoding];
                
                [output appendString:string];
                
            }
            
            DDLogVerbose(@"MediaInfo results:\n%@", output);
            
            return output;
        }
    }
    @catch (NSException *exception)
    {
        DDLogError(@"Exception %@ while launching MediaInfo: %@", [exception name], [exception reason]);
        
    }

    return @"";
}

- (void)uploadMediaInfoOutput:(NSString*)output
{
    
    NSURL* url = [NSURL URLWithString:[NSString stringWithFormat:@"%@/scanerror-report.php", [BrowserHelper host]]];
    
    DDLogVerbose(@"Posting to %@", url);
    
    ASIFormDataRequest *request = [ASIFormDataRequest requestWithURL:url];
    
    if (self.odessaReturnCode != Success)
        [request setPostValue:[[NSNumber numberWithInteger:self.odessaReturnCode] stringValue] forKey:@"odessaReturnCode"];
    
    if ([self.errorCode isEqualToString:@""] == NO)
        [request setPostValue:self.errorCode forKey:@"errorCode"];
    
    [request setPostValue:output forKey:@"message"];
    
    NSDictionary* infoDict = [[NSBundle mainBundle] infoDictionary];
    NSString* version = [NSString stringWithFormat:@"Mac %@", infoDict[@"CFBundleShortVersionString"]];
    [request setPostValue:version forKey:@"version"];

    [request startSynchronous];
    
    NSError *error = [request error];
    if (!error) {
        NSString *response = [request responseString];
        DDLogVerbose(@"Response: %@", response);
    }
    
}

+ (NSURL*)getMediaInfoURL
{
    return [[NSBundle mainBundle] URLForResource:@"mediainfo" withExtension:nil];
    
}


@end

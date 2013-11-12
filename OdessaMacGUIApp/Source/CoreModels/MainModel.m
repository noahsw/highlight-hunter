//
//  MainModel.m
//  OdessaMacPrototype
//
//  Created by Noah Spitzer-Williams on 8/28/12.
//
//

#import "MainModel.h"

#if DEBUG
static const int ddLogLevel = LOG_LEVEL_VERBOSE | LOG_LEVEL_INFO | LOG_LEVEL_ERROR | LOG_LEVEL_WARN;
#else
static const int ddLogLevel = LOG_LEVEL_INFO | LOG_LEVEL_ERROR | LOG_LEVEL_WARN;
#endif


@implementation MainModel

- (id)init
{
    self = [super init];
    if (self)
    {
        DDLogVerbose(@"mainModel init");
        self.inputFileObjects = [NSMutableArray new];
        self.highlightObjects = [NSMutableArray new];
    }
    return self;
}

- (void)addInputFileObjectsObject:(InputFileObject *)object
{
    DDLogVerbose(@"called");
    
    [self.inputFileObjects addObject:object];
}



+ (NSURL*) getParentFolder: (NSURL*) url
{
    //DDLogVerbose(@"called for %@", [url path]);
    
    // REMED because we want this to work even if the folder doesn't exist
    //if ([url checkResourceIsReachableAndReturnError:nil] == false)
    //    return nil;
    
    // url = /Users/noahsw/Documents/movie.mpg
    NSString* inputFilePath = [url path];
    if ([inputFilePath length] == 0)
        return nil;
    
    NSString* filename = [url lastPathComponent]; // movie.mpg
    
    if ([filename length] == 0)
        return nil;
    
    NSUInteger endIndex = [inputFilePath length] - [filename length] - 1; // minus 1 for the slash
    
    NSURL* parentFolderPath = [NSURL fileURLWithPath:[inputFilePath substringToIndex:endIndex]];
    
    //DDLogInfo(@"returning %@", [parentFolderPath path]);
    
    return parentFolderPath;
    
}



+ (NSURL*) getDefaultFolderURL
{
    
    NSArray *paths = NSSearchPathForDirectoriesInDomains(NSMoviesDirectory, NSUserDomainMask, YES);
    
    NSURL *defaultFolderURL;
    if ([paths count] > 0) {
        defaultFolderURL = [NSURL fileURLWithPath:paths[0]];
    }
    else
    {
        defaultFolderURL = [NSURL fileURLWithPath: @"/"];
    }
    
    DDLogInfo(@"returning %@", [defaultFolderURL path]);
    
    return defaultFolderURL;
}



+ (NSURL*) getURLToSampleVideo
{
    DDLogInfo(@"called");
    
    NSURL* sampleClipPath = [[NSBundle mainBundle] URLForResource:@"Pro snowboarder Justin Morgan" withExtension:@"mov"];
    if ([sampleClipPath checkResourceIsReachableAndReturnError:nil])
    {
    
        DDLogInfo(@"returning %@", sampleClipPath.path);
    
        return sampleClipPath;
    }
    else
        return nil;
}





@end

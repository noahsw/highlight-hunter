//
//  ExportWorker.m
//  OdessaMacGUIApp
//
//  Created by Noah Spitzer-Williams on 10/1/12.
//  Copyright (c) 2012 Authentically Digital LLC. All rights reserved.
//

#import "ExportWorker.h"

#if DEBUG
static const int ddLogLevel = LOG_LEVEL_VERBOSE | LOG_LEVEL_INFO | LOG_LEVEL_ERROR | LOG_LEVEL_WARN;
#else
static const int ddLogLevel = LOG_LEVEL_INFO | LOG_LEVEL_ERROR | LOG_LEVEL_WARN;
#endif

@implementation ExportWorker

- (void)exportHighlights:(NSWindow *)window
{
    for (HighlightObject* highlightObject in self.highlightObjects)
        highlightObject.hasBeenReviewed = YES; // so we don't pop warning when user quits app
    
    
    // make sure we have enough supported highlights
    if ([[self unsupportedExtensions] count] > 0 && [self removeUnsupportedHighlights] > 0)
    {
        
        NSMutableString* extensionList = [NSMutableString new];
        for (NSString* extension in [self unsupportedExtensions])
        {
            if ([extensionList isEqualToString:@""])
                [extensionList appendString:extension];
            else
                [extensionList appendFormat:@", %@", extension];
        }
        
        NSInteger supportedHighlights = [self.highlightObjects count];
        NSAlert *alert = [[NSAlert alloc] init];
        NSString* errorMsg;
        if (supportedHighlights == 0)
        {
            errorMsg = [NSString stringWithFormat:@"Unfortunately %@ doesn't support the videos with the following extensions: %@.", [self appName], extensionList];
        }
        else
        {
            errorMsg = [NSString stringWithFormat:@"Unfortunately %@ doesn't support the videos with the following extensions: %@.\n\nWe'll automatically remove these videos when we save your project.", [self appName], extensionList];
        }
        [alert addButtonWithTitle:@"OK"];
        [alert setMessageText:@"Unsupported videos"];
        [alert setInformativeText:errorMsg];
        [alert setAlertStyle:NSWarningAlertStyle];
        [alert runModalSheetForWindow:window];
        
        if ([self.highlightObjects count] == 0)
            return;
        
    }
    
    // prompt user to save
    NSSavePanel* savePanel = [NSSavePanel savePanel];
    savePanel.message = [NSString stringWithFormat:@"Where should we save your %@ project file?", [self appName]];
    
    NSUserDefaults *standardUserDefaults = [NSUserDefaults standardUserDefaults];
    if (standardUserDefaults)
    {
        savePanel.directoryURL = [NSURL fileURLWithPath:[standardUserDefaults objectForKey:@"LastSavedPath"]];
    }
    else
        savePanel.directoryURL = [MainModel getDefaultFolderURL];
    
    savePanel.allowsOtherFileTypes = NO;
    
    savePanel.allowedFileTypes = @[[self allowedFileExtension]];
    
    savePanel.nameFieldStringValue = [self projectFileName];
    
    NSInteger result = [savePanel runModal]; // modal so we can pop dialog afterwards without savePanel still being visible
    
    if (result == NSFileHandlingPanelOKButton) {
        
        [standardUserDefaults setObject:[MainModel getParentFolder:savePanel.URL].path forKey:@"LastSavedPath"];
        [standardUserDefaults synchronize];
        
        DDLogInfo(@"Saved path: %@", savePanel.URL.path);
        
        self.projectFileName = savePanel.URL.lastPathComponent;
        
        if ([self exportHighlightsToFile:savePanel.URL])
        {
            [AnalyticsHelper fireEvent:[NSString stringWithFormat:@"Export - %@", [self appName]] eventValue:0];
            
            NSAlert *alert = [[NSAlert alloc] init];
            NSString* errorMsg = [NSString stringWithFormat:@"Highlights successfully saved to %@", savePanel.URL.path];
            [alert addButtonWithTitle:@"Open project"];
            [alert addButtonWithTitle:@"Close"];
            [alert setMessageText:[NSString stringWithFormat:@"Open %@ project?", [self appName]]];
            [alert setInformativeText:errorMsg];
            [alert setAlertStyle:NSWarningAlertStyle];
            NSInteger returnCode = [alert runModalSheetForWindow:window];
            if (returnCode == NSAlertFirstButtonReturn)
            {
                [[NSWorkspace sharedWorkspace] openFile:savePanel.URL.path];
            }
            
        }
        else
        {
            NSAlert *alert = [[NSAlert alloc] init];
            NSString* errorMsg = [NSString stringWithFormat:@"We had trouble saving to %@\n\nPlease make sure you have permission to save to that location.", savePanel.URL.path];
            [alert addButtonWithTitle:@"OK"];
            [alert setMessageText:@"Error saving project"];
            [alert setInformativeText:errorMsg];
            [alert setAlertStyle:NSWarningAlertStyle];
            [alert runModalSheetForWindow:window];
        }
    }

    
}

- (NSString *)appName
{
    NSAssert(false, @"Override by subclass");
    return @"";
}

- (BOOL)exportHighlightsToFile:(NSURL*)outputURL
{
    NSAssert(false, @"Override by subclass");
    return NO;
}

- (NSString*)allowedFileExtension
{
    NSAssert(false, @"Override by subclass");
    return @"";
}

- (NSString*)launchAppMessage
{
    NSAssert(false, @"Override by subclass");
    return @"";
}

- (NSArray *)unsupportedExtensions
{
    NSAssert(false, @"Override by subclass");
    return @[@""];
}


- (NSInteger)removeUnsupportedHighlights
{
    NSInteger count = 0;
    NSInteger i = 0;
    while (i < [self.highlightObjects count])
    {
        HighlightObject* highlightObject = [self.highlightObjects objectAtIndex:i];
        
        for (NSString* extension in [self unsupportedExtensions])
        {
            if ([[highlightObject.inputFileObject.sourceURL.pathExtension uppercaseString] isEqualToString:extension])
            {
                [self.highlightObjects removeObjectAtIndex:i];
                i--;
                count++;
                break;
            }
        }
        i++;
    }

    return count;
}


- (NSArray *)inputFileObjectsWithHighlights
{
    NSMutableArray* inputFileObjects = [NSMutableArray new];
    for (HighlightObject* highlightObject in self.highlightObjects)
    {
        InputFileObject* parentInputFileObject = highlightObject.inputFileObject;
        BOOL isAdded = NO;
        for (InputFileObject* inputFileObject in inputFileObjects)
        {
            if (parentInputFileObject == inputFileObject)
                isAdded = YES;
        }
        if (!isAdded)
            [inputFileObjects addObject:parentInputFileObject];
    }
    
    return inputFileObjects;
}


@end

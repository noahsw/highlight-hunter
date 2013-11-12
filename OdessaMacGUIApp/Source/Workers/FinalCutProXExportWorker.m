//
//  FinalCutProExportWorker.m
//  OdessaMacGUIApp
//
//  Created by Noah Spitzer-Williams on 10/1/12.
//  Copyright (c) 2012 Authentically Digital LLC. All rights reserved.
//

#import "FinalCutProXExportWorker.h"

#if DEBUG
static const int ddLogLevel = LOG_LEVEL_VERBOSE | LOG_LEVEL_INFO | LOG_LEVEL_ERROR | LOG_LEVEL_WARN;
#else
static const int ddLogLevel = LOG_LEVEL_INFO | LOG_LEVEL_ERROR | LOG_LEVEL_WARN;
#endif


@implementation FinalCutProXExportWorker

// http://developer.apple.com/library/mac/documentation/FinalCutProX/Reference/FinalCutProXXMLFormat/FinalCutProXXMLFormat.pdf
// http://postpost.tv/2011/09/fcpxml/
// http://www.philiphodgetts.com/2011/09/why-is-it-so-hard-to-convert-fcp-7-xml-to-fcp-x-xml/


+ (FinalCutProXExportWorker*)workerWithHighlights:(NSArray*)highlightObjects
{
    FinalCutProXExportWorker* newWorker = [[super alloc] init];
    if (newWorker)
    {
        newWorker.highlightObjects = [NSMutableArray arrayWithArray:[highlightObjects copy]]; // copy so we can remove unsupported highlights without affecting main highlights
        newWorker.projectFileName = [NSString stringWithFormat:@"Highlights from Highlight Hunter.%@", [newWorker allowedFileExtension]];
    }
    return newWorker;
}



- (NSString *)appName
{
    return @"Final Cut Pro X";
}

- (NSString *)allowedFileExtension
{
    return @"fcpxml";
}

- (NSArray *)unsupportedExtensions
{
    return @[@"MTS"];
}

- (BOOL)exportHighlightsToFile:(NSURL *)outputURL
{
    
    NSMutableString* output = [NSMutableString new];
    
    [output appendString:@"<fcpxml version=\"1.0\">\n"];
    
    NSDateFormatter* dateFormatter = [[NSDateFormatter alloc] init];
    [dateFormatter setDateStyle:NSDateFormatterShortStyle];
    [dateFormatter setTimeStyle:NSDateFormatterShortStyle];
    NSString *dateString = [dateFormatter stringFromDate:[NSDate date]];
    dateString = [self safeString:dateString]; // needed so there aren't conflicts with event names
    
    [output appendFormat:@"<project name=\"%@ - %@\">\n", self.projectFileName.stringByDeletingPathExtension, dateString];
    
    [output appendString:@"\t<resources>\n"];
    
    //[output appendString:@"\t<projectRef id=\"event\" name=\"Highlights\" />\n"];
    
    // <asset id=”video1” projectRef=”event” src=”file:/Volumes/PostPostMedia/LondonEye.mov”/>
    // url encode. spaces -> %20
    
    // <format id=”myFormat” name=”FFVideoFormat720p25″/>
    NSArray* inputFileObjects = [super inputFileObjectsWithHighlights];

    for (InputFileObject* inputFileObject in inputFileObjects)
    {
        NSInteger assetNumber = [inputFileObjects indexOfObject:inputFileObject];
        NSString* encodedURL = [JFUrlUtil encodeUrl:inputFileObject.sourceURL.absoluteString];
        NSString* asset = [NSString stringWithFormat:@"\t\t<asset id=\"video%ld\" src=\"%@\" />\n", assetNumber, encodedURL]; // projectRef=\"event\" 
        [output appendString:asset];
    }
    
    [output appendString:@"\t\t<format id=\"myFormat\" name=\"FFVideoFormat720p5994\" />\n"]; // TODO: update format attribute
    
    [output appendString:@"\t</resources>\n"];
    
    // <sequence format=”myFormat” tcStart=”36000″>
    // tcStart is in seconds
    
    //[output appendString:@"\t<sequence format=\"myFormat\">\n"]; 
    
    //[output appendString:@"\t\t<spine>\n"];
    
    for (HighlightObject* highlightObject in self.highlightObjects)
    {
        NSInteger assetNumber = [inputFileObjects indexOfObject:highlightObject.inputFileObject];
        NSString* highlightDuration = [self rationalNumber:highlightObject.endTime - highlightObject.beginTime];
        NSString* inputFileDuration = [self rationalNumber:highlightObject.inputFileObject.videoDuration];
        NSString* start = [self rationalNumber:highlightObject.beginTime];
        
        [output appendFormat:@"\t<clip name=\"%@\" duration=\"%@s\" start=\"%@s\" format=\"myFormat\">\n", [self safeString:highlightObject.title], highlightDuration, start];
        
        [output appendFormat:@"\t\t<video ref=\"video%ld\" duration=\"%@s\">\n", assetNumber, inputFileDuration];
        
        [output appendFormat:@"\t\t\t<audio lane=\"-1\" ref=\"video%ld\" duration=\"%@s\" />\n", assetNumber, inputFileDuration];
        
        [output appendString:@"\t\t</video>\n"];
        
        [output appendString:@"\t</clip>\n"];
    }
    
    //[output appendString:@"\t\t</spine>\n"];
    
    //[output appendString:@"\t</sequence>\n"];
    
    [output appendString:@"</project>\n"];
    
    [output appendString:@"</fcpxml>\n"];
    
    // save to file
    NSError *error;
    BOOL succeed = [output writeToURL:outputURL atomically:YES encoding:NSUTF8StringEncoding error:&error];
    if (!succeed)
    {
        DDLogError(@"Error writing: %@", error);
        return NO;
    }

    return YES;
}


- (NSString*)rationalNumber:(NSTimeInterval)duration
{
    long maxDenominator = 42;
    Fraction aFraction = fractionFromReal(duration, maxDenominator);
    
    return [NSString stringWithFormat:@"%ld/%ld", aFraction.nominator, aFraction.denominator];
}

- (NSString*)safeString:(NSString*)string
{
    string = [string stringByReplacingOccurrencesOfString:@"/" withString:@"-"];
    string = [string stringByReplacingOccurrencesOfString:@"\"" withString:@"'"]; // remove quotes
    string = [string stringByReplacingOccurrencesOfString:@"*" withString:@""];
    string = [string stringByReplacingOccurrencesOfString:@"?" withString:@""];
    string = [string stringByReplacingOccurrencesOfString:@":" withString:@"."];
    string = [string stringByReplacingOccurrencesOfString:@"~" withString:@""];
    return string;
}


/*
 * Find rational approximation to given real number
 * David Eppstein / UC Irvine / 8 Aug 1993
 *
 * With corrections from Arno Formella, May 2008
 * Function wrapper by Regexident, April 2011
 *
 * usage: fractionFromReal(double realNumber, long maxDenominator)
 *   realNumber: is real number to approx
 *   maxDenominator: is the maximum denominator allowed
 *
 * based on the theory of continued fractions
 * if x = a1 + 1/(a2 + 1/(a3 + 1/(a4 + ...)))
 * then best approximation is found by truncating this series
 * (with some adjustments in the last term).
 *
 * Note the fraction can be recovered as the first column of the matrix
 *  ( a1 1 ) ( a2 1 ) ( a3 1 ) ...
 *  ( 1  0 ) ( 1  0 ) ( 1  0 )
 * Instead of keeping the sequence of continued fraction terms,
 * we just keep the last partial product of these matrices.
 */
Fraction fractionFromReal(double realNumber, long maxDenominator) {
    double atof();
    int atoi();
    void exit();
    
    long m[2][2];
    double startx;
    long ai;
    
    startx = realNumber;
    
    // initialize matrix:
    m[0][0] = m[1][1] = 1;
    m[0][1] = m[1][0] = 0;
    
    // loop finding terms until denom gets too big:
    while (m[1][0] *  (ai = (long)realNumber) + m[1][1] <= maxDenominator) {
        long t;
        t = m[0][0] * ai + m[0][1];
        m[0][1] = m[0][0];
        m[0][0] = t;
        t = m[1][0] * ai + m[1][1];
        m[1][1] = m[1][0];
        m[1][0] = t;
        
        if (realNumber == (double)ai) {
            // AF: division by zero
            break;
        }
        
        realNumber = 1 / (realNumber - (double)ai);
        
        if (realNumber > (double)0x7FFFFFFF) {
            // AF: representation failure
            break;
        }
    }
    
    ai = (maxDenominator - m[1][1]) / m[1][0];
    m[0][0] = m[0][0] * ai + m[0][1];
    m[1][0] = m[1][0] * ai + m[1][1];
    return (Fraction) { .nominator = m[0][0], .denominator = m[1][0], .error = startx - ((double)m[0][0] / (double)m[1][0]) };
}

/*
 <format> Names:
 FFVideoFormatRateUndefined
 FFVideoFormat1080i5994
 FFVideoFormat1080p2398
 FFVideoFormat1080p24
 FFVideoFormat1080p25
 FFVideoFormat1080p2997
 FFVideoFormat1080p30
 FFVideoFormat1080p50
 FFVideoFormat1080p5994
 FFVideoFormat1080p60
 FFVideoFormat1280x1080i5994
 FFVideoFormat1280x1080p2398
 FFVideoFormat1280x1080p24
 FFVideoFormat1280x1080p2997
 FFVideoFormat1280x1080p30
 FFVideoFormat1280x1080p5994
 FFVideoFormat1280x1080p60
 FFVideoFormat1440x1080i50
 FFVideoFormat1440x1080i5994
 FFVideoFormat1440x1080p2398
 FFVideoFormat1440x1080p24
 FFVideoFormat1440x1080p25
 FFVideoFormat1440x1080p2997
 FFVideoFormat1440x1080p30
 FFVideoFormat1440x1080p50
 FFVideoFormat1440x1080p5994
 FFVideoFormat1440x1080p60
 FFVideoFormat2048x1024p2398
 FFVideoFormat2048x1024p24
 FFVideoFormat2048x1024p25
 FFVideoFormat2048x1024p2997
 FFVideoFormat2048x1024p30
 FFVideoFormat2048x1024p50
 FFVideoFormat2048x1024p5994
 FFVideoFormat2048x1024p60
 FFVideoFormat2048x1080p2398
 FFVideoFormat2048x1080p24
 FFVideoFormat2048x1080p25
 FFVideoFormat2048x1080p2997
 FFVideoFormat2048x1080p30
 FFVideoFormat2048x1080p50
 FFVideoFormat2048x1080p5994
 FFVideoFormat2048x1080p60
 FFVideoFormat2048x1152p2398
 FFVideoFormat2048x1152p24
 FFVideoFormat2048x1152p25
 FFVideoFormat2048x1152p2997
 FFVideoFormat2048x1152p30
 FFVideoFormat2048x1152p50
 FFVideoFormat2048x1152p5994
 FFVideoFormat2048x1152p60
 FFVideoFormat2048x1556p2398
 FFVideoFormat2048x1556p24
 FFVideoFormat2048x1556p25
 FFVideoFormat2048x1556p2997
 FFVideoFormat2048x1556p30
 FFVideoFormat2048x1556p50
 FFVideoFormat2048x1556p5994
 FFVideoFormat2048x1556p60
 FFVideoFormat4096x2048p2398
 FFVideoFormat4096x2048p24
 FFVideoFormat4096x2048p25
 FFVideoFormat4096x2048p2997
 FFVideoFormat4096x2048p30
 FFVideoFormat4096x2048p50
 FFVideoFormat4096x2048p5994
 FFVideoFormat4096x2048p60
 FFVideoFormat4096x2160p2398
 FFVideoFormat4096x2160p24
 FFVideoFormat4096x2160p25
 FFVideoFormat4096x2160p2997
 FFVideoFormat4096x2160p30
 FFVideoFormat4096x2160p50
 FFVideoFormat4096x2160p5994
 FFVideoFormat4096x2160p60
 FFVideoFormat4096x2304p2398
 FFVideoFormat4096x2304p24
 FFVideoFormat4096x2304p25
 FFVideoFormat4096x2304p2997
 FFVideoFormat4096x2304p30
 FFVideoFormat4096x2304p50
 FFVideoFormat4096x2304p5994
 FFVideoFormat4096x2304p60
 FFVideoFormat4096x3112p2398
 FFVideoFormat4096x3112p24
 FFVideoFormat4096x3112p25
 FFVideoFormat4096x3112p2997
 FFVideoFormat4096x3112p30
 FFVideoFormat4096x3112p50
 FFVideoFormat4096x3112p5994
 FFVideoFormat4096x3112p60
 FFVideoFormat640x480p2398
 FFVideoFormat640x480p24
 FFVideoFormat640x480p25
 FFVideoFormat640x480p2997
 FFVideoFormat640x480p30
 FFVideoFormat720p2398
 FFVideoFormat720p24
 FFVideoFormat720p25
 FFVideoFormat720p2997
 FFVideoFormat720p30
 FFVideoFormat720p50
 FFVideoFormat720p5994
 FFVideoFormat720p60
 FFVideoFormat720x486i5994
 FFVideoFormat720x486i5994_16x9
 FFVideoFormat720x486p2398
 FFVideoFormat720x486p2398_16x9
 FFVideoFormat720x486p2997
 FFVideoFormat720x486p2997_16x9
 FFVideoFormat720x576i50
 FFVideoFormat720x576i50_16x9
 FFVideoFormat720x576p25
 FFVideoFormat720x576p25_16x9
 FFVideoFormat960x540p2398
 FFVideoFormat960x540p24
 FFVideoFormat960x540p25
 FFVideoFormat960x540p2997
 FFVideoFormat960x540p30
 FFVideoFormat960x720p2398
 FFVideoFormat960x720p24
 FFVideoFormat960x720p25
 FFVideoFormat960x720p2997
 FFVideoFormat960x720p30
 FFVideoFormat960x720p50
 FFVideoFormat960x720p5994
 FFVideoFormat960x720p60
 FFVideoFormatDV720x480i5994
 FFVideoFormatDV720x480i5994_16x9
 FFVideoFormatDV720x480p2398
 FFVideoFormatDV720x480p2398_16x9
 FFVideoFormatDV720x480p2997
 FFVideoFormatDV720x480p2997_16x9
 FFVideoFormatDV720x576i50
 FFVideoFormatDV720x576i50_16x9
 
 */
@end

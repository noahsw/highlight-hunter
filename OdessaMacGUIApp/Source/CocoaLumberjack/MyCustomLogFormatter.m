//
//  MyCustomLogFormatter.m
//  OdessaMacGUIApp
//
//  Created by Noah Spitzer-Williams on 12/13/11.
//  Copyright (c) 2011 Authentically Digital LLC. All rights reserved.
//

#import "MyCustomLogFormatter.h"

@implementation MyCustomLogFormatter

- (id)init
{
    if((self = [super init]))
    {
        dateFormatter = [[NSDateFormatter alloc] init];
        [dateFormatter setFormatterBehavior:NSDateFormatterBehavior10_4];
        [dateFormatter setDateFormat:@"MM/dd/yy HH:mm:ss"];
    }
    return self;
}

- (NSString *)formatLogMessage:(DDLogMessage *)logMessage
{
    NSString *logLevel;
    switch (logMessage->logFlag)
    {
        case LOG_FLAG_ERROR : logLevel = @"E"; break;
        case LOG_FLAG_WARN  : logLevel = @"W"; break;
        case LOG_FLAG_INFO  : logLevel = @"I"; break;
        default             : logLevel = @"V"; break;
    }
    
    NSString *dateAndTime = [dateFormatter stringFromDate:(logMessage->timestamp)];
    
    NSString *logMsg = logMessage->logMsg;
    
    return [NSString stringWithFormat:@"%@ %@ %@ %@ | %@\n", logLevel, dateAndTime, logMessage.fileName, logMessage.methodName, logMsg];
}


@end
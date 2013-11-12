//
//  MyCustomLogFormatter.h
//  OdessaMacGUIApp
//
//  Created by Noah Spitzer-Williams on 12/13/11.
//  Copyright (c) 2011 Authentically Digital LLC. All rights reserved.
//

#import <Foundation/Foundation.h>
#import "DDLog.h"

@interface MyCustomLogFormatter : NSObject <DDLogFormatter>
{
    NSDateFormatter *dateFormatter;
}

@end

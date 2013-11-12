//
//  main.m
//  OdessaGUIProject-Mac
//
//  Created by Noah Spitzer-Williams on 9-17-11.
//  Copyright (c) 2011 Authentically Digital LLC. All rights reserved.
//

#import <Cocoa/Cocoa.h>
#import "ReceiptValidation/RVNReceiptValidation.h"


int main(int argc, char *argv[])
{
    @autoreleasepool {
        return NSApplicationMain(argc, (const char **)argv);

        // use RVN if we want to validate the bundle signing
        //        return RVNValidateAndRunApplication(argc, argv);
    }
}



//
//  RVNReceiptValidation.h
//  OdessaMacGUIApp
//
//  Created by Noah Spitzer-Williams on 8/24/12.
//  Copyright (c) 2012 Authentically Digital LLC. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <Security/SecStaticCode.h>

@interface RVNReceiptValidation : NSObject

int RVNValidateAndRunApplication(int argc, char *argv[]);

@end

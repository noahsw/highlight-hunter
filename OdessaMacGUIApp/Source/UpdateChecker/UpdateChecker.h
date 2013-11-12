//
//  UpdateChecker.h
//  OdessaMacGUIApp
//
//  Created by Noah Spitzer-Williams on 12/19/11.
//  Copyright (c) 2011 Authentically Digital LLC. All rights reserved.
//

@interface UpdateChecker : NSObject
{
    struct UpdateResult
    {
        bool isUpdateAvailable;
        __unsafe_unretained NSString* latestVersion;
        __unsafe_unretained NSString* whatsNew;
        __unsafe_unretained NSString* downloadPage;
    };

@private
    
}


- (void) checkForUpdate: (NSWindow*)window;
- (bool) isTimeToCheck;

@end
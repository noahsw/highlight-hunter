//
//  UpdateCheckerWorker.h
//  OdessaMacGUIApp
//
//  Created by Noah Spitzer-Williams on 12/19/11.
//  Copyright (c) 2011 Authentically Digital LLC. All rights reserved.
//

@interface UpdateCheckerWorker : NSOperation
{
    
    @private
    NSWindow *window;
}

@property(retain) NSWindow* window;


- (struct UpdateResult) getUpdateResult;
- (void) promptUserForUpdate: (NSValue*)value;


@end

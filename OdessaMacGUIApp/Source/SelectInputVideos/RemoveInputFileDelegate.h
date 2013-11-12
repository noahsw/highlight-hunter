//
//  RemoveInputFileDelegate.h
//  OdessaMacGUIApp
//
//  Created by Noah Spitzer-Williams on 9/21/12.
//  Copyright (c) 2012 Authentically Digital LLC. All rights reserved.
//

#import <Foundation/Foundation.h>
@class InputFileObject;

@protocol RemoveInputFileDelegate <NSObject>

- (void)removeInputFile:(InputFileObject*)inputFileObject;

@end

//
//  InputFilesDropDelegate.h
//  OdessaMacPrototype
//
//  Created by Noah Spitzer-Williams on 8/28/12.
//
//

#import <Foundation/Foundation.h>

@protocol InputFilesDropDelegate <NSObject>

-(BOOL)inputFileAdded:(NSURL*)file;

@end

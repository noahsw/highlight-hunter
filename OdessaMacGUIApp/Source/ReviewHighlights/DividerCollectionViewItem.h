//
//  HighlightHeaderCollectionViewItem.h
//  OdessaMacGUIApp
//
//  Created by Noah Spitzer-Williams on 9/10/12.
//  Copyright (c) 2012 Authentically Digital LLC. All rights reserved.
//

#import "AMCollectionViewItem.h"
#import "InputFileObject.h"
#import "HighlightObject.h"
#import "MainModel.h"

@interface DividerCollectionViewItem : AMCollectionViewItem {
}

@property (unsafe_unretained) InputFileObject* inputFileObject;

@property (unsafe_unretained) MainModel* mainModel;

@property (strong) IBOutlet NSTextField *headerLabel;

- (void)updateLabel;

@end

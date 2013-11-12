//
//  FacebookTestAppDelegate.h
//  FacebookTest
//
//  Created by Philippe on 10-08-25.
//  Copyright 2010 Philippe Casgrain. All rights reserved.
//

#import <Cocoa/Cocoa.h>
#import <PhFacebook/PhFacebook.h>

@interface FacebookTestAppDelegate : NSObject <PhFacebookDelegate>
{
    PhFacebook *fb;

    NSTextField *__unsafe_unretained token_label;
    NSTextField *__unsafe_unretained request_label;
    NSTextField *__unsafe_unretained request_text;
    NSTextView *__unsafe_unretained result_text;
    NSImageView *__unsafe_unretained profile_picture;
    NSButton *__unsafe_unretained send_request;
    NSWindow *__unsafe_unretained window;
}

@property (unsafe_unretained) IBOutlet NSTextField *token_label;
@property (unsafe_unretained) IBOutlet NSTextField *request_label;
@property (unsafe_unretained) IBOutlet NSTextField *request_text;
@property (unsafe_unretained) IBOutlet NSTextView *result_text;
@property (unsafe_unretained) IBOutlet NSImageView *profile_picture;
@property (unsafe_unretained) IBOutlet NSButton *send_request;
@property (unsafe_unretained) IBOutlet NSWindow *window;
@property (unsafe_unretained) IBOutlet NSTextField *movie_path_text;
@property (unsafe_unretained) IBOutlet NSButton *post_request;

- (IBAction) getAccessToken: (id) sender;
- (IBAction) sendRequest: (id) sender;
- (IBAction)postRequest:(id)sender;

@end

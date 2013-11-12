//
//  FacebookTestAppDelegate.m
//  FacebookTest
//
//  Created by Philippe on 10-08-25.
//  Copyright 2010 Philippe Casgrain. All rights reserved.
//

#import "FacebookTestAppDelegate.h"
#import "ApplicationID.h"

@implementation FacebookTestAppDelegate
@synthesize token_label;
@synthesize request_label;
@synthesize request_text;
@synthesize result_text;
@synthesize profile_picture;
@synthesize send_request;
@synthesize window;

- (void) applicationDidFinishLaunching: (NSNotification*) aNotification
{
    fb = [[PhFacebook alloc] initWithApplicationID: APPLICATION_ID delegate: self];
    self.token_label.stringValue = @"Invalid";
    [self.request_label setEnabled: NO];
    [self.request_text setEnabled: NO];
    [self.send_request setEnabled: NO];
    [self.result_text setEditable: NO];
    [self.result_text setFont: [NSFont fontWithName: @"Monaco" size: 10.0]];
}

#pragma mark IBActions

- (IBAction) getAccessToken: (id) sender
{
    // Always get a new token, don't get a cached one
    [fb getAccessTokenForPermissions: [NSArray arrayWithObjects: @"read_stream", @"export_stream", nil] cached: NO];
}

- (IBAction) sendRequest: (id) sender
{
    [self.send_request setEnabled: NO];
    [self.post_request setEnabled:NO];

    [fb sendRequest: request_text.stringValue];

}

- (IBAction)postRequest:(id)sender {
    [self.send_request setEnabled:NO];
    [self.post_request setEnabled:NO];
    
//    NSData* data = [NSData dataWithContentsOfFile:self.movie_path_text.stringValue];
    
    NSDictionary* params = @{
        @"source" : self.movie_path_text.stringValue,
        @"title" : @"test",
        @"description" : @"test"
    };
    
    [fb sendRequest:request_text.stringValue params:params usePostRequest:YES];
}

#pragma mark PhFacebookDelegate methods

- (void) tokenResult: (NSDictionary*) result
{
    if ([[result valueForKey: @"valid"] boolValue])
    {
        self.token_label.stringValue = @"Valid";
        [self.request_label setEnabled: YES];
        [self.request_text setEnabled: YES];
        [self.send_request setEnabled: YES];
        [self.result_text setEditable: YES];
        [fb sendRequest: @"me/picture"];
    }
    else
    {
        [self.result_text setString: [NSString stringWithFormat: @"Error: {%@}", [result valueForKey: @"error"]]];
        
    }
}

- (void) requestResult: (NSDictionary*) result
{
    if ([[result objectForKey: @"request"] isEqualTo: @"me/picture"])
    {
        NSImage *pic = [[NSImage alloc] initWithData: [result objectForKey: @"raw"]];
        self.profile_picture.image = pic;
        
    } else 
    {
        [self.send_request setEnabled: YES];
        [self.result_text setString: [NSString stringWithFormat: @"Request: {%@}\n%@", [result objectForKey: @"request"], [result objectForKey: @"result"]]];
        
        NSData *jsonData = [[result objectForKey:@"result"] dataUsingEncoding:NSUTF8StringEncoding];
        id jsonObject = [NSJSONSerialization JSONObjectWithData:jsonData options:NSJSONReadingMutableContainers error:nil];
        
        if ([jsonObject isKindOfClass:[NSArray class]]) {
            NSLog(@"its an array!");
            NSArray *jsonArray = (NSArray *)jsonObject;
            NSLog(@"jsonArray - %@",jsonArray);
        }
        else {
            NSLog(@"its probably a dictionary");
            NSDictionary *jsonDictionary = (NSDictionary *)jsonObject;
            NSLog(@"jsonDictionary - %@",jsonDictionary);
        }
    }
}

- (void) willShowUINotification: (PhFacebook*) sender
{
    [NSApp requestUserAttention: NSInformationalRequest];
}

@end

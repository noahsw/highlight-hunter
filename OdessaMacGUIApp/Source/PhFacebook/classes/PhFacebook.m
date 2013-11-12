//
//  PhFacebook.m
//  PhFacebook
//
//  Created by Philippe on 10-08-25.
//  Copyright 2010 Philippe Casgrain. All rights reserved.
//

#import "PhFacebook.h"
#import "PhWebViewController.h"
#import "PhAuthenticationToken.h"
#import "PhFacebook_URLs.h"
#import "Debug.h"
#import <ASIHTTPRequest/ASIFormDataRequest.h>
#import <ASIHTTPRequest/ASIHTTPRequest.h>

#define kFBStoreAccessToken @"FBAStoreccessToken"
#define kFBStoreTokenExpiry @"FBStoreTokenExpiry"
#define kFBStoreAccessPermissions @"FBStoreAccessPermissions"

@interface PhFacebook ()

@property (strong) ASIFormDataRequest* asiFormDataRequest;

@end

@implementation PhFacebook

#pragma mark Initialization

- (id) initWithApplicationID: (NSString*) appID delegate: (id) delegate
{
    if ((self = [super init]))
    {
        if (appID)
            _appID = [NSString stringWithString: appID];
        _delegate = delegate; // Don't retain delegate to avoid retain cycles
        _webViewController = nil;
        _authToken = nil;
        _permissions = nil;
        NSLog(@"Initialized with AppID '%@'", _appID);
    }

    return self;
}


- (void) notifyDelegateForToken: (PhAuthenticationToken*) token withError: (NSString*) errorReason
{
    NSMutableDictionary *result = [NSMutableDictionary dictionary];
    if (token)
    {
        // Save it to user defaults
        NSUserDefaults *defaults = [NSUserDefaults standardUserDefaults];
        [defaults setObject: token.authenticationToken forKey: kFBStoreAccessToken];
        if (token.expiry)
            [defaults setObject: token.expiry forKey: kFBStoreTokenExpiry];
        else
            [defaults removeObjectForKey: kFBStoreTokenExpiry];
        [defaults setObject: token.permissions forKey: kFBStoreAccessPermissions];
        [defaults synchronize];

        [result setObject: [NSNumber numberWithBool: YES] forKey: @"valid"];
    }
    else
    {
        [result setObject: [NSNumber numberWithBool: NO] forKey: @"valid"];
        [result setObject: errorReason forKey: @"error"];
    }

    if ([_delegate respondsToSelector: @selector(tokenResult:)])
        [_delegate tokenResult: result];
}

#pragma mark Access

- (void) clearToken
{
    _authToken = nil;
}

-(void) invalidateCachedToken
{
    [self clearToken];
    NSUserDefaults *defaults = [NSUserDefaults standardUserDefaults];
    [defaults removeObjectForKey:kFBStoreAccessToken];
    [defaults removeObjectForKey: kFBStoreTokenExpiry];
    [defaults removeObjectForKey: kFBStoreAccessPermissions];

    // Allow logout by clearing the left-over cookies (issue #35)
    NSURL *facebookUrl = [NSURL URLWithString:kFBURL];
    NSURL *facebookSecureUrl = [NSURL URLWithString:kFBSecureURL];

    NSHTTPCookieStorage *cookieStorage = [NSHTTPCookieStorage sharedHTTPCookieStorage];
    NSArray *cookies = [[cookieStorage cookiesForURL: facebookUrl] arrayByAddingObjectsFromArray:[cookieStorage cookiesForURL: facebookSecureUrl]];

    for (NSHTTPCookie *cookie in cookies)
        [cookieStorage deleteCookie: cookie];
}

- (void) setAccessToken: (NSString*) accessToken expires: (NSTimeInterval) tokenExpires permissions: (NSString*) perms
{
    [self clearToken];

    if (accessToken)
        _authToken = [[PhAuthenticationToken alloc] initWithToken: accessToken secondsToExpiry: tokenExpires permissions: perms];
}

- (void) getAccessTokenForPermissions: (NSArray*) permissions cached: (BOOL) canCache modalWindow:(NSWindow*)modalWindow
{
    BOOL validToken = NO;
    NSString *scope = [permissions componentsJoinedByString: @","];

    if (canCache && _authToken == nil)
    {
        NSUserDefaults *defaults = [NSUserDefaults standardUserDefaults];
        NSString *accessToken = [defaults stringForKey: kFBStoreAccessToken];
        NSDate *date = [defaults objectForKey: kFBStoreTokenExpiry];
        NSString *perms = [defaults stringForKey: kFBStoreAccessPermissions];
        if (accessToken && perms)
        {
            // Do not notify delegate yet...
            [self setAccessToken: accessToken expires: [date timeIntervalSinceNow] permissions: perms];
        }
    }

    if ([_authToken.permissions isCaseInsensitiveLike: scope])
    {
        // We already have a token for these permissions; check if it has expired or not
        if (_authToken.expiry == nil || [[_authToken.expiry laterDate: [NSDate date]] isEqual: _authToken.expiry])
            validToken = YES;
    }

    if (validToken)
    {
        [self notifyDelegateForToken: _authToken withError: nil];
    }
    else
    {
        [self clearToken];

        // Use _webViewController to request a new token
        NSString *authURL;
        if (scope)
            authURL = [NSString stringWithFormat: kFBAuthorizeWithScopeURL, _appID, kFBLoginSuccessURL, scope];
        else
            authURL = [NSString stringWithFormat: kFBAuthorizeURL, _appID, kFBLoginSuccessURL];
      
        if ([_delegate respondsToSelector: @selector(needsAuthentication:forPermissions:)]) 
        {
            if ([_delegate needsAuthentication: authURL forPermissions: scope]) 
            {
                // If needsAuthentication returns YES, let the delegate handle the authentication UI
                return;
            }
        }
      
        // Retrieve token from web page
        if (_webViewController == nil)
        {
            _webViewController = [[PhWebViewController alloc] init];
            [NSBundle loadNibNamed: @"FacebookBrowser" owner: _webViewController];
        }

        // Prepare window but keep it ordered out. The _webViewController will make it visible
        // if it needs to.
        
        /*
        [NSApp beginSheet: _webViewController.window
           modalForWindow: modalWindow
            modalDelegate: nil
           didEndSelector: nil
              contextInfo: nil];
         */

        _webViewController.parent = self;
        _webViewController.permissions = scope;
        [_webViewController.webView setMainFrameURL: authURL];

        /*
        [NSApp runModalForWindow: _webViewController.window];
        // Dialog is up here.
        [NSApp endSheet: _webViewController.window];
         
        [_webViewController.window orderOut: self];
         */
    }
}

- (void) setAccessToken: (NSString*) accessToken expires: (NSTimeInterval) tokenExpires permissions: (NSString*) perms error: (NSString*) errorReason
{
	[self setAccessToken: accessToken expires: tokenExpires permissions: perms];
	[self notifyDelegateForToken: _authToken withError: errorReason];
}

- (NSString*) accessToken
{
    return [[_authToken.authenticationToken copy] autorelease];
}

- (void) sendFacebookRequest: (NSDictionary*) allParams
{
    @autoreleasepool {

        if (_authToken)
        {
            NSString *request = [allParams objectForKey: @"request"];
            NSString *str;
            BOOL postRequest = [[allParams objectForKey: @"postRequest"] boolValue];
                    
            if (postRequest)
            {
                str = [NSString stringWithFormat: kFBGraphApiPostURL, request];
                self.asiFormDataRequest = [ASIFormDataRequest requestWithURL:[NSURL URLWithString:str]];
                
            }
            else
            {
                // Check if request already has optional parameters
                NSString *formatStr = kFBGraphApiGetURL;
                NSRange rng = [request rangeOfString:@"?"];
                if (rng.length > 0)
                    formatStr = kFBGraphApiGetURLWithParams;
                str = [NSString stringWithFormat: formatStr, request, _authToken.authenticationToken];
            }

            
            NSDictionary *params = [allParams objectForKey: @"params"];
            //NSMutableString *strPostParams = nil;
            if (params != nil) 
            {
                if (postRequest)
                {
                    [self.asiFormDataRequest setPostValue:_authToken.authenticationToken forKey:@"access_token"];
                    
                    //strPostParams = [NSMutableString stringWithFormat: @"access_token=%@", _authToken.authenticationToken];
                    for (NSString *p in [params allKeys])
                    {
                        if ([p isEqualToString:@"source"])
                        {
                            [self.asiFormDataRequest setFile:[params objectForKey:p] forKey:p];
                        }
                        else
                        {
                            [self.asiFormDataRequest setPostValue:[params objectForKey:p] forKey:p];
                        }
                        //[strPostParams appendFormat: @"&%@=%@", p, [params objectForKey: p]];
                    }
                }
                else
                {
                    NSMutableString *strWithParams = [NSMutableString stringWithString: str];
                    for (NSString *p in [params allKeys]) 
                        [strWithParams appendFormat: @"&%@=%@", p, [params objectForKey: p]];
                    str = strWithParams;
                }
            }
            
            if (postRequest)
            {
                [self.asiFormDataRequest setDelegate:self];
                [self.asiFormDataRequest setUploadProgressDelegate:self];
                [self.asiFormDataRequest startAsynchronous];
                /*
                NSData *requestData = [NSData dataWithBytes: [strPostParams UTF8String] length: [strPostParams length]];
                [req setHTTPMethod: @"POST"];
                [req setHTTPBody: requestData];
                [req setValue: @"application/x-www-form-urlencoded" forHTTPHeaderField: @"content-type"];
                 */
            }
            else
            {
            
                NSMutableURLRequest *req = [NSMutableURLRequest requestWithURL: [NSURL URLWithString: str]];
                
                NSURLResponse *response = nil;
                NSError *error = nil;
                NSData *data = [NSURLConnection sendSynchronousRequest: req returningResponse: &response error: &error];

                if ([_delegate respondsToSelector: @selector(requestResult:)])
                {
                    NSString *str = [[NSString alloc] initWithBytesNoCopy: (void*)[data bytes] length: [data length] encoding:NSASCIIStringEncoding freeWhenDone: NO];
                    [str autorelease];

                    NSDictionary *result = [NSDictionary dictionaryWithObjectsAndKeys:
                        str, @"result",
                        request, @"request",
                        data, @"raw",                                    
                        self, @"sender",
                        nil];
                    [_delegate performSelectorOnMainThread:@selector(requestResult:) withObject: result waitUntilDone:YES];
                }
                    
            }
        }
    }
    
}

- (void) cancelRequest
{
    if (self.asiFormDataRequest != nil)
        [self.asiFormDataRequest cancel];
}

- (void)requestFinished:(ASIHTTPRequest *)request
{
    // Use when fetching text data
    NSString *responseString = [request responseString];
    
    /*
     {
     "id": "912812948388"
     }
     */
    
    // Use when fetching binary data
    NSData *responseData = [request responseData];
    
    if ([_delegate respondsToSelector: @selector(requestResult:)])
    {
        NSDictionary *result = [NSDictionary dictionaryWithObjectsAndKeys:
                                responseString, @"result",
                                request, @"request",
                                responseData, @"raw",
                                self, @"sender",
                                nil];
        [_delegate performSelectorOnMainThread:@selector(requestResult:) withObject: result waitUntilDone:YES];
    }
    
//    NSLog(@"responseString: %@", responseString);
}

- (void)requestFailed:(ASIHTTPRequest *)request
{
    NSError *error = [request error];
    
    if ([_delegate respondsToSelector: @selector(requestResult:)])
    {
        NSDictionary *result = [NSDictionary dictionaryWithObjectsAndKeys:
                                error, @"error",
                                request, @"request",
                                self, @"sender",
                                nil];
        [_delegate performSelectorOnMainThread:@selector(requestResult:) withObject: result waitUntilDone:YES];
    }
    
    NSLog(@"error: %@", error);
}

- (void)setMaxValue:(double)newMax
{
    //NSLog(@"newMax: %f", newMax);
}

- (void)setDoubleValue:(double)newProgress
{
    //NSLog(@"newProgress: %f", newProgress);
    
    [_delegate uploadProgressChanged:newProgress * 100]; // convert from 0.93 to 93
    
}

- (void) sendRequest: (NSString*) request params: (NSDictionary*) params usePostRequest: (BOOL) postRequest
{
    NSMutableDictionary *allParams = [NSMutableDictionary dictionaryWithObject: request forKey: @"request"];
    if (params != nil)
        [allParams setObject: params forKey: @"params"];
        
    [allParams setObject: [NSNumber numberWithBool: postRequest] forKey: @"postRequest"];

	[NSThread detachNewThreadSelector: @selector(sendFacebookRequest:) toTarget: self withObject: allParams];    
}

- (void) sendRequest: (NSString*) request
{
    [self sendRequest: request params: nil usePostRequest: NO];
}

- (void) sendFacebookFQLRequest: (NSString*) query
{
    @autoreleasepool {

        if (_authToken)
        {
            NSString *str = [NSString stringWithFormat: kFBGraphApiFqlURL, [query stringByAddingPercentEscapesUsingEncoding:NSUTF8StringEncoding], _authToken.authenticationToken];

            NSMutableURLRequest *req = [NSMutableURLRequest requestWithURL: [NSURL URLWithString: str]];

            NSURLResponse *response = nil;
            NSError *error = nil;
            NSData *data = [NSURLConnection sendSynchronousRequest: req returningResponse: &response error: &error];

            if ([_delegate respondsToSelector: @selector(requestResult:)])
            {
                NSString *str = [[NSString alloc] initWithBytesNoCopy: (void*)[data bytes] length: [data length] encoding:NSASCIIStringEncoding freeWhenDone: NO];
                [str autorelease];
                NSDictionary *result = [NSDictionary dictionaryWithObjectsAndKeys:
                                        str, @"result",
                                        query, @"request",
                                        data, @"raw",
                                        self, @"sender",
                                        nil];
                [_delegate performSelectorOnMainThread:@selector(requestResult:) withObject: result waitUntilDone:YES];
            }
        }
    }
}

- (void) sendFQLRequest: (NSString*) query
{
    [NSThread detachNewThreadSelector: @selector(sendFacebookFQLRequest:) toTarget: self withObject: query];
}

#pragma mark Notifications

- (void) webViewWillShowUI
{
    if ([_delegate respondsToSelector: @selector(willShowUINotification:)])
        [_delegate performSelectorOnMainThread: @selector(willShowUINotification:) withObject: self waitUntilDone: YES];
}

- (void) didDismissUI
{
    if ([_delegate respondsToSelector: @selector(didDismissUI:)])
        [_delegate performSelectorOnMainThread: @selector(didDismissUI:) withObject: self waitUntilDone: YES];
}

@end

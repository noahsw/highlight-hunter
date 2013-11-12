//
//  TrackingRequest.m
//  OdessaMacGUIApp
//
//  Created by Noah Spitzer-Williams on 11/24/11.
//  Copyright (c) 2011 Authentically Digital LLC. All rights reserved.
//

#import "TrackingRequest.h"
#import "NSString+URLEncoding.h"

#include <stdlib.h> // for random number

#import "DDLog.h"
static const int ddLogLevel = LOG_LEVEL_VERBOSE | LOG_LEVEL_INFO | LOG_LEVEL_ERROR | LOG_LEVEL_WARN;

@implementation TrackingRequest

@synthesize pageTitle;
@synthesize pageDomain;
@synthesize pageUrl;

@synthesize requestedByIpAddress;

@synthesize referralSource;
@synthesize medium;
@synthesize campaign;

@synthesize trackingEvent;

@synthesize analyticsAccountCode;

@synthesize visitCount;

- (id)init
{
    
    self = [super init];
    if (self) {
        referralSource = @"(direct)";
        medium = @"(none)";
        campaign = @"(direct)";
        visitCount = @"2"; // not sure why this is 2 but it was like this in the original code
        
        pageTitle = @"";
        pageDomain = @"";
        pageUrl = @"/";
        
#if DEBUG
        analyticsAccountCode = @"UA-28160719-2";
#else
        analyticsAccountCode = @"UA-28160719-3";
#endif
    }
    
    return self;
}


- (NSString*) trackingGifURL
{
    
    
    // REQUEST URL FORMAT:
    // http://www.google-analytics.com/__utm.gif?utmwv=4.6.5&utmn=488134812&utmhn=facebook.com&utmcs=UTF-8&utmsr=1024x576&utmsc=24-bit&utmul=en-gb&utmje=0&utmfl=10.0%20r42&utmdt=Facebook%20Contact%20Us&utmhid=700048481&utmr=-&utmp=%2Fwebdigi%2Fcontact&utmac=UA-3659733-5&utmcc=__utma%3D155417661.474914265.1263033522.1265456497.1265464692.6%3B%2B__utmz%3D155417661.1263033522.1.1.utmcsr%3D(direct)%7Cutmccn%3D(direct)%7Cutmcmd%3D(none)%3B
    
    NSMutableDictionary* paramList = [NSMutableDictionary dictionary];
    paramList[@"utmwv"] = @"5.2.2d"; // analytics version
    paramList[@"utmn"] = [NSString stringWithFormat:@"%d", arc4random() % 1000000000]; // random request number
    
    paramList[@"utmhn"] = pageDomain; // domain name
    paramList[@"utmcs"] = @"UTF-8"; // document encoding
    paramList[@"utmsr"] = @"-"; // screen resolution
    paramList[@"utmsc"] = @"-"; // screen resolution
    paramList[@"utmul"] = @"-"; // user language
    paramList[@"utmje"] = @"0"; // java enabled or not
    paramList[@"utmfl"] = @"-"; // flash version
    paramList[@"utmni"] = @"1"; // ignore this event from bounce rate calculations
    paramList[@"utmdt"] = [JFUrlUtil encodeUrl:pageTitle]; // [pageTitle urlEncodeUsingEncoding:NSUTF8StringEncoding]; // page title
    paramList[@"utmhid"] = [NSString stringWithFormat:@"%d", arc4random() % 1000000000]; // page title
    paramList[@"utmr"] = @"-"; // referrer URL
    paramList[@"utmp"] = pageUrl; // document page URL (relative to root)
    paramList[@"utmac"] = analyticsAccountCode; // GA account code
    paramList[@"utmcc"] = [self utmcCookieString]; // cookie string encoded
    
    /*
    &utmhid=512635904&utmcs=UTF-8&utmcc=__utma=(null).-1420992512.1322432025.1322432025.1322432025.2;+__utmcz=(null).1322432025.1.1.utmcsr=(direct)%7Cutmccn=(direct)%7Cutmcmd=(none);&utmwv=4.7.2&utmje=0&utmdt=&utmfl=-&utmul=-&utmp=&utme=5(category*action*label)((null))
     
     List<KeyValuePair<string, string>> paramList = new List<KeyValuePair<string, string>>
     {
     new KeyValuePair<string,string>("utmwv", "4.7.2"),                                                                              // Analytics version
     new KeyValuePair<string,string>("utmn", randomNumber.Next(1000000000).ToString()),      // Random request number
     new KeyValuePair<string,string>("utmhn", PageDomain ?? string.Empty),                                                           // Your domain name
     new KeyValuePair<string,string>("utmcs", "UTF-8"),                                                                      // Document encoding
     new KeyValuePair<string,string>("utmsr", "-"),                                                                          // Screen Resolution
     new KeyValuePair<string,string>("utmsc", "-"),                                                                          // Screen Resolution
     new KeyValuePair<string,string>("utmul", "-"),                                                                          // user language
     new KeyValuePair<string,string>("utmje", "0"),                                                                          // java enabled or not
     new KeyValuePair<string,string>("utmfl", "-"),                                                                          // user flash version
     new KeyValuePair<string,string>("utmdt", PageTitle != null ? Uri.EscapeDataString(PageTitle): string.Empty),                    // page title
     new KeyValuePair<string,string>("utmhid", randomNumber.Next(1000000000).ToString()),                                                                            // page title
     new KeyValuePair<string,string>("utmr", "-"),                                                                                   // referrer URL
     new KeyValuePair<string,string>("utmp", PageUrl),                                                                       // document page URL (relative to root)
     new KeyValuePair<string,string>("utmac", AnalyticsAccountCode ?? string.Empty),                                         // Your GA account code
     new KeyValuePair<string,string>("utmcc", UtmcCookieString)                                      // cookie string (encoded)
     };
     */
     
     //check if our tracking event is null and if not add to the params
     if (trackingEvent != nil)
     {
         //DDLogInfo(@"trackingEvent.category = %@", trackingEvent.category);
         //DDLogInfo(@"trackingEvent.action = %@", trackingEvent.action);
         //DDLogInfo(@"trackingEvent.label = %@", trackingEvent.label);
         //if ([trackingEvent.val isNotEqualTo:nil])
             //DDLogInfo(@"trackingEvent.val = %@", trackingEvent.val);
             
         NSString* eventString;
         if ([trackingEvent.val isNotEqualTo:nil])
         {
             int valInt = (int)(trackingEvent.val.intValue);
             NSNumber* valNumber = @(valInt);
             
             eventString = [NSString stringWithFormat:@"5(%@*%@*%@)(%@)",
                                  trackingEvent.category,
                                  trackingEvent.action,
                                  trackingEvent.label,
                                  valNumber];
         }
         else
             eventString = [NSString stringWithFormat:@"5(%@*%@*%@)()",
                            trackingEvent.category,
                            trackingEvent.action,
                            trackingEvent.label];
         
         //taken from http://code.google.com/apis/analytics/docs/tracking/gaTrackingTroubleshooting.html
         /*
             string eventString = String.Format("5({0}*{1}*{2})({3})",
         TrackingEvent.Category,
         TrackingEvent.Action,
         TrackingEvent.Label,
         TrackingEvent.Value
          );
            
         
         paramList.Add(new KeyValuePair<string, string>("utme", Uri.EscapeDataString(eventString)));
         
         paramList.Add(new KeyValuePair<string, string>("gaq","1"));
         
         paramList.Add(new KeyValuePair<string, string>("utmt", "event"));                                       // type of request (page view or event etc)
              */
      
         paramList[@"utme"] = [JFUrlUtil encodeUrl:eventString]; // [eventString urlEncodeUsingEncoding:NSUTF8StringEncoding];
         paramList[@"gaq"] = @"1";
         paramList[@"utmt"] = @"event"; // type of request (page view or event, etc)
         
     }
     
    /* we don't support transactions yet
     //check if the transaction object is null and if not add the transaction params
     if (TrackingTransaction!=null)
     {
     TrackingTransaction.Validate();
     
     
     //taken from http://code.google.com/apis/analytics/docs/tracking/gaTrackingTroubleshooting.html
     
     //add product name
     paramList.Add(new KeyValuePair<string, string>("utmipn", Uri.EscapeDataString(TrackingTransaction.ProductName)));
     
     //add product code/SKU
     paramList.Add(new KeyValuePair<string, string>("utmipc", Uri.EscapeDataString(TrackingTransaction.ProductSku)));
     
     //add unit variation (ie Red)
     paramList.Add(new KeyValuePair<string, string>("utmiva", Uri.EscapeDataString(TrackingTransaction.ProductVariant)));
     
     //add unit price
     paramList.Add(new KeyValuePair<string, string>("utmipr", TrackingTransaction.UnitPrice.ToString("#.##")));
     
     //add quantity
     paramList.Add(new KeyValuePair<string, string>("utmiqt", TrackingTransaction.Quantity.ToString()));
     
     //add billing city
     paramList.Add(new KeyValuePair<string, string>("utmtci", Uri.EscapeDataString(TrackingTransaction.City)));
     
     //add billing country
     paramList.Add(new KeyValuePair<string, string>("utmtco", Uri.EscapeDataString(TrackingTransaction.Country)));
     
     // add order ID
     paramList.Add(new KeyValuePair<string, string>("utmtid", TrackingTransaction.OrderID));
     
     //add shipping cost
     paramList.Add(new KeyValuePair<string, string>("utmtsp", TrackingTransaction.ShippingCost.ToString("#.##")));
     
     //add transaction total
     paramList.Add(new KeyValuePair<string, string>("utmtto", TrackingTransaction.TotalCost.ToString("#.##")));
     
     //add tax total
     paramList.Add(new KeyValuePair<string, string>("utmttx", TrackingTransaction.TaxCost.ToString("#.##")));
     
     paramList.Add(new KeyValuePair<string, string>("utmt", "transaction"));                                 // type of request (page view or event etc)
     
     }
     */
     
     //add ip address if we have one
     if ([requestedByIpAddress length] > 0)
     {
         paramList[@"utmip"] = requestedByIpAddress;
         //paramList.Add(new KeyValuePair<string, string>("utmip", RequestedByIpAddress));
     }
     
     
    //get final param string
    NSMutableString* gaParams = [NSMutableString new];
    //for (long i = [paramList count] - 1; i >= 0; i--)
    for (NSString* key in paramList)
    {
        NSString* value = paramList[key];
        [gaParams appendFormat:@"%@=%@&", key, value];
    }
        
    NSString* finalURL = [NSString stringWithFormat:@"http://www.google-analytics.com/__utm.gif?%@", gaParams];
    
    //DDLogVerbose(@"Google Analytics URL: %@", finalURL);
    
    return finalURL;
    
    /*
     StringBuilder GaParams = new StringBuilder();
     foreach (KeyValuePair<string, string> pair in paramList)
     {
     GaParams.Append(String.Format("{0}={1}&", pair.Key, pair.Value));
     }
     string paramsFinal = GaParams.ToString();
     paramsFinal = paramsFinal.Substring(0, paramsFinal.Length - 1);
     
     //return the google gif plus all our params
     return String.Format("http://www.google-analytics.com/__utm.gif?{0}",
     paramsFinal);
     }
     */
    
}


- (NSString*) utmcCookieString
{
 
    NSString* cachedTimeStamp = [self timeStampCurrent];
    NSString *utma = [NSString stringWithFormat:@"%d.%@.%@.%@.%@.%@",
                      [self domainHash],
                      [NSString stringWithFormat:@"%d", arc4random() % 1000000000],
                      cachedTimeStamp,
                      cachedTimeStamp,
                      cachedTimeStamp,
                      visitCount];
    
    /*
     string utma = String.Format("{0}.{1}.{2}.{3}.{4}.{5}",
     DomainHash,
     int.Parse(randomNumber.Next(1000000000).ToString()),
     timeStampCurrent,
     timeStampCurrent,
     timeStampCurrent,
     visitCount);
     */
    
    
    //referral informaiton
    NSString *utmz = [NSString stringWithFormat:@"%d.%@.%@.%@.utmcsr=%@|utmccn=%@|utmcmd=%@",
                      [self domainHash],
                      cachedTimeStamp,
                      @"1",
                      @"1",
                      referralSource,
                      campaign,
                      medium];
    
    /*
     string utmz = String.Format("{0}.{1}.{2}.{3}.utmcsr={4}|utmccn={5}|utmcmd={6}",
     DomainHash,
     timeStampCurrent,
     "1",
     "1",
     ReferralSource,
     Campaign,
     Medium);
    */
    
    NSString *utmcc = [NSString stringWithFormat:@"__utma=%@;+__utmz=%@;",
                       utma,
                       utmz];
    utmcc = [JFUrlUtil encodeUrl:utmcc]; // [utmcc urlEncodeUsingEncoding:NSUTF8StringEncoding];
    
    
    /*
    //return String.Format("__utma%3D{0}.{1}.{2}.{3}.{4}.{5}",)))
     string utmcc = Uri.EscapeDataString(String.Format("__utma={0};+__utmz={1};",
     utma,
     utmz
     ));
     */
    
    return (utmcc);
     
}


- (int) domainHash
{
    if ([pageDomain length] > 0)
    {
        // converted from the google domain hash code listed here:
        // http://www.google.com/support/forum/p/Google+Analytics/thread?tid=626b0e277aaedc3c&hl=en
        int a = 1;
        int c = 0;
        long h;
        char chrCharacter;
        int intCharacter;
        
        a = 0;
        for (h = [pageDomain length] - 1; h >= 0; h--)
        {
            
            chrCharacter = (char)([pageDomain substringWithRange:NSMakeRange(h, 1)]);
            //chrCharacter = char.Parse( PageDomain.Substring(h, 1));
            intCharacter = (int) chrCharacter;
            a = (a << 6 & 268435455) + intCharacter + (intCharacter << 14);
            c = a & 266338304;
            a = c != 0 ? a ^ c >> 21 : a;
        }
        
        return a;
    }
                                  
    return 0;
}



- (NSString*) timeStampCurrent
{
    /*
  //string timeStampCurrent = GoogleHashHelper.ConvertToUnixTimestamp(DateTime.Now).ToString();
    //create Timespan by subtracting the value provided from
    //the Unix Epoch
    TimeSpan span = (value - new DateTime(1970, 1, 1, 0, 0, 0, 0).ToLocalTime());
    
    //return the total seconds (which is a UNIX timestamp)
    return (int)span.TotalSeconds;
    */
    
    
    NSString *timestamp = [NSString stringWithFormat:@"%0.0f", [[NSDate date] timeIntervalSince1970]];
    
    return timestamp;
    
}

                                  
                                  
@end
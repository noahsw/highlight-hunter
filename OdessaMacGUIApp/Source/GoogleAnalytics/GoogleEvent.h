//
//  GoogleEvent.h
//  OdessaMacGUIApp
//
//  Created by Noah Spitzer-Williams on 11/26/11.
//  Copyright (c) 2011 Authentically Digital LLC. All rights reserved.
//



@interface GoogleEvent : NSObject
{
    NSString *domainName;
    NSString *category;
    NSString *action;
    NSString *label;
    NSNumber *val;
    
    /*
    public string DomainName { get; private set; }
    
    public string Category { get; private set; }
    
    public string Action { get; private set; }
    
    public string Label { get; private set; }
    
    public int? Value { get; private set; }
    */
    
}

@property (nonatomic, copy) NSString *domainName;
@property (nonatomic, copy) NSString *category;
@property (nonatomic, copy) NSString *action;
@property (nonatomic, copy) NSString *label;
@property (nonatomic, copy) NSNumber *val;

- (id)initWithParams: (NSString*)_domainName category:(NSString*)_category action:(NSString*)_action label:(NSString*)_label value:(NSNumber*)_val;


@end

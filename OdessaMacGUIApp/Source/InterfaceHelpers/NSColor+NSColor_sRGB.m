//
//  NSColor+NSColor_sRGB.m
//
//  Created by Alexander Danmayer on 10/17/12.
//  Copyright (c) 2012 Alexander Danmayer. All rights reserved.
//

#import <objc/runtime.h>
#import "NSColor+NSColor_sRGB.h"

id _colorSRGB (id self, SEL _cmd, CGFloat red, CGFloat green, CGFloat blue, CGFloat alpha)
{
    CGFloat comps[4] = {red, green, blue, alpha};
    return [NSColor colorWithColorSpace:[NSColorSpace sRGBColorSpace] components:comps count:4];
};

@implementation NSColor (NSColor_sRGB)

+ (BOOL)resolveClassMethod:(SEL)sel
{
    if (sel == @selector(colorWithSRGBRed:green:blue:alpha:))
        return YES;
    return [super resolveInstanceMethod:sel];
}

+ (void) load {
    // check if SRGB method is available ...
    if ([NSColor resolveClassMethod:@selector(colorWithSRGBRed:green:blue:alpha:)])
    {
        // get the signature of a similar method
        Method m = class_getClassMethod([NSColor class], @selector(colorWithCalibratedRed:green:blue:alpha:));
        const char *code = method_getTypeEncoding(m);
        
        // retrieve meta class for class methods
        Class selfMetaClass = objc_getMetaClass([[self className] UTF8String]);
        class_addMethod(selfMetaClass, @selector(colorWithSRGBRed:green:blue:alpha:), (IMP) _colorSRGB, code);
    }
}

@end
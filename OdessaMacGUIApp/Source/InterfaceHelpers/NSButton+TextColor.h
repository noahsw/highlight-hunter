#import <Cocoa/Cocoa.h>
@interface NSButton (TextColor)

- (NSColor *)textColor;
- (void)setTextColor:(NSColor *)textColor;

- (NSNumber*)textUnderline;
- (void)setTextUnderline:(int)style;

@end


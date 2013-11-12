#import "NSButton+TextColor.h"
@implementation NSButton (TextColor)

- (NSColor *)textColor
{
    NSAttributedString *attrTitle = [self attributedTitle];
    long len = [attrTitle length];
    NSRange range = NSMakeRange(0, MIN(len, 1)); // take color from first char
    NSDictionary *attrs = [attrTitle fontAttributesInRange:range];
    NSColor *textColor = [NSColor controlTextColor];
    if (attrs) {
        textColor = attrs[NSForegroundColorAttributeName];
    }
    return textColor;
}

- (void)setTextColor:(NSColor *)textColor
{
    NSMutableAttributedString *attrTitle = [[NSMutableAttributedString alloc] 
                                            initWithAttributedString:[self attributedTitle]];
    long len = [attrTitle length];
    NSRange range = NSMakeRange(0, len);
    [attrTitle addAttribute:NSForegroundColorAttributeName 
                      value:textColor 
                      range:range];
    [attrTitle fixAttributesInRange:range];
    [self setAttributedTitle:attrTitle];
}


- (NSNumber*)textUnderline
{
    NSAttributedString *attrTitle = [self attributedTitle];
    long len = [attrTitle length];
    NSRange range = NSMakeRange(0, MIN(len, 1)); // take color from first char
    NSDictionary *attrs = [attrTitle fontAttributesInRange:range];
    NSNumber* style = 0;
    if (attrs) {
        style = attrs[NSUnderlineStyleAttributeName];
    }
    return style;
}

- (void)setTextUnderline:(int)style
{
    NSMutableAttributedString *attrTitle = [[NSMutableAttributedString alloc] 
                                            initWithAttributedString:[self attributedTitle]];
    long len = [attrTitle length];
    NSRange range = NSMakeRange(0, len);
    [attrTitle addAttribute:NSUnderlineStyleAttributeName
                      value:@(style) 
                      range:range];
    [attrTitle fixAttributesInRange:range];
    [self setAttributedTitle:attrTitle];
}




@end
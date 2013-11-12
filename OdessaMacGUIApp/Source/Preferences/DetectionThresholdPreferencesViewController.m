
#import "DetectionThresholdPreferencesViewController.h"

@implementation DetectionThresholdPreferencesViewController
@synthesize mySlider;
@synthesize mySliderLabel;

- (id)init
{
    return [super initWithNibName:@"DetectionThresholdPreferencesView" bundle:nil];
}

#pragma mark -
#pragma mark MASPreferencesViewController

- (NSString *)identifier
{
    return @"DetectionThresholdPreferences";
}

- (NSImage *)toolbarItemImage
{
    return [NSImage imageNamed:NSImageNameBookmarksTemplate];
}

- (NSString *)toolbarItemLabel
{
    return NSLocalizedString(@"Scan Sensitivity", @"How strict Highlight Hunter should be when looking for marks");
}

- (void)viewWillAppear
{
    NSUserDefaults *standardUserDefaults = [NSUserDefaults standardUserDefaults];
    if (standardUserDefaults) 
    {
        [mySlider setIntegerValue:[standardUserDefaults integerForKey:@"DetectionThreshold"]];
        [self sliderValueChanged:mySlider];
    }
}

- (IBAction)sliderValueChanged:(id)sender {
    NSUserDefaults *standardUserDefaults = [NSUserDefaults standardUserDefaults];
    if (standardUserDefaults) 
    {
        [standardUserDefaults setInteger:[mySlider intValue] forKey:@"DetectionThreshold"];
        [standardUserDefaults synchronize];
    }
}
@end

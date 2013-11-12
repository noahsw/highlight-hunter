
#import "HighlightDurationPreferencesViewController.h"

@implementation HighlightDurationPreferencesViewController
@synthesize mySlider;
@synthesize mySliderLabel;

- (id)init
{
    return [super initWithNibName:@"HighlightDurationPreferencesView" bundle:nil];
}


#pragma mark -
#pragma mark MASPreferencesViewController

- (NSString *)identifier
{
    return @"HighlightDurationPreferences";
}

- (NSImage *)toolbarItemImage
{
    return [NSImage imageNamed:NSImageNameAdvanced];
}

- (NSString *)toolbarItemLabel
{
    return NSLocalizedString(@"Default Highlight Duration", @"How long the highlights should be");
}

- (void)viewWillAppear
{

    NSUserDefaults *standardUserDefaults = [NSUserDefaults standardUserDefaults];
    if (standardUserDefaults) 
    {
        [mySlider setIntegerValue:[standardUserDefaults integerForKey:@"CaptureDurationInSeconds"]];
        [self sliderValueChanged:mySlider];
        
    }
}

- (void)viewDidDisappear
{

}

- (IBAction)sliderValueChanged:(id)sender {

    int minutes = [sender intValue] / 60;
    int seconds = [sender intValue] % 60;
    
    NSMutableString* ret = [NSMutableString stringWithString:@""];
    if (minutes == 1)
    {
        if (seconds > 0)
        {
            [ret appendFormat:@"1 minute %d seconds", seconds];
        }
        else
        {
            [ret appendString:@"1 minute"];
        }

    }
    else if (minutes > 1)
    {
        if (seconds > 0)
        {
            [ret appendFormat:@"%d", minutes];
            [ret appendString:@" minutes and "];
            [ret appendFormat:@"%d", seconds];
            [ret appendString:@" seconds"];            
        }
        else
        {
            [ret appendFormat:@"%d", minutes];
            [ret appendString:@" minutes"];
        }
    }
    else
    {
        [ret appendFormat:@"%d", seconds];
        [ret appendString:@" seconds"];
    }
    
    [mySliderLabel setStringValue: ret];
    
    NSUserDefaults *standardUserDefaults = [NSUserDefaults standardUserDefaults];
    if (standardUserDefaults) 
    {
        [standardUserDefaults setInteger:[mySlider intValue] forKey:@"CaptureDurationInSeconds"];
        [standardUserDefaults synchronize];
    }
}





@end

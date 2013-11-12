//
// This is a sample General preference pane
//

#import "MASPreferencesViewController.h"

@interface HighlightDurationPreferencesViewController : NSViewController <MASPreferencesViewController> {
    NSSlider *__unsafe_unretained mySlider;
    NSTextField *__unsafe_unretained mySliderLabel;
}


@property (unsafe_unretained) IBOutlet NSSlider *mySlider;
@property (unsafe_unretained) IBOutlet NSTextField *mySliderLabel;

- (IBAction)sliderValueChanged:(id)sender;

@end

//
// This is a sample General preference pane
//

#import "MASPreferencesViewController.h"
#import "BrowserHelper.h"
#import "NSAlert+SynchronousSheet.h"

@interface AdvancedPreferencesViewController : NSViewController <MASPreferencesViewController> {
}

@property (unsafe_unretained) IBOutlet NSButton *ignoreEarlyHighlightsCheckbox;

@property (strong) IBOutlet NSButton *proResCheckbox;

@property (strong) IBOutlet NSButton *proGlyphButton;

- (IBAction)proResCheckChanged:(id)sender;

- (IBAction)ignoreEarlyHighlightsStateChanged:(id)sender;

- (IBAction)openProInfo:(id)sender;
@end

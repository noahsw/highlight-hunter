
#import "AdvancedPreferencesViewController.h"

@implementation AdvancedPreferencesViewController

- (id)init
{
    return [super initWithNibName:@"AdvancedPreferencesView" bundle:nil];
}


#pragma mark -
#pragma mark MASPreferencesViewController

- (NSString *)identifier
{
    return @"AdvancedPreferences";
}

- (NSImage *)toolbarItemImage
{
    return [NSImage imageNamed:NSImageNameAdvanced];
}

- (NSString *)toolbarItemLabel
{
    return NSLocalizedString(@"Advanced", @"Advanced preferences");
}

- (void)viewWillAppear
{
    
#if APPSTORE_PREMIUM
    //[self.proResCheckbox setHidden:YES]; // Apple doesn't allow ProRes output without using their codecs. Taking this offline with them.
    [self.proGlyphButton setHidden:YES]; // Apple doesn't allow this to be visible in Pro version
#elif APPSTORE_FREE
    [self.proResCheckbox setHidden:YES];
    [self.proGlyphButton setHidden:YES];
#endif
    
    
    NSUserDefaults *standardUserDefaults = [NSUserDefaults standardUserDefaults];
    if (standardUserDefaults) 
    {
        [self.ignoreEarlyHighlightsCheckbox setState:[standardUserDefaults boolForKey:@"IgnoreEarlyHighlights"]];
        
        
        NSString* outputFormat = [standardUserDefaults stringForKey:@"SaveOutputFormat"];
        if ([outputFormat isEqualToString:@"ProRes"])
            self.proResCheckbox.state = 1;
        else
            self.proResCheckbox.state = 0;
    }
}

- (void)viewDidDisappear
{

}


- (void)ignoreEarlyHighlightsStateChanged:(id)sender
{    
    // save setting
    NSUserDefaults *standardUserDefaults = [NSUserDefaults standardUserDefaults];
    if (standardUserDefaults) 
    {
        [standardUserDefaults setBool:[sender state] forKey:@"IgnoreEarlyHighlights"];
        [standardUserDefaults synchronize];
    }
}

- (IBAction)openProInfo:(id)sender {
    [BrowserHelper launchBrowser:@"appstore-redirect.php" term:@"settings" addDomain:YES];
}

- (IBAction)proResCheckChanged:(id)sender {
    NSUserDefaults *standardUserDefaults = [NSUserDefaults standardUserDefaults];
    if (standardUserDefaults)
    {
        if (self.proResCheckbox.state == 1)
        {
            if (!NSClassFromString(@"AVAsset") || !NSClassFromString(@"AVAssetExportSession"))
            {
                NSString* alertMsg = @"Unfortunately ProRes is only supported on OSX 10.7 and above.";
                NSAlert *alert = [[NSAlert alloc] init];
                [alert addButtonWithTitle:@"OK"];
                [alert setMessageText:@"ProRes not supported"];
                [alert setInformativeText:alertMsg];
                [alert setAlertStyle:NSInformationalAlertStyle];
                [alert runModalSheetForWindow:[[self view] window]];
                self.proResCheckbox.state = 0;
            }
            else // ProRes supported
                [standardUserDefaults setValue:@"ProRes" forKey:@"SaveOutputFormat"];
        }
        else
            [standardUserDefaults setValue:@"Original" forKey:@"SaveOutputFormat"];
        
        [standardUserDefaults synchronize];
    }
}

@end

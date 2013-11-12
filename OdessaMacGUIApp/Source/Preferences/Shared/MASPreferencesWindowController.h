//
// You create an application Preferences window using code like this:
//     _preferencesWindowController = [[MASPreferencesWindowController alloc] initWithViewControllers:controllers
//                                                                                              title:title]
//
// To open the Preferences window:
//     [_preferencesWindowController showWindow:sender]
//

#import "MASPreferencesViewController.h"

extern NSString *const kMASPreferencesWindowControllerDidChangeViewNotification;

#if MAC_OS_X_VERSION_MAX_ALLOWED > MAC_OS_X_VERSION_10_5
@interface MASPreferencesWindowController : NSWindowController <NSToolbarDelegate, NSWindowDelegate>
#else
@interface MASPreferencesWindowController : NSWindowController
#endif


@property (strong) NSArray *viewControllers;
@property (readonly) NSUInteger indexOfSelectedController;

@property (strong) NSString *title;

- (id)initWithViewControllers:(NSArray *)viewControllers;
- (id)initWithViewControllers:(NSArray *)viewControllers title:(NSString *)title;

- (void)selectControllerAtIndex:(NSUInteger)controllerIndex;

- (IBAction)goNextTab:(id)sender;
- (IBAction)goPreviousTab:(id)sender;

@end


@interface MASPreferencesWindowController ()

@property (strong) NSMutableDictionary *minimumViewRects;

@property (nonatomic, strong) NSViewController <MASPreferencesViewController> *selectedViewController;

@property (unsafe_unretained, readonly) NSArray *toolbarItemIdentifiers;

- (NSViewController <MASPreferencesViewController> *)viewControllerForIdentifier:(NSString *)identifier;

@end
using Microsoft.Playwright;
using Playwright.E2ETests.Browser;
using Playwright.E2ETests.Context;
using Playwright.E2ETests.Utils;
using Reqnroll;
using Reqnroll.BoDi;

namespace Playwright.E2ETests.Hooks.Setup;

public class SetupHelper
{
    private readonly IObjectContainer _objectContainer;
    private readonly IPlaywright _playwright;
    private readonly ScenarioContext _scenarioContext;

    public SetupHelper(IObjectContainer objectContainer, ScenarioContext scenarioContext, IPlaywright playwright)
    {
        _objectContainer = objectContainer;
        _scenarioContext = scenarioContext;
        _playwright = playwright;
    }

    public async Task SetupAsync()
    {
        Console.WriteLine($"SetupHelper: Processing scenario - {_scenarioContext.ScenarioInfo.Title}");

        // Initialize test context first (needed for accessibility tracking)
        InitializeTestContext();

        // Initialize browser for UI tests
        Console.WriteLine("SetupHelper: Initializing browser for UI test");
        await InitializeBrowserAsync(_playwright);
        Console.WriteLine("SetupHelper: Setup complete");
    }

    private async Task InitializeBrowserAsync(IPlaywright playwright)
    {
        var browserManager = new BrowserManager(playwright);
        var browser = await browserManager.GetBrowserAsync();
        var context = await browserManager.GetContextAsync(browser);
        await context.ClearCookiesAsync();
        var page = await context.NewPageAsync();
        await InitializeTracingAsync(context);
        _objectContainer.RegisterInstanceAs(browser);
        _objectContainer.RegisterInstanceAs(context);
        _objectContainer.RegisterInstanceAs(page);

        // Initialize automatic accessibility tracking
        InitializeAccessibilityListener(page);
    }

    private async Task InitializeTracingAsync(IBrowserContext browserContext)
    {
        await browserContext.Tracing.StartAsync(new TracingStartOptions
        {
            Title = _scenarioContext.ScenarioInfo.Title, Screenshots = true, Snapshots = true, Sources = true
        });
    }

    private void InitializeTestContext()
    {
        var testContext = new CustomTestContext();
        var accessibilityHelper = new AccessibilityHelper(_scenarioContext.ScenarioInfo.Title);
        testContext.Put(CustomTestContext.Keys.AccessibilityHelper, accessibilityHelper);
        testContext.Put(CustomTestContext.Keys.AccessibilityFailures, new List<string>());
        testContext.Put(CustomTestContext.Keys.ScannedPageUrls, new HashSet<string>());
        _objectContainer.RegisterInstanceAs(testContext);
    }

    private void InitializeAccessibilityListener(IPage page)
    {
        var testContext = _objectContainer.Resolve<CustomTestContext>();
        var accessibilityHelper = (AccessibilityHelper)testContext.Get(CustomTestContext.Keys.AccessibilityHelper);

        // Attach listener to initial page
        var listener = new AccessibilityPageEventListener(page, accessibilityHelper, testContext);
        listener.Attach();

        // Register the listener so it can be detached later if needed
        _objectContainer.RegisterInstanceAs(listener);

        // Listen for new pages/tabs created in the browser context and attach listeners to them
        page.Context.Page += async (_, newPage) =>
        {
            Console.WriteLine($"SetupHelper: New page/tab detected - {newPage.Url}");
            var newPageListener = new AccessibilityPageEventListener(newPage, accessibilityHelper, testContext);
            newPageListener.Attach();
            Console.WriteLine($"SetupHelper: Accessibility listener attached to new page/tab - {newPage.Url}");
        };

        Console.WriteLine("SetupHelper: Automatic accessibility tracking initialized for all pages");
    }
}

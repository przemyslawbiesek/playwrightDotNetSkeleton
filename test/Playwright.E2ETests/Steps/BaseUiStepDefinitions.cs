using Microsoft.Playwright;
using Playwright.E2ETests.Context;
using Playwright.E2ETests.Utils;

namespace Playwright.E2ETests.Steps;

public class BaseUiStepDefinitions : BaseStepDefinitions
{
    protected readonly IPage Page;

    public BaseUiStepDefinitions(CustomTestContext customTestContext, IPage page) : base(customTestContext)
    {
        AccessibilityHelper = (AccessibilityHelper)customTestContext.Get(CustomTestContext.Keys.AccessibilityHelper);
        Page = page;
    }

    protected AccessibilityHelper AccessibilityHelper { get; }

    /// <summary>
    /// Runs accessibility check and tracks failures without immediately failing the test
    /// Failures will be reported at the end of scenario execution
    /// </summary>
    protected async Task TrackAccessibilityAsync(IPage page)
    {
        var pageUrl = page.Url;

        // Check if this page URL has already been scanned
        HashSet<string> scannedUrls;
        try
        {
            scannedUrls = (HashSet<string>)CustomTestContext.Get(CustomTestContext.Keys.ScannedPageUrls);
        }
        catch
        {
            scannedUrls = new HashSet<string>();
            CustomTestContext.Put(CustomTestContext.Keys.ScannedPageUrls, scannedUrls);
        }

        if (scannedUrls.Contains(pageUrl))
        {
            Console.WriteLine($"BaseUiStepDefinitions: Page already scanned, skipping - {pageUrl}");
            return;
        }

        // Mark this URL as scanned
        scannedUrls.Add(pageUrl);
        Console.WriteLine($"BaseUiStepDefinitions: Scanning page for the first time - {pageUrl}");

        var result = await AccessibilityHelper.RunAndTrack(page);
        Validators.AccessibilityValidator.TrackAccessibilityCheck(result.Passed, result.PageUrl);

        if (!result.Passed)
        {
            // Get or create the list of accessibility failures
            List<string> failures;
            try
            {
                failures = (List<string>)CustomTestContext.Get(CustomTestContext.Keys.AccessibilityFailures);
            }
            catch
            {
                failures = new List<string>();
                CustomTestContext.Put(CustomTestContext.Keys.AccessibilityFailures, failures);
            }

            failures.Add($"{result.PageUrl} (checked at {result.CheckedAt:HH:mm:ss})");
        }
    }
}

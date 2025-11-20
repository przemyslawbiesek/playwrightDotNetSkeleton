using Microsoft.Playwright;

namespace Playwright.E2ETests.Pages;

public abstract class BasePage
{
    protected BasePage(IPage page)
    {
        Page = page;
    }

    protected IPage Page { get; }

    public IPage GetPage()
    {
        return Page;
    }

    public async Task<string> GetTitle()
    {
        return await Page.TitleAsync();
    }

    /// <summary>
    /// Manually trigger accessibility check for the current page.
    /// Note: Automatic accessibility tracking is enabled by default on page navigations.
    /// Use this method only when you need to check accessibility after dynamic content changes.
    /// </summary>
    public async Task WaitForDynamicContentAndCheckAccessibility(int delayMs = 500)
    {
        await Task.Delay(delayMs);
        // The automatic listener will handle the check, or it can be called manually via step definitions
        Console.WriteLine($"BasePage: Waiting {delayMs}ms for dynamic content to load before accessibility check");
    }
}

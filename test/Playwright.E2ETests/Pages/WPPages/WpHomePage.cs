using Microsoft.Playwright;

namespace Playwright.E2ETests.Pages.NhsWalesPages;

public class WpHomePage : BasePage
{
    private readonly ILocator _pocztaLink;
    private readonly ILocator _wpLogo;
    private readonly ILocator _acceptCookiesButton;

    public WpHomePage(IPage page) : base(page)
    {
        _pocztaLink = page.GetByRole(AriaRole.Link, new() { Name = "Poczta" });
        _wpLogo = page.GetByAltText("30 lat WP – logo");
        _acceptCookiesButton = page.GetByRole(AriaRole.Button, new() { Name = "Akceptuję i przechodzę do serwisu" });
    }

    public async Task NavigateAsync()
    {
        await Page.GotoAsync("https://wp.pl");
        await Page.WaitForLoadStateAsync(LoadState.Load);
    }

    public async Task AcceptCookiesIfPresentAsync()
    {
        try
        {
            await _acceptCookiesButton.WaitForAsync(new() { State = WaitForSelectorState.Visible, Timeout = 3000 });
            if (await _acceptCookiesButton.IsVisibleAsync())
            {
                await _acceptCookiesButton.ClickAsync();
            }
        }
        catch (TimeoutException)
        {
            // Cookie banner not present, continue
        }
    }

    public async Task<bool> IsHomePageDisplayedAsync()
    {
        return await _wpLogo.IsVisibleAsync() && await _pocztaLink.IsVisibleAsync();
    }

    public async Task ClickPocztaLinkAsync()
    {
        // Set up a task to wait for the new page
        var newPageTask = Page.Context.WaitForPageAsync();

        // Click the link that opens in a new tab
        await _pocztaLink.ClickAsync();

        // Wait for the new page to be created
        var pocztaPage = await newPageTask;

        // Wait for the new page to fully load
        await pocztaPage.WaitForLoadStateAsync(LoadState.Load);
    }

    public async Task<IPage> GetPocztaPageAsync()
    {
        var pages = Page.Context.Pages;
        return pages.Count > 1 ? pages[pages.Count - 1] : Page;
    }
}


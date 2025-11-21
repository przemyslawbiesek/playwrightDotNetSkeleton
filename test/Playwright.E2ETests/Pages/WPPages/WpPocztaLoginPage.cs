using Microsoft.Playwright;

namespace Playwright.E2ETests.Pages.NhsWalesPages;

public class WpPocztaLoginPage : BasePage
{
    private readonly ILocator _emailField;
    private readonly ILocator _passwordField;
    private readonly ILocator _loginButton;
    private readonly ILocator _createAccountLink;
    private readonly ILocator _wpPocztaLogo;
    private readonly ILocator _acceptCookiesButton;

    public WpPocztaLoginPage(IPage page) : base(page)
    {
        // Try multiple selectors for email field (could be type='text' or type='email')
        _emailField = page.Locator("input[type='email'], input[type='text'], input[name*='login'], input[name*='email'], input[placeholder*='mail']").First;
        _passwordField = page.Locator("input[type='password']").First;
        _loginButton = page.GetByRole(AriaRole.Button, new() { Name = "Zaloguj się" });
        _createAccountLink = page.GetByRole(AriaRole.Link, new() { Name = "Załóż nowe konto" });
        _wpPocztaLogo = page.GetByAltText("Poczta WP");
        _acceptCookiesButton = page.GetByRole(AriaRole.Button, new() { Name = "Akceptuję i przechodzę do serwisu" });
    }

    public async Task NavigateAsync()
    {
        await Page.GotoAsync("https://poczta.wp.pl");
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
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

    public async Task<bool> IsLoginPageDisplayedAsync()
    {
        try
        {
            // Check URL to ensure we're on the Poczta page
            var url = Page.Url;
            if (!url.Contains("poczta.wp.pl"))
            {
                Console.WriteLine($"Not on Poczta page. Current URL: {url}");
                return false;
            }

            // Check each element with timeout and log the results
            await Page.WaitForTimeoutAsync(5000); // Wait for page to stabilize

            var logoVisible = await _wpPocztaLogo.IsVisibleAsync();
            Console.WriteLine($"Logo visible: {logoVisible}");

            var emailVisible = await _emailField.IsVisibleAsync();
            Console.WriteLine($"Email field visible: {emailVisible}");

            var loginButtonVisible = await _loginButton.IsVisibleAsync();
            Console.WriteLine($"Login button visible: {loginButtonVisible}");

            return logoVisible && emailVisible && loginButtonVisible;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception in IsLoginPageDisplayedAsync: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> IsEmailFieldVisibleAsync()
    {
        return await _emailField.IsVisibleAsync();
    }

    public async Task<bool> IsPasswordFieldVisibleAsync()
    {
        return await _passwordField.IsVisibleAsync();
    }

    public async Task<bool> IsLoginButtonVisibleAsync()
    {
        return await _loginButton.IsVisibleAsync();
    }

    public async Task<bool> IsCreateAccountLinkVisibleAsync()
    {
        return await _createAccountLink.IsVisibleAsync();
    }

    public async Task<string?> GetPageTitleAsync()
    {
        return await Page.TitleAsync();
    }

    public async Task FillEmailAsync(string email)
    {
        await _emailField.FillAsync(email);
    }

    public async Task FillPasswordAsync(string password)
    {
        await _passwordField.FillAsync(password);
    }

    public async Task ClickLoginButtonAsync()
    {
        await _loginButton.ClickAsync();
    }

    public async Task ClickCreateAccountLinkAsync()
    {
        await _createAccountLink.ClickAsync();
    }
}


using Microsoft.Playwright;

namespace Playwright.E2ETests.Pages.NhsWalesPages;

public class WpPocztaRegistrationPage : BasePage
{
    private readonly ILocator _registrationForm;
    private readonly ILocator _emailField;
    private readonly ILocator _pageHeading;

    public WpPocztaRegistrationPage(IPage page) : base(page)
    {
        // Locators for registration page elements
        _registrationForm = page.Locator("form");
        _emailField = page.Locator("input[type='email'], input[name*='email']").First;
        _pageHeading = page.Locator("h1, h2").First;
    }

    public async Task<bool> IsRegistrationPageDisplayedAsync()
    {
        try
        {
            // Check URL to ensure we're on the registration page
            var url = Page.Url;
            var isRegistrationUrl = url.Contains("rejestracja") || url.Contains("register") || url.Contains("signup");

            if (!isRegistrationUrl)
            {
                Console.WriteLine($"Not on registration page. Current URL: {url}");
                return false;
            }

            // Wait for page to stabilize
            await Page.WaitForLoadStateAsync(LoadState.Load);

            // Check if form is visible
            var formVisible = await _registrationForm.IsVisibleAsync();
            Console.WriteLine($"Registration form visible: {formVisible}");

            return formVisible;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception in IsRegistrationPageDisplayedAsync: {ex.Message}");
            return false;
        }
    }

    public string GetPageUrl()
    {
        return Page.Url;
    }

    public async Task<string?> GetPageTitleAsync()
    {
        return await Page.TitleAsync();
    }

    public async Task<string?> GetPageHeadingAsync()
    {
        try
        {
            return await _pageHeading.TextContentAsync();
        }
        catch
        {
            return null;
        }
    }
}


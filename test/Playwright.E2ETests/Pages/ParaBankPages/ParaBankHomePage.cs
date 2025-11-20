using Microsoft.Playwright;

namespace Playwright.E2ETests.Pages.ParaBankPages;

public class ParaBankHomePage : BasePage
{
    private readonly ILocator _aboutUsLink;
    private readonly ILocator _servicesLink;
    private readonly ILocator _paraBankLogo;
    private readonly ILocator _customerLoginHeading;

    public ParaBankHomePage(IPage page) : base(page)
    {
        _aboutUsLink = page.Locator("#headerPanel").GetByRole(AriaRole.Link, new() { Name = "About Us" });
        _servicesLink = page.Locator("#headerPanel").GetByRole(AriaRole.Link, new() { Name = "Services" });
        _paraBankLogo = page.Locator("#topPanel").GetByRole(AriaRole.Link, new() { Name = "ParaBank", Exact = true });
        _customerLoginHeading = page.GetByRole(AriaRole.Heading, new() { Name = "Customer Login" });
    }

    public async Task NavigateToAsync()
    {
        await Page.GotoAsync("https://parabank.parasoft.com/");
    }

    public async Task ClickAboutUsAsync()
    {
        await _aboutUsLink.ClickAsync();
    }

    public async Task ClickServicesAsync()
    {
        await _servicesLink.ClickAsync();
    }

    public ILocator GetCustomerLoginHeading()
    {
        return _customerLoginHeading;
    }

    public ILocator GetParaBankLogo()
    {
        return _paraBankLogo;
    }

    public ILocator GetAboutUsLink()
    {
        return _aboutUsLink;
    }

    public new IPage GetPage()
    {
        return Page;
    }
}


using Microsoft.Playwright;

namespace Playwright.E2ETests.Pages.ParaBankPages;

public class ParaBankAboutUsPage : BasePage
{
    private readonly ILocator _aboutUsHeading;
    private readonly ILocator _servicesLink;
    private readonly ILocator _parasoftLink;

    public ParaBankAboutUsPage(IPage page) : base(page)
    {
        _aboutUsHeading = page.GetByRole(AriaRole.Heading, new() { Name = "ParaSoft Demo Website" });
        _servicesLink = page.Locator("#headerPanel").GetByRole(AriaRole.Link, new() { Name = "Services" });
        _parasoftLink = page.GetByRole(AriaRole.Link, new() { Name = "www.parasoft.com" }).First;
    }

    public async Task ClickServicesAsync()
    {
        await _servicesLink.ClickAsync();
    }

    public ILocator GetAboutUsHeading()
    {
        return _aboutUsHeading;
    }

    public ILocator GetParasoftLink()
    {
        return _parasoftLink;
    }
}


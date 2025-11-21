using Microsoft.Playwright;

namespace Playwright.E2ETests.Pages.ParaBankPages;

public class ParaBankServicesPage : BasePage
{
    private readonly ILocator _bookstoreServicesTable;
    private readonly ILocator _paraBankServicesTable;
    private readonly ILocator _restfulServicesTable;
    private readonly ILocator _aboutUsLink;

    public ParaBankServicesPage(IPage page) : base(page)
    {
        _bookstoreServicesTable = page.Locator("table").First;
        _paraBankServicesTable = page.Locator("table").Nth(2);
        _restfulServicesTable = page.Locator("table").Nth(4);
        _aboutUsLink = page.Locator("#headerPanel").GetByRole(AriaRole.Link, new() { Name = "About Us" });
    }

    public ILocator GetBookstoreServicesTable()
    {
        return _bookstoreServicesTable;
    }

    public ILocator GetParaBankServicesTable()
    {
        return _paraBankServicesTable;
    }

    public ILocator GetRestfulServicesTable()
    {
        return _restfulServicesTable;
    }

    public async Task ClickAboutUsAsync()
    {
        await _aboutUsLink.ClickAsync();
    }
}


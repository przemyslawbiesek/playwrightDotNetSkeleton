using Microsoft.Playwright;
using Playwright.E2ETests.Configuration;

namespace Playwright.E2ETests.Browser;

public class BrowserManager
{
    private static readonly bool Headless = bool.Parse(ConfigurationManager.GetCommon().Headless ?? "false");

    private readonly BrowserConfiguration _configuration;
    private readonly IPlaywright _playwright;

    public BrowserManager(IPlaywright playwright)
    {
        _playwright = playwright;
        _configuration = ConfigurationManager.GetBrowser();
    }

    public async Task<IBrowser> GetBrowserAsync()
    {
        switch (_configuration.Browser)
        {
            case "Chrome": return await _playwright.Chromium.LaunchAsync(GetLaunchOptions());
            case "Firefox": return await _playwright.Firefox.LaunchAsync(GetLaunchOptions("firefox"));
            case "Edge": return await _playwright.Chromium.LaunchAsync(GetLaunchOptions("msedge"));
            case "Safari": return await _playwright.Webkit.LaunchAsync(GetLaunchOptions());
            default: throw new ArgumentException($"Browser type {_configuration.Browser} is not supported");
        }
    }

    public async Task<IBrowserContext> GetContextAsync(IBrowser browser)
    {
        if (_configuration.Device != null)
        {
            var device = _playwright.Devices[_configuration.Device];
            return await browser.NewContextAsync(device);
        }

        var contextOptions = new BrowserNewContextOptions();

        if (_configuration.ViewportWidth.HasValue && _configuration.ViewportHeight.HasValue)
        {
            contextOptions.ViewportSize = new ViewportSize
            {
                Width = _configuration.ViewportWidth.Value,
                Height = _configuration.ViewportHeight.Value
            };
        }

        return await browser.NewContextAsync(contextOptions);
    }

    private static BrowserTypeLaunchOptions GetLaunchOptions()
    {
        return new BrowserTypeLaunchOptions { Headless = Headless };
    }

    private static BrowserTypeLaunchOptions GetLaunchOptions(string channel)
    {
        return new BrowserTypeLaunchOptions { Headless = Headless, Channel = channel };
    }
}

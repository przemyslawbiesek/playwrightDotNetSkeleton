namespace Playwright.E2ETests.Configuration;

public class BrowserConfiguration
{
    public string? Browser { get; set; }
    public string? Device { get; set; }
    public int? ViewportWidth { get; set; }
    public int? ViewportHeight { get; set; }
}

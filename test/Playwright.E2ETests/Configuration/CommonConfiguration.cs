namespace Playwright.E2ETests.Configuration;

public class CommonConfiguration
{
    public string? ScreenshotsDirectory { get; set; }
    public string? AccessibilityDirectory { get; set; }
    public List<string>? AccessibilityTags { get; set; }
    public string? TracesDirectory { get; set; }
    public string? Headless { get; set; }
    public bool AutoAccessibilityTracking { get; set; } = true;
}

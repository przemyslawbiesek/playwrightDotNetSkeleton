using Microsoft.Playwright;
using Playwright.E2ETests.Configuration;

namespace Playwright.E2ETests.Utils;

public class ScreenshotHelper
{
    private static readonly string ScreenshotsDirectory = ConfigurationManager.GetCommon().ScreenshotsDirectory ??
                                                          throw new ArgumentException("Screenshot is not set");

    public static async Task<string> MakeScreenshotAsync(IPage page)
    {
        var filePath = Path.Combine(ScreenshotsDirectory, $"{Guid.NewGuid()}.png");
        await page.ScreenshotAsync(new PageScreenshotOptions { Path = filePath, FullPage = true });
        return filePath;
    }
}

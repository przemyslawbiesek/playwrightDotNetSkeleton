using Microsoft.Playwright;
using System.Threading.Tasks;

namespace Playwright.E2ETests.Utils
{
    public static class WebElementHelper
    {
        public static async Task<string?> IsVisibleOrNull(ILocator locator)
        {
            return await locator.IsVisibleAsync() ? await locator.InnerTextAsync() : null;
        }
    }
}

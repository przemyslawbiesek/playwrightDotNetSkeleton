using System;
using System.Threading.Tasks;

namespace Playwright.E2ETests.Utils;

public static class Awaitility
{
    public static async Task WaitUntilAsync(
        Func<Task<bool>> condition,
        TimeSpan timeout,
        TimeSpan pollInterval)
    {
        var start = DateTime.UtcNow;
        while (DateTime.UtcNow - start < timeout)
        {
            if (await condition())
                return;
            await Task.Delay(pollInterval);
        }
        throw new TimeoutException("Condition was not met within the timeout.");
    }
}

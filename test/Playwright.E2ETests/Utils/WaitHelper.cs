namespace Playwright.E2ETests.Utils;

public static class WaitHelper
{
    public static async Task RunUntilTrueAsync(Func<Task<bool>> until, int waitTime = 1000, int iterations = 10)
    {
        for (var i = 0; i < iterations; i++)
        {
            if (await until()) return;

            Thread.Sleep(waitTime);
        }

        throw new TimeoutException("Waiting for condition failed.");
    }

    public static async Task<bool> RunAndWaitToBeTrueAsync(Func<Task<bool>> until, int waitTime = 1000,
        int iterations = 10)
    {
        for (var i = 0; i < iterations; i++)
        {
            if (await until()) return true;

            Thread.Sleep(waitTime);
        }

        return false;
    }
}

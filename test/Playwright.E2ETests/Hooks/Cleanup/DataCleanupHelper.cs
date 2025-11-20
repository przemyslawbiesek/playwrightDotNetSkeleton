using Playwright.E2ETests.Context;
using Reqnroll.BoDi;

namespace Playwright.E2ETests.Hooks.Cleanup;

public class DataCleanupHelper
{
    private readonly CustomTestContext _customTestContext;

    public DataCleanupHelper(IObjectContainer objectContainer)
    {
        _customTestContext = objectContainer.Resolve<CustomTestContext>();
    }
}

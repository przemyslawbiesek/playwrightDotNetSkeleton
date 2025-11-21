using Playwright.E2ETests.Context;
using Reqnroll;

namespace Playwright.E2ETests.Steps;

[Binding]
public class BaseStepDefinitions
{
    public BaseStepDefinitions(CustomTestContext customTestContext)
    {
        CustomTestContext = customTestContext;
    }

    protected CustomTestContext CustomTestContext { get; }
}

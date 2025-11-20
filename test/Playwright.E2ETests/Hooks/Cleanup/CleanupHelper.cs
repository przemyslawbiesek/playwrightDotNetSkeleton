using Microsoft.Playwright;
using Playwright.E2ETests.Configuration;
using Playwright.E2ETests.Context;
using Playwright.E2ETests.Utils;
using Playwright.E2ETests.Validators;
using Reqnroll;
using Reqnroll.BoDi;

namespace Playwright.E2ETests.Hooks.Cleanup;

public class CleanupHelper
{
    private readonly IObjectContainer _objectContainer;
    private readonly ScenarioContext _scenarioContext;

    public CleanupHelper(IObjectContainer objectContainer, ScenarioContext scenarioContext)
    {
        _objectContainer = objectContainer;
        _scenarioContext = scenarioContext;
    }

    public async Task CleanupAsync()
    {
        // Cleanup browser resources
        await CleanupBrowserAsync();

        // Check accessibility failures only if the functional test passed
        // This ensures accessibility issues don't mask functional failures
        if (_scenarioContext.TestError == null)
        {
            CheckAccessibilityFailures();
        }
    }

    private void CheckAccessibilityFailures()
    {
        try
        {
            var testContext = _objectContainer.Resolve<Context.CustomTestContext>();

            // Generate consolidated accessibility report
            string? consolidatedReportPath = null;
            try
            {
                var accessibilityHelper = (Utils.AccessibilityHelper)testContext.Get(Context.CustomTestContext.Keys.AccessibilityHelper);
                consolidatedReportPath = accessibilityHelper.GenerateConsolidatedReportAsync().Result;

                if (!string.IsNullOrEmpty(consolidatedReportPath) && File.Exists(consolidatedReportPath))
                {
                    Console.WriteLine("📊 Consolidated accessibility report generation completed");

                    // Attach to Allure report
                    try
                    {
                        Allure.Net.Commons.AllureApi.AddAttachment(
                            "Consolidated Accessibility Report",
                            "text/html",
                            consolidatedReportPath);
                        Console.WriteLine("✅ Consolidated accessibility report attached to Allure");
                    }
                    catch (Exception allureEx)
                    {
                        Console.WriteLine($"⚠️ Warning: Could not attach accessibility report to Allure: {allureEx.Message}");
                    }
                }
                else
                {
                    Console.WriteLine("ℹ️ No accessibility checks were performed in this scenario");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"⚠️ Warning: Could not generate consolidated accessibility report: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
            }

            // Retrieve failures list
            List<string> failures;
            try
            {
                failures = (List<string>)testContext.Get(Context.CustomTestContext.Keys.AccessibilityFailures);
            }
            catch
            {
                // If failures list doesn't exist, assume no failures
                failures = new List<string>();
            }

            if (failures.Count > 0)
            {
                Console.WriteLine("\n========================================");
                Console.WriteLine("ACCESSIBILITY CHECK SUMMARY");
                Console.WriteLine("========================================");
                Console.WriteLine($"✅ All functional tests passed");
                Console.WriteLine($"❌ Accessibility issues found on {failures.Count} page(s)");
                Console.WriteLine("========================================\n");

                Validators.AccessibilityValidator.ValidateAllAccessibilityChecks(failures);
            }
            else
            {
                Console.WriteLine("\n✅ All functional tests passed");
                Console.WriteLine("✅ All accessibility checks passed\n");
            }
        }
        catch (ObjectContainerException)
        {
            // CustomTestContext not registered or AccessibilityFailures key not set
            // This means no accessibility checks were run or all passed
        }
        catch (KeyNotFoundException)
        {
            // No accessibility failures recorded - all checks passed
        }
    }

    private async Task CleanupBrowserAsync()
    {
        try
        {
            await CleanupTracingAsync();
            var browser = _objectContainer.Resolve<IBrowser>();
            await browser.CloseAsync();
        }
        catch (ObjectContainerException)
        {
            // Browser components not registered
        }
    }

    private async Task CleanupTracingAsync()
    {
        try
        {
            var context = _objectContainer.Resolve<IBrowserContext>();
            var tracesDirectory = ConfigurationManager.GetCommon().TracesDirectory ??
                                  throw new ArgumentException("Traces directory name is not set");
            await context.Tracing.StopAsync(new TracingStopOptions
            {
                Path = Path.Combine(tracesDirectory, $"{_scenarioContext.ScenarioInfo.Title}.zip")
            });
        }
        catch (ObjectContainerException)
        {
            // Browser context not registered
        }
    }
}

using Playwright.E2ETests.Configuration;
using Microsoft.Playwright;
using Playwright.E2ETests.Hooks.Cleanup;
using Playwright.E2ETests.Hooks.Setup;
using Reqnroll;
using Reqnroll.BoDi;

namespace Playwright.E2ETests.Hooks;

[Binding]
public class Hooks
{
    private readonly IObjectContainer _objectContainer;
    private readonly ScenarioContext _scenarioContext;

    public Hooks(ScenarioContext scenarioContext, IObjectContainer objectContainer)
    {
        _objectContainer = objectContainer;
        _scenarioContext = scenarioContext;
    }

    [BeforeScenario]
    public async Task SetupScenario(ScenarioContext scenarioContext, FeatureContext featureContext)
    {
        // Get feature tags and scenario tags
        var featureTags = featureContext.FeatureInfo.Tags ?? [];
        var scenarioTags = scenarioContext.ScenarioInfo.Tags ?? [];
        
        // Combine feature and scenario tags
        var allTags = featureTags.Concat(scenarioTags).Distinct().ToList();
        var tagsString = string.Join(", ", allTags);
        
        Console.WriteLine($"Setting up scenario: {scenarioContext.ScenarioInfo.Title}");
        Console.WriteLine($"Feature Tags: {string.Join(", ", featureTags)}");
        Console.WriteLine($"Scenario Tags: {string.Join(", ", scenarioTags)}");
        Console.WriteLine($"All Combined Tags: {tagsString}");
        
        // Check if this is API-only test
        var isApiOnlyTest = allTags.Contains("api", StringComparer.OrdinalIgnoreCase) && 
                           !allTags.Contains("UI", StringComparer.OrdinalIgnoreCase);
        
        Console.WriteLine($"Is API-only test: {isApiOnlyTest}");
        
        // Use the existing method but with tag override in the context
        // Store the combined tags in scenario context for SetupHelper to use
        scenarioContext["AllTags"] = allTags;
        
        var playwright = Microsoft.Playwright.Playwright.CreateAsync().Result;
        await new SetupHelper(_objectContainer, _scenarioContext, playwright).SetupAsync();
    }

    [AfterScenario]
    public async Task CleanUpAsync()
    {
        await new CleanupHelper(_objectContainer, _scenarioContext).CleanupAsync();
    }

    [AfterStep]
    public async Task AfterStepAsync()
    {
        if (_scenarioContext.TestError != null)
        {
            try
            {
                // Only attempt screenshot for UI tests (where page is available)
                var page = _objectContainer.Resolve<Microsoft.Playwright.IPage>();
                if (page != null)
                {
                    var screenshotPath = Path.Combine(
                        AppDomain.CurrentDomain.BaseDirectory,
                        "Screenshots",
                        $"failed-step-{DateTime.Now:yyyy-MM-dd-HH-mm-ss-fff}.png");

                    // Ensure Screenshots directory exists
                    Directory.CreateDirectory(Path.GetDirectoryName(screenshotPath)!);

                    // Take screenshot
                    await page.ScreenshotAsync(new Microsoft.Playwright.PageScreenshotOptions
                    {
                        Path = screenshotPath,
                        FullPage = true
                    });

                    // Attach to Allure report
                    Allure.Net.Commons.AllureApi.AddAttachment("Failed Step Screenshot", "image/png", screenshotPath);
                }
            }
            catch (ObjectContainerException)
            {
                // Page not available - this is expected for API-only tests
                // Log the error details for API tests instead
                Console.WriteLine($"API Test Error: {_scenarioContext.TestError.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to capture screenshot for failed step: {ex.Message}");
            }
        }
    }

    [BeforeTestRun]
    public static void CleanTestArtifactsFolders()
    {
        // Clean allure-results folder
        var allureResultsPath = Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory,
            "allure-results");
        if (Directory.Exists(allureResultsPath))
        {
            Directory.Delete(allureResultsPath, true);
            Directory.CreateDirectory(allureResultsPath);
        }

        // Clean Traces folder
        var tracesPath = Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory,
            "Traces");
        if (Directory.Exists(tracesPath))
        {
            Directory.Delete(tracesPath, true);
            Directory.CreateDirectory(tracesPath);
        }

        // Clean AccessibilityReport folder
        var accessibilityPath = Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory,
            "AccessibilityReport");
        if (Directory.Exists(accessibilityPath))
        {
            Directory.Delete(accessibilityPath, true);
            Directory.CreateDirectory(accessibilityPath);
        }

        // Clean Screenshots folder
        var screenshotsPath = Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory,
            "Screenshots");
        if (Directory.Exists(screenshotsPath))
        {
            Directory.Delete(screenshotsPath, true);
            Directory.CreateDirectory(screenshotsPath);
        }
    }
}

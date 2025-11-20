namespace Playwright.E2ETests.Configuration;

public class ConfigurationManager
{
    static ConfigurationManager()
    {
        // Load .env file from the workspace root
        var envPath = Path.Combine(GetWorkspaceRoot(), ".env");
        if (File.Exists(envPath))
        {
            DotNetEnv.Env.Load(envPath);
        }
    }

    private static string GetWorkspaceRoot()
    {
        var currentDirectory = Directory.GetCurrentDirectory();
        while (!File.Exists(Path.Combine(currentDirectory, ".env")) &&
               !File.Exists(Path.Combine(currentDirectory, ".env.example")))
        {
            var parent = Directory.GetParent(currentDirectory);
            if (parent == null)
            {
                // If not found, return the current directory
                return Directory.GetCurrentDirectory();
            }
            currentDirectory = parent.FullName;
        }
        return currentDirectory;
    }

    public static CommonConfiguration GetCommon()
    {
        return new CommonConfiguration
        {
            ScreenshotsDirectory = GetEnvOrDefault("SCREENSHOTS_DIRECTORY", "Screenshots"),
            AccessibilityDirectory = GetEnvOrDefault("ACCESSIBILITY_DIRECTORY", "AccessibilityReport"),
            TracesDirectory = GetEnvOrDefault("TRACES_DIRECTORY", "Traces"),
            AccessibilityTags = GetEnvOrDefault("ACCESSIBILITY_TAGS", "wcag2a,wcag2aa,wcag21a,wcag21aa")
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(t => t.Trim())
                .ToList(),
            Headless = GetEnvOrDefault("HEADLESS", "false"),
            AutoAccessibilityTracking = bool.Parse(GetEnvOrDefault("AUTO_ACCESSIBILITY_TRACKING", "true"))
        };
    }


    public static BrowserConfiguration GetBrowser()
    {
        var configuration = GetEnvOrDefault("BROWSER_CONFIGURATION", "chrome");

        // Check if BROWSER is explicitly set (overrides BROWSER_CONFIGURATION)
        var browser = System.Environment.GetEnvironmentVariable("BROWSER");
        var device = System.Environment.GetEnvironmentVariable("DEVICE");

        if (!string.IsNullOrEmpty(browser))
        {
            return new BrowserConfiguration
            {
                Browser = browser,
                Device = device,
                ViewportWidth = ParseIntOrNull("VIEWPORT_WIDTH"),
                ViewportHeight = ParseIntOrNull("VIEWPORT_HEIGHT")
            };
        }

        // Use predefined configurations based on BROWSER_CONFIGURATION
        return GetPredefinedBrowserConfiguration(configuration);
    }

    private static BrowserConfiguration GetPredefinedBrowserConfiguration(string configuration)
    {
        return configuration.ToLowerInvariant() switch
        {
            "chrome" => new BrowserConfiguration { Browser = "Chrome", ViewportWidth = 1920, ViewportHeight = 1080 },
            "firefox" => new BrowserConfiguration { Browser = "Firefox", ViewportWidth = 1920, ViewportHeight = 1080 },
            "safari" => new BrowserConfiguration { Browser = "Safari", ViewportWidth = 1920, ViewportHeight = 1080 },
            "edge" => new BrowserConfiguration { Browser = "Edge", ViewportWidth = 1920, ViewportHeight = 1080 },
            "safari-iphone-13" => new BrowserConfiguration { Browser = "Safari", Device = "iPhone 13" },
            "safari-iphone-13-landscape" => new BrowserConfiguration { Browser = "Safari", Device = "iPhone 13 landscape" },
            "chrome-galaxy-s15" => new BrowserConfiguration { Browser = "Chrome", Device = "Galaxy S9+" },
            "chrome-galaxy-s15-landscape" => new BrowserConfiguration { Browser = "Chrome", Device = "Galaxy S9+ landscape" },
            _ => throw new ArgumentException($"Browser configuration '{configuration}' is not defined")
        };
    }

    private static string GetEnvOrDefault(string key, string defaultValue)
    {
        return System.Environment.GetEnvironmentVariable(key) ?? defaultValue;
    }

    private static int? ParseIntOrNull(string key)
    {
        var value = System.Environment.GetEnvironmentVariable(key);
        return int.TryParse(value, out var result) ? result : null;
    }
}

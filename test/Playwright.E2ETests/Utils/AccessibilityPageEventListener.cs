using System.Collections.Concurrent;
using Microsoft.Playwright;
using Playwright.E2ETests.Configuration;
using Playwright.E2ETests.Context;

namespace Playwright.E2ETests.Utils;

/// <summary>
/// Automatically tracks accessibility on page navigations using Playwright page events.
/// Can be enabled/disabled via configuration.
/// Uses global static tracking to prevent duplicate scans across parallel test scenarios.
/// </summary>
public class AccessibilityPageEventListener
{
    // Static thread-safe collection for GLOBAL URL tracking across ALL test scenarios
    // This prevents duplicate Axe scans when multiple scenarios access the same URL in parallel
    private static readonly ConcurrentDictionary<string, byte> GlobalScannedUrls = new();

    private readonly IPage _page;
    private readonly AccessibilityHelper _accessibilityHelper;
    private readonly CustomTestContext _customTestContext;
    private readonly bool _isEnabled;
    private bool _isAttached;

    public AccessibilityPageEventListener(
        IPage page,
        AccessibilityHelper accessibilityHelper,
        CustomTestContext customTestContext)
    {
        _page = page;
        _accessibilityHelper = accessibilityHelper;
        _customTestContext = customTestContext;
        _isEnabled = ConfigurationManager.GetCommon().AutoAccessibilityTracking;
    }

    /// <summary>
    /// Clears the global scanned URLs cache.
    /// Useful for test isolation or when running tests sequentially that should re-scan pages.
    /// Note: In parallel execution, this should typically not be called during active test runs.
    /// </summary>
    public static void ClearGlobalCache()
    {
        GlobalScannedUrls.Clear();
        Console.WriteLine("AccessibilityPageEventListener: Global scanned URLs cache cleared");
    }

    /// <summary>
    /// Gets the count of globally scanned URLs (useful for diagnostics)
    /// </summary>
    public static int GetGlobalScannedUrlCount()
    {
        return GlobalScannedUrls.Count;
    }

    /// <summary>
    /// Attach event listeners to the page for automatic accessibility tracking
    /// </summary>
    public void Attach()
    {
        if (!_isEnabled || _isAttached)
        {
            if (!_isEnabled)
            {
                Console.WriteLine("AccessibilityPageEventListener: Auto accessibility tracking is disabled in configuration");
            }
            else if (_isAttached)
            {
                Console.WriteLine("AccessibilityPageEventListener: Already attached, skipping");
            }
            return;
        }

        Console.WriteLine("AccessibilityPageEventListener: Attaching automatic accessibility tracking");
        Console.WriteLine($"AccessibilityPageEventListener: Current page URL: {_page.Url}");

        // Track accessibility after successful navigation
        _page.Load += async (_, _) => await OnPageLoadAsync();
        Console.WriteLine("AccessibilityPageEventListener: Attached to Page.Load event");

        // Note: DOMContentLoaded is disabled for ParaBank tests as it fires too early during navigation
        // causing "Execution context destroyed" errors. Page.Load event is more reliable.
        // Uncomment below for SPA applications that need early accessibility checks:
        // _page.DOMContentLoaded += async (_, _) => await OnDOMContentLoadedAsync();
        // Console.WriteLine("AccessibilityPageEventListener: Attached to Page.DOMContentLoaded event");

        _isAttached = true;
        Console.WriteLine("AccessibilityPageEventListener: Event listeners successfully attached");
    }

    /// <summary>
    /// Detach event listeners from the page
    /// </summary>
    public void Detach()
    {
        if (!_isAttached)
        {
            return;
        }

        Console.WriteLine("AccessibilityPageEventListener: Detaching automatic accessibility tracking");

        // Note: Playwright doesn't support removing individual event handlers
        // The listeners will be automatically cleaned up when the page is closed
        _isAttached = false;
    }

    private async Task OnPageLoadAsync()
    {
        if (!_isEnabled)
        {
            return;
        }

        try
        {
            Console.WriteLine($"AccessibilityPageEventListener: Page Load event fired - {_page.Url}");

            // Run accessibility scan immediately without waiting for NetworkIdle
            // Page.Load event means the page is already loaded enough for accessibility checks
            Console.WriteLine($"AccessibilityPageEventListener: Running accessibility check - {_page.Url}");
            await TrackAccessibilityAsync();
        }
        catch (Exception ex) when (ex.Message.Contains("Target page, context or browser has been closed") ||
                                    ex.Message.Contains("Execution context was destroyed"))
        {
            // Page navigated away or closed - skip this scan
            Console.WriteLine($"AccessibilityPageEventListener: Skipping scan, page navigated away - {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"AccessibilityPageEventListener: Error tracking accessibility on page load: {ex.Message}");
        }
    }

    private async Task OnDOMContentLoadedAsync()
    {
        if (!_isEnabled)
        {
            return;
        }

        try
        {
            Console.WriteLine($"AccessibilityPageEventListener: DOMContentLoaded event fired - {_page.Url}");

            // Add a small delay to ensure dynamic content is rendered
            // This is useful for SPAs and dynamic content
            await Task.Delay(300);

            Console.WriteLine($"AccessibilityPageEventListener: After delay, running accessibility check - {_page.Url}");
            await TrackAccessibilityAsync();
        }
        catch (Exception ex) when (ex.Message.Contains("Target page, context or browser has been closed") ||
                                    ex.Message.Contains("Execution context was destroyed"))
        {
            // Page navigated away or closed during delay - skip this scan
            Console.WriteLine($"AccessibilityPageEventListener: Skipping scan, page navigated during delay");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"AccessibilityPageEventListener: Error tracking accessibility on DOM content loaded: {ex.Message}");
        }
    }

    private async Task TrackAccessibilityAsync()
    {
        try
        {
            var pageUrl = _page.Url;

            // Use global static ConcurrentDictionary for thread-safe deduplication across ALL scenarios
            // TryAdd returns true only if the key was added (first time), false if already exists
            bool wasAdded = GlobalScannedUrls.TryAdd(pageUrl, 0);

            if (!wasAdded)
            {
                Console.WriteLine($"AccessibilityPageEventListener: Page already scanned globally, skipping - {pageUrl}");
                return;
            }

            Console.WriteLine($"AccessibilityPageEventListener: Scanning page for the first time globally - {pageUrl}");

            var result = await _accessibilityHelper.RunAndTrack(_page);
            Validators.AccessibilityValidator.TrackAccessibilityCheck(result.Passed, result.PageUrl);

            if (!result.Passed)
            {
                // Get or create the list of accessibility failures with thread safety
                List<string> failures;
                try
                {
                    failures = (List<string>)_customTestContext.Get(CustomTestContext.Keys.AccessibilityFailures);
                }
                catch
                {
                    failures = new List<string>();
                    _customTestContext.Put(CustomTestContext.Keys.AccessibilityFailures, failures);
                }

                var failureMessage = $"{result.PageUrl} (checked at {result.CheckedAt:HH:mm:ss})";

                // Thread-safe add to failures list to prevent duplicates in parallel execution
                lock (failures)
                {
                    if (!failures.Contains(failureMessage))
                    {
                        failures.Add(failureMessage);
                    }
                }
            }

            Console.WriteLine($"AccessibilityPageEventListener: Accessibility check completed for {result.PageUrl} - {(result.Passed ? "PASSED" : "FAILED")}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"AccessibilityPageEventListener: Error during accessibility tracking: {ex.Message}");
        }
    }
}


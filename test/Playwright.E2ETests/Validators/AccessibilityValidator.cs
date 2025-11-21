using NUnit.Framework;

namespace Playwright.E2ETests.Validators
{
    /// <summary>
    /// Validator for accessibility-related assertions
    /// </summary>
    public static class AccessibilityValidator
    {
        /// <summary>
        /// Validates that the page passes accessibility checks
        /// </summary>
        /// <param name="accessibilityCheckResult">Result of the accessibility check</param>
        /// <param name="customMessage">Optional custom message for the assertion</param>
        public static void ValidateAccessibilityCheck(bool accessibilityCheckResult, string? customMessage = null)
        {
            var message = customMessage ?? "Accessibility check failed";
            Assert.IsTrue(accessibilityCheckResult, message);
        }

        /// <summary>
        /// Tracks accessibility check result without failing immediately
        /// Logs warning if check fails but continues test execution
        /// </summary>
        /// <param name="accessibilityCheckResult">Result of the accessibility check</param>
        /// <param name="pageUrl">URL of the page that was checked</param>
        public static void TrackAccessibilityCheck(bool accessibilityCheckResult, string pageUrl)
        {
            if (!accessibilityCheckResult)
            {
                Console.WriteLine($"⚠️ WARNING: Accessibility check failed for page: {pageUrl}");
                Console.WriteLine("Accessibility issues detected. Test will continue but fail after scenario completion.");
            }
            else
            {
                Console.WriteLine($"✅ Accessibility check passed for page: {pageUrl}");
            }
        }

        /// <summary>
        /// Validates all collected accessibility failures at the end of the test
        /// </summary>
        /// <param name="failures">List of pages that failed accessibility checks</param>
        public static void ValidateAllAccessibilityChecks(List<string> failures)
        {
            if (failures.Count > 0)
            {
                var message = $"Accessibility checks failed for {failures.Count} page(s):\n" +
                             string.Join("\n", failures.Select((url, i) => $"{i + 1}. {url}"));
                Assert.Fail(message);
            }
        }
    }
}

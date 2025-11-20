using Microsoft.Playwright;
using Playwright.Axe;
using Playwright.E2ETests.Configuration;
using System.Text;

namespace Playwright.E2ETests.Utils;

public class AccessibilityHelper
{
    private readonly string _rootDirectory = ConfigurationManager.GetCommon().AccessibilityDirectory ??
                                             throw new InvalidOperationException();

    private readonly string _scenarioDirectory;
    private readonly List<PageAccessibilityResult> _allResults = new();

    public AccessibilityHelper(string scenarioDirectory)
    {
        _scenarioDirectory = scenarioDirectory;
    }

    public async Task<bool> Run(IPage page)
    {
        var axeResults = await page.RunAxe(SetupAxeRunOptions());

        // Store result for consolidated report
        _allResults.Add(new PageAccessibilityResult
        {
            PageUrl = page.Url,
            AxeResults = axeResults,
            CheckedAt = DateTime.Now,
            ReportPath = string.Empty // Not needed for consolidated report only
        });

        // Individual page reports are disabled - only consolidated report will be generated
        // This reduces noise in Allure reports and provides a single comprehensive view

        return axeResults.Violations.Count == 0;
    }

    public async Task<AccessibilityCheckResult> RunAndTrack(IPage page)
    {
        var passed = await Run(page);
        return new AccessibilityCheckResult
        {
            PageUrl = page.Url,
            Passed = passed,
            CheckedAt = DateTime.Now
        };
    }

    public async Task<string?> GenerateConsolidatedReportAsync()
    {
        if (_allResults.Count == 0)
            return null;

        var consolidatedPath = Path.Combine(_rootDirectory, _scenarioDirectory, "consolidated-accessibility-report.html");
        Directory.CreateDirectory(Path.GetDirectoryName(consolidatedPath)!);

        var sb = new StringBuilder();
        var totalViolations = _allResults.Sum(r => r.AxeResults.Violations.Count);
        var totalPasses = _allResults.Sum(r => r.AxeResults.Passes.Count);
        var pagesWithIssues = _allResults.Count(r => r.AxeResults.Violations.Count > 0);

        // Generate consolidated HTML report
        sb.AppendLine("<!DOCTYPE html>");
        sb.AppendLine("<html lang='en'>");
        sb.AppendLine("<head>");
        sb.AppendLine("    <meta charset='UTF-8'>");
        sb.AppendLine("    <meta name='viewport' content='width=device-width, initial-scale=1.0'>");
        sb.AppendLine("    <title>Consolidated Accessibility Report</title>");
        sb.AppendLine("    <style>");
        sb.AppendLine("        body { font-family: Arial, sans-serif; margin: 20px; background: #f5f5f5; }");
        sb.AppendLine("        .container { max-width: 1400px; margin: 0 auto; background: white; padding: 20px; box-shadow: 0 2px 4px rgba(0,0,0,0.1); }");
        sb.AppendLine("        h1 { color: #d32f2f; border-bottom: 3px solid #d32f2f; padding-bottom: 10px; }");
        sb.AppendLine("        .summary { background: #e3f2fd; padding: 20px; border-radius: 4px; margin: 20px 0; border-left: 4px solid #1976d2; }");
        sb.AppendLine("        .summary-grid { display: grid; grid-template-columns: repeat(auto-fit, minmax(200px, 1fr)); gap: 15px; margin-top: 15px; }");
        sb.AppendLine("        .summary-card { background: white; padding: 15px; border-radius: 4px; text-align: center; box-shadow: 0 1px 3px rgba(0,0,0,0.1); }");
        sb.AppendLine("        .summary-number { font-size: 32px; font-weight: bold; margin-bottom: 5px; }");
        sb.AppendLine("        .summary-label { font-size: 14px; color: #666; }");
        sb.AppendLine("        .violations-number { color: #d32f2f; }");
        sb.AppendLine("        .passes-number { color: #7cb342; }");
        sb.AppendLine("        .pages-number { color: #1976d2; }");
        sb.AppendLine("        .page-section { background: #fff; border: 1px solid #ddd; margin: 20px 0; border-radius: 4px; overflow: hidden; }");
        sb.AppendLine("        .page-header { padding: 15px; cursor: pointer; background: #f5f5f5; }");
        sb.AppendLine("        .page-header:hover { background: #e0e0e0; }");
        sb.AppendLine("        .page-header.success { border-left: 4px solid #7cb342; }");
        sb.AppendLine("        .page-header.has-issues { border-left: 4px solid #d32f2f; }");
        sb.AppendLine("        .page-body { padding: 15px; display: none; }");
        sb.AppendLine("        .page-body.active { display: block; }");
        sb.AppendLine("        .violation { background: #fff; border: 1px solid #ddd; margin: 15px 0; border-radius: 4px; }");
        sb.AppendLine("        .violation-header { padding: 12px; cursor: pointer; }");
        sb.AppendLine("        .violation-header:hover { background: #f5f5f5; }");
        sb.AppendLine("        .critical { border-left: 4px solid #d32f2f; }");
        sb.AppendLine("        .serious { border-left: 4px solid #ff6f00; }");
        sb.AppendLine("        .moderate { border-left: 4px solid #fbc02d; }");
        sb.AppendLine("        .minor { border-left: 4px solid #7cb342; }");
        sb.AppendLine("        .impact-badge { display: inline-block; padding: 4px 12px; border-radius: 3px; font-size: 11px; font-weight: bold; text-transform: uppercase; }");
        sb.AppendLine("        .impact-critical { background: #d32f2f; color: white; }");
        sb.AppendLine("        .impact-serious { background: #ff6f00; color: white; }");
        sb.AppendLine("        .impact-moderate { background: #fbc02d; color: black; }");
        sb.AppendLine("        .impact-minor { background: #7cb342; color: white; }");
        sb.AppendLine("        .violation-body { padding: 0 12px 12px 12px; display: none; }");
        sb.AppendLine("        .violation-body.active { display: block; }");
        sb.AppendLine("        .element { background: #fafafa; border: 1px solid #e0e0e0; margin: 10px 0; padding: 12px; border-radius: 4px; font-size: 13px; }");
        sb.AppendLine("        .selector { background: #e3f2fd; padding: 6px; border-radius: 3px; font-family: monospace; font-size: 12px; margin: 5px 0; word-break: break-all; }");
        sb.AppendLine("        .html-snippet { background: #f5f5f5; padding: 8px; border-radius: 3px; font-family: monospace; font-size: 11px; margin: 5px 0; white-space: pre-wrap; word-break: break-all; }");
        sb.AppendLine("        .issue { background: #fff3e0; padding: 8px; border-left: 3px solid #ff6f00; margin: 5px 0; font-size: 12px; }");
        sb.AppendLine("        .wcag-tags { margin: 8px 0; }");
        sb.AppendLine("        .tag { display: inline-block; background: #e0e0e0; padding: 3px 8px; margin: 2px; border-radius: 3px; font-size: 10px; }");
        sb.AppendLine("        .help-link { color: #1976d2; text-decoration: none; font-size: 13px; }");
        sb.AppendLine("        .help-link:hover { text-decoration: underline; }");
        sb.AppendLine("        .toggle-icon { float: right; }");
        sb.AppendLine("        .page-url { font-size: 18px; font-weight: bold; color: #333; }");
        sb.AppendLine("        .page-stats { font-size: 14px; color: #666; margin-top: 5px; }");
        sb.AppendLine("        .status-badge { display: inline-block; padding: 4px 10px; border-radius: 3px; font-size: 12px; font-weight: bold; margin-left: 10px; }");
        sb.AppendLine("        .status-pass { background: #7cb342; color: white; }");
        sb.AppendLine("        .status-fail { background: #d32f2f; color: white; }");
        sb.AppendLine("    </style>");
        sb.AppendLine("</head>");
        sb.AppendLine("<body>");
        sb.AppendLine("    <div class='container'>");
        sb.AppendLine("        <h1>🔍 Consolidated Accessibility Report</h1>");
        sb.AppendLine("        <div class='summary'>");
        sb.AppendLine($"            <div><strong>Test Scenario:</strong> {System.Net.WebUtility.HtmlEncode(_scenarioDirectory)}</div>");
        sb.AppendLine($"            <div><strong>Report Generated:</strong> {DateTime.Now:yyyy-MM-dd HH:mm:ss}</div>");
        sb.AppendLine("            <div class='summary-grid'>");
        sb.AppendLine("                <div class='summary-card'>");
        sb.AppendLine($"                    <div class='summary-number pages-number'>{_allResults.Count}</div>");
        sb.AppendLine("                    <div class='summary-label'>Pages Checked</div>");
        sb.AppendLine("                </div>");
        sb.AppendLine("                <div class='summary-card'>");
        sb.AppendLine($"                    <div class='summary-number violations-number'>{totalViolations}</div>");
        sb.AppendLine("                    <div class='summary-label'>Total Violations</div>");
        sb.AppendLine("                </div>");
        sb.AppendLine("                <div class='summary-card'>");
        sb.AppendLine($"                    <div class='summary-number passes-number'>{totalPasses}</div>");
        sb.AppendLine("                    <div class='summary-label'>Total Passes</div>");
        sb.AppendLine("                </div>");
        sb.AppendLine("                <div class='summary-card'>");
        sb.AppendLine($"                    <div class='summary-number violations-number'>{pagesWithIssues}</div>");
        sb.AppendLine("                    <div class='summary-label'>Pages with Issues</div>");
        sb.AppendLine("                </div>");
        sb.AppendLine("            </div>");
        sb.AppendLine("        </div>");

        // Add each page section
        foreach (var pageResult in _allResults)
        {
            AppendPageSection(sb, pageResult);
        }

        sb.AppendLine("    </div>");
        sb.AppendLine("    <script>");
        sb.AppendLine("        function togglePage(header) {");
        sb.AppendLine("            const body = header.nextElementSibling;");
        sb.AppendLine("            const icon = header.querySelector('.toggle-icon');");
        sb.AppendLine("            body.classList.toggle('active');");
        sb.AppendLine("            icon.textContent = body.classList.contains('active') ? '▲' : '▼';");
        sb.AppendLine("        }");
        sb.AppendLine("        function toggleViolation(header) {");
        sb.AppendLine("            const body = header.nextElementSibling;");
        sb.AppendLine("            const icon = header.querySelector('.toggle-icon');");
        sb.AppendLine("            body.classList.toggle('active');");
        sb.AppendLine("            icon.textContent = body.classList.contains('active') ? '▲' : '▼';");
        sb.AppendLine("        }");
        sb.AppendLine("        const firstPageWithIssues = document.querySelector('.page-header.has-issues');");
        sb.AppendLine("        if (firstPageWithIssues) { togglePage(firstPageWithIssues); }");
        sb.AppendLine("    </script>");
        sb.AppendLine("</body>");
        sb.AppendLine("</html>");

        await File.WriteAllTextAsync(consolidatedPath, sb.ToString());
        Console.WriteLine($"📊 Consolidated accessibility report generated: {consolidatedPath}");
        return consolidatedPath;
    }

    private void AppendPageSection(StringBuilder sb, PageAccessibilityResult pageResult)
    {
        var hasIssues = pageResult.AxeResults.Violations.Count > 0;
        var statusClass = hasIssues ? "has-issues" : "success";
        var statusBadge = hasIssues ? "<span class='status-badge status-fail'>FAILED</span>" : "<span class='status-badge status-pass'>PASSED</span>";

        sb.AppendLine("        <div class='page-section'>");
        sb.AppendLine($"            <div class='page-header {statusClass}' onclick='togglePage(this)'>");
        sb.AppendLine($"                <div class='page-url'>{System.Net.WebUtility.HtmlEncode(pageResult.PageUrl)}{statusBadge}<span class='toggle-icon'>▼</span></div>");
        sb.AppendLine($"                <div class='page-stats'>Violations: {pageResult.AxeResults.Violations.Count} | Passes: {pageResult.AxeResults.Passes.Count} | Checked: {pageResult.CheckedAt:HH:mm:ss}</div>");
        sb.AppendLine("            </div>");
        sb.AppendLine("            <div class='page-body'>");

        if (hasIssues)
        {
            foreach (var violation in pageResult.AxeResults.Violations)
            {
                AppendViolation(sb, violation);
            }
        }
        else
        {
            sb.AppendLine("                <div style='padding: 20px; text-align: center; color: #7cb342;'>");
            sb.AppendLine("                    <h3>✅ No accessibility issues found on this page!</h3>");
            sb.AppendLine($"                    <p>All {pageResult.AxeResults.Passes.Count} accessibility checks passed.</p>");
            sb.AppendLine("                </div>");
        }

        sb.AppendLine("            </div>");
        sb.AppendLine("        </div>");
    }

    private void AppendViolation(StringBuilder sb, dynamic violation)
    {
        var impactClass = violation.Impact?.ToString().ToLowerInvariant() ?? "minor";
        var impactDisplay = violation.Impact?.ToString() ?? "minor";

        sb.AppendLine($"                <div class='violation {impactClass}'>");
        sb.AppendLine("                    <div class='violation-header' onclick='toggleViolation(this)'>");
        sb.AppendLine($"                        <span class='impact-badge impact-{impactClass}'>{impactDisplay}</span>");
        sb.AppendLine($"                        <strong>{System.Net.WebUtility.HtmlEncode(violation.Id)}</strong> - {System.Net.WebUtility.HtmlEncode(violation.Description)}");
        sb.AppendLine("                        <span class='toggle-icon'>▼</span>");
        sb.AppendLine("                    </div>");
        sb.AppendLine("                    <div class='violation-body'>");
        sb.AppendLine($"                        <div><strong>Help:</strong> {System.Net.WebUtility.HtmlEncode(violation.Help)}</div>");
        sb.AppendLine($"                        <div><strong>Help URL:</strong> <a href='{System.Net.WebUtility.HtmlEncode(violation.HelpUrl.ToString())}' target='_blank' class='help-link'>{System.Net.WebUtility.HtmlEncode(violation.HelpUrl.ToString())}</a></div>");
        sb.AppendLine("                        <div class='wcag-tags'>");
        sb.Append("                            <strong>WCAG Tags:</strong> ");
        foreach (var tag in violation.Tags)
        {
            sb.Append($"<span class='tag'>{System.Net.WebUtility.HtmlEncode(tag)}</span>");
        }
        sb.AppendLine();
        sb.AppendLine("                        </div>");
        sb.AppendLine($"                        <div style='margin-top: 10px;'><strong>Affected Elements ({violation.Nodes.Count}):</strong></div>");

        for (int i = 0; i < violation.Nodes.Count; i++)
        {
            var node = violation.Nodes[i];
            var targetSelector = string.Join(" > ", ((IEnumerable<dynamic>)node.Target).Select(t => System.Net.WebUtility.HtmlEncode(t.ToString())));

            sb.AppendLine("                        <div class='element'>");
            sb.AppendLine($"                            <div><strong>Element #{i + 1}</strong></div>");
            sb.AppendLine("                            <div><strong>Selector:</strong></div>");
            sb.AppendLine($"                            <div class='selector'>{targetSelector}</div>");

            if (!string.IsNullOrEmpty(node.Html))
            {
                sb.AppendLine("                            <div style='margin-top: 8px;'><strong>HTML:</strong></div>");
                sb.AppendLine($"                            <div class='html-snippet'>{System.Net.WebUtility.HtmlEncode(node.Html)}</div>");
            }

            if (!string.IsNullOrEmpty(node.FailureSummary))
            {
                sb.AppendLine($"                            <div class='issue'><strong>Issue:</strong> {System.Net.WebUtility.HtmlEncode(node.FailureSummary)}</div>");
            }

            sb.AppendLine("                        </div>");
        }

        sb.AppendLine("                    </div>");
        sb.AppendLine("                </div>");
    }



    private static AxeRunOptions SetupAxeRunOptions()
    {
        var tags = ConfigurationManager.GetCommon().AccessibilityTags;
        if (tags is { Count: > 0 })
            return new AxeRunOptions(
                new AxeRunOnly(AxeRunOnlyType.Tags, tags)
            );
        return new AxeRunOptions();
    }

    private static string SetPageDirectoryName(string pageUrl)
    {
        return pageUrl.Replace("https://", "")
            .Replace("http://", "")
            .Replace("/", "_");
    }
}

public class AccessibilityCheckResult
{
    public string PageUrl { get; set; } = string.Empty;
    public bool Passed { get; set; }
    public DateTime CheckedAt { get; set; }
}

public class PageAccessibilityResult
{
    public string PageUrl { get; set; } = string.Empty;
    public AxeResults AxeResults { get; set; } = null!;
    public DateTime CheckedAt { get; set; }
    public string ReportPath { get; set; } = string.Empty;
}


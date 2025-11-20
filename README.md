# PlaywrightTests

This project contains automated tests for the NHS Wales Messages API and NHS Wales web application using:

- [Reqnroll](https://reqnroll.net/) - for BDD test scenarios in Gherkin syntax
- [Playwright](https://playwright.dev/dotnet/) - for UI and API testing
- [NUnit](https://nunit.org/) - as the test framework
- [MongoDB](https://www.mongodb.com/) - for test data management

## Project Structure

The solution consists of one project:

- `Playwright.E2ETests`: Main test project containing automated tests using Reqnroll and Playwright

## Prerequisites

- .NET 8.0 SDK
- JetBrains Rider 2024
- Node.js (v16 or higher) and npm - Required for accessibility testing with axe-html-reporter

## Getting Started

1. Clone the repository
2. Open the solution file `PlaywrightTests.sln` in Visual Studio/Rider
3. Configure environment settings (see Configuration section below)
4. Install Node.js dependencies for accessibility testing
5. Install Playwright browsers
6. Build the solution
7. Run tests

### Configuration

This project uses `.env` files for configuration. To get started:

```bash
# Copy the example configuration
cp .env.example .env

# Edit .env with your local settings
# See CONFIGURATION.md for all available options
```

**Key configuration files:**
- `.env` - Your local configuration (git-ignored)
- `.env.example` - Template with all available options
- `.env.uat` - UAT environment configuration

For complete configuration documentation, see [CONFIGURATION.md](CONFIGURATION.md).


### Installing Node.js Dependencies

For accessibility testing features, install the required npm packages:

```bash
npm install
```

This will install:
- `axe-html-reporter` - Generates detailed HTML reports for accessibility violations
6. Setup environment variables

### Installing Playwright browsers

If you have powershell, run command from root project directory:

`pwsh bin/Debug/netX/playwright.ps1 install`

If not try to install powershell:

`brew install powershell/tap/powershell`

If it does not work, try with go:

`brew install go`

Then install browsers:

`go run github.com/playwright-community/playwright-go/cmd/playwright@latest install --with-deps`


## Main Project Structure

The `Playwright.E2ETests` project is organized into the following directories:

### Core Test Infrastructure
- `Browser/`: Browser configurations and management
- `Configuration/`: Environment settings and test configuration management
- `Context/`: Reqnroll test context classes for data sharing between steps
- `Hooks/`: Reqnroll hooks for setup and teardown operations

### Test Features and Implementation
- `Features/`: BDD feature files organized by test type:
  - `Features/Api/`: API testing scenarios including Messages API endpoints
  - `Features/UI/`: User interface testing scenarios
- `Steps/`: Step definitions mapping Gherkin steps to C# implementation:
  - `Steps/Api/`: API step definitions
  - `Steps/UI/`: UI step definitions

### Test Support Components
- `Pages/`: Page Object Model classes for UI interactions
  - `Pages/NhsWalesPages/`: NHS Wales specific page objects
- `Clients/`: Service clients for external systems:
  - API clients (`BaseApiService`, `MessagesApiService`)
  - Database clients (`MongoDbService`)
- `Model/`: Data models and DTOs:
  - `Model/MessagingService/`: Models for Messages API (requests, responses, entities)
  - `Model/Mongo/`: MongoDB document models
  - `Model/Translations/`: Translation and localization models
- `DataBuilders/`: Test data builders for creating test objects
- `Validators/`: Assertion and validation logic
- `Utils/`: Utility classes and helper methods
- `Resources/`: Test configuration files and static resources
- `Reports/`: Generated test reports and artifacts

## Test Coverage

### API Testing
The project includes comprehensive API tests for the NHS Wales Messages API covering:

#### Messages Endpoint (`/users/me/messages`)
- **GET**: Retrieve messages with summary/sender filtering
- **POST**: Create new messages 
- **PATCH**: Update message read status
- **Response Scenarios**: 200 (success), 204 (no content), 400 (bad request), 401 (unauthorized)

#### Message by ID Endpoint (`/users/me/messages/{id}`)
- **GET**: Retrieve specific message by ID
- **Response Scenarios**: 200 (success), 400 (invalid ID), 401 (unauthorized), 404 (not found)

#### Senders Endpoint (`/users/me/messages/senders`)
- **GET**: Retrieve unique message senders with unread counts
- **Response Scenarios**: 200 (success), 204 (no content), 401 (unauthorized)

### UI Testing
- NHS Wales web application user interface testing
- Organ donation decision workflows
- HTTP stub testing for development scenarios

### Authentication Testing
- Bearer token authentication validation
- Invalid token handling
- Missing token scenarios

## Running Tests

### Quick Start Commands

Run all tests:
```bash
dotnet test
```

Run only API tests:
```bash
dotnet test --filter "Category=Api"
```

Run only UI tests:
```bash
dotnet test --filter "Category=UI"
```

Run tests with specific tags:
```bash
dotnet test --filter "Category=stubs"
```

Run specific feature tests:
```bash
dotnet test --filter "Name~MessagesApi"
dotnet test --filter "Name~Senders"
```

### Test Execution Options

You can run the tests using:
- Visual Studio Test Explorer
- JetBrains Rider test runner  
- `dotnet test` command in terminal
- CI/CD pipelines

### Environment Configuration

| Variable             | Required | Description                                                                                                   | Default |
|---------------------|----------|---------------------------------------------------------------------------------------------------------------|---------|
| BrowserConfiguration | No       | Browser and platform configuration. Available options in `Resources/browsers.json`                          | chrome  |
| Headless            | No       | Run browser in headless mode                                                                                  | true    |
| Environment         | No       | Target environment for test execution                                                                         | local   |
| MongoDb__*          | Yes      | MongoDB connection configuration for test data management                                                     | -       |
| MessagesApi__*      | Yes      | Messages API configuration including base URL and authentication                                              | -       |

### Parallel Execution

Configure parallel test execution:

1. Edit `AssemblyInfo.cs`: `[assembly: LevelOfParallelism(4)]` 
2. Edit `run.runsettings`: `<MaxCpuCount>4</MaxCpuCount>`
3. Run with settings: `dotnet test --settings run.runsettings`

### Environment-Specific Execution

Set environment before running tests:
```bash
Environment=staging dotnet test
Environment=production dotnet test --filter "Category=smoke"
Accessibility analysis reports are automatically generated during UI tests when `AccessibilityHelper.Run()` is called.

**Report Location:** `bin/Debug/net8.0/AccessibilityReport/[scenario]/[page]/[guid]/`

**Generated Files:**
- `accessibility-report.html` - Interactive HTML report with detailed information about all accessibility violations including:
  - Full list of affected elements with CSS selectors
  - HTML snippets of each failing element
  - WCAG criteria violated
  - Impact levels (critical, serious, moderate, minor)
  - Fix recommendations and help links
  - Collapsible sections for easy navigation
- `results.json` - Raw JSON results from axe-core analysis

**Features:**
- Color-coded violations by severity
- Click to expand/collapse violation details
- Shows all affected elements (not just summaries)
- Element-specific failure descriptions
### Test Reports and Artifacts

#### Allure Reports
After test execution, generate detailed reports:
```bash
cd bin/Debug/net8.0
allure generate --clean
```
Reports are generated in `/bin/Debug/net8.0/allure-report/`

#### Accessibility Reports
Accessibility analysis reports are automatically generated in:
`bin/Debug/net8.0/AccessibilityReport/`

#### Playwright Traces
Test execution traces for debugging are stored in:
`bin/Debug/net8.0/Traces/`

View traces using: https://trace.playwright.dev/

#### Test Results
Standard test results are stored in:
`TestResults/` directory with timestamped folders


## Key Dependencies

### Core Testing Framework
- **Reqnroll** (2.3.0) - BDD testing framework for .NET
- **NUnit** (3.14.0) - Unit testing framework
- **Playwright** (1.47.0) - Cross-browser automation

### Assertion and Validation
- **FluentAssertions** (6.12.1) - Fluent assertion library
- **Then** (1.0.3) - Additional assertion extensions

### External Integrations
- **MongoDB.Driver** - MongoDB database client
- **Azure.Identity** (1.12.1) - Azure authentication
- **Otp.NET** (1.4.0) - One-time password generation

### Utilities
- **Newtonsoft.Json** - JSON serialization
- **Microsoft.Extensions.Configuration** - Configuration management

For a complete list of dependencies, refer to the `Playwright.E2ETests.csproj` file.

## Architecture Patterns

### Page Object Model (POM)
UI tests use the Page Object pattern with:
- `BasePage` - Common page functionality
- Page-specific classes in `Pages/NhsWalesPages/`
- Locator initialization in constructors
- Action methods representing user interactions

### API Service Layer
API tests use service layer pattern with:
- `BaseApiService` - Common HTTP functionality
- `MessagesApiService` - Messages API specific methods
- Authentication and error handling
- Response validation

### BDD Step Definitions
Test steps follow clear separation:
- `BaseStepDefinitions` - Common step functionality
- `BaseUiStepDefinitions` - UI-specific base steps
- Feature-specific step definition classes

### Test Data Management
- `DataBuilders` - Builder pattern for test data creation
- `MongoDataBuilder` - Database test data setup
- `CustomTestContext` - Data sharing between steps

## Contributing

1. Follow existing code patterns and naming conventions
2. Use Page Object Model for UI interactions
3. Implement proper error handling and validation
4. Add comprehensive test coverage for new features
5. Update documentation when adding new functionality

For detailed coding standards, see: `test/Playwright.E2ETests/MCPContext.md`

Accessibility Reports Integration with Allure
Overview
The accessibility reports are now fully integrated with Allure reporting. This means all accessibility checks are automatically attached to your Allure test reports, providing a comprehensive view of both functional and accessibility test results.

What's Integrated
1. Individual Page Accessibility Reports
   When: Generated whenever an accessibility check finds violations on a page
   Location in Allure: Attached as Accessibility Report - {page_name}
   Format: HTML report with detailed violations, affected elements, and WCAG guidelines
   Contains:
   Violation severity (Critical, Serious, Moderate, Minor)
   WCAG compliance rules violated
   Specific HTML elements affected
   Selectors for each failing element
   Remediation guidance with links to Deque University
2. Consolidated Accessibility Report
   When: Generated at the end of each test scenario
   Location in Allure: Attached as Consolidated Accessibility Report
   Format: HTML report aggregating all accessibility checks from the scenario
   Contains:
   Summary statistics (total pages checked, violations, passes)
   All pages tested with their individual results
   Expandable sections for each page
   Collapsible violation details
   Cross-page accessibility overview
   How It Works
   During Test Execution
   Page Check: When AccessibilityHelper.Run() or AccessibilityHelper.RunAndTrack() is called:

Axe-core analyzes the page for WCAG violations
If violations are found, an HTML report is generated
The report is automatically attached to Allure with the page name
Test Cleanup: After the scenario completes:

A consolidated report is generated combining all page checks
The consolidated report is attached to Allure
Accessibility failures are summarized in the console
Viewing Reports in Allure
Run your tests as usual:

dotnet test
Generate Allure report:

allure serve allure-results
In the Allure report:

Navigate to your test scenario
Scroll to the Attachments section at the bottom
You'll see:
Individual page reports (if violations were found)
Consolidated Accessibility Report (always generated if accessibility checks ran)
Report Features
Interactive HTML Reports
Both individual and consolidated reports include:

✅ Color-coded severity levels

🔴 Critical (red)
🟠 Serious (orange)
🟡 Moderate (yellow)
🟢 Minor (green)
✅ Expandable sections - Click to expand/collapse details

✅ WCAG tags - Shows which WCAG criteria are violated

✅ Direct links - Links to Deque University for remediation guidance

✅ HTML snippets - Shows the exact HTML causing the issue

✅ CSS selectors - Provides selectors to locate failing elements

✅ Summary statistics - Overall metrics at a glance

Consolidated Report Benefits
The consolidated report provides:

Cross-page analysis: See which pages have issues at a glance
Trend identification: Identify common accessibility patterns
Quick navigation: Jump to specific pages with issues
Executive summary: Total violations, passes, and affected pages
File Locations
Reports are also saved to the file system:

Individual reports:

bin/Debug/net8.0/AccessibilityReport/{ScenarioName}/{PageName}/accessibility-report.html
Consolidated report:

bin/Debug/net8.0/AccessibilityReport/{ScenarioName}/consolidated-accessibility-report.html
JSON results:

bin/Debug/net8.0/AccessibilityReport/{ScenarioName}/{PageName}/results.json
Example Usage
In Your Step Definitions
[When(@"I navigate to the login page")]
public async Task WhenINavigateToTheLoginPage()
{
await _loginPage.NavigateAsync();

    // Run accessibility check and attach to Allure automatically
    await TrackAccessibilityAsync(Page);
}
What Gets Attached to Allure
For a scenario that checks 2 pages:

✅ Accessibility Report - www.example.com (individual page)
✅ Accessibility Report - login.example.com (individual page)
✅ Consolidated Accessibility Report (all pages summary)
Benefits
Centralized Reporting: All test results (functional + accessibility) in one place
Historical Tracking: Allure history shows accessibility improvements over time
Team Visibility: Developers and QA can see accessibility status in familiar reports
Easy Sharing: Share Allure reports with stakeholders including accessibility data
CI/CD Integration: Works seamlessly in your existing CI/CD pipeline
Console Output
During test execution, you'll see:

📊 Consolidated accessibility report generation completed
✅ Consolidated accessibility report attached to Allure
If accessibility checks pass:

✅ All functional tests passed
✅ All accessibility checks passed
If accessibility issues are found:

========================================
ACCESSIBILITY CHECK SUMMARY
========================================
✅ All functional tests passed
❌ Accessibility issues found on 2 page(s)
========================================
Configuration
The accessibility testing configuration is managed in your configuration files:

WCAG tags can be configured to focus on specific standards
Severity levels can be filtered
Reports are generated automatically - no additional configuration needed
Troubleshooting
If reports are not appearing in Allure:

Check console output - Look for warning messages about report generation
Verify file existence - Check if HTML reports are created in the file system
Check Allure version - Ensure Allure.Net.Commons is properly installed
Review permissions - Ensure write access to the AccessibilityReport directory
Next Steps
Review accessibility reports after each test run
Address critical and serious violations first
Use the remediation links to learn how to fix issues
Track progress over time using Allure's historical trends

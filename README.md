# Playwright .NET Test Automation Framework

> A comprehensive BDD test automation framework using ReqnRoll, Playwright, and C# with built-in accessibility testing and enterprise reporting.

[![.NET](https://img.shields.io/badge/.NET-8.0-blue.svg)](https://dotnet.microsoft.com/download)
[![Playwright](https://img.shields.io/badge/Playwright-.NET-green.svg)](https://playwright.dev/dotnet/)
[![ReqnRoll](https://img.shields.io/badge/ReqnRoll-BDD-orange.svg)](https://reqnroll.net/)

## 📋 Table of Contents
- [Overview](#overview)
- [Features](#features)
- [Prerequisites](#prerequisites)
- [Quick Start](#quick-start)
- [Project Structure](#project-structure)
- [Configuration](#configuration)
- [Running Tests](#running-tests)
- [Reports & Accessibility](#reports--accessibility)
- [Architecture](#architecture)
- [Contributing](#contributing)

---

## 🎯 Overview

- **[ReqnRoll](https://reqnroll.net/)** - BDD test scenarios in Gherkin syntax (SpecFlow successor)
- **[Playwright](https://playwright.dev/dotnet/)** - Modern, reliable browser automation for UI and API testing
- **[NUnit](https://nunit.org/)** - Powerful test framework with parallel execution
- **Allure** - Rich HTML reports with screenshots, traces, and accessibility results
- **Axe-core** - Automated WCAG accessibility compliance testing

The framework implements **Page Object Model**, **separation of concerns**, and **accessibility-first testing** approach.

---

## ✨ Features

✅ **BDD Support** - Write tests in business-readable Gherkin syntax  
✅ **Multi-Browser** - Chrome, Firefox, Safari, Edge support  
✅ **Mobile Emulation** - Test responsive designs with device emulation  
✅ **Accessibility Testing** - Automatic WCAG compliance checking with Axe-core  
✅ **Rich Reporting** - Allure reports with screenshots, traces, and accessibility results  
✅ **Parallel Execution** - Run tests in parallel for faster feedback  
✅ **Environment Management** - .env-based configuration for multiple environments  
✅ **API Testing** - Built-in support for API testing alongside UI tests  
✅ **CI/CD Ready** - Configured for GitHub Actions with artifact storage

---

## 📦 Prerequisites

- **.NET 8.0 SDK** or higher
- **JetBrains Rider 2024** (or Visual Studio 2022, VS Code)
- **Node.js 18+** (for accessibility testing with axe-html-reporter)
- **PowerShell** (for Playwright browser installation)
- **Git**

---

## 🚀 Quick Start

### 1. Clone Repository
```bash
git clone <repository-url>
cd playwrightDotNetSkeleton
```

### 2. Install Dependencies

```bash
# Restore .NET packages
dotnet restore

# Install Node.js dependencies for accessibility testing
npm install
```

This will install:
- `axe-html-reporter` - Generates detailed HTML reports for accessibility violations

### 3. Install Playwright Browsers

If you have PowerShell:
```bash
pwsh test/Playwright.E2ETests/bin/Debug/net8.0/playwright.ps1 install
```

If not, install PowerShell first:
```bash
brew install powershell/tap/powershell
```

Alternative with Go:
```bash
brew install go
go run github.com/playwright-community/playwright-go/cmd/playwright@latest install --with-deps
```

### 4. Configure Environment

Copy the example configuration:
```bash
cp .env.example .env
```

Edit `.env` with your local settings (see [Configuration](#configuration) section for details).

### 5. Build the Solution
```bash
dotnet build
```

### 7. View Reports
```bash
# Generate and open Allure report
cd test/Playwright.E2ETests/bin/Debug/net8.0
allure generate --clean
allure open allure-report
```

---

## 📁 Project Structure

The solution consists of one main project:

### `Playwright.E2ETests` - Main Test Project

```
test/Playwright.E2ETests/
├── Features/                    # BDD feature files (.feature)
│   ├── UI/                      # UI test scenarios
│   └── API/                     # API test scenarios
├── Pages/                       # Page Object Model classes
│   ├── ParaBankPages/          # ParaBank pages
│   ├── WPPages/                # WP.pl pages
│   └── BasePage.cs             # Base page with common functionality
├── Steps/                       # Step definitions (Gherkin → C#)
│   ├── UI/                      # UI step definitions
│   └── API/                     # API step definitions
├── Validators/                  # Assertion and validation classes
│   ├── AccessibilityValidator.cs
│   ├── ParaBankValidator.cs
│   └── WpPocztaValidator.cs
├── Model/                       # Data models and DTOs
├── DataBuilders/               # Test data builders (Builder pattern)
├── Context/                     # Test context classes
│   └── CustomTestContext.cs   # Data sharing between steps
├── Utils/                       # Utility classes and helpers
│   ├── AccessibilityHelper.cs  # Axe-core integration
│   └── AccessibilityPageEventListener.cs
├── Clients/                     # API service clients
├── Configuration/              # Configuration management
│   ├── BrowserConfiguration.cs
│   ├── CommonConfiguration.cs
│   └── ConfigurationManager.cs # Loads .env files
├── Hooks/                       # ReqnRoll hooks (setup/teardown)
│   ├── Hook.cs
│   ├── Setup/
│   └── Cleanup/
├── Browser/                     # Browser management
│   └── BrowserManager.cs
└── Resources/                   # Test resources (empty after .env migration)
```

---

## ⚙️ Configuration

The framework uses **`.env` files** for all configuration (migrated from JSON).

### Environment Files

- **`.env`** - Your local configuration (git-ignored, **never commit!**)
- **`.env.example`** - Template with all available options
- **`.env.uat`** - UAT environment configuration
- **`.env.staging`** - Staging environment (optional)
- **`.env.production`** - Production environment (optional)

### Configuration Variables

#### Environment Settings
```env
ENVIRONMENT=local                    # Environment name (local, uat, staging, production)
```

#### Browser Configuration
```env
# Option 1: Predefined browser configuration
BROWSER_CONFIGURATION=chrome         # chrome, firefox, safari, edge,
                                     # safari-iphone-13, safari-iphone-13-landscape
                                     # chrome-galaxy-s15, chrome-galaxy-s15-landscape

# Option 2: Custom browser configuration (overrides BROWSER_CONFIGURATION)
BROWSER=Chrome                       # Chrome, Firefox, Safari, Edge
DEVICE=                              # Empty for desktop, or device name (e.g., "iPhone 13")
VIEWPORT_WIDTH=1920                  # Browser width in pixels
VIEWPORT_HEIGHT=1080                 # Browser height in pixels
```

#### Common Settings
```env
SCREENSHOTS_DIRECTORY=Screenshots                    # Screenshot storage
ACCESSIBILITY_DIRECTORY=AccessibilityReport         # Accessibility reports
TRACES_DIRECTORY=Traces                             # Playwright traces
ACCESSIBILITY_TAGS=wcag2a,wcag2aa,wcag21a,wcag21aa # WCAG rules to check
HEADLESS=false                                      # Run in headless mode (true/false)
AUTO_ACCESSIBILITY_TRACKING=true                    # Auto accessibility checks
```

### Switching Environments

```bash
# Copy environment-specific file
cp .env.uat .env

# Or set environment variable
export ENVIRONMENT=uat
dotnet test
```

### Predefined Browser Configurations

| Configuration | Browser | Viewport/Device | Use Case |
|--------------|---------|----------------|----------|
| `chrome` | Chrome | 1920x1080 | Desktop testing |
| `firefox` | Firefox | 1920x1080 | Cross-browser |
| `safari` | Safari | 1920x1080 | macOS testing |
| `edge` | Edge | 1920x1080 | Windows testing |
| `safari-iphone-13` | Safari | iPhone 13 | Mobile iOS |
| `safari-iphone-13-landscape` | Safari | iPhone 13 landscape | Mobile iOS landscape |
| `chrome-galaxy-s15` | Chrome | Galaxy S9+ | Mobile Android |
| `chrome-galaxy-s15-landscape` | Chrome | Galaxy S9+ landscape | Mobile Android landscape |

### Configuration Security

⚠️ **Important Security Notes:**
- **NEVER commit `.env` or `.env.local`** to version control
- These files may contain sensitive credentials
- Only commit environment-specific files (`.env.uat`, `.env.staging`) if they don't contain secrets
- The `.env` file is already in `.gitignore`

---

## 🧪 Running Tests

### Basic Commands

```bash
# Run all tests
dotnet test

# Run specific category
dotnet test --filter "Category=smoke"
dotnet test --filter "Category=regression"
dotnet test --filter "Category=api"
dotnet test --filter "Category=ui"

# Run specific feature
dotnet test --filter "Name~ParaBankNavigation"
dotnet test --filter "Name~MessagesApi"
dotnet test --filter "Name~Senders"

# Run multiple categories (OR logic)
dotnet test --filter "Category=smoke|Category=regression"

# Exclude category
dotnet test --filter "Category!=wip"

# Parallel execution
dotnet test --parallel

# Headless mode
HEADLESS=true dotnet test
```

### Parallel Execution

Configure in `AssemblyInfo.cs` and `run.runsettings`:

```csharp
// AssemblyInfo.cs
[assembly: LevelOfParallelism(4)]
```

```xml
<!-- run.runsettings -->
<MaxCpuCount>4</MaxCpuCount>
```

Run with settings:
```bash
dotnet test --settings run.runsettings
```

### Environment-Specific Execution

```bash
# UAT environment
cp .env.uat .env
dotnet test
```

---


## 📊 Reports & Accessibility

### Allure Reports

Rich HTML reports with comprehensive test results:

✅ **Test execution results** - Pass/Fail status with detailed steps  
✅ **Screenshots on failures** - Automatic screenshot capture  
✅ **Playwright traces** - Full browser interaction recording  
✅ **Accessibility scan results** - WCAG compliance reports  
✅ **Execution timeline** - Test duration and sequence  
✅ **Trends and statistics** - Historical data and metrics  

#### Generate and View Reports

```bash
# After test execution
cd test/Playwright.E2ETests/bin/Debug/net8.0

# Generate fresh report
allure generate --clean

# Open report in browser
allure open allure-report

# Or generate and serve in one command
allure serve allure-results
```

**Report Locations:**
- Allure results: `bin/Debug/net8.0/allure-results/`
- Generated report: `bin/Debug/net8.0/allure-report/`

### Accessibility Testing (MANDATORY)

**Automatic WCAG compliance checking** is integrated using Axe-core.

#### When to Track Accessibility

**REQUIRED** after:
- ✅ Page navigation
- ✅ Modal opening
- ✅ Dynamic content loading
- ✅ Significant page state changes

#### How to Track Accessibility

```csharp
// After navigation
await _page.GotoAsync(url);
await _page.TrackAccessibilityAsync(_page);

// After modal opens
await _openModalButton.ClickAsync();
await _page.TrackAccessibilityAsync(_page);

// After dynamic content loads
await _page.WaitForSelectorAsync(".dynamic-content");
await _page.TrackAccessibilityAsync(_page);
```

#### WCAG Compliance Levels

The framework checks against:
- **WCAG 2.0 Level A** (`wcag2a`)
- **WCAG 2.0 Level AA** (`wcag2aa`)
- **WCAG 2.1 Level A** (`wcag21a`)
- **WCAG 2.1 Level AA** (`wcag21aa`)

Configure via `ACCESSIBILITY_TAGS` in `.env` file.

#### Accessibility Reports

**Individual Page Reports:**
- Location: `bin/Debug/net8.0/AccessibilityReport/{Scenario}/{Page}/`
- Format: Interactive HTML with detailed violations
- Attached to Allure as: `Accessibility Report - {page_name}`

**Consolidated Reports:**
- Location: `bin/Debug/net8.0/AccessibilityReport/{Scenario}/consolidated-accessibility-report.html`
- Contains: Summary of all pages checked in a scenario
- Attached to Allure as: `Consolidated Accessibility Report`

**Report Features:**
- 🔴 Color-coded severity (Critical, Serious, Moderate, Minor)
- 📋 WCAG tags and criteria violated
- 🔍 Affected HTML elements with CSS selectors
- 🛠️ Remediation guidance with links to Deque University
- 📊 Summary statistics and trends
- 🔽 Expandable/collapsible sections

**Console Output:**
```
📊 Consolidated accessibility report generation completed
✅ Consolidated accessibility report attached to Allure

========================================
ACCESSIBILITY CHECK SUMMARY
========================================
✅ All functional tests passed
❌ Accessibility issues found on 2 page(s)
========================================
```

**Note:** Accessibility violations are **reported** but **don't fail tests** by default.

### Playwright Traces

Full browser traces for debugging captured automatically on test failure:

**Trace Contents:**
- Network activity and requests
- Console logs and errors
- DOM snapshots at each step
- Screenshots of actions
- Action timeline with timing

**Viewing Traces:**
```bash
# Traces saved to
bin/Debug/net8.0/Traces/

# View online
# Navigate to: https://trace.playwright.dev/
# Drag and drop trace.zip file
```

**Traces are automatically:**
- Saved to `Traces/` directory
- Attached to failed tests in Allure
- Compressed for efficient storage

---


## 🏗️ Architecture

### Page Object Model (POM)

UI tests use the Page Object pattern for maintainability:

```csharp
public class LoginPage : BasePage
{
    private readonly ILocator _usernameField;
    private readonly ILocator _passwordField;
    private readonly ILocator _loginButton;

    public LoginPage(IPage page) : base(page)
    {
        // Initialize locators using stable selectors
        _usernameField = Page.GetByLabel("Username");
        _passwordField = Page.GetByLabel("Password");
        _loginButton = Page.GetByRole(AriaRole.Button, new() { Name = "Log In" });
    }

    public async Task LoginAsync(string username, string password)
    {
        await _usernameField.FillAsync(username);
        await _passwordField.FillAsync(password);
        await _loginButton.ClickAsync();
        
        // Mandatory accessibility tracking after action
        await TrackAccessibilityAsync(Page);
    }
}
```

**Page Object Principles:**
- ✅ One class per unique URL
- ✅ All locators as `ILocator` fields
- ✅ Methods represent user actions (not assertions)
- ✅ Inherit from `BasePage`
- ✅ Use accessibility-first selectors: `data-testid` > `role` > `label` > CSS

### Step Definitions

BDD step definitions bind Gherkin to code:

```csharp
[Binding]
public class LoginSteps : BaseUiStepDefinitions
{
    private readonly LoginPage _loginPage;

    public LoginSteps(CustomTestContext context, IPage page) : base(context, page)
    {
        _loginPage = new LoginPage(page);
    }

    [When(@"I log in with username ""(.*)"" and password ""(.*)""")]
    public async Task WhenILogIn(string username, string password)
    {
        await _loginPage.LoginAsync(username, password);
    }

    [Then(@"I should see the dashboard")]
    public async Task ThenIShouldSeeTheDashboard()
    {
        await LoginValidator.ValidateDashboardDisplayed(Page);
    }
}
```

**Step Definition Principles:**
- ✅ One class per feature
- ✅ Use `CustomTestContext` for data sharing
- ✅ Keep steps independent and atomic
- ✅ No assertions in step definitions (use Validators)

### Validators

Separate assertion logic for clarity:

```csharp
public class LoginValidator
{
    public static async Task ValidateDashboardDisplayed(IPage page)
    {
        var welcomeMessage = page.GetByText("Welcome");
        var dashboard = page.GetByRole(AriaRole.Main);
        
        // Playwright assertions
        await Expect(welcomeMessage).ToBeVisibleAsync();
        await Expect(dashboard).ToContainTextAsync("Dashboard");
    }

    public static void ValidateUserData(User expected, User actual)
    {
        // NUnit/FluentAssertions for data
        actual.Username.Should().Be(expected.Username);
        actual.Email.Should().Be(expected.Email);
    }
}
```

**Validator Principles:**
- ✅ Static methods for easy access
- ✅ Playwright `Expect` for web elements
- ✅ NUnit `Assert` or `FluentAssertions` for data
- ✅ Grouped by domain (Login, Messages, etc.)

### Data Builders

Builder pattern for test data creation:

```csharp
public class UserBuilder
{
    private string _username = "testuser";
    private string _password = "Test123!";
    private string _email = "test@example.com";

    public UserBuilder WithUsername(string username)
    {
        _username = username;
        return this;
    }

    public UserBuilder WithPassword(string password)
    {
        _password = password;
        return this;
    }

    public UserBuilder WithEmail(string email)
    {
        _email = email;
        return this;
    }

    public User Build() => new User
    {
        Username = _username,
        Password = _password,
        Email = _email
    };
}

// Usage
var user = new UserBuilder()
    .WithUsername("john.doe")
    .WithEmail("john@example.com")
    .Build();
```

### API Service Layer

Structured API client pattern:

```csharp
public class MessagesApiService : BaseApiService
{
    public MessagesApiService(string baseUrl, string apiKey)
        : base(baseUrl)
    {
        _httpClient.DefaultRequestHeaders.Add("X-API-Key", apiKey);
    }

    public async Task<List<Message>> GetMessagesAsync()
    {
        var response = await _httpClient.GetAsync("/users/me/messages");
        response.EnsureSuccessStatusCode();
        return await DeserializeAsync<List<Message>>(response);
    }

    public async Task<Message> CreateMessageAsync(CreateMessageRequest request)
    {
        var content = SerializeToJson(request);
        var response = await _httpClient.PostAsync("/users/me/messages", content);
        response.EnsureSuccessStatusCode();
        return await DeserializeAsync<Message>(response);
    }
}
```

### Test Context

Share data between steps using `CustomTestContext`:

```csharp
public class CustomTestContext
{
    public IPage Page { get; set; }
    public IBrowser Browser { get; set; }
    public Dictionary<string, object> ScenarioData { get; } = new();

    public T Get<T>(string key) => (T)ScenarioData[key];
    public void Set(string key, object value) => ScenarioData[key] = value;
}

// Usage in steps
_context.Set("createdMessage", message);
var message = _context.Get<Message>("createdMessage");
```

---

## 🤝 Contributing

### Code Standards

✅ Follow C# coding conventions  
✅ Use meaningful names (Page Objects, Steps, Validators)  
✅ Keep scenarios independent and atomic  
✅ **Add accessibility tracking to all page interactions**  
✅ Write comprehensive commit messages  

### Pull Request Process

1. Create feature branch from `main`
2. Write tests in BDD format (Given/When/Then)
3. Ensure all tests pass locally
4. Run accessibility checks
5. Generate Allure report
6. Submit PR with test report screenshots
7. Address review comments

### Testing Standards

| Component | Guideline | Example |
|-----------|-----------|---------|
| **Page Objects** | One class per unique URL | `LoginPage`, `DashboardPage` |
| **Step Definitions** | One class per feature | `LoginSteps`, `MessagesSteps` |
| **Validators** | Separate assertion logic | `LoginValidator.ValidateSuccess()` |
| **Accessibility** | Mandatory after page changes | `await TrackAccessibilityAsync(page)` |
| **Selectors** | Priority order | `data-testid` > `role` > `label` > CSS |
| **Test Data** | Use builders | `new UserBuilder().WithEmail().Build()` |

### Coding Conventions

**Naming:**
```csharp
// Page Objects: [Feature]Page
public class LoginPage : BasePage

// Steps: [Feature]Steps
public class LoginSteps : BaseUiStepDefinitions

// Validators: [Feature]Validator
public class LoginValidator

// Builders: [Domain]Builder  
public class UserBuilder
```

**File Organization:**
- Group related classes together
- One public class per file
- Match filename to class name
- Use folders for domain grouping

---

## 🔧 Troubleshooting

### Common Issues

**❌ Browsers not installed:**
```bash
pwsh test/Playwright.E2ETests/bin/Debug/net8.0/playwright.ps1 install
```

**❌ Tests timing out:**
- Increase timeout in `.runsettings`
- Check network connectivity
- Verify application is accessible
- Check for slow database queries

**❌ Accessibility failures:**
- Review `AccessibilityReport/` directory
- Check Allure report attachments
- Violations don't fail tests by default
- Use remediation links for fixes

**❌ Configuration not loading:**
- Verify `.env` exists in workspace root
- Check variable names match exactly (case-sensitive)
- Ensure format: `KEY=value` (no spaces around `=`)
- Check for trailing whitespace

**❌ Parallel execution issues:**
- Reduce `LevelOfParallelism` in `AssemblyInfo.cs`
- Check for shared state between tests
- Ensure tests are truly independent
- Use `CustomTestContext` for isolation

**❌ NuGet restore fails:**
```bash
dotnet clean
dotnet restore --force
dotnet build
```

**❌ Allure report not generating:**
```bash
# Check Allure is installed
allure --version

# Install if needed
npm install -g allure-commandline

# Regenerate report
cd test/Playwright.E2ETests/bin/Debug/net8.0
rm -rf allure-report
allure generate --clean
```

---

## 📚 Additional Resources

### Documentation
- [ReqnRoll Documentation](https://reqnroll.net/) - BDD framework
- [Playwright .NET Docs](https://playwright.dev/dotnet/) - Browser automation
- [Allure Framework](https://docs.qameta.io/allure/) - Test reporting
- [Axe-core Rules](https://github.com/dequelabs/axe-core/blob/develop/doc/rule-descriptions.md) - Accessibility rules
- [WCAG Guidelines](https://www.w3.org/WAI/WCAG21/quickref/) - Accessibility standards
- [NUnit Documentation](https://docs.nunit.org/) - Test framework

### Internal Documentation
- **MCPContext.md** - Detailed project standards and guidelines
- **.github/copilot-instructions.md** - AI assistant guidelines
- **.github/instructions/** - Path-specific development instructions

### Learning Resources
- [C# Best Practices](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/)
- [Playwright Best Practices](https://playwright.dev/docs/best-practices)
- [BDD with Gherkin](https://cucumber.io/docs/gherkin/)
- [Accessibility Testing Guide](https://www.w3.org/WAI/test-evaluate/)

---

## 📊 Key Dependencies

### Core Testing Framework
- **Reqnroll** (2.3.0) - BDD framework (SpecFlow successor)
- **NUnit** (3.14.0) - Test runner
- **Playwright** (1.47.0) - Browser automation
- **Playwright.Axe** (1.3.0) - Accessibility testing

### Assertion and Validation
- **FluentAssertions** (6.12.1) - Fluent assertion library
- **Then** (1.0.3) - Additional assertion extensions

### Reporting
- **Allure.Reqnroll** (2.12.1) - Allure integration

### External Integrations  
- **MongoDB.Driver** - Database client
- **Azure.Identity** (1.12.1) - Azure authentication
- **Otp.NET** (1.4.0) - OTP generation

### Utilities
- **DotNetEnv** (3.1.1) - .env file support
- **Newtonsoft.Json** - JSON serialization
- **Microsoft.Extensions.Configuration** - Configuration management

For complete dependencies, see `Playwright.E2ETests.csproj`.

---


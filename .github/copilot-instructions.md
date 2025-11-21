# GitHub Copilot Instructions - Playwright .NET Test Automation Project

## Project Overview

This is a production-ready test automation project using **ReqnRoll BDD framework with C# and NUnit** for end-to-end testing with Playwright, featuring built-in accessibility testing and comprehensive reporting.

## Core Technologies
- **Framework**: ReqnRoll 2.3.0 BDD (SpecFlow successor)
- **Test Runner**: NUnit 3.14.0
- **Browser Automation**: Playwright 1.47.0 for .NET
- **Language**: C# (.NET 8.0)
- **Configuration**: DotNetEnv (.env files)
- **Reporting**: Allure with Axe-core accessibility reports
- **Assertions**: FluentAssertions 6.12.1

## Project Structure
```
test/Playwright.E2ETests/
  ├── Features/                    // BDD Feature files (.feature)
  │   ├── UI/                      // UI test scenarios
  │   └── API/                     // API test scenarios (future)
  ├── Pages/                       // Page Object Model classes
  │   └── BasePage.cs             // Base page with common functionality
  ├── Steps/                       // Step definitions
  │   ├── UI/                      // UI step definitions
  ├── Validators/                  // Assertion classes
  ├── Model/                       // Data model classes
  ├── DataBuilders/               // Test data builders (Builder pattern)
  ├── Context/                     // Test context classes
  │   └── CustomTestContext.cs    // Share data between steps
  ├── Configuration/              // Configuration management
  │   ├── BrowserConfiguration.cs
  │   ├── CommonConfiguration.cs
  │   └── ConfigurationManager.cs // Loads .env files
  ├── Utils/                       // Utility classes
  │   ├── AccessibilityHelper.cs  // Axe-core integration
  │   └── AccessibilityPageEventListener.cs
  ├── Clients/                     // API service clients
  ├── Hooks/                       // ReqnRoll hooks
  │   ├── Setup/
  │   └── Cleanup/
  └── Browser/                     // Browser management
      └── BrowserManager.cs
```

## Architecture Patterns

### Configuration Management (.env)
- **Uses .env files** instead of JSON configuration
- `ConfigurationManager.cs` loads environment variables via DotNetEnv
- Configuration classes: `BrowserConfiguration`, `CommonConfiguration`
- Environment files:
  - `.env` - Local config (git-ignored, never commit!)
  - `.env.example` - Template
  - `.env.uat` - UAT environment
- All configuration variables defined in `.env.example`
- Access config via: `ConfigurationManager.GetCommon()` or `ConfigurationManager.GetBrowser()`

### Page Object Pattern
- **Each unique URL requires separate page object class**
- All page objects inherit from `BasePage`
- Methods represent user actions (not assertions)
- Locators initialized in constructor as `ILocator` private readonly fields
- Use underscore prefix for locator fields: `_submitButton`
- Selector priority: `data-testid` > `role` > `label` > CSS

**Example:**
```csharp
public class LoginPage : BasePage
{
    private readonly ILocator _usernameField;
    private readonly ILocator _loginButton;

    public LoginPage(IPage page) : base(page)
    {
        _usernameField = page.GetByLabel("Username");
        _loginButton = page.GetByRole(AriaRole.Button, new() { Name = "Log In" });
    }

    public async Task LoginAsync(string username, string password)
    {
        await _usernameField.FillAsync(username);
        await _loginButton.ClickAsync();
        await TrackAccessibilityAsync(Page); // REQUIRED!
    }
}
```

### BDD with ReqnRoll
- User journeys written in Gherkin syntax (.feature files)
- Step definitions bind scenarios to code
- Use `CustomTestContext` to share data between steps
- Keep scenarios independent and atomic
- Tag scenarios with `@tags` for filtering (e.g., `@smoke`, `@regression`, `@ParaBank`)
- Steps inherit from `BaseUiStepDefinitions` or `BaseStepDefinitions`

**Example:**
```csharp
[Binding]
public class LoginSteps : BaseUiStepDefinitions
{
    private readonly LoginPage _loginPage;

    public LoginSteps(CustomTestContext context, IPage page) : base(context, page)
    {
        _loginPage = new LoginPage(page);
    }

    [When(@"I log in as ""(.*)""")]
    public async Task WhenILogInAs(string username)
    {
        await _loginPage.LoginAsync(username, "password");
    }
}
```

### Separation of Concerns
- **Page Objects**: User interactions with pages (NO assertions)
- **Step Definitions**: Bind Gherkin steps to code (minimal logic)
- **Validators**: All assertions
  - Use Playwright `Expect` for web elements: `await Expect(element).ToBeVisibleAsync()`
  - Use NUnit `Assert` or `FluentAssertions` for data: `actual.Should().Be(expected)`
- **Models**: Data structures from UI/API
- **DataBuilders**: Test data creation using builder pattern
- **Configuration**: Environment-specific settings via `.env` files

**Validator Example:**
```csharp
public class LoginValidator
{
    public static async Task ValidateSuccessfulLogin(IPage page)
    {
        var welcomeMessage = page.GetByText("Welcome");
        await Expect(welcomeMessage).ToBeVisibleAsync();
    }
}
```

## Key Principles
- **Always check if components (PageObject, Steps, Validators) exist before creating new ones**
- Use Playwright MCP to explore pages before implementation
- Prefer stable selectors (data-testid, roles, labels) over fragile CSS selectors
- Keep tests independent and atomic
- Follow accessibility-first approach with mandatory WCAG checks
- Use `.env` for all configuration (never hardcode URLs, credentials, etc.)
- One Page Object per unique URL
- No assertions in Page Objects or Step Definitions (use Validators)
- Use `CustomTestContext` to share data between steps in a scenario

## Running Tests

### Basic Commands
```bash
# Run all tests
dotnet test

# Run with specific tag
dotnet test --filter "Category=smoke"
dotnet test --filter "Category=ParaBank"

# Run specific feature
dotnet test --filter "Name~ParaBankNavigation"

# Multiple tags (OR)
dotnet test --filter "Category=smoke|Category=regression"

# Exclude tag
dotnet test --filter "Category!=wip"
```


## Path-Specific Instructions
Detailed instructions for specific directories are in `.github/instructions/`:
- **Pages**: `pages.instructions.md`
- **Steps**: `steps.instructions.md`
- **Validators**: `validators.instructions.md`
- **Features**: `features.instructions.md`
- **Models**: `model.instructions.md`
- **DataBuilders**: `databuilders.instructions.md`

## Shell Command Execution
When executing commands using `run_in_terminal`:
- Always explicitly show the command being executed in a code block before running it
- Format: "Running: `command here`" or show in bash code block
- This ensures transparency - users can see exactly what commands are being executed
- Example:
  ```
  Running:
  ```bash
  dotnet test test/Playwright.E2ETests/Playwright.E2ETests.csproj --filter "Name~ParaBank"
  ```
  Then execute and share results.

## Terminal Command Guidelines

When running terminal commands in this project:
- Prefer explicit output over silent operations
- Use `--verbose` flags when available
- For long operations, set `isBackground: true`
- Break down operations into smaller steps if timeout occurs

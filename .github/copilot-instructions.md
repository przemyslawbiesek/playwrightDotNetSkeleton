# GitHub Copilot Instructions - Test Automation Project

## Project Overview

This is a test automation project using **ReqnRoll BDD framework with C# and NUnit** for end-to-end testing with Playwright.

## Core Technologies
- **Framework**: ReqnRoll BDD (SpecFlow successor)
- **Test Runner**: NUnit
- **Browser Automation**: Playwright for .NET
- **Language**: C# (.NET)
- **Reporting**: Allure with accessibility reports

## Project Structure
```
test/Playwright.E2ETests/
  ├── Features/            // BDD Feature files (.feature)
  │   ├── UI/             // UI test scenarios
  │   └── API/            // API test scenarios
  ├── Pages/              // Page Object classes
  │   └── NhsWalesPages/  
  ├── Steps/              // Step definitions
  │   ├── UI/             // UI step definitions
  │   └── API/            // API step definitions
  ├── Validators/         // Assertion classes
  ├── Model/              // Data model classes
  ├── DataBuilders/       // Test data builders
  ├── Context/            // Test context classes
  ├── Utils/              // Utility classes
  └── Clients/            // API service clients
```

## Architecture Patterns

### Page Object Pattern
- Each unique URL requires separate page object class
- All page objects inherit from `BasePage`
- Methods represent user actions (not assertions)
- Locators initialized in constructor as `ILocator` fields

### BDD with ReqnRoll
- User journeys written in Gherkin syntax (.feature files)
- Step definitions bind scenarios to code
- Use `CustomTestContext` to share data between steps
- Keep scenarios independent and atomic

### Separation of Concerns
- **Page Objects**: User interactions with pages
- **Step Definitions**: Bind Gherkin steps to code
- **Validators**: All assertions (Playwright Expect for web, NUnit Assert for data)
- **Models**: Data structures from UI/API
- **DataBuilders**: Test data creation using builder pattern

## Key Principles
- Always check if components (PageObject, Steps) exist before creating new ones
- Use Playwright MCP to explore pages before implementation
- Prefer stable selectors (data-testid, roles) over fragile CSS selectors
- Keep tests independent and atomic
- Follow accessibility-first approach with mandatory WCAG checks

## Accessibility Testing (REQUIRED)
- Add `await TrackAccessibilityAsync(Page)` after:
  - Page navigation
  - Modal opening
  - Dynamic content loading
  - Significant page state changes
- Axe-core WCAG reports automatically generated and attached to Allure
- Accessibility failures reported but don't fail functional tests

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

Common commands and expected duration:
- `npm install` - 10-30 seconds
- `npm run dev` - Background process
- `npm run build` - 30-90 seconds
- `git status` - <1 second

## Additional Resources
For complete detailed standards, see: `test/Playwright.E2ETests/MCPContext.md`

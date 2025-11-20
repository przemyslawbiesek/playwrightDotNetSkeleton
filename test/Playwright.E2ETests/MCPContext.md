MCP Context - Test Automation Project Standards
================================================

PROJECT REQUIREMENTS:
- Tests are added using ReqnRoll BDD framework with C#
- Tests are executed with NUnit test runner
- Use current project structure and check if PageObject already exists before creating new ones
- Use Page Object Pattern: each page with different URL requires a new page object class
- All assertions should be placed in the Validators package/folder
- New user journeys should be added as new feature files
- All web element locators must be defined as ILocator objects
- Prefer selectors that are stable and meanigfull (e.g., data-testid attributes)
- Keep tests independent and atomic
- When extracting data from web elements, use models when applicable


FOLDER STRUCTURE:
test/Playwright.E2ETests/
  ├── Features/                    // BDD Feature files (.feature)
  │   └── UI/                      // UI test scenarios
  ├── Pages/NhsWalesPages/         // Page Object classes
  ├── Steps/                       // Step definition classes
  │   ├── UI/                      // UI step definitions  
  │   └── Api/                     // API step definitions
  ├── Validators/                  // Assertion classes
  ├── Model/                       // Data model classes
  ├── Context/                     // Test context classes
  └── Utils/                       // Utility classes

PAGE OBJECT PATTERN:
- Each unique URL/page should have its own page object class
- All locators must be defined as ILocator fields
- Initialize locators in constructor
- Methods should represent user actions on the page

VALIDATION PATTERN:
- Create static validator classes for each domain/feature
- Use Playwright Expect assertions for web-specific validations (auto-retry and wait)
- Use NUnit Assert methods for data/object comparisons
- Provide descriptive error messages

ASSERTION EXAMPLES:
// Playwright assertions (recommended for web elements)
await Expect(page).ToHaveURLAsync("https://example.com/dashboard");
await Expect(locator).ToHaveTextAsync("Expected Text");
await Expect(locator).ToBeVisibleAsync();

// NUnit assertions (for data validation)
Assert.AreEqual(expected, actual, "Values should match");
Assert.IsNotNull(object, "Object should not be null");

BDD PATTERN:
- Write scenarios in Gherkin syntax in .feature files
- Create step definitions that call page object methods
- Use CustomTestContext to share data between steps

EXAMPLE PAGE OBJECT:
public class LoginPage : BasePage
{
    private readonly ILocator _usernameField;
    private readonly ILocator _passwordField;
    private readonly ILocator _loginButton;

    public LoginPage(IPage page) : base(page)
    {
        _usernameField = page.Locator("#username");
        _passwordField = page.Locator("#password");
        _loginButton = page.Locator("button[type='submit']");
    }

    public async Task LoginAsync(string username, string password)
    {
        await _usernameField.FillAsync(username);
        await _passwordField.FillAsync(password);
        await _loginButton.ClickAsync();
    }
}
 
EXAMPLE VALIDATOR:
public static class LoginValidator
{
    // Playwright assertion for web validation
    public static async Task ValidateLoginSuccessAsync(IPage page, string expectedUrl)
    {
        await Expect(page).ToHaveURLAsync(expectedUrl);
    }
    
    // NUnit assertion for data validation
    public static void ValidateUserData(string actualName, string expectedName)
    {
        Assert.AreEqual(expectedName, actualName, 
            $"Expected user name to be {expectedName}, but was {actualName}");
    }
}

EXAMPLE STEP DEFINITION:
[Given(@"I am on the login page")]
public async Task GivenIAmOnTheLoginPage()
{
    await _loginPage.NavigateAsync();
}

[When(@"I login with username ""(.*)"" and password ""(.*)""")]
public async Task WhenILoginWithCredentials(string username, string password)
{
    await _loginPage.LoginAsync(username, password);
}

[Then(@"I should be logged in successfully")]
public async Task ThenIShouldBeLoggedIn()
{
    await LoginValidator.ValidateLoginSuccessAsync(Page, "https://app.com/dashboard");
}

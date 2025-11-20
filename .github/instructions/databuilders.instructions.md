# Data Builders Instructions

## Applies to: `test/Playwright.E2ETests/DataBuilders/**`

### Data Builder Pattern Standards

#### Builder Class Structure
```csharp
public class UserDataBuilder
{
    private string _username = "defaultUser";
    private string _email = "user@example.com";
    private string _firstName = "John";
    private string _lastName = "Doe";

    public UserDataBuilder WithUsername(string username)
    {
        _username = username;
        return this;
    }

    public UserDataBuilder WithEmail(string email)
    {
        _email = email;
        return this;
    }

    public UserModel Build()
    {
        return new UserModel
        {
            Username = _username,
            Email = _email,
            FirstName = _firstName,
            LastName = _lastName
        };
    }
}
```

### Key Rules
- Use Builder pattern for test data creation
- Provide sensible defaults for all fields
- Use fluent API with `With*` methods returning `this`
- End with `Build()` method that creates the model
- Name classes with `*DataBuilder` suffix

### Usage Example
```csharp
var user = new UserDataBuilder()
    .WithUsername("testuser")
    .WithEmail("test@example.com")
    .Build();
```

### Existing Builders
- `AppointmentsDataBuilder` - Appointment data
- `MessageDataBuilder` - Message data
- `MongoDataBuilder` - MongoDB test data
- `PrescriptionDataBuilder` - Prescription data

### Best Practices
- Keep builders focused on single domain entity
- Use builders in step definitions for test data setup
- Provide multiple `With*` methods for flexibility
- Consider random data generation for unique values
# Page Object Instructions

## Applies to: `test/Playwright.E2ETests/Pages/**`

### Page Object Pattern Standards

#### Class Structure
```csharp
public class ExamplePage : BasePage
{
    private readonly ILocator _usernameField;
    private readonly ILocator _submitButton;

    public ExamplePage(IPage page) : base(page)
    {
        _usernameField = page.Locator("[data-testid='username']");
        _submitButton = page.GetByRole(AriaRole.Button, new() { Name = "Submit" });
    }

    public async Task FillUsernameAsync(string username)
    {
        await _usernameField.FillAsync(username);
    }
}
```

### Key Rules
- **Always check if PageObject exists before creating new ones**
- Initialize all locators in constructor as `ILocator` fields
- Methods should represent user actions on the page (not assertions)
- Use private readonly fields with underscore prefix for locators
- Each unique URL requires separate page object class
- Inherit from `BasePage`

### Selector Preferences (in priority order)
1. **data-testid attributes**: `page.Locator("[data-testid='submit-button']")`
2. **Role-based selectors**: `page.GetByRole(AriaRole.Button, new() { Name = "Submit" })`
3. **Semantic selectors**: `page.GetByLabel("Username")`
4. **CSS selectors**: `page.Locator("#submit-btn")`

### Method Naming
- Use descriptive async method names ending with `Async`
- Examples: `FillUsernameAsync()`, `ClickSubmitButtonAsync()`, `NavigateAsync()`
- Methods should be atomic and represent single user actions


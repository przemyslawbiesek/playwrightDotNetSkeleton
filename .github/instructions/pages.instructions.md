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

### Accessibility Tracking (MANDATORY)
**REQUIRED** - Add `await TrackAccessibilityAsync(Page)` after:
- Page navigation
- Modal/dialog opening
- Dynamic content loading
- Significant page state changes

```csharp
public async Task NavigateAsync()
{
    await Page.GotoAsync("https://example.com");
    await TrackAccessibilityAsync(Page); // REQUIRED!
}

public async Task OpenModalAsync()
{
    await _openModalButton.ClickAsync();
    await TrackAccessibilityAsync(Page); // REQUIRED!
}
```

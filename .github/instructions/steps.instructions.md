# Step Definitions Instructions

## Applies to: `test/Playwright.E2ETests/Steps/**`

### Step Definition Pattern

#### UI Step Definitions
```csharp
[Binding]
public class ExampleSteps : BaseUiStepDefinitions
{
    private readonly ExamplePage _examplePage;

    public ExampleSteps(CustomTestContext context, IPage page, ExamplePage examplePage) 
        : base(context, page)
    {
        _examplePage = examplePage;
    }

    [Given(@"I am on the example page")]
    public async Task GivenIAmOnTheExamplePage()
    {
        await _examplePage.NavigateAsync();
        
        // IMPORTANT: Always run accessibility check after page navigation
        await TrackAccessibilityAsync(Page);
    }
}
```

#### API Step Definitions
```csharp
[Binding]
public class ApiExampleSteps : BaseStepDefinitions
{
    public ApiExampleSteps(CustomTestContext context) : base(context)
    {
    }
    
    // API step implementation
}
```

### Key Rules
- **Check if step definitions already exist before creating new ones**
- UI steps inherit from `BaseUiStepDefinitions`
- API steps inherit from `BaseStepDefinitions`
- Use `CustomTestContext` to share data between steps
- Use ReqnRoll `[Binding]` attribute on classes
- Use Gherkin step attributes: `[Given]`, `[When]`, `[Then]`

### Accessibility Testing - REQUIRED
**Always add accessibility checks after:**
- Page navigation
- Modal opening
- Dynamic content loading
- Tab switching
- Significant page state changes

```csharp
await TrackAccessibilityAsync(Page);
```

**Key Points:**
- `TrackAccessibilityAsync(Page)` runs Axe-core WCAG compliance checks
- Automatically generates HTML reports with detailed violations
- Reports are attached to Allure (individual page + consolidated)
- Accessibility failures are reported at end of scenario (won't fail functional tests)

### Data Context
- Use `Context` property to store/retrieve data between steps
- Store complex objects in context for later validation


# Validators Instructions

## Applies to: `test/Playwright.E2ETests/Validators/**`

### Validator Pattern Standards

#### Class Structure
```csharp
public static class ExampleValidator
{
    // Use Playwright assertions for web elements (auto-retry/wait)
    public static async Task ValidateElementVisibleAsync(ILocator element)
    {
        await Expect(element).ToBeVisibleAsync();
    }
    
    // Use NUnit assertions for data validation
    public static void ValidateUserData(string actual, string expected)
    {
        Assert.AreEqual(expected, actual, "User data should match");
    }
}
```

### Key Rules
- **All assertions must be placed in Validators package/folder**
- Validators are static classes with static methods
- Use Playwright `Expect` assertions for web element validations (provides auto-retry/wait)
- Use NUnit `Assert` for data/model validations
- Never put assertions in Page Objects or Step Definitions

### Assertion Types

#### Playwright Assertions (for web elements)
```csharp
await Expect(element).ToBeVisibleAsync();
await Expect(element).ToHaveTextAsync("expected text");
await Expect(element).ToBeEnabledAsync();
await Expect(element).ToHaveCountAsync(5);
```

#### NUnit Assertions (for data)
```csharp
Assert.AreEqual(expected, actual, "message");
Assert.IsTrue(condition, "message");
Assert.IsNotNull(value, "message");
Assert.That(actual, Is.EqualTo(expected));
```

### Method Naming
- Use descriptive method names starting with `Validate`
- Async methods end with `Async`
- Examples: `ValidatePageTitleAsync()`, `ValidateUserData()`


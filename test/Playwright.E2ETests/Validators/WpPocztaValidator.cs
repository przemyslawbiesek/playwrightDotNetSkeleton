using NUnit.Framework;

namespace Playwright.E2ETests.Validators;

public static class WpPocztaValidator
{
    public static void ValidateLoginPageDisplayed(bool isDisplayed)
    {
        Assert.That(isDisplayed, Is.True, "WP Poczta login page should be displayed");
    }

    public static void ValidateEmailFieldVisible(bool isVisible)
    {
        Assert.That(isVisible, Is.True, "Email address field should be visible");
    }

    public static void ValidatePasswordFieldVisible(bool isVisible)
    {
        Assert.That(isVisible, Is.True, "Password field should be visible");
    }

    public static void ValidateLoginButtonVisible(bool isVisible)
    {
        Assert.That(isVisible, Is.True, "Login button should be visible");
    }

    public static void ValidateCreateAccountLinkVisible(bool isVisible)
    {
        Assert.That(isVisible, Is.True, "Create new account link should be visible");
    }

    public static void ValidatePageTitle(string? actualTitle)
    {
        Assert.That(actualTitle, Is.Not.Null.And.Not.Empty, "Page title should not be null or empty");
        Assert.That(actualTitle, Does.Contain("Poczta"),
            $"Expected page title to contain 'Poczta', but got '{actualTitle}'");
    }

    public static void ValidateRegistrationPageDisplayed(bool isDisplayed, string currentUrl)
    {
        Assert.That(isDisplayed, Is.True,
            $"Expected registration page to be displayed but got false. Current URL is: {currentUrl}");
    }
}


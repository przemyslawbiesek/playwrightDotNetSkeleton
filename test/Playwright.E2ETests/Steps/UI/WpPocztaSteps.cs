using Microsoft.Playwright;
using Playwright.E2ETests.Context;
using Playwright.E2ETests.Pages.NhsWalesPages;
using Playwright.E2ETests.Utils;
using Playwright.E2ETests.Validators;
using Reqnroll;

namespace Playwright.E2ETests.Steps.UI;

[Binding]
public class WpPocztaSteps : BaseUiStepDefinitions
{
    private WpPocztaLoginPage _wpPocztaLoginPage;
    private WpPocztaRegistrationPage _wpPocztaRegistrationPage;
    private readonly WpHomePage _wpHomePage;

    public WpPocztaSteps(CustomTestContext context, IPage page) : base(context, page)
    {
        _wpHomePage = new WpHomePage(page);
        _wpPocztaLoginPage = new WpPocztaLoginPage(page);
        _wpPocztaRegistrationPage = new WpPocztaRegistrationPage(page);
    }

    [Given(@"I navigate to WP\.pl homepage")]
    public async Task GivenINavigateToWpPlHomepage()
    {
        await _wpHomePage.NavigateAsync();
        await _wpHomePage.AcceptCookiesIfPresentAsync();
    }

    [When(@"I click on the Poczta link")]
    public async Task WhenIClickOnThePocztaLink()
    {
        await _wpHomePage.ClickPocztaLinkAsync();

        // Get the new Poczta page and create new page object for it
        var pocztaPage = await _wpHomePage.GetPocztaPageAsync();
        _wpPocztaLoginPage = new WpPocztaLoginPage(pocztaPage);

        // Initialize accessibility listener for the new page
    //    InitializeAccessibilityListenerForNewPage(pocztaPage);

        // Accept cookies on the Poczta page if present
        await _wpPocztaLoginPage.AcceptCookiesIfPresentAsync();
    }

    [Then(@"I should see the WP Poczta login page")]
    public async Task ThenIShouldSeeTheWpPocztaLoginPage()
    {
        var isDisplayed = await _wpPocztaLoginPage.IsLoginPageDisplayedAsync();
        WpPocztaValidator.ValidateLoginPageDisplayed(isDisplayed);

        var pageTitle = await _wpPocztaLoginPage.GetPageTitleAsync();
        WpPocztaValidator.ValidatePageTitle(pageTitle);
    }

    [Then(@"I should see the email address field")]
    public async Task ThenIShouldSeeTheEmailAddressField()
    {
        var isVisible = await _wpPocztaLoginPage.IsEmailFieldVisibleAsync();
        WpPocztaValidator.ValidateEmailFieldVisible(isVisible);
    }

    [Then(@"I should see the password field")]
    public async Task ThenIShouldSeeThePasswordField()
    {
        var isVisible = await _wpPocztaLoginPage.IsPasswordFieldVisibleAsync();
        WpPocztaValidator.ValidatePasswordFieldVisible(isVisible);
    }

    [Then(@"I should see the login button")]
    public async Task ThenIShouldSeeTheLoginButton()
    {
        var isVisible = await _wpPocztaLoginPage.IsLoginButtonVisibleAsync();
        WpPocztaValidator.ValidateLoginButtonVisible(isVisible);
    }

    [Then(@"I should see the create new account link")]
    public async Task ThenIShouldSeeTheCreateNewAccountLink()
    {
        var isVisible = await _wpPocztaLoginPage.IsCreateAccountLinkVisibleAsync();
        WpPocztaValidator.ValidateCreateAccountLinkVisible(isVisible);
    }

    [When(@"I click on the create new account link")]
    public async Task WhenIClickOnTheCreateNewAccountLink()
    {
        // Try to detect if a new page will open, but don't wait indefinitely
        var currentPage = _wpPocztaLoginPage.GetPage();
        var pagesCountBefore = currentPage.Context.Pages.Count;

        // Start waiting for potential new page with a short timeout
        Task<IPage>? newPageTask = null;
        try
        {
            // Create wait task with custom timeout
            newPageTask = currentPage.Context.WaitForPageAsync(new() { Timeout = 5000 });

            await _wpPocztaLoginPage.ClickCreateAccountLinkAsync();

            // Wait briefly to see if new page opens
            var newPage = await newPageTask;

            // New page/tab opened - use it
            await newPage.WaitForLoadStateAsync(LoadState.Load);
            _wpPocztaRegistrationPage = new WpPocztaRegistrationPage(newPage);
         //   InitializeAccessibilityListenerForNewPage(newPage);
            Console.WriteLine($"Switched to registration page in new tab: {newPage.Url}");
        }
        catch (TimeoutException)
        {
            // No new page opened - navigation happened in same page
            await currentPage.WaitForLoadStateAsync(LoadState.Load);
            _wpPocztaRegistrationPage = new WpPocztaRegistrationPage(currentPage);
            Console.WriteLine($"Navigation to registration page in same tab: {currentPage.Url}");
        }
    }

    [Then(@"I should see the registration page")]
    public async Task ThenIShouldSeeTheRegistrationPage()
    {
        var isDisplayed = await _wpPocztaRegistrationPage.IsRegistrationPageDisplayedAsync();
        var url = _wpPocztaRegistrationPage.GetPageUrl();
        WpPocztaValidator.ValidateRegistrationPageDisplayed(isDisplayed, url);
    }


    // private void InitializeAccessibilityListenerForNewPage(IPage page)
    // {
    //     var listener = new AccessibilityPageEventListener(page, AccessibilityHelper, CustomTestContext);
    //     listener.Attach();
    //     Console.WriteLine($"WpPocztaSteps: Accessibility listener attached to new page - {page.Url}");
    // }
}


using Microsoft.Playwright;
using Playwright.E2ETests.Context;
using Playwright.E2ETests.Pages.ParaBankPages;
using Playwright.E2ETests.Validators;
using Reqnroll;

namespace Playwright.E2ETests.Steps.UI;

[Binding]
public class ParaBankNavigationSteps : BaseUiStepDefinitions
{
    private ParaBankHomePage _homePage;
    private ParaBankAboutUsPage _aboutUsPage;
    private ParaBankServicesPage _servicesPage;

    public ParaBankNavigationSteps(CustomTestContext customTestContext, IPage page) : base(customTestContext, page)
    {
        _homePage = new ParaBankHomePage(page);
        _aboutUsPage = new ParaBankAboutUsPage(page);
        _servicesPage = new ParaBankServicesPage(page);
    }

    [Given(@"I am on the ParaBank home page")]
    public async Task GivenIAmOnTheParaBankHomePage()
    {
        await _homePage.NavigateToAsync();
    }

    [Then(@"the ParaBank home page should be displayed")]
    public async Task ThenTheParaBankHomePageShouldBeDisplayed()
    {
        await ParaBankValidator.ValidateHomePageDisplayed(_homePage);
    }

    [When(@"I navigate to the About Us page")]
    public async Task WhenINavigateToTheAboutUsPage()
    {
        await _homePage.ClickAboutUsAsync();
    }

    [Then(@"the About Us page should be displayed")]
    public async Task ThenTheAboutUsPageShouldBeDisplayed()
    {
        await ParaBankValidator.ValidateAboutUsPageDisplayed(_aboutUsPage);
    }

    [When(@"I navigate to the Services page")]
    public async Task WhenINavigateToTheServicesPage()
    {
        await _aboutUsPage.ClickServicesAsync();
    }

    [Then(@"the Services page should be displayed")]
    public async Task ThenTheServicesPageShouldBeDisplayed()
    {
        await ParaBankValidator.ValidateServicesPageDisplayed(_servicesPage);
    }
}


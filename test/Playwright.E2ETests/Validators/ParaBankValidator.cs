using System.Text.RegularExpressions;
using Microsoft.Playwright;
using Playwright.E2ETests.Pages.ParaBankPages;

namespace Playwright.E2ETests.Validators;

public class ParaBankValidator
{
    public static async Task ValidateHomePageDisplayed(ParaBankHomePage homePage)
    {
        await Assertions.Expect(homePage.GetCustomerLoginHeading()).ToBeVisibleAsync();
        await Assertions.Expect(homePage.GetAboutUsLink()).ToBeVisibleAsync();
        await Assertions.Expect(homePage.GetPage()).ToHaveTitleAsync(new Regex("ParaBank.*Welcome.*Online Banking"));
        await Assertions.Expect(homePage.GetPage()).ToHaveURLAsync(new Regex(".*parabank\\.parasoft\\.com/parabank/index\\.htm"));
    }

    public static async Task ValidateAboutUsPageDisplayed(ParaBankAboutUsPage aboutUsPage)
    {
        await Assertions.Expect(aboutUsPage.GetAboutUsHeading()).ToBeVisibleAsync();
        await Assertions.Expect(aboutUsPage.GetParasoftLink()).ToBeVisibleAsync();
        await Assertions.Expect(aboutUsPage.GetPage()).ToHaveTitleAsync(new Regex("ParaBank.*About Us"));
        await Assertions.Expect(aboutUsPage.GetPage()).ToHaveURLAsync(new Regex(".*about\\.htm"));
    }

    public static async Task ValidateServicesPageDisplayed(ParaBankServicesPage servicesPage)
    {
        await Assertions.Expect(servicesPage.GetBookstoreServicesTable()).ToBeVisibleAsync();
        await Assertions.Expect(servicesPage.GetParaBankServicesTable()).ToBeVisibleAsync();
        await Assertions.Expect(servicesPage.GetRestfulServicesTable()).ToBeVisibleAsync();
        await Assertions.Expect(servicesPage.GetPage()).ToHaveTitleAsync(new Regex("ParaBank.*Services"));
        await Assertions.Expect(servicesPage.GetPage()).ToHaveURLAsync(new Regex(".*services\\.htm"));
    }
}


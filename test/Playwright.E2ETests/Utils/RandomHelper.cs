using Playwright.E2ETests.Model;

namespace Playwright.E2ETests.Utils;

public class RandomHelper
{

    private static readonly Random _random = new();
    public static string GenerateRandomString(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        var random = new Random();
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }

    public static string GenerateRandomAddress()
    {
        var streetNumber = new Random().Next(1, 9999);
        var streetName = GenerateRandomString(8) + " St";
        var city = GenerateRandomString(6);
        var postcode = GenerateRandomString(2).ToUpperInvariant() + new Random().Next(10, 99).ToString(System.Globalization.CultureInfo.InvariantCulture) + " " + new Random().Next(1, 9).ToString(System.Globalization.CultureInfo.InvariantCulture) +
                       GenerateRandomString(2).ToUpperInvariant();
        return $"{streetNumber} {streetName}, {city}, {postcode}";
    }
}

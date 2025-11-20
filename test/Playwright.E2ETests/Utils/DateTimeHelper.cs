using System.Globalization;

namespace Playwright.E2ETests.Utils;

public class DateTimeHelper
{
    public static string FormatDate(DateOnly date)
    {
        var formattedDate = date.ToString("dd MMMM yyyy", CultureInfo.InvariantCulture);
        return formattedDate;
    }

    public static DateOnly ParseDate(string dateString)
    {
        return DateOnly.ParseExact(dateString, "dd MMMM yyyy", CultureInfo.InvariantCulture);
    }
}

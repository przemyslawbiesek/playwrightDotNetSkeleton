using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Allure.Net.Commons;

namespace Playwright.E2ETests.Context;

public class CustomTestContext
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        Converters = { new JsonStringEnumConverter() }
    };

    public enum Keys
    {
        AccessibilityHelper,
        AccessibilityFailures,
        ScannedPageUrls
    }

    private readonly Dictionary<Keys, object> _data;

    public CustomTestContext()
    {
        _data = new Dictionary<Keys, object>();
    }

    public void Put(Keys key, object element)
    {
        _data[key] = element; // Use indexer to allow overwriting existing keys
        var json = JsonSerializer.Serialize(element, JsonOptions);
        var bytes = Encoding.UTF8.GetBytes(json);
        AllureApi.AddAttachment(key.ToString(), "text/plain", bytes, ".json");
    }

    public object Get(Keys key)
    {
        return _data[key];
    }
}

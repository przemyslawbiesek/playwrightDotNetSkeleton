using Newtonsoft.Json;
using Playwright.E2ETests.Configuration.Environment;

namespace Playwright.E2ETests.Configuration;

public class ConfigurationManager
{
    private static readonly string ConfigurationRootDirectoryPath = "Resources/Configuration";
    private static readonly string CommonConfigurationPath = $"{ConfigurationRootDirectoryPath}/common.json";
    private static readonly string BrowserConfigurationPath = $"{ConfigurationRootDirectoryPath}/browsers.json";

    public static CommonConfiguration GetCommon()
    {
        return JsonConvert.DeserializeObject<CommonConfiguration>(File.ReadAllText(CommonConfigurationPath)) ??
               throw new FileLoadException("Cannot load common configuration file.");
    }


    public static EnvironmentConfiguration GetEnvironment()
    {
        var environment = EnvironmentVariableReader.GetValue(EnvironmentVariableName.Environment) ?? "local";
        var path = Path.Combine(ConfigurationRootDirectoryPath, "Environment", $"{environment}.json");
        return JsonConvert.DeserializeObject<EnvironmentConfiguration>(
            File.ReadAllText(path)) ?? throw new FileLoadException("Cannot load environment configuration file.");
    }

    public static BrowserConfiguration GetBrowser()
    {
        var configuration = EnvironmentVariableReader.GetValue(EnvironmentVariableName.BrowserConfiguration) ??
                            "chrome";
        return JsonConvert.DeserializeObject<Dictionary<string, BrowserConfiguration>>(
                   File.ReadAllText(BrowserConfigurationPath))?[configuration] ??
               throw new ArgumentException($"Configuration {configuration} is not defined") ??
                     throw new FileLoadException("Cannot load browser configuration file.");
    }
}

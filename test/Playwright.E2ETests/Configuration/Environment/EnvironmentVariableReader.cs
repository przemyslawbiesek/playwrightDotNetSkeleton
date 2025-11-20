namespace Playwright.E2ETests.Configuration.Environment;

public class EnvironmentVariableReader
{
    public static string? GetValue(EnvironmentVariableName name)
    {
        return System.Environment.GetEnvironmentVariable(name.ToString());
    }

    public static string GetValueFromString(string name)
    {
        return System.Environment.GetEnvironmentVariable(name) ??
               throw new ArgumentException($"Environment variable with name ${name} is not set.");
    }

    public static string GetMandatoryValue(EnvironmentVariableName name)
    {
        var value = GetValue(name);
        if (value is null) throw new ArgumentException($"Environment variable {name} is not set.");

        return value;
    }
}

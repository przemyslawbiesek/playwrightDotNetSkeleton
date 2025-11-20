namespace Playwright.E2ETests.Configuration;

public class EnvironmentConfiguration
{
    public string? BaseUrl { get; set; }
    public string? ApiBaseUrl { get; set; }
    public string? MessagesBackendUrl { get; set; }
    public string? UserName { get; set; }
    public string? Password { get; set; }
    public string? OptCode { get; set; }
    public string? UserFullname { get; set; }
    public string? UserBirthday { get; set; }
    public string? UserNHSNumber { get; set; }
    public string? XApiKey { get; set; }
    public string? XConsumerApplicationId8 { get; set; }
    public string? XConsumerApplicationId2 { get; set; }
    public string? XConsumerApplicationId3 { get; set; }
    public string? XNhswappKey { get; set; }
    public string? MongoDbUrl { get; set; }
    public string? DbName { get; set; }
    public MongoCollections? Collections { get; set; }
}

public class MongoCollections
{
    public string? Consent { get; set; }
    public string? Info { get; set; }
    public string? Messages { get; set; }
}

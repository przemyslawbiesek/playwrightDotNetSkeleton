# Data Builders Instructions

## Applies to: `test/Playwright.E2ETests/DataBuilders/**`

### Data Builder Pattern Standards

#### Builder Class Structure
```csharp
public class UserDataBuilder
{
    private string _username = "defaultUser";
    private string _email = "user@example.com";
    private string _firstName = "John";
    private string _lastName = "Doe";

    public UserDataBuilder WithUsername(string username)
    {
        _username = username;
        return this;
    }

    public UserDataBuilder WithEmail(string email)
    {
        _email = email;
        return this;
    }

    public UserModel Build()
    {
        return new UserModel
        {
            Username = _username,
            Email = _email,
            FirstName = _firstName,
            LastName = _lastName
        };
    }
}
```

### Key Rules
- Use Builder pattern for test data creation
- Provide sensible defaults for all fields
- Use fluent API with `With*` methods returning `this`
- End with `Build()` method that creates the model
- Name classes with `*DataBuilder` suffix

### Usage Example
```csharp
var user = new UserDataBuilder()
    .WithUsername("testuser")
    .WithEmail("test@example.com")
    .Build();
```

### Existing Builders
- `AppointmentsDataBuilder` - Appointment data
- `MessageDataBuilder` - Message data
- `MongoDataBuilder` - MongoDB test data
- `PrescriptionDataBuilder` - Prescription data

### Best Practices
- Keep builders focused on single domain entity
- Use builders in step definitions for test data setup
- Provide multiple `With*` methods for flexibility
- Consider random data generation for unique values

### Integration with Tests
```csharp
// In step definitions
[Given(@"I have a test user")]
public void GivenIHaveATestUser()
{
    var user = new UserDataBuilder()
        .WithUsername($"testuser_{Guid.NewGuid()}")
        .Build();
    
    _context.Set("user", user);
}
```

### Advanced Patterns
```csharp
// Builder with method chaining for complex objects
public class MessageDataBuilder
{
    private string _subject = "Default Subject";
    private string _body = "Default Body";
    private DateTime _sendDate = DateTime.Now;
    private List<string> _recipients = new();

    public MessageDataBuilder WithSubject(string subject)
    {
        _subject = subject;
        return this;
    }

    public MessageDataBuilder WithRecipients(params string[] recipients)
    {
        _recipients.AddRange(recipients);
        return this;
    }

    public MessageDataBuilder SentYesterday()
    {
        _sendDate = DateTime.Now.AddDays(-1);
        return this;
    }

    public MessageModel Build()
    {
        return new MessageModel
        {
            Subject = _subject,
            Body = _body,
            SendDate = _sendDate,
            Recipients = _recipients
        };
    }
}
```

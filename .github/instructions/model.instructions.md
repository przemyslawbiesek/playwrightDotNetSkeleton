# Model Classes Instructions

## Applies to: `test/Playwright.E2ETests/Model/**`

### Model Pattern Standards

#### Class Structure
```csharp
public class UserModel
{
    public string Username { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
}

public class PrescriptionInfo
{
    public string PrescriptionId { get; set; }
    public DateTime IssueDate { get; set; }
    public List<string> Medications { get; set; }
}
```

### Key Rules
- **Use models when extracting data from web elements**
- Put models in proper Model classes organized by domain
- Use C# properties with proper types
- Models represent data structures from UI or API
- Keep models simple and focused on data


### Naming Conventions
- Use descriptive names ending with `Model` or specific domain name
- Examples: `UserModel`, `PrescriptionInfo`, `PatientDetails`
- Use PascalCase for class and property names

### Usage
- Extract web element data into models in Page Objects
- Pass models between steps via CustomTestContext
- Validate model data in Validators


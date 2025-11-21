# Feature Files Instructions

## Applies to: `test/Playwright.E2ETests/Features/**`

### Gherkin Syntax Standards

#### Feature File Structure
```gherkin
@tag @another-tag
Feature: User Login
  As a registered user
  I want to log in to the application
  So that I can access my account

  Background:
    Given I am on the login page

  @smoke @login
  Scenario: Successful login with valid credentials
    Given I have valid user credentials
    When I enter my username and password
    And I click the login button
    Then I should be redirected to the dashboard
    And I should see my username displayed

  Scenario Outline: Login with invalid credentials
    When I enter username "<username>" and password "<password>"
    And I click the login button
    Then I should see error message "<error>"
    
    Examples:
      | username | password | error                |
      | invalid  | pass123  | Invalid credentials  |
      | user123  | wrong    | Invalid credentials  |
```

### Key Rules
- **New user journeys added as new feature files**
- Write scenarios in Gherkin syntax (.feature files)
- UI scenarios in `Features/UI/` directory
- API scenarios in `Features/API/` directory
- Keep tests independent and atomic
- Use tags for test categorization and filtering

### Gherkin Best Practices
- Use business language, not technical implementation
- Keep scenarios focused on single user journey
- Use `Background` for common setup steps
- Use `Scenario Outline` for data-driven tests
- Add meaningful tags for test organization
- Use Given-When-Then structure consistently

### File Organization
- One feature per file
- Group related scenarios in same feature file
- Filename should match feature name (e.g., `UserLogin.feature`)


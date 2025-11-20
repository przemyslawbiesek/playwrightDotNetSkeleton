# GitHub Copilot Instructions - Organization Guide

## Structure Overview

Following GitHub's [path-specific custom instructions](https://docs.github.com/en/copilot/how-tos/configure-custom-instructions/add-repository-instructions#creating-path-specific-custom-instructions-1) pattern, the Copilot instructions are now organized as:

### Root Level Instructions
**File**: `.github/copilot-instructions.md`
- **Scope**: Applies to entire repository
- **Content**: General project overview, architecture patterns, core technologies
- **Purpose**: High-level guidance applicable across all files

### Path-Specific Instructions
**Directory**: `.github/instructions/`
- **Scope**: Each file applies to specific directory paths
- **Format**: `{component_name}.instructions.md`
- **Purpose**: Detailed, context-specific guidance for working in particular directories

## Path-Specific Instruction Files

| File | Applies To | Content |
|------|------------|---------|
| `pages.instructions.md` | `test/Playwright.E2ETests/Pages/**` | Page Object pattern, locator initialization, selector preferences |
| `steps.instructions.md` | `test/Playwright.E2ETests/Steps/**` | Step definition patterns, accessibility testing, UI/API steps |
| `validators.instructions.md` | `test/Playwright.E2ETests/Validators/**` | Validator patterns, Playwright vs NUnit assertions |
| `features.instructions.md` | `test/Playwright.E2ETests/Features/**` | Gherkin syntax, feature file structure, BDD best practices |
| `model.instructions.md` | `test/Playwright.E2ETests/Model/**` | Model class patterns, data structure organization |
| `databuilders.instructions.md` | `test/Playwright.E2ETests/DataBuilders/**` | Builder pattern for test data creation |

## How It Works

When you work on a file, GitHub Copilot automatically:
1. **Loads root instructions** from `.github/copilot-instructions.md`
2. **Loads path-specific instructions** for the directory you're working in
3. **Combines both** to provide contextually relevant suggestions

### Example Scenarios

**Scenario 1: Creating a new Page Object**
- Working in: `test/Playwright.E2ETests/Pages/NhsWalesPages/LoginPage.cs`
- Copilot loads:
  - Root instructions (general project context)
  - `pages.instructions.md` (Page Object specific guidance)
- Result: Suggestions focused on Page Object patterns, locator initialization, selector preferences

**Scenario 2: Writing Step Definitions**
- Working in: `test/Playwright.E2ETests/Steps/UI/LoginSteps.cs`
- Copilot loads:
  - Root instructions (general project context)
  - `steps.instructions.md` (Step definition specific guidance)
- Result: Suggestions include accessibility checks, BaseUiStepDefinitions inheritance, proper step attributes

**Scenario 3: Creating Gherkin Scenarios**
- Working in: `test/Playwright.E2ETests/Features/UI/Login.feature`
- Copilot loads:
  - Root instructions (general project context)
  - `features.instructions.md` (Gherkin/BDD specific guidance)
- Result: Suggestions for proper Gherkin syntax, scenario structure, tagging

## Example Use Cases

| You're editing... | Copilot loads... | You get suggestions for... |
|-------------------|------------------|----------------------------|
| `Pages/LoginPage.cs` | `pages.instructions.md` | Page Object patterns, locators, selectors |
| `Steps/UI/LoginSteps.cs` | `steps.instructions.md` | Step definitions, accessibility checks |
| `Validators/LoginValidator.cs` | `validators.instructions.md` | Playwright Expect vs NUnit Assert |
| `Features/UI/Login.feature` | `features.instructions.md` | Gherkin syntax, scenario structure |
| `Model/UserModel.cs` | `model.instructions.md` | Model class patterns |
| `DataBuilders/UserDataBuilder.cs` | `databuilders.instructions.md` | Builder pattern |

## Benefits

✅ **Contextual Relevance**: Copilot suggestions tailored to the specific component you're building  
✅ **Better Organization**: Easier to maintain and update specific guidance  
✅ **Reduced Noise**: Only relevant patterns shown for current context  
✅ **Scalability**: Easy to add new path-specific instructions as project grows  
✅ **Standard Compliance**: Follows GitHub's recommended approach  

## Maintenance

### Adding New Path-Specific Instructions
1. Create new file in `.github/instructions/`
2. Name it: `{component_name}.instructions.md`
3. Add "Applies to:" header specifying the path pattern
4. Document specific patterns and rules for that directory

### Updating Existing Instructions
- Root level changes: Edit `.github/copilot-instructions.md`
- Path-specific changes: Edit corresponding file in `.github/instructions/`

## File Naming Convention

Path-specific instruction files use simple component names with `.instructions.md` extension:

- `test/Playwright.E2ETests/Pages/**` → `pages.instructions.md`
- `test/Playwright.E2ETests/Steps/**` → `steps.instructions.md`
- `test/Playwright.E2ETests/Features/**` → `features.instructions.md`
- `test/Playwright.E2ETests/Model/**` → `model.instructions.md`
- `test/Playwright.E2ETests/Validators/**` → `validators.instructions.md`
- `test/Playwright.E2ETests/DataBuilders/**` → `databuilders.instructions.md`

## References

- [GitHub Docs: Path-Specific Custom Instructions](https://docs.github.com/en/copilot/how-tos/configure-custom-instructions/add-repository-instructions#creating-path-specific-custom-instructions-1)
- Project specific details: `test/Playwright.E2ETests/MCPContext.md`


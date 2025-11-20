
# Quick Start Guide - New .env Configuration

## For New Developers

### 1. Setup Configuration (2 minutes)

```bash
# Navigate to workspace root
cd /Users/przemyslawb/dev/playwrightDotNetSkeleton

# Copy the template
cp .env.example .env

# Open and edit the .env file
# Update any values specific to your local environment
```

### 2. Build and Run (1 minute)

```bash
# Build the project
dotnet build

# Run all tests
dotnet test

# Or run specific tests
dotnet test --filter "Name~ParaBankNavigation"
```

## For Existing Developers

### Migrating from JSON Configuration

```bash
# 1. Create your .env file
cp .env.example .env

# 2. Transfer your custom values from old JSON files to .env
# (if you had any custom local settings)

# 3. Build and test
dotnet build
dotnet test
```

**Note**: The old JSON files in `Resources/Configuration/` are no longer used.

## Configuration Quick Reference

### Common Variables You Might Change

```env
# Browser settings
BROWSER_CONFIGURATION=chrome          # chrome, firefox, safari, edge
HEADLESS=false                        # true for headless mode

# Application URLs
BASE_URL=http://web.local.bitraft.io:3000/
API_BASE_URL=http://web.local.bitraft.io:7100

# Test credentials (if needed)
USER_NAME=
PASSWORD=
```

### Switching Environments

```bash
# For UAT testing
cp .env.uat .env

# For local development
cp .env.example .env
```

## Browser Configuration Options

Use `BROWSER_CONFIGURATION` for quick setup:

- `chrome` - Chrome desktop (1920x1080)
- `firefox` - Firefox desktop (1920x1080)
- `safari` - Safari desktop (1920x1080)
- `edge` - Edge desktop (1920x1080)
- `safari-iphone-13` - iPhone 13 simulation
- `chrome-galaxy-s15` - Galaxy S9+ simulation

## Need More Info?

- **All configuration options**: See [CONFIGURATION.md](CONFIGURATION.md)
- **Migration details**: See [MIGRATION.md](MIGRATION.md)
- **Complete checklist**: See [CHECKLIST.md](CHECKLIST.md)
- **Example values**: See [.env.example](.env.example)

## Troubleshooting

### Tests can't find configuration
✅ **Solution**: Make sure `.env` file exists in workspace root
```bash
ls -la .env  # Should show the file
```

### Wrong configuration values being used
✅ **Solution**: Check your `.env` file has the correct values
```bash
cat .env  # Review the values
```

### Configuration not updating
✅ **Solution**: Rebuild the project
```bash
dotnet clean
dotnet build
```

## That's It! 🎉

You're ready to go. The new .env configuration is simpler and more secure than the old JSON approach.

Happy testing! 🚀


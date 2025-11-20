# Environment Configuration Guide

This project uses `.env` files for configuration instead of JSON files.

## Setup

1. Copy `.env.example` to `.env`:
   ```bash
   cp .env.example .env
   ```

2. Update the values in `.env` file according to your environment.

## Environment Files

- `.env` - Your local configuration (not committed to git)
- `.env.example` - Template with all available configuration options
- `.env.uat` - UAT environment configuration (committed to git)
- `.env.staging` - Staging environment configuration (optional, committed to git)
- `.env.production` - Production environment configuration (optional, committed to git)

## Configuration Variables

### Environment Settings
- `ENVIRONMENT` - Environment name (local, uat, staging, production)

### Browser Configuration
- `BROWSER_CONFIGURATION` - Predefined browser config (chrome, firefox, safari, edge, safari-iphone-13, etc.)
- `BROWSER` - Browser type (Chrome, Firefox, Safari, Edge) - overrides BROWSER_CONFIGURATION
- `DEVICE` - Device emulation (e.g., "iPhone 13", "Galaxy S9+")
- `VIEWPORT_WIDTH` - Browser viewport width in pixels
- `VIEWPORT_HEIGHT` - Browser viewport height in pixels

### Common Settings
- `SCREENSHOTS_DIRECTORY` - Directory for screenshots
- `ACCESSIBILITY_DIRECTORY` - Directory for accessibility reports
- `TRACES_DIRECTORY` - Directory for Playwright traces
- `ACCESSIBILITY_TAGS` - Comma-separated WCAG tags (e.g., wcag2a,wcag2aa,wcag21a,wcag21aa)
- `HEADLESS` - Run browser in headless mode (true/false)
- `AUTO_ACCESSIBILITY_TRACKING` - Enable automatic accessibility tracking (true/false)

### Application URLs
- `BASE_URL` - Main application URL
- `API_BASE_URL` - API base URL
- `MESSAGES_BACKEND_URL` - Messages backend URL

### User Credentials
- `USER_NAME` - Test user username/email
- `PASSWORD` - Test user password
- `OPT_CODE` - OTP code
- `USER_FULLNAME` - User's full name
- `USER_BIRTHDAY` - User's birthday
- `USER_NHS_NUMBER` - NHS number

### API Keys
- `X_API_KEY` - API key
- `X_CONSUMER_APPLICATION_ID_8` - Consumer application ID 8
- `X_CONSUMER_APPLICATION_ID_2` - Consumer application ID 2
- `X_CONSUMER_APPLICATION_ID_3` - Consumer application ID 3
- `X_NHSWAPP_KEY` - NHS Wales app key

### MongoDB Configuration
- `MONGODB_URL` - MongoDB connection URL
- `DB_NAME` - Database name
- `COLLECTION_CONSENT` - Consent collection name
- `COLLECTION_INFO` - Info collection name
- `COLLECTION_MESSAGES` - Messages collection name

## Using Different Environments

To switch between environments, you can:

1. **Copy environment-specific file:**
   ```bash
   cp .env.uat .env
   ```

2. **Or set ENVIRONMENT variable and load different file in your CI/CD:**
   ```bash
   export ENVIRONMENT=uat
   ```

## Predefined Browser Configurations

When using `BROWSER_CONFIGURATION`, the following presets are available:
- `chrome` - Chrome browser, 1920x1080
- `firefox` - Firefox browser, 1920x1080
- `safari` - Safari browser, 1920x1080
- `edge` - Edge browser, 1920x1080
- `safari-iphone-13` - Safari on iPhone 13 (device emulation)
- `safari-iphone-13-landscape` - Safari on iPhone 13 landscape
- `chrome-galaxy-s15` - Chrome on Galaxy S9+ (device emulation)
- `chrome-galaxy-s15-landscape` - Chrome on Galaxy S9+ landscape

## Migration from JSON Configuration

The old JSON-based configuration files in `Resources/Configuration/` have been removed. All configuration is now managed through environment variables in `.env` files.

## Security Note

⚠️ **Never commit `.env` or `.env.local` files to version control** - they may contain sensitive credentials!

Only environment-specific files like `.env.uat`, `.env.staging`, `.env.production` should be committed if they don't contain sensitive data.


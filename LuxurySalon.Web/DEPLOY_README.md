# Luxury Salon - Somee Deployment Guide (IIS Windows Hosting)

This guide provides instructions on how to deploy the Luxury Salon appointment system to Somee.com hosting.

## 1. Update Connection String

Before starting the application, you must update the SQL Server connection string in `appsettings.Production.json`:

1. Log in to your Somee account.
2. Create a SQL Server database.
3. Go to the Connection String section in Somee dashboard.
4. Open `appsettings.Production.json` in the website root.
5. Replace the `DefaultConnection` value with the one provided by Somee.

## 2. Running Migrations

Since Somee provides a web interface for database management, you have two options for migrations:

### Option A: Manual Script (Recommended)

1. Run the following command locally to generate a SQL script:
    `dotnet ef migrations script --project LuxurySalon.Infrastructure --startup-project LuxurySalon.Web --output script.sql`
2. Copy the content of `script.sql`.
3. Run it via the Somee SQL Query tool.

### Option B: Automatic Migration

The application is configured to run `await DbInitializer.Initialize(services);` on startup. This will attempt to create the database if it doesn't exist and seed initial data. **Note:** This requires the database user to have `CREATE TABLE` permissions.

## 3. How to Publish

If you need to re-publish the application:

1. Open power shell in the root directory.
2. Run: `dotnet publish -c Release -o ./publish`
3. Zip the contents of the `./publish` folder.
4. Upload the zip file via Somee Control Panel (FTP or File Manager).

## 4. HTTPS and Environment

- The application is set to `Production` mode via `web.config`.
- HTTPS Redirection is enabled. Somee provides a free SSL/TLS shared certificate or you can use your own.

## 5. Logging

Logs are stored in the `/Logs` folder in the website root. If the application doesn't start, check `production-error-.log` or enable `stdoutLogEnabled="true"` in `web.config` for more details.

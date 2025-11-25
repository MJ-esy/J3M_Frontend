# J3M

A Razor-based meal-planning web application for discovering recipes, exploring ingredients, and building weekly meal plans. Built with ASP.NET Core targeting .NET 9 and a responsive UI using Bootstrap and custom styles.

---

## Table of contents
- [Project overview](#project-overview)
- [Features](#features)
- [Tech stack](#tech-stack)
- [Prerequisites](#prerequisites)
- [Developer setup](#developer-setup)
- [Configuration](#configuration)
- [Run in Visual Studio 2022](#run-in-visual-studio-2022)
- [Run with dotnet CLI](#run-with-dotnet-cli)
- [Client-side libraries](#client-side-libraries)
- [Testing](#testing)
- [Troubleshooting](#troubleshooting)
- [Contributing](#contributing)
- [License & acknowledgements](#license--acknowledgements)
- [Repository info / contacts](#repository-info--contacts)

## Project overview
J3M is a Razor-based web front-end that helps users:
- Browse and discover recipes
- Explore ingredients and dietary filters (keto, vegan, pescetarian, paleo, vegetarian)
- Create and manage weekly meal plans
- Sign in to a personal user page

Key files:
- Shared layout: `Views/Shared/_Layout.cshtml` (SEO meta tags, navigation, cookie consent)
- Home page: `Views/Home/Index.cshtml`
- Static assets: `wwwroot/lib/`, `wwwroot/css/`, `wwwroot/js/`
- Cookie-based authentication: layout reads cookie `AuthToken`

## Features
- Responsive UI with Bootstrap and custom styles
- Cookie consent banner with persistence (localStorage)
- Basic authentication flow (Login / Logout) and user page links
- SEO-friendly meta tags in layout

## Tech stack
- .NET 9 / C# 13
- ASP.NET Core (Razor views / Razor Pages compatible)
- Bootstrap, jQuery, Google Fonts
- Static assets served from `wwwroot`

## Prerequisites
- Visual Studio 2022 with ASP.NET and web development workload
- .NET 9 SDK installed
- Git
- (Optional) Node.js / npm if you use front-end tooling
- (Optional) Database server (SQL Server, SQLite, etc.) if persistence is required

## Developer setup
1. Clone the repo:
   - `git clone https://github.com/MJ-esy/J3M_Frontend.git`
   - `cd J3M_Frontend`
2. Restore NuGet packages:
   - `dotnet restore` or use __NuGet Package Manager > Restore__
3. Restore client libraries (if using LibMan):
   - `libman restore` or use Visual Studio __Manage Client-Side Libraries__ -> __Restore__
4. (Optional) Initialize user secrets for local config:
   - `dotnet user-secrets init`
   - `dotnet user-secrets set "ConnectionStrings:DefaultConnection" "<value>"`

## Configuration
- App settings live in `appsettings.json` and `appsettings.Development.json`.
- Typical keys:
  - `ConnectionStrings:DefaultConnection` — database connection
  - Authentication and token settings (layout checks `AuthToken` cookie)
- Use environment-specific settings and the user-secrets store for secrets during development.

## Run in Visual Studio 2022
1. Open the solution in Visual Studio 2022.
2. Restore packages and client libs:
   - __NuGet Package Manager > Restore__ or right-click solution -> __Restore NuGet Packages__
   - __Manage Client-Side Libraries__ -> __Restore__ (if using LibMan)
3. Set the startup project (right-click project -> __Set as StartUp Project__).
4. Build the solution: __Build > Build Solution__.
5. Run/debug: __Debug > Start Debugging__ or __Debug > Start Without Debugging__.

## Run with dotnet CLI
From the repository root or project folder:
- Restore and build:
  - `dotnet restore`
  - `dotnet build`
- Run:
  - `dotnet run --project <Path/To/Project.csproj>`
  - or `dotnet run` from the project folder

Replace `<Path/To/Project.csproj>` with the actual `.csproj` path.

## Client-side libraries
- Client libs are referenced under `~/lib` (Bootstrap, jQuery).
- Managed via LibMan (`libman.json`) or manually in `wwwroot`.
- Restore with Visual Studio __Manage Client-Side Libraries__ or `libman restore`.

## Testing
- If test projects exist, run:
  - `dotnet test`
- Use Visual Studio Test Explorer to run and debug tests.

## Troubleshooting
- Missing static files: ensure `UseStaticFiles()` is enabled in `Program.cs`.
- Client libs not found: restore LibMan or re-add libraries.
- Cookie auth issues: verify cookie name `AuthToken`, authentication middleware and anti-forgery tokens (logout form in layout contains `@Html.AntiForgeryToken()`).
- Port conflicts: edit `launchSettings.json` or change the IIS Express profile in Visual Studio.

## Contributing
- Fork the repo, create a branch `feature/<name>` or `fix/<name>`, implement changes, open a pull request.
- Include tests for new behavior where appropriate.
- Follow existing code style and formatting; `dotnet format` can help.

## License & acknowledgements
- Add a `LICENSE` file to the repo. Recommended: MIT for permissive use.
- Acknowledge third-party libraries: Bootstrap, jQuery, Google Fonts.

## Repository info / contacts
- Remote: `https://github.com/MJ-esy/J3M_Frontend`
- Current branch in local workspace (example): `feature/SEO-implementation`
- For questions, open an issue in the repository or contact the maintainers.

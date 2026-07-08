# Axlis.CleanArchitecture.Sample

> Sample Clean Architecture .NET 8 WebApi demonstrating the Axlis Sitecore Headless GraphQL ORM.

**Note:** This project demonstrates production-ready patterns for integrating Axlis into a Clean Architecture solution. Axlis-specific documentation uses the `.AXLIS.md` suffix (e.g., `CHANGELOG.AXLIS.md`, `CONTRIBUTING.AXLIS.md`).

[![.NET 8](https://img.shields.io/badge/.NET-8.0-purple)](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](LICENSE)

---

## What is this?

**Axlis.CleanArchitecture.Sample** is a working example of integrating the Axlis ORM into a Clean Architecture ASP.NET Core WebApi. It demonstrates:

- Wiring Axlis and Axlis.GraphQL services via dependency injection
- Defining strongly-typed Sitecore template POCOs
- Using the Axlis facade (`ISitecoreFacade`) in CQRS handlers
- Exercising field types: TextField, ImageField, MultilistField, ItemReferenceField
- Axes traversal: Parent, Children, Siblings, GetChildren, GetDescendants
- The WithResult rich API: value, metadata, and diagnostics

This is a **sample project**, not a template. It demonstrates production-ready patterns for integrating Axlis into a Clean Architecture solution. All Axlis packages are published to NuGet.org at v0.1.0, so you can use this as a reference for your own projects.

---

## Architecture Overview

The solution follows Clean Architecture with strict dependency rules. Axlis is wired in the WebApi composition root and consumed by the Application layer via `ISitecoreFacade`.

```
┌──────────────────────────────────────────────┐
│                   WebApi                     │  Composition root · Axlis DI · user-secrets
├──────────────────────────────────────────────┤
│                 Presentation                 │  Controllers · SitecoreController (/v1/sitecore)
├──────────────────────────────────────────────┤
│       Application                           │  CQRS handlers · Sitecore template POCOs
│       └── Sitecore/                          │  Disclaimer, HomePage, DictionaryRoot, Style
├──────────────────────────────────────────────┤
│               Infrastructure                 │  DateTimeProvider · Operational wiring
├──────────────────────┬───────────────────────┤
│        Domain        │        Shared         │  BaseEntity · IAggregateRoot   │   Enums
└──────────────────────┴───────────────────────┘
```

| Layer | Project | Axlis Integration |
|---|---|---|
| **WebApi** | `CleanArchitecture.WebApi` | Registers `AddAxlis()` and `AddAxlisGraphQL()`, wires `UseAxlis()`, reads config from appsettings + user-secrets |
| **Presentation** | `CleanArchitecture.Presentation` | `SitecoreController` exposes `/v1/sitecore/showcase` endpoint |
| **Application** | `CleanArchitecture.Application` | Defines Sitecore template POCOs, CQRS handler uses `ISitecoreFacade` |
| **Infrastructure** | `CleanArchitecture.Infrastructure` | No Axlis code (clean separation) |
| **Domain** | `CleanArchitecture.Domain` | No Axlis code (pure domain) |
| **Shared** | `CleanArchitecture.Shared` | No Axlis code (shared types) |

---

## Project Structure

```
Axlis.CleanArchitecture.Sample/
├── src/
│   ├── CleanArchitecture.Domain/           # BaseEntity, IAggregateRoot
│   ├── CleanArchitecture.Shared/           # Enums, constants
│   ├── CleanArchitecture.Application/      # CQRS handlers, Sitecore template POCOs
│   │   ├── Api/
│   │   │   ├── Samples/                    # PowerCSharp Cache sample endpoints
│   │   │   └── Sitecore/                   # Axlis showcase: Query, Handler, Response
│   │   ├── Sitecore/
│   │   │   └── Templates/                  # Disclaimer, HomePage, DictionaryRoot, Style
│   │   └── Common/                         # Behaviors, BaseRequestHandler
│   ├── CleanArchitecture.Operational/      # Polly retry policies
│   ├── CleanArchitecture.Infrastructure/   # DateTimeProvider
│   ├── CleanArchitecture.Presentation/     # Controllers, ApiResponse<T>
│   │   └── Controllers/v1/
│   │       ├── SamplesController           # PowerCSharp Cache demo
│   │       └── SitecoreController          # Axlis showcase endpoint
│   └── CleanArchitecture.WebApi/           # Program.cs (Axlis DI, user-secrets)
├── tests/
│   ├── CleanArchitecture.Tests.Shared/
│   ├── CleanArchitecture.WebApi.UnitTests/
│   └── CleanArchitecture.WebApi.IntegrationTests/
├── .github/
│   └── workflows/
│       └── ci.yml
├── Directory.Build.props
├── global.json
└── PowerCSharp.CleanArchitecture.sln
```

---

## What's Included

### Axlis Integration
- **Axlis service registration** in `Program.cs` via `AddAxlis()` and `AddAxlisGraphQL()`
- **Ambient lazy-loader** wired via `UseAxlis()` for ExtendedItem.Axes traversal
- **User-secrets** for sensitive config (Endpoint, ApiKey) — never committed
- **NuGet packages** at v0.1.0 (Axlis, Axlis.Abstractions, Axlis.Core, Axlis.GraphQL) from nuget.org
- **Configurable caching** via PowerCSharp.Feature.Cache providers (BitFaster, Disk)
- **Diagnostic support** via EnableDiagnostics option for troubleshooting

### Sitecore Template POCOs
Located in `src/CleanArchitecture.Application/Sitecore/Templates/`:

**System Templates:**
- **Language** — Language settings including charset, encoding, ISO codes, and fallback language
- **MainSection** — Base system template (foundation for other templates)
- **Node** — Base template for hierarchical structures
- **PublishingTarget** — Publishing target database configuration

**Sample Templates:**
- **SampleItem** — Demonstrates TextField usage with Title and Text fields

All template classes include comprehensive XML documentation explaining their purpose and field mappings.

### CQRS Showcase Endpoint
`GET /v1/sitecore/showcase` exercises six Axlis API pivots:
1. **TextField** — SampleItem.Title and SampleItem.Text field access
2. **Axes traversal** — Parent, Children, Grandparent, Siblings navigation
3. **GetDescendants** — Recursive traversal with template type filtering (Language items)
4. **WithResult rich API** — Metadata (ItemId, ItemPath, ItemVersion, Timestamp) and Diagnostics (warnings, errors, info)
5. **Lazy-loading** — Demonstrates Axes lazy-fetch behavior for items beyond initial fetch
6. **Caching** — Shows how Axlis caching integrates with the facade

The handler includes comprehensive developer notes explaining each API pattern, performance considerations, and best practices.

### Clean Architecture Foundation
- Strict dependency rule enforced by project references
- CQRS via MediatR with `BaseRequestHandler<T, TResponse>`
- FluentValidation integration
- `ApiResponse<T>` envelope on all endpoints
- `LoggingBehavior<TRequest, TResponse>` pipeline behavior

### PowerCSharp Features Framework
- CORS, Cache (BitFaster), DiskCache, Samples feature modules
- Flag-gating via `PowerFeatures:<Key>:Enabled` in `appsettings.json`
- Samples feature enabled in Development for showcase endpoint access

---

## Prerequisites

| Tool | Version |
|---|---|
| [.NET SDK](https://dotnet.microsoft.com/download/dotnet/8.0) | 8.0 or later |
| Git | any recent version |
| Sitecore Headless GraphQL endpoint | accessible from dev machine |

---

## Quick Start

### Clone and configure

```bash
git clone https://github.com/marioarce/Axlis.CleanArchitecture.Sample.git
cd Axlis.CleanArchitecture.Sample

dotnet restore
dotnet build
```

### Set user-secrets for Sitecore connection

```bash
cd src/CleanArchitecture.WebApi
dotnet user-secrets set "AxlisGraphQL:Endpoint" "https://your-sitecore-instance/sitecore/api/graph/edge"
dotnet user-secrets set "AxlisGraphQL:ApiKey" "{YOUR-API-KEY}"
```

### Run the API

```bash
dotnet run --project src/CleanArchitecture.WebApi/CleanArchitecture.WebApi.csproj
```

The API starts on `https://localhost:7xxx` / `http://localhost:5xxx`. Open the Swagger UI at `/swagger`.

### Test the showcase endpoint

```bash
curl "https://localhost:7235/v1/sitecore/showcase?rootPath=/sitecore/content/home"
```

Adjust `rootPath` to match your Sitecore content tree structure.

---

## Running Tests

```bash
# All tests
dotnet test

# Unit tests only
dotnet test tests/CleanArchitecture.WebApi.UnitTests

# Integration tests only
dotnet test tests/CleanArchitecture.WebApi.IntegrationTests
```

Integration tests use `WebApplicationFactory<Program>` — no external dependencies required.

---

## Configuration

### Axlis Configuration

`appsettings.json` contains the Axlis and AxlisGraphQL sections:

```json
{
  "Axlis": {
    "CacheTtl": "00:30:00",
    "EnableDiagnostics": true
  },
  "AxlisGraphQL": {
    "Endpoint": "",
    "BatchSize": 10,
    "TimeoutSeconds": 30
  }
}
```

The `Endpoint` and `ApiKey` values are intentionally empty in `appsettings.json`. Set them via user-secrets:

```bash
dotnet user-secrets set "AxlisGraphQL:Endpoint" "https://your-sitecore-instance/sitecore/api/graph/edge"
dotnet user-secrets set "AxlisGraphQL:ApiKey" "{YOUR-API-KEY}"
```

### PowerCSharp Features

The `Samples` feature flag must be enabled to access the showcase endpoint:

```json
{
  "PowerFeatures": {
    "Samples": {
      "Enabled": true
    }
  }
}
```

This is already set to `true` in `appsettings.json` for this sample.

---

## Axlis Package Versions

This sample uses the published NuGet packages from nuget.org:

- `Axlis` v0.1.0
- `Axlis.Abstractions` v0.1.0
- `Axlis.Core` v0.1.0
- `Axlis.GraphQL` v0.1.0

All packages are configured in the respective project files:
- `src/CleanArchitecture.Application/CleanArchitecture.Application.csproj` — references `Axlis.Core`
- `src/CleanArchitecture.WebApi/CleanArchitecture.WebApi.csproj` — references `Axlis` and `Axlis.GraphQL`

---

## Template POCO GUIDs

The sample template POCOs use placeholder GUIDs. Replace these with the real template IDs from your Sitecore instance:

**System Templates:**
- `Language.cs` — `{F68F13A6-3395-426A-B9A1-FA2DC60D94EB}`
- `MainSection.cs` — `{E3E2D58C-DF95-4230-ADC9-279924CECE84}`
- `Node.cs` — `{239F9CF4-E5A0-44E0-B342-0F32CD4C6D8B}`
- `PublishingTarget.cs` — `{E130C748-C13B-40D5-B6C6-4B150DC3FAB3}`

**Sample Templates:**
- `SampleItem.cs` — `{76036F5E-C477-44E2-8178-773413C533F7}`

To find your template GUIDs in Sitecore:
1. Navigate to the template in the Content Editor
2. Click the item and view the Quick Info section
3. Copy the ID value

---

## Axlis Documentation

For detailed Axlis documentation, see:
- [Axlis GitHub Repository](https://github.com/marioarce/Axlis)
- [Axlis NuGet Packages](https://www.nuget.org/packages/Axlis/)

### Key Axlis Concepts

**Clean API vs Rich API:**
- `GetItemByPathAsync<T>()` — Returns T? (null if not found). Use for simple fetches.
- `GetItemByPathWithResultAsync<T>()` — Returns `AxlisResult<T>` with Value, Metadata, and Diagnostics. Use for troubleshooting or when you need provenance data.

**Lazy Loading:**
- Axes traversal (Parent, Children, Siblings) may trigger lazy-fetches if data wasn't included in the initial GraphQL response
- Lazy fetches are synchronous (property getter cannot be async)
- Safe in ASP.NET Core, but avoid deep traversal in non-ASP environments
- Use `GetItemsByPathsAsync` for batch pre-fetch in performance-critical scenarios

**Caching:**
- Powered by `ICacheService` from PowerCSharp.Feature.Cache.Abstractions
- Dual-indexing (by ID and path) with null-safety (null results never cached)
- Configure CacheTtl in `AddAxlis` options (default: 60 minutes)
- Use `SitecoreItemCacheManager.InvalidateAsync(key)` to evict specific items

**Field Types:**
- `TextField` — Simple text fields
- `ImageField` — Image metadata (Src, Alt, etc.)
- `MultilistField` — Multi-select item references
- `ItemReferenceField` — Single item reference
- All fields inherit from `BaseField` with FieldName, RawValue, and IsEmpty

---

## License

This project is licensed under the [MIT License](LICENSE).

---

## Documentation Files

- `README.md` — This file (main project documentation)
- `CHANGELOG.AXLIS.md` — Axlis-specific changelog
- `CONTRIBUTING.AXLIS.md` — Contribution guidelines for this sample
- `SECURITY.AXLIS.md` — Security considerations for Axlis integration

# Axlis.CleanArchitecture.Sample

> Sample Clean Architecture .NET 8 WebApi demonstrating the Axlis Sitecore Headless GraphQL ORM.

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

This is a **sample project**, not a template. It contains dev-only ProjectReferences to the Axlis packages for rapid iteration during development. Before using this as a reference for production, replace the ProjectReferences with NuGet PackageReferences (see the `// fixme` markers in the code).

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
- **Dev-only ProjectReferences** to Axlis packages (marked `// fixme` for NuGet swap)

### Sitecore Template POCOs
Located in `src/CleanArchitecture.Application/Sitecore/Templates/`:
- **Disclaimer** — TextField Heading/Description
- **HomePage** — ImageField MetaThumbnail + MultilistField HeadCssLinks
- **PresentationAssetLink** — TextField Link
- **DictionaryRoot** — ItemReferenceField FallbackDomain
- **Style** — typed predicate for Axes.GetChildren/GetDescendants

### CQRS Showcase Endpoint
`GET /v1/sitecore/showcase` exercises six Axlis API pivots:
1. **TextField** — Disclaimer Heading/Description
2. **ImageField + MultilistField** — HomePage MetaThumbnail + HeadCssLinks
3. **ItemReferenceField** — Dictionary FallbackDomain
4. **Axes traversal** — Parent, Children, Grandparent, Siblings, typed GetChildren
5. **GetDescendants** — recursive typed traversal
6. **WithResult rich API** — value, metadata, diagnostics

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

## Dev-Only References

This sample uses **ProjectReferences** to the Axlis packages for rapid iteration during development. These are marked with `// fixme` comments and must be replaced with NuGet PackageReferences before using this code in production:

- `src/CleanArchitecture.Application/CleanArchitecture.Application.csproj` — references `Axlis.Core`
- `src/CleanArchitecture.WebApi/CleanArchitecture.WebApi.csproj` — references `Axlis` and `Axlis.GraphQL`

Replace the ProjectReference items with PackageReference items when preparing for release:

```xml
<!-- Replace this dev-only ProjectReference -->
<ProjectReference Include="..\..\..\Axlis\src\Axlis.Core\Axlis.Core.csproj" />

<!-- With this NuGet PackageReference -->
<PackageReference Include="Axlis.Core" Version="0.1.0-preview" />
```

---

## Template POCO GUIDs

The sample template POCOs use placeholder GUIDs marked with `// fixme`. Replace these with the real template IDs from your Sitecore instance:

- `Disclaimer.cs` — `{00000000-0000-0000-0000-000000000001}`
- `HomePage.cs` — `{00000000-0000-0000-0000-000000000002}`
- `PresentationAssetLink.cs` — `{00000000-0000-0000-0000-000000000003}`
- `DictionaryRoot.cs` — `{00000000-0000-0000-0000-000000000004}`
- `Style.cs` — `{00000000-0000-0000-0000-000000000005}`

---

## License

This project is licensed under the [MIT License](LICENSE).

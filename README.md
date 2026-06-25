# PowerCSharp.CleanArchitecture

> Clean Architecture .NET 8 WebApi starter template powered by PowerCSharp —  
> start your next project on a production-ready foundation, not from scratch.

[![CI](https://github.com/marioarce/PowerCSharp.CleanArchitecture/actions/workflows/ci.yml/badge.svg)](https://github.com/marioarce/PowerCSharp.CleanArchitecture/actions/workflows/ci.yml)
[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](LICENSE)
[![.NET 8](https://img.shields.io/badge/.NET-8.0-purple)](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
[![PowerCSharp](https://img.shields.io/badge/powered%20by-PowerCSharp-orange)](https://www.nuget.org/profiles/marioalbertoarce)

---

## What is this?

**PowerCSharp.CleanArchitecture** is a production-ready starter template for building ASP.NET Core Web APIs following Clean Architecture principles. It is opinionated by design: the architectural decisions, folder conventions, dependency rules, and cross-cutting concerns are already solved so you can focus on shipping your domain logic.

This repo is:
- A **GitHub Template Repository** — click "Use this template" to generate your own repo pre-populated with the full structure.
- A **`dotnet new` template** — scaffold locally with a single command (see [Using as a Template](#using-as-a-template)).

---

## Architecture Overview

The solution follows a strict **dependency rule**: inner layers never reference outer layers. Dependencies always point inward.

```
┌──────────────────────────────────────────────┐
│                   WebApi                     │  Composition root · host · middleware pipeline
├──────────────────────────────────────────────┤
│                 Presentation                 │  Controllers · HTTP response mapping · ApiResponse<T>
├─────────────────────────┬────────────────────┤
│       Application       │    Operational     │  CQRS · MediatR · validators   │   Resilience · Polly
├─────────────────────────┴────────────────────┤
│               Infrastructure                 │  Adapters · external services · DI wiring
├──────────────────────┬───────────────────────┤
│        Domain        │        Shared         │  Entities · interfaces · aggregates   │   Enums · constants
└──────────────────────┴───────────────────────┘
```

| Layer | Project | Responsibility |
|---|---|---|
| **Domain** | `CleanArchitecture.Domain` | Core business entities (`BaseEntity<TId>`), aggregate root marker (`IAggregateRoot`). Zero external dependencies. |
| **Shared** | `CleanArchitecture.Shared` | Enums and constants shared across layers without creating circular dependencies. |
| **Application** | `CleanArchitecture.Application` | Use cases via CQRS (MediatR). Commands, Queries, Handlers, Validators (FluentValidation), pipeline behaviors. |
| **Operational** | `CleanArchitecture.Operational` | Cross-cutting resilience concerns: retry policies via Polly (`IRetryPolicyProvider`). |
| **Infrastructure** | `CleanArchitecture.Infrastructure` | Concrete adapters: `DateTimeProvider`, Operational wiring. Third-party integrations live here. |
| **Presentation** | `CleanArchitecture.Presentation` | API controllers (extend `BaseApiController`), `ApiResponse<T>` envelope, `[FeatureGate]` attribute. |
| **WebApi** | `CleanArchitecture.WebApi` | Composition root — wires all layers, configures PowerCSharp Features, registers middleware. |

---

## Project Structure

```
PowerCSharp.CleanArchitecture/
├── src/
│   ├── CleanArchitecture.Domain/           # Entities, aggregate root marker
│   ├── CleanArchitecture.Shared/           # Cross-layer enums & constants
│   ├── CleanArchitecture.Application/      # CQRS handlers, validators, behaviors
│   │   ├── Abstractions/                   # Ports (e.g. IDateTimeProvider)
│   │   ├── Api/
│   │   │   ├── Samples/                    # Living-doc sample use cases
│   │   │   └── Shared/                     # Shared application data/factories
│   │   └── Common/
│   │       ├── Behaviors/                  # MediatR pipeline behaviors
│   │       └── Handlers/                   # Shared handler utilities
│   ├── CleanArchitecture.Operational/      # Resilience (Polly retry pipelines)
│   ├── CleanArchitecture.Infrastructure/   # Adapters (DateTimeProvider, etc.)
│   ├── CleanArchitecture.Presentation/     # Controllers, ApiResponse<T> envelope
│   │   ├── Api/                            # Response types, attributes
│   │   ├── Controllers/
│   │   │   └── v1/                         # Versioned controllers (SamplesController)
│   │   └── Extensions/                     # Result → IActionResult mapping
│   └── CleanArchitecture.WebApi/           # Host: Program.cs, appsettings, Features
│       └── Features/                       # Host-local feature modules
├── tests/
│   ├── CleanArchitecture.Tests.Shared/     # Shared test utilities & fixtures
│   ├── CleanArchitecture.WebApi.UnitTests/ # Unit tests
│   └── CleanArchitecture.WebApi.IntegrationTests/ # Integration tests (WebApplicationFactory)
├── .github/
│   ├── workflows/
│   │   ├── ci.yml                          # Build + test on push / PR
│   │   └── release.yml                     # Tag-triggered release & NuGet publish
│   ├── ISSUE_TEMPLATE/
│   └── pull_request_template.md
├── .template.config/
│   └── template.json                       # dotnet new template definition
├── Directory.Build.props                   # Shared build settings (TargetFramework, Nullable, etc.)
├── NuGet.Config                            # Package sources (nuget.org only)
├── global.json                             # Pinned .NET SDK version
├── Dockerfile                              # Multi-stage production image
├── docker-compose.yml                      # Local development stack
└── PowerCSharp.CleanArchitecture.sln
```

---

## What's Included

### Clean Architecture
- **Strict dependency rule** enforced by project references — outer layers cannot leak into inner layers.
- **`BaseEntity<TId>`** — generic, strongly-typed entity base in the Domain layer.
- **`IAggregateRoot`** — marker interface for aggregate roots.

### CQRS via MediatR
- Commands and Queries separated into dedicated folders.
- `BaseApiController.SendAsync<T>()` dispatches through MediatR and maps the result to HTTP.
- **`LoggingBehavior<TRequest, TResponse>`** — MediatR pipeline behavior that logs every request/response automatically.

### Validation via FluentValidation
- Validators registered automatically via assembly scanning.
- Integrated with MediatR pipeline — invalid requests are rejected before reaching handlers.

### Consistent HTTP Responses
- **`ApiResponse<T>`** / **`ApiResponse`** envelope on every endpoint.
- `Ardalis.Result` used in handlers; extension method maps it to the correct HTTP status code automatically.

### PowerCSharp Features Framework
Pluggable feature modules with flag-gating (`PowerFeatures:<Key>:Enabled` in `appsettings.json`):

| Feature | Package | What it gives you |
|---|---|---|
| **CORS** | `PowerCSharp.BuiltInFeatures` | Configurable CORS policy, flag-gated |
| **Cache (BitFaster)** | `PowerCSharp.Feature.Cache.BitFaster` | High-performance in-memory cache via `ICacheService` |
| **Cache (Disk)** | `PowerCSharp.Feature.Cache.Disk` | Persistent disk cache via `IDiskCacheService` |
| **Samples** | host-local module | Living-documentation endpoints at `/v1/samples` (flag-gated, disabled in production) |

### Resilience
- **Polly** retry policies via `IRetryPolicyProvider` in the `Operational` layer — ready to wire into any infrastructure adapter.

### Health Checks
- `/health` endpoint registered and mapped out of the box.

### Living-Documentation Samples
The `Samples` feature (disabled by default, enabled in Development) ships working endpoint examples for:
- `GET /v1/samples/cache` — cache miss → hit demonstration
- `GET /v1/samples/cache/status` — inspect cache keys
- `DELETE /v1/samples/cache` — clear cache
- `GET /v1/samples/disk-cache` — disk cache demonstration
- Full integration tests covering the above

---

## Prerequisites

| Tool | Version |
|---|---|
| [.NET SDK](https://dotnet.microsoft.com/download/dotnet/8.0) | 8.0 or later |
| [Docker](https://www.docker.com/) *(optional)* | 20.10+ |
| Git | any recent version |

---

## Quick Start

### Clone and run

```bash
git clone https://github.com/marioarce/PowerCSharp.CleanArchitecture.git
cd PowerCSharp.CleanArchitecture

dotnet restore
dotnet build
dotnet run --project src/CleanArchitecture.WebApi
```

The API starts on `https://localhost:7xxx` / `http://localhost:5xxx`. Open the Swagger UI at `/swagger`.

### Run with Docker

```bash
docker compose up --build
```

The API is available at `http://localhost:8080`.

---

## Using as a Template

### Option A — GitHub Template (recommended for new repos)

1. Click **"Use this template"** at the top of this page.
2. Name your new repository and choose visibility.
3. GitHub creates a new repo with the full project structure — no fork, clean history.

### Option B — `dotnet new` (scaffold locally)

```bash
# Install the template once
dotnet new install PowerCSharp.CleanArchitecture.Template

# Scaffold a new project
dotnet new powercsharp-cleanarchitecture --name MyAwesomeApi --output ./MyAwesomeApi
cd MyAwesomeApi
dotnet run --project src/MyAwesomeApi.WebApi
```

`--name` replaces `CleanArchitecture` everywhere: filenames, namespaces, project references, and the solution file.

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

Key feature flags in `appsettings.json`:

```json
{
  "PowerFeatures": {
    "Cors": { "Enabled": true, "AllowedOrigins": [ "https://localhost" ] },
    "Cache": { "Enabled": true, "Provider": "BitFaster", "Capacity": 1000 },
    "DiskCache": { "Enabled": true, "DirectoryPath": "./cache" },
    "Samples": { "Enabled": false }
  }
}
```

`appsettings.Development.json` overrides `Samples.Enabled` to `true` so the living-documentation endpoints are visible locally but hidden in production.

---

## PowerCSharp Ecosystem

All packages are available on [nuget.org](https://www.nuget.org/profiles/marioalbertoarce):

| Package | Description |
|---|---|
| [`PowerCSharp.Features`](https://www.nuget.org/packages/PowerCSharp.Features) | Feature module system & flag resolution |
| [`PowerCSharp.BuiltInFeatures`](https://www.nuget.org/packages/PowerCSharp.BuiltInFeatures) | CORS and other built-in feature modules |
| [`PowerCSharp.Feature.Cache`](https://www.nuget.org/packages/PowerCSharp.Feature.Cache) | Cache feature module |
| [`PowerCSharp.Feature.Cache.Abstractions`](https://www.nuget.org/packages/PowerCSharp.Feature.Cache.Abstractions) | `ICacheService` / `IDiskCacheService` contracts |
| [`PowerCSharp.Feature.Cache.BitFaster`](https://www.nuget.org/packages/PowerCSharp.Feature.Cache.BitFaster) | High-performance in-memory cache provider |
| [`PowerCSharp.Feature.Cache.Disk`](https://www.nuget.org/packages/PowerCSharp.Feature.Cache.Disk) | Persistent disk cache provider |

---

## Contributing

Contributions are welcome. Please read [CONTRIBUTING.md](CONTRIBUTING.md) before opening a pull request.

---

## License

This project is licensed under the [MIT License](LICENSE).  
Copyright (c) 2026 Mario Alberto Arce.

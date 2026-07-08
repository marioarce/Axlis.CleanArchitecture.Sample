# Contributing to Axlis.CleanArchitecture.Sample

Thank you for your interest in contributing to this Axlis sample project. This document covers how to propose changes, coding standards, and the review process specific to this demonstration project.

---

## Table of Contents

- [Code of Conduct](#code-of-conduct)
- [How to Contribute](#how-to-contribute)
- [Development Setup](#development-setup)
- [Coding Standards](#coding-standards)
- [Commit Message Format](#commit-message-format)
- [Pull Request Guidelines](#pull-request-guidelines)
- [Reporting Bugs](#reporting-bugs)
- [Requesting Features](#requesting-features)

---

## Code of Conduct

This project follows the [Contributor Covenant Code of Conduct](CODE_OF_CONDUCT.md). By participating, you agree to uphold it. Please report unacceptable behavior to the project maintainer.

---

## How to Contribute

1. **Search existing issues** before opening a new one — your question or bug may already be tracked.
2. **Open an issue** for non-trivial changes before writing code. This allows design discussion and avoids wasted effort.
3. **Fork** the repository and create a dedicated branch from `main`:
   ```bash
   git checkout -b feat/my-feature
   ```
4. **Implement** your change following the coding standards below.
5. **Test** your change — all existing tests must pass and new behavior should have test coverage.
6. **Push** your branch and open a **Pull Request** against `main`.

---

## Development Setup

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- Git
- Sitecore Headless GraphQL endpoint (for integration testing)

### Steps

```bash
git clone https://github.com/marioarce/Axlis.CleanArchitecture.Sample.git
cd Axlis.CleanArchitecture.Sample

dotnet restore
dotnet build
dotnet test
```

All Axlis packages resolve from nuget.org — no additional local feed setup is required.

### Configure Sitecore Connection

Set user-secrets for the Sitecore GraphQL endpoint:

```bash
cd src/CleanArchitecture.WebApi
dotnet user-secrets set "AxlisGraphQL:Endpoint" "https://your-sitecore-instance/sitecore/api/graph/edge"
dotnet user-secrets set "AxlisGraphQL:ApiKey" "{YOUR-API-KEY}"
```

---

## Coding Standards

This repo follows the conventions defined in `.editorconfig`. Key rules:

- **File-scoped namespaces** — required (`namespace Foo;` not `namespace Foo { ... }`).
- **Explicit accessibility modifiers** — always required (`public`, `private`, etc.).
- **Nullable reference types** — enabled globally; no `!` suppressions without justification.
- **No `this.` prefix** — avoid qualifying members with `this.`.
- **`var`** — use only when the type is evident from the right-hand side.
- **XML doc comments** — required on all `public` members.
- **No trailing whitespace**, LF line endings, UTF-8 encoding.

The build enforces code style (`EnforceCodeStyleInBuild = true`). A build that introduces style violations will fail.

### Axlis-Specific Guidelines

- **Template POCOs**: All Sitecore template classes must inherit from `ExtendedItem` and use `[SitecoreTemplate]` attribute
- **Field Properties**: Use `[SitecoreField]` attribute with the exact Sitecore field name (case-insensitive)
- **XML Documentation**: Provide comprehensive XML docs for all template classes explaining their purpose and fields
- **Handler Documentation**: Add developer notes (`/* DevNote: */`) explaining Axlis API usage patterns

---

## Commit Message Format

```
<scope>: <description>

<changes>
```

- **Scope** is required. Use the area being changed (e.g. `docs`, `axlis`, `template`, `chore`, `feat`, `fix`).
- **Description** is a short imperative sentence (no period at end).
- **Changes** are bullet points (`- `) summarising what was done.
- No emojis in commit messages.

Example:

```
feat: add Language template POCO

- Add Language class inheriting from ExtendedItem
- Add fields: Charset, CodePage, Dictionary, Encoding, FallbackLanguage, Iso, RegionalIsoCode
- Add comprehensive XML documentation
- Update GetSitecoreShowcaseQueryHandler to use Language template
```

---

## Pull Request Guidelines

- Keep PRs **focused** — one logical change per PR.
- Fill out the [PR template](.github/pull_request_template.md) completely.
- Ensure all CI checks pass before requesting review.
- Update `CHANGELOG.AXLIS.md` under the `[Unreleased]` section.
- Do not force-push to a PR branch after review has started.
- For template-related changes, ensure they don't conflict with the base PowerCSharp.CleanArchitecture.Template structure.

---

## Reporting Bugs

Use the [Bug Report issue template](.github/ISSUE_TEMPLATE/bug_report.yml). Include:

- Steps to reproduce
- Expected vs actual behaviour
- .NET SDK version and OS
- Sitecore version (if applicable)
- Axlis package versions

---

## Requesting Features

Use the [Feature Request issue template](.github/ISSUE_TEMPLATE/feature_request.yml). Explain:

- The problem you are trying to solve
- The proposed solution
- Alternatives you have considered
- How this feature would benefit the Axlis community

---

## Documentation Strategy

This project uses a `.AXLIS.md` suffix for documentation files that are specific to this sample project:

- `CHANGELOG.AXLIS.md` - Axlis-specific changelog (this file)
- `CONTRIBUTING.AXLIS.md` - Axlis-specific contribution guidelines (this file)
- `SECURITY.AXLIS.md` - Axlis-specific security policy
- `README.md` - Main project documentation (already Axlis-specific)

The original template files (`CHANGELOG.md`, `CONTRIBUTING.md`, `SECURITY.md`) are preserved from the base PowerCSharp.CleanArchitecture.Template for comparison and reference when merging template updates.

When updating from the base template, merge the original files and review the `.AXLIS.md` files separately to ensure Axlis-specific content is preserved.

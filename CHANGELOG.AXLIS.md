# Changelog - Axlis.CleanArchitecture.Sample

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

---

## [Unreleased]

---

## [0.1.0] - 2026-07-08

### Added

- Initial public release of Axlis.CleanArchitecture.Sample
- Axlis v0.1.0 integration (Axlis, Axlis.Abstractions, Axlis.Core, Axlis.GraphQL)
- Sitecore template POCOs: SampleItem, Language, MainSection, Node, PublishingTarget
- CQRS showcase endpoint `/v1/sitecore/showcase` demonstrating six Axlis API pivots:
  - TextField access (SampleItem.Title, SampleItem.Text)
  - Axes traversal (Parent, Children, Grandparent, Siblings)
  - GetDescendants with template filtering (Language items)
  - WithResult rich API (Metadata and DiagnosticsData)
- Comprehensive XML documentation for all Sitecore template classes
- Developer notes in GetSitecoreShowcaseQueryHandler explaining Axlis API usage
- Updated NuGet package references from preview versions to stable v0.1.0
- Removed local NuGet source from NuGet.Config (now uses only nuget.org)
- Documentation strategy: .AXLIS.md suffix files for Axlis-specific documentation

### Changed

- Updated dependency versions:
  - Ardalis.Result: 9.1.0 → 10.1.0
  - FluentValidation: 11.9.2 → 12.1.1
  - MediatR: 12.3.0 → 14.1.0
  - Microsoft.Extensions.* packages: 8.0.x → 10.0.0
  - PowerCSharp.Feature.Cache: 1.3.1 → 1.3.2
- Fixed GetSitecoreShowcaseQuery to use request.RootPath parameter instead of hardcoded path
- Fixed typo in GetSitecoreShowcaseResponse: "PAxes" → "Axes"

### Documentation

- Added comprehensive XML documentation to all Sitecore template classes
- Added detailed developer notes in GetSitecoreShowcaseQueryHandler explaining:
  - ISitecoreFacade usage and setup
  - GetItemByPathAsync vs GetItemByPathWithResultAsync
  - Template POCO field access patterns
  - Axes traversal and lazy-loading behavior
  - GetDescendants performance considerations
  - Caching strategy and configuration
- Enhanced pivot class documentation in GetSitecoreShowcaseResponse

[Unreleased]: https://github.com/marioarce/Axlis.CleanArchitecture.Sample/compare/v0.1.0...HEAD
[0.1.0]: https://github.com/marioarce/Axlis.CleanArchitecture.Sample/releases/tag/v0.1.0

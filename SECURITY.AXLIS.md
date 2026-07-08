# Security Policy - Axlis.CleanArchitecture.Sample

## Supported Versions

| Version | Supported |
|---------|-----------|
| Latest (`main`) | Yes |
| Older tags | No — please upgrade to the latest version |

This is a sample/demo project demonstrating Axlis integration. Security fixes are applied to the `main` branch and released as new versions. Projects using this as a reference should review and adapt security practices for their specific use case.

## Reporting a Vulnerability

**Do not open a public GitHub issue for security vulnerabilities.**

Please report security vulnerabilities via **GitHub's private security advisory** feature:

1. Go to the [Security tab](https://github.com/marioarce/Axlis.CleanArchitecture.Sample/security) of this repository.
2. Click **"Report a vulnerability"**.
3. Fill in the details — include steps to reproduce, potential impact, and any suggested mitigations.

You will receive an acknowledgement within **72 hours**. We aim to provide a fix or mitigation plan within **14 days** for critical issues.

## Security Considerations for Axlis Integration

This sample project demonstrates Axlis Sitecore Headless GraphQL integration. When adapting this sample for production, consider the following security aspects:

### Sitecore GraphQL Configuration

- **API Key Management**: The sample uses user-secrets for the Sitecore GraphQL API key. In production:
  - Use Azure Key Vault, AWS Secrets Manager, or your organization's secret management solution
  - Never commit API keys to source control
  - Rotate API keys regularly
  - Use separate API keys for different environments (dev, staging, production)

- **Endpoint Security**: 
  - Ensure your Sitecore GraphQL endpoint uses HTTPS
  - Configure IP whitelisting on the Sitecore GraphQL endpoint if possible
  - Use Sitecore's built-in authentication and authorization for GraphQL

### Axlis-Specific Security

- **Caching**: Axlis uses caching to improve performance. Ensure:
  - Cache TTL is appropriate for your content freshness requirements
  - Cache invalidation is implemented for published content changes
  - Sensitive data is not cached longer than necessary

- **Lazy Loading**: Axes traversal can trigger lazy-fetches. In production:
  - Be aware of potential performance implications
  - Consider pre-fetching strategies for frequently accessed content
  - Monitor GraphQL query complexity and rate limits

- **Diagnostics**: The sample has diagnostics enabled. In production:
  - Disable diagnostics (`EnableDiagnostics: false`) in high-throughput scenarios
  - Diagnostic events may contain sensitive information in error messages
  - Review and sanitize diagnostic logs before storage

### Sample Endpoints

This sample includes demo endpoints that may expose or mutate server-side state:

- `/v1/sitecore/showcase` - Demonstrates Axlis API usage
- `/v1/samples/*` - PowerCSharp cache demo endpoints

These endpoints are:
- **Flag-gated** (`PowerFeatures:Samples:Enabled`): disabled by default, enabled in `Development` only
- **Not protected by authentication or authorization** — they are demos, not production code

Before going to production:
- Disable or remove sample/demo endpoints
- Add proper authentication (e.g., JWT Bearer) and authorization policies
- Review and scope the CORS policy to your actual front-end origins
- Do not expose internal cache state or destructive operations without authorization

### Dependency Security

Dependencies are monitored via [Dependabot](.github/dependabot.yml). Pull requests for security updates are created automatically and prioritized for review.

Key dependencies to monitor:
- Axlis packages (Axlis, Axlis.Abstractions, Axlis.Core, Axlis.GraphQL)
- Sitecore GraphQL client libraries
- PowerCSharp packages

### Clean Architecture Security

This sample follows Clean Architecture principles which provide inherent security benefits:

- **Dependency Rule**: Dependencies point inward, preventing external libraries from directly accessing inner layers
- **Domain Layer Purity**: The Domain layer contains no external dependencies, making it inherently secure
- **Infrastructure Isolation**: External concerns (Sitecore, caching, HTTP) are isolated in the Infrastructure layer

When adapting this sample, maintain these architectural boundaries to preserve security benefits.

## Additional Resources

- [Axlis Documentation](https://github.com/marioarce/Axlis)
- [Sitecore Security Best Practices](https://doc.sitecore.com/)
- [OWASP ASP.NET Security Cheat Sheet](https://cheatsheetseries.owasp.org/cheatsheets/ASP.NET_Security_Cheat_Sheet.html)

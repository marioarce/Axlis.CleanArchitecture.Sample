using Axlis.Attributes;
using Axlis.Core;

namespace CleanArchitecture.Application.Sitecore.Templates;

/// <summary>
/// Represents the Sitecore Style template.
/// Used as a typed predicate target for Axes traversal (GetChildren, GetDescendants).
/// </summary>
// fixme: replace the placeholder GUID with the real template ID from your Sitecore instance
[SitecoreTemplate("{00000000-0000-0000-0000-000000000005}")]
public class Style : ExtendedItem
{
}

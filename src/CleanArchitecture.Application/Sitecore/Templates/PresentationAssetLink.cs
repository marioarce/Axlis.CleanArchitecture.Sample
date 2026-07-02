using Axlis.Attributes;
using Axlis.Core;
using Axlis.Core.FieldTypes;

namespace CleanArchitecture.Application.Sitecore.Templates;

/// <summary>
/// Represents the Sitecore PresentationAssetLink template (e.g. a CSS or JS asset link item).
/// </summary>
// fixme: replace the placeholder GUID with the real template ID from your Sitecore instance
[SitecoreTemplate("{00000000-0000-0000-0000-000000000003}")]
public class PresentationAssetLink : ExtendedItem
{
    /// <summary>The URL of the asset.</summary>
    [SitecoreField("Link")]
    public TextField Link => GetField<TextField>("Link");
}

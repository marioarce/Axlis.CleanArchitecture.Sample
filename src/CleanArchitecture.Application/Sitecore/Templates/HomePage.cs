using Axlis.Attributes;
using Axlis.Core;
using Axlis.Core.FieldTypes;

namespace CleanArchitecture.Application.Sitecore.Templates;

/// <summary>
/// Represents the Sitecore Home page template.
/// Demonstrates <see cref="ImageField"/> and <see cref="MultilistField"/> field access.
/// </summary>
// fixme: replace the placeholder GUID with the real template ID from your Sitecore instance
[SitecoreTemplate("{00000000-0000-0000-0000-000000000002}")]
public class HomePage : ExtendedItem
{
    /// <summary>The meta thumbnail image for the page.</summary>
    [SitecoreField("MetaThumbnail")]
    public ImageField MetaThumbnail => GetField<ImageField>("MetaThumbnail");

    /// <summary>CSS asset links referenced in the page head.</summary>
    [SitecoreField("HeadCSSLinks")]
    public MultilistField HeadCssLinks => GetField<MultilistField>("HeadCSSLinks");
}

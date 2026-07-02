using Axlis.Attributes;
using Axlis.Core;
using Axlis.Core.FieldTypes;

namespace CleanArchitecture.Application.Sitecore.Templates;

/// <summary>
/// Represents the Sitecore Disclaimer template.
/// </summary>
// fixme: replace the placeholder GUID with the real template ID from your Sitecore instance
[SitecoreTemplate("{00000000-0000-0000-0000-000000000001}")]
public class Disclaimer : ExtendedItem
{
    /// <summary>The disclaimer heading text.</summary>
    [SitecoreField("Heading")]
    public TextField Heading => GetField<TextField>("Heading");

    /// <summary>The disclaimer body text.</summary>
    [SitecoreField("Description")]
    public TextField Description => GetField<TextField>("Description");
}

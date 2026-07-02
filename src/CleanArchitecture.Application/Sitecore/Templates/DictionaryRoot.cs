using Axlis.Attributes;
using Axlis.Core;
using Axlis.Core.FieldTypes;

namespace CleanArchitecture.Application.Sitecore.Templates;

/// <summary>
/// Represents the Sitecore Dictionary root item template.
/// Demonstrates <see cref="ItemReferenceField"/> field access.
/// </summary>
// fixme: replace the placeholder GUID with the real template ID from your Sitecore instance
[SitecoreTemplate("{00000000-0000-0000-0000-000000000004}")]
public class DictionaryRoot : ExtendedItem
{
    /// <summary>The fallback domain for dictionary lookups.</summary>
    [SitecoreField("FallbackDomain")]
    public ItemReferenceField FallbackDomain => GetField<ItemReferenceField>("FallbackDomain");
}

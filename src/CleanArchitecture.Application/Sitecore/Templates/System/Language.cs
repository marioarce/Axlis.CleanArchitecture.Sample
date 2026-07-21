using Axlis.ORM.Attributes;
using Axlis.ORM.Core;
using Axlis.ORM.Core.FieldTypes;

namespace CleanArchitecture.Application.Sitecore.Templates.System;

/// <summary>
/// Represents a language template from Sitecore.
/// Contains language-specific settings including charset, encoding, and ISO codes.
/// </summary>
[SitecoreTemplate("{F68F13A6-3395-426A-B9A1-FA2DC60D94EB}")]
public class Language : ExtendedItem
{
    /// <summary>Gets the charset field for the language.</summary>
    [SitecoreField("Charset")]
    public TextField Charset => GetField<TextField>("Charset");

    /// <summary>Gets the code page field for the language.</summary>
    [SitecoreField("Code page")]
    public TextField CodePage => GetField<TextField>("Code page");

    /// <summary>Gets the dictionary field for the language.</summary>
    [SitecoreField("Dictionary")]
    public TextField Dictionary => GetField<TextField>("Dictionary");

    /// <summary>Gets the encoding field for the language.</summary>
    [SitecoreField("Encoding")]
    public TextField Encoding => GetField<TextField>("Encoding");

    /// <summary>Gets the fallback language reference field.</summary>
    [SitecoreField("FallbackLanguage")]
    public ItemReferenceField FallbackLanguage => GetField<ItemReferenceField>("FallbackLanguage");

    /// <summary>Gets the ISO code field for the language.</summary>
    [SitecoreField("Iso")]
    public TextField Iso => GetField<TextField>("Iso");

    /// <summary>Gets the regional ISO code field for the language.</summary>
    [SitecoreField("Regional Iso Code")]
    public TextField RegionalIsoCode => GetField<TextField>("Regional Iso Code");
}

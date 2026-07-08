using Axlis.Attributes;
using Axlis.Core;
using Axlis.Core.FieldTypes;

namespace CleanArchitecture.Application.Sitecore.Templates.System;

/// <summary>Language template from Sitecore.</summary>
[SitecoreTemplate("{F68F13A6-3395-426A-B9A1-FA2DC60D94EB}")]
public class Language : ExtendedItem
{
    /// <summary>Charset field.</summary>
    [SitecoreField("Charset")]
    public TextField Charset => GetField<TextField>("Charset");

    /// <summary>Code page field.</summary>
    [SitecoreField("Code page")]
    public TextField CodePage => GetField<TextField>("Code page");

    /// <summary>Dictionary field.</summary>
    [SitecoreField("Dictionary")]
    public TextField Dictionary => GetField<TextField>("Dictionary");

    /// <summary>Encoding field.</summary>
    [SitecoreField("Encoding")]
    public TextField Encoding => GetField<TextField>("Encoding");

    /// <summary>Fallback language field.</summary>
    [SitecoreField("FallbackLanguage")]
    public ItemReferenceField FallbackLanguage => GetField<ItemReferenceField>("FallbackLanguage");

    /// <summary>ISO code field.</summary>
    [SitecoreField("Iso")]
    public TextField Iso => GetField<TextField>("Iso");

    /// <summary>Regional ISO code field.</summary>
    [SitecoreField("Regional Iso Code")]
    public TextField RegionalIsoCode => GetField<TextField>("Regional Iso Code");
}

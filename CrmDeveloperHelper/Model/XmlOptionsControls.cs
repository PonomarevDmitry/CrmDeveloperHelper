using System;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Model
{
    [Flags]
    public enum XmlOptionsControls
    {
        None = 0,

        XmlAttributeOnNewLine = 1,

        SetXmlSchemas = 2,

        SetIntellisenseContext = 4,

        Ribbon = 8,

        XmlSimple = SetIntellisenseContext | SetXmlSchemas,

        XmlFull = SetIntellisenseContext | SetXmlSchemas | XmlAttributeOnNewLine,

        All = Ribbon | SetIntellisenseContext | SetXmlSchemas | XmlAttributeOnNewLine,
    }
}
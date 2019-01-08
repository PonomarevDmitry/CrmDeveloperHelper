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

        RibbonFilters = 8,

        SortXmlAttributes = 16,

        SortRibbonCommandsAndRulesById = 32,

        SortFormXmlElements = 64,

        SolutionComponentWithManagedInfo = 128,

        SolutionComponentWithSolutionInfo = 256,

        SolutionComponentWithUrl = 512,

        XmlSimple = SetIntellisenseContext | SetXmlSchemas,

        XmlFull = SetIntellisenseContext | SetXmlSchemas | XmlAttributeOnNewLine | SortXmlAttributes,

        FormXml = XmlFull | SortFormXmlElements,

        RibbonFull = RibbonFilters | SetIntellisenseContext | SetXmlSchemas | XmlAttributeOnNewLine | SortRibbonCommandsAndRulesById | SortXmlAttributes,

        SolutionComponentSettings = SolutionComponentWithManagedInfo | SolutionComponentWithSolutionInfo | SolutionComponentWithUrl,
    }
}
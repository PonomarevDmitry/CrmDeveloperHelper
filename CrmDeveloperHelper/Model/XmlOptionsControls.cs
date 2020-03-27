using System;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Model
{
    [Flags]
    public enum XmlOptionsControls
    {
        None = 0,

        XmlAttributeOnNewLine = 0b1,

        SetXmlSchemas = 0b10,

        SetIntellisenseContext = 0b100,

        RibbonFilters = 0b1000,

        SortXmlAttributes = 0b1_0000,

        SortRibbonCommandsAndRulesById = 0b10_0000,

        SortFormXmlElements = 0b100_0000,

        SolutionComponentWithManagedInfo = 0b1000_0000,

        SolutionComponentWithSolutionInfo = 0b1_0000_0000,

        SolutionComponentWithUrl = 0b10_0000_0000,

        XmlSimple = SetIntellisenseContext | SetXmlSchemas,

        XmlFull = SetIntellisenseContext | SetXmlSchemas | XmlAttributeOnNewLine | SortXmlAttributes,

        CustomControlXmlOptions = XmlFull,

        ImportJobXmlOptions = XmlAttributeOnNewLine | SortXmlAttributes,

        OrganizationXmlOptions = XmlFull,

        SiteMapXmlOptions = XmlFull,

        WebResourceDependencyXmlOptions = XmlFull,

        WorkflowXmlOptions = SetIntellisenseContext,

        SavedQueryXmlOptions = XmlSimple,

        SavedQueryVisualizationXmlOptions = SetXmlSchemas,

        FormXmlOptions = XmlFull | SortFormXmlElements,

        RibbonXmlOptions = RibbonFilters | SetIntellisenseContext | SetXmlSchemas | XmlAttributeOnNewLine | SortRibbonCommandsAndRulesById | SortXmlAttributes,

        SolutionComponentXmlOptions = SolutionComponentWithManagedInfo | SolutionComponentWithSolutionInfo | SolutionComponentWithUrl,
    }
}
using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal abstract class AbstractDynamicCommandXsdSchemas : AbstractDynamicCommand<Tuple<string, string[]>>
    {
        public const string SiteMapXmlRoot = "SiteMap";
        public const string SiteMapXmlSchema = "SiteMapXml";

        public const string FetchSchema = "Fetch.xsd";

        public const string FetchRoot = "fetch";
        public const string GridRoot = "grid";
        public const string ColumnSetRoot = "columnset";
        public const string SavedQueryRoot = "savedquery";

        public static readonly XName WorkflowActivityRoot = XNamespace.Get("http://schemas.microsoft.com/netfx/2009/xaml/activities") + "Activity";

        public const string PluginTypeCustomWorkflowActivityInfoSchema = "PluginType CustomWorkflowActivityInfo";
        public const string PluginTypeCustomWorkflowActivityInfoRoot = "SandboxCustomActivityInfo";

        public const string WebResourceDependencyXmlSchema = "WebResource.DependencyXml.xsd";
        public const string WebResourceDependencyXmlRoot = "Dependencies";

        public const string FormXmlRoot = "form";
        public const string FormXmlSchema = "FormXml";
        public const string FormXmlManagedSchema = "FormXmlManaged";

        public const string RibbonSchema = "RibbonXml";

        public const string RibbonDiffXmlRoot = "RibbonDiffXml";
        public const string RibbonXmlRoot = "RibbonDefinitions";

        public const string ModulesContextSchema = "ModulesContext";

        public const string ManifestSchema = "ManifestSchema.xsd";

        public const string VisualizationDataDescriptionSchema = "VisualizationDataDescription.xsd";

        private static TupleList<string, string[]> ListXsdSchemas { get; } = new TupleList<string, string[]>()
        {
            {  FetchSchema, new string[] { "Fetch.xsd" } }

            , {  VisualizationDataDescriptionSchema, new string[] { "VisualizationDataDescription.xsd" } }

            , {  FormXmlSchema, new string[] { "FormXml.xsd", "RibbonCore.xsd", "RibbonTypes.xsd", "RibbonWSS.xsd" } }
            , {  FormXmlManagedSchema, new string[] { "FormXmlManaged.xsd", "RibbonCore.xsd", "RibbonTypes.xsd", "RibbonWSS.xsd" } }

            , {  PluginTypeCustomWorkflowActivityInfoSchema, new string[] { "PluginType.CustomWorkflowActivityInfo.xsd" } }

            , {  WebResourceDependencyXmlSchema, new string[] { "WebResource.DependencyXml.xsd" } }

            , {  ManifestSchema, new string[] { "ManifestSchema.xsd" } }

            , {  ModulesContextSchema, new string[] { "ModulesContext.xsd", "Fetch.xsd" } }

            , {  RibbonSchema, new string[] { "RibbonCore.xsd", "RibbonTypes.xsd", "RibbonWSS.xsd" } }

            , {  SiteMapXmlSchema, new string[] { "SiteMap.xsd", "SiteMapType.xsd" } }

            , {  "CustomizationsSolution", new string[] { "CustomizationsSolution.xsd", "isv.config.xsd", "SiteMapType.xsd", "FormXml.xsd", "RibbonCore.xsd", "RibbonTypes.xsd", "RibbonWSS.xsd", "Fetch.xsd" } }
            , {  "CustomizationsSolutionManaged", new string[] { "CustomizationsSolution_FormManaged.xsd", "isv.config.xsd", "SiteMapTypeManaged.xsd", "FormXmlManaged.xsd", "RibbonCore.xsd", "RibbonTypes.xsd", "RibbonWSS.xsd", "Fetch.xsd" } }

            , {  "ImportMapSchema.xsd", new string[] { "ImportMapSchema.xsd" } }

            , {  "ISV.config.xsd", new string[] { "isv.config.xsd" } }

            , {  "ParameterXml.xsd", new string[] { "ParameterXml.xsd" } }

            , {  "ProcessDefinition.xsd", new string[] { "ProcessDefinition.xsd" } }

            , {  "Reports.config.xsd", new string[] { "reports.config.xsd" } }

            , {  "SimilarityRuleCondition.xsd", new string[] { "SimilarityRuleCondition.xsd" } }

            , {  "TaskBasedFlowXml.xsd", new string[] { "TaskBasedFlowXml.xsd" } }
        };

        protected AbstractDynamicCommandXsdSchemas(OleMenuCommandService commandService, int baseIdStart)
            : base(commandService, PackageGuids.guidDynamicXmlSchemaCommandSet, baseIdStart, ListXsdSchemas.Count)
        {
        }

        protected override IList<Tuple<string, string[]>> GetElementSourceCollection()
        {
            return ListXsdSchemas;
        }

        protected override string GetElementName(Tuple<string, string[]> schemas)
        {
            return schemas.Item1;
        }

        public static string[] GetXsdSchemas(string key)
        {
            return ListXsdSchemas.FirstOrDefault(e => string.Equals(e.Item1, key, StringComparison.InvariantCultureIgnoreCase))?.Item2;
        }

        public static string[] GetXsdSchemasByRootName(string docRootName)
        {
            var schemas = GetSchemaByRootName(docRootName);

            if (!string.IsNullOrEmpty(schemas))
            {
                return GetXsdSchemas(schemas);
            }

            return null;
        }

        public static string GetSchemaByRootName(string docRootName)
        {
            string schemas = string.Empty;

            if (string.Equals(docRootName, SiteMapXmlRoot, StringComparison.InvariantCultureIgnoreCase))
            {
                schemas = SiteMapXmlSchema;
            }
            else if (string.Equals(docRootName, RibbonDiffXmlRoot, StringComparison.InvariantCultureIgnoreCase)
                || string.Equals(docRootName, RibbonXmlRoot, StringComparison.InvariantCultureIgnoreCase)
            )
            {
                schemas = RibbonSchema;
            }
            else if (string.Equals(docRootName, FetchRoot, StringComparison.InvariantCultureIgnoreCase)
                || string.Equals(docRootName, GridRoot, StringComparison.InvariantCultureIgnoreCase)
                || string.Equals(docRootName, ColumnSetRoot, StringComparison.InvariantCultureIgnoreCase)
            )
            {
                schemas = FetchSchema;
            }
            else if (string.Equals(docRootName, FormXmlRoot, StringComparison.InvariantCultureIgnoreCase))
            {
                schemas = FormXmlSchema;
            }
            else if (string.Equals(docRootName, WebResourceDependencyXmlRoot, StringComparison.InvariantCultureIgnoreCase))
            {
                schemas = WebResourceDependencyXmlSchema;
            }
            else if (string.Equals(docRootName, PluginTypeCustomWorkflowActivityInfoRoot, StringComparison.InvariantCultureIgnoreCase))
            {
                schemas = PluginTypeCustomWorkflowActivityInfoSchema;
            }

            return schemas;
        }
    }
}
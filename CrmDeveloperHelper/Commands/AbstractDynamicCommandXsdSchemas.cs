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
        public const string RootSiteMap = "SiteMap";

        public const string RootFetch = "fetch";
        public const string RootGrid = "grid";
        public const string RootColumnSet = "columnset";

        public const string RootSavedQuery = "savedquery";

        public static readonly XName RootActivity = XNamespace.Get("http://schemas.microsoft.com/netfx/2009/xaml/activities") + "Activity";

        public const string RootWebResourceDependencies = "Dependencies";

        public const string RootForm = "form";
        public const string RootRibbonDiffXml = "RibbonDiffXml";
        public const string RootRibbonDefinitions = "RibbonDefinitions";

        public const string SchemaFormXml = "FormXml";
        public const string SchemaFormXmlManaged = "FormXmlManaged";

        public const string SchemaModulesContext = "ModulesContext";

        public const string SchemaManifest = "ManifestSchema.xsd";

        public const string SchemaDependencyXml = "DependencyXml.xsd";

        public const string SchemaRibbonXml = "RibbonXml";
        public const string SchemaFetch = "Fetch.xsd";
        public const string SchemaSiteMapXml = "SiteMapXml";
        public const string SchemaVisualizationDataDescription = "VisualizationDataDescription.xsd";

        private static TupleList<string, string[]> ListXsdSchemas { get; } = new TupleList<string, string[]>()
        {
            {  "CustomizationsSolution", new string[] { "CustomizationsSolution.xsd", "isv.config.xsd", "SiteMapType.xsd", "FormXml.xsd", "RibbonCore.xsd", "RibbonTypes.xsd", "RibbonWSS.xsd", "Fetch.xsd" } }
            , {  "CustomizationsSolutionManaged", new string[] { "CustomizationsSolution_FormManaged.xsd", "isv.config.xsd", "SiteMapTypeManaged.xsd", "FormXmlManaged.xsd", "RibbonCore.xsd", "RibbonTypes.xsd", "RibbonWSS.xsd", "Fetch.xsd" } }

            , {  SchemaDependencyXml, new string[] { "DependencyXml.xsd" } }

            , {  SchemaFetch, new string[] { "Fetch.xsd" } }

            , {  SchemaFormXml, new string[] { "FormXml.xsd", "RibbonCore.xsd", "RibbonTypes.xsd", "RibbonWSS.xsd" } }
            , {  SchemaFormXmlManaged, new string[] { "FormXmlManaged.xsd", "RibbonCore.xsd", "RibbonTypes.xsd", "RibbonWSS.xsd" } }

            , {  "ImportMapSchema.xsd", new string[] { "ImportMapSchema.xsd" } }

            , {  "isv.config.xsd", new string[] { "isv.config.xsd" } }

            , {  SchemaManifest, new string[] { "ManifestSchema.xsd" } }

            , {  SchemaModulesContext, new string[] { "ModulesContext.xsd", "Fetch.xsd" } }

            , {  "ParameterXml.xsd", new string[] { "ParameterXml.xsd" } }

            , {  "ProcessDefinition.xsd", new string[] { "ProcessDefinition.xsd" } }

            , {  "reports.config.xsd", new string[] { "reports.config.xsd" } }

            , {  SchemaRibbonXml, new string[] { "RibbonCore.xsd", "RibbonTypes.xsd", "RibbonWSS.xsd" } }

            , {  "SimilarityRuleCondition.xsd", new string[] { "SimilarityRuleCondition.xsd" } }

            , {  SchemaSiteMapXml, new string[] { "SiteMap.xsd", "SiteMapType.xsd" } }

            , {  "TaskBasedFlowXml.xsd", new string[] { "TaskBasedFlowXml.xsd" } }

            , {  SchemaVisualizationDataDescription, new string[] { "VisualizationDataDescription.xsd" } }
        };

        public AbstractDynamicCommandXsdSchemas(OleMenuCommandService commandService, int baseIdStart)
            : base(commandService, baseIdStart, ListXsdSchemas.Count)
        {
        }

        protected override ICollection<Tuple<string, string[]>> GetElementSourceCollection()
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

            if (string.Equals(docRootName, RootSiteMap, StringComparison.InvariantCultureIgnoreCase))
            {
                schemas = SchemaSiteMapXml;
            }
            else if (string.Equals(docRootName, RootRibbonDiffXml, StringComparison.InvariantCultureIgnoreCase)
                || string.Equals(docRootName, RootRibbonDefinitions, StringComparison.InvariantCultureIgnoreCase)
            )
            {
                schemas = SchemaRibbonXml;
            }
            else if (string.Equals(docRootName, RootFetch, StringComparison.InvariantCultureIgnoreCase)
                || string.Equals(docRootName, RootGrid, StringComparison.InvariantCultureIgnoreCase)
                || string.Equals(docRootName, RootColumnSet, StringComparison.InvariantCultureIgnoreCase)
            )
            {
                schemas = SchemaFetch;
            }
            else if (string.Equals(docRootName, RootForm, StringComparison.InvariantCultureIgnoreCase))
            {
                schemas = SchemaFormXml;
            }
            else if (string.Equals(docRootName, RootWebResourceDependencies, StringComparison.InvariantCultureIgnoreCase))
            {
                schemas = SchemaDependencyXml;
            }

            return schemas;
        }
    }
}
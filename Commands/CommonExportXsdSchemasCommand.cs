using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System;
using System.Linq;
using System.ComponentModel.Design;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class CommonExportXsdSchemasCommand : IServiceProviderOwner
    {
        private readonly Package _package;

        public IServiceProvider ServiceProvider => _package;

        private const int _baseIdStart = PackageIds.CommonExportXsdSchemasCommandId;

        public const string SchemaFormXml = "FormXml";
        public const string SchemaRibbonXml = "RibbonXml";
        public const string SchemaFetch = "Fetch.xsd";
        public const string SchemaSiteMapXml = "SiteMapXml";
        public const string SchemaVisualizationDataDescription = "VisualizationDataDescription";

        internal static TupleList<string, string[]> ListXsdSchemas { get; private set; } = new TupleList<string, string[]>()
        {
            {  "CustomizationsSolution", new string[] { "CustomizationsSolution.xsd", "isv.config.xsd", "SiteMapType.xsd", "FormXml.xsd", "RibbonCore.xsd", "RibbonTypes.xsd", "RibbonWSS.xsd", "Fetch.xsd" } }

            , {  "isv.config.xsd", new string[] { "isv.config.xsd" } }
            , {  "ParameterXml.xsd", new string[] { "ParameterXml.xsd" } }

            , {  SchemaFormXml, new string[] { "FormXml.xsd", "RibbonCore.xsd", "RibbonTypes.xsd", "RibbonWSS.xsd" } }
            , {  SchemaFetch, new string[] { "Fetch.xsd" } }
            , {  SchemaVisualizationDataDescription, new string[] { "VisualizationDataDescription.xsd" } }

            , {  SchemaRibbonXml, new string[] { "RibbonCore.xsd", "RibbonTypes.xsd", "RibbonWSS.xsd" } }

            , {  SchemaSiteMapXml, new string[] { "SiteMap.xsd", "SiteMapType.xsd" } }

            , {  "reports.config.xsd", new string[] { "reports.config.xsd" } }
        };

        public static string[] GetXsdSchemas(string key)
        {
            return ListXsdSchemas.FirstOrDefault(e => string.Equals(e.Item1, key, StringComparison.InvariantCultureIgnoreCase))?.Item2;
        }

        private CommonExportXsdSchemasCommand(Package package)
        {
            _package = package ?? throw new ArgumentNullException(nameof(package));

            OleMenuCommandService commandService = ServiceProvider.GetService(typeof(IMenuCommandService)) as OleMenuCommandService;

            if (commandService != null)
            {
                for (int i = 0; i < ListXsdSchemas.Count; i++)
                {
                    CommandID menuCommandID = new CommandID(PackageGuids.guidDynamicCommandSet, _baseIdStart + i);

                    OleMenuCommand menuCommand = new OleMenuCommand(menuItemCallback, menuCommandID);

                    menuCommand.Enabled = menuCommand.Visible = true;

                    menuCommand.Text = ListXsdSchemas[i].Item1;

                    commandService.AddCommand(menuCommand);
                }
            }
        }

        public static CommonExportXsdSchemasCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new CommonExportXsdSchemasCommand(package);
        }

        private void menuItemCallback(object sender, EventArgs e)
        {
            try
            {
                OleMenuCommand menuCommand = sender as OleMenuCommand;
                if (menuCommand == null)
                {
                    return;
                }

                EnvDTE80.DTE2 applicationObject = ServiceProvider.GetService(typeof(EnvDTE.DTE)) as EnvDTE80.DTE2;
                if (applicationObject == null)
                {
                    return;
                }

                int index = menuCommand.CommandID.ID - _baseIdStart;

                if (0 <= index && index < ListXsdSchemas.Count)
                {
                    var selectedSchemas = ListXsdSchemas[index].Item2;

                    DTEHelper helper = DTEHelper.Create(applicationObject);

                    helper.HandleExportXsdSchema(selectedSchemas);
                }
            }
            catch (Exception ex)
            {
                DTEHelper.WriteExceptionToOutput(ex);
            }
        }
    }
}

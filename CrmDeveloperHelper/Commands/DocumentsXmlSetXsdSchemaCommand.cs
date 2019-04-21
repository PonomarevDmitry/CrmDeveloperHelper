using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class DocumentsXmlSetXsdSchemaCommand : IServiceProviderOwner
    {
        private readonly Package _package;

        public IServiceProvider ServiceProvider => _package;

        private const int _baseIdStart = PackageIds.DocumentsXmlSetXsdSchemaCommandId;

        private DocumentsXmlSetXsdSchemaCommand(Package package)
        {
            _package = package ?? throw new ArgumentNullException(nameof(package));

            OleMenuCommandService commandService = ServiceProvider.GetService(typeof(IMenuCommandService)) as OleMenuCommandService;

            if (commandService != null)
            {
                for (int i = 0; i < CommonExportXsdSchemasCommand.ListXsdSchemas.Count; i++)
                {
                    CommandID menuCommandID = new CommandID(PackageGuids.guidDynamicCommandSet, _baseIdStart + i);

                    OleMenuCommand menuCommand = new OleMenuCommand(menuItemCallback, menuCommandID);

                    menuCommand.Enabled = menuCommand.Visible = true;

                    menuCommand.Text = CommonExportXsdSchemasCommand.ListXsdSchemas[i].Item1;

                    menuCommand.BeforeQueryStatus += menuItem_BeforeQueryStatus;

                    commandService.AddCommand(menuCommand);
                }
            }
        }

        public static DocumentsXmlSetXsdSchemaCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new DocumentsXmlSetXsdSchemaCommand(package);
        }

        private void menuItem_BeforeQueryStatus(object sender, EventArgs e)
        {
            try
            {
                if (sender is OleMenuCommand menuCommand)
                {
                    menuCommand.Enabled = menuCommand.Visible = false;

                    var index = menuCommand.CommandID.ID - _baseIdStart;

                    if (0 <= index && index < CommonExportXsdSchemasCommand.ListXsdSchemas.Count)
                    {
                        menuCommand.Enabled = menuCommand.Visible = true;

                        CommonHandlers.ActionBeforeQueryStatusOpenedDocumentsXml(this, menuCommand);
                    }
                }
            }
            catch (Exception ex)
            {
                DTEHelper.WriteExceptionToOutput(null, ex);
            }
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

                if (0 <= index && index < CommonExportXsdSchemasCommand.ListXsdSchemas.Count)
                {
                    var selectedSchemas = CommonExportXsdSchemasCommand.ListXsdSchemas[index].Item2;

                    DTEHelper helper = DTEHelper.Create(applicationObject);

                    List<EnvDTE.Document> listDocuments = helper.GetOpenedDocumentsAsDocument(FileOperations.SupportsXmlType);

                    if (listDocuments.Any())
                    {
                        foreach (var document in listDocuments)
                        {
                            ContentCoparerHelper.ReplaceXsdSchemaInDocument(document, selectedSchemas);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                DTEHelper.WriteExceptionToOutput(null, ex);
            }
        }
    }
}

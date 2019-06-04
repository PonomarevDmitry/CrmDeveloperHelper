using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using System;
using System.ComponentModel.Design;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Xmls
{
    internal sealed class FolderXmlSetXsdSchemaCommand : IServiceProviderOwner
    {
        private readonly Package _package;

        public IServiceProvider ServiceProvider => _package;

        private const int _baseIdStart = PackageIds.FolderXmlSetXsdSchemaCommandId;

        private FolderXmlSetXsdSchemaCommand(Package package)
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

        public static FolderXmlSetXsdSchemaCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new FolderXmlSetXsdSchemaCommand(package);
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

                        CommonHandlers.ActionBeforeQueryStatusSolutionExplorerXmlRecursive(this, menuCommand);
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

                    var listFiles = helper.GetSelectedFilesInSolutionExplorer(FileOperations.SupportsXmlType, true).ToList();

                    if (listFiles.Any())
                    {
                        foreach (var document in listFiles.Where(s => s.Document != null && s.Document.ActiveWindow != null && s.Document.ActiveWindow.Visible).Select(s => s.Document))
                        {
                            ContentCoparerHelper.ReplaceXsdSchemaInDocument(document, selectedSchemas);
                        }

                        foreach (var filePath in listFiles.Where(s => !(s.Document != null && s.Document.ActiveWindow != null && s.Document.ActiveWindow.Visible)).Select(s => s.FilePath))
                        {
                            ContentCoparerHelper.ReplaceXsdSchemaInFile(filePath, selectedSchemas);
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

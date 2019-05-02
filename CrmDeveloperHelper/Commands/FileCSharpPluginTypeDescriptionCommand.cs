using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using System;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class FileCSharpPluginTypeDescriptionCommand : AbstractCommand
    {
        private FileCSharpPluginTypeDescriptionCommand(Package package)
            : base(package, PackageGuids.guidCommandSet, PackageIds.FileCSharpPluginTypeDescriptionCommandId, ActionExecute, ActionBeforeQueryStatus) { }

        public static FileCSharpPluginTypeDescriptionCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new FileCSharpPluginTypeDescriptionCommand(package);
        }

        private static void ActionBeforeQueryStatus(IServiceProviderOwner command, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusSolutionExplorerCSharpSingle(command, menuCommand);

            CommonHandlers.ActionBeforeQueryStatusSolutionExplorerSingleItemContainsProject(command, menuCommand);
        }

        private static async void ActionExecute(DTEHelper helper)
        {
            try
            {
                var projectItem = helper.GetSingleSelectedProjectItemInSolutionExplorer(FileOperations.SupportsCSharpType);

                if (projectItem != null)
                {
                    helper.WriteToOutput(null, Properties.OutputStrings.GettingClassFullNameFromFileFormat1, projectItem.FileNames[1]);
                    helper.ActivateOutputWindow(null);
                    string fileType = await PropertiesHelper.GetTypeFullNameAsync(projectItem);

                    helper.HandleExportPluginTypeDescription(fileType);
                }
            }
            catch (Exception ex)
            {
                DTEHelper.WriteExceptionToOutput(null, ex);
            }
        }
    }
}
using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using System;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.CSharp
{
    internal sealed class FileCSharpPluginTypeExplorerCommand : AbstractCommand
    {
        private FileCSharpPluginTypeExplorerCommand(Package package)
            : base(package, PackageGuids.guidCommandSet, PackageIds.FileCSharpPluginTypeExplorerCommandId, ActionExecute, ActionBeforeQueryStatus) { }

        public static FileCSharpPluginTypeExplorerCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new FileCSharpPluginTypeExplorerCommand(package);
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

                    helper.HandleOpenPluginTypeExplorer(fileType);
                }
            }
            catch (Exception ex)
            {
                DTEHelper.WriteExceptionToOutput(null, ex);
            }
        }
    }
}
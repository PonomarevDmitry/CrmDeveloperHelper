using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using System;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class FileCSharpPluginTreeCommand : AbstractCommand
    {
        private FileCSharpPluginTreeCommand(Package package)
            : base(package, PackageGuids.guidCommandSet, PackageIds.FileCSharpPluginTreeCommandId, ActionExecute, ActionBeforeQueryStatus) { }

        public static FileCSharpPluginTreeCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new FileCSharpPluginTreeCommand(package);
        }

        private static void ActionBeforeQueryStatus(IServiceProviderOwner command, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusSolutionExplorerCSharpSingle(command, menuCommand);

            CommonHandlers.ActionBeforeQueryStatusSolutionExplorerSingleItemContainsProject(command, menuCommand, FileOperations.SupportsCSharpType);
        }

        private async static void ActionExecute(DTEHelper helper)
        {
            try
            {
                EnvDTE.SelectedItem item = helper.GetSingleSelectedItemInSolutionExplorer(FileOperations.SupportsCSharpType);

                if (item != null)
                {
                    string fileType = await PropertiesHelper.GetTypeFullNameAsync(item);

                    helper.HandleOpenPluginTree(string.Empty, fileType, string.Empty);
                }
            }
            catch (Exception ex)
            {
                DTEHelper.WriteExceptionToOutput(ex);
            }
        }
    }
}

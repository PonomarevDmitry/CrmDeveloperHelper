using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using System.Linq;

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

            CommonHandlers.ActionBeforeQueryStatusSolutionExplorerSingleItemContainsProject(command, menuCommand, FileOperations.SupportsCSharpType);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            EnvDTE.SelectedItem item = helper.GetSingleSelectedItemInSolutionExplorer(FileOperations.SupportsCSharpType);

            if (item != null)
            {
                string selection = string.Empty;

                if (item.ProjectItem != null && item.ProjectItem.FileCount > 0)
                {
                    selection = item.ProjectItem.Name.Split('.').FirstOrDefault();
                }
                else if (!string.IsNullOrEmpty(item.Name))
                {
                    selection = item.Name;
                }

                helper.HandleExportPluginTypeDescription(selection);
            }
        }
    }
}
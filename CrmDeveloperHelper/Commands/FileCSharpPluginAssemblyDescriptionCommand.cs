using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class FileCSharpPluginAssemblyDescriptionCommand : AbstractCommand
    {
        private FileCSharpPluginAssemblyDescriptionCommand(Package package)
            : base(package, PackageGuids.guidCommandSet, PackageIds.FileCSharpPluginAssemblyDescriptionCommandId, ActionExecute, ActionBeforeQueryStatus) { }

        public static FileCSharpPluginAssemblyDescriptionCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new FileCSharpPluginAssemblyDescriptionCommand(package);
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

                if (item.ProjectItem != null && item.ProjectItem.ContainingProject != null)
                {
                    selection = item.ProjectItem.ContainingProject.Name;
                }
                else if (!string.IsNullOrEmpty(item.Name))
                {
                    selection = item.Name;
                }

                helper.HandleExportPluginAssembly(selection);
            }
        }
    }
}
using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class FileCSharpPluginAssemblyExplorerCommand : AbstractCommand
    {
        private FileCSharpPluginAssemblyExplorerCommand(Package package)
            : base(package, PackageGuids.guidCommandSet, PackageIds.FileCSharpPluginAssemblyExplorerCommandId, ActionExecute, ActionBeforeQueryStatus) { }

        public static FileCSharpPluginAssemblyExplorerCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new FileCSharpPluginAssemblyExplorerCommand(package);
        }

        private static void ActionBeforeQueryStatus(IServiceProviderOwner command, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusSolutionExplorerCSharpSingle(command, menuCommand);

            CommonHandlers.ActionBeforeQueryStatusSolutionExplorerSingleItemContainsProject(command, menuCommand);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            var projectItem = helper.GetSingleSelectedProjectItemInSolutionExplorer(FileOperations.SupportsCSharpType);

            if (projectItem != null)
            {
                string selection = string.Empty;

                if (projectItem != null && projectItem.ContainingProject != null)
                {
                    selection = projectItem.ContainingProject.Name;
                }
                else if (!string.IsNullOrEmpty(projectItem.Name))
                {
                    selection = projectItem.Name;
                }

                helper.HandleOpenPluginAssemblyExplorer(selection);
            }
        }
    }
}
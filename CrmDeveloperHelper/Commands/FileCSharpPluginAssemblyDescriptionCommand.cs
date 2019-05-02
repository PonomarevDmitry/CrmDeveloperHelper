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

                helper.HandleExportPluginAssembly(selection);
            }
        }
    }
}
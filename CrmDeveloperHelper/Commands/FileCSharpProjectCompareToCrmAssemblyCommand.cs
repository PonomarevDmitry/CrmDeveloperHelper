using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class FileCSharpProjectCompareToCrmAssemblyCommand : AbstractCommand
    {
        private FileCSharpProjectCompareToCrmAssemblyCommand(Package package)
            : base(package, PackageGuids.guidCommandSet, PackageIds.FileCSharpProjectCompareToCrmAssemblyCommandId, ActionExecute, ActionBeforeQueryStatus) { }

        public static FileCSharpProjectCompareToCrmAssemblyCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new FileCSharpProjectCompareToCrmAssemblyCommand(package);
        }

        private static void ActionBeforeQueryStatus(IServiceProviderOwner command, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusSolutionExplorerCSharpSingle(command, menuCommand);

            CommonHandlers.ActionBeforeQueryStatusSolutionExplorerSingleItemContainsProject(command, menuCommand, FileOperations.SupportsCSharpType);

            CommonHandlers.CorrectCommandNameForConnectionName(command, menuCommand, Properties.CommandNames.FileCSharpProjectCompareToCrmAssemblyCommand);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            EnvDTE.SelectedItem item = helper.GetSingleSelectedItemInSolutionExplorer(FileOperations.SupportsCSharpType);

            if (item != null)
            {
                if (item.ProjectItem != null && item.ProjectItem.ContainingProject != null)
                {
                    helper.HandleComparingPluginAssemblyAndLocalAssemblyCommand(null, item.ProjectItem.ContainingProject);
                }
            }
        }
    }
}
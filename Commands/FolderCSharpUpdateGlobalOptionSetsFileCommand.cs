using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System.Collections.Generic;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class FolderCSharpUpdateGlobalOptionSetsFileCommand : AbstractCommand
    {
        private FolderCSharpUpdateGlobalOptionSetsFileCommand(Package package)
            : base(package, PackageGuids.guidCommandSet, PackageIds.FolderCSharpUpdateGlobalOptionSetsFileCommandId, ActionExecute, ActionBeforeQueryStatus) { }

        public static FolderCSharpUpdateGlobalOptionSetsFileCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new FolderCSharpUpdateGlobalOptionSetsFileCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            List<SelectedFile> selectedFiles = helper.GetSelectedFilesInSolutionExplorer(FileOperations.SupportsCSharpType, true);

            helper.HandleUpdateGlobalOptionSetsFile(selectedFiles, false);
        }

        private static void ActionBeforeQueryStatus(IServiceProviderOwner command, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusSolutionExplorerCSharpRecursive(command, menuCommand);

            CommonHandlers.CorrectCommandNameForConnectionName(command, menuCommand, Properties.CommandNames.FolderCSharpUpdateGlobalOptionSetsFileCommand);
        }
    }
}
using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System.Collections.Generic;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class FileCSharpUpdateEntityMetadataFileCommand : AbstractCommand
    {
        private FileCSharpUpdateEntityMetadataFileCommand(Package package)
            : base(package, PackageGuids.guidCommandSet, PackageIds.FileCSharpUpdateEntityMetadataFileCommandId, ActionExecute, ActionBeforeQueryStatus) { }

        public static FileCSharpUpdateEntityMetadataFileCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new FileCSharpUpdateEntityMetadataFileCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            List<SelectedFile> selectedFiles = helper.GetSelectedFilesInSolutionExplorer(FileOperations.SupportsCSharpType, false);

            helper.HandleUpdateEntityMetadataFile(selectedFiles, false);
        }

        private static void ActionBeforeQueryStatus(IServiceProviderOwner command, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusSolutionExplorerCSharpAny(command, menuCommand);

            CommonHandlers.CorrectCommandNameForConnectionName(command, menuCommand, Properties.CommandNames.FileCSharpUpdateEntityMetadataFileCommand);
        }
    }
}
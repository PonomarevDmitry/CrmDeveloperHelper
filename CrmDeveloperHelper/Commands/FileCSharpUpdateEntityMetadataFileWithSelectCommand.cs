using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System.Collections.Generic;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class FileCSharpUpdateEntityMetadataFileWithSelectCommand : AbstractCommand
    {
        private FileCSharpUpdateEntityMetadataFileWithSelectCommand(Package package)
            : base(package, PackageGuids.guidCommandSet, PackageIds.FileCSharpUpdateEntityMetadataFileWithSelectCommandId, ActionExecute, ActionBeforeQueryStatus) { }

        public static FileCSharpUpdateEntityMetadataFileWithSelectCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new FileCSharpUpdateEntityMetadataFileWithSelectCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            List<SelectedFile> selectedFiles = helper.GetSelectedFilesInSolutionExplorer(FileOperations.SupportsCSharpType, false);

            helper.HandleUpdateEntityMetadataFileCSharp(null, selectedFiles, true);
        }

        private static void ActionBeforeQueryStatus(IServiceProviderOwner command, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusSolutionExplorerCSharpSingle(command, menuCommand);

            CommonHandlers.CorrectCommandNameForConnectionName(command, menuCommand, Properties.CommandNames.FileCSharpUpdateEntityMetadataFileWithSelectCommand);
        }
    }
}

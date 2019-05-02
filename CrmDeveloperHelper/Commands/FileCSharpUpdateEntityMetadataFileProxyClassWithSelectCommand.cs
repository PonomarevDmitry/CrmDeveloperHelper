using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System.Collections.Generic;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class FileCSharpUpdateEntityMetadataFileProxyClassWithSelectCommand : AbstractCommand
    {
        private FileCSharpUpdateEntityMetadataFileProxyClassWithSelectCommand(Package package)
            : base(package, PackageGuids.guidCommandSet, PackageIds.FileCSharpUpdateEntityMetadataFileProxyClassWithSelectCommandId, ActionExecute, ActionBeforeQueryStatus) { }

        public static FileCSharpUpdateEntityMetadataFileProxyClassWithSelectCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new FileCSharpUpdateEntityMetadataFileProxyClassWithSelectCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            List<SelectedFile> selectedFiles = helper.GetSelectedFilesInSolutionExplorer(FileOperations.SupportsCSharpType, false).ToList();

            helper.HandleUpdateEntityMetadataFileCSharpProxyClass(null, selectedFiles, true);
        }

        private static void ActionBeforeQueryStatus(IServiceProviderOwner command, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusSolutionExplorerCSharpSingle(command, menuCommand);

            CommonHandlers.CorrectCommandNameForConnectionName(command, menuCommand, Properties.CommandNames.FileCSharpUpdateEntityMetadataFileProxyClassWithSelectCommand);
        }
    }
}

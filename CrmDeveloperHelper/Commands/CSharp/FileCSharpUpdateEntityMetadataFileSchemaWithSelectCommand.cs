using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System.Collections.Generic;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.CSharp
{
    internal sealed class FileCSharpUpdateEntityMetadataFileSchemaWithSelectCommand : AbstractCommand
    {
        private FileCSharpUpdateEntityMetadataFileSchemaWithSelectCommand(Package package)
            : base(package, PackageGuids.guidCommandSet, PackageIds.FileCSharpUpdateEntityMetadataFileSchemaWithSelectCommandId, ActionExecute, ActionBeforeQueryStatus) { }

        public static FileCSharpUpdateEntityMetadataFileSchemaWithSelectCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new FileCSharpUpdateEntityMetadataFileSchemaWithSelectCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            List<SelectedFile> selectedFiles = helper.GetSelectedFilesInSolutionExplorer(FileOperations.SupportsCSharpType, false).ToList();

            helper.HandleUpdateEntityMetadataFileCSharpSchema(null, selectedFiles, true);
        }

        private static void ActionBeforeQueryStatus(IServiceProviderOwner command, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusSolutionExplorerCSharpSingle(command, menuCommand);

            CommonHandlers.CorrectCommandNameForConnectionName(command, menuCommand, Properties.CommandNames.FileCSharpUpdateEntityMetadataFileSchemaWithSelectCommand);
        }
    }
}

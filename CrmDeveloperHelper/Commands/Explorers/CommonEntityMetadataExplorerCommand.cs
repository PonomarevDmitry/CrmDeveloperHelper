using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Explorers
{
    internal sealed class CommonEntityMetadataExplorerCommand : AbstractCommand
    {
        private CommonEntityMetadataExplorerCommand(Package package)
           : base(package, PackageGuids.guidCommandSet, PackageIds.CommonExportEntityMetadataCommandId, ActionExecute, ActionBeforeQueryStatus) { }

        public static CommonEntityMetadataExplorerCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new CommonEntityMetadataExplorerCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            helper.HandleExportFileWithEntityMetadata();
        }

        private static void ActionBeforeQueryStatus(IServiceProviderOwner command, OleMenuCommand menuCommand)
        {
            CommonHandlers.CorrectCommandNameForConnectionName(command, menuCommand, Properties.CommandNames.CommonExportEntityMetadataCommand);
        }
    }
}
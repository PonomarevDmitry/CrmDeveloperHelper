using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.OutputWindows.Explorers
{
    internal sealed class OutputEntityMetadataExplorerCommand : AbstractOutputWindowCommand
    {
        private OutputEntityMetadataExplorerCommand(OleMenuCommandService commandService)
           : base(commandService, PackageIds.OutputEntityMetadataExplorerCommandId) { }

        public static OutputEntityMetadataExplorerCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new OutputEntityMetadataExplorerCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            helper.HandleExportFileWithEntityMetadata(connectionData);
        }
    }
}
using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.OutputWindows.Explorers
{
    internal sealed class OutputGlobalOptionSetsExplorerCommand : AbstractOutputWindowCommand
    {
        private OutputGlobalOptionSetsExplorerCommand(OleMenuCommandService commandService)
           : base(commandService, PackageIds.OutputGlobalOptionSetsExplorerCommandId) { }

        public static OutputGlobalOptionSetsExplorerCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new OutputGlobalOptionSetsExplorerCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            helper.HandleExportGlobalOptionSets(connectionData);
        }
    }
}

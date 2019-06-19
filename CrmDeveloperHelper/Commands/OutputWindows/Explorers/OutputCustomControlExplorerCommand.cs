using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.OutputWindows.Explorers
{
    internal sealed class OutputCustomControlExplorerCommand : AbstractOutputWindowCommand
    {
        private OutputCustomControlExplorerCommand(OleMenuCommandService commandService)
           : base(commandService, PackageIds.OutputCustomControlExplorerCommandId) { }

        public static OutputCustomControlExplorerCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new OutputCustomControlExplorerCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            helper.HandleExportCustomControl(connectionData);
        }
    }
}

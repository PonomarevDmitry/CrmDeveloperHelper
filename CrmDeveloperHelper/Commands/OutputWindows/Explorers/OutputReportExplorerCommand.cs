using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.OutputWindows.Explorers
{
    internal sealed class OutputReportExplorerCommand : AbstractOutputWindowCommand
    {
        private OutputReportExplorerCommand(OleMenuCommandService commandService)
           : base(commandService, PackageIds.guidCommandSet.OutputReportExplorerCommandId) { }

        public static OutputReportExplorerCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new OutputReportExplorerCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            helper.HandleExportReport(connectionData);
        }
    }
}

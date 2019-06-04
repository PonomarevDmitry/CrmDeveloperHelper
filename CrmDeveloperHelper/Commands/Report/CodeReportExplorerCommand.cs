using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class CodeReportExplorerCommand : AbstractCommand
    {
        private CodeReportExplorerCommand(Package package)
            : base(package, PackageGuids.guidCommandSet, PackageIds.CodeReportExplorerCommandId, ActionExecute, ActionBeforeQueryStatus) { }

        public static CodeReportExplorerCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new CodeReportExplorerCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            helper.HandleOpenReportExplorerCommand();
        }

        private static void ActionBeforeQueryStatus(IServiceProviderOwner command, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusActiveDocumentReport(command, menuCommand);

            if (menuCommand.Enabled)
            {
                CommonHandlers.CorrectCommandNameForConnectionName(command, menuCommand, Properties.CommandNames.CodeReportExplorerCommand);
            }
        }
    }
}
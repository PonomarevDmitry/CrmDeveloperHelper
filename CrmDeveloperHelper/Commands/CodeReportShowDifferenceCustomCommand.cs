using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class CodeReportShowDifferenceCustomCommand : AbstractCommand
    {
        private CodeReportShowDifferenceCustomCommand(Package package)
            : base(package, PackageGuids.guidCommandSet, PackageIds.CodeReportShowDifferenceCustomCommandId, ActionExecute, ActionBeforeQueryStatus) { }

        public static CodeReportShowDifferenceCustomCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new CodeReportShowDifferenceCustomCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            helper.HandleReportDifferenceCommand(null, true);
        }

        private static void ActionBeforeQueryStatus(IServiceProviderOwner command, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusActiveDocumentReport(command, menuCommand);

            CommonHandlers.CorrectCommandNameForConnectionName(command, menuCommand, "Show Difference with Select");
        }
    }
}

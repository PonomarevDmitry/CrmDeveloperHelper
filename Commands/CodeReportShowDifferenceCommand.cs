using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class CodeReportShowDifferenceCommand : AbstractCommand
    {
        private CodeReportShowDifferenceCommand(Package package)
            : base(package, PackageGuids.guidCommandSet, PackageIds.CodeReportShowDifferenceCommandId, ActionExecute, ActionBeforeQueryStatus) { }

        public static CodeReportShowDifferenceCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new CodeReportShowDifferenceCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            helper.HandleReportDifferenceCommand(null, false);
        }

        private static void ActionBeforeQueryStatus(IServiceProviderOwner command, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusActiveDocumentReport(command, menuCommand);

            CommonHandlers.CorrectCommandNameForConnectionName(command, menuCommand, "Show Difference");
        }
    }
}

using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class ListForPublishCompareCommand : AbstractCommand
    {
        private ListForPublishCompareCommand(Package package)
          : base(package, PackageGuids.guidCommandSet, PackageIds.ListForPublishCompareCommandId, ActionExecute, ActionBeforeQueryStatus) { }

        public static ListForPublishCompareCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new ListForPublishCompareCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            helper.HandleFileCompareListForPublishCommand(null, false);
        }

        private static void ActionBeforeQueryStatus(IServiceProviderOwner command, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusConnectionIsNotReadOnly(command, menuCommand);

            CommonHandlers.ActionBeforeQueryStatusListForPublishWebResourceAny(command, menuCommand);

            CommonHandlers.CorrectCommandNameForConnectionName(command, menuCommand, Properties.CommandNames.ListForPublishCompareCommand);
        }
    }
}
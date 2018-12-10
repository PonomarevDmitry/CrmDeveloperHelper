using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class ListForPublishCompareWithDetailsCommand : AbstractCommand
    {
        private ListForPublishCompareWithDetailsCommand(Package package)
          : base(package, PackageGuids.guidCommandSet, PackageIds.ListForPublishCompareWithDetailsCommandId, ActionExecute, ActionBeforeQueryStatus) { }

        public static ListForPublishCompareWithDetailsCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new ListForPublishCompareWithDetailsCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            helper.HandleFileCompareListForPublishCommand(null, true);
        }

        private static void ActionBeforeQueryStatus(IServiceProviderOwner command, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusConnectionIsNotReadOnly(command, menuCommand);

            CommonHandlers.ActionBeforeQueryStatusListForPublishWebResourceAny(command, menuCommand);

            CommonHandlers.CorrectCommandNameForConnectionName(command, menuCommand, Properties.CommandNames.ListForPublishCompareWithDetailsCommand);
        }
    }
}
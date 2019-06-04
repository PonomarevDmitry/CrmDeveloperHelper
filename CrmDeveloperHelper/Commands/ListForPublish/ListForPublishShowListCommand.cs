using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.ListForPublish
{
    internal sealed class ListForPublishShowListCommand : AbstractCommand
    {
        private ListForPublishShowListCommand(Package package)
           : base(package, PackageGuids.guidCommandSet, PackageIds.ListForPublishShowListCommandId, ActionExecute, ActionBeforeQueryStatus) { }

        public static ListForPublishShowListCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new ListForPublishShowListCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            helper.ShowListForPublish(null);
        }

        private static void ActionBeforeQueryStatus(IServiceProviderOwner command, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusConnectionIsNotReadOnly(command, menuCommand);

            CommonHandlers.ActionBeforeQueryStatusListForPublishWebResourceAny(command, menuCommand);

            CommonHandlers.CorrectCommandNameForConnectionName(command, menuCommand, Properties.CommandNames.ListForPublishShowListCommand);
        }
    }
}
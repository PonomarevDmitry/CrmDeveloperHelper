using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class CommonCrmConnectionTestCommand : AbstractCommand
    {
        private CommonCrmConnectionTestCommand(Package package)
            : base(package, PackageGuids.guidCommandSet, PackageIds.CommonCrmConnectionTestCommandId, ActionExecute, ActionBeforeQueryStatus) { }

        public static CommonCrmConnectionTestCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new CommonCrmConnectionTestCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            helper.TestCurrentConnection();
        }

        private static void ActionBeforeQueryStatus(IServiceProviderOwner command, OleMenuCommand menuCommand)
        {
            CommonHandlers.CorrectCommandNameForConnectionName(command, menuCommand, Properties.CommandNames.CommonCrmConnectionTestCommand);
        }
    }
}
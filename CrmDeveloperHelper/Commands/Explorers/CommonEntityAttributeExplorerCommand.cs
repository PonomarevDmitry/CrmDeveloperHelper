using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class CommonEntityAttributeExplorerCommand : AbstractCommand
    {
        private CommonEntityAttributeExplorerCommand(Package package)
           : base(package, PackageGuids.guidCommandSet, PackageIds.CommonEntityAttributeExplorerCommandId, ActionExecute, ActionBeforeQueryStatus) { }

        public static CommonEntityAttributeExplorerCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new CommonEntityAttributeExplorerCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            helper.HandleOpenEntityAttributeExplorer();
        }

        private static void ActionBeforeQueryStatus(IServiceProviderOwner command, OleMenuCommand menuCommand)
        {
            CommonHandlers.CorrectCommandNameForConnectionName(command, menuCommand, Properties.CommandNames.CommonEntityAttributeExplorerCommand);
        }
    }
}
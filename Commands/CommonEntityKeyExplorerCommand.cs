using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class CommonEntityKeyExplorerCommand : AbstractCommand
    {
        private CommonEntityKeyExplorerCommand(Package package)
           : base(package, PackageGuids.guidCommandSet, PackageIds.CommonEntityKeyExplorerCommandId, ActionExecute, ActionBeforeQueryStatus) { }

        public static CommonEntityKeyExplorerCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new CommonEntityKeyExplorerCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            helper.HandleOpenEntityKeyExplorer();
        }

        private static void ActionBeforeQueryStatus(IServiceProviderOwner command, OleMenuCommand menuCommand)
        {
            
            CommonHandlers.CorrectCommandNameForConnectionName(command, menuCommand, Properties.CommandNames.CommonEntityKeyExplorerCommand);
        }
    }
}
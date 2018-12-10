using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class CommonEntityRelationshipManyToManyExplorerCommand : AbstractCommand
    {
        private CommonEntityRelationshipManyToManyExplorerCommand(Package package)
           : base(package, PackageGuids.guidCommandSet, PackageIds.CommonEntityRelationshipManyToManyExplorerCommandId, ActionExecute, ActionBeforeQueryStatus) { }

        public static CommonEntityRelationshipManyToManyExplorerCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new CommonEntityRelationshipManyToManyExplorerCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            helper.HandleOpenEntityRelationshipManyToManyExplorer();
        }

        private static void ActionBeforeQueryStatus(IServiceProviderOwner command, OleMenuCommand menuCommand)
        {
            CommonHandlers.CorrectCommandNameForConnectionName(command, menuCommand, Properties.CommandNames.CommonEntityRelationshipManyToManyExplorerCommand);
        }
    }
}
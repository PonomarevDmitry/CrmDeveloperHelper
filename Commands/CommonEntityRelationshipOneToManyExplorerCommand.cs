using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class CommonEntityRelationshipOneToManyExplorerCommand : AbstractCommand
    {
        private CommonEntityRelationshipOneToManyExplorerCommand(Package package)
           : base(package, PackageGuids.guidCommandSet, PackageIds.CommonEntityRelationshipOneToManyExplorerCommandId, ActionExecute, ActionBeforeQueryStatus) { }

        public static CommonEntityRelationshipOneToManyExplorerCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new CommonEntityRelationshipOneToManyExplorerCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            helper.HandleOpenEntityRelationshipOneToManyExplorer();
        }

        private static void ActionBeforeQueryStatus(IServiceProviderOwner command, OleMenuCommand menuCommand)
        {
            CommonHandlers.CorrectCommandNameForConnectionName(command, menuCommand, Properties.CommandNames.CommonEntityRelationshipOneToManyExplorerCommand);
        }
    }
}
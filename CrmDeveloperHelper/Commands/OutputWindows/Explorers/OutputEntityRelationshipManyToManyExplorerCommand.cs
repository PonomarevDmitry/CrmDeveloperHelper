using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.OutputWindows.Explorers
{
    internal sealed class OutputEntityRelationshipManyToManyExplorerCommand : AbstractOutputWindowCommand
    {
        private OutputEntityRelationshipManyToManyExplorerCommand(OleMenuCommandService commandService)
           : base(commandService, PackageIds.guidCommandSet.OutputEntityRelationshipManyToManyExplorerCommandId) { }

        public static OutputEntityRelationshipManyToManyExplorerCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new OutputEntityRelationshipManyToManyExplorerCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            helper.HandleOpenEntityRelationshipManyToManyExplorer(connectionData);
        }
    }
}
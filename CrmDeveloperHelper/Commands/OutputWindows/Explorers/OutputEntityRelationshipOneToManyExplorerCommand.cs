using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.OutputWindows.Explorers
{
    internal sealed class OutputEntityRelationshipOneToManyExplorerCommand : AbstractOutputWindowCommand
    {
        private OutputEntityRelationshipOneToManyExplorerCommand(OleMenuCommandService commandService)
           : base(commandService, PackageIds.OutputEntityRelationshipOneToManyExplorerCommandId) { }

        public static OutputEntityRelationshipOneToManyExplorerCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new OutputEntityRelationshipOneToManyExplorerCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            helper.HandleOpenEntityRelationshipOneToManyExplorer(connectionData);
        }
    }
}
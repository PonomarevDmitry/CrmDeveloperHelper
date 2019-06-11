using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Explorers
{
    internal sealed class CommonSystemSavedQueryExplorerCommand : AbstractCommand
    {
        private CommonSystemSavedQueryExplorerCommand(OleMenuCommandService commandService)
           : base(commandService, PackageIds.CommonSystemSavedQueryExplorerCommandId) { }

        public static CommonSystemSavedQueryExplorerCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new CommonSystemSavedQueryExplorerCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper)
        {
            helper.HandleExplorerSystemSavedQuery();
        }
    }
}

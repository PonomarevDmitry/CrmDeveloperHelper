using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.FindEdit
{
    internal sealed class CommonFindEntityObjectsByIdCommand : AbstractCommand
    {
        private CommonFindEntityObjectsByIdCommand(OleMenuCommandService commandService)
           : base(commandService, PackageIds.CommonFindEntityObjectsByIdCommandId) { }

        public static CommonFindEntityObjectsByIdCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new CommonFindEntityObjectsByIdCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper)
        {
            helper.HandleFindEntityById();
        }
    }
}

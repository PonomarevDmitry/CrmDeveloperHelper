using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.FindEdit
{
    internal sealed class CommonEditEntityObjectsByIdCommand : AbstractCommand
    {
        private CommonEditEntityObjectsByIdCommand(OleMenuCommandService commandService)
           : base(commandService, PackageIds.CommonEditEntityObjectsByIdCommandId) { }

        public static CommonEditEntityObjectsByIdCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new CommonEditEntityObjectsByIdCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper)
        {
            helper.HandleEditEntityById();
        }
    }
}

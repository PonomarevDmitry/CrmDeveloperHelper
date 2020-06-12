using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.FindEdit
{
    internal sealed class CommonEditEntitiesByIdCommand : AbstractSingleCommand
    {
        private CommonEditEntitiesByIdCommand(OleMenuCommandService commandService)
           : base(commandService, PackageIds.guidCommandSet.CommonEditEntitiesByIdCommandId) { }

        public static CommonEditEntitiesByIdCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new CommonEditEntitiesByIdCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper)
        {
            helper.HandleEditEntityById();
        }
    }
}

using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.FindEdit
{
    internal sealed class CommonFindEntitiesByIdCommand : AbstractSingleCommand
    {
        private CommonFindEntitiesByIdCommand(OleMenuCommandService commandService)
           : base(commandService, PackageIds.guidCommandSet.CommonFindEntitiesByIdCommandId) { }

        public static CommonFindEntitiesByIdCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new CommonFindEntitiesByIdCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper)
        {
            helper.HandleFindEntityById();
        }
    }
}

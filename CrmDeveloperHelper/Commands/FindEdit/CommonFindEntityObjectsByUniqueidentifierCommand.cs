using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.FindEdit
{
    internal sealed class CommonFindEntityObjectsByUniqueidentifierCommand : AbstractCommand
    {
        private CommonFindEntityObjectsByUniqueidentifierCommand(OleMenuCommandService commandService)
           : base(commandService, PackageIds.CommonFindEntityObjectsByUniqueidentifierCommandId) { }

        public static CommonFindEntityObjectsByUniqueidentifierCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new CommonFindEntityObjectsByUniqueidentifierCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper)
        {
            helper.HandleFindEntityByUniqueidentifier();
        }
    }
}

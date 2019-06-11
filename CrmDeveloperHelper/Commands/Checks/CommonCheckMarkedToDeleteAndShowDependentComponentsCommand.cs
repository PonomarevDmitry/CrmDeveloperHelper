using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Checks
{
    internal sealed class CommonCheckMarkedToDeleteAndShowDependentComponentsCommand : AbstractCommand
    {
        private CommonCheckMarkedToDeleteAndShowDependentComponentsCommand(OleMenuCommandService commandService)
           : base(commandService, PackageIds.CommonCheckMarkedToDeleteAndShowDependentComponentsCommandId) { }

        public static CommonCheckMarkedToDeleteAndShowDependentComponentsCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new CommonCheckMarkedToDeleteAndShowDependentComponentsCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper)
        {
            helper.HandleCheckMarkedAndShowDependentComponents();
        }
    }
}

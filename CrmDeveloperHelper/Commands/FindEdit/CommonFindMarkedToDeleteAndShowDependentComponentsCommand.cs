using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.FindEdit
{
    internal sealed class CommonFindMarkedToDeleteAndShowDependentComponentsCommand : AbstractCommand
    {
        private CommonFindMarkedToDeleteAndShowDependentComponentsCommand(OleMenuCommandService commandService)
           : base(commandService, PackageIds.CommonFindMarkedToDeleteAndShowDependentComponentsCommandId) { }

        public static CommonFindMarkedToDeleteAndShowDependentComponentsCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new CommonFindMarkedToDeleteAndShowDependentComponentsCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper)
        {
            helper.HandleCheckMarkedAndShowDependentComponents();
        }
    }
}

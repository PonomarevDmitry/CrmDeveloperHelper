using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.FindEdit
{
    internal sealed class CommonFindEntityObjectsMarkedToDeleteAndShowDependentComponentsCommand : AbstractCommand
    {
        private CommonFindEntityObjectsMarkedToDeleteAndShowDependentComponentsCommand(OleMenuCommandService commandService)
           : base(commandService, PackageIds.guidCommandSet.CommonFindEntityObjectsMarkedToDeleteAndShowDependentComponentsCommandId) { }

        public static CommonFindEntityObjectsMarkedToDeleteAndShowDependentComponentsCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new CommonFindEntityObjectsMarkedToDeleteAndShowDependentComponentsCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper)
        {
            helper.HandleFindMarkedToDeleteAndShowDependentComponents();
        }
    }
}

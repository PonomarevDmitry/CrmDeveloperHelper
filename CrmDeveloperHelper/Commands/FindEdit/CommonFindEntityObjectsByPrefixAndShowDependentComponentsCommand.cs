using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.FindEdit
{
    internal sealed class CommonFindEntityObjectsByPrefixAndShowDependentComponentsCommand : AbstractCommand
    {
        private CommonFindEntityObjectsByPrefixAndShowDependentComponentsCommand(OleMenuCommandService commandService)
           : base(commandService, PackageIds.guidCommandSet.CommonFindEntityObjectsByPrefixAndShowDependentComponentsCommandId) { }

        public static CommonFindEntityObjectsByPrefixAndShowDependentComponentsCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new CommonFindEntityObjectsByPrefixAndShowDependentComponentsCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper)
        {
            helper.HandleFindEntityObjectsByPrefixAndShowDependentComponents();
        }
    }
}

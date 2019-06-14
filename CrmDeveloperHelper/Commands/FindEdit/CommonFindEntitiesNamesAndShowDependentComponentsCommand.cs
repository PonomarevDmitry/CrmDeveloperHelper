using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.FindEdit
{
    internal sealed class CommonFindEntitiesNamesAndShowDependentComponentsCommand : AbstractCommand
    {
        private CommonFindEntitiesNamesAndShowDependentComponentsCommand(OleMenuCommandService commandService)
           : base(commandService, PackageIds.CommonFindEntitiesNamesAndShowDependentComponentsCommandId) { }

        public static CommonFindEntitiesNamesAndShowDependentComponentsCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new CommonFindEntitiesNamesAndShowDependentComponentsCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper)
        {
            helper.HandleCheckEntitiesNamesAndShowDependentComponents();
        }
    }
}

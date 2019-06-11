using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Checks
{
    internal sealed class CommonCheckEntitiesNamesAndShowDependentComponentsCommand : AbstractCommand
    {
        private CommonCheckEntitiesNamesAndShowDependentComponentsCommand(OleMenuCommandService commandService)
           : base(commandService, PackageIds.CommonCheckEntitiesNamesAndShowDependentComponentsCommandId) { }

        public static CommonCheckEntitiesNamesAndShowDependentComponentsCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new CommonCheckEntitiesNamesAndShowDependentComponentsCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper)
        {
            helper.HandleCheckEntitiesNamesAndShowDependentComponents();
        }
    }
}

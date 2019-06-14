using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.FindEdit
{
    internal sealed class CommonFindEntitiesNamesCommand : AbstractCommand
    {
        private CommonFindEntitiesNamesCommand(OleMenuCommandService commandService)
           : base(commandService, PackageIds.CommonFindEntitiesNamesCommandId) { }

        public static CommonFindEntitiesNamesCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new CommonFindEntitiesNamesCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper)
        {
            helper.HandleCheckEntitiesNames();
        }
    }
}

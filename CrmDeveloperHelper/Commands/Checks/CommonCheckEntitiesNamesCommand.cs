using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Checks
{
    internal sealed class CommonCheckEntitiesNamesCommand : AbstractCommand
    {
        private CommonCheckEntitiesNamesCommand(OleMenuCommandService commandService)
           : base(commandService, PackageIds.CommonCheckEntitiesNamesCommandId) { }

        public static CommonCheckEntitiesNamesCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new CommonCheckEntitiesNamesCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper)
        {
            helper.HandleCheckEntitiesNames();
        }
    }
}

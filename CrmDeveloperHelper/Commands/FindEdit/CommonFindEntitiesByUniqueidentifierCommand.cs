using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.FindEdit
{
    internal sealed class CommonFindEntitiesByUniqueidentifierCommand : AbstractSingleCommand
    {
        private CommonFindEntitiesByUniqueidentifierCommand(OleMenuCommandService commandService)
           : base(commandService, PackageIds.guidCommandSet.CommonFindEntitiesByUniqueidentifierCommandId) { }

        public static CommonFindEntitiesByUniqueidentifierCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new CommonFindEntitiesByUniqueidentifierCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper)
        {
            helper.HandleFindEntityByUniqueidentifier();
        }
    }
}

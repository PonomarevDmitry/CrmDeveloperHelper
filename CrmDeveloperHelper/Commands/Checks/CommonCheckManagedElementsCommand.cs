using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Checks
{
    internal sealed class CommonCheckManagedElementsCommand : AbstractCommandByConnectionAll
    {
        private CommonCheckManagedElementsCommand(OleMenuCommandService commandService)
            : base(
                commandService
                , PackageIds.CommonCheckManagedElementsCommandId
            )
        {

        }

        public static CommonCheckManagedElementsCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new CommonCheckManagedElementsCommand(commandService);
        }

        private static void ActionExecute(DTEHelper helper, ConnectionData connectionData)
        {
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            helper.HandleCheckManagedEntities(connectionData);
        }
    }
}

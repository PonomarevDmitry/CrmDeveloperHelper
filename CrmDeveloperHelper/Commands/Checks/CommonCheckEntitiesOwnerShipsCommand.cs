using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Checks
{
    internal sealed class CommonCheckEntitiesOwnerShipsCommand : AbstractDynamicCommandByConnectionAll
    {
        private CommonCheckEntitiesOwnerShipsCommand(OleMenuCommandService commandService)
            : base(
                commandService
                , PackageIds.CommonCheckEntitiesOwnerShipsCommandId
            )
        {

        }

        public static CommonCheckEntitiesOwnerShipsCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new CommonCheckEntitiesOwnerShipsCommand(commandService);
        }

        private static void ActionExecute(DTEHelper helper, ConnectionData connectionData)
        {
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            helper.HandleCheckEntitiesOwnership(connectionData);
        }
    }
}

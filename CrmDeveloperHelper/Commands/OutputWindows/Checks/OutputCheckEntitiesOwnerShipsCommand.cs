using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.OutputWindows.Checks
{
    internal sealed class OutputCheckEntitiesOwnerShipsCommand : AbstractOutputWindowCommand
    {
        private OutputCheckEntitiesOwnerShipsCommand(OleMenuCommandService commandService)
            : base(
                commandService
                , PackageIds.guidCommandSet.OutputCheckEntitiesOwnerShipsCommandId
            )
        {

        }

        public static OutputCheckEntitiesOwnerShipsCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new OutputCheckEntitiesOwnerShipsCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            helper.HandleCheckEntitiesOwnership(connectionData);
        }
    }
}

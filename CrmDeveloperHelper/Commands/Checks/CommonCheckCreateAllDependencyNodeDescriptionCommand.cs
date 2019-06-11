using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Checks
{
    internal sealed class CommonCheckCreateAllDependencyNodeDescriptionCommand : AbstractCommandByConnectionAll
    {
        private CommonCheckCreateAllDependencyNodeDescriptionCommand(OleMenuCommandService commandService)
            : base(
                commandService
                , PackageIds.CommonCheckCreateAllDependencyNodeDescriptionCommandId
            )
        {

        }

        public static CommonCheckCreateAllDependencyNodeDescriptionCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new CommonCheckCreateAllDependencyNodeDescriptionCommand(commandService);
        }

        private static void ActionExecute(DTEHelper helper, ConnectionData connectionData)
        {
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            helper.HandleCreateAllDependencyNodesDescription(connectionData);
        }
    }
}
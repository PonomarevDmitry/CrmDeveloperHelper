using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.OutputWindows.Checks
{
    internal sealed class OutputCheckCreateAllDependencyNodeDescriptionCommand : AbstractOutputWindowCommand
    {
        private OutputCheckCreateAllDependencyNodeDescriptionCommand(OleMenuCommandService commandService)
            : base(
                commandService
                , PackageIds.guidCommandSet.OutputCheckCreateAllDependencyNodeDescriptionCommandId
            )
        {

        }

        public static OutputCheckCreateAllDependencyNodeDescriptionCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new OutputCheckCreateAllDependencyNodeDescriptionCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            helper.HandleCheckCreateAllDependencyNodesDescription(connectionData);
        }
    }
}
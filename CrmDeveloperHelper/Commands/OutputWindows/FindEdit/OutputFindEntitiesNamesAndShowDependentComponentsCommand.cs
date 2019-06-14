using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.FindEdit
{
    internal sealed class OutputFindEntitiesNamesAndShowDependentComponentsCommand : AbstractCommand
    {
        private OutputFindEntitiesNamesAndShowDependentComponentsCommand(OleMenuCommandService commandService)
           : base(commandService, PackageIds.OutputFindEntitiesNamesAndShowDependentComponentsCommandId) { }

        public static OutputFindEntitiesNamesAndShowDependentComponentsCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new OutputFindEntitiesNamesAndShowDependentComponentsCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper)
        {
            var connectionData = helper.GetOutputWindowConnection();

            if (connectionData == null)
            {
                return;
            }

            helper.HandleCheckEntitiesNamesAndShowDependentComponents(connectionData);
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusIsConnectionOutput(applicationObject, menuCommand);
        }
    }
}

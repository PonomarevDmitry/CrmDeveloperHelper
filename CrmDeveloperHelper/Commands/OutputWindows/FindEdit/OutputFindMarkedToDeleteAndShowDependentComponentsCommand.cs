using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.FindEdit
{
    internal sealed class OutputFindMarkedToDeleteAndShowDependentComponentsCommand : AbstractCommand
    {
        private OutputFindMarkedToDeleteAndShowDependentComponentsCommand(OleMenuCommandService commandService)
           : base(commandService, PackageIds.OutputFindMarkedToDeleteAndShowDependentComponentsCommandId) { }

        public static OutputFindMarkedToDeleteAndShowDependentComponentsCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new OutputFindMarkedToDeleteAndShowDependentComponentsCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper)
        {
            var connectionData = helper.GetOutputWindowConnection();

            if (connectionData == null)
            {
                return;
            }

            helper.HandleCheckMarkedAndShowDependentComponents(connectionData);
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusIsConnectionOutput(applicationObject, menuCommand);
        }
    }
}

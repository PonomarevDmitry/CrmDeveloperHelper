using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.FindEdit
{
    internal sealed class OutputFindEntityObjectsByIdCommand : AbstractCommand
    {
        private OutputFindEntityObjectsByIdCommand(OleMenuCommandService commandService)
           : base(commandService, PackageIds.OutputFindEntityObjectsByIdCommandId) { }

        public static OutputFindEntityObjectsByIdCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new OutputFindEntityObjectsByIdCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper)
        {
            var connectionData = helper.GetOutputWindowConnection();

            if (connectionData == null)
            {
                return;
            }

            helper.HandleFindEntityById(connectionData);
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusIsConnectionOutput(applicationObject, menuCommand);
        }
    }
}

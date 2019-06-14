using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.FindEdit
{
    internal sealed class OutputFindEntityObjectsByUniqueidentifierCommand : AbstractCommand
    {
        private OutputFindEntityObjectsByUniqueidentifierCommand(OleMenuCommandService commandService)
           : base(commandService, PackageIds.OutputFindEntityObjectsByUniqueidentifierCommandId) { }

        public static OutputFindEntityObjectsByUniqueidentifierCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new OutputFindEntityObjectsByUniqueidentifierCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper)
        {
            var connectionData = helper.GetOutputWindowConnection();

            if (connectionData == null)
            {
                return;
            }

            helper.HandleFindEntityByUniqueidentifier(connectionData);
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusIsConnectionOutput(applicationObject, menuCommand);
        }
    }
}

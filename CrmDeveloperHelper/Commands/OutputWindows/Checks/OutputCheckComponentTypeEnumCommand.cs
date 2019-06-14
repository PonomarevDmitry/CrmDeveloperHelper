using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.OutputWindows.Checks
{
    internal sealed class OutputCheckComponentTypeEnumCommand : AbstractCommand
    {
        private OutputCheckComponentTypeEnumCommand(OleMenuCommandService commandService)
            : base(
                commandService
                , PackageIds.OutputCheckComponentTypeEnumCommandId
            )
        {

        }

        public static OutputCheckComponentTypeEnumCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new OutputCheckComponentTypeEnumCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper)
        {
            var connectionData = helper.GetOutputWindowConnection();

            if (connectionData == null)
            {
                return;
            }

            helper.HandleCheckComponentTypeEnum(connectionData);
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusIsConnectionOutput(applicationObject, menuCommand);
        }
    }
}

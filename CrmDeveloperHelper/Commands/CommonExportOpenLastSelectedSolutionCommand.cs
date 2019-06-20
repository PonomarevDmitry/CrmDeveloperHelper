using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class CommonExportOpenLastSelectedSolutionCommand : AbstractDynamicCommandOnSolutionLast
    {
        private readonly ActionOpenComponent _actionOpen;

        private CommonExportOpenLastSelectedSolutionCommand(OleMenuCommandService commandService, int baseIdStart, ActionOpenComponent action)
            : base(
                commandService
                , baseIdStart
            )
        {
            this._actionOpen = action;
        }

        public static CommonExportOpenLastSelectedSolutionCommand InstanceOpenInWeb { get; private set; }

        public static CommonExportOpenLastSelectedSolutionCommand InstanceOpenInWindow { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            InstanceOpenInWeb = new CommonExportOpenLastSelectedSolutionCommand(commandService, PackageIds.CommonExportOpenLastSelectedSolutionInWebCommandId, ActionOpenComponent.OpenInWeb);

            InstanceOpenInWindow = new CommonExportOpenLastSelectedSolutionCommand(commandService, PackageIds.CommonExportOpenLastSelectedSolutionInExplorerCommandId, ActionOpenComponent.OpenInExplorer);
        }

        protected override void CommandAction(DTEHelper helper, string solutionUniqueName)
        {
            var connectionConfig = ConnectionConfiguration.Get();

            if (connectionConfig.CurrentConnectionData != null)
            {
                helper.HandleOpenLastSelectedSolution(connectionConfig.CurrentConnectionData, solutionUniqueName, this._actionOpen);
            }
        }
    }
}
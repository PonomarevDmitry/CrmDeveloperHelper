using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Commons
{
    internal sealed class CommonExportOpenLastSelectedSolutionCommand : AbstractDynamicCommandOnSolutionLast
    {
        private readonly ActionOnComponent _actionOpen;

        private CommonExportOpenLastSelectedSolutionCommand(OleMenuCommandService commandService, int baseIdStart, ActionOnComponent action)
            : base(
                commandService
                , baseIdStart
            )
        {
            this._actionOpen = action;
        }

        public static CommonExportOpenLastSelectedSolutionCommand InstanceOpenInWeb { get; private set; }

        public static CommonExportOpenLastSelectedSolutionCommand InstanceOpenInExplorer { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            InstanceOpenInWeb = new CommonExportOpenLastSelectedSolutionCommand(commandService, PackageIds.guidDynamicCommandSet.CommonExportOpenLastSelectedSolutionInWebCommandId, ActionOnComponent.OpenInWeb);

            InstanceOpenInExplorer = new CommonExportOpenLastSelectedSolutionCommand(commandService, PackageIds.guidDynamicCommandSet.CommonExportOpenLastSelectedSolutionInExplorerCommandId, ActionOnComponent.OpenInExplorer);
        }

        protected override void CommandAction(DTEHelper helper, string solutionUniqueName)
        {
            var connectionConfig = ConnectionConfiguration.Get();

            if (connectionConfig.CurrentConnectionData != null)
            {
                helper.HandleSolutionOpenLastSelected(connectionConfig.CurrentConnectionData, solutionUniqueName, this._actionOpen);
            }
        }
    }
}
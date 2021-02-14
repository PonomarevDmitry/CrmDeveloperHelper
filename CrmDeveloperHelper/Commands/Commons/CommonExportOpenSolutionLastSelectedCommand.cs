using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Commons
{
    internal sealed class CommonExportOpenSolutionLastSelectedCommand : AbstractDynamicCommandOnSolutionLastSelected
    {
        private readonly ActionOnComponent _actionOnComponent;

        private CommonExportOpenSolutionLastSelectedCommand(OleMenuCommandService commandService, int baseIdStart, ActionOnComponent actionOnComponent)
            : base(commandService, baseIdStart)
        {
            this._actionOnComponent = actionOnComponent;
        }

        public static CommonExportOpenSolutionLastSelectedCommand InstanceOpenInWeb { get; private set; }

        public static CommonExportOpenSolutionLastSelectedCommand InstanceOpenInExplorer { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            InstanceOpenInWeb = new CommonExportOpenSolutionLastSelectedCommand(commandService, PackageIds.guidDynamicSolutionLastSelectedCommandSet.CommonExportOpenSolutionLastSelectedInWebCommandId, ActionOnComponent.OpenInWeb);

            InstanceOpenInExplorer = new CommonExportOpenSolutionLastSelectedCommand(commandService, PackageIds.guidDynamicSolutionLastSelectedCommandSet.CommonExportOpenSolutionLastSelectedInExplorerCommandId, ActionOnComponent.OpenInExplorer);
        }

        protected override void CommandAction(DTEHelper helper, string solutionUniqueName)
        {
            var connectionConfig = ConnectionConfiguration.Get();

            if (connectionConfig.CurrentConnectionData != null)
            {
                helper.HandleSolutionOpenLastSelected(connectionConfig.CurrentConnectionData, solutionUniqueName, this._actionOnComponent);
            }
        }
    }
}
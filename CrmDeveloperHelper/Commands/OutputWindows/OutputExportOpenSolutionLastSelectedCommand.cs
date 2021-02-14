using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.OutputWindows
{
    internal sealed class OutputExportOpenSolutionLastSelectedCommand : AbstractOutputWindowDynamicCommandOnSolutionLastSelected
    {
        private readonly ActionOnComponent _actionOnComponent;

        private OutputExportOpenSolutionLastSelectedCommand(OleMenuCommandService commandService, int baseIdStart, ActionOnComponent actionOnComponent)
            : base(commandService, baseIdStart)
        {
            this._actionOnComponent = actionOnComponent;
        }

        public static OutputExportOpenSolutionLastSelectedCommand InstanceOpenInWeb { get; private set; }

        public static OutputExportOpenSolutionLastSelectedCommand InstanceOpenInExplorer { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            InstanceOpenInWeb = new OutputExportOpenSolutionLastSelectedCommand(
                commandService
                , PackageIds.guidDynamicSolutionLastSelectedCommandSet.OutputExportOpenSolutionLastSelectedInWebCommandId
                , ActionOnComponent.OpenInWeb
            );

            InstanceOpenInExplorer = new OutputExportOpenSolutionLastSelectedCommand(
                commandService
                , PackageIds.guidDynamicSolutionLastSelectedCommandSet.OutputExportOpenSolutionLastSelectedInExplorerCommandId
                , ActionOnComponent.OpenInExplorer
            );
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData, string solutionUniqueName)
        {
            helper.HandleSolutionOpenLastSelected(connectionData, solutionUniqueName, this._actionOnComponent);
        }
    }
}
using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper;
using Nav.Common.VSPackages.CrmDeveloperHelper.Commands;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.OutputWindows
{
    internal sealed class OutputExportOpenLastSelectedSolutionCommand : AbstractOutputWindowDynamicCommand<string>
    {
        private readonly ActionOnComponent _actionOpen;

        private OutputExportOpenLastSelectedSolutionCommand(OleMenuCommandService commandService, int baseIdStart, ActionOnComponent action)
            : base(
                commandService
                , baseIdStart
                , ConnectionData.CountLastSolutions
            )
        {
            this._actionOpen = action;
        }

        public static OutputExportOpenLastSelectedSolutionCommand InstanceOpenInWeb { get; private set; }

        public static OutputExportOpenLastSelectedSolutionCommand InstanceOpenInExplorer { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            InstanceOpenInWeb = new OutputExportOpenLastSelectedSolutionCommand(commandService, PackageIds.guidDynamicCommandSet.OutputExportOpenLastSelectedSolutionInWebCommandId, ActionOnComponent.OpenInWeb);

            InstanceOpenInExplorer = new OutputExportOpenLastSelectedSolutionCommand(commandService, PackageIds.guidDynamicCommandSet.OutputExportOpenLastSelectedSolutionInExplorerCommandId, ActionOnComponent.OpenInExplorer);
        }

        protected override ICollection<string> GetElementSourceCollection(ConnectionData connectionData)
        {
            return connectionData.LastSelectedSolutionsUniqueName;
        }

        protected override string GetElementName(ConnectionData connectionData, string solutionUniqueName)
        {
            return solutionUniqueName;
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData, string solutionUniqueName)
        {
            helper.HandleSolutionOpenLastSelected(connectionData, solutionUniqueName, this._actionOpen);
        }
    }
}
using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper;
using Nav.Common.VSPackages.CrmDeveloperHelper.Commands;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.OutputWindows
{
    internal sealed class OutputExportOpenLastSelectedSolutionCommand : AbstractDynamicCommand<string>
    {
        private readonly Collection<string> EmptyCollection = new Collection<string>();

        private readonly ActionOpenComponent _actionOpen;

        private OutputExportOpenLastSelectedSolutionCommand(OleMenuCommandService commandService, int baseIdStart, ActionOpenComponent action)
            : base(
                commandService
                , baseIdStart
                , ConnectionData.CountLastSolutions
            )
        {
            this._actionOpen = action;
        }

        public static OutputExportOpenLastSelectedSolutionCommand InstanceOpenInWeb { get; private set; }

        public static OutputExportOpenLastSelectedSolutionCommand InstanceOpenInWindow { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            InstanceOpenInWeb = new OutputExportOpenLastSelectedSolutionCommand(commandService, PackageIds.OutputExportOpenLastSelectedSolutionInWebCommandId, ActionOpenComponent.OpenInWeb);

            InstanceOpenInWindow = new OutputExportOpenLastSelectedSolutionCommand(commandService, PackageIds.OutputExportOpenLastSelectedSolutionInWindowCommandId, ActionOpenComponent.OpenInWindow);
        }

        protected override ICollection<string> GetElementSourceCollection()
        {
            var applicationObject = CrmDeveloperHelperPackage.Singleton.ApplicationObject;
            if (applicationObject != null)
            {
                var helper = DTEHelper.Create(applicationObject);

                var connectionData = helper.GetOutputWindowConnection();

                if (connectionData != null)
                {
                    return connectionData.LastSelectedSolutionsUniqueName;
                }
            }

            return EmptyCollection;
        }

        protected override string GetElementName(string solutionUniqueName)
        {
            return solutionUniqueName;
        }

        protected override void CommandAction(DTEHelper helper, string solutionUniqueName)
        {
            var connectionData = helper.GetOutputWindowConnection();

            if (connectionData == null)
            {
                return;
            }

            helper.HandleOpenLastSelectedSolution(connectionData, solutionUniqueName, this._actionOpen);
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, string element, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusIsConnectionOutput(applicationObject, menuCommand);
        }
    }
}
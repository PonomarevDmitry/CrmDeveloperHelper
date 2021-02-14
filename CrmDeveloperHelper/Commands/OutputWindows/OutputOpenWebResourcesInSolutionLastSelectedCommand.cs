using EnvDTE80;
using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.OutputWindows
{
    internal sealed class OutputOpenWebResourcesInSolutionLastSelectedCommand : AbstractOutputWindowDynamicCommandOnSolutionLastSelected
    {
        private readonly bool _inTextEditor;

        private OutputOpenWebResourcesInSolutionLastSelectedCommand(OleMenuCommandService commandService, int baseIdStart, bool inTextEditor)
            : base(commandService, baseIdStart)
        {
            this._inTextEditor = inTextEditor;
        }

        public static OutputOpenWebResourcesInSolutionLastSelectedCommand Instance { get; private set; }

        public static OutputOpenWebResourcesInSolutionLastSelectedCommand InstanceInTextEditor { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new OutputOpenWebResourcesInSolutionLastSelectedCommand(commandService, PackageIds.guidDynamicSolutionLastSelectedCommandSet.OutputOpenWebResourcesInSolutionLastSelectedCommandId, false);

            InstanceInTextEditor = new OutputOpenWebResourcesInSolutionLastSelectedCommand(commandService, PackageIds.guidDynamicSolutionLastSelectedCommandSet.OutputOpenWebResourcesInSolutionLastSelectedInTextEditorCommandId, true);
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData, string solutionUniqueName)
        {
            helper.HandleSolutionOpenWebResourcesInLastSelected(connectionData, solutionUniqueName, this._inTextEditor);
        }

        protected override void CommandBeforeQueryStatus(DTE2 applicationObject, ConnectionData connectionData, string solutionUniqueName, OleMenuCommand menuCommand)
        {
            if (_inTextEditor)
            {
                CommonHandlers.ActionBeforeQueryStatusTextEditorProgramExists(applicationObject, menuCommand);
            }
        }
    }
}

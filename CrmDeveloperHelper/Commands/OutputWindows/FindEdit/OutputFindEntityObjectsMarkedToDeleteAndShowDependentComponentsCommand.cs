using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.OutputWindows.FindEdit
{
    internal sealed class OutputFindEntityObjectsMarkedToDeleteAndShowDependentComponentsCommand : AbstractOutputWindowCommand
    {
        private OutputFindEntityObjectsMarkedToDeleteAndShowDependentComponentsCommand(OleMenuCommandService commandService)
           : base(commandService, PackageIds.guidCommandSet.OutputFindEntityObjectsMarkedToDeleteAndShowDependentComponentsCommandId) { }

        public static OutputFindEntityObjectsMarkedToDeleteAndShowDependentComponentsCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new OutputFindEntityObjectsMarkedToDeleteAndShowDependentComponentsCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            helper.HandleFindMarkedToDeleteAndShowDependentComponents(connectionData);
        }
    }
}

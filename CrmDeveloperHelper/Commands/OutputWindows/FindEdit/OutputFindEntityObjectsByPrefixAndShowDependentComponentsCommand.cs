using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.OutputWindows.FindEdit
{
    internal sealed class OutputFindEntityObjectsByPrefixAndShowDependentComponentsCommand : AbstractOutputWindowCommand
    {
        private OutputFindEntityObjectsByPrefixAndShowDependentComponentsCommand(OleMenuCommandService commandService)
           : base(commandService, PackageIds.guidCommandSet.OutputFindEntityObjectsByPrefixAndShowDependentComponentsCommandId) { }

        public static OutputFindEntityObjectsByPrefixAndShowDependentComponentsCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new OutputFindEntityObjectsByPrefixAndShowDependentComponentsCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            helper.HandleFindEntityObjectsByPrefixAndShowDependentComponents(connectionData);
        }
    }
}

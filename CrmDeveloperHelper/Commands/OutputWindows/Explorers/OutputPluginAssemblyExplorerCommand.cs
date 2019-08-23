using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.OutputWindows.Explorers
{
    internal sealed class OutputPluginAssemblyExplorerCommand : AbstractOutputWindowCommand
    {
        private OutputPluginAssemblyExplorerCommand(OleMenuCommandService commandService)
           : base(commandService, PackageIds.guidCommandSet.OutputPluginAssemblyExplorerCommandId) { }

        public static OutputPluginAssemblyExplorerCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new OutputPluginAssemblyExplorerCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            helper.HandleOpenPluginAssemblyExplorer(connectionData);
        }
    }
}

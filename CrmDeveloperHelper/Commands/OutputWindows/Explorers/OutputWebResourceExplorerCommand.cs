using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.WebResources
{
    internal sealed class OutputWebResourceExplorerCommand : AbstractOutputWindowCommand
    {
        private OutputWebResourceExplorerCommand(OleMenuCommandService commandService)
           : base(commandService, PackageIds.OutputWebResourceExplorerCommandId) { }

        public static OutputWebResourceExplorerCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new OutputWebResourceExplorerCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            helper.HandleExportWebResource(connectionData);
        }
    }
}

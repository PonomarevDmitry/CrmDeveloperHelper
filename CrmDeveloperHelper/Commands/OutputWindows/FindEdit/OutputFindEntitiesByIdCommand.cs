using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.OutputWindows.FindEdit
{
    internal sealed class OutputFindEntitiesByIdCommand : AbstractOutputWindowCommand
    {
        private OutputFindEntitiesByIdCommand(OleMenuCommandService commandService)
           : base(commandService, PackageIds.OutputFindEntitiesByIdCommandId) { }

        public static OutputFindEntitiesByIdCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new OutputFindEntitiesByIdCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            helper.HandleFindEntityById(connectionData);
        }
    }
}

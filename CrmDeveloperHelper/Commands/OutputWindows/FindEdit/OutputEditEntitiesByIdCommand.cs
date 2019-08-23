using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.OutputWindows.FindEdit
{
    internal sealed class OutputEditEntitiesByIdCommand : AbstractOutputWindowCommand
    {
        private OutputEditEntitiesByIdCommand(OleMenuCommandService commandService)
           : base(commandService, PackageIds.guidCommandSet.OutputEditEntitiesByIdCommandId) { }

        public static OutputEditEntitiesByIdCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new OutputEditEntitiesByIdCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            helper.HandleEditEntityById(connectionData);
        }
    }
}

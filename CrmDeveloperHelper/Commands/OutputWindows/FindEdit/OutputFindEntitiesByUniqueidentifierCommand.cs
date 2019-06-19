using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.FindEdit
{
    internal sealed class OutputFindEntitiesByUniqueidentifierCommand : AbstractOutputWindowCommand
    {
        private OutputFindEntitiesByUniqueidentifierCommand(OleMenuCommandService commandService)
           : base(commandService, PackageIds.OutputFindEntitiesByUniqueidentifierCommandId) { }

        public static OutputFindEntitiesByUniqueidentifierCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new OutputFindEntitiesByUniqueidentifierCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            helper.HandleFindEntityByUniqueidentifier(connectionData);
        }
    }
}

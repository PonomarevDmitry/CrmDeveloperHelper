using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.OutputWindows.Checks
{
    internal sealed class OutputCheckManagedElementsCommand : AbstractOutputWindowCommand
    {
        private OutputCheckManagedElementsCommand(OleMenuCommandService commandService)
            : base(
                commandService
                , PackageIds.OutputCheckManagedElementsCommandId
            )
        {

        }

        public static OutputCheckManagedElementsCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new OutputCheckManagedElementsCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            helper.HandleCheckManagedEntities(connectionData);
        }
    }
}

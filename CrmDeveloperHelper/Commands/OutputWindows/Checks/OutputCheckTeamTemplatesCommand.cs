using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.OutputWindows.Checks
{
    internal sealed class OutputCheckTeamTemplatesCommand : AbstractOutputWindowCommand
    {
        private OutputCheckTeamTemplatesCommand(OleMenuCommandService commandService)
            : base(commandService, PackageIds.guidCommandSet.OutputCheckTeamTemplatesCommandId)
        {
        }

        public static OutputCheckTeamTemplatesCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new OutputCheckTeamTemplatesCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            helper.HandleCheckTeamTemplates(connectionData);
        }
    }
}

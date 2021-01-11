using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.OutputWindows.Checks
{
    internal sealed class OutputCheckCreateMissingTeamTemplatesInSystemFormsCommand : AbstractOutputWindowCommand
    {
        private OutputCheckCreateMissingTeamTemplatesInSystemFormsCommand(OleMenuCommandService commandService)
            : base(commandService, PackageIds.guidCommandSet.OutputCheckCreateMissingTeamTemplatesInSystemFormsCommandId)
        {
        }

        public static OutputCheckCreateMissingTeamTemplatesInSystemFormsCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new OutputCheckCreateMissingTeamTemplatesInSystemFormsCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            helper.HandleCreateMissingTeamTemplatesInSystemForms(connectionData);
        }
    }
}

using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Checks
{
    internal sealed class CommonCheckCreateMissingTeamTemplatesInSystemFormsCommand : AbstractDynamicCommandByConnectionAll
    {
        private CommonCheckCreateMissingTeamTemplatesInSystemFormsCommand(OleMenuCommandService commandService)
            : base(commandService, PackageIds.guidDynamicCommandSet.CommonCheckCreateMissingTeamTemplatesInSystemFormsCommandId)
        {
        }

        public static CommonCheckCreateMissingTeamTemplatesInSystemFormsCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new CommonCheckCreateMissingTeamTemplatesInSystemFormsCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            helper.HandleCreateMissingTeamTemplatesInSystemForms(connectionData);
        }
    }
}

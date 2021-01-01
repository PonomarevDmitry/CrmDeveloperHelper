using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Checks
{
    internal sealed class CommonCheckSystemFormsWithNonExistentTeamTemplateCommand : AbstractDynamicCommandByConnectionAll
    {
        private CommonCheckSystemFormsWithNonExistentTeamTemplateCommand(OleMenuCommandService commandService)
            : base(commandService, PackageIds.guidDynamicCommandSet.CommonCheckSystemFormsWithNonExistentTeamTemplateCommandId)
        {
        }

        public static CommonCheckSystemFormsWithNonExistentTeamTemplateCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new CommonCheckSystemFormsWithNonExistentTeamTemplateCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            helper.HandleCheckSystemFormsWithNonExistentTeamTemplate(connectionData);
        }
    }
}

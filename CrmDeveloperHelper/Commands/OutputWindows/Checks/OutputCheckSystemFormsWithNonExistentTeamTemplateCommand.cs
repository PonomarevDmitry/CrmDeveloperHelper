using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.OutputWindows.Checks
{
    internal sealed class OutputCheckSystemFormsWithNonExistentTeamTemplateCommand : AbstractOutputWindowCommand
    {
        private OutputCheckSystemFormsWithNonExistentTeamTemplateCommand(OleMenuCommandService commandService)
            : base(commandService, PackageIds.guidCommandSet.OutputCheckSystemFormsWithNonExistentTeamTemplateCommandId)
        {
        }

        public static OutputCheckSystemFormsWithNonExistentTeamTemplateCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new OutputCheckSystemFormsWithNonExistentTeamTemplateCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            helper.HandleCheckSystemFormsWithNonExistentTeamTemplate(connectionData);
        }
    }
}

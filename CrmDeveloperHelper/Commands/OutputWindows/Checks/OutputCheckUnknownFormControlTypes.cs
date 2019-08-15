using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.OutputWindows.Checks
{
    internal sealed class OutputCheckUnknownFormControlTypes : AbstractOutputWindowCommand
    {
        private OutputCheckUnknownFormControlTypes(OleMenuCommandService commandService)
            : base(
                commandService
                , PackageIds.OutputCheckUnknownFormControlTypesId
            )
        {

        }

        public static OutputCheckUnknownFormControlTypes Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new OutputCheckUnknownFormControlTypes(commandService);
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            helper.HandleCheckUnknownFormControlTypes(connectionData);
        }
    }
}

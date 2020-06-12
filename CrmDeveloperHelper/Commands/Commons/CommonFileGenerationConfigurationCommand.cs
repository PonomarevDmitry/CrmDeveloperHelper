using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Commons
{
    internal sealed class CommonFileGenerationConfigurationCommand : AbstractSingleCommand
    {
        private CommonFileGenerationConfigurationCommand(OleMenuCommandService commandService)
            : base(commandService, PackageIds.guidCommandSet.CommonFileGenerationConfigurationCommandId)
        {
        }

        public static CommonFileGenerationConfigurationCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new CommonFileGenerationConfigurationCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper)
        {
            helper.HandleFileGenerationConfiguration();
        }
    }
}
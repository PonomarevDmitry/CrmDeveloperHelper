using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Commons
{
    internal sealed class CommonFileGenerationOptionsCommand : AbstractSingleCommand
    {
        private CommonFileGenerationOptionsCommand(OleMenuCommandService commandService)
            : base(commandService, PackageIds.guidCommandSet.CommonFileGenerationOptionsCommandId)
        {
        }

        public static CommonFileGenerationOptionsCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new CommonFileGenerationOptionsCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper)
        {
            helper.HandleFileGenerationOptions();
        }
    }
}

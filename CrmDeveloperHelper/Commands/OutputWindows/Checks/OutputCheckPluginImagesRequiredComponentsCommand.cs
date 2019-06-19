using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.OutputWindows.Checks
{
    internal sealed class OutputCheckPluginImagesRequiredComponentsCommand : AbstractOutputWindowCommand
    {
        private OutputCheckPluginImagesRequiredComponentsCommand(OleMenuCommandService commandService)
            : base(
                commandService
                , PackageIds.OutputCheckPluginImagesRequiredComponentsCommandId
            )
        {

        }

        public static OutputCheckPluginImagesRequiredComponentsCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new OutputCheckPluginImagesRequiredComponentsCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            helper.HandleCheckPluginImagesRequiredComponents(connectionData);
        }
    }
}

using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Checks
{
    internal sealed class CommonCheckPluginImagesRequiredComponentsCommand : AbstractDynamicCommandByConnectionAll
    {
        private CommonCheckPluginImagesRequiredComponentsCommand(OleMenuCommandService commandService)
            : base(
                commandService
                , PackageIds.guidDynamicCommandSet.CommonCheckPluginImagesRequiredComponentsCommandId
            )
        {

        }

        public static CommonCheckPluginImagesRequiredComponentsCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new CommonCheckPluginImagesRequiredComponentsCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            helper.HandleCheckPluginImagesRequiredComponents(connectionData);
        }
    }
}

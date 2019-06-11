using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Checks
{
    internal sealed class CommonCheckPluginImagesCommand : AbstractDynamicCommandByConnectionAll
    {
        private CommonCheckPluginImagesCommand(OleMenuCommandService commandService)
            : base(
                commandService
                , PackageIds.CommonCheckPluginImagesCommandId
            )
        {

        }

        public static CommonCheckPluginImagesCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new CommonCheckPluginImagesCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            helper.HandleCheckPluginImages(connectionData);
        }
    }
}

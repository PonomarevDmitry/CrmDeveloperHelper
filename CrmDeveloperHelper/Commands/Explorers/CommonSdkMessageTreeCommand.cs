using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Explorers
{
    internal sealed class CommonSdkMessageTreeCommand : AbstractCommand
    {
        private CommonSdkMessageTreeCommand(OleMenuCommandService commandService)
           : base(commandService, PackageIds.CommonSdkMessageTreeCommandId) { }

        public static CommonSdkMessageTreeCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new CommonSdkMessageTreeCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper)
        {
            helper.HandleSdkMessageTree();
        }
    }
}

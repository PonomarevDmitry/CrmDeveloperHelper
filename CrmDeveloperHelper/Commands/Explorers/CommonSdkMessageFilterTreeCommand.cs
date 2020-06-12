using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Explorers
{
    internal sealed class CommonSdkMessageFilterTreeCommand : AbstractSingleCommand
    {
        private CommonSdkMessageFilterTreeCommand(OleMenuCommandService commandService)
           : base(commandService, PackageIds.guidCommandSet.CommonSdkMessageFilterTreeCommandId)
        {
        }

        public static CommonSdkMessageFilterTreeCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new CommonSdkMessageFilterTreeCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper)
        {
            helper.HandleSdkMessageFilterTree();
        }
    }
}
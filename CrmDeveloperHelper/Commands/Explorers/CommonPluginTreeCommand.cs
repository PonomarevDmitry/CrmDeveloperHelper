using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Explorers
{
    internal sealed class CommonPluginTreeCommand : AbstractCommand
    {
        private CommonPluginTreeCommand(OleMenuCommandService commandService)
           : base(commandService, PackageIds.guidCommandSet.CommonPluginTreeCommandId) { }

        public static CommonPluginTreeCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new CommonPluginTreeCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper)
        {
            string selection = helper.GetSelectedText();

            helper.HandleOpenPluginTree(selection, string.Empty, string.Empty);
        }
    }
}

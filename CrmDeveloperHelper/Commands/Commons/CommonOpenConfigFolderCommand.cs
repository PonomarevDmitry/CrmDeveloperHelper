using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Commons
{
    internal sealed class CommonOpenConfigFolderCommand : AbstractSingleCommand
    {
        private CommonOpenConfigFolderCommand(OleMenuCommandService commandService)
            : base(commandService, PackageIds.guidCommandSet.CommonOpenConfigFolderCommandId) { }

        public static CommonOpenConfigFolderCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new CommonOpenConfigFolderCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper)
        {
            helper.HandleOpenConfigFolderCommand();
        }
    }
}
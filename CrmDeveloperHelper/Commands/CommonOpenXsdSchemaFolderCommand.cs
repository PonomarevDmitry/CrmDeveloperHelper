using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class CommonOpenXsdSchemaFolderCommand : AbstractCommand
    {
        private CommonOpenXsdSchemaFolderCommand(OleMenuCommandService commandService)
           : base(commandService, PackageIds.guidCommandSet.CommonOpenXsdSchemaFolderCommandId) { }

        public static CommonOpenXsdSchemaFolderCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new CommonOpenXsdSchemaFolderCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper)
        {
            helper.HandleOpenXsdSchemaFolder();
        }
    }
}

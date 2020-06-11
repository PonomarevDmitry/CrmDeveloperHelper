using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.OutputWindows
{
    internal sealed class OutputOpenConfigFolderCommand : AbstractOutputWindowCommand
    {
        private OutputOpenConfigFolderCommand(OleMenuCommandService commandService)
            : base(commandService, PackageIds.guidCommandSet.OutputOpenConfigFolderCommandId)
        {
        }

        public static OutputOpenConfigFolderCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new OutputOpenConfigFolderCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            helper.HandleOpenConfigFolderCommand();
        }
    }
}
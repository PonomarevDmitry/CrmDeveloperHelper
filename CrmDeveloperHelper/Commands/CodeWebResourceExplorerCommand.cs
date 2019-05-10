using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class CodeWebResourceExplorerCommand : AbstractCommand
    {
        private CodeWebResourceExplorerCommand(Package package)
            : base(package, PackageGuids.guidCommandSet, PackageIds.CodeWebResourceExplorerCommandId, ActionExecute, ActionBeforeQueryStatus) { }

        public static CodeWebResourceExplorerCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new CodeWebResourceExplorerCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            helper.HandleOpenWebResourceExplorerCommand();
        }

        private static void ActionBeforeQueryStatus(IServiceProviderOwner command, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusActiveDocumentWebResource(command, menuCommand);

            CommonHandlers.CorrectCommandNameForConnectionName(command, menuCommand, Properties.CommandNames.CodeWebResourceExplorerCommand);
        }
    }
}
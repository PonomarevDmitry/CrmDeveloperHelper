using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class CodeWebResourceDownloadCommand : AbstractCommand
    {
        private CodeWebResourceDownloadCommand(Package package)
            : base(package, PackageGuids.guidCommandSet, PackageIds.CodeWebResourceDownloadCommandId, ActionExecute, ActionBeforeQueryStatus) { }

        public static CodeWebResourceDownloadCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new CodeWebResourceDownloadCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            helper.HandleWebResourceDownloadCommand();
        }

        private static void ActionBeforeQueryStatus(IServiceProviderOwner command, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusActiveDocumentWebResource(command, menuCommand);

            CommonHandlers.CorrectCommandNameForConnectionName(command, menuCommand, Properties.CommandNames.CodeWebResourceDownloadCommand);
        }
    }
}
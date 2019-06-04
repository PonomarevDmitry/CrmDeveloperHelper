using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class CodeWebResourceShowDifferenceCustomCommand : AbstractCommand
    {
        private CodeWebResourceShowDifferenceCustomCommand(Package package)
            : base(package, PackageGuids.guidCommandSet, PackageIds.CodeWebResourceShowDifferenceCustomCommandId, ActionExecute, ActionBeforeQueryStatus) { }

        public static CodeWebResourceShowDifferenceCustomCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new CodeWebResourceShowDifferenceCustomCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            helper.HandleWebResourceDifferenceCommand(null, true);
        }

        private static void ActionBeforeQueryStatus(IServiceProviderOwner command, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusActiveDocumentWebResourceText(command, menuCommand);

            CommonHandlers.CorrectCommandNameForConnectionName(command, menuCommand, Properties.CommandNames.CodeWebResourceShowDifferenceCustomCommand);
        }
    }
}
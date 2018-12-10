using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class CodeWebResourceShowDifferenceCommand : AbstractCommand
    {
        private CodeWebResourceShowDifferenceCommand(Package package)
          : base(package, PackageGuids.guidCommandSet, PackageIds.CodeWebResourceShowDifferenceCommandId, ActionExecute, ActionBeforeQueryStatus) { }

        public static CodeWebResourceShowDifferenceCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new CodeWebResourceShowDifferenceCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            helper.HandleWebResourceDifferenceCommand(null, false);
        }

        private static void ActionBeforeQueryStatus(IServiceProviderOwner command, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusActiveDocumentWebResourceText(command, menuCommand);

            CommonHandlers.CorrectCommandNameForConnectionName(command, menuCommand, Properties.CommandNames.CodeWebResourceShowDifferenceCommand);
        }
    }
}
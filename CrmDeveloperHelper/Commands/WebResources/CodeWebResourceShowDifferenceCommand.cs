using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.WebResources
{
    internal sealed class CodeWebResourceShowDifferenceCommand : AbstractCommand
    {
        private CodeWebResourceShowDifferenceCommand(OleMenuCommandService commandService)
          : base(commandService, PackageIds.guidCommandSet.CodeWebResourceShowDifferenceCommandId) { }

        public static CodeWebResourceShowDifferenceCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new CodeWebResourceShowDifferenceCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper)
        {
            helper.HandleWebResourceDifferenceCommand(null, false);
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusActiveDocumentWebResourceText(applicationObject, menuCommand);

            CommonHandlers.CorrectCommandNameForConnectionName(applicationObject, menuCommand, Properties.CommandNames.CodeWebResourceShowDifferenceCommand);
        }
    }
}
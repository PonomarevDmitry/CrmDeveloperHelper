using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.WebResources.JavaScripts
{
    internal sealed class CodeJavaScriptLinkedGlobalOptionSetExplorerCommand : AbstractSingleCommand
    {
        private CodeJavaScriptLinkedGlobalOptionSetExplorerCommand(OleMenuCommandService commandService)
            : base(commandService, PackageIds.guidCommandSet.CodeJavaScriptLinkedGlobalOptionSetExplorerCommandId)
        {
        }

        public static CodeJavaScriptLinkedGlobalOptionSetExplorerCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new CodeJavaScriptLinkedGlobalOptionSetExplorerCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper)
        {
            if (helper.TryGetLinkedGlobalOptionSetName(out string optionSetName))
            {
                helper.HandleExportGlobalOptionSets(optionSetName);
            }
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusActiveDocumentJavaScriptHasLinkedGlobalOptionSetName(applicationObject, menuCommand);

            CommonHandlers.CorrectCommandNameForConnectionName(applicationObject, menuCommand, Properties.CommandNames.CodeJavaScriptLinkedGlobalOptionSetExplorerCommand);
        }
    }
}

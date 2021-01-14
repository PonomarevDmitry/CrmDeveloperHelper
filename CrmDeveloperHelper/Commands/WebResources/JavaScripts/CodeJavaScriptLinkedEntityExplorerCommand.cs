using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.WebResources.JavaScripts
{
    internal sealed class CodeJavaScriptLinkedEntityExplorerCommand : AbstractSingleCommand
    {
        private CodeJavaScriptLinkedEntityExplorerCommand(OleMenuCommandService commandService)
            : base(commandService, PackageIds.guidCommandSet.CodeJavaScriptLinkedEntityExplorerCommandId)
        {
        }

        public static CodeJavaScriptLinkedEntityExplorerCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new CodeJavaScriptLinkedEntityExplorerCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper)
        {
            if (helper.TryGetLinkedEntityName(out string entityName))
            {
                helper.HandleExportFileWithEntityMetadata(entityName);
            }
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusActiveDocumentJavaScriptHasLinkedEntityName(applicationObject, menuCommand);

            CommonHandlers.CorrectCommandNameForConnectionName(applicationObject, menuCommand, Properties.CommandNames.CodeJavaScriptLinkedEntityExplorerCommand);
        }
    }
}
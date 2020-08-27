using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using System;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.WebResources.JavaScripts
{
    internal sealed class CodeJavaScriptLinkedSystemFormExplorerCommand : AbstractSingleCommand
    {
        private CodeJavaScriptLinkedSystemFormExplorerCommand(OleMenuCommandService commandService)
            : base(commandService, PackageIds.guidCommandSet.CodeJavaScriptLinkedSystemFormExplorerCommandId)
        {
        }

        public static CodeJavaScriptLinkedSystemFormExplorerCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new CodeJavaScriptLinkedSystemFormExplorerCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper)
        {
            if (helper.TryGetLinkedSystemForm(out string entityName, out Guid formId, out int formType))
            {
                helper.HandleExplorerSystemForm(entityName, formId.ToString());
            }
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusActiveDocumentJavaScriptHasLinkedSystemForm(applicationObject, menuCommand);

            CommonHandlers.CorrectCommandNameForConnectionName(applicationObject, menuCommand, Properties.CommandNames.CodeJavaScriptLinkedSystemFormExplorerCommand);
        }
    }
}
using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using System;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.WebResources.JavaScripts
{
    internal sealed class CodeJavaScriptLinkedSystemFormAddToSolutionLastSelectedCommand : AbstractDynamicCommandOnSolutionLastSelected
    {
        private CodeJavaScriptLinkedSystemFormAddToSolutionLastSelectedCommand(OleMenuCommandService commandService)
            : base(commandService, PackageIds.guidDynamicSolutionLastSelectedCommandSet.CodeJavaScriptLinkedSystemFormAddToSolutionLastSelectedCommandId)
        {
        }

        public static CodeJavaScriptLinkedSystemFormAddToSolutionLastSelectedCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new CodeJavaScriptLinkedSystemFormAddToSolutionLastSelectedCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper, string solutionUniqueName)
        {
            if (helper.TryGetLinkedSystemForm(out string entityName, out Guid formId, out int formType))
            {
                helper.HandleLinkedSystemFormAddingToSolutionCommand(null, solutionUniqueName, false, new[] { formId });
            }
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, string solutionUniqueName, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusActiveDocumentJavaScriptHasLinkedSystemForm(applicationObject, menuCommand);
        }
    }
}
using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.JavaScripts
{
    internal sealed class CodeJavaScriptFileGenerationOptionsCommand : AbstractSingleCommand
    {
        private CodeJavaScriptFileGenerationOptionsCommand(OleMenuCommandService commandService)
            : base(commandService, PackageIds.guidCommandSet.CodeJavaScriptFileGenerationOptionsCommandId)
        {
        }

        public static CodeJavaScriptFileGenerationOptionsCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new CodeJavaScriptFileGenerationOptionsCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper)
        {
            helper.HandleJavaScriptFileGenerationOptions();
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusActiveDocumentJavaScript(applicationObject, menuCommand);

            CommonHandlers.CorrectCommandNameForConnectionName(applicationObject, menuCommand, Properties.CommandNames.JavaScriptFileGenerationOptionsCommand);
        }
    }
}

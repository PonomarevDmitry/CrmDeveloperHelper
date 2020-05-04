using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.CSharp
{
    internal sealed class DocumentsJavaScriptFileGenerationOptionsCommand : AbstractCommand
    {
        private DocumentsJavaScriptFileGenerationOptionsCommand(OleMenuCommandService commandService)
            : base(commandService, PackageIds.guidCommandSet.DocumentsJavaScriptFileGenerationOptionsCommandId)
        {
        }

        public static DocumentsJavaScriptFileGenerationOptionsCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new DocumentsJavaScriptFileGenerationOptionsCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper)
        {
            helper.HandleJavaScriptFileGenerationOptions();
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusOpenedDocumentsJavaScript(applicationObject, menuCommand);

            CommonHandlers.CorrectCommandNameForConnectionName(applicationObject, menuCommand, Properties.CommandNames.JavaScriptFileGenerationOptionsCommand);
        }
    }
}

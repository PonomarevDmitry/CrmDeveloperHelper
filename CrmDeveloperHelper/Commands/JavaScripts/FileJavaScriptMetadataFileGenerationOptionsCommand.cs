using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.JavaScripts
{
    internal sealed class FileJavaScriptFileGenerationOptionsCommand : AbstractSingleCommand
    {
        private FileJavaScriptFileGenerationOptionsCommand(OleMenuCommandService commandService)
            : base(commandService, PackageIds.guidCommandSet.FileJavaScriptFileGenerationOptionsCommandId)
        {
        }

        public static FileJavaScriptFileGenerationOptionsCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new FileJavaScriptFileGenerationOptionsCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper)
        {
            helper.HandleJavaScriptFileGenerationOptions();
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusSolutionExplorerJavaScriptAny(applicationObject, menuCommand);

            CommonHandlers.CorrectCommandNameForConnectionName(applicationObject, menuCommand, Properties.CommandNames.JavaScriptFileGenerationOptionsCommand);
        }
    }
}

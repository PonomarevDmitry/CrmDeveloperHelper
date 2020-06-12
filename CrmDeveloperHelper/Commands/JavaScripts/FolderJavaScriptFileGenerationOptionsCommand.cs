using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.CSharp
{
    internal sealed class FolderJavaScriptFileGenerationOptionsCommand : AbstractSingleCommand
    {
        private FolderJavaScriptFileGenerationOptionsCommand(OleMenuCommandService commandService)
            : base(commandService, PackageIds.guidCommandSet.FolderJavaScriptFileGenerationOptionsCommandId)
        {
        }

        public static FolderJavaScriptFileGenerationOptionsCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new FolderJavaScriptFileGenerationOptionsCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper)
        {
            helper.HandleJavaScriptFileGenerationOptions();
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusSolutionExplorerJavaScriptRecursive(applicationObject, menuCommand);

            CommonHandlers.CorrectCommandNameForConnectionName(applicationObject, menuCommand, Properties.CommandNames.JavaScriptFileGenerationOptionsCommand);
        }
    }
}

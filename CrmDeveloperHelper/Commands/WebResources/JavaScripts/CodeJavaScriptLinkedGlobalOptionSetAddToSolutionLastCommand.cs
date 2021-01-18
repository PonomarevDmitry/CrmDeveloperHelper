using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.WebResources.JavaScripts
{
    internal sealed class CodeJavaScriptLinkedGlobalOptionSetAddToSolutionLastCommand : CodeLinkedGlobalOptionSetAddToSolutionLastCommand
    {
        private CodeJavaScriptLinkedGlobalOptionSetAddToSolutionLastCommand(OleMenuCommandService commandService)
            : base(commandService, PackageIds.guidDynamicCommandSet.CodeJavaScriptLinkedGlobalOptionSetAddToSolutionLastCommandId, SelectedFileType.WebResourceJavaScriptHasLinkedGlobalOptionSet)
        {
        }

        public static CodeJavaScriptLinkedGlobalOptionSetAddToSolutionLastCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new CodeJavaScriptLinkedGlobalOptionSetAddToSolutionLastCommand(commandService);
        }
    }
}

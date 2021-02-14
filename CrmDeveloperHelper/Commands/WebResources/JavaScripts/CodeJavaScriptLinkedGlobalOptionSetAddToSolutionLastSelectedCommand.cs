using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.WebResources.JavaScripts
{
    internal sealed class CodeJavaScriptLinkedGlobalOptionSetAddToSolutionLastSelectedCommand : CodeLinkedGlobalOptionSetAddToSolutionLastSelectedCommand
    {
        private CodeJavaScriptLinkedGlobalOptionSetAddToSolutionLastSelectedCommand(OleMenuCommandService commandService)
            : base(commandService, PackageIds.guidDynamicSolutionLastSelectedCommandSet.CodeJavaScriptLinkedGlobalOptionSetAddToSolutionLastSelectedCommandId, SelectedFileType.WebResourceJavaScriptHasLinkedGlobalOptionSet)
        {
        }

        public static CodeJavaScriptLinkedGlobalOptionSetAddToSolutionLastSelectedCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new CodeJavaScriptLinkedGlobalOptionSetAddToSolutionLastSelectedCommand(commandService);
        }
    }
}

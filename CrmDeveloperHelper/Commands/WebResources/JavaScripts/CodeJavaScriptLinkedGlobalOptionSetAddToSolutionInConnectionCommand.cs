using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.WebResources.JavaScripts
{
    internal sealed class CodeJavaScriptLinkedGlobalOptionSetAddToSolutionInConnectionCommand : CodeLinkedGlobalOptionSetAddToSolutionInConnectionCommand
    {
        private CodeJavaScriptLinkedGlobalOptionSetAddToSolutionInConnectionCommand(OleMenuCommandService commandService)
            : base(commandService, PackageIds.guidDynamicCommandSet.CodeJavaScriptLinkedGlobalOptionSetAddToSolutionInConnectionCommandId, SelectedFileType.WebResourceJavaScriptHasLinkedGlobalOptionSet)
        {
        }

        public static CodeJavaScriptLinkedGlobalOptionSetAddToSolutionInConnectionCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new CodeJavaScriptLinkedGlobalOptionSetAddToSolutionInConnectionCommand(commandService);
        }
    }
}

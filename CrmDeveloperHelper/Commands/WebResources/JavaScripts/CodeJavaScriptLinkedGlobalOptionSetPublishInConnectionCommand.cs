using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.WebResources.JavaScripts
{
    internal sealed class CodeJavaScriptLinkedGlobalOptionSetPublishInConnectionCommand : CodeLinkedGlobalOptionSetPublishInConnectionCommand
    {
        private CodeJavaScriptLinkedGlobalOptionSetPublishInConnectionCommand(OleMenuCommandService commandService)
            : base(commandService, PackageIds.guidDynamicCommandSet.CodeJavaScriptLinkedGlobalOptionSetPublishInConnectionCommandId, SelectedFileType.WebResourceJavaScriptHasLinkedGlobalOptionSet)
        {
        }

        public static CodeJavaScriptLinkedGlobalOptionSetPublishInConnectionCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new CodeJavaScriptLinkedGlobalOptionSetPublishInConnectionCommand(commandService);
        }
    }
}

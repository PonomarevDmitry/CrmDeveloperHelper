using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.CSharp
{
    internal sealed class CodeCSharpLinkedGlobalOptionSetPublishInConnectionCommand : CodeLinkedGlobalOptionSetPublishInConnectionCommand
    {
        private CodeCSharpLinkedGlobalOptionSetPublishInConnectionCommand(OleMenuCommandService commandService)
            : base(commandService, PackageIds.guidDynamicCommandSet.CodeCSharpLinkedGlobalOptionSetPublishInConnectionCommandId, SelectedFileType.CSharpHasLinkedGlobalOptionSet)
        {
        }

        public static CodeCSharpLinkedGlobalOptionSetPublishInConnectionCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new CodeCSharpLinkedGlobalOptionSetPublishInConnectionCommand(commandService);
        }
    }
}

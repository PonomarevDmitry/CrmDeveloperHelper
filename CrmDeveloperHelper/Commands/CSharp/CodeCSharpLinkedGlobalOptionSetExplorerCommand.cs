using Microsoft.VisualStudio.Shell;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.CSharp
{
    internal sealed class CodeCSharpLinkedGlobalOptionSetExplorerCommand : CodeLinkedGlobalOptionSetExplorerCommand
    {
        private CodeCSharpLinkedGlobalOptionSetExplorerCommand(OleMenuCommandService commandService)
            : base(commandService, PackageIds.guidCommandSet.CodeCSharpLinkedGlobalOptionSetExplorerCommandId, Model.SelectedFileType.CSharpHasLinkedGlobalOptionSet)
        {
        }

        public static CodeCSharpLinkedGlobalOptionSetExplorerCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new CodeCSharpLinkedGlobalOptionSetExplorerCommand(commandService);
        }
    }
}

using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.CSharp
{
    internal sealed class CodeCSharpLinkedGlobalOptionSetAddToSolutionLastSelectedCommand : CodeLinkedGlobalOptionSetAddToSolutionLastSelectedCommand
    {
        private CodeCSharpLinkedGlobalOptionSetAddToSolutionLastSelectedCommand(OleMenuCommandService commandService)
            : base(commandService, PackageIds.guidDynamicSolutionLastSelectedCommandSet.CodeCSharpLinkedGlobalOptionSetAddToSolutionLastSelectedCommandId, SelectedFileType.CSharpHasLinkedGlobalOptionSet)
        {
        }

        public static CodeCSharpLinkedGlobalOptionSetAddToSolutionLastSelectedCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new CodeCSharpLinkedGlobalOptionSetAddToSolutionLastSelectedCommand(commandService);
        }
    }
}

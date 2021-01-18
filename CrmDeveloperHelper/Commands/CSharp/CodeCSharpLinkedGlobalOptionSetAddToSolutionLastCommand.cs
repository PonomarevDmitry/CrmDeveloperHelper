using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.CSharp
{
    internal sealed class CodeCSharpLinkedGlobalOptionSetAddToSolutionLastCommand : CodeLinkedGlobalOptionSetAddToSolutionLastCommand
    {
        private CodeCSharpLinkedGlobalOptionSetAddToSolutionLastCommand(OleMenuCommandService commandService)
            : base(commandService, PackageIds.guidDynamicCommandSet.CodeCSharpLinkedGlobalOptionSetAddToSolutionLastCommandId, SelectedFileType.CSharpHasLinkedGlobalOptionSet)
        {
        }

        public static CodeCSharpLinkedGlobalOptionSetAddToSolutionLastCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new CodeCSharpLinkedGlobalOptionSetAddToSolutionLastCommand(commandService);
        }
    }
}

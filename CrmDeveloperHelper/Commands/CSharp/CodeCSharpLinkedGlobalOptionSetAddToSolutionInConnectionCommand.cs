using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.CSharp
{
    internal sealed class CodeCSharpLinkedGlobalOptionSetAddToSolutionInConnectionCommand : CodeLinkedGlobalOptionSetAddToSolutionInConnectionCommand
    {
        private CodeCSharpLinkedGlobalOptionSetAddToSolutionInConnectionCommand(OleMenuCommandService commandService)
            : base(commandService, PackageIds.guidDynamicCommandSet.CodeCSharpLinkedGlobalOptionSetAddToSolutionInConnectionCommandId, SelectedFileType.CSharpHasLinkedGlobalOptionSet)
        {
        }

        public static CodeCSharpLinkedGlobalOptionSetAddToSolutionInConnectionCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new CodeCSharpLinkedGlobalOptionSetAddToSolutionInConnectionCommand(commandService);
        }
    }
}

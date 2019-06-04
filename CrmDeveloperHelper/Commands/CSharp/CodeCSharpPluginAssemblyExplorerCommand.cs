using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.CSharp
{
    internal sealed class CodeCSharpPluginAssemblyExplorerCommand : AbstractCommand
    {
        private CodeCSharpPluginAssemblyExplorerCommand(Package package)
            : base(package, PackageGuids.guidCommandSet, PackageIds.CodeCSharpPluginAssemblyExplorerCommandId, ActionExecute, ActionBeforeQueryStatus) { }

        public static CodeCSharpPluginAssemblyExplorerCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new CodeCSharpPluginAssemblyExplorerCommand(package);
        }

        private static void ActionBeforeQueryStatus(IServiceProviderOwner command, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusActiveDocumentCSharp(command, menuCommand);

            CommonHandlers.ActionBeforeQueryStatusActiveDocumentContainingProject(command, menuCommand);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            var document = helper.GetOpenedDocumentInCodeWindow(FileOperations.SupportsCSharpType);

            if (document != null
                && document.ProjectItem != null
                && document.ProjectItem.ContainingProject != null
                )
            {
                helper.HandleOpenPluginAssemblyExplorer(document.ProjectItem.ContainingProject.Name);
            }
        }
    }
}
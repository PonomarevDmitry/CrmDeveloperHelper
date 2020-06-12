using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.CSharp
{
    internal sealed class CodeCSharpPluginAssemblyExplorerCommand : AbstractSingleCommand
    {
        private CodeCSharpPluginAssemblyExplorerCommand(OleMenuCommandService commandService)
            : base(commandService, PackageIds.guidCommandSet.CodeCSharpPluginAssemblyExplorerCommandId) { }

        public static CodeCSharpPluginAssemblyExplorerCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new CodeCSharpPluginAssemblyExplorerCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper)
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

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusActiveDocumentCSharp(applicationObject, menuCommand);

            CommonHandlers.ActionBeforeQueryStatusActiveDocumentContainingProject(applicationObject, menuCommand);
        }
    }
}
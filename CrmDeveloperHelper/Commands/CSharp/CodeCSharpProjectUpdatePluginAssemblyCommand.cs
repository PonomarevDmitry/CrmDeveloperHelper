using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.CSharp
{
    internal sealed class CodeCSharpProjectUpdatePluginAssemblyCommand : AbstractCommand
    {
        private CodeCSharpProjectUpdatePluginAssemblyCommand(OleMenuCommandService commandService)
            : base(commandService, PackageIds.guidCommandSet.CodeCSharpProjectUpdatePluginAssemblyCommandId) { }

        public static CodeCSharpProjectUpdatePluginAssemblyCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new CodeCSharpProjectUpdatePluginAssemblyCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper)
        {
            var document = helper.GetOpenedDocumentInCodeWindow(FileOperations.SupportsCSharpType);

            if (document != null
                && document.ProjectItem != null
                && document.ProjectItem.ContainingProject != null
                )
            {
                helper.HandlePluginAssemblyUpdatingInWindowCommand(null, document.ProjectItem.ContainingProject);
            }
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusActiveDocumentCSharp(applicationObject, menuCommand);

            CommonHandlers.ActionBeforeQueryStatusActiveDocumentContainingProject(applicationObject, menuCommand);

            CommonHandlers.CorrectCommandNameForConnectionName(applicationObject, menuCommand, Properties.CommandNames.CodeCSharpProjectUpdatePluginAssemblyCommand);
        }
    }
}
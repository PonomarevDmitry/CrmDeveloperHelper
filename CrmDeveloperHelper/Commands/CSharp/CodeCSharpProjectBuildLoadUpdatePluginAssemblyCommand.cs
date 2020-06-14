using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.CSharp
{
    internal sealed class CodeCSharpProjectBuildLoadUpdatePluginAssemblyCommand : AbstractSingleCommand
    {
        private CodeCSharpProjectBuildLoadUpdatePluginAssemblyCommand(OleMenuCommandService commandService)
            : base(commandService, PackageIds.guidCommandSet.CodeCSharpProjectBuildLoadUpdatePluginAssemblyCommandId)
        {
        }

        public static CodeCSharpProjectBuildLoadUpdatePluginAssemblyCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new CodeCSharpProjectBuildLoadUpdatePluginAssemblyCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper)
        {
            var document = helper.GetOpenedDocumentInCodeWindow(FileOperations.SupportsCSharpType);

            if (document != null
                && document.ProjectItem != null
                && document.ProjectItem.ContainingProject != null
            )
            {
                helper.HandlePluginAssemblyBuildProjectUpdateCommand(null, false, document.ProjectItem.ContainingProject);
            }
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusActiveDocumentCSharp(applicationObject, menuCommand);

            CommonHandlers.ActionBeforeQueryStatusActiveDocumentContainingProject(applicationObject, menuCommand);

            CommonHandlers.CorrectCommandNameForConnectionName(applicationObject, menuCommand, Properties.CommandNames.CodeCSharpProjectBuildLoadUpdatePluginAssemblyCommand);
        }
    }
}

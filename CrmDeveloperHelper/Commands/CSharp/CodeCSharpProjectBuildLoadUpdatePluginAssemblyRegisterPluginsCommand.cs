using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.CSharp
{
    internal sealed class CodeCSharpProjectBuildLoadUpdatePluginAssemblyRegisterPluginsCommand : AbstractSingleCommand
    {
        private CodeCSharpProjectBuildLoadUpdatePluginAssemblyRegisterPluginsCommand(OleMenuCommandService commandService)
            : base(commandService, PackageIds.guidCommandSet.CodeCSharpProjectBuildLoadUpdatePluginAssemblyRegisterPluginsCommandId) { }

        public static CodeCSharpProjectBuildLoadUpdatePluginAssemblyRegisterPluginsCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new CodeCSharpProjectBuildLoadUpdatePluginAssemblyRegisterPluginsCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper)
        {
            var document = helper.GetOpenedDocumentInCodeWindow(FileOperations.SupportsCSharpType);

            if (document != null
                && document.ProjectItem != null
                && document.ProjectItem.ContainingProject != null
                )
            {
                helper.HandlePluginAssemblyBuildProjectUpdateCommand(null, true, document.ProjectItem.ContainingProject);
            }
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusActiveDocumentCSharp(applicationObject, menuCommand);

            CommonHandlers.ActionBeforeQueryStatusActiveDocumentContainingProject(applicationObject, menuCommand);

            CommonHandlers.CorrectCommandNameForConnectionName(applicationObject, menuCommand, Properties.CommandNames.CodeCSharpProjectBuildLoadUpdatePluginAssemblyRegisterPluginsCommand);
        }
    }
}

using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.CSharp
{
    internal sealed class CodeCSharpProjectBuildLoadUpdatePluginAssemblyRegisterPluginsInConnectionCommand : AbstractDynamicCommandByConnectionAllWithoutCurrent
    {
        private CodeCSharpProjectBuildLoadUpdatePluginAssemblyRegisterPluginsInConnectionCommand(OleMenuCommandService commandService)
            : base(commandService, PackageIds.guidDynamicCommandSet.CodeCSharpProjectBuildLoadUpdatePluginAssemblyRegisterPluginsInConnectionCommandId)
        {
        }

        public static CodeCSharpProjectBuildLoadUpdatePluginAssemblyRegisterPluginsInConnectionCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new CodeCSharpProjectBuildLoadUpdatePluginAssemblyRegisterPluginsInConnectionCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            var document = helper.GetOpenedDocumentInCodeWindow(FileOperations.SupportsCSharpType);

            if (document != null
                && document.ProjectItem != null
                && document.ProjectItem.ContainingProject != null
            )
            {
                helper.HandlePluginAssemblyBuildProjectUpdateCommand(connectionData, true, document.ProjectItem.ContainingProject);
            }
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, ConnectionData connectionData, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusActiveDocumentCSharp(applicationObject, menuCommand);

            CommonHandlers.ActionBeforeQueryStatusActiveDocumentContainingProject(applicationObject, menuCommand);
        }
    }
}
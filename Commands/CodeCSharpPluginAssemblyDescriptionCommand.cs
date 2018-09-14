using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class CodeCSharpPluginAssemblyDescriptionCommand : AbstractCommand
    {
        private CodeCSharpPluginAssemblyDescriptionCommand(Package package)
            : base(package, PackageGuids.guidCommandSet, PackageIds.CodeCSharpPluginAssemblyDescriptionCommandId, ActionExecute, ActionBeforeQueryStatus) { }

        public static CodeCSharpPluginAssemblyDescriptionCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new CodeCSharpPluginAssemblyDescriptionCommand(package);
        }

        private static void ActionBeforeQueryStatus(IServiceProviderOwner command, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusActiveDocumentCSharp(command, menuCommand);

            CommonHandlers.ActionBeforeQueryStatusActiveDocumentContainingProject(command, menuCommand, FileOperations.SupportsCSharpType);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            var document = helper.GetOpenedDocumentInCodeWindow(FileOperations.SupportsCSharpType);

            if (document != null
                && document.ProjectItem != null
                && document.ProjectItem.ContainingProject != null
                )
            {
                helper.HandleExportPluginAssembly(document.ProjectItem.ContainingProject.Name);
            }
        }
    }
}
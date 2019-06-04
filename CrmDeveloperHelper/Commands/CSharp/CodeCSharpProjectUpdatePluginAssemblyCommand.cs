using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class CodeCSharpProjectUpdatePluginAssemblyCommand : AbstractCommand
    {
        private CodeCSharpProjectUpdatePluginAssemblyCommand(Package package)
            : base(package, PackageGuids.guidCommandSet, PackageIds.CodeCSharpProjectUpdatePluginAssemblyCommandId, ActionExecute, ActionBeforeQueryStatus) { }

        public static CodeCSharpProjectUpdatePluginAssemblyCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new CodeCSharpProjectUpdatePluginAssemblyCommand(package);
        }

        private static void ActionBeforeQueryStatus(IServiceProviderOwner command, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusActiveDocumentCSharp(command, menuCommand);

            CommonHandlers.ActionBeforeQueryStatusActiveDocumentContainingProject(command, menuCommand);
            
            CommonHandlers.CorrectCommandNameForConnectionName(command, menuCommand, Properties.CommandNames.CodeCSharpProjectUpdatePluginAssemblyCommand);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            var document = helper.GetOpenedDocumentInCodeWindow(FileOperations.SupportsCSharpType);

            if (document != null
                && document.ProjectItem != null
                && document.ProjectItem.ContainingProject != null
                )
            {
                helper.HandleUpdatingPluginAssemblyCommand(null, document.ProjectItem.ContainingProject);
            }
        }
    }
}
using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.CSharp
{
    internal sealed class FileCSharpPluginAssemblyExplorerCommand : AbstractCommand
    {
        private FileCSharpPluginAssemblyExplorerCommand(OleMenuCommandService commandService)
            : base(commandService, PackageIds.FileCSharpPluginAssemblyExplorerCommandId) { }

        public static FileCSharpPluginAssemblyExplorerCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new FileCSharpPluginAssemblyExplorerCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper)
        {
            var projectItem = helper.GetSingleSelectedProjectItemInSolutionExplorer(FileOperations.SupportsCSharpType);

            if (projectItem != null)
            {
                string selection = string.Empty;

                if (projectItem != null && projectItem.ContainingProject != null)
                {
                    selection = projectItem.ContainingProject.Name;
                }
                else if (!string.IsNullOrEmpty(projectItem.Name))
                {
                    selection = projectItem.Name;
                }

                helper.HandleOpenPluginAssemblyExplorer(selection);
            }
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusSolutionExplorerCSharpSingle(applicationObject, menuCommand);

            CommonHandlers.ActionBeforeQueryStatusSolutionExplorerSingleItemContainsProject(applicationObject, menuCommand);
        }
    }
}
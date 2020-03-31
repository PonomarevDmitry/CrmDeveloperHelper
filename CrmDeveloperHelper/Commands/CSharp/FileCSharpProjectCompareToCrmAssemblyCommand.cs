using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.CSharp
{
    internal sealed class FileCSharpProjectCompareToCrmAssemblyCommand : AbstractCommand
    {
        private FileCSharpProjectCompareToCrmAssemblyCommand(OleMenuCommandService commandService)
            : base(commandService, PackageIds.guidCommandSet.FileCSharpProjectCompareToCrmAssemblyCommandId) { }

        public static FileCSharpProjectCompareToCrmAssemblyCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new FileCSharpProjectCompareToCrmAssemblyCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper)
        {
            var projectItem = helper.GetSingleSelectedProjectItemInSolutionExplorer(FileOperations.SupportsCSharpType);

            if (projectItem != null && projectItem.ContainingProject != null)
            {
                helper.HandlePluginAssemblyComparingWithLocalAssemblyCommand(null, projectItem.ContainingProject);
            }
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusSolutionExplorerCSharpSingle(applicationObject, menuCommand);

            CommonHandlers.ActionBeforeQueryStatusSolutionExplorerSingleItemContainsProject(applicationObject, menuCommand);

            CommonHandlers.CorrectCommandNameForConnectionName(applicationObject, menuCommand, Properties.CommandNames.FileCSharpProjectCompareToCrmAssemblyCommand);
        }
    }
}
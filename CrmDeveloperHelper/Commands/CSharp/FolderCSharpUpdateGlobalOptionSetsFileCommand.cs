using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System.Collections.Generic;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.CSharp
{
    internal sealed class FolderCSharpUpdateGlobalOptionSetsFileCommand : AbstractDynamicCommandByConnectionAll
    {
        private FolderCSharpUpdateGlobalOptionSetsFileCommand(OleMenuCommandService commandService)
            : base(
                commandService
                , PackageIds.guidDynamicCommandSet.FolderCSharpUpdateGlobalOptionSetsFileCommandId
            )
        {

        }

        public static FolderCSharpUpdateGlobalOptionSetsFileCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new FolderCSharpUpdateGlobalOptionSetsFileCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            List<SelectedFile> selectedFiles = helper.GetSelectedFilesInSolutionExplorer(FileOperations.SupportsCSharpType, true).ToList();

            helper.HandleCSharpGlobalOptionSetsFileUpdateSchema(connectionData, selectedFiles, false);
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, ConnectionData connectionData, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusSolutionExplorerCSharpRecursive(applicationObject, menuCommand);
        }
    }
}
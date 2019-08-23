using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System.Collections.Generic;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.WebResources
{
    internal sealed class FolderWebResourceCompareInConnectionGroupCommand : AbstractDynamicCommandByConnectionByGroupWithoutCurrent
    {
        private readonly bool _withDetails;

        private FolderWebResourceCompareInConnectionGroupCommand(OleMenuCommandService commandService, int baseIdStart, bool withDetails)
            : base(
                commandService
                , baseIdStart
            )
        {
            this._withDetails = withDetails;
        }

        public static FolderWebResourceCompareInConnectionGroupCommand Instance { get; private set; }

        public static FolderWebResourceCompareInConnectionGroupCommand InstanceWithDetails { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new FolderWebResourceCompareInConnectionGroupCommand(commandService, PackageIds.guidDynamicCommandSet.FolderWebResourceCompareInConnectionGroupCommandId, false);

            InstanceWithDetails = new FolderWebResourceCompareInConnectionGroupCommand(commandService, PackageIds.guidDynamicCommandSet.FolderWebResourceCompareWithDetailsInConnectionGroupCommandId, true);
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            List<SelectedFile> selectedFiles = helper.GetSelectedFilesInSolutionExplorer(FileOperations.SupportsWebResourceTextType, true).ToList();

            helper.HandleFileCompareCommand(connectionData, selectedFiles, this._withDetails);
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, ConnectionData connectionData, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusSolutionExplorerWebResourceRecursive(applicationObject, menuCommand);
        }
    }
}

using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System.Collections.Generic;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.WebResources
{
    internal sealed class FileWebResourceCompareInConnectionGroupCommand : AbstractDynamicCommandByConnectionByGroupWithoutCurrent
    {
        private readonly bool _withDetails;

        private FileWebResourceCompareInConnectionGroupCommand(OleMenuCommandService commandService, int baseIdStart, bool withDetails)
            : base(
                commandService
                , baseIdStart
            )
        {
            this._withDetails = withDetails;
        }

        public static FileWebResourceCompareInConnectionGroupCommand Instance { get; private set; }

        public static FileWebResourceCompareInConnectionGroupCommand InstanceWithDetails { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new FileWebResourceCompareInConnectionGroupCommand(commandService, PackageIds.guidDynamicCommandSet.FileWebResourceCompareInConnectionGroupCommandId, false);

            InstanceWithDetails = new FileWebResourceCompareInConnectionGroupCommand(commandService, PackageIds.guidDynamicCommandSet.FileWebResourceCompareWithDetailsInConnectionGroupCommandId, true);
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            List<SelectedFile> selectedFiles = helper.GetSelectedFilesInSolutionExplorer(FileOperations.SupportsWebResourceTextType, false).ToList();

            helper.HandleFileCompareCommand(connectionData, selectedFiles, this._withDetails);
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, ConnectionData connectionData, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusSolutionExplorerWebResourceTextAny(applicationObject, menuCommand);
        }
    }
}
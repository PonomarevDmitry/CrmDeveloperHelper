using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.ListForPublish
{
    internal sealed class ListForPublishFilesRemoveCommand : AbstractCommand
    {
        private ListForPublishFilesRemoveCommand(OleMenuCommandService commandService)
           : base(commandService, PackageIds.ListForPublishFilesRemoveCommandId) { }

        public static ListForPublishFilesRemoveCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new ListForPublishFilesRemoveCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper)
        {
            var selectedFiles = helper.GetOpenedFileInCodeWindow(FileOperations.SupportsWebResourceType).ToList();

            helper.RemoveFromListForPublish(selectedFiles);
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusConnectionIsNotReadOnly(applicationObject, menuCommand);

            CommonHandlers.ActionBeforeQueryStatusFilesToAdd(applicationObject, menuCommand);
        }
    }
}

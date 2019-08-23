using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.ListForPublish
{
    internal sealed class ListForPublishFilesAddCommand : AbstractCommand
    {
        private ListForPublishFilesAddCommand(OleMenuCommandService commandService)
           : base(commandService, PackageIds.guidCommandSet.ListForPublishFilesAddCommandId) { }

        public static ListForPublishFilesAddCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new ListForPublishFilesAddCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper)
        {
            var selectedFiles = helper.GetSelectedFilesAll(FileOperations.SupportsWebResourceType, true);

            helper.AddToListForPublish(selectedFiles);
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusConnectionIsNotReadOnly(applicationObject, menuCommand);

            CommonHandlers.ActionBeforeQueryStatusFilesToAdd(applicationObject, menuCommand);
        }
    }
}

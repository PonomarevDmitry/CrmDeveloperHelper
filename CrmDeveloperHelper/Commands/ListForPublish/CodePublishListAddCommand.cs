using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System.Collections.Generic;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.ListForPublish
{
    internal sealed class CodePublishListAddCommand : AbstractSingleCommand
    {
        private CodePublishListAddCommand(OleMenuCommandService commandService)
            : base(commandService, PackageIds.guidCommandSet.CodePublishListAddCommandId) { }

        public static CodePublishListAddCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new CodePublishListAddCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper)
        {
            List<SelectedFile> selectedFiles = helper.GetOpenedFileInCodeWindow(FileOperations.SupportsWebResourceType).ToList();

            helper.AddToListForPublish(selectedFiles);
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusConnectionIsNotReadOnly(applicationObject, menuCommand);

            CommonHandlers.ActionBeforeQueryStatusActiveDocumentWebResource(applicationObject, menuCommand);
        }
    }
}
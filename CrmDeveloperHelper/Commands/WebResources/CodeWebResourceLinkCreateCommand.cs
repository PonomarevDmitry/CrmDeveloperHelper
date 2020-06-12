using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System.Collections.Generic;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.WebResources
{
    internal sealed class CodeWebResourceLinkCreateCommand : AbstractSingleCommand
    {
        private CodeWebResourceLinkCreateCommand(OleMenuCommandService commandService)
            : base(commandService, PackageIds.guidCommandSet.CodeWebResourceLinkCreateCommandId) { }

        public static CodeWebResourceLinkCreateCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new CodeWebResourceLinkCreateCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper)
        {
            List<SelectedFile> selectedFiles = helper.GetOpenedFileInCodeWindow(FileOperations.SupportsWebResourceType).ToList();

            helper.HandleWebResourceCreateLaskLinkMultipleCommand(selectedFiles);
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusActiveDocumentWebResource(applicationObject, menuCommand);
        }
    }
}

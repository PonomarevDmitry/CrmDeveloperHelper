using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System.Collections.Generic;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class CodeWebResourceUpdateContentPublishCommand : AbstractCommand
    {
        private CodeWebResourceUpdateContentPublishCommand(Package package)
            : base(package, PackageGuids.guidCommandSet, PackageIds.CodeWebResourceUpdateContentPublishCommandId, ActionExecute, ActionBeforeQueryStatus) { }

        public static CodeWebResourceUpdateContentPublishCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new CodeWebResourceUpdateContentPublishCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            List<SelectedFile> selectedFiles = helper.GetOpenedFileInCodeWindow(FileOperations.SupportsWebResourceType);

            helper.HandleUpdateContentWebResourcesAndPublishCommand(null, selectedFiles);
        }

        private static void ActionBeforeQueryStatus(IServiceProviderOwner command, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusConnectionIsNotReadOnly(command, menuCommand);

            CommonHandlers.ActionBeforeQueryStatusActiveDocumentWebResource(command, menuCommand);

            CommonHandlers.CorrectCommandNameForConnectionName(command, menuCommand, Properties.CommandNames.CodeWebResourceUpdateContentPublishCommand);
        }
    }
}
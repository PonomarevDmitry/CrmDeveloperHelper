using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System.Collections.Generic;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class CodeWebResourceLinkClearCommand : AbstractCommand
    {
        private CodeWebResourceLinkClearCommand(Package package)
          : base(package, PackageGuids.guidCommandSet, PackageIds.CodeWebResourceLinkClearCommandId, ActionExecute, CommonHandlers.ActionBeforeQueryStatusActiveDocumentWebResource) { }

        public static CodeWebResourceLinkClearCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new CodeWebResourceLinkClearCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            List<SelectedFile> selectedFiles = helper.GetOpenedFileInCodeWindow(FileOperations.SupportsWebResourceType);

            helper.HandleFileClearLink(selectedFiles);
        }
    }
}

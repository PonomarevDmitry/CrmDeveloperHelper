using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System.Collections.Generic;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.WebResources
{
    internal sealed class CodeWebResourceLinkCreateCommand : AbstractCommand
    {
        private CodeWebResourceLinkCreateCommand(Package package)
            : base(package, PackageGuids.guidCommandSet, PackageIds.CodeWebResourceLinkCreateCommandId, ActionExecute, CommonHandlers.ActionBeforeQueryStatusActiveDocumentWebResource) { }

        public static CodeWebResourceLinkCreateCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new CodeWebResourceLinkCreateCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            List<SelectedFile> selectedFiles = helper.GetOpenedFileInCodeWindow(FileOperations.SupportsWebResourceType).ToList();

            helper.HandleCreateLaskLinkWebResourcesMultipleCommand(selectedFiles);
        }
    }
}

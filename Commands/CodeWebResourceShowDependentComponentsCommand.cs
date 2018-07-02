using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System.Collections.Generic;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class CodeWebResourceShowDependentComponentsCommand : AbstractCommand
    {
        private CodeWebResourceShowDependentComponentsCommand(Package package)
            : base(package, PackageGuids.guidCommandSet, PackageIds.CodeWebResourceShowDependentComponentsCommandId, ActionExecute, CommonHandlers.ActionBeforeQueryStatusActiveDocumentWebResource) { }

        public static CodeWebResourceShowDependentComponentsCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new CodeWebResourceShowDependentComponentsCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            List<SelectedFile> selectedFiles = helper.GetOpenedFileInCodeWindow(FileOperations.SupportsWebResourceType);

            helper.HandleShowingWebResourcesDependentComponents(selectedFiles);
        }
    }
}

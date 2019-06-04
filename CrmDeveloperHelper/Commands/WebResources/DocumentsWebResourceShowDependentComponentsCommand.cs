using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System.Collections.Generic;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.WebResources
{
    internal sealed class DocumentsWebResourceShowDependentComponentsCommand : AbstractCommand
    {
        private DocumentsWebResourceShowDependentComponentsCommand(Package package)
            : base(package, PackageGuids.guidCommandSet, PackageIds.DocumentsWebResourceShowDependentComponentsCommandId, ActionExecute, CommonHandlers.ActionBeforeQueryStatusOpenedDocumentsWebResource) { }

        public static DocumentsWebResourceShowDependentComponentsCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new DocumentsWebResourceShowDependentComponentsCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            List<SelectedFile> selectedFiles = helper.GetOpenedDocuments(FileOperations.SupportsWebResourceType).ToList();

            helper.HandleShowingWebResourcesDependentComponents(selectedFiles);
        }
    }
}

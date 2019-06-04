using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System.Collections.Generic;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.WebResources
{
    internal class CodeWebResourceCheckEncodingCommand : AbstractCommand
    {
        private CodeWebResourceCheckEncodingCommand(Package package)
            : base(package, PackageGuids.guidCommandSet, PackageIds.CodeWebResourceCheckEncodingCommandId, ActionExecute, CommonHandlers.ActionBeforeQueryStatusActiveDocumentWebResourceText) { }

        public static CodeWebResourceCheckEncodingCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new CodeWebResourceCheckEncodingCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            List<SelectedFile> selectedFiles = helper.GetOpenedFileInCodeWindow(FileOperations.SupportsWebResourceTextType).ToList();

            helper.HandleCheckFileEncodingCommand(selectedFiles);
        }
    }
}
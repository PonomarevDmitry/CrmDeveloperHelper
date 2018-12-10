using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System.Collections.Generic;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class CodeCSharpUpdateEntityMetadataFileWithEntitySelectCommand : AbstractCommand
    {
        private CodeCSharpUpdateEntityMetadataFileWithEntitySelectCommand(Package package)
            : base(package, PackageGuids.guidCommandSet, PackageIds.CodeCSharpUpdateEntityMetadataFileWithEntitySelectCommandId, ActionExecute, CommonHandlers.ActionBeforeQueryStatusActiveDocumentCSharp) { }

        public static CodeCSharpUpdateEntityMetadataFileWithEntitySelectCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new CodeCSharpUpdateEntityMetadataFileWithEntitySelectCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            List<SelectedFile> selectedFiles = helper.GetOpenedFileInCodeWindow(FileOperations.SupportsCSharpType);

            helper.HandleUpdateEntityMetadataFile(selectedFiles, true);
        }
    }
}

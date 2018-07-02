using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System.Collections.Generic;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class CodeCSharpPluginTypeDescriptionCommand : AbstractCommand
    {
        private CodeCSharpPluginTypeDescriptionCommand(Package package)
            : base(package, PackageGuids.guidCommandSet, PackageIds.CodeCSharpPluginTypeDescriptionCommandId, ActionExecute, CommonHandlers.ActionBeforeQueryStatusActiveDocumentCSharp) { }

        public static CodeCSharpPluginTypeDescriptionCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new CodeCSharpPluginTypeDescriptionCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            List<SelectedFile> selectedFiles = helper.GetOpenedFileInCodeWindow(FileOperations.SupportsCSharpType);

            var file = selectedFiles.FirstOrDefault();

            if (file != null)
            {
                string selection = file.Name.Split('.').FirstOrDefault();

                helper.HandleExportPluginTypeDescription(selection);
            }
        }
    }
}
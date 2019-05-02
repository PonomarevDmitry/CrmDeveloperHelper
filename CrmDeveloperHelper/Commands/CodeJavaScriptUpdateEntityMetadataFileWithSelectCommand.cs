using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System.Collections.Generic;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class CodeJavaScriptUpdateEntityMetadataFileWithSelectCommand : AbstractCommand
    {
        private CodeJavaScriptUpdateEntityMetadataFileWithSelectCommand(Package package)
            : base(package, PackageGuids.guidCommandSet, PackageIds.CodeJavaScriptUpdateEntityMetadataFileWithSelectCommandId, ActionExecute, ActionBeforeQueryStatus) { }

        public static CodeJavaScriptUpdateEntityMetadataFileWithSelectCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new CodeJavaScriptUpdateEntityMetadataFileWithSelectCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            List<SelectedFile> selectedFiles = helper.GetOpenedFileInCodeWindow(FileOperations.SupportsJavaScriptType).ToList();

            helper.HandleUpdateEntityMetadataFileJavaScript(null, selectedFiles, true);
        }

        private static void ActionBeforeQueryStatus(IServiceProviderOwner command, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusActiveDocumentJavaScript(command, menuCommand);

            CommonHandlers.CorrectCommandNameForConnectionName(command, menuCommand, Properties.CommandNames.CodeJavaScriptUpdateEntityMetadataFileWithSelectCommand);
        }
    }
}

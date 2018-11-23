using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System.Collections.Generic;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    /// <summary>
    /// Command handler
    /// </summary>
    internal sealed class CodeWebResourceCompareWithDetailsCommand : AbstractCommand
    {
        private CodeWebResourceCompareWithDetailsCommand(Package package)
            : base(package, PackageGuids.guidCommandSet, PackageIds.CodeWebResourceCompareWithDetailsCommandId, ActionExecute, ActionBeforeQueryStatus) { }

        public static CodeWebResourceCompareWithDetailsCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new CodeWebResourceCompareWithDetailsCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            List<SelectedFile> selectedFiles = helper.GetOpenedFileInCodeWindow(FileOperations.SupportsWebResourceType);

            helper.HandleFileCompareCommand(null, selectedFiles, true);
        }

        private static void ActionBeforeQueryStatus(IServiceProviderOwner command, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusActiveDocumentWebResourceText(command, menuCommand);

            CommonHandlers.CorrectCommandNameForConnectionName(command, menuCommand, Properties.CommandNames.CodeWebResourceCompareWithDetailsCommand);
        }
    }
}
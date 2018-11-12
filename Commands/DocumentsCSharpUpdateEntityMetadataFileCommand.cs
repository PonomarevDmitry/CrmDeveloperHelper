using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System.Collections.Generic;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class DocumentsCSharpUpdateEntityMetadataFileCommand : AbstractCommand
    {
        private DocumentsCSharpUpdateEntityMetadataFileCommand(Package package)
            : base(package, PackageGuids.guidCommandSet, PackageIds.DocumentsCSharpUpdateEntityMetadataFileCommandId, ActionExecute, ActionBeforeQueryStatus) { }

        public static DocumentsCSharpUpdateEntityMetadataFileCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new DocumentsCSharpUpdateEntityMetadataFileCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            List<SelectedFile> selectedFiles = helper.GetOpenedDocuments(FileOperations.SupportsCSharpType);

            helper.HandleUpdateEntityMetadataFile(selectedFiles, false);
        }

        private static void ActionBeforeQueryStatus(IServiceProviderOwner command, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusOpenedDocumentsCSharp(command, menuCommand);

            CommonHandlers.CorrectCommandNameForConnectionName(command, menuCommand, Properties.CommandNames.DocumentsCSharpUpdateEntityMetadataFileCommand);
        }
    }
}
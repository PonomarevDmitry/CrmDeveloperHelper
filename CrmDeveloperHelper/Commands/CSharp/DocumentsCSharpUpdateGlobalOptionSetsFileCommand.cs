using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System.Collections.Generic;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.CSharp
{
    internal sealed class DocumentsCSharpUpdateGlobalOptionSetsFileCommand : AbstractDynamicCommandByConnectionAll
    {
        private DocumentsCSharpUpdateGlobalOptionSetsFileCommand(OleMenuCommandService commandService)
            : base(
                commandService
                , PackageIds.guidDynamicCommandSet.DocumentsCSharpUpdateGlobalOptionSetsFileCommandId
            )
        {

        }

        public static DocumentsCSharpUpdateGlobalOptionSetsFileCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new DocumentsCSharpUpdateGlobalOptionSetsFileCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            List<SelectedFile> selectedFiles = helper.GetOpenedDocuments(FileOperations.SupportsCSharpType).ToList();

            helper.HandleCSharpGlobalOptionSetsFileUpdateSchema(connectionData, selectedFiles, false);
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, ConnectionData connectionData, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusOpenedDocumentsCSharp(applicationObject, menuCommand);
        }
    }
}
using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Xmls
{
    internal sealed class CodeXmlExecuteFetchXmlRequestInConnectionsCommand : AbstractDynamicCommandByConnectionAll
    {
        private CodeXmlExecuteFetchXmlRequestInConnectionsCommand(OleMenuCommandService commandService)
            : base(
                commandService
                , PackageIds.CodeXmlExecuteFetchXmlRequestInConnectionsCommandId
            )
        {

        }

        public static CodeXmlExecuteFetchXmlRequestInConnectionsCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new CodeXmlExecuteFetchXmlRequestInConnectionsCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            var selectedFile = helper.GetOpenedFileInCodeWindow(FileOperations.SupportsXmlType).FirstOrDefault();

            if (selectedFile == null)
            {
                return;
            }

            if (helper.ApplicationObject.ActiveWindow != null
               && helper.ApplicationObject.ActiveWindow.Type == EnvDTE.vsWindowType.vsWindowTypeDocument
               && helper.ApplicationObject.ActiveWindow.Document != null
            )
            {
                if (!helper.ApplicationObject.ActiveWindow.Document.Saved)
                {
                    helper.ApplicationObject.ActiveWindow.Document.Save();
                }
            }

            helper.HandleExecutingFetchXml(connectionData, selectedFile, true);
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, ConnectionData connectionData, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusActiveDocumentIsXmlWithRoot(applicationObject, menuCommand, out _, CommonExportXsdSchemasCommand.RootFetch);
        }
    }
}
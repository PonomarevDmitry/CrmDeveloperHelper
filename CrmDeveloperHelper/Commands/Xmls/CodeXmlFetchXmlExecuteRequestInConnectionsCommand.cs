using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Xmls
{
    internal sealed class CodeXmlFetchXmlExecuteRequestInConnectionsCommand : AbstractDynamicCommandByConnectionAll
    {
        private CodeXmlFetchXmlExecuteRequestInConnectionsCommand(OleMenuCommandService commandService)
            : base(
                commandService
                , PackageIds.guidDynamicCommandSet.CodeXmlFetchXmlExecuteRequestInConnectionsCommandId
            )
        {

        }

        public static CodeXmlFetchXmlExecuteRequestInConnectionsCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new CodeXmlFetchXmlExecuteRequestInConnectionsCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            var selectedFile = helper.GetOpenedFileInCodeWindow(FileOperations.SupportsXmlType).FirstOrDefault();

            if (selectedFile == null)
            {
                return;
            }

            helper.HandleFetchXmlExecuting(connectionData, selectedFile, true);
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, ConnectionData connectionData, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusActiveDocumentIsXmlWithRoot(applicationObject, menuCommand, out _, AbstractDynamicCommandXsdSchemas.RootFetch);
        }
    }
}
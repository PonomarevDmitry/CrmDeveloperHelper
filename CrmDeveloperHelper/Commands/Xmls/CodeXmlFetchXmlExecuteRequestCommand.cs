using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Xmls
{
    internal sealed class CodeXmlFetchXmlExecuteRequestCommand : AbstractCommand
    {
        private CodeXmlFetchXmlExecuteRequestCommand(OleMenuCommandService commandService)
            : base(commandService, PackageIds.guidCommandSet.CodeXmlFetchXmlExecuteRequestCommandId) { }

        public static CodeXmlFetchXmlExecuteRequestCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new CodeXmlFetchXmlExecuteRequestCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper)
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

            helper.HandleExecutingFetchXml(null, selectedFile, false);
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusActiveDocumentIsXmlWithRoot(applicationObject, menuCommand, out _, AbstractDynamicCommandXsdSchemas.RootFetch);

            CommonHandlers.CorrectCommandNameForConnectionName(applicationObject, menuCommand, Properties.CommandNames.CodeXmlExecuteFetchXmlRequestCommand);
        }
    }
}
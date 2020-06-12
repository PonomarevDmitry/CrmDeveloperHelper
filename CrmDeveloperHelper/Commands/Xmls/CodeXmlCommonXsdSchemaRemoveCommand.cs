using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Xmls
{
    internal sealed class CodeXmlCommonXsdSchemaRemoveCommand : AbstractSingleCommand
    {
        private CodeXmlCommonXsdSchemaRemoveCommand(OleMenuCommandService commandService)
            : base(commandService, PackageIds.guidCommandSet.CodeXmlCommonXsdSchemaRemoveCommandId) { }

        public static CodeXmlCommonXsdSchemaRemoveCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new CodeXmlCommonXsdSchemaRemoveCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper)
        {
            EnvDTE.Document document = helper.GetOpenedDocumentInCodeWindow(FileOperations.SupportsXmlType);

            if (document != null)
            {
                ContentComparerHelper.RemoveXsdSchemaInDocument(document);
            }
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusActiveDocumentXml(applicationObject, menuCommand);
        }
    }
}
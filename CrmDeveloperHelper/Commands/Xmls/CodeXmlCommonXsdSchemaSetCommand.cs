using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using System;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Xmls
{
    internal sealed class CodeXmlCommonXsdSchemaSetCommand : AbstractDynamicCommandXsdSchemas
    {
        private CodeXmlCommonXsdSchemaSetCommand(OleMenuCommandService commandService)
            : base(commandService, PackageIds.guidDynamicCommandSet.CodeXmlCommonXsdSchemaSetCommandId)
        {

        }

        public static CodeXmlCommonXsdSchemaSetCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new CodeXmlCommonXsdSchemaSetCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper, Tuple<string, string[]> schemas)
        {
            EnvDTE.Document document = helper.GetOpenedDocumentInCodeWindow(FileOperations.SupportsXmlType);

            if (document != null)
            {
                ContentComparerHelper.ReplaceXsdSchemaInDocument(document, schemas.Item2);
            }
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, Tuple<string, string[]> schemas, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusActiveDocumentXml(applicationObject, menuCommand);
        }
    }
}
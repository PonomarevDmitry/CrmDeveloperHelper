using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Xmls
{
    internal sealed class CodeXmlRibbonDiffInsertIntellisenseContextCommand : AbstractCommand
    {
        private CodeXmlRibbonDiffInsertIntellisenseContextCommand(OleMenuCommandService commandService)
            : base(commandService, PackageIds.guidCommandSet.CodeXmlRibbonDiffInsertIntellisenseContextCommandId) { }

        public static CodeXmlRibbonDiffInsertIntellisenseContextCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new CodeXmlRibbonDiffInsertIntellisenseContextCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper)
        {
            EnvDTE.Document document = helper.GetOpenedDocumentInCodeWindow(FileOperations.SupportsXmlType);

            if (document != null)
            {
                ContentCoparerHelper.InsertIntellisenseContextEntityNameInDocument(document, string.Empty);
            }
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusActiveDocumentIsXmlWithRoot(applicationObject, menuCommand, out _, AbstractDynamicCommandXsdSchemas.RootRibbonDiffXml, AbstractDynamicCommandXsdSchemas.RootRibbonDefinitions);
        }
    }
}

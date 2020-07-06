using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Xmls
{
    internal sealed class CodeXmlRibbonDiffRemoveIntellisenseContextCommand : AbstractSingleCommand
    {
        private CodeXmlRibbonDiffRemoveIntellisenseContextCommand(OleMenuCommandService commandService)
            : base(commandService, PackageIds.guidCommandSet.CodeXmlRibbonDiffRemoveIntellisenseContextCommandId) { }

        public static CodeXmlRibbonDiffRemoveIntellisenseContextCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new CodeXmlRibbonDiffRemoveIntellisenseContextCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper)
        {
            EnvDTE.Document document = helper.GetOpenedDocumentInCodeWindow(FileOperations.SupportsXmlType);

            if (document != null)
            {
                ContentComparerHelper.RemoveIntellisenseContextEntityNameInDocument(document);
            }
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusActiveDocumentIsXmlWithRoot(applicationObject, menuCommand, out _, AbstractDynamicCommandXsdSchemas.RibbonDiffXmlRoot, AbstractDynamicCommandXsdSchemas.RibbonXmlRoot);
        }
    }
}

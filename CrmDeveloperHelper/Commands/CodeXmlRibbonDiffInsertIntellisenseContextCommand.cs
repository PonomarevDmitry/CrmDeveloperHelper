using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class CodeXmlRibbonDiffInsertIntellisenseContextCommand : AbstractCommand
    {
        private CodeXmlRibbonDiffInsertIntellisenseContextCommand(Package package)
            : base(package, PackageGuids.guidCommandSet, PackageIds.CodeXmlRibbonDiffInsertIntellisenseContextCommandId, ActionExecute, ActionBeforeQueryStatus) { }

        public static CodeXmlRibbonDiffInsertIntellisenseContextCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new CodeXmlRibbonDiffInsertIntellisenseContextCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            EnvDTE.Document document = helper.GetOpenedDocumentInCodeWindow(FileOperations.SupportsXmlType);

            if (document != null)
            {
                ContentCoparerHelper.InsertIntellisenseContextEntityNameInDocument(document, string.Empty);
            }
        }

        private static void ActionBeforeQueryStatus(IServiceProviderOwner command, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusActiveDocumentIsXmlWithRoot(command, menuCommand, out _, "RibbonDiffXml", "RibbonDefinitions");
        }
    }
}

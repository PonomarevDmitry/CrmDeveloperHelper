using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class CodeXmlRibbonDiffRemoveIntellisenseContextCommand : AbstractCommand
    {
        private CodeXmlRibbonDiffRemoveIntellisenseContextCommand(Package package)
            : base(package, PackageGuids.guidCommandSet, PackageIds.CodeXmlRibbonDiffRemoveIntellisenseContextCommandId, ActionExecute, ActionBeforeQueryStatus) { }

        public static CodeXmlRibbonDiffRemoveIntellisenseContextCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new CodeXmlRibbonDiffRemoveIntellisenseContextCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            EnvDTE.Document document = helper.GetOpenedDocumentInCodeWindow(FileOperations.SupportsXmlType);

            if (document != null)
            {
                ContentCoparerHelper.RemoveIntellisenseContextEntityNameInDocument(document);
            }
        }

        private static void ActionBeforeQueryStatus(IServiceProviderOwner command, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusActiveDocumentIsXmlWithRoot(command, menuCommand, out _, "RibbonDiffXml", "RibbonDefinitions");
        }
    }
}

using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class CodeXmlRemoveXsdSchemaCommand : AbstractCommand
    {
        private CodeXmlRemoveXsdSchemaCommand(Package package)
            : base(package, PackageGuids.guidCommandSet, PackageIds.CodeXmlRemoveXsdSchemaCommandId, ActionExecute, CommonHandlers.ActionBeforeQueryStatusActiveDocumentXml) { }

        public static CodeXmlRemoveXsdSchemaCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new CodeXmlRemoveXsdSchemaCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            EnvDTE.Document document = helper.GetOpenedDocumentInCodeWindow(FileOperations.SupportsXmlType);

            if (document != null)
            {
                ContentCoparerHelper.RemoveXsdSchemaInDocument(document);
            }
        }
    }
}
using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class CodeXmlOpenXsdSchemaFolderCommand : AbstractCommand
    {
        private CodeXmlOpenXsdSchemaFolderCommand(Package package)
           : base(package, PackageGuids.guidCommandSet, PackageIds.CodeXmlOpenXsdSchemaFolderCommandId, ActionExecute, CommonHandlers.ActionBeforeQueryStatusActiveDocumentXml) { }
         
        public static CodeXmlOpenXsdSchemaFolderCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new CodeXmlOpenXsdSchemaFolderCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            helper.HandleOpenXsdSchemaFolder();
        }
    }
}

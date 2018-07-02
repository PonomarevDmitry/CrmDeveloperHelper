using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    class CodeXmlConvertFetchXmlToJavaScriptCodeCommand : AbstractCommand
    {
        private CodeXmlConvertFetchXmlToJavaScriptCodeCommand(Package package)
            : base(package, PackageGuids.guidCommandSet, PackageIds.CodeXmlConvertFetchXmlToJavaScriptCodeCommandId, ActionExecute, CommonHandlers.ActionBeforeQueryStatusActiveDocumentXml) { }

        public static CodeXmlConvertFetchXmlToJavaScriptCodeCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new CodeXmlConvertFetchXmlToJavaScriptCodeCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
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

            helper.HandleConvertingFetchXmlToJavaScriptCode(null, selectedFile);
        }
    }
}

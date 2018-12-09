using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class CodeXmlExecuteFetchXmlRequestCommand : AbstractCommand
    {
        private CodeXmlExecuteFetchXmlRequestCommand(Package package)
            : base(package, PackageGuids.guidCommandSet, PackageIds.CodeXmlExecuteFetchXmlRequestCommandId, ActionExecute, ActionBeforeQueryStatus) { }

        public static CodeXmlExecuteFetchXmlRequestCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new CodeXmlExecuteFetchXmlRequestCommand(package);
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

            helper.HandleExecutingFetchXml(null, selectedFile);
        }

        private static void ActionBeforeQueryStatus(IServiceProviderOwner command, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusActiveDocumentIsXmlWithRoot(command, menuCommand, out _, "fetch");

            CommonHandlers.CorrectCommandNameForConnectionName(command, menuCommand, Properties.CommandNames.CodeXmlExecuteFetchXmlRequestCommand);
        }
    }
}
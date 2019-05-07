using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class CodeJavaScriptMinifyCommand : AbstractCommand
    {
        private CodeJavaScriptMinifyCommand(Package package)
            : base(package, PackageGuids.guidCommandSet, PackageIds.CodeJavaScriptMinifyCommandId, ActionExecute, CommonHandlers.ActionBeforeQueryStatusActiveDocumentSupportsMinification) { }

        public static CodeJavaScriptMinifyCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new CodeJavaScriptMinifyCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            var document = helper.GetOpenedDocumentInCodeWindow(FileOperations.SupportsMinification);

            helper.MinifyDocuments(new[] { document });
        }
    }
}
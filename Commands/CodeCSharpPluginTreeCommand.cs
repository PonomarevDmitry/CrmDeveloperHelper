using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using System;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class CodeCSharpPluginTreeCommand : AbstractCommand
    {
        private CodeCSharpPluginTreeCommand(Package package)
            : base(package, PackageGuids.guidCommandSet, PackageIds.CodeCSharpPluginTreeCommandId, ActionExecute, CommonHandlers.ActionBeforeQueryStatusActiveDocumentCSharp) { }

        public static CodeCSharpPluginTreeCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new CodeCSharpPluginTreeCommand(package);
        }

        private static async void ActionExecute(DTEHelper helper)
        {
            try
            {
                var document = helper.GetOpenedDocumentInCodeWindow(FileOperations.SupportsCSharpType);

                string fileType = await PropertiesHelper.GetTypeFullNameAsync(document);

                helper.HandleOpenPluginTree(string.Empty, fileType, string.Empty);
            }
            catch (Exception ex)
            {
                DTEHelper.WriteExceptionToOutput(ex);
            }
        }
    }
}

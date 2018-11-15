using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using System;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class CodeCSharpPluginTypeDescriptionCommand : AbstractCommand
    {
        private CodeCSharpPluginTypeDescriptionCommand(Package package)
            : base(package, PackageGuids.guidCommandSet, PackageIds.CodeCSharpPluginTypeDescriptionCommandId, ActionExecute, CommonHandlers.ActionBeforeQueryStatusActiveDocumentCSharp) { }

        public static CodeCSharpPluginTypeDescriptionCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new CodeCSharpPluginTypeDescriptionCommand(package);
        }

        private static async void ActionExecute(DTEHelper helper)
        {
            try
            {
                var document = helper.GetOpenedDocumentInCodeWindow(FileOperations.SupportsCSharpType);

                string fileType = await PropertiesHelper.GetTypeFullNameAsync(document);

                helper.HandleExportPluginTypeDescription(fileType);
            }
            catch (Exception ex)
            {
                DTEHelper.WriteExceptionToOutput(ex);
            }
        }
    }
}
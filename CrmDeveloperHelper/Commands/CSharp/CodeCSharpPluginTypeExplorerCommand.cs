using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using System;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.CSharp
{
    internal sealed class CodeCSharpPluginTypeExplorerCommand : AbstractCommand
    {
        private CodeCSharpPluginTypeExplorerCommand(Package package)
            : base(package, PackageGuids.guidCommandSet, PackageIds.CodeCSharpPluginTypeExplorerCommandId, ActionExecute, CommonHandlers.ActionBeforeQueryStatusActiveDocumentCSharp) { }

        public static CodeCSharpPluginTypeExplorerCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new CodeCSharpPluginTypeExplorerCommand(package);
        }

        private static async void ActionExecute(DTEHelper helper)
        {
            try
            {
                var document = helper.GetOpenedDocumentInCodeWindow(FileOperations.SupportsCSharpType);

                helper.WriteToOutput(null, Properties.OutputStrings.GettingClassFullNameFromFileFormat1, document?.FullName);
                helper.ActivateOutputWindow(null);
                string fileType = await PropertiesHelper.GetTypeFullNameAsync(document);

                helper.HandleOpenPluginTypeExplorer(fileType);
            }
            catch (Exception ex)
            {
                DTEHelper.WriteExceptionToOutput(null, ex);
            }
        }
    }
}
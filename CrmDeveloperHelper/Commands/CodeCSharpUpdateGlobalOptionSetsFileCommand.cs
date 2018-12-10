using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System.Collections.Generic;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands
{
    internal sealed class CodeCSharpUpdateGlobalOptionSetsFileCommand : AbstractCommand
    {
        private CodeCSharpUpdateGlobalOptionSetsFileCommand(Package package)
          : base(package, PackageGuids.guidCommandSet, PackageIds.CodeCSharpUpdateGlobalOptionSetsFileCommandId, ActionExecute, ActionBeforeQueryStatus) { }

        public static CodeCSharpUpdateGlobalOptionSetsFileCommand Instance { get; private set; }

        public static void Initialize(Package package)
        {
            Instance = new CodeCSharpUpdateGlobalOptionSetsFileCommand(package);
        }

        private static void ActionExecute(DTEHelper helper)
        {
            List<SelectedFile> selectedFiles = helper.GetOpenedFileInCodeWindow(FileOperations.SupportsCSharpType);

            helper.HandleUpdateGlobalOptionSetsFile(selectedFiles, false);
        }

        private static void ActionBeforeQueryStatus(IServiceProviderOwner command, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusActiveDocumentCSharp(command, menuCommand);

            CommonHandlers.CorrectCommandNameForConnectionName(command, menuCommand, Properties.CommandNames.CodeCSharpUpdateGlobalOptionSetsFileCommand);
        }
    }
}
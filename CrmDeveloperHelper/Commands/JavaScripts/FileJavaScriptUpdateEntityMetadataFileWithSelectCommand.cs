using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System.Collections.Generic;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.JavaScripts
{
    internal sealed class FileJavaScriptUpdateEntityMetadataFileWithSelectCommand : AbstractCommand
    {
        private FileJavaScriptUpdateEntityMetadataFileWithSelectCommand(OleMenuCommandService commandService)
            : base(commandService, PackageIds.FileJavaScriptUpdateEntityMetadataFileWithSelectCommandId) { }

        public static FileJavaScriptUpdateEntityMetadataFileWithSelectCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new FileJavaScriptUpdateEntityMetadataFileWithSelectCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper)
        {
            List<SelectedFile> selectedFiles = helper.GetSelectedFilesInSolutionExplorer(FileOperations.SupportsJavaScriptType, false).ToList();

            helper.HandleUpdateEntityMetadataFileJavaScript(null, selectedFiles, true);
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusSolutionExplorerJavaScriptSingle(applicationObject, menuCommand);

            CommonHandlers.CorrectCommandNameForConnectionName(applicationObject, menuCommand, Properties.CommandNames.FileJavaScriptUpdateEntityMetadataFileWithSelectCommand);
        }
    }
}

using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System.Collections.Generic;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.JavaScripts
{
    internal sealed class FileJavaScriptIncludeReferencesToDependencyXmlInConnectionGroupCommand : AbstractDynamicCommandByConnectionByGroupWithoutCurrent
    {
        private FileJavaScriptIncludeReferencesToDependencyXmlInConnectionGroupCommand(OleMenuCommandService commandService)
            : base(commandService, PackageIds.guidDynamicCommandSet.FileJavaScriptIncludeReferencesToDependencyXmlInConnectionGroupCommandId)
        {
        }

        public static FileJavaScriptIncludeReferencesToDependencyXmlInConnectionGroupCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new FileJavaScriptIncludeReferencesToDependencyXmlInConnectionGroupCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            List<SelectedFile> selectedFiles = helper.GetSelectedFilesInSolutionExplorer(FileOperations.SupportsJavaScriptType, false).ToList();

            helper.HandleIncludeReferencesToDependencyXmlCommand(null, selectedFiles);
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, ConnectionData connectionData, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusSolutionExplorerJavaScriptAny(applicationObject, menuCommand);
        }
    }
}
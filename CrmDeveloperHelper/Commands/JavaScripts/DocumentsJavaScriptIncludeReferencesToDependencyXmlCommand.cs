using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System.Collections.Generic;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.JavaScripts
{
    internal sealed class DocumentsJavaScriptIncludeReferencesToDependencyXmlCommand : AbstractSingleCommand
    {
        private DocumentsJavaScriptIncludeReferencesToDependencyXmlCommand(OleMenuCommandService commandService)
            : base(commandService, PackageIds.guidCommandSet.DocumentsJavaScriptIncludeReferencesToDependencyXmlCommandId)
        {
        }

        public static DocumentsJavaScriptIncludeReferencesToDependencyXmlCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new DocumentsJavaScriptIncludeReferencesToDependencyXmlCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper)
        {
            List<SelectedFile> selectedFiles = helper.GetOpenedDocuments(FileOperations.SupportsJavaScriptType).ToList();

            helper.HandleIncludeReferencesToDependencyXmlCommand(null, selectedFiles);
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusOpenedDocumentsJavaScript(applicationObject, menuCommand);

            CommonHandlers.CorrectCommandNameForConnectionName(applicationObject, menuCommand, Properties.CommandNames.DocumentsJavaScriptIncludeReferencesToDependencyXmlCommand);
        }
    }
}
using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System.Collections.Generic;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.WebResources
{
    /// <summary>
    /// Command handler
    /// </summary>
    internal sealed class CodeWebResourceCompareWithDetailsCommand : AbstractCommand
    {
        private CodeWebResourceCompareWithDetailsCommand(OleMenuCommandService commandService)
            : base(commandService, PackageIds.CodeWebResourceCompareWithDetailsCommandId) { }

        public static CodeWebResourceCompareWithDetailsCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new CodeWebResourceCompareWithDetailsCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper)
        {
            List<SelectedFile> selectedFiles = helper.GetOpenedFileInCodeWindow(FileOperations.SupportsWebResourceType).ToList();

            helper.HandleFileCompareCommand(null, selectedFiles, true);
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusActiveDocumentWebResourceText(applicationObject, menuCommand);

            CommonHandlers.CorrectCommandNameForConnectionName(applicationObject, menuCommand, Properties.CommandNames.CodeWebResourceCompareWithDetailsCommand);
        }
    }
}
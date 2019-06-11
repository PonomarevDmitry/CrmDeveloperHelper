using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System.Collections.Generic;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.CSharp
{
    internal sealed class FileCSharpUpdateEntityMetadataFileSchemaWithSelectCommand : AbstractCommand
    {
        private FileCSharpUpdateEntityMetadataFileSchemaWithSelectCommand(OleMenuCommandService commandService)
            : base(commandService, PackageIds.FileCSharpUpdateEntityMetadataFileSchemaWithSelectCommandId) { }

        public static FileCSharpUpdateEntityMetadataFileSchemaWithSelectCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new FileCSharpUpdateEntityMetadataFileSchemaWithSelectCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper)
        {
            List<SelectedFile> selectedFiles = helper.GetSelectedFilesInSolutionExplorer(FileOperations.SupportsCSharpType, false).ToList();

            helper.HandleUpdateEntityMetadataFileCSharpSchema(null, selectedFiles, true);
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusSolutionExplorerCSharpSingle(applicationObject, menuCommand);

            CommonHandlers.CorrectCommandNameForConnectionName(applicationObject, menuCommand, Properties.CommandNames.FileCSharpUpdateEntityMetadataFileSchemaWithSelectCommand);
        }
    }
}

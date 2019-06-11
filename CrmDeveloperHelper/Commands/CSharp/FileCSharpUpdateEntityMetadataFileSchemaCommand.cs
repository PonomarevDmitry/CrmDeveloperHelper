using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System.Collections.Generic;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.CSharp
{
    internal sealed class FileCSharpUpdateEntityMetadataFileSchemaCommand : AbstractCommandByConnectionAll
    {
        private FileCSharpUpdateEntityMetadataFileSchemaCommand(OleMenuCommandService commandService)
            : base(
                commandService
                , PackageIds.FileCSharpUpdateEntityMetadataFileSchemaCommandId
            )
        {

        }

        public static FileCSharpUpdateEntityMetadataFileSchemaCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new FileCSharpUpdateEntityMetadataFileSchemaCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            List<SelectedFile> selectedFiles = helper.GetSelectedFilesInSolutionExplorer(FileOperations.SupportsCSharpType, false).ToList();

            helper.HandleUpdateEntityMetadataFileCSharpSchema(connectionData, selectedFiles, false);
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, ConnectionData connectionData, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusSolutionExplorerCSharpAny(applicationObject, menuCommand);
        }
    }
}
using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using System.Collections.Generic;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.CSharp
{
    internal sealed class FileCSharpUpdateEntityMetadataFileProxyClassOrSchemaCommand : AbstractDynamicCommandByConnectionAll
    {
        private FileCSharpUpdateEntityMetadataFileProxyClassOrSchemaCommand(OleMenuCommandService commandService)
            : base(
                commandService
                , PackageIds.guidDynamicCommandSet.FileCSharpUpdateEntityMetadataFileProxyClassOrSchemaCommandId
            )
        {

        }

        public static FileCSharpUpdateEntityMetadataFileProxyClassOrSchemaCommand Instance { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            Instance = new FileCSharpUpdateEntityMetadataFileProxyClassOrSchemaCommand(commandService);
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            List<SelectedFile> selectedFiles = helper.GetSelectedFilesInSolutionExplorer(FileOperations.SupportsCSharpType, false).ToList();

            helper.HandleCSharpEntityMetadataFileUpdateProxyClassOrSchema(connectionData, selectedFiles, false);
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, ConnectionData connectionData, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusSolutionExplorerCSharpAny(applicationObject, menuCommand);
        }
    }
}
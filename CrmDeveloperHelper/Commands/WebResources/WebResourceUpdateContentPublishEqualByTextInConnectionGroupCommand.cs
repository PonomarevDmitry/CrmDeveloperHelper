using EnvDTE80;
using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model.SourcesSelectedFiles;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.WebResources
{
    internal sealed class WebResourceUpdateContentPublishEqualByTextInConnectionGroupCommand : AbstractDynamicCommandByConnectionByGroupWithCurrent
    {
        private readonly ISourceSelectedFiles _sourceSelectedFiles;

        private WebResourceUpdateContentPublishEqualByTextInConnectionGroupCommand(OleMenuCommandService commandService, int baseIdStart, ISourceSelectedFiles sourceSelectedFiles)
            : base(commandService, baseIdStart)
        {
            this._sourceSelectedFiles = sourceSelectedFiles;
        }

        public static WebResourceUpdateContentPublishEqualByTextInConnectionGroupCommand InstanceDocuments { get; private set; }

        public static WebResourceUpdateContentPublishEqualByTextInConnectionGroupCommand InstanceFile { get; private set; }

        public static WebResourceUpdateContentPublishEqualByTextInConnectionGroupCommand InstanceFolder { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            var sourceDocuments = DocumentsSourceSelectedFiles.CreateSource();

            var sourceFile = FileSourceSelectedFiles.CreateSource();

            var sourceFolder = FolderSourceSelectedFiles.CreateSource();

            InstanceDocuments = new WebResourceUpdateContentPublishEqualByTextInConnectionGroupCommand(commandService, PackageIds.guidDynamicCommandSet.DocumentsWebResourceUpdateContentPublishEqualByTextInConnectionGroupCommandId, sourceDocuments);

            InstanceFile = new WebResourceUpdateContentPublishEqualByTextInConnectionGroupCommand(commandService, PackageIds.guidDynamicCommandSet.FileWebResourceUpdateContentPublishEqualByTextInConnectionGroupCommandId, sourceFile);

            InstanceFolder = new WebResourceUpdateContentPublishEqualByTextInConnectionGroupCommand(commandService, PackageIds.guidDynamicCommandSet.FolderWebResourceUpdateContentPublishEqualByTextInConnectionGroupCommandId, sourceFolder);
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            if (connectionData.IsReadOnly)
            {
                helper.WriteToOutput(connectionData, Properties.OutputStrings.ConnectionIsReadOnlyFormat1, connectionData.Name);
                return;
            }

            var selectedFiles = _sourceSelectedFiles.GetSelectedFiles(helper, SelectedFileType.WebResourceText).ToList();

            helper.HandleUpdateContentWebResourcesAndPublishEqualByTextCommand(connectionData, selectedFiles);
        }

        protected override void CommandBeforeQueryStatus(DTE2 applicationObject, ConnectionData connectionData, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusConnectionIsNotReadOnly(applicationObject, menuCommand);

            _sourceSelectedFiles.CommandBeforeQueryStatus(applicationObject, menuCommand, SelectedFileType.WebResourceText);
        }
    }
}
using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model.SourcesSelectedFiles;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.WebResources
{
    internal sealed class WebResourceUpdateContentPublishInConnectionGroupWithoutCurrentCommand : AbstractDynamicCommandByConnectionByGroupWithoutCurrent
    {
        private readonly ISourceSelectedFiles _sourceSelectedFiles;

        private WebResourceUpdateContentPublishInConnectionGroupWithoutCurrentCommand(OleMenuCommandService commandService, int baseIdStart, ISourceSelectedFiles sourceSelectedFiles)
            : base(commandService, baseIdStart)
        {
            this._sourceSelectedFiles = sourceSelectedFiles;
        }

        public static WebResourceUpdateContentPublishInConnectionGroupWithoutCurrentCommand InstanceCode { get; private set; }

        public static WebResourceUpdateContentPublishInConnectionGroupWithoutCurrentCommand InstanceFile { get; private set; }

        public static WebResourceUpdateContentPublishInConnectionGroupWithoutCurrentCommand InstanceFolder { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            var sourceCode = CodeSourceSelectedFiles.CreateSource();

            var sourceFile = FileSourceSelectedFiles.CreateSource();

            var sourceFolder = FolderSourceSelectedFiles.CreateSource();

            InstanceCode = new WebResourceUpdateContentPublishInConnectionGroupWithoutCurrentCommand(commandService, PackageIds.guidDynamicCommandSet.CodeWebResourceUpdateContentPublishInConnectionGroupCommandId, sourceCode);

            InstanceFile = new WebResourceUpdateContentPublishInConnectionGroupWithoutCurrentCommand(commandService, PackageIds.guidDynamicCommandSet.FileWebResourceUpdateContentPublishInConnectionGroupCommandId, sourceFile);

            InstanceFolder = new WebResourceUpdateContentPublishInConnectionGroupWithoutCurrentCommand(commandService, PackageIds.guidDynamicCommandSet.FolderWebResourceUpdateContentPublishInConnectionGroupCommandId, sourceFolder);
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            if (connectionData.IsReadOnly)
            {
                helper.WriteToOutput(connectionData, Properties.OutputStrings.ConnectionIsReadOnlyFormat1, connectionData.Name);
                return;
            }

            var selectedFiles = _sourceSelectedFiles.GetSelectedFiles(helper, SelectedFileType.WebResource).ToList();

            helper.HandleUpdateContentWebResourcesAndPublishCommand(connectionData, selectedFiles);
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, ConnectionData connectionData, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusConnectionIsNotReadOnly(applicationObject, menuCommand);

            _sourceSelectedFiles.CommandBeforeQueryStatus(applicationObject, menuCommand, SelectedFileType.WebResource);
        }
    }
}
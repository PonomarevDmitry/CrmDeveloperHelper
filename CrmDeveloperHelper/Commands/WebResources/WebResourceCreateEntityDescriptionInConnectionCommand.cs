using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model.Sources;
using System.Collections.Generic;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.WebResources
{
    internal sealed class WebResourceCreateEntityDescriptionInConnectionCommand : AbstractDynamicCommandByConnectionAll
    {
        private readonly ISourceSelectedFiles _sourceSelectedFiles;

        private WebResourceCreateEntityDescriptionInConnectionCommand(OleMenuCommandService commandService, int baseIdStart, ISourceSelectedFiles sourceSelectedFiles)
            : base(commandService, baseIdStart)
        {
            this._sourceSelectedFiles = sourceSelectedFiles;
        }

        public static WebResourceCreateEntityDescriptionInConnectionCommand InstanceCode { get; private set; }

        public static WebResourceCreateEntityDescriptionInConnectionCommand InstanceDocuments { get; private set; }

        public static WebResourceCreateEntityDescriptionInConnectionCommand InstanceFile { get; private set; }

        public static WebResourceCreateEntityDescriptionInConnectionCommand InstanceFolder { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            var sourceCode = CodeSourceSelectedFiles.CreateSource();

            var sourceDocuments = DocumentsSourceSelectedFiles.CreateSource();

            var sourceFile = FileSourceSelectedFiles.CreateSource();

            var sourceFolder = FolderSourceSelectedFiles.CreateSource();

            InstanceCode = new WebResourceCreateEntityDescriptionInConnectionCommand(
                commandService
                , PackageIds.guidDynamicCommandSet.CodeWebResourceCreateEntityDescriptionInConnectionCommandId
                , sourceCode
            );

            InstanceDocuments = new WebResourceCreateEntityDescriptionInConnectionCommand(
                commandService
                , PackageIds.guidDynamicCommandSet.DocumentsWebResourceCreateEntityDescriptionInConnectionCommandId
                , sourceDocuments
            );

            InstanceFile = new WebResourceCreateEntityDescriptionInConnectionCommand(
                commandService
                , PackageIds.guidDynamicCommandSet.FileWebResourceCreateEntityDescriptionInConnectionCommandId
                , sourceFile
            );

            InstanceFolder = new WebResourceCreateEntityDescriptionInConnectionCommand(
                commandService
                , PackageIds.guidDynamicCommandSet.FolderWebResourceCreateEntityDescriptionInConnectionCommandId
                , sourceFolder
            );
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            List<SelectedFile> selectedFiles = _sourceSelectedFiles.GetSelectedFiles(helper, SelectedFileType.WebResource).ToList();

            helper.HandleWebResourceCreateEntityDescriptionCommand(connectionData, selectedFiles);
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, ConnectionData connectionData, OleMenuCommand menuCommand)
        {
            _sourceSelectedFiles.CommandBeforeQueryStatus(applicationObject, menuCommand, SelectedFileType.WebResource);
        }
    }
}
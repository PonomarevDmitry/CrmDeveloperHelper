using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model.SourcesSelectedFiles;
using System.Collections.Generic;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.WebResources
{
    internal class WebResourceAddFilesIntoListForPublishCommand : AbstractDynamicCommandByOpenFilesType
    {
        private readonly ISourceSelectedFiles _sourceSelectedFiles;

        protected WebResourceAddFilesIntoListForPublishCommand(
            OleMenuCommandService commandService
            , int baseIdStart
            , IList<OpenFilesType> sourceOpenTypes
            , ISourceSelectedFiles sourceSelectedFiles
        ) : base(commandService, baseIdStart, sourceOpenTypes)
        {
            this._sourceSelectedFiles = sourceSelectedFiles;
        }

        public static WebResourceAddFilesIntoListForPublishCommand InstanceDocumentsOrdinal { get; private set; }

        public static WebResourceAddFilesIntoListForPublishCommand InstanceDocumentsChanges { get; private set; }

        public static WebResourceAddFilesIntoListForPublishCommand InstanceDocumentsMirror { get; private set; }

        public static WebResourceAddFilesIntoListForPublishCommand InstanceFileOrdinal { get; private set; }

        public static WebResourceAddFilesIntoListForPublishCommand InstanceFileChanges { get; private set; }

        public static WebResourceAddFilesIntoListForPublishCommand InstanceFileMirror { get; private set; }

        public static WebResourceAddFilesIntoListForPublishCommand InstanceFolderOrdinal { get; private set; }

        public static WebResourceAddFilesIntoListForPublishCommand InstanceFolderChanges { get; private set; }

        public static WebResourceAddFilesIntoListForPublishCommand InstanceFolderMirror { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            var sourceDocuments = new DocumentsWebResourceSourceSelectedFiles();

            var sourceFile = new FileWebResourceSourceSelectedFiles();

            var sourceFolder = new FolderWebResourceSourceSelectedFiles();

            InstanceDocumentsOrdinal = new WebResourceAddFilesIntoListForPublishCommand(commandService, PackageIds.guidDynamicCommandSet.DocumentsWebResourceAddFilesIntoListForPublishOrdinalCommandId, _typesOrdinal, sourceDocuments);
            InstanceDocumentsChanges = new WebResourceAddFilesIntoListForPublishCommand(commandService, PackageIds.guidDynamicCommandSet.DocumentsWebResourceAddFilesIntoListForPublishWithChangesCommandId, _typesChanges, sourceDocuments);
            InstanceDocumentsMirror = new WebResourceAddFilesIntoListForPublishCommand(commandService, PackageIds.guidDynamicCommandSet.DocumentsWebResourceAddFilesIntoListForPublishWithMirrorCommandId, _typesMirror, sourceDocuments);

            InstanceFileOrdinal = new WebResourceAddFilesIntoListForPublishCommand(commandService, PackageIds.guidDynamicCommandSet.FileWebResourceAddFilesIntoListForPublishOrdinalCommandId, _typesOrdinal, sourceFile);
            InstanceFileChanges = new WebResourceAddFilesIntoListForPublishCommand(commandService, PackageIds.guidDynamicCommandSet.FileWebResourceAddFilesIntoListForPublishWithChangesCommandId, _typesChanges, sourceFile);
            InstanceFileMirror = new WebResourceAddFilesIntoListForPublishCommand(commandService, PackageIds.guidDynamicCommandSet.FileWebResourceAddFilesIntoListForPublishWithMirrorCommandId, _typesMirror, sourceFile);

            InstanceFolderOrdinal = new WebResourceAddFilesIntoListForPublishCommand(commandService, PackageIds.guidDynamicCommandSet.FolderWebResourceAddFilesIntoListForPublishOrdinalCommandId, _typesOrdinal, sourceFolder);
            InstanceFolderChanges = new WebResourceAddFilesIntoListForPublishCommand(commandService, PackageIds.guidDynamicCommandSet.FolderWebResourceAddFilesIntoListForPublishWithChangesCommandId, _typesChanges, sourceFolder);
            InstanceFolderMirror = new WebResourceAddFilesIntoListForPublishCommand(commandService, PackageIds.guidDynamicCommandSet.FolderWebResourceAddFilesIntoListForPublishWithMirrorCommandId, _typesMirror, sourceFolder);
        }

        protected override void CommandAction(DTEHelper helper, OpenFilesType openFilesType)
        {
            var webResourceType = _comparersForOpenFilesType[openFilesType];

            IEnumerable<SelectedFile> selectedFiles = _sourceSelectedFiles.GetSelectedFiles(helper, webResourceType);

            if (selectedFiles.Any())
            {
                helper.HandleAddingIntoPublishListFilesByTypeCommand(selectedFiles, openFilesType);
            }
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, OpenFilesType openFilesType, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusConnectionIsNotReadOnly(applicationObject, menuCommand);

            var webResourceType = _comparersForOpenFilesType[openFilesType];

            _sourceSelectedFiles.CommandBeforeQueryStatus(applicationObject, menuCommand, webResourceType);
        }
    }
}
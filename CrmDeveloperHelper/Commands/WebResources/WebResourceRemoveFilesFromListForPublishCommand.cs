using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model.Sources;
using System.Collections.Generic;
using System.Linq;


namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.WebResources
{
    internal sealed class WebResourceRemoveFilesFromListForPublishCommand : AbstractDynamicCommandByOpenFilesType
    {
        private readonly ISourceSelectedFiles _sourceSelectedFiles;

        private WebResourceRemoveFilesFromListForPublishCommand(
            OleMenuCommandService commandService
            , int baseIdStart
            , IList<OpenFilesType> sourceOpenTypes
            , ISourceSelectedFiles sourceSelectedFiles
        ) : base(commandService, baseIdStart, sourceOpenTypes)
        {
            this._sourceSelectedFiles = sourceSelectedFiles;
        }

        public static WebResourceRemoveFilesFromListForPublishCommand InstanceDocumentsOrdinal { get; private set; }

        public static WebResourceRemoveFilesFromListForPublishCommand InstanceDocumentsChanges { get; private set; }

        public static WebResourceRemoveFilesFromListForPublishCommand InstanceDocumentsMirror { get; private set; }

        public static WebResourceRemoveFilesFromListForPublishCommand InstanceFileOrdinal { get; private set; }

        public static WebResourceRemoveFilesFromListForPublishCommand InstanceFileChanges { get; private set; }

        public static WebResourceRemoveFilesFromListForPublishCommand InstanceFileMirror { get; private set; }

        public static WebResourceRemoveFilesFromListForPublishCommand InstanceFolderOrdinal { get; private set; }

        public static WebResourceRemoveFilesFromListForPublishCommand InstanceFolderChanges { get; private set; }

        public static WebResourceRemoveFilesFromListForPublishCommand InstanceFolderMirror { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            var sourceDocuments = DocumentsSourceSelectedFiles.CreateSource();

            var sourceFile = FileSourceSelectedFiles.CreateSource();

            var sourceFolder = FolderSourceSelectedFiles.CreateSource();

            InstanceDocumentsOrdinal = new WebResourceRemoveFilesFromListForPublishCommand(commandService, PackageIds.guidDynamicOpenFilesTypeCommandSet.DocumentsWebResourceRemoveFilesFromListForPublishOrdinalCommandId, _typesOrdinal, sourceDocuments);
            InstanceDocumentsChanges = new WebResourceRemoveFilesFromListForPublishCommand(commandService, PackageIds.guidDynamicOpenFilesTypeCommandSet.DocumentsWebResourceRemoveFilesFromListForPublishWithChangesCommandId, _typesChanges, sourceDocuments);
            InstanceDocumentsMirror = new WebResourceRemoveFilesFromListForPublishCommand(commandService, PackageIds.guidDynamicOpenFilesTypeCommandSet.DocumentsWebResourceRemoveFilesFromListForPublishWithMirrorCommandId, _typesMirror, sourceDocuments);

            InstanceFileOrdinal = new WebResourceRemoveFilesFromListForPublishCommand(commandService, PackageIds.guidDynamicOpenFilesTypeCommandSet.FileWebResourceRemoveFilesFromListForPublishOrdinalCommandId, _typesOrdinal, sourceFile);
            InstanceFileChanges = new WebResourceRemoveFilesFromListForPublishCommand(commandService, PackageIds.guidDynamicOpenFilesTypeCommandSet.FileWebResourceRemoveFilesFromListForPublishWithChangesCommandId, _typesChanges, sourceFile);
            InstanceFileMirror = new WebResourceRemoveFilesFromListForPublishCommand(commandService, PackageIds.guidDynamicOpenFilesTypeCommandSet.FileWebResourceRemoveFilesFromListForPublishWithMirrorCommandId, _typesMirror, sourceFile);

            InstanceFolderOrdinal = new WebResourceRemoveFilesFromListForPublishCommand(commandService, PackageIds.guidDynamicOpenFilesTypeCommandSet.FolderWebResourceRemoveFilesFromListForPublishOrdinalCommandId, _typesOrdinal, sourceFolder);
            InstanceFolderChanges = new WebResourceRemoveFilesFromListForPublishCommand(commandService, PackageIds.guidDynamicOpenFilesTypeCommandSet.FolderWebResourceRemoveFilesFromListForPublishWithChangesCommandId, _typesChanges, sourceFolder);
            InstanceFolderMirror = new WebResourceRemoveFilesFromListForPublishCommand(commandService, PackageIds.guidDynamicOpenFilesTypeCommandSet.FolderWebResourceRemoveFilesFromListForPublishWithMirrorCommandId, _typesMirror, sourceFolder);
        }

        protected override void CommandAction(DTEHelper helper, OpenFilesType openFilesType)
        {
            var selectedFileType = _selectedFileTypeForOpenFilesType[openFilesType];

            IEnumerable<SelectedFile> selectedFiles = _sourceSelectedFiles.GetSelectedFiles(helper, selectedFileType);

            if (selectedFiles.Any())
            {
                helper.HandleRemovingFromPublishListFilesByTypeCommand(selectedFiles, openFilesType);
            }
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, OpenFilesType openFilesType, OleMenuCommand menuCommand)
        {
            CommonHandlers.ActionBeforeQueryStatusConnectionIsNotReadOnly(applicationObject, menuCommand);

            var selectedFileType = _selectedFileTypeForOpenFilesType[openFilesType];

            _sourceSelectedFiles.CommandBeforeQueryStatus(applicationObject, menuCommand, selectedFileType);
        }
    }
}
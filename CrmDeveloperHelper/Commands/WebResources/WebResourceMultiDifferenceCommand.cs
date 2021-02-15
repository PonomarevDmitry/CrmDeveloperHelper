using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model.Sources;
using System.Collections.Generic;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.WebResources
{
    internal class WebResourceMultiDifferenceCommand : AbstractDynamicCommandByOpenFilesType
    {
        private readonly ISourceSelectedFiles _sourceSelectedFiles;

        protected WebResourceMultiDifferenceCommand(
            OleMenuCommandService commandService
            , int baseIdStart
            , IList<OpenFilesType> sourceOpenTypes
            , ISourceSelectedFiles sourceSelectedFiles
        ) : base(commandService, baseIdStart, sourceOpenTypes)
        {
            this._sourceSelectedFiles = sourceSelectedFiles;
        }

        public static WebResourceMultiDifferenceCommand InstanceDocumentsExistsOrHasLink { get; private set; }

        public static WebResourceMultiDifferenceCommand InstanceDocumentsChanges { get; private set; }

        public static WebResourceMultiDifferenceCommand InstanceDocumentsMirror { get; private set; }

        public static WebResourceMultiDifferenceCommand InstanceFileExistsOrHasLink { get; private set; }

        public static WebResourceMultiDifferenceCommand InstanceFileChanges { get; private set; }

        public static WebResourceMultiDifferenceCommand InstanceFileMirror { get; private set; }

        public static WebResourceMultiDifferenceCommand InstanceFolderExistsOrHasLink { get; private set; }

        public static WebResourceMultiDifferenceCommand InstanceFolderChanges { get; private set; }

        public static WebResourceMultiDifferenceCommand InstanceFolderMirror { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            var sourceDocuments = DocumentsSourceSelectedFiles.CreateSource();

            var sourceFile = FileSourceSelectedFiles.CreateSource();

            var sourceFolder = FolderSourceSelectedFiles.CreateSource();

            InstanceDocumentsExistsOrHasLink = new WebResourceMultiDifferenceCommand(commandService, PackageIds.guidDynamicOpenFilesTypeCommandSet.DocumentsWebResourceMultiDifferenceFilesExistsOrHasLinkCommandId, _typesExistsOrHasLink, sourceDocuments);
            InstanceDocumentsChanges = new WebResourceMultiDifferenceCommand(commandService, PackageIds.guidDynamicOpenFilesTypeCommandSet.DocumentsWebResourceMultiDifferenceFilesWithChangesCommandId, _typesChanges, sourceDocuments);
            InstanceDocumentsMirror = new WebResourceMultiDifferenceCommand(commandService, PackageIds.guidDynamicOpenFilesTypeCommandSet.DocumentsWebResourceMultiDifferenceFilesWithMirrorCommandId, _typesMirror, sourceDocuments);

            InstanceFileExistsOrHasLink = new WebResourceMultiDifferenceCommand(commandService, PackageIds.guidDynamicOpenFilesTypeCommandSet.FileWebResourceMultiDifferenceFilesExistsOrHasLinkCommandId, _typesExistsOrHasLink, sourceFile);
            InstanceFileChanges = new WebResourceMultiDifferenceCommand(commandService, PackageIds.guidDynamicOpenFilesTypeCommandSet.FileWebResourceMultiDifferenceFilesWithChangesCommandId, _typesChanges, sourceFile);
            InstanceFileMirror = new WebResourceMultiDifferenceCommand(commandService, PackageIds.guidDynamicOpenFilesTypeCommandSet.FileWebResourceMultiDifferenceFilesWithMirrorCommandId, _typesMirror, sourceFile);

            InstanceFolderExistsOrHasLink = new WebResourceMultiDifferenceCommand(commandService, PackageIds.guidDynamicOpenFilesTypeCommandSet.FolderWebResourceMultiDifferenceFilesExistsOrHasLinkCommandId, _typesExistsOrHasLink, sourceFolder);
            InstanceFolderChanges = new WebResourceMultiDifferenceCommand(commandService, PackageIds.guidDynamicOpenFilesTypeCommandSet.FolderWebResourceMultiDifferenceFilesWithChangesCommandId, _typesChanges, sourceFolder);
            InstanceFolderMirror = new WebResourceMultiDifferenceCommand(commandService, PackageIds.guidDynamicOpenFilesTypeCommandSet.FolderWebResourceMultiDifferenceFilesWithMirrorCommandId, _typesMirror, sourceFolder);
        }

        protected override void CommandAction(DTEHelper helper, OpenFilesType openFilesType)
        {
            var selectedFileType = _selectedFileTypeForOpenFilesType[openFilesType];

            IEnumerable<SelectedFile> selectedFiles = _sourceSelectedFiles.GetSelectedFiles(helper, selectedFileType);

            if (selectedFiles.Any())
            {
                helper.HandleWebResourceMultiDifferenceFiles(selectedFiles, openFilesType);
            }
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, OpenFilesType openFilesType, OleMenuCommand menuCommand)
        {
            var selectedFileType = _selectedFileTypeForOpenFilesType[openFilesType];

            _sourceSelectedFiles.CommandBeforeQueryStatus(applicationObject, menuCommand, selectedFileType);
        }
    }
}
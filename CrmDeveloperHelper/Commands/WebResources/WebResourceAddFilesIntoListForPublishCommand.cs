﻿using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model.Sources;
using System.Collections.Generic;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.WebResources
{
    internal sealed class WebResourceAddFilesIntoListForPublishCommand : AbstractDynamicCommandByOpenFilesType
    {
        private readonly ISourceSelectedFiles _sourceSelectedFiles;

        private WebResourceAddFilesIntoListForPublishCommand(
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
            var sourceDocuments = DocumentsSourceSelectedFiles.CreateSource();

            var sourceFile = FileSourceSelectedFiles.CreateSource();

            var sourceFolder = FolderSourceSelectedFiles.CreateSource();

            InstanceDocumentsOrdinal = new WebResourceAddFilesIntoListForPublishCommand(commandService, PackageIds.guidDynamicOpenFilesTypeCommandSet.DocumentsWebResourceAddFilesIntoListForPublishOrdinalCommandId, _typesOrdinal, sourceDocuments);
            InstanceDocumentsChanges = new WebResourceAddFilesIntoListForPublishCommand(commandService, PackageIds.guidDynamicOpenFilesTypeCommandSet.DocumentsWebResourceAddFilesIntoListForPublishWithChangesCommandId, _typesChanges, sourceDocuments);
            InstanceDocumentsMirror = new WebResourceAddFilesIntoListForPublishCommand(commandService, PackageIds.guidDynamicOpenFilesTypeCommandSet.DocumentsWebResourceAddFilesIntoListForPublishWithMirrorCommandId, _typesMirror, sourceDocuments);

            InstanceFileOrdinal = new WebResourceAddFilesIntoListForPublishCommand(commandService, PackageIds.guidDynamicOpenFilesTypeCommandSet.FileWebResourceAddFilesIntoListForPublishOrdinalCommandId, _typesOrdinal, sourceFile);
            InstanceFileChanges = new WebResourceAddFilesIntoListForPublishCommand(commandService, PackageIds.guidDynamicOpenFilesTypeCommandSet.FileWebResourceAddFilesIntoListForPublishWithChangesCommandId, _typesChanges, sourceFile);
            InstanceFileMirror = new WebResourceAddFilesIntoListForPublishCommand(commandService, PackageIds.guidDynamicOpenFilesTypeCommandSet.FileWebResourceAddFilesIntoListForPublishWithMirrorCommandId, _typesMirror, sourceFile);

            InstanceFolderOrdinal = new WebResourceAddFilesIntoListForPublishCommand(commandService, PackageIds.guidDynamicOpenFilesTypeCommandSet.FolderWebResourceAddFilesIntoListForPublishOrdinalCommandId, _typesOrdinal, sourceFolder);
            InstanceFolderChanges = new WebResourceAddFilesIntoListForPublishCommand(commandService, PackageIds.guidDynamicOpenFilesTypeCommandSet.FolderWebResourceAddFilesIntoListForPublishWithChangesCommandId, _typesChanges, sourceFolder);
            InstanceFolderMirror = new WebResourceAddFilesIntoListForPublishCommand(commandService, PackageIds.guidDynamicOpenFilesTypeCommandSet.FolderWebResourceAddFilesIntoListForPublishWithMirrorCommandId, _typesMirror, sourceFolder);
        }

        protected override void CommandAction(DTEHelper helper, OpenFilesType openFilesType)
        {
            var selectedFileType = _selectedFileTypeForOpenFilesType[openFilesType];

            IEnumerable<SelectedFile> selectedFiles = _sourceSelectedFiles.GetSelectedFiles(helper, selectedFileType);

            if (selectedFiles.Any())
            {
                helper.HandleAddingIntoPublishListFilesByTypeCommand(selectedFiles, openFilesType);
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
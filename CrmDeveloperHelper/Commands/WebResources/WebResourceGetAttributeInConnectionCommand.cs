using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model.Sources;
using System.Collections.Generic;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.WebResources
{
    internal sealed class WebResourceGetAttributeInConnectionCommand : AbstractDynamicCommandByConnectionAll
    {
        private readonly ISourceSelectedFiles _sourceSelectedFiles;
        private readonly string _fieldName;
        private readonly string _fieldTitle;

        private WebResourceGetAttributeInConnectionCommand(OleMenuCommandService commandService, int baseIdStart, ISourceSelectedFiles sourceSelectedFiles, string fieldName, string fieldTitle)
            : base(commandService, baseIdStart)
        {
            this._sourceSelectedFiles = sourceSelectedFiles;
            this._fieldName = fieldName;
            this._fieldTitle = fieldTitle;
        }

        public static WebResourceGetAttributeInConnectionCommand InstanceCodeContentJson { get; private set; }

        public static WebResourceGetAttributeInConnectionCommand InstanceCodeDependencyXml { get; private set; }

        public static WebResourceGetAttributeInConnectionCommand InstanceDocumentsContentJson { get; private set; }

        public static WebResourceGetAttributeInConnectionCommand InstanceDocumentsDependencyXml { get; private set; }

        public static WebResourceGetAttributeInConnectionCommand InstanceFileContentJson { get; private set; }

        public static WebResourceGetAttributeInConnectionCommand InstanceFileDependencyXml { get; private set; }

        public static WebResourceGetAttributeInConnectionCommand InstanceFolderContentJson { get; private set; }

        public static WebResourceGetAttributeInConnectionCommand InstanceFolderDependencyXml { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            var sourceCode = CodeSourceSelectedFiles.CreateSource();

            var sourceDocuments = DocumentsSourceSelectedFiles.CreateSource();

            var sourceFile = FileSourceSelectedFiles.CreateSource();

            var sourceFolder = FolderSourceSelectedFiles.CreateSource();

            InstanceCodeContentJson = new WebResourceGetAttributeInConnectionCommand(
                commandService
                , PackageIds.guidDynamicCommandSet.CodeWebResourceGetAttributeContentJsonInConnectionCommandId
                , sourceCode
                , WebResource.Schema.Attributes.contentjson
                , WebResource.Schema.Headers.contentjson
            );

            InstanceCodeDependencyXml = new WebResourceGetAttributeInConnectionCommand(
               commandService
               , PackageIds.guidDynamicCommandSet.CodeWebResourceGetAttributeDependencyXmlInConnectionCommandId
               , sourceCode
               , WebResource.Schema.Attributes.dependencyxml
               , WebResource.Schema.Headers.dependencyxml
            );

            InstanceDocumentsContentJson = new WebResourceGetAttributeInConnectionCommand(
                commandService
                , PackageIds.guidDynamicCommandSet.DocumentsWebResourceGetAttributeContentJsonInConnectionCommandId
                , sourceDocuments
                , WebResource.Schema.Attributes.contentjson
                , WebResource.Schema.Headers.contentjson
            );

            InstanceDocumentsDependencyXml = new WebResourceGetAttributeInConnectionCommand(
               commandService
               , PackageIds.guidDynamicCommandSet.DocumentsWebResourceGetAttributeDependencyXmlInConnectionCommandId
               , sourceDocuments
               , WebResource.Schema.Attributes.dependencyxml
               , WebResource.Schema.Headers.dependencyxml
            );

            InstanceFileContentJson = new WebResourceGetAttributeInConnectionCommand(
                commandService
                , PackageIds.guidDynamicCommandSet.FileWebResourceGetAttributeContentJsonInConnectionCommandId
                , sourceFile
                , WebResource.Schema.Attributes.contentjson
                , WebResource.Schema.Headers.contentjson
            );

            InstanceFileDependencyXml = new WebResourceGetAttributeInConnectionCommand(
               commandService
               , PackageIds.guidDynamicCommandSet.FileWebResourceGetAttributeDependencyXmlInConnectionCommandId
               , sourceFile
               , WebResource.Schema.Attributes.dependencyxml
               , WebResource.Schema.Headers.dependencyxml
            );

            InstanceFolderContentJson = new WebResourceGetAttributeInConnectionCommand(
                commandService
                , PackageIds.guidDynamicCommandSet.FolderWebResourceGetAttributeContentJsonInConnectionCommandId
                , sourceFolder
                , WebResource.Schema.Attributes.contentjson
                , WebResource.Schema.Headers.contentjson
            );

            InstanceFolderDependencyXml = new WebResourceGetAttributeInConnectionCommand(
               commandService
               , PackageIds.guidDynamicCommandSet.FolderWebResourceGetAttributeDependencyXmlInConnectionCommandId
               , sourceFolder
               , WebResource.Schema.Attributes.dependencyxml
               , WebResource.Schema.Headers.dependencyxml
            );
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            List<SelectedFile> selectedFiles = _sourceSelectedFiles.GetSelectedFiles(helper, SelectedFileType.WebResource).Take(2).ToList();

            helper.HandleWebResourceGetAttributeCommand(connectionData, selectedFiles, this._fieldName, this._fieldTitle);
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, ConnectionData connectionData, OleMenuCommand menuCommand)
        {
            _sourceSelectedFiles.CommandBeforeQueryStatus(applicationObject, menuCommand, SelectedFileType.WebResource);
        }
    }
}
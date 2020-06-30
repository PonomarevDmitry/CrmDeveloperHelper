using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model.SourcesSelectedFiles;
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

        public static WebResourceGetAttributeInConnectionCommand InstanceContentJson { get; private set; }

        public static WebResourceGetAttributeInConnectionCommand InstanceDependencyXml { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            var sourceCode = CodeSourceSelectedFiles.CreateSource();

            var sourceFile = FileSourceSelectedFileSingle.CreateSource();

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

            InstanceContentJson = new WebResourceGetAttributeInConnectionCommand(
                commandService
                , PackageIds.guidDynamicCommandSet.FileWebResourceGetAttributeContentJsonInConnectionCommandId
                , sourceFile
                , WebResource.Schema.Attributes.contentjson
                , WebResource.Schema.Headers.contentjson
            );

            InstanceDependencyXml = new WebResourceGetAttributeInConnectionCommand(
               commandService
               , PackageIds.guidDynamicCommandSet.FileWebResourceGetAttributeDependencyXmlInConnectionCommandId
               , sourceFile
               , WebResource.Schema.Attributes.dependencyxml
               , WebResource.Schema.Headers.dependencyxml
            );
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            List<SelectedFile> selectedFiles = _sourceSelectedFiles.GetSelectedFiles(helper, SelectedFileType.WebResource).Take(2).ToList();

            if (selectedFiles.Count == 1)
            {
                helper.HandleWebResourceGetAttributeCommand(connectionData, selectedFiles.FirstOrDefault(), this._fieldName, this._fieldTitle);
            }
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, ConnectionData connectionData, OleMenuCommand menuCommand)
        {
            _sourceSelectedFiles.CommandBeforeQueryStatus(applicationObject, menuCommand, SelectedFileType.WebResource);
        }
    }
}
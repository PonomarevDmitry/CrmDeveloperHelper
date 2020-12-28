using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model.Sources;
using System.Collections.Generic;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.WebResources
{
    internal sealed class WebResourceOpenInWebInConnectionCommand : AbstractDynamicCommandByConnectionAll
    {
        private readonly ISourceSelectedFiles _sourceSelectedFiles;
        private readonly ActionOnComponent _actionOnComponent;

        private WebResourceOpenInWebInConnectionCommand(OleMenuCommandService commandService, int baseIdStart, ISourceSelectedFiles sourceSelectedFiles, ActionOnComponent actionOnComponent)
            : base(commandService, baseIdStart)
        {
            this._sourceSelectedFiles = sourceSelectedFiles;
            this._actionOnComponent = actionOnComponent;
        }

        public static WebResourceOpenInWebInConnectionCommand InstanceCodeOpenInWebInConnection { get; private set; }

        public static WebResourceOpenInWebInConnectionCommand InstanceCodeOpenDependentComponentsInWebInConnection { get; private set; }

        public static WebResourceOpenInWebInConnectionCommand InstanceDocumentsOpenInWebInConnection { get; private set; }

        public static WebResourceOpenInWebInConnectionCommand InstanceDocumentsOpenDependentComponentsInWebInConnection { get; private set; }

        public static WebResourceOpenInWebInConnectionCommand InstanceFileOpenInWebInConnection { get; private set; }

        public static WebResourceOpenInWebInConnectionCommand InstanceFileOpenDependentComponentsInWebInConnection { get; private set; }

        public static WebResourceOpenInWebInConnectionCommand InstanceFolderOpenInWebInConnection { get; private set; }

        public static WebResourceOpenInWebInConnectionCommand InstanceFolderOpenDependentComponentsInWebInConnection { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            var sourceCode = CodeSourceSelectedFiles.CreateSource();

            var sourceDocuments = DocumentsSourceSelectedFiles.CreateSource();

            var sourceFile = FileSourceSelectedFiles.CreateSource();

            var sourceFolder = FolderSourceSelectedFiles.CreateSource();

            InstanceCodeOpenInWebInConnection = new WebResourceOpenInWebInConnectionCommand(
                commandService
                , PackageIds.guidDynamicCommandSet.CodeWebResourceOpenInWebInConnectionCommandId
                , sourceCode
                , ActionOnComponent.OpenInWeb
            );

            InstanceCodeOpenDependentComponentsInWebInConnection = new WebResourceOpenInWebInConnectionCommand(
                commandService
                , PackageIds.guidDynamicCommandSet.CodeWebResourceOpenDependentInWebInConnectionCommandId
                , sourceCode
                , ActionOnComponent.OpenDependentComponentsInWeb
            );

            InstanceDocumentsOpenInWebInConnection = new WebResourceOpenInWebInConnectionCommand(
                commandService
                , PackageIds.guidDynamicCommandSet.DocumentsWebResourceOpenInWebInConnectionCommandId
                , sourceDocuments
                , ActionOnComponent.OpenInWeb
            );

            InstanceDocumentsOpenDependentComponentsInWebInConnection = new WebResourceOpenInWebInConnectionCommand(
                commandService
                , PackageIds.guidDynamicCommandSet.DocumentsWebResourceOpenDependentInWebInConnectionCommandId
                , sourceDocuments
                , ActionOnComponent.OpenDependentComponentsInWeb
            );

            InstanceFileOpenInWebInConnection = new WebResourceOpenInWebInConnectionCommand(
                commandService
                , PackageIds.guidDynamicCommandSet.FileWebResourceOpenInWebInConnectionCommandId
                , sourceFile
                , ActionOnComponent.OpenInWeb
            );

            InstanceFileOpenDependentComponentsInWebInConnection = new WebResourceOpenInWebInConnectionCommand(
                commandService
                , PackageIds.guidDynamicCommandSet.FileWebResourceOpenDependentInWebInConnectionCommandId
                , sourceFile
                , ActionOnComponent.OpenDependentComponentsInWeb
            );

            InstanceFolderOpenInWebInConnection = new WebResourceOpenInWebInConnectionCommand(
                commandService
                , PackageIds.guidDynamicCommandSet.FolderWebResourceOpenInWebInConnectionCommandId
                , sourceFolder
                , ActionOnComponent.OpenInWeb
            );

            InstanceFolderOpenDependentComponentsInWebInConnection = new WebResourceOpenInWebInConnectionCommand(
                commandService
                , PackageIds.guidDynamicCommandSet.FolderWebResourceOpenDependentInWebInConnectionCommandId
                , sourceFolder
                , ActionOnComponent.OpenDependentComponentsInWeb
            );
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            List<SelectedFile> selectedFiles = _sourceSelectedFiles.GetSelectedFiles(helper, SelectedFileType.WebResource).ToList();

            helper.HandleOpenWebResourceInWeb(connectionData, this._actionOnComponent, selectedFiles);
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, ConnectionData connectionData, OleMenuCommand menuCommand)
        {
            _sourceSelectedFiles.CommandBeforeQueryStatus(applicationObject, menuCommand, SelectedFileType.WebResource);
        }
    }
}

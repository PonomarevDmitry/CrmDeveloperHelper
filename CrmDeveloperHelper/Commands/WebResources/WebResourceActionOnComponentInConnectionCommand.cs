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
    internal sealed class WebResourceActionOnComponentInConnectionCommand : AbstractDynamicCommandByConnectionAll
    {
        private readonly ISourceSelectedFiles _sourceSelectedFiles;
        private readonly ActionOnComponent _actionOnComponent;

        private WebResourceActionOnComponentInConnectionCommand(OleMenuCommandService commandService, int baseIdStart, ISourceSelectedFiles sourceSelectedFiles, ActionOnComponent actionOnComponent)
            : base(commandService, baseIdStart)
        {
            this._sourceSelectedFiles = sourceSelectedFiles;
            this._actionOnComponent = actionOnComponent;
        }

        public static WebResourceActionOnComponentInConnectionCommand InstanceCodeOpenInWebInConnection { get; private set; }

        public static WebResourceActionOnComponentInConnectionCommand InstanceCodeOpenDependentComponentsInWebInConnection { get; private set; }

        public static WebResourceActionOnComponentInConnectionCommand InstanceCodeOpenDependentComponentsInExplorerInConnection { get; private set; }

        public static WebResourceActionOnComponentInConnectionCommand InstanceCodeOpenSolutionsContainingComponentInExplorerInConnection { get; private set; }

        public static WebResourceActionOnComponentInConnectionCommand InstanceFileOpenInWebInConnection { get; private set; }

        public static WebResourceActionOnComponentInConnectionCommand InstanceFileOpenDependentComponentsInWebInConnection { get; private set; }

        public static WebResourceActionOnComponentInConnectionCommand InstanceFileOpenDependentComponentsInExplorerInConnection { get; private set; }

        public static WebResourceActionOnComponentInConnectionCommand InstanceFileOpenSolutionsContainingComponentInExplorerInConnection { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            var sourceCode = CodeSourceSelectedFiles.CreateSource();

            var sourceFile = FileSourceSelectedFileSingle.CreateSource();

            InstanceCodeOpenInWebInConnection = new WebResourceActionOnComponentInConnectionCommand(
                commandService
                , PackageIds.guidDynamicCommandSet.CodeWebResourceOpenInWebInConnectionCommandId
                , sourceCode
                , ActionOnComponent.OpenInWeb
            );

            InstanceCodeOpenDependentComponentsInWebInConnection = new WebResourceActionOnComponentInConnectionCommand(
                commandService
                , PackageIds.guidDynamicCommandSet.CodeWebResourceOpenDependentInWebInConnectionCommandId
                , sourceCode
                , ActionOnComponent.OpenDependentComponentsInWeb
            );

            InstanceCodeOpenDependentComponentsInExplorerInConnection = new WebResourceActionOnComponentInConnectionCommand(
                commandService
                , PackageIds.guidDynamicCommandSet.CodeWebResourceOpenDependentInExplorerInConnectionCommandId
                , sourceCode
                , ActionOnComponent.OpenDependentComponentsInExplorer
            );

            InstanceCodeOpenSolutionsContainingComponentInExplorerInConnection = new WebResourceActionOnComponentInConnectionCommand(
                commandService
                , PackageIds.guidDynamicCommandSet.CodeWebResourceOpenSolutionsListWithComponentInExplorerInConnectionCommandId
                , sourceCode
                , ActionOnComponent.OpenSolutionsListWithComponentInExplorer
            );

            InstanceFileOpenInWebInConnection = new WebResourceActionOnComponentInConnectionCommand(
                commandService
                , PackageIds.guidDynamicCommandSet.FileWebResourceOpenInWebInConnectionCommandId
                , sourceFile
                , ActionOnComponent.OpenInWeb
            );

            InstanceFileOpenDependentComponentsInWebInConnection = new WebResourceActionOnComponentInConnectionCommand(
                commandService
                , PackageIds.guidDynamicCommandSet.FileWebResourceOpenDependentInWebInConnectionCommandId
                , sourceFile
                , ActionOnComponent.OpenDependentComponentsInWeb
            );

            InstanceFileOpenDependentComponentsInExplorerInConnection = new WebResourceActionOnComponentInConnectionCommand(
                commandService
                , PackageIds.guidDynamicCommandSet.FileWebResourceOpenDependentInExplorerInConnectionCommandId
                , sourceFile
                , ActionOnComponent.OpenDependentComponentsInExplorer
            );

            InstanceFileOpenSolutionsContainingComponentInExplorerInConnection = new WebResourceActionOnComponentInConnectionCommand(
                commandService
                , PackageIds.guidDynamicCommandSet.FileWebResourceOpenSolutionsListWithComponentInExplorerInConnectionCommandId
                , sourceFile
                , ActionOnComponent.OpenSolutionsListWithComponentInExplorer
            );
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            List<SelectedFile> selectedFiles = _sourceSelectedFiles.GetSelectedFiles(helper, SelectedFileType.WebResource).Take(2).ToList();

            if (selectedFiles.Count == 1)
            {
                helper.HandleOpenWebResource(connectionData, selectedFiles.FirstOrDefault(), this._actionOnComponent);
            }
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, ConnectionData connectionData, OleMenuCommand menuCommand)
        {
            _sourceSelectedFiles.CommandBeforeQueryStatus(applicationObject, menuCommand, SelectedFileType.WebResource);
        }
    }
}
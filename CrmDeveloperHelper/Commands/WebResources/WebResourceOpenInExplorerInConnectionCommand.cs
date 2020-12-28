using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model.Sources;
using System.Collections.Generic;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.WebResources
{
    internal sealed class WebResourceOpenInExplorerInConnectionCommand : AbstractDynamicCommandByConnectionAll
    {
        private readonly ISourceSelectedFiles _sourceSelectedFiles;
        private readonly ActionOnComponent _actionOnComponent;

        private WebResourceOpenInExplorerInConnectionCommand(OleMenuCommandService commandService, int baseIdStart, ISourceSelectedFiles sourceSelectedFiles, ActionOnComponent actionOnComponent)
            : base(commandService, baseIdStart)
        {
            this._sourceSelectedFiles = sourceSelectedFiles;
            this._actionOnComponent = actionOnComponent;
        }

        public static WebResourceOpenInExplorerInConnectionCommand InstanceCodeOpenDependentComponentsInExplorerInConnection { get; private set; }

        public static WebResourceOpenInExplorerInConnectionCommand InstanceCodeOpenSolutionsContainingComponentInExplorerInConnection { get; private set; }

        public static WebResourceOpenInExplorerInConnectionCommand InstanceFileOpenDependentComponentsInExplorerInConnection { get; private set; }

        public static WebResourceOpenInExplorerInConnectionCommand InstanceFileOpenSolutionsContainingComponentInExplorerInConnection { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            var sourceCode = CodeSourceSelectedFiles.CreateSource();

            var sourceFile = FileSourceSelectedFileSingle.CreateSource();

            InstanceCodeOpenDependentComponentsInExplorerInConnection = new WebResourceOpenInExplorerInConnectionCommand(
                commandService
                , PackageIds.guidDynamicCommandSet.CodeWebResourceOpenDependentInExplorerInConnectionCommandId
                , sourceCode
                , ActionOnComponent.OpenDependentComponentsInExplorer
            );

            InstanceCodeOpenSolutionsContainingComponentInExplorerInConnection = new WebResourceOpenInExplorerInConnectionCommand(
                commandService
                , PackageIds.guidDynamicCommandSet.CodeWebResourceOpenSolutionsListWithComponentInExplorerInConnectionCommandId
                , sourceCode
                , ActionOnComponent.OpenSolutionsListWithComponentInExplorer
            );

            InstanceFileOpenDependentComponentsInExplorerInConnection = new WebResourceOpenInExplorerInConnectionCommand(
                commandService
                , PackageIds.guidDynamicCommandSet.FileWebResourceOpenDependentInExplorerInConnectionCommandId
                , sourceFile
                , ActionOnComponent.OpenDependentComponentsInExplorer
            );

            InstanceFileOpenSolutionsContainingComponentInExplorerInConnection = new WebResourceOpenInExplorerInConnectionCommand(
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
                helper.HandleOpenWebResourceInExplorer(connectionData, selectedFiles.FirstOrDefault(), this._actionOnComponent);
            }
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, ConnectionData connectionData, OleMenuCommand menuCommand)
        {
            _sourceSelectedFiles.CommandBeforeQueryStatus(applicationObject, menuCommand, SelectedFileType.WebResource);
        }
    }
}
using EnvDTE80;
using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model.SourcesSelectedFiles;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.CSharp
{
    internal sealed class CSharpProjectPluginTypeActionOnComponentInConnectionCommand : AbstractDynamicCommandByConnectionAll
    {
        private readonly ISourceSelectedFiles _sourceSelectedFiles;
        private readonly ActionOnComponent _actionOnComponent;

        private CSharpProjectPluginTypeActionOnComponentInConnectionCommand(OleMenuCommandService commandService, int baseIdStart, ISourceSelectedFiles sourceSelectedFiles, ActionOnComponent actionOnComponent)
            : base(commandService, baseIdStart)
        {
            this._sourceSelectedFiles = sourceSelectedFiles;
            this._actionOnComponent = actionOnComponent;
        }

        public static CSharpProjectPluginTypeActionOnComponentInConnectionCommand InstanceCodeOpenDependentComponentsInWebInConnection { get; private set; }

        public static CSharpProjectPluginTypeActionOnComponentInConnectionCommand InstanceCodeOpenDependentComponentsInExplorerInConnection { get; private set; }

        public static CSharpProjectPluginTypeActionOnComponentInConnectionCommand InstanceCodeOpenSolutionsListWithComponentInExplorerInConnection { get; private set; }



        public static CSharpProjectPluginTypeActionOnComponentInConnectionCommand InstanceDocumentsOpenDependentComponentsInWebInConnection { get; private set; }

        public static CSharpProjectPluginTypeActionOnComponentInConnectionCommand InstanceDocumentsOpenDependentComponentsInExplorerInConnection { get; private set; }

        public static CSharpProjectPluginTypeActionOnComponentInConnectionCommand InstanceDocumentsOpenSolutionsListWithComponentInExplorerInConnection { get; private set; }



        public static CSharpProjectPluginTypeActionOnComponentInConnectionCommand InstanceFileOpenDependentComponentsInWebInConnection { get; private set; }

        public static CSharpProjectPluginTypeActionOnComponentInConnectionCommand InstanceFileOpenDependentComponentsInExplorerInConnection { get; private set; }

        public static CSharpProjectPluginTypeActionOnComponentInConnectionCommand InstanceFileOpenSolutionsListWithComponentInExplorerInConnection { get; private set; }



        public static CSharpProjectPluginTypeActionOnComponentInConnectionCommand InstanceOpenDependentComponentsInWebInConnection { get; private set; }

        public static CSharpProjectPluginTypeActionOnComponentInConnectionCommand InstanceOpenDependentComponentsInExplorerInConnection { get; private set; }

        public static CSharpProjectPluginTypeActionOnComponentInConnectionCommand InstanceOpenSolutionsListWithComponentInExplorerInConnection { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            var sourceCode = CodeSourceSelectedFiles.CreateSource();

            var sourceDocuments = DocumentsSourceSelectedFiles.CreateSource();

            var sourceFile = FileSourceSelectedFiles.CreateSource();

            var sourceFolder = FolderSourceSelectedFiles.CreateSource();

            InstanceCodeOpenDependentComponentsInWebInConnection = new CSharpProjectPluginTypeActionOnComponentInConnectionCommand(
                 commandService
                 , PackageIds.guidDynamicCommandSet.CodeCSharpProjectPluginTypeOpenDependentInWebInConnectionCommandId
                 , sourceCode
                 , ActionOnComponent.OpenDependentComponentsInWeb
             );

            InstanceCodeOpenDependentComponentsInExplorerInConnection = new CSharpProjectPluginTypeActionOnComponentInConnectionCommand(
                commandService
                , PackageIds.guidDynamicCommandSet.CodeCSharpProjectPluginTypeOpenDependentInExplorerInConnectionCommandId
                , sourceCode
                , ActionOnComponent.OpenDependentComponentsInExplorer
            );

            InstanceCodeOpenSolutionsListWithComponentInExplorerInConnection = new CSharpProjectPluginTypeActionOnComponentInConnectionCommand(
                commandService
                , PackageIds.guidDynamicCommandSet.CodeCSharpProjectPluginTypeOpenSolutionsListWithComponentInExplorerInConnectionCommandId
                , sourceCode
                , ActionOnComponent.OpenSolutionsListWithComponentInExplorer
            );



            InstanceDocumentsOpenDependentComponentsInWebInConnection = new CSharpProjectPluginTypeActionOnComponentInConnectionCommand(
                commandService
                , PackageIds.guidDynamicCommandSet.DocumentsCSharpProjectPluginTypeOpenDependentInWebInConnectionCommandId
                , sourceDocuments
                , ActionOnComponent.OpenDependentComponentsInWeb
            );

            InstanceDocumentsOpenDependentComponentsInExplorerInConnection = new CSharpProjectPluginTypeActionOnComponentInConnectionCommand(
                commandService
                , PackageIds.guidDynamicCommandSet.DocumentsCSharpProjectPluginTypeOpenDependentInExplorerInConnectionCommandId
                , sourceDocuments
                , ActionOnComponent.OpenDependentComponentsInExplorer
            );

            InstanceDocumentsOpenSolutionsListWithComponentInExplorerInConnection = new CSharpProjectPluginTypeActionOnComponentInConnectionCommand(
                commandService
                , PackageIds.guidDynamicCommandSet.DocumentsCSharpProjectPluginTypeOpenSolutionsListWithComponentInExplorerInConnectionCommandId
                , sourceDocuments
                , ActionOnComponent.OpenSolutionsListWithComponentInExplorer
            );



            InstanceFileOpenDependentComponentsInWebInConnection = new CSharpProjectPluginTypeActionOnComponentInConnectionCommand(
                commandService
                , PackageIds.guidDynamicCommandSet.FileCSharpProjectPluginTypeOpenDependentInWebInConnectionCommandId
                , sourceFile
                , ActionOnComponent.OpenDependentComponentsInWeb
            );

            InstanceFileOpenDependentComponentsInExplorerInConnection = new CSharpProjectPluginTypeActionOnComponentInConnectionCommand(
                commandService
                , PackageIds.guidDynamicCommandSet.FileCSharpProjectPluginTypeOpenDependentInExplorerInConnectionCommandId
                , sourceFile
                , ActionOnComponent.OpenDependentComponentsInExplorer
            );

            InstanceFileOpenSolutionsListWithComponentInExplorerInConnection = new CSharpProjectPluginTypeActionOnComponentInConnectionCommand(
                commandService
                , PackageIds.guidDynamicCommandSet.FileCSharpProjectPluginTypeOpenSolutionsListWithComponentInExplorerInConnectionCommandId
                , sourceFile
                , ActionOnComponent.OpenSolutionsListWithComponentInExplorer
            );



            InstanceOpenDependentComponentsInWebInConnection = new CSharpProjectPluginTypeActionOnComponentInConnectionCommand(
                commandService
                , PackageIds.guidDynamicCommandSet.FolderCSharpProjectPluginTypeOpenDependentInWebInConnectionCommandId
                , sourceFolder
                , ActionOnComponent.OpenDependentComponentsInWeb
            );

            InstanceOpenDependentComponentsInExplorerInConnection = new CSharpProjectPluginTypeActionOnComponentInConnectionCommand(
                commandService
                , PackageIds.guidDynamicCommandSet.FolderCSharpProjectPluginTypeOpenDependentInExplorerInConnectionCommandId
                , sourceFolder
                , ActionOnComponent.OpenDependentComponentsInExplorer
            );

            InstanceOpenSolutionsListWithComponentInExplorerInConnection = new CSharpProjectPluginTypeActionOnComponentInConnectionCommand(
                commandService
                , PackageIds.guidDynamicCommandSet.FolderCSharpProjectPluginTypeOpenSolutionsListWithComponentInExplorerInConnectionCommandId
                , sourceFolder
                , ActionOnComponent.OpenSolutionsListWithComponentInExplorer
            );
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            var selectedFiles = _sourceSelectedFiles.GetSelectedFiles(helper, SelectedFileType.CSharp).ToList();

            helper.HandleActionOnPluginTypesCommand(connectionData, selectedFiles, this._actionOnComponent, string.Empty, string.Empty);
        }

        protected override void CommandBeforeQueryStatus(DTE2 applicationObject, ConnectionData connectionData, OleMenuCommand menuCommand)
        {
            _sourceSelectedFiles.CommandBeforeQueryStatus(applicationObject, menuCommand, SelectedFileType.CSharp);
        }
    }
}
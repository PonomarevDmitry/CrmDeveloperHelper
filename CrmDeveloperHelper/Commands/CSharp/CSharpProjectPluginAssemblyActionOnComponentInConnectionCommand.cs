using EnvDTE80;
using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model.Sources;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.CSharp
{
    internal sealed class CSharpProjectPluginAssemblyActionOnComponentInConnectionCommand : AbstractDynamicCommandByConnectionAll
    {
        private readonly ISourceSelectedProjects _sourceSelectedProjects;
        private readonly ActionOnComponent _actionOnComponent;

        private CSharpProjectPluginAssemblyActionOnComponentInConnectionCommand(OleMenuCommandService commandService, int baseIdStart, ISourceSelectedProjects sourceSelectedProjects, ActionOnComponent actionOnComponent)
            : base(commandService, baseIdStart)
        {
            this._sourceSelectedProjects = sourceSelectedProjects;
            this._actionOnComponent = actionOnComponent;
        }

        public static CSharpProjectPluginAssemblyActionOnComponentInConnectionCommand InstanceCodeOpenDependentComponentsInWebInConnection { get; private set; }

        public static CSharpProjectPluginAssemblyActionOnComponentInConnectionCommand InstanceCodeOpenDependentComponentsInExplorerInConnection { get; private set; }

        public static CSharpProjectPluginAssemblyActionOnComponentInConnectionCommand InstanceCodeOpenSolutionsListWithComponentInExplorerInConnection { get; private set; }



        public static CSharpProjectPluginAssemblyActionOnComponentInConnectionCommand InstanceDocumentsOpenDependentComponentsInWebInConnection { get; private set; }

        public static CSharpProjectPluginAssemblyActionOnComponentInConnectionCommand InstanceDocumentsOpenDependentComponentsInExplorerInConnection { get; private set; }

        public static CSharpProjectPluginAssemblyActionOnComponentInConnectionCommand InstanceDocumentsOpenSolutionsListWithComponentInExplorerInConnection { get; private set; }



        public static CSharpProjectPluginAssemblyActionOnComponentInConnectionCommand InstanceFileOpenDependentComponentsInWebInConnection { get; private set; }

        public static CSharpProjectPluginAssemblyActionOnComponentInConnectionCommand InstanceFileOpenDependentComponentsInExplorerInConnection { get; private set; }

        public static CSharpProjectPluginAssemblyActionOnComponentInConnectionCommand InstanceFileOpenSolutionsListWithComponentInExplorerInConnection { get; private set; }



        public static CSharpProjectPluginAssemblyActionOnComponentInConnectionCommand InstanceFolderOpenDependentComponentsInWebInConnection { get; private set; }

        public static CSharpProjectPluginAssemblyActionOnComponentInConnectionCommand InstanceFolderOpenDependentComponentsInExplorerInConnection { get; private set; }

        public static CSharpProjectPluginAssemblyActionOnComponentInConnectionCommand InstanceFolderOpenSolutionsListWithComponentInExplorerInConnection { get; private set; }



        public static CSharpProjectPluginAssemblyActionOnComponentInConnectionCommand InstanceProjectOpenDependentComponentsInWebInConnection { get; private set; }

        public static CSharpProjectPluginAssemblyActionOnComponentInConnectionCommand InstanceProjectOpenDependentComponentsInExplorerInConnection { get; private set; }

        public static CSharpProjectPluginAssemblyActionOnComponentInConnectionCommand InstanceProjectOpenSolutionsListWithComponentInExplorerInConnection { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            var sourceCode = CodeSourceSelectedProjects.CreateSource();

            var sourceDocuments = DocumentsSourceSelectedProjects.CreateSource();

            var sourceFile = FileSourceSelectedProjects.CreateSource();

            var sourceFolder = FolderSourceSelectedProjects.CreateSource();

            var sourceProject = ProjectSourceSelectedProjects.CreateSource();

            InstanceCodeOpenDependentComponentsInWebInConnection = new CSharpProjectPluginAssemblyActionOnComponentInConnectionCommand(
                 commandService
                 , PackageIds.guidDynamicCommandSet.CodeCSharpProjectPluginAssemblyOpenDependentInWebInConnectionCommandId
                 , sourceCode
                 , ActionOnComponent.OpenDependentComponentsInWeb
             );

            InstanceCodeOpenDependentComponentsInExplorerInConnection = new CSharpProjectPluginAssemblyActionOnComponentInConnectionCommand(
                commandService
                , PackageIds.guidDynamicCommandSet.CodeCSharpProjectPluginAssemblyOpenDependentInExplorerInConnectionCommandId
                , sourceCode
                , ActionOnComponent.OpenDependentComponentsInExplorer
            );

            InstanceCodeOpenSolutionsListWithComponentInExplorerInConnection = new CSharpProjectPluginAssemblyActionOnComponentInConnectionCommand(
                commandService
                , PackageIds.guidDynamicCommandSet.CodeCSharpProjectPluginAssemblyOpenSolutionsListWithComponentInExplorerInConnectionCommandId
                , sourceCode
                , ActionOnComponent.OpenSolutionsListWithComponentInExplorer
            );



            InstanceDocumentsOpenDependentComponentsInWebInConnection = new CSharpProjectPluginAssemblyActionOnComponentInConnectionCommand(
                commandService
                , PackageIds.guidDynamicCommandSet.DocumentsCSharpProjectPluginAssemblyOpenDependentInWebInConnectionCommandId
                , sourceDocuments
                , ActionOnComponent.OpenDependentComponentsInWeb
            );

            InstanceDocumentsOpenDependentComponentsInExplorerInConnection = new CSharpProjectPluginAssemblyActionOnComponentInConnectionCommand(
                commandService
                , PackageIds.guidDynamicCommandSet.DocumentsCSharpProjectPluginAssemblyOpenDependentInExplorerInConnectionCommandId
                , sourceDocuments
                , ActionOnComponent.OpenDependentComponentsInExplorer
            );

            InstanceDocumentsOpenSolutionsListWithComponentInExplorerInConnection = new CSharpProjectPluginAssemblyActionOnComponentInConnectionCommand(
                commandService
                , PackageIds.guidDynamicCommandSet.DocumentsCSharpProjectPluginAssemblyOpenSolutionsListWithComponentInExplorerInConnectionCommandId
                , sourceDocuments
                , ActionOnComponent.OpenSolutionsListWithComponentInExplorer
            );



            InstanceFileOpenDependentComponentsInWebInConnection = new CSharpProjectPluginAssemblyActionOnComponentInConnectionCommand(
                commandService
                , PackageIds.guidDynamicCommandSet.FileCSharpProjectPluginAssemblyOpenDependentInWebInConnectionCommandId
                , sourceFile
                , ActionOnComponent.OpenDependentComponentsInWeb
            );

            InstanceFileOpenDependentComponentsInExplorerInConnection = new CSharpProjectPluginAssemblyActionOnComponentInConnectionCommand(
                commandService
                , PackageIds.guidDynamicCommandSet.FileCSharpProjectPluginAssemblyOpenDependentInExplorerInConnectionCommandId
                , sourceFile
                , ActionOnComponent.OpenDependentComponentsInExplorer
            );

            InstanceFileOpenSolutionsListWithComponentInExplorerInConnection = new CSharpProjectPluginAssemblyActionOnComponentInConnectionCommand(
                commandService
                , PackageIds.guidDynamicCommandSet.FileCSharpProjectPluginAssemblyOpenSolutionsListWithComponentInExplorerInConnectionCommandId
                , sourceFile
                , ActionOnComponent.OpenSolutionsListWithComponentInExplorer
            );



            InstanceFolderOpenDependentComponentsInWebInConnection = new CSharpProjectPluginAssemblyActionOnComponentInConnectionCommand(
                commandService
                , PackageIds.guidDynamicCommandSet.FolderCSharpProjectPluginAssemblyOpenDependentInWebInConnectionCommandId
                , sourceFolder
                , ActionOnComponent.OpenDependentComponentsInWeb
            );

            InstanceFolderOpenDependentComponentsInExplorerInConnection = new CSharpProjectPluginAssemblyActionOnComponentInConnectionCommand(
                commandService
                , PackageIds.guidDynamicCommandSet.FolderCSharpProjectPluginAssemblyOpenDependentInExplorerInConnectionCommandId
                , sourceFolder
                , ActionOnComponent.OpenDependentComponentsInExplorer
            );

            InstanceFolderOpenSolutionsListWithComponentInExplorerInConnection = new CSharpProjectPluginAssemblyActionOnComponentInConnectionCommand(
                commandService
                , PackageIds.guidDynamicCommandSet.FolderCSharpProjectPluginAssemblyOpenSolutionsListWithComponentInExplorerInConnectionCommandId
                , sourceFolder
                , ActionOnComponent.OpenSolutionsListWithComponentInExplorer
            );



            InstanceProjectOpenDependentComponentsInWebInConnection = new CSharpProjectPluginAssemblyActionOnComponentInConnectionCommand(
                commandService
                , PackageIds.guidDynamicCommandSet.ProjectPluginAssemblyOpenDependentInWebInConnectionCommandId
                , sourceProject
                , ActionOnComponent.OpenDependentComponentsInWeb
            );

            InstanceProjectOpenDependentComponentsInExplorerInConnection = new CSharpProjectPluginAssemblyActionOnComponentInConnectionCommand(
                commandService
                , PackageIds.guidDynamicCommandSet.ProjectPluginAssemblyOpenDependentInExplorerInConnectionCommandId
                , sourceProject
                , ActionOnComponent.OpenDependentComponentsInExplorer
            );

            InstanceProjectOpenSolutionsListWithComponentInExplorerInConnection = new CSharpProjectPluginAssemblyActionOnComponentInConnectionCommand(
                commandService
                , PackageIds.guidDynamicCommandSet.ProjectPluginAssemblyOpenSolutionsListWithComponentInExplorerInConnectionCommandId
                , sourceProject
                , ActionOnComponent.OpenSolutionsListWithComponentInExplorer
            );
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            var selectedProjects = _sourceSelectedProjects.GetSelectedProjects(helper).ToList();

            helper.HandleActionOnProjectPluginAssemblyCommand(connectionData, selectedProjects, this._actionOnComponent);
        }

        protected override void CommandBeforeQueryStatus(DTE2 applicationObject, ConnectionData connectionData, OleMenuCommand menuCommand)
        {
            _sourceSelectedProjects.CommandBeforeQueryStatus(applicationObject, menuCommand);
        }
    }
}
using EnvDTE80;
using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model.Sources;
using System.Linq;


namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.CSharp
{
    internal sealed class CSharpProjectPluginAssemblyActionOnComponentInConnectionWithoutCurrentCommand : AbstractDynamicCommandByConnectionAllWithoutCurrent
    {
        private readonly ISourceSelectedProjects _sourceSelectedProjects;
        private readonly ActionOnComponent _actionOnComponent;

        private CSharpProjectPluginAssemblyActionOnComponentInConnectionWithoutCurrentCommand(OleMenuCommandService commandService, int baseIdStart, ISourceSelectedProjects sourceSelectedProjects, ActionOnComponent actionOnComponent)
            : base(commandService, baseIdStart)
        {
            this._sourceSelectedProjects = sourceSelectedProjects;
            this._actionOnComponent = actionOnComponent;
        }

        public static CSharpProjectPluginAssemblyActionOnComponentInConnectionWithoutCurrentCommand InstanceCodeEntityDescription { get; private set; }

        public static CSharpProjectPluginAssemblyActionOnComponentInConnectionWithoutCurrentCommand InstanceCodeDescription { get; private set; }



        public static CSharpProjectPluginAssemblyActionOnComponentInConnectionWithoutCurrentCommand InstanceDocumentsEntityDescription { get; private set; }

        public static CSharpProjectPluginAssemblyActionOnComponentInConnectionWithoutCurrentCommand InstanceDocumentsDescription { get; private set; }



        public static CSharpProjectPluginAssemblyActionOnComponentInConnectionWithoutCurrentCommand InstanceFileEntityDescription { get; private set; }

        public static CSharpProjectPluginAssemblyActionOnComponentInConnectionWithoutCurrentCommand InstanceFileDescription { get; private set; }



        public static CSharpProjectPluginAssemblyActionOnComponentInConnectionWithoutCurrentCommand InstanceFolderEntityDescription { get; private set; }

        public static CSharpProjectPluginAssemblyActionOnComponentInConnectionWithoutCurrentCommand InstanceFolderDescription { get; private set; }



        public static CSharpProjectPluginAssemblyActionOnComponentInConnectionWithoutCurrentCommand InstanceProjectEntityDescription { get; private set; }

        public static CSharpProjectPluginAssemblyActionOnComponentInConnectionWithoutCurrentCommand InstanceProjectDescription { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            var sourceCode = CodeSourceSelectedProjects.CreateSource();

            var sourceDocuments = DocumentsSourceSelectedProjects.CreateSource();

            var sourceFile = FileSourceSelectedProjects.CreateSource();

            var sourceFolder = FolderSourceSelectedProjects.CreateSource();

            var sourceProject = ProjectSourceSelectedProjects.CreateSource();

            InstanceCodeEntityDescription = new CSharpProjectPluginAssemblyActionOnComponentInConnectionWithoutCurrentCommand(
                commandService
                , PackageIds.guidDynamicCommandSet.CodeCSharpProjectPluginAssemblyCreateEntityDescriptionInConnectionCommandId
                , sourceCode
                , ActionOnComponent.EntityDescription
            );

            InstanceCodeDescription = new CSharpProjectPluginAssemblyActionOnComponentInConnectionWithoutCurrentCommand(
                commandService
                , PackageIds.guidDynamicCommandSet.CodeCSharpProjectPluginAssemblyCreateDescriptionInConnectionCommandId
                , sourceCode
                , ActionOnComponent.Description
            );



            InstanceDocumentsEntityDescription = new CSharpProjectPluginAssemblyActionOnComponentInConnectionWithoutCurrentCommand(
                commandService
                , PackageIds.guidDynamicCommandSet.DocumentsCSharpProjectPluginAssemblyCreateEntityDescriptionInConnectionCommandId
                , sourceDocuments
                , ActionOnComponent.EntityDescription
            );

            InstanceDocumentsDescription = new CSharpProjectPluginAssemblyActionOnComponentInConnectionWithoutCurrentCommand(
                commandService
                , PackageIds.guidDynamicCommandSet.DocumentsCSharpProjectPluginAssemblyCreateDescriptionInConnectionCommandId
                , sourceDocuments
                , ActionOnComponent.Description
            );



            InstanceFileEntityDescription = new CSharpProjectPluginAssemblyActionOnComponentInConnectionWithoutCurrentCommand(
                commandService
                , PackageIds.guidDynamicCommandSet.FileCSharpProjectPluginAssemblyCreateEntityDescriptionInConnectionCommandId
                , sourceFile
                , ActionOnComponent.EntityDescription
            );

            InstanceFileDescription = new CSharpProjectPluginAssemblyActionOnComponentInConnectionWithoutCurrentCommand(
                commandService
                , PackageIds.guidDynamicCommandSet.FileCSharpProjectPluginAssemblyCreateDescriptionInConnectionCommandId
                , sourceFile
                , ActionOnComponent.Description
            );



            InstanceFolderEntityDescription = new CSharpProjectPluginAssemblyActionOnComponentInConnectionWithoutCurrentCommand(
                commandService
                , PackageIds.guidDynamicCommandSet.FolderCSharpProjectPluginAssemblyCreateEntityDescriptionInConnectionCommandId
                , sourceFolder
                , ActionOnComponent.EntityDescription
            );

            InstanceFolderDescription = new CSharpProjectPluginAssemblyActionOnComponentInConnectionWithoutCurrentCommand(
                commandService
                , PackageIds.guidDynamicCommandSet.FolderCSharpProjectPluginAssemblyCreateDescriptionInConnectionCommandId
                , sourceFolder
                , ActionOnComponent.Description
            );



            InstanceProjectEntityDescription = new CSharpProjectPluginAssemblyActionOnComponentInConnectionWithoutCurrentCommand(
                commandService
                , PackageIds.guidDynamicCommandSet.ProjectPluginAssemblyCreateEntityDescriptionInConnectionCommandId
                , sourceProject
                , ActionOnComponent.EntityDescription
            );

            InstanceProjectDescription = new CSharpProjectPluginAssemblyActionOnComponentInConnectionWithoutCurrentCommand(
                commandService
                , PackageIds.guidDynamicCommandSet.ProjectPluginAssemblyCreateDescriptionInConnectionCommandId
                , sourceProject
                , ActionOnComponent.Description
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
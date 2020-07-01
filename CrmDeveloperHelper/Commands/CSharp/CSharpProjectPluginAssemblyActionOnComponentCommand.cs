using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model.Sources;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.CSharp
{
    internal sealed class CSharpProjectPluginAssemblyActionOnComponentCommand : AbstractSingleCommand
    {
        private readonly ISourceSelectedProjects _sourceSelectedProjects;
        private readonly string _commandNameForCorrection;
        private readonly ActionOnComponent _actionOnComponent;

        private CSharpProjectPluginAssemblyActionOnComponentCommand(OleMenuCommandService commandService, int idCommand, ISourceSelectedProjects sourceSelectedProjects, string commandNameForCorrection, ActionOnComponent actionOnComponent)
            : base(commandService, idCommand)
        {
            this._sourceSelectedProjects = sourceSelectedProjects;
            this._commandNameForCorrection = commandNameForCorrection;
            this._actionOnComponent = actionOnComponent;
        }

        public static CSharpProjectPluginAssemblyActionOnComponentCommand InstanceCodeEntityDescription { get; private set; }

        public static CSharpProjectPluginAssemblyActionOnComponentCommand InstanceCodeDescription { get; private set; }



        public static CSharpProjectPluginAssemblyActionOnComponentCommand InstanceDocumentsEntityDescription { get; private set; }

        public static CSharpProjectPluginAssemblyActionOnComponentCommand InstanceDocumentsDescription { get; private set; }



        public static CSharpProjectPluginAssemblyActionOnComponentCommand InstanceFileEntityDescription { get; private set; }

        public static CSharpProjectPluginAssemblyActionOnComponentCommand InstanceFileDescription { get; private set; }



        public static CSharpProjectPluginAssemblyActionOnComponentCommand InstanceFolderEntityDescription { get; private set; }

        public static CSharpProjectPluginAssemblyActionOnComponentCommand InstanceFolderDescription { get; private set; }



        public static CSharpProjectPluginAssemblyActionOnComponentCommand InstanceProjectEntityDescription { get; private set; }

        public static CSharpProjectPluginAssemblyActionOnComponentCommand InstanceProjectDescription { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            var sourceCode = CodeSourceSelectedProjects.CreateSource();

            var sourceDocuments = DocumentsSourceSelectedProjects.CreateSource();

            var sourceFile = FileSourceSelectedProjects.CreateSource();

            var sourceFolder = FolderSourceSelectedProjects.CreateSource();

            var sourceProject = ProjectSourceSelectedProjects.CreateSource();

            InstanceCodeEntityDescription = new CSharpProjectPluginAssemblyActionOnComponentCommand(
                commandService
                , PackageIds.guidCommandSet.CodeCSharpProjectPluginAssemblyCreateEntityDescriptionCommandId
                , sourceCode
                , Properties.CommandNames.CodeCSharpProjectPluginAssemblyCreateEntityDescriptionCommand
                , ActionOnComponent.EntityDescription
            );

            InstanceCodeDescription = new CSharpProjectPluginAssemblyActionOnComponentCommand(
                commandService
                , PackageIds.guidCommandSet.CodeCSharpProjectPluginAssemblyCreateDescriptionCommandId
                , sourceCode
                , Properties.CommandNames.CodeCSharpProjectPluginAssemblyCreateDescriptionCommand
                , ActionOnComponent.Description
            );



            InstanceDocumentsEntityDescription = new CSharpProjectPluginAssemblyActionOnComponentCommand(
                commandService
                , PackageIds.guidCommandSet.DocumentsCSharpProjectPluginAssemblyCreateEntityDescriptionCommandId
                , sourceDocuments
                , Properties.CommandNames.DocumentsCSharpProjectPluginAssemblyCreateEntityDescriptionCommand
                , ActionOnComponent.EntityDescription
            );

            InstanceDocumentsDescription = new CSharpProjectPluginAssemblyActionOnComponentCommand(
                commandService
                , PackageIds.guidCommandSet.DocumentsCSharpProjectPluginAssemblyCreateDescriptionCommandId
                , sourceDocuments
                , Properties.CommandNames.DocumentsCSharpProjectPluginAssemblyCreateDescriptionCommand
                , ActionOnComponent.Description
            );



            InstanceFileEntityDescription = new CSharpProjectPluginAssemblyActionOnComponentCommand(
                commandService
                , PackageIds.guidCommandSet.FileCSharpProjectPluginAssemblyCreateEntityDescriptionCommandId
                , sourceFile
                , Properties.CommandNames.FileCSharpProjectPluginAssemblyCreateEntityDescriptionCommand
                , ActionOnComponent.EntityDescription
            );

            InstanceFileDescription = new CSharpProjectPluginAssemblyActionOnComponentCommand(
                commandService
                , PackageIds.guidCommandSet.FileCSharpProjectPluginAssemblyCreateDescriptionCommandId
                , sourceFile
                , Properties.CommandNames.FileCSharpProjectPluginAssemblyCreateDescriptionCommand
                , ActionOnComponent.Description
            );



            InstanceFolderEntityDescription = new CSharpProjectPluginAssemblyActionOnComponentCommand(
                commandService
                , PackageIds.guidCommandSet.FolderCSharpProjectPluginAssemblyCreateEntityDescriptionCommandId
                , sourceFolder
                , Properties.CommandNames.FolderCSharpProjectPluginAssemblyCreateEntityDescriptionCommand
                , ActionOnComponent.EntityDescription
            );

            InstanceFolderDescription = new CSharpProjectPluginAssemblyActionOnComponentCommand(
                commandService
                , PackageIds.guidCommandSet.FolderCSharpProjectPluginAssemblyCreateDescriptionCommandId
                , sourceFolder
                , Properties.CommandNames.FolderCSharpProjectPluginAssemblyCreateDescriptionCommand
                , ActionOnComponent.Description
            );



            InstanceProjectEntityDescription = new CSharpProjectPluginAssemblyActionOnComponentCommand(
                commandService
                , PackageIds.guidCommandSet.ProjectPluginAssemblyCreateEntityDescriptionCommandId
                , sourceProject
                , Properties.CommandNames.ProjectPluginAssemblyCreateEntityDescriptionCommand
                , ActionOnComponent.EntityDescription
            );

            InstanceProjectDescription = new CSharpProjectPluginAssemblyActionOnComponentCommand(
                commandService
                , PackageIds.guidCommandSet.ProjectPluginAssemblyCreateDescriptionCommandId
                , sourceProject
                , Properties.CommandNames.ProjectPluginAssemblyCreateDescriptionCommand
                , ActionOnComponent.Description
            );
        }

        protected override void CommandAction(DTEHelper helper)
        {
            var selectedProjects = _sourceSelectedProjects.GetSelectedProjects(helper).ToList();

            helper.HandleActionOnProjectPluginAssemblyCommand(null, selectedProjects, this._actionOnComponent);
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            _sourceSelectedProjects.CommandBeforeQueryStatus(applicationObject, menuCommand);

            CommonHandlers.CorrectCommandNameForConnectionName(applicationObject, menuCommand, this._commandNameForCorrection);
        }
    }
}

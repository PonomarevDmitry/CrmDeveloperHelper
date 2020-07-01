using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Entities;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model.Sources;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.CSharp
{
    internal sealed class CSharpProjectPluginTypeActionOnComponentCommand : AbstractSingleCommand
    {
        private readonly ISourceSelectedFiles _sourceSelectedFiles;
        private readonly string _commandNameForCorrection;
        private readonly ActionOnComponent _actionOnComponent;
        private readonly string _fieldName = string.Empty;
        private readonly string _fieldTitle = string.Empty;

        private CSharpProjectPluginTypeActionOnComponentCommand(OleMenuCommandService commandService, int idCommand, ISourceSelectedFiles sourceSelectedFiles, string commandNameForCorrection, ActionOnComponent actionOnComponent)
            : base(commandService, idCommand)
        {
            this._sourceSelectedFiles = sourceSelectedFiles;
            this._commandNameForCorrection = commandNameForCorrection;
            this._actionOnComponent = actionOnComponent;
        }

        private CSharpProjectPluginTypeActionOnComponentCommand(OleMenuCommandService commandService, int idCommand, ISourceSelectedFiles sourceSelectedFiles, string commandNameForCorrection, ActionOnComponent actionOnComponent, string fieldName, string fieldTitle)
            : this(commandService, idCommand, sourceSelectedFiles, commandNameForCorrection, actionOnComponent)
        {
            this._fieldName = fieldName;
            this._fieldTitle = fieldTitle;
        }

        public static CSharpProjectPluginTypeActionOnComponentCommand InstanceCodeEntityDescription { get; private set; }

        public static CSharpProjectPluginTypeActionOnComponentCommand InstanceCodeGetCustomWorkflowActivityInfo { get; private set; }

        public static CSharpProjectPluginTypeActionOnComponentCommand InstanceCodeDescription { get; private set; }



        public static CSharpProjectPluginTypeActionOnComponentCommand InstanceDocumentsEntityDescription { get; private set; }

        public static CSharpProjectPluginTypeActionOnComponentCommand InstanceDocumentsGetCustomWorkflowActivityInfo { get; private set; }

        public static CSharpProjectPluginTypeActionOnComponentCommand InstanceDocumentsDescription { get; private set; }



        public static CSharpProjectPluginTypeActionOnComponentCommand InstanceFileEntityDescription { get; private set; }

        public static CSharpProjectPluginTypeActionOnComponentCommand InstanceFileGetCustomWorkflowActivityInfo { get; private set; }

        public static CSharpProjectPluginTypeActionOnComponentCommand InstanceFileDescription { get; private set; }



        public static CSharpProjectPluginTypeActionOnComponentCommand InstanceEntityDescription { get; private set; }

        public static CSharpProjectPluginTypeActionOnComponentCommand InstanceGetCustomWorkflowActivityInfo { get; private set; }

        public static CSharpProjectPluginTypeActionOnComponentCommand InstanceDescription { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            var sourceCode = CodeSourceSelectedFiles.CreateSource();

            var sourceDocuments = DocumentsSourceSelectedFiles.CreateSource();

            var sourceFile = FileSourceSelectedFiles.CreateSource();

            var sourceFolder = FolderSourceSelectedFiles.CreateSource();

            InstanceCodeEntityDescription = new CSharpProjectPluginTypeActionOnComponentCommand(
                commandService
                , PackageIds.guidCommandSet.CodeCSharpProjectPluginTypeCreateEntityDescriptionCommandId
                , sourceCode
                , Properties.CommandNames.CodeCSharpProjectPluginTypeCreateEntityDescriptionCommand
                , ActionOnComponent.EntityDescription
            );

            InstanceCodeGetCustomWorkflowActivityInfo = new CSharpProjectPluginTypeActionOnComponentCommand(
                commandService
                , PackageIds.guidCommandSet.CodeCSharpProjectPluginTypeGetCustomWorkflowActivityInfoCommandId
                , sourceCode
                , Properties.CommandNames.CodeCSharpProjectPluginTypeGetCustomWorkflowActivityInfoCommand
                , ActionOnComponent.SingleXmlField
                , PluginType.Schema.Attributes.customworkflowactivityinfo
                , PluginType.Schema.Headers.customworkflowactivityinfo
            );

            InstanceCodeDescription = new CSharpProjectPluginTypeActionOnComponentCommand(
                commandService
                , PackageIds.guidCommandSet.CodeCSharpProjectPluginTypeCreateDescriptionCommandId
                , sourceCode
                , Properties.CommandNames.CodeCSharpProjectPluginTypeCreateDescriptionCommand
                , ActionOnComponent.Description
            );



            InstanceDocumentsEntityDescription = new CSharpProjectPluginTypeActionOnComponentCommand(
                commandService
                , PackageIds.guidCommandSet.DocumentsCSharpProjectPluginTypeCreateEntityDescriptionCommandId
                , sourceDocuments
                , Properties.CommandNames.DocumentsCSharpProjectPluginTypeCreateEntityDescriptionCommand
                , ActionOnComponent.EntityDescription
            );

            InstanceDocumentsGetCustomWorkflowActivityInfo = new CSharpProjectPluginTypeActionOnComponentCommand(
                commandService
                , PackageIds.guidCommandSet.DocumentsCSharpProjectPluginTypeGetCustomWorkflowActivityInfoCommandId
                , sourceDocuments
                , Properties.CommandNames.DocumentsCSharpProjectPluginTypeGetCustomWorkflowActivityInfoCommand
                , ActionOnComponent.SingleXmlField
                , PluginType.Schema.Attributes.customworkflowactivityinfo
                , PluginType.Schema.Headers.customworkflowactivityinfo
            );

            InstanceDocumentsDescription = new CSharpProjectPluginTypeActionOnComponentCommand(
                commandService
                , PackageIds.guidCommandSet.DocumentsCSharpProjectPluginTypeCreateDescriptionCommandId
                , sourceDocuments
                , Properties.CommandNames.DocumentsCSharpProjectPluginTypeCreateDescriptionCommand
                , ActionOnComponent.Description
            );



            InstanceFileEntityDescription = new CSharpProjectPluginTypeActionOnComponentCommand(
                commandService
                , PackageIds.guidCommandSet.FileCSharpProjectPluginTypeCreateEntityDescriptionCommandId
                , sourceFile
                , Properties.CommandNames.FileCSharpProjectPluginTypeCreateEntityDescriptionCommand
                , ActionOnComponent.EntityDescription
            );

            InstanceFileGetCustomWorkflowActivityInfo = new CSharpProjectPluginTypeActionOnComponentCommand(
                commandService
                , PackageIds.guidCommandSet.FileCSharpProjectPluginTypeGetCustomWorkflowActivityInfoCommandId
                , sourceFile
                , Properties.CommandNames.FileCSharpProjectPluginTypeGetCustomWorkflowActivityInfoCommand
                , ActionOnComponent.SingleXmlField
                , PluginType.Schema.Attributes.customworkflowactivityinfo
                , PluginType.Schema.Headers.customworkflowactivityinfo
            );

            InstanceFileDescription = new CSharpProjectPluginTypeActionOnComponentCommand(
                commandService
                , PackageIds.guidCommandSet.FileCSharpProjectPluginTypeCreateDescriptionCommandId
                , sourceFile
                , Properties.CommandNames.FileCSharpProjectPluginTypeCreateDescriptionCommand
                , ActionOnComponent.Description
            );



            InstanceEntityDescription = new CSharpProjectPluginTypeActionOnComponentCommand(
                commandService
                , PackageIds.guidCommandSet.FolderCSharpProjectPluginTypeCreateEntityDescriptionCommandId
                , sourceFolder
                , Properties.CommandNames.FolderCSharpProjectPluginTypeCreateEntityDescriptionCommand
                , ActionOnComponent.EntityDescription
            );

            InstanceGetCustomWorkflowActivityInfo = new CSharpProjectPluginTypeActionOnComponentCommand(
                commandService
                , PackageIds.guidCommandSet.FolderCSharpProjectPluginTypeGetCustomWorkflowActivityInfoCommandId
                , sourceFolder
                , Properties.CommandNames.FolderCSharpProjectPluginTypeGetCustomWorkflowActivityInfoCommand
                , ActionOnComponent.SingleXmlField
                , PluginType.Schema.Attributes.customworkflowactivityinfo
                , PluginType.Schema.Headers.customworkflowactivityinfo
            );

            InstanceDescription = new CSharpProjectPluginTypeActionOnComponentCommand(
                commandService
                , PackageIds.guidCommandSet.FolderCSharpProjectPluginTypeCreateDescriptionCommandId
                , sourceFolder
                , Properties.CommandNames.FolderCSharpProjectPluginTypeCreateDescriptionCommand
                , ActionOnComponent.Description
            );
        }

        protected override void CommandAction(DTEHelper helper)
        {
            var selectedFiles = _sourceSelectedFiles.GetSelectedFiles(helper, SelectedFileType.CSharp).ToList();

            helper.HandleActionOnPluginTypesCommand(null, selectedFiles, this._actionOnComponent, this._fieldName, this._fieldTitle);
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            _sourceSelectedFiles.CommandBeforeQueryStatus(applicationObject, menuCommand, SelectedFileType.CSharp);

            CommonHandlers.CorrectCommandNameForConnectionName(applicationObject, menuCommand, this._commandNameForCorrection);
        }
    }
}
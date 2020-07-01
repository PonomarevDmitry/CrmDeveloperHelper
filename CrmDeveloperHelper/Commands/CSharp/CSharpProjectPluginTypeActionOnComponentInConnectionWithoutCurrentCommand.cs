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
    internal sealed class CSharpProjectPluginTypeActionOnComponentInConnectionWithoutCurrentCommand : AbstractDynamicCommandByConnectionAllWithoutCurrent
    {
        private readonly ISourceSelectedFiles _sourceSelectedFiles;
        private readonly ActionOnComponent _actionOnComponent;
        private readonly string _fieldName = string.Empty;
        private readonly string _fieldTitle = string.Empty;

        private CSharpProjectPluginTypeActionOnComponentInConnectionWithoutCurrentCommand(OleMenuCommandService commandService, int baseIdStart, ISourceSelectedFiles sourceSelectedFiles, ActionOnComponent actionOnComponent)
            : base(commandService, baseIdStart)
        {
            this._sourceSelectedFiles = sourceSelectedFiles;
            this._actionOnComponent = actionOnComponent;
        }

        private CSharpProjectPluginTypeActionOnComponentInConnectionWithoutCurrentCommand(OleMenuCommandService commandService, int baseIdStart, ISourceSelectedFiles sourceSelectedFiles, ActionOnComponent actionOnComponent, string fieldName, string fieldTitle)
            : this(commandService, baseIdStart, sourceSelectedFiles, actionOnComponent)
        {
            this._fieldName = fieldName;
            this._fieldTitle = fieldTitle;
        }

        public static CSharpProjectPluginTypeActionOnComponentInConnectionWithoutCurrentCommand InstanceCodeEntityDescription { get; private set; }

        public static CSharpProjectPluginTypeActionOnComponentInConnectionWithoutCurrentCommand InstanceCodeGetCustomWorkflowActivityInfo { get; private set; }

        public static CSharpProjectPluginTypeActionOnComponentInConnectionWithoutCurrentCommand InstanceCodeDescription { get; private set; }



        public static CSharpProjectPluginTypeActionOnComponentInConnectionWithoutCurrentCommand InstanceDocumentsEntityDescription { get; private set; }

        public static CSharpProjectPluginTypeActionOnComponentInConnectionWithoutCurrentCommand InstanceDocumentsGetCustomWorkflowActivityInfo { get; private set; }

        public static CSharpProjectPluginTypeActionOnComponentInConnectionWithoutCurrentCommand InstanceDocumentsDescription { get; private set; }



        public static CSharpProjectPluginTypeActionOnComponentInConnectionWithoutCurrentCommand InstanceFileEntityDescription { get; private set; }

        public static CSharpProjectPluginTypeActionOnComponentInConnectionWithoutCurrentCommand InstanceFileGetCustomWorkflowActivityInfo { get; private set; }

        public static CSharpProjectPluginTypeActionOnComponentInConnectionWithoutCurrentCommand InstanceFileDescription { get; private set; }



        public static CSharpProjectPluginTypeActionOnComponentInConnectionWithoutCurrentCommand InstanceFolderEntityDescription { get; private set; }

        public static CSharpProjectPluginTypeActionOnComponentInConnectionWithoutCurrentCommand InstanceFolderGetCustomWorkflowActivityInfo { get; private set; }

        public static CSharpProjectPluginTypeActionOnComponentInConnectionWithoutCurrentCommand InstanceFolderDescription { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            var sourceCode = CodeSourceSelectedFiles.CreateSource();

            var sourceDocuments = DocumentsSourceSelectedFiles.CreateSource();

            var sourceFile = FileSourceSelectedFiles.CreateSource();

            var sourceFolder = FolderSourceSelectedFiles.CreateSource();

            InstanceCodeEntityDescription = new CSharpProjectPluginTypeActionOnComponentInConnectionWithoutCurrentCommand(
                commandService
                , PackageIds.guidDynamicCommandSet.CodeCSharpProjectPluginTypeCreateEntityDescriptionInConnectionCommandId
                , sourceCode
                , ActionOnComponent.EntityDescription
            );

            InstanceCodeGetCustomWorkflowActivityInfo = new CSharpProjectPluginTypeActionOnComponentInConnectionWithoutCurrentCommand(
                commandService
                , PackageIds.guidDynamicCommandSet.CodeCSharpProjectPluginTypeGetCustomWorkflowActivityInfoInConnectionCommandId
                , sourceCode
                , ActionOnComponent.SingleXmlField
                , PluginType.Schema.Attributes.customworkflowactivityinfo
                , PluginType.Schema.Headers.customworkflowactivityinfo
            );

            InstanceCodeDescription = new CSharpProjectPluginTypeActionOnComponentInConnectionWithoutCurrentCommand(
                commandService
                , PackageIds.guidDynamicCommandSet.CodeCSharpProjectPluginTypeCreateDescriptionInConnectionCommandId
                , sourceCode
                , ActionOnComponent.Description
            );



            InstanceDocumentsEntityDescription = new CSharpProjectPluginTypeActionOnComponentInConnectionWithoutCurrentCommand(
                commandService
                , PackageIds.guidDynamicCommandSet.DocumentsCSharpProjectPluginTypeCreateEntityDescriptionInConnectionCommandId
                , sourceDocuments
                , ActionOnComponent.EntityDescription
            );

            InstanceDocumentsGetCustomWorkflowActivityInfo = new CSharpProjectPluginTypeActionOnComponentInConnectionWithoutCurrentCommand(
                commandService
                , PackageIds.guidDynamicCommandSet.DocumentsCSharpProjectPluginTypeGetCustomWorkflowActivityInfoInConnectionCommandId
                , sourceDocuments
                , ActionOnComponent.SingleXmlField
                , PluginType.Schema.Attributes.customworkflowactivityinfo
                , PluginType.Schema.Headers.customworkflowactivityinfo
            );

            InstanceDocumentsDescription = new CSharpProjectPluginTypeActionOnComponentInConnectionWithoutCurrentCommand(
                commandService
                , PackageIds.guidDynamicCommandSet.DocumentsCSharpProjectPluginTypeCreateDescriptionInConnectionCommandId
                , sourceDocuments
                , ActionOnComponent.Description
            );



            InstanceFileEntityDescription = new CSharpProjectPluginTypeActionOnComponentInConnectionWithoutCurrentCommand(
                commandService
                , PackageIds.guidDynamicCommandSet.FileCSharpProjectPluginTypeCreateEntityDescriptionInConnectionCommandId
                , sourceFile
                , ActionOnComponent.EntityDescription
            );

            InstanceFileGetCustomWorkflowActivityInfo = new CSharpProjectPluginTypeActionOnComponentInConnectionWithoutCurrentCommand(
                commandService
                , PackageIds.guidDynamicCommandSet.FileCSharpProjectPluginTypeGetCustomWorkflowActivityInfoInConnectionCommandId
                , sourceFile
                , ActionOnComponent.SingleXmlField
                , PluginType.Schema.Attributes.customworkflowactivityinfo
                , PluginType.Schema.Headers.customworkflowactivityinfo
            );

            InstanceFileDescription = new CSharpProjectPluginTypeActionOnComponentInConnectionWithoutCurrentCommand(
                commandService
                , PackageIds.guidDynamicCommandSet.FileCSharpProjectPluginTypeCreateDescriptionInConnectionCommandId
                , sourceFile
                , ActionOnComponent.Description
            );



            InstanceFolderEntityDescription = new CSharpProjectPluginTypeActionOnComponentInConnectionWithoutCurrentCommand(
                commandService
                , PackageIds.guidDynamicCommandSet.FolderCSharpProjectPluginTypeCreateEntityDescriptionInConnectionCommandId
                , sourceFolder
                , ActionOnComponent.EntityDescription
            );

            InstanceFolderGetCustomWorkflowActivityInfo = new CSharpProjectPluginTypeActionOnComponentInConnectionWithoutCurrentCommand(
                commandService
                , PackageIds.guidDynamicCommandSet.FolderCSharpProjectPluginTypeGetCustomWorkflowActivityInfoInConnectionCommandId
                , sourceFolder
                , ActionOnComponent.SingleXmlField
                , PluginType.Schema.Attributes.customworkflowactivityinfo
                , PluginType.Schema.Headers.customworkflowactivityinfo
            );

            InstanceFolderDescription = new CSharpProjectPluginTypeActionOnComponentInConnectionWithoutCurrentCommand(
                commandService
                , PackageIds.guidDynamicCommandSet.FolderCSharpProjectPluginTypeCreateDescriptionInConnectionCommandId
                , sourceFolder
                , ActionOnComponent.Description
            );
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            var selectedFiles = _sourceSelectedFiles.GetSelectedFiles(helper, SelectedFileType.CSharp).ToList();

            helper.HandleActionOnPluginTypesCommand(connectionData, selectedFiles, this._actionOnComponent, this._fieldName, this._fieldTitle);
        }

        protected override void CommandBeforeQueryStatus(DTE2 applicationObject, ConnectionData connectionData, OleMenuCommand menuCommand)
        {
            _sourceSelectedFiles.CommandBeforeQueryStatus(applicationObject, menuCommand, SelectedFileType.CSharp);
        }
    }
}
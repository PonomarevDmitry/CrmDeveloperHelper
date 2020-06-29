using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model.SourcesSelectedFiles;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.Xmls
{
    internal sealed class XmlCommonXsdSchemaOpenFolderCommand : AbstractSingleCommand
    {
        private readonly ISourceSelectedFiles _sourceSelectedFiles;

        private XmlCommonXsdSchemaOpenFolderCommand(OleMenuCommandService commandService, int idCommand, ISourceSelectedFiles sourceSelectedFiles)
            : base(commandService, idCommand)
        {
            this._sourceSelectedFiles = sourceSelectedFiles;
        }

        public static XmlCommonXsdSchemaOpenFolderCommand InstanceCode { get; private set; }

        public static XmlCommonXsdSchemaOpenFolderCommand InstanceDocuments { get; private set; }

        public static XmlCommonXsdSchemaOpenFolderCommand InstanceFile { get; private set; }

        public static XmlCommonXsdSchemaOpenFolderCommand InstanceFolder { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            var sourceCode = CodeSourceSelectedFiles.CreateSource();

            var sourceDocuments = DocumentsSourceSelectedFiles.CreateSource();

            var sourceFile = FileSourceSelectedFiles.CreateSource();

            var sourceFolder = FolderSourceSelectedFiles.CreateSource();

            InstanceCode = new XmlCommonXsdSchemaOpenFolderCommand(commandService, PackageIds.guidCommandSet.CodeXmlCommonXsdSchemaOpenFolderCommandId, sourceCode);

            InstanceDocuments = new XmlCommonXsdSchemaOpenFolderCommand(commandService, PackageIds.guidCommandSet.DocumentsXmlCommonXsdSchemaOpenFolderCommandId, sourceDocuments);

            InstanceFile = new XmlCommonXsdSchemaOpenFolderCommand(commandService, PackageIds.guidCommandSet.FileXmlCommonXsdSchemaOpenFolderCommandId, sourceFile);

            InstanceFolder = new XmlCommonXsdSchemaOpenFolderCommand(commandService, PackageIds.guidCommandSet.FolderXmlCommonXsdSchemaOpenFolderCommandId, sourceFolder);
        }

        protected override void CommandAction(DTEHelper helper)
        {
            helper.HandleXsdSchemaOpenFolder();
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            _sourceSelectedFiles.CommandBeforeQueryStatus(applicationObject, menuCommand, SelectedFileType.Xml);
        }
    }
}
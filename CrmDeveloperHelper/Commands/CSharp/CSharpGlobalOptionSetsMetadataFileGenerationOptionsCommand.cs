using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model.SourcesSelectedFiles;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.CSharp
{
    internal sealed class CSharpGlobalOptionSetsMetadataFileGenerationOptionsCommand : AbstractSingleCommand
    {
        private readonly ISourceSelectedFiles _sourceSelectedFiles;

        private CSharpGlobalOptionSetsMetadataFileGenerationOptionsCommand(OleMenuCommandService commandService, int idCommand, ISourceSelectedFiles sourceSelectedFiles)
            : base(commandService, idCommand)
        {
            this._sourceSelectedFiles = sourceSelectedFiles;
        }

        public static CSharpGlobalOptionSetsMetadataFileGenerationOptionsCommand InstanceCode { get; private set; }

        public static CSharpGlobalOptionSetsMetadataFileGenerationOptionsCommand InstanceDocuments { get; private set; }

        public static CSharpGlobalOptionSetsMetadataFileGenerationOptionsCommand InstanceFile { get; private set; }

        public static CSharpGlobalOptionSetsMetadataFileGenerationOptionsCommand InstanceFolder { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            var sourceCode = CodeSourceSelectedFiles.CreateSource();

            var sourceDocuments = DocumentsSourceSelectedFiles.CreateSource();

            var sourceFile = FileSourceSelectedFiles.CreateSource();

            var sourceFolder = FolderSourceSelectedFiles.CreateSource();

            InstanceCode = new CSharpGlobalOptionSetsMetadataFileGenerationOptionsCommand(commandService, PackageIds.guidCommandSet.CodeCSharpGlobalOptionSetsMetadataFileGenerationOptionsCommandId, sourceCode);

            InstanceDocuments = new CSharpGlobalOptionSetsMetadataFileGenerationOptionsCommand(commandService, PackageIds.guidCommandSet.DocumentsCSharpGlobalOptionSetsMetadataFileGenerationOptionsCommandId, sourceDocuments);

            InstanceFile = new CSharpGlobalOptionSetsMetadataFileGenerationOptionsCommand(commandService, PackageIds.guidCommandSet.FileCSharpGlobalOptionSetsMetadataFileGenerationOptionsCommandId, sourceFile);

            InstanceFolder = new CSharpGlobalOptionSetsMetadataFileGenerationOptionsCommand(commandService, PackageIds.guidCommandSet.FolderCSharpGlobalOptionSetsMetadataFileGenerationOptionsCommandId, sourceFolder);
        }

        protected override void CommandAction(DTEHelper helper)
        {
            helper.HandleGlobalOptionSetsMetadataFileGenerationOptions();
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            _sourceSelectedFiles.CommandBeforeQueryStatus(applicationObject, menuCommand, SelectedFileType.CSharp);

            CommonHandlers.CorrectCommandNameForConnectionName(applicationObject, menuCommand, Properties.CommandNames.CSharpGlobalOptionSetsMetadataFileGenerationOptionsCommand);
        }
    }
}
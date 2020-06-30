using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model.SourcesSelectedFiles;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.CSharp
{
    internal sealed class CSharpEntityMetadataFileGenerationOptionsCommand : AbstractSingleCommand
    {
        private readonly ISourceSelectedFiles _sourceSelectedFiles;

        private CSharpEntityMetadataFileGenerationOptionsCommand(OleMenuCommandService commandService, int idCommand, ISourceSelectedFiles sourceSelectedFiles)
            : base(commandService, idCommand)
        {
            this._sourceSelectedFiles = sourceSelectedFiles;
        }

        public static CSharpEntityMetadataFileGenerationOptionsCommand InstanceCode { get; private set; }

        public static CSharpEntityMetadataFileGenerationOptionsCommand InstanceDocuments { get; private set; }

        public static CSharpEntityMetadataFileGenerationOptionsCommand InstanceFile { get; private set; }

        public static CSharpEntityMetadataFileGenerationOptionsCommand InstanceFolder { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            var sourceCode = CodeSourceSelectedFiles.CreateSource();

            var sourceDocuments = DocumentsSourceSelectedFiles.CreateSource();

            var sourceFile = FileSourceSelectedFiles.CreateSource();

            var sourceFolder = FolderSourceSelectedFiles.CreateSource();

            InstanceCode = new CSharpEntityMetadataFileGenerationOptionsCommand(commandService, PackageIds.guidCommandSet.CodeCSharpEntityMetadataFileGenerationOptionsCommandId, sourceCode);

            InstanceDocuments = new CSharpEntityMetadataFileGenerationOptionsCommand(commandService, PackageIds.guidCommandSet.DocumentsCSharpEntityMetadataFileGenerationOptionsCommandId, sourceDocuments);

            InstanceFile = new CSharpEntityMetadataFileGenerationOptionsCommand(commandService, PackageIds.guidCommandSet.FileCSharpEntityMetadataFileGenerationOptionsCommandId, sourceFile);

            InstanceFolder = new CSharpEntityMetadataFileGenerationOptionsCommand(commandService, PackageIds.guidCommandSet.FolderCSharpEntityMetadataFileGenerationOptionsCommandId, sourceFolder);
        }

        protected override void CommandAction(DTEHelper helper)
        {
            helper.HandleEntityMetadataFileGenerationOptions();
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            _sourceSelectedFiles.CommandBeforeQueryStatus(applicationObject, menuCommand, SelectedFileType.CSharp);

            CommonHandlers.CorrectCommandNameForConnectionName(applicationObject, menuCommand, Properties.CommandNames.CSharpEntityMetadataFileGenerationOptionsCommand);
        }
    }
}

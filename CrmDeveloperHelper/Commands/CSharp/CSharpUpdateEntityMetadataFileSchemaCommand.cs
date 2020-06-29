using EnvDTE80;
using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model.SourcesSelectedFiles;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.CSharp
{
    internal sealed class CSharpUpdateEntityMetadataFileSchemaCommand : AbstractDynamicCommandByConnectionAll
    {
        private readonly ISourceSelectedFiles _sourceSelectedFiles;

        private CSharpUpdateEntityMetadataFileSchemaCommand(OleMenuCommandService commandService, int baseIdStart, ISourceSelectedFiles sourceSelectedFiles)
            : base(commandService, baseIdStart)
        {
            this._sourceSelectedFiles = sourceSelectedFiles;
        }

        public static CSharpUpdateEntityMetadataFileSchemaCommand InstanceCode { get; private set; }

        public static CSharpUpdateEntityMetadataFileSchemaCommand InstanceDocuments { get; private set; }

        public static CSharpUpdateEntityMetadataFileSchemaCommand InstanceFile { get; private set; }

        public static CSharpUpdateEntityMetadataFileSchemaCommand InstanceFolder { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            var sourceCode = CodeSourceSelectedFiles.CreateSource();

            var sourceDocuments = DocumentsSourceSelectedFiles.CreateSource();

            var sourceFile = FileSourceSelectedFiles.CreateSource();

            var sourceFolder = FolderSourceSelectedFiles.CreateSource();

            InstanceCode = new CSharpUpdateEntityMetadataFileSchemaCommand(commandService, PackageIds.guidDynamicCommandSet.CodeCSharpUpdateEntityMetadataFileSchemaCommandId, sourceCode);

            InstanceDocuments = new CSharpUpdateEntityMetadataFileSchemaCommand(commandService, PackageIds.guidDynamicCommandSet.DocumentsCSharpUpdateEntityMetadataFileSchemaCommandId, sourceDocuments);

            InstanceFile = new CSharpUpdateEntityMetadataFileSchemaCommand(commandService, PackageIds.guidDynamicCommandSet.FileCSharpUpdateEntityMetadataFileSchemaCommandId, sourceFile);

            InstanceFolder = new CSharpUpdateEntityMetadataFileSchemaCommand(commandService, PackageIds.guidDynamicCommandSet.FolderCSharpUpdateEntityMetadataFileSchemaCommandId, sourceFolder);
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            var selectedFiles = _sourceSelectedFiles.GetSelectedFiles(helper, SelectedFileType.CSharp).ToList();

            helper.HandleCSharpEntityMetadataFileUpdateSchema(connectionData, selectedFiles, false);
        }

        protected override void CommandBeforeQueryStatus(DTE2 applicationObject, ConnectionData connectionData, OleMenuCommand menuCommand)
        {
            _sourceSelectedFiles.CommandBeforeQueryStatus(applicationObject, menuCommand, SelectedFileType.CSharp);
        }
    }
}
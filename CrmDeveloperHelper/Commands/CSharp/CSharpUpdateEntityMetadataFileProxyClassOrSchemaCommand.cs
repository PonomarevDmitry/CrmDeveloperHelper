using EnvDTE80;
using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model.Sources;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.CSharp
{
    internal sealed class CSharpUpdateEntityMetadataFileProxyClassOrSchemaCommand : AbstractDynamicCommandByConnectionAll
    {
        private readonly ISourceSelectedFiles _sourceSelectedFiles;

        private CSharpUpdateEntityMetadataFileProxyClassOrSchemaCommand(OleMenuCommandService commandService, int baseIdStart, ISourceSelectedFiles sourceSelectedFiles)
            : base(commandService, baseIdStart)
        {
            this._sourceSelectedFiles = sourceSelectedFiles;
        }

        public static CSharpUpdateEntityMetadataFileProxyClassOrSchemaCommand InstanceCode { get; private set; }

        public static CSharpUpdateEntityMetadataFileProxyClassOrSchemaCommand InstanceDocuments { get; private set; }

        public static CSharpUpdateEntityMetadataFileProxyClassOrSchemaCommand InstanceFile { get; private set; }

        public static CSharpUpdateEntityMetadataFileProxyClassOrSchemaCommand InstanceFolder { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            var sourceCode = CodeSourceSelectedFiles.CreateSource();

            var sourceDocuments = DocumentsSourceSelectedFiles.CreateSource();

            var sourceFile = FileSourceSelectedFiles.CreateSource();

            var sourceFolder = FolderSourceSelectedFiles.CreateSource();

            InstanceCode = new CSharpUpdateEntityMetadataFileProxyClassOrSchemaCommand(commandService, PackageIds.guidDynamicCommandSet.CodeCSharpUpdateEntityMetadataFileProxyClassOrSchemaCommandId, sourceCode);

            InstanceDocuments = new CSharpUpdateEntityMetadataFileProxyClassOrSchemaCommand(commandService, PackageIds.guidDynamicCommandSet.DocumentsCSharpUpdateEntityMetadataFileProxyClassOrSchemaCommandId, sourceDocuments);

            InstanceFile = new CSharpUpdateEntityMetadataFileProxyClassOrSchemaCommand(commandService, PackageIds.guidDynamicCommandSet.FileCSharpUpdateEntityMetadataFileProxyClassOrSchemaCommandId, sourceFile);

            InstanceFolder = new CSharpUpdateEntityMetadataFileProxyClassOrSchemaCommand(commandService, PackageIds.guidDynamicCommandSet.FolderCSharpUpdateEntityMetadataFileProxyClassOrSchemaCommandId, sourceFolder);
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            var selectedFiles = _sourceSelectedFiles.GetSelectedFiles(helper, SelectedFileType.CSharp).ToList();

            helper.HandleCSharpEntityMetadataFileUpdateProxyClassOrSchema(connectionData, selectedFiles, false);
        }

        protected override void CommandBeforeQueryStatus(DTE2 applicationObject, ConnectionData connectionData, OleMenuCommand menuCommand)
        {
            _sourceSelectedFiles.CommandBeforeQueryStatus(applicationObject, menuCommand, SelectedFileType.CSharp);
        }
    }
}

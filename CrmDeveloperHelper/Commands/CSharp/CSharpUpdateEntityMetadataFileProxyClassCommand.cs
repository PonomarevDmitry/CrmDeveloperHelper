using EnvDTE80;
using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model.Sources;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.CSharp
{
    internal sealed class CSharpUpdateEntityMetadataFileProxyClassCommand : AbstractDynamicCommandByConnectionAll
    {
        private readonly ISourceSelectedFiles _sourceSelectedFiles;

        private CSharpUpdateEntityMetadataFileProxyClassCommand(OleMenuCommandService commandService, int baseIdStart, ISourceSelectedFiles sourceSelectedFiles)
            : base(commandService, baseIdStart)
        {
            this._sourceSelectedFiles = sourceSelectedFiles;
        }

        public static CSharpUpdateEntityMetadataFileProxyClassCommand InstanceCode { get; private set; }

        public static CSharpUpdateEntityMetadataFileProxyClassCommand InstanceDocuments { get; private set; }

        public static CSharpUpdateEntityMetadataFileProxyClassCommand InstanceFile { get; private set; }

        public static CSharpUpdateEntityMetadataFileProxyClassCommand InstanceFolder { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            var sourceCode = CodeSourceSelectedFiles.CreateSource();

            var sourceDocuments = DocumentsSourceSelectedFiles.CreateSource();

            var sourceFile = FileSourceSelectedFiles.CreateSource();

            var sourceFolder = FolderSourceSelectedFiles.CreateSource();

            InstanceCode = new CSharpUpdateEntityMetadataFileProxyClassCommand(commandService, PackageIds.guidDynamicCommandSet.CodeCSharpUpdateEntityMetadataFileProxyClassCommandId, sourceCode);

            InstanceDocuments = new CSharpUpdateEntityMetadataFileProxyClassCommand(commandService, PackageIds.guidDynamicCommandSet.DocumentsCSharpUpdateEntityMetadataFileProxyClassCommandId, sourceDocuments);

            InstanceFile = new CSharpUpdateEntityMetadataFileProxyClassCommand(commandService, PackageIds.guidDynamicCommandSet.FileCSharpUpdateEntityMetadataFileProxyClassCommandId, sourceFile);

            InstanceFolder = new CSharpUpdateEntityMetadataFileProxyClassCommand(commandService, PackageIds.guidDynamicCommandSet.FolderCSharpUpdateEntityMetadataFileProxyClassCommandId, sourceFolder);
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            var selectedFiles = _sourceSelectedFiles.GetSelectedFiles(helper, SelectedFileType.CSharp).ToList();

            helper.HandleCSharpEntityMetadataFileUpdateProxyClass(connectionData, selectedFiles, false);
        }

        protected override void CommandBeforeQueryStatus(DTE2 applicationObject, ConnectionData connectionData, OleMenuCommand menuCommand)
        {
            _sourceSelectedFiles.CommandBeforeQueryStatus(applicationObject, menuCommand, SelectedFileType.CSharp);
        }
    }
}
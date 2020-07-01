using EnvDTE80;
using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model.Sources;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.CSharp
{
    internal sealed class CSharpUpdateGlobalOptionSetsFileCommand : AbstractDynamicCommandByConnectionAll
    {
        private readonly ISourceSelectedFiles _sourceSelectedFiles;

        private CSharpUpdateGlobalOptionSetsFileCommand(OleMenuCommandService commandService, int baseIdStart, ISourceSelectedFiles sourceSelectedFiles)
            : base(commandService, baseIdStart)
        {
            this._sourceSelectedFiles = sourceSelectedFiles;
        }

        public static CSharpUpdateGlobalOptionSetsFileCommand InstanceCode { get; private set; }

        public static CSharpUpdateGlobalOptionSetsFileCommand InstanceDocuments { get; private set; }

        public static CSharpUpdateGlobalOptionSetsFileCommand InstanceFile { get; private set; }

        public static CSharpUpdateGlobalOptionSetsFileCommand InstanceFolder { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            var sourceCode = CodeSourceSelectedFiles.CreateSource();

            var sourceDocuments = DocumentsSourceSelectedFiles.CreateSource();

            var sourceFile = FileSourceSelectedFiles.CreateSource();

            var sourceFolder = FolderSourceSelectedFiles.CreateSource();

            InstanceCode = new CSharpUpdateGlobalOptionSetsFileCommand(commandService, PackageIds.guidDynamicCommandSet.CodeCSharpUpdateGlobalOptionSetsFileCommandId, sourceCode);

            InstanceDocuments = new CSharpUpdateGlobalOptionSetsFileCommand(commandService, PackageIds.guidDynamicCommandSet.DocumentsCSharpUpdateGlobalOptionSetsFileCommandId, sourceDocuments);

            InstanceFile = new CSharpUpdateGlobalOptionSetsFileCommand(commandService, PackageIds.guidDynamicCommandSet.FileCSharpUpdateGlobalOptionSetsFileCommandId, sourceFile);

            InstanceFolder = new CSharpUpdateGlobalOptionSetsFileCommand(commandService, PackageIds.guidDynamicCommandSet.FolderCSharpUpdateGlobalOptionSetsFileCommandId, sourceFolder);
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            var selectedFiles = _sourceSelectedFiles.GetSelectedFiles(helper, SelectedFileType.CSharp).ToList();

            helper.HandleCSharpGlobalOptionSetsFileUpdateSchema(connectionData, selectedFiles, false);
        }

        protected override void CommandBeforeQueryStatus(DTE2 applicationObject, ConnectionData connectionData, OleMenuCommand menuCommand)
        {
            _sourceSelectedFiles.CommandBeforeQueryStatus(applicationObject, menuCommand, SelectedFileType.CSharp);
        }
    }
}

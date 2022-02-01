using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model.Sources;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.CSharp
{
    internal sealed class CSharpCheckEncodingCommand : AbstractSingleCommand
    {
        private readonly ISourceSelectedFiles _sourceSelectedFiles;

        private CSharpCheckEncodingCommand(OleMenuCommandService commandService, int idCommand, ISourceSelectedFiles sourceSelectedFiles)
            : base(commandService, idCommand)
        {
            this._sourceSelectedFiles = sourceSelectedFiles;
        }

        public static CSharpCheckEncodingCommand InstanceCode { get; private set; }

        public static CSharpCheckEncodingCommand InstanceDocuments { get; private set; }

        public static CSharpCheckEncodingCommand InstanceFile { get; private set; }

        public static CSharpCheckEncodingCommand InstanceFolder { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            var sourceCode = CodeSourceSelectedFiles.CreateSource();

            var sourceDocuments = DocumentsSourceSelectedFiles.CreateSource();

            var sourceFile = FileSourceSelectedFiles.CreateSource();

            var sourceFolder = FolderSourceSelectedFiles.CreateSource();

            InstanceCode = new CSharpCheckEncodingCommand(commandService, PackageIds.guidCommandSet.CodeCSharpCheckEncodingCommandId, sourceCode);

            InstanceDocuments = new CSharpCheckEncodingCommand(commandService, PackageIds.guidCommandSet.DocumentsCSharpCheckEncodingCommandId, sourceDocuments);

            InstanceFile = new CSharpCheckEncodingCommand(commandService, PackageIds.guidCommandSet.FileCSharpCheckEncodingCommandId, sourceFile);

            InstanceFolder = new CSharpCheckEncodingCommand(commandService, PackageIds.guidCommandSet.FolderCSharpCheckEncodingCommandId, sourceFolder);
        }

        protected override void CommandAction(DTEHelper helper)
        {
            var selectedFiles = _sourceSelectedFiles.GetSelectedFiles(helper, SelectedFileType.CSharp).ToList();

            helper.HandleWebResourceCheckFileEncodingCommand(selectedFiles);
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            _sourceSelectedFiles.CommandBeforeQueryStatus(applicationObject, menuCommand, SelectedFileType.CSharp);
        }
    }
}
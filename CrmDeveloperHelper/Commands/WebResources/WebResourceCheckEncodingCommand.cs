using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model.Sources;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.WebResources
{
    internal sealed class WebResourceCheckEncodingCommand : AbstractSingleCommand
    {
        private readonly ISourceSelectedFiles _sourceSelectedFiles;

        private WebResourceCheckEncodingCommand(OleMenuCommandService commandService, int idCommand, ISourceSelectedFiles sourceSelectedFiles)
            : base(commandService, idCommand)
        {
            this._sourceSelectedFiles = sourceSelectedFiles;
        }

        public static WebResourceCheckEncodingCommand InstanceCode { get; private set; }

        public static WebResourceCheckEncodingCommand InstanceDocuments { get; private set; }

        public static WebResourceCheckEncodingCommand InstanceFile { get; private set; }

        public static WebResourceCheckEncodingCommand InstanceFolder { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            var sourceCode = CodeSourceSelectedFiles.CreateSource();

            var sourceDocuments = DocumentsSourceSelectedFiles.CreateSource();

            var sourceFile = FileSourceSelectedFiles.CreateSource();

            var sourceFolder = FolderSourceSelectedFiles.CreateSource();

            InstanceCode = new WebResourceCheckEncodingCommand(commandService, PackageIds.guidCommandSet.CodeWebResourceCheckEncodingCommandId, sourceCode);

            InstanceDocuments = new WebResourceCheckEncodingCommand(commandService, PackageIds.guidCommandSet.DocumentsWebResourceCheckEncodingCommandId, sourceDocuments);

            InstanceFile = new WebResourceCheckEncodingCommand(commandService, PackageIds.guidCommandSet.FileWebResourceCheckEncodingCommandId, sourceFile);

            InstanceFolder = new WebResourceCheckEncodingCommand(commandService, PackageIds.guidCommandSet.FolderWebResourceCheckEncodingCommandId, sourceFolder);
        }

        protected override void CommandAction(DTEHelper helper)
        {
            var selectedFiles = _sourceSelectedFiles.GetSelectedFiles(helper, SelectedFileType.WebResourceText).ToList();

            helper.HandleWebResourceCheckFileEncodingCommand(selectedFiles);
        }

        protected override void CommandBeforeQueryStatus(EnvDTE80.DTE2 applicationObject, OleMenuCommand menuCommand)
        {
            _sourceSelectedFiles.CommandBeforeQueryStatus(applicationObject, menuCommand, SelectedFileType.WebResourceText);
        }
    }
}
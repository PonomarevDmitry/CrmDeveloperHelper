using EnvDTE80;
using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model.Sources;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.WebResources
{
    internal sealed class WebResourceAddToSolutionInConnectionCommand : AbstractDynamicCommandByConnectionAll
    {
        private readonly ISourceSelectedFiles _sourceSelectedFiles;

        private WebResourceAddToSolutionInConnectionCommand(OleMenuCommandService commandService, int baseIdStart, ISourceSelectedFiles sourceSelectedFiles)
            : base(commandService, baseIdStart)
        {
            this._sourceSelectedFiles = sourceSelectedFiles;
        }

        public static WebResourceAddToSolutionInConnectionCommand InstanceCode { get; private set; }

        public static WebResourceAddToSolutionInConnectionCommand InstanceDocuments { get; private set; }

        public static WebResourceAddToSolutionInConnectionCommand InstanceFile { get; private set; }

        public static WebResourceAddToSolutionInConnectionCommand InstanceFolder { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            var sourceCode = CodeSourceSelectedFiles.CreateSource();

            var sourceDocuments = DocumentsSourceSelectedFiles.CreateSource();

            var sourceFile = FileSourceSelectedFiles.CreateSource();

            var sourceFolder = FolderSourceSelectedFiles.CreateSource();

            InstanceCode = new WebResourceAddToSolutionInConnectionCommand(commandService, PackageIds.guidDynamicCommandSet.CodeWebResourceAddToSolutionInConnectionCommandId, sourceCode);

            InstanceDocuments = new WebResourceAddToSolutionInConnectionCommand(commandService, PackageIds.guidDynamicCommandSet.DocumentsWebResourceAddToSolutionInConnectionCommandId, sourceDocuments);

            InstanceFile = new WebResourceAddToSolutionInConnectionCommand(commandService, PackageIds.guidDynamicCommandSet.FileWebResourceAddToSolutionInConnectionCommandId, sourceFile);

            InstanceFolder = new WebResourceAddToSolutionInConnectionCommand(commandService, PackageIds.guidDynamicCommandSet.FolderWebResourceAddToSolutionInConnectionCommandId, sourceFolder);
        }

        protected override void CommandAction(DTEHelper helper, ConnectionData connectionData)
        {
            var selectedFiles = _sourceSelectedFiles.GetSelectedFiles(helper, SelectedFileType.WebResource).ToList();

            helper.HandleWebResourceAddingToSolutionCommand(connectionData, null, true, selectedFiles);
        }

        protected override void CommandBeforeQueryStatus(DTE2 applicationObject, ConnectionData connectionData, OleMenuCommand menuCommand)
        {
            _sourceSelectedFiles.CommandBeforeQueryStatus(applicationObject, menuCommand, SelectedFileType.WebResource);
        }
    }
}
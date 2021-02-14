using EnvDTE80;
using Microsoft.VisualStudio.Shell;
using Nav.Common.VSPackages.CrmDeveloperHelper.Helpers;
using Nav.Common.VSPackages.CrmDeveloperHelper.Interfaces;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model;
using Nav.Common.VSPackages.CrmDeveloperHelper.Model.Sources;
using System.Linq;

namespace Nav.Common.VSPackages.CrmDeveloperHelper.Commands.WebResources
{
    internal sealed class WebResourceAddToSolutionLastSelectedCommand : AbstractDynamicCommandOnSolutionLastSelected
    {
        private readonly ISourceSelectedFiles _sourceSelectedFiles;

        private WebResourceAddToSolutionLastSelectedCommand(OleMenuCommandService commandService, int baseIdStart, ISourceSelectedFiles sourceSelectedFiles)
            : base(commandService, baseIdStart)
        {
            this._sourceSelectedFiles = sourceSelectedFiles;
        }

        public static WebResourceAddToSolutionLastSelectedCommand InstanceCode { get; private set; }

        public static WebResourceAddToSolutionLastSelectedCommand InstanceDocuments { get; private set; }

        public static WebResourceAddToSolutionLastSelectedCommand InstanceFile { get; private set; }

        public static WebResourceAddToSolutionLastSelectedCommand InstanceFolder { get; private set; }

        public static void Initialize(OleMenuCommandService commandService)
        {
            var sourceCode = CodeSourceSelectedFiles.CreateSource();

            var sourceDocuments = DocumentsSourceSelectedFiles.CreateSource();

            var sourceFile = FileSourceSelectedFiles.CreateSource();

            var sourceFolder = FolderSourceSelectedFiles.CreateSource();

            InstanceCode = new WebResourceAddToSolutionLastSelectedCommand(commandService, PackageIds.guidDynamicSolutionLastSelectedCommandSet.CodeWebResourceAddToSolutionLastSelectedCommandId, sourceCode);

            InstanceDocuments = new WebResourceAddToSolutionLastSelectedCommand(commandService, PackageIds.guidDynamicSolutionLastSelectedCommandSet.DocumentsWebResourceAddToSolutionLastSelectedCommandId, sourceDocuments);

            InstanceFile = new WebResourceAddToSolutionLastSelectedCommand(commandService, PackageIds.guidDynamicSolutionLastSelectedCommandSet.FileWebResourceAddToSolutionLastSelectedCommandId, sourceFile);

            InstanceFolder = new WebResourceAddToSolutionLastSelectedCommand(commandService, PackageIds.guidDynamicSolutionLastSelectedCommandSet.FolderWebResourceAddToSolutionLastSelectedCommandId, sourceFolder);
        }

        protected override void CommandAction(DTEHelper helper, string solutionUniqueName)
        {
            var selectedFiles = _sourceSelectedFiles.GetSelectedFiles(helper, SelectedFileType.WebResource).ToList();

            helper.HandleWebResourceAddingToSolutionCommand(null, solutionUniqueName, false, selectedFiles);
        }

        protected override void CommandBeforeQueryStatus(DTE2 applicationObject, string element, OleMenuCommand menuCommand)
        {
            _sourceSelectedFiles.CommandBeforeQueryStatus(applicationObject, menuCommand, SelectedFileType.WebResource);
        }
    }
}